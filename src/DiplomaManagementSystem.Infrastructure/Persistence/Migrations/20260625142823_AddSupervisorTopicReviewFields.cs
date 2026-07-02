using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddSupervisorTopicReviewFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "SupervisorReviewedAt",
            table: "diploma_topic_versions",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SupervisorReviewedById",
            table: "diploma_topic_versions",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_diploma_topic_versions_SupervisorReviewedById",
            table: "diploma_topic_versions",
            column: "SupervisorReviewedById");

        migrationBuilder.AddForeignKey(
            name: "FK_diploma_topic_versions_users_SupervisorReviewedById",
            table: "diploma_topic_versions",
            column: "SupervisorReviewedById",
            principalTable: "users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.Sql(
            """
            UPDATE diploma_topic_versions
            SET "SupervisorReviewedById" = "ReviewedById",
                "SupervisorReviewedAt" = "ReviewedAt",
                "ReviewedById" = NULL,
                "ReviewedAt" = NULL
            WHERE "Status" = 1
              AND "ReviewedById" IS NOT NULL
              AND "SupervisorReviewedById" IS NULL;

            UPDATE diploma_topic_versions tv
            SET "SupervisorReviewedById" = tv."ReviewedById",
                "SupervisorReviewedAt" = tv."ReviewedAt",
                "ReviewedById" = head."EmployeeId"
            FROM diplomas d
            JOIN annual_role_assignments head
              ON head."DefenceSessionId" = d."DefenceSessionId"
             AND head."RoleType" = 0
            WHERE tv."DiplomaId" = d."Id"
              AND tv."Status" = 2
              AND tv."ReviewedById" IS NOT NULL
              AND tv."SupervisorReviewedById" IS NULL
              AND d."SupervisorId" IS NOT NULL
              AND tv."ReviewedById" = d."SupervisorId";
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_diploma_topic_versions_users_SupervisorReviewedById",
            table: "diploma_topic_versions");

        migrationBuilder.DropIndex(
            name: "IX_diploma_topic_versions_SupervisorReviewedById",
            table: "diploma_topic_versions");

        migrationBuilder.DropColumn(
            name: "SupervisorReviewedAt",
            table: "diploma_topic_versions");

        migrationBuilder.DropColumn(
            name: "SupervisorReviewedById",
            table: "diploma_topic_versions");
    }
}
