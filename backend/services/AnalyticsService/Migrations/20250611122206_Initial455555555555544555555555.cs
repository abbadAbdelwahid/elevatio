using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class Initial455555555555544555555555 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "NegativeFeedBackPct",
                table: "StatistiquesEnseignants",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PositiveFeedBackPct",
                table: "StatistiquesEnseignants",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "RapportPdf",
                table: "RapportMQs",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegativeFeedBackPct",
                table: "StatistiquesEnseignants");

            migrationBuilder.DropColumn(
                name: "PositiveFeedBackPct",
                table: "StatistiquesEnseignants");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RapportPdf",
                table: "RapportMQs",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}
