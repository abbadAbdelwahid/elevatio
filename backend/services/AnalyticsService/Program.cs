using AnalyticsService.Data;
using Microsoft.EntityFrameworkCore ; 
using Microsoft.OpenApi.Models; 
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.Http;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("AnalyticsDb");

// 2) Enregistre le DbContext pour l’exécution de l’API
builder.Services.AddDbContext<AnalyticsDbContext>(opts =>
    opts.UseNpgsql(conn));

// Charger la configuration des URLs depuis le fichier appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Charger la section "External" dans une variable de configuration
var config = builder.Configuration.GetSection("External");

// Ajouter les services HTTP avec l'URL de base
builder.Services.AddHttpClient<IQuestionnaireClient, HttpQuestionnaireClient>(c =>
{
    // Utiliser l'URL du Mock Server
    c.BaseAddress = new Uri(config["MockServerUrl"]);
});

builder.Services.AddHttpClient<IAnswerClient, HttpAnswerClient>(c =>
{
    // Utiliser l'URL du Mock Server
    c.BaseAddress = new Uri(config["MockServerUrl"]);
}); 
builder.Services.AddHttpClient<IQuestionClient, HttpQuestionClient>(c =>
{
    // Utiliser l'URL du Mock Server
    c.BaseAddress = new Uri(config["MockServerUrl"]);
});

// … tes autres services (HttpClients, ReportService, etc.)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();