using DiplomaManagementSystem.Domain;
using DiplomaManagementSystem.Domain.Entities;
using DiplomaManagementSystem.Domain.Enums;
using DiplomaManagementSystem.Domain.Services;

using Npgsql;

string connectionString = args.FirstOrDefault()
    ?? Environment.GetEnvironmentVariable("DIPLOMA_REPAIR_PG")
    ?? throw new InvalidOperationException("Pass connection string as arg or set DIPLOMA_REPAIR_PG.");

await using NpgsqlConnection connection = new(connectionString);
await connection.OpenAsync();

List<Diploma> diplomas = await LoadDiplomasAsync(connection);
DiplomaLifecycleService lifecycleService = new(new AdmissionReadinessService());

int lifecycleUpdates = 0;
int stepUpdates = 0;

await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

foreach (Diploma diploma in diplomas)
{
    DiplomaTopicVersion? latestTopic = diploma.TopicVersions
        .OrderByDescending(version => version.VersionNumber)
        .FirstOrDefault();

    DiplomaLifecycleStatus expectedLifecycle = lifecycleService.Recalculate(
        diploma,
        latestTopic,
        diploma.AdmissionStepAttempts);

    if (expectedLifecycle >= DiplomaLifecycleStatus.TopicInReview
        && (diploma.SupervisorId is null
            || diploma.SupervisorAssignmentStatus != SupervisorAssignmentStatus.Confirmed))
    {
        expectedLifecycle = DiplomaLifecycleStatus.AwaitingSupervisor;
    }

    if (diploma.LifecycleStatus != expectedLifecycle)
    {
        await using NpgsqlCommand updateLifecycle = new(
            """
            UPDATE diplomas
            SET "LifecycleStatus" = @lifecycle,
                "UpdatedAt" = NOW()
            WHERE "Id" = @id
            """,
            connection,
            transaction);
        updateLifecycle.Parameters.AddWithValue("lifecycle", (int)expectedLifecycle);
        updateLifecycle.Parameters.AddWithValue("id", diploma.Id);
        await updateLifecycle.ExecuteNonQueryAsync();
        lifecycleUpdates++;
        diploma.LifecycleStatus = expectedLifecycle;
    }

    if (diploma.AdmissionStatus != DiplomaAdmissionStatus.Admitted
        && diploma.AdmissionStepAttempts.Count > 0)
    {
        AdmissionStep? expectedStep = AdmissionStepStatusResolver.ResolveCurrentStep(
            diploma,
            diploma.AdmissionStepAttempts);

        if (diploma.CurrentAdmissionStep != expectedStep)
        {
            await using NpgsqlCommand updateStep = new(
                """
                UPDATE diplomas
                SET "CurrentAdmissionStep" = @step,
                    "UpdatedAt" = NOW()
                WHERE "Id" = @id
                """,
                connection,
                transaction);
            updateStep.Parameters.AddWithValue("step", (object?)expectedStep ?? DBNull.Value);
            updateStep.Parameters.AddWithValue("id", diploma.Id);
            await updateStep.ExecuteNonQueryAsync();
            stepUpdates++;
        }
    }
}

// Clear invalid supervisor review metadata on rejected topics.
await using (NpgsqlCommand clearRejected = new(
    """
    UPDATE diploma_topic_versions
    SET "SupervisorReviewedById" = NULL,
        "SupervisorReviewedAt" = NULL
    WHERE "Status" = 3
      AND ("SupervisorReviewedById" IS NOT NULL OR "SupervisorReviewedAt" IS NOT NULL)
    """,
    connection,
    transaction))
{
    int clearedRejected = await clearRejected.ExecuteNonQueryAsync();
    Console.WriteLine($"Cleared supervisor review on rejected topics: {clearedRejected}");
}

