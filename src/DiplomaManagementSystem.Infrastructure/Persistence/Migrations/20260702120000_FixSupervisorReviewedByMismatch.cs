using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class FixSupervisorReviewedByMismatch : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE diploma_topic_versions tv
            SET "SupervisorReviewedById" = d."SupervisorId"
            FROM diplomas d
            WHERE tv."DiplomaId" = d."Id"
              AND d."SupervisorId" IS NOT NULL
              AND tv."Status" IN (1, 2)
              AND (
                tv."SupervisorReviewedById" IS NULL
                OR tv."SupervisorReviewedById" <> d."SupervisorId"
              );

            UPDATE diploma_topic_versions tv
            SET "SupervisorReviewedAt" = COALESCE(tv."SupervisorReviewedAt", tv."SubmittedAt")
            FROM diplomas d
            WHERE tv."DiplomaId" = d."Id"
              AND d."SupervisorId" IS NOT NULL
              AND tv."Status" IN (1, 2)
              AND tv."SupervisorReviewedById" = d."SupervisorId"
              AND tv."SupervisorReviewedAt" IS NULL;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
