using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class Initial45555555555555 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RapportFQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    FiliereId = table.Column<int>(type: "integer", nullable: false),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportFQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RapportMQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionnaireId = table.Column<int>(type: "integer", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportMQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesEnseignants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    AverageM = table.Column<double>(type: "double precision", nullable: true),
                    MedianNotes = table.Column<double>(type: "double precision", nullable: true),
                    StdEv = table.Column<double>(type: "double precision", nullable: true),
                    ModuleMaxPass = table.Column<string>(type: "text", nullable: true),
                    ModuleMinPass = table.Column<string>(type: "text", nullable: true),
                    MaxModuleRated = table.Column<string>(type: "text", nullable: true),
                    NoteMax = table.Column<double>(type: "double precision", nullable: true),
                    NoteMin = table.Column<double>(type: "double precision", nullable: true),
                    PassRate = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rapport = table.Column<string>(type: "text", nullable: false),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesEnseignants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesEtudiants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    FiliereId = table.Column<int>(type: "integer", nullable: true),
                    NoteMoyenne = table.Column<double>(type: "double precision", nullable: false),
                    NoteMax = table.Column<double>(type: "double precision", nullable: false),
                    NoteMin = table.Column<double>(type: "double precision", nullable: false),
                    PassRate = table.Column<double>(type: "double precision", nullable: true),
                    Median = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observation = table.Column<string>(type: "text", nullable: true),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesEtudiants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesFilieres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FiliereId = table.Column<int>(type: "integer", nullable: false),
                    FiliereName = table.Column<string>(type: "text", nullable: false),
                    FiliereRoot = table.Column<string>(type: "text", nullable: true),
                    NbrEtds = table.Column<int>(type: "integer", nullable: true),
                    AverageRating = table.Column<double>(type: "double precision", nullable: true),
                    AverageMoyenne = table.Column<double>(type: "double precision", nullable: true),
                    ModuleMaxPass = table.Column<string>(type: "text", nullable: true),
                    ModuleMinPass = table.Column<string>(type: "text", nullable: true),
                    Majorant = table.Column<string>(type: "text", nullable: true),
                    MaxMoyenne = table.Column<double>(type: "double precision", nullable: true),
                    MaxModuleRated = table.Column<string>(type: "text", nullable: true),
                    MedianRating = table.Column<double>(type: "double precision", nullable: true),
                    StdDevRating = table.Column<double>(type: "double precision", nullable: true),
                    SatisfactionRate = table.Column<double>(type: "double precision", nullable: true),
                    NpsScore = table.Column<double>(type: "double precision", nullable: true),
                    PositiveFeedbackPct = table.Column<double>(type: "double precision", nullable: true),
                    NegativeFeedbackPct = table.Column<double>(type: "double precision", nullable: true),
                    PassRate = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesFilieres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: true),
                    AverageNotes = table.Column<double>(type: "double precision", nullable: true),
                    NoteMax = table.Column<double>(type: "double precision", nullable: true),
                    NoteMin = table.Column<double>(type: "double precision", nullable: true),
                    MedianNotes = table.Column<double>(type: "double precision", nullable: true),
                    PassRate = table.Column<double>(type: "double precision", nullable: true),
                    StdevNotes = table.Column<double>(type: "double precision", nullable: true),
                    ParticipationRate = table.Column<double>(type: "double precision", nullable: true),
                    SatisfactionRate = table.Column<double>(type: "double precision", nullable: true),
                    NpsScore = table.Column<double>(type: "double precision", nullable: true),
                    PositiveFeedbackPct = table.Column<double>(type: "double precision", nullable: true),
                    NegativeFeedbackPct = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rapport = table.Column<string>(type: "text", nullable: true),
                    RapportPdf = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesModules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RapportFQs");

            migrationBuilder.DropTable(
                name: "RapportMQs");

            migrationBuilder.DropTable(
                name: "StatistiquesEnseignants");

            migrationBuilder.DropTable(
                name: "StatistiquesEtudiants");

            migrationBuilder.DropTable(
                name: "StatistiquesFilieres");

            migrationBuilder.DropTable(
                name: "StatistiquesModules");
        }
    }
}