// Pending supervisor topics must not carry head approval metadata.
await using (NpgsqlCommand clearPendingSupervisor = new(
    """
    UPDATE diploma_topic_versions
    SET "ReviewedById" = NULL,
        "ReviewedAt" = NULL
    WHERE "Status" = 0
      AND ("ReviewedById" IS NOT NULL OR "ReviewedAt" IS NOT NULL)
    """,
    connection,
    transaction))
{
    int clearedPending = await clearPendingSupervisor.ExecuteNonQueryAsync();
    Console.WriteLine($"Cleared head review on pending-supervisor topics: {clearedPending}");
}

// Invariant A4: lifecycle cannot reach topic review without a confirmed supervisor.
await using (NpgsqlCommand fixNoSupervisor = new(
    """
    UPDATE diplomas
    SET "LifecycleStatus" = 0,
        "UpdatedAt" = NOW()
    WHERE "AdmissionStatus" = 0
      AND ("SupervisorId" IS NULL OR "SupervisorAssignmentStatus" <> 1)
      AND "LifecycleStatus" >= 2
    """,
    connection,
    transaction))
{
    int fixedNoSupervisor = await fixNoSupervisor.ExecuteNonQueryAsync();
    Console.WriteLine($"Reset lifecycle for missing confirmed supervisor: {fixedNoSupervisor}");
}

// Approved / pending-head topics must have supervisor review metadata when supervisor is assigned.
await using (NpgsqlCommand fillSupervisorReview = new(
    """
    UPDATE diploma_topic_versions tv
    SET "SupervisorReviewedById" = d."SupervisorId",
        "SupervisorReviewedAt" = COALESCE(tv."SupervisorReviewedAt", tv."SubmittedAt", NOW())
    FROM diplomas d
    WHERE tv."DiplomaId" = d."Id"
      AND tv."Status" IN (1, 2)
      AND d."SupervisorId" IS NOT NULL
      AND d."SupervisorAssignmentStatus" = 1
      AND (tv."SupervisorReviewedById" IS NULL OR tv."SupervisorReviewedAt" IS NULL)
    """,
    connection,
    transaction))
{
    int filledSupervisorReview = await fillSupervisorReview.ExecuteNonQueryAsync();
    Console.WriteLine($"Filled missing supervisor review metadata: {filledSupervisorReview}");
}

// Admitted diplomas must use the Admitted lifecycle status.
await using (NpgsqlCommand fixAdmittedLifecycle = new(
    """
    UPDATE diplomas
    SET "LifecycleStatus" = 7,
        "UpdatedAt" = NOW()
    WHERE "AdmissionStatus" = 1
      AND "LifecycleStatus" <> 7
    """,
    connection,
    transaction))
{
    int fixedAdmitted = await fixAdmittedLifecycle.ExecuteNonQueryAsync();
    Console.WriteLine($"Aligned admitted lifecycle status: {fixedAdmitted}");
}

// Clear admission step pointer when there are no attempts.
await using (NpgsqlCommand clearOrphanStep = new(
    """
    UPDATE diplomas d
    SET "CurrentAdmissionStep" = NULL,
        "UpdatedAt" = NOW()
    WHERE d."CurrentAdmissionStep" IS NOT NULL
      AND NOT EXISTS (
        SELECT 1 FROM diploma_admission_step_attempts a
        WHERE a."DiplomaId" = d."Id")
    """,
    connection,
    transaction))
{
    int clearedOrphanStep = await clearOrphanStep.ExecuteNonQueryAsync();
    Console.WriteLine($"Cleared orphan CurrentAdmissionStep: {clearedOrphanStep}");
}

await transaction.CommitAsync();

Console.WriteLine($"Lifecycle recalculated: {lifecycleUpdates}");
Console.WriteLine($"CurrentAdmissionStep recalculated: {stepUpdates}");
await PrintAuditAsync(connection);

if (args.Contains("--register-migrations", StringComparer.OrdinalIgnoreCase))
{
    await RegisterMigrationsAsync(connection);
}

