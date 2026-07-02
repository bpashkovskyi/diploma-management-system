using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaManagementSystem.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RemoveAcademicYearSessionRoles : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("TRUNCATE TABLE annual_role_assignments;");

        migrationBuilder.DropForeignKey(
            name: "FK_annual_role_assignments_academic_years_AcademicYearId",
            table: "annual_role_assignments");

        migrationBuilder.DropForeignKey(
            name: "FK_defence_sessions_academic_years_AcademicYearId",
            table: "defence_sessions");

        migrationBuilder.DropIndex(
            name: "IX_annual_role_assignments_AcademicYearId_RoleType",
            table: "annual_role_assignments");

        migrationBuilder.DropIndex(
            name: "IX_defence_sessions_AcademicYearId",
            table: "defence_sessions");

        migrationBuilder.DropColumn(
            name: "AcademicYearId",
            table: "annual_role_assignments");

        migrationBuilder.DropColumn(
            name: "AcademicYearId",
            table: "defence_sessions");

        migrationBuilder.AddColumn<Guid>(
            name: "DefenceSessionId",
            table: "annual_role_assignments",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<int>(
            name: "Year",
            table: "defence_sessions",
            type: "integer",
            nullable: false,
            defaultValue: 2026);

        migrationBuilder.AlterColumn<Guid>(
            name: "DefenceSessionId",
            table: "annual_role_assignments",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_annual_role_assignments_DefenceSessionId_RoleType",
            table: "annual_role_assignments",
            columns: new[] { "DefenceSessionId", "RoleType" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_annual_role_assignments_defence_sessions_DefenceSessionId",
            table: "annual_role_assignments",
            column: "DefenceSessionId",
            principalTable: "defence_sessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.DropTable(
            name: "academic_years");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_annual_role_assignments_defence_sessions_DefenceSessionId",
            table: "annual_role_assignments");

        migrationBuilder.DropIndex(
            name: "IX_annual_role_assignments_DefenceSessionId_RoleType",
            table: "annual_role_assignments");

        migrationBuilder.DropColumn(
            name: "DefenceSessionId",
            table: "annual_role_assignments");

        migrationBuilder.DropColumn(
            name: "Year",
            table: "defence_sessions");

        migrationBuilder.CreateTable(
            name: "academic_years",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Label = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_academic_years", x => x.Id);
            });

        migrationBuilder.AddColumn<Guid>(
            name: "AcademicYearId",
            table: "defence_sessions",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "AcademicYearId",
            table: "annual_role_assignments",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_academic_years_Label",
            table: "academic_years",
            column: "Label",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_annual_role_assignments_AcademicYearId_RoleType",
            table: "annual_role_assignments",
            columns: new[] { "AcademicYearId", "RoleType" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_defence_sessions_AcademicYearId",
            table: "defence_sessions",
            column: "AcademicYearId");

        migrationBuilder.AddForeignKey(
            name: "FK_annual_role_assignments_academic_years_AcademicYearId",
            table: "annual_role_assignments",
            column: "AcademicYearId",
            principalTable: "academic_years",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_defence_sessions_academic_years_AcademicYearId",
            table: "defence_sessions",
            column: "AcademicYearId",
            principalTable: "academic_years",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
