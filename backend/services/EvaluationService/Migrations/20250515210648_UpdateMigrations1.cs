using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Questionnaire_Type",
                table: "Questionnaires");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Questionnaire_Type",
                table: "Questionnaires",
                sql: "NOT(\"FiliereId\" IS NULL AND \"ModuleId\" IS NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Questionnaire_Type",
                table: "Questionnaires");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Questionnaire_Type",
                table: "Questionnaires",
                sql: "(\"FiliereId\" IS NOT NULL AND \"ModuleId\" IS NULL AND \"Type\" = 0) OR (\"ModuleId\" IS NOT NULL AND \"FiliereId\" IS NULL AND \"Type\" = 1)");
        }
    }
}