static async Task RegisterMigrationsAsync(NpgsqlConnection connection)
{
    string[] migrationIds =
    [
        "20260702120000_FixSupervisorReviewedByMismatch",
        "20260702130000_RepairDiplomaWorkflowInconsistencies",
    ];

    foreach (string migrationId in migrationIds)
    {
        await using NpgsqlCommand command = new(
            """
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            SELECT @id, '10.0.0'
            WHERE NOT EXISTS (
                SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = @id)
            """,
            connection);
        command.Parameters.AddWithValue("id", migrationId);
        int inserted = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"{migrationId}: {(inserted > 0 ? "inserted" : "already present")}");
    }
}

static async Task<List<Diploma>> LoadDiplomasAsync(NpgsqlConnection connection)
{
    Dictionary<Guid, Diploma> diplomas = new();

    await using (NpgsqlCommand command = new(
        """
        SELECT "Id", "DefenceSessionId", "StudentId", "SupervisorId", "ReviewerId",
               "SupervisorAssignmentStatus", "ReviewAssignmentStatus", "LifecycleStatus",
               "AdmissionStatus", "CurrentAdmissionStep", "DefenceDate"
        FROM diplomas
        """,
        connection))
    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            Guid id = reader.GetGuid(0);
            diplomas[id] = new Diploma
            {
                Id = id,
                DefenceSessionId = reader.GetGuid(1),
                StudentId = reader.GetGuid(2),
                SupervisorId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                ReviewerId = reader.IsDBNull(4) ? null : reader.GetGuid(4),
                SupervisorAssignmentStatus = (SupervisorAssignmentStatus)reader.GetInt32(5),
                ReviewAssignmentStatus = (ReviewAssignmentStatus)reader.GetInt32(6),
                LifecycleStatus = (DiplomaLifecycleStatus)reader.GetInt32(7),
                AdmissionStatus = (DiplomaAdmissionStatus)reader.GetInt32(8),
                CurrentAdmissionStep = reader.IsDBNull(9) ? null : (AdmissionStep)reader.GetInt32(9),
                DefenceDate = reader.IsDBNull(10) ? null : reader.GetFieldValue<DateOnly>(10),
                DefenceSession = new DefenceSession { Status = DefenceSessionStatus.Active },
            };
        }
    }

    await using (NpgsqlCommand command = new(
        """
        SELECT "Id", "DiplomaId", "VersionNumber", "Title", "Status",
               "SupervisorReviewedById", "SupervisorReviewedAt", "ReviewedById", "ReviewedAt"
        FROM diploma_topic_versions
        ORDER BY "DiplomaId", "VersionNumber"
        """,
        connection))
    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            Guid diplomaId = reader.GetGuid(1);
            if (!diplomas.TryGetValue(diplomaId, out Diploma? diploma))
            {
                continue;
            }

            DiplomaTopicVersion version = new()
            {
                Id = reader.GetGuid(0),
                DiplomaId = diplomaId,
                Diploma = diploma,
                VersionNumber = reader.GetInt32(2),
                Title = reader.GetString(3),
                Status = (TopicVersionStatus)reader.GetInt32(4),
                SupervisorReviewedById = reader.IsDBNull(5) ? null : reader.GetGuid(5),
                SupervisorReviewedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                ReviewedById = reader.IsDBNull(7) ? null : reader.GetGuid(7),
                ReviewedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
            };
            diploma.TopicVersions.Add(version);
        }
    }

    await using (NpgsqlCommand command = new(
        """
        SELECT "Id", "DiplomaId", "Step", "AttemptNumber", "Outcome",
               "Comment", "RecordedById", "RecordedAt", "IsSecretaryOverride"
        FROM diploma_admission_step_attempts
        """,
        connection))
    await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            Guid diplomaId = reader.GetGuid(1);
            if (!diplomas.TryGetValue(diplomaId, out Diploma? diploma))
            {
                continue;
            }

            diploma.AdmissionStepAttempts.Add(new DiplomaAdmissionStepAttempt
            {
                Id = reader.GetGuid(0),
                DiplomaId = diplomaId,
                Diploma = diploma,
                Step = (AdmissionStep)reader.GetInt32(2),
                AttemptNumber = reader.GetInt32(3),
                Outcome = (CheckpointOutcome)reader.GetInt32(4),
                Comment = reader.IsDBNull(5) ? null : reader.GetString(5),
                RecordedById = reader.GetGuid(6),
                RecordedAt = reader.GetDateTime(7),
                IsSecretaryOverride = reader.GetBoolean(8),
            });
        }
    }

    return diplomas.Values.ToList();
}

