using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RenameCheckpointOutcome : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DO $rename$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = current_schema()
                      AND table_name = 'diploma_admission_checkpoints'
                      AND column_name = 'FormattingReviewOutcome')
                THEN
                    ALTER TABLE diploma_admission_checkpoints
                        RENAME COLUMN "FormattingReviewOutcome" TO "Outcome";

                    UPDATE diploma_admission_checkpoints
                    SET "Outcome" = 0
                    WHERE "IsCompleted" = true AND "Outcome" IS NULL;
                END IF;
            END
            $rename$;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Outcome",
            table: "diploma_admission_checkpoints",
            newName: "FormattingReviewOutcome");
    }
}
