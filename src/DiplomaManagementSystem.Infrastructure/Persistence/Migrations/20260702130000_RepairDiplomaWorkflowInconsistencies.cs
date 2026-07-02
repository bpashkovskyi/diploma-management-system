using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RepairDiplomaWorkflowInconsistencies : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE diplomas d
            SET "LifecycleStatus" = 2,
                "UpdatedAt" = NOW()
            FROM (
                SELECT tv."DiplomaId"
                FROM diploma_topic_versions tv
                INNER JOIN (
                    SELECT "DiplomaId", MAX("VersionNumber") AS max_version
                    FROM diploma_topic_versions
                    GROUP BY "DiplomaId"
                ) latest
                    ON latest."DiplomaId" = tv."DiplomaId"
                   AND latest.max_version = tv."VersionNumber"
                WHERE tv."Status" = 3
            ) rejected_latest
            WHERE d."Id" = rejected_latest."DiplomaId"
              AND d."AdmissionStatus" = 0
              AND d."LifecycleStatus" > 2
              AND d."SupervisorId" IS NOT NULL
              AND d."SupervisorAssignmentStatus" = 1
              AND NOT EXISTS (
                SELECT 1
                FROM diploma_admission_step_attempts a
                WHERE a."DiplomaId" = d."Id");

            UPDATE diplomas
            SET "LifecycleStatus" = 0,
                "UpdatedAt" = NOW()
            WHERE "AdmissionStatus" = 0
              AND ("SupervisorId" IS NULL OR "SupervisorAssignmentStatus" <> 1)
              AND "LifecycleStatus" >= 2;

            UPDATE diplomas d
            SET "CurrentAdmissionStep" = NULL,
                "UpdatedAt" = NOW()
            WHERE d."CurrentAdmissionStep" IS NOT NULL
              AND NOT EXISTS (
                SELECT 1
                FROM diploma_admission_step_attempts a
                WHERE a."DiplomaId" = d."Id");

            UPDATE diploma_topic_versions
            SET "ReviewedById" = NULL,
                "ReviewedAt" = NULL
            WHERE "Status" = 0
              AND ("ReviewedById" IS NOT NULL OR "ReviewedAt" IS NOT NULL);

            UPDATE diplomas
            SET "LifecycleStatus" = 7,
                "UpdatedAt" = NOW()
            WHERE "AdmissionStatus" = 1
              AND "LifecycleStatus" <> 7;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