static async Task PrintAuditAsync(NpgsqlConnection connection)
{
    async Task Count(string label, string sql)
    {
        await using NpgsqlCommand command = new(sql, connection);
        Console.WriteLine($"{label}: {await command.ExecuteScalarAsync()}");
    }

    await Count(
        "A3 SupervisorReviewedById mismatch",
        """
        SELECT COUNT(*) FROM diploma_topic_versions tv
        JOIN diplomas d ON d."Id" = tv."DiplomaId"
        WHERE tv."Status" IN (1,2) AND d."SupervisorId" IS NOT NULL
          AND tv."SupervisorReviewedById" IS NOT NULL
          AND tv."SupervisorReviewedById" <> d."SupervisorId"
        """);

    await Count(
        "A4 Lifecycle>=TopicInReview without confirmed supervisor",
        """
        SELECT COUNT(*) FROM diplomas d
        WHERE d."LifecycleStatus" >= 2
          AND (d."SupervisorId" IS NULL OR d."SupervisorAssignmentStatus" <> 1)
        """);

    await Count(
        "A6 Lifecycle>=TopicApproved without approved topic",
        """
        SELECT COUNT(*) FROM diplomas d
        WHERE d."LifecycleStatus" >= 3
          AND NOT EXISTS (
            SELECT 1 FROM diploma_topic_versions tv
            WHERE tv."DiplomaId" = d."Id" AND tv."Status" = 2)
        """);

    await Count(
        "WorkInProgress+ without approved latest topic",
        """
        SELECT COUNT(*) FROM diplomas d
        JOIN LATERAL (
          SELECT tv."Status"
          FROM diploma_topic_versions tv
          WHERE tv."DiplomaId" = d."Id"
          ORDER BY tv."VersionNumber" DESC
          LIMIT 1
        ) lt ON true
        WHERE d."LifecycleStatus" >= 4 AND lt."Status" <> 2
        """);

    await Count(
        "Approved/PendingHead topic missing supervisor review",
        """
        SELECT COUNT(*) FROM diploma_topic_versions tv
        JOIN diplomas d ON d."Id" = tv."DiplomaId"
        WHERE tv."Status" IN (1, 2)
          AND d."SupervisorId" IS NOT NULL
          AND d."SupervisorAssignmentStatus" = 1
          AND (tv."SupervisorReviewedById" IS NULL OR tv."SupervisorReviewedAt" IS NULL)
        """);

    await Count(
        "Approved topic missing head review metadata",
        """
        SELECT COUNT(*) FROM diploma_topic_versions
        WHERE "Status" = 2
          AND ("ReviewedById" IS NULL OR "ReviewedAt" IS NULL)
        """);

    await Count(
        "Admitted diploma with wrong lifecycle",
        """
        SELECT COUNT(*) FROM diplomas
        WHERE "AdmissionStatus" = 1 AND "LifecycleStatus" <> 7
        """);

    await Count(
        "CurrentAdmissionStep without attempts",
        """
        SELECT COUNT(*) FROM diplomas d
        WHERE d."CurrentAdmissionStep" IS NOT NULL
          AND NOT EXISTS (
            SELECT 1 FROM diploma_admission_step_attempts a
            WHERE a."DiplomaId" = d."Id")
        """);
}
