using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddWorkInProgressByStudentLifecycle : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE "Diplomas"
            SET "LifecycleStatus" = "LifecycleStatus" + 1
            WHERE "LifecycleStatus" >= 4;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE "Diplomas"
            SET "LifecycleStatus" = "LifecycleStatus" - 1
            WHERE "LifecycleStatus" >= 5;
            """);
    }
}
