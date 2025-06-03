using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnalyticsService.Migrations
{
    /// <inheritdoc />
    public partial class CreateStatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatistiquesEnseignants",
                columns: table => new
                {
                    StatistiqueEnseignantId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: false),
                    MedianRating = table.Column<double>(type: "double precision", nullable: false),
                    StdDevRating = table.Column<double>(type: "double precision", nullable: false),
                    ParticipationRate = table.Column<double>(type: "double precision", nullable: false),
                    NpsScore = table.Column<double>(type: "double precision", nullable: false),
                    PositiveFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    NegativeFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    PeerReviewScore = table.Column<double>(type: "double precision", nullable: false),
                    ResponseTimeAvg = table.Column<double>(type: "double precision", nullable: false),
                    ImprovementTrend = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesEnseignants", x => x.StatistiqueEnseignantId);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesFilieres",
                columns: table => new
                {
                    StatistiqueFiliereId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FiliereId = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: false),
                    MedianRating = table.Column<double>(type: "double precision", nullable: false),
                    StdDevRating = table.Column<double>(type: "double precision", nullable: false),
                    SatisfactionRate = table.Column<double>(type: "double precision", nullable: false),
                    ParticipationRate = table.Column<double>(type: "double precision", nullable: false),
                    NpsScore = table.Column<double>(type: "double precision", nullable: false),
                    PositiveFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    NegativeFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    ActionPlanCount = table.Column<int>(type: "integer", nullable: false),
                    ImprovementTrend = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesFilieres", x => x.StatistiqueFiliereId);
                });

            migrationBuilder.CreateTable(
                name: "StatistiquesModules",
                columns: table => new
                {
                    StatistiqueModuleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: false),
                    MedianRating = table.Column<double>(type: "double precision", nullable: false),
                    StdDevRating = table.Column<double>(type: "double precision", nullable: false),
                    ParticipationRate = table.Column<double>(type: "double precision", nullable: false),
                    CompletionTimeAvg = table.Column<double>(type: "double precision", nullable: false),
                    DropoutRate = table.Column<double>(type: "double precision", nullable: false),
                    NpsScore = table.Column<double>(type: "double precision", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false),
                    PositiveFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    NegativeFeedbackPct = table.Column<double>(type: "double precision", nullable: false),
                    ImprovementTrend = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistiquesModules", x => x.StatistiqueModuleId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatistiquesEnseignants");

            migrationBuilder.DropTable(
                name: "StatistiquesFilieres");

            migrationBuilder.DropTable(
                name: "StatistiquesModules");
        }
    }
}
