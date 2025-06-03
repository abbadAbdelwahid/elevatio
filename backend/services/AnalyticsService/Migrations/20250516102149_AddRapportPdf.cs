using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class AddRapportPdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RapportEns");

            migrationBuilder.DropTable(
                name: "RapportFs");

            migrationBuilder.DropTable(
                name: "RapportMs");

            migrationBuilder.AddColumn<string>(
                name: "Rapport",
                table: "StatistiquesModules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "RapportPdf",
                table: "StatistiquesModules",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RapportPdf",
                table: "StatistiquesFilieres",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<double>(
                name: "AverageRatingM",
                table: "StatistiquesEnseignants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Rapport",
                table: "StatistiquesEnseignants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "RapportPdf",
                table: "StatistiquesEnseignants",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "RapportMQs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rapport",
                table: "StatistiquesModules");

            migrationBuilder.DropColumn(
                name: "RapportPdf",
                table: "StatistiquesModules");

            migrationBuilder.DropColumn(
                name: "RapportPdf",
                table: "StatistiquesFilieres");

            migrationBuilder.DropColumn(
                name: "AverageRatingM",
                table: "StatistiquesEnseignants");

            migrationBuilder.DropColumn(
                name: "Rapport",
                table: "StatistiquesEnseignants");

            migrationBuilder.DropColumn(
                name: "RapportPdf",
                table: "StatistiquesEnseignants");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "RapportMQs");

            migrationBuilder.CreateTable(
                name: "RapportEns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportEns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportFs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: false),
                    FiliereId = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportMs", x => x.Id);
                });
        }
    }
}
