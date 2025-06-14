using AnalyticsService.Data;
using Microsoft.EntityFrameworkCore ; 
using Microsoft.OpenApi.Models; 
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Models;
using AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("AnalyticsDb");

// 2) Enregistre le DbContext pour l’exécution de l’API
builder.Services.AddDbContext<AnalyticsDbContext>(opts =>
    opts.UseNpgsql(conn));


// Charger la section "External" dans une variable de configuration
var config = builder.Configuration.GetSection("EvaluationService");
// 3) Enregistrement du service de statistiques pour le module
builder.Services.AddScoped<IStatistiqueModuleService<StatistiqueModule>, StatistiqueModuleService>();
// 3) Enregistrement du service de statistiques pour le module
builder.Services.AddScoped<IStatistiqueFiliereService<StatistiqueFiliere>, StatistiqueFiliereService>();
builder.Services.AddScoped<IStatistiqueUserService<StatistiqueEtudiant>, StatistiqueEtdService>();
builder.Services.AddScoped<IStatistiqueUserService<StatistiqueEnseignant>, StatistiqueEnsService>();
builder.Services.AddScoped<IReportPropertyService, ReportPropertyService>(); 
builder.Services.AddScoped<IReportUserService, ReportEnsService>();
builder.Services.AddScoped<IReportEtdService, ReportEtdService>();


// Ajouter les services HTTP avec l'URL de base
builder.Services.AddHttpClient<IQuestionnaireClient, HttpQuestionnaireClient>();
builder.Services.AddHttpClient<INoteClient, HttpNoteClient>(); 
builder.Services.AddHttpClient<IAnswerClient, HttpAnswerClient>(); 
builder.Services.AddHttpClient<IQuestionClient, HttpQuestionClient>(); 
builder.Services.AddHttpClient<IFiliereClient, HttpFiliereClient>(); 
builder.Services.AddHttpClient<IModuleClient, HttpModuleClient>(); 
builder.Services.AddHttpClient<IEvaluationClient, HttpEvaluationClient>();
builder.Services.AddHttpClient<IGroqAIClient, GroqAIClient>();
// … tes autres services (HttpClients, ReportService, etc.)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin() // ou ton URL réelle
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();
}
app.UseCors("AllowFrontend");
ApplyMigration(); 
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();  