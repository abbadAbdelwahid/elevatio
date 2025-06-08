using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class Initial4555555 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "RapportMQs");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "RapportFQs");

            migrationBuilder.RenameColumn(
                name: "GeneratedAt",
                table: "RapportMQs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "GeneratedAt",
                table: "RapportFQs",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<double>(
                name: "StdDevRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "PositiveFeedbackPct",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "ParticipationRate",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "NpsScore",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "NegativeFeedbackPct",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "MedianRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "ImprovementTrend",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "DropoutRate",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "CompletionTimeAvg",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "StatistiquesModules",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<byte[]>(
                name: "RapportPdf",
                table: "RapportMQs",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FiliereId",
                table: "RapportFQs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RapportPdf",
                table: "RapportFQs",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RapportPdf",
                table: "RapportMQs");

            migrationBuilder.DropColumn(
                name: "FiliereId",
                table: "RapportFQs");

            migrationBuilder.DropColumn(
                name: "RapportPdf",
                table: "RapportFQs");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RapportMQs",
                newName: "GeneratedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RapportFQs",
                newName: "GeneratedAt");

            migrationBuilder.AlterColumn<double>(
                name: "StdDevRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PositiveFeedbackPct",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ParticipationRate",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "NpsScore",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "NegativeFeedbackPct",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MedianRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ImprovementTrend",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DropoutRate",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CompletionTimeAvg",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "StatistiquesModules",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "StatistiquesModules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "RapportMQs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "RapportFQs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
