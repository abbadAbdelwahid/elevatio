using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrations_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatName",
                table: "Questions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_StandardQuestionId",
                table: "Questions",
                column: "StandardQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_StandardQuestions_StandardQuestionId",
                table: "Questions",
                column: "StandardQuestionId",
                principalTable: "StandardQuestions",
                principalColumn: "StandardQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_StandardQuestions_StandardQuestionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_StandardQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "StatName",
                table: "Questions");
        }
    }
}
