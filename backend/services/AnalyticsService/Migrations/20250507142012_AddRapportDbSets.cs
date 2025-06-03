using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class AddRapportDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatistiqueModuleId",
                table: "StatistiquesModules",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StatistiqueFiliereId",
                table: "StatistiquesFilieres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StatistiqueEnseignantId",
                table: "StatistiquesEnseignants",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "RapportEns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportEns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportFQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportFQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportFs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FiliereId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportMQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportMQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportMs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RapportEns");

            migrationBuilder.DropTable(
                name: "RapportFQs");

            migrationBuilder.DropTable(
                name: "RapportFs");

            migrationBuilder.DropTable(
                name: "RapportMQs");

            migrationBuilder.DropTable(
                name: "RapportMs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StatistiquesModules",
                newName: "StatistiqueModuleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StatistiquesFilieres",
                newName: "StatistiqueFiliereId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StatistiquesEnseignants",
                newName: "StatistiqueEnseignantId");
        }
    }
}
