using CourseManagementService;
using CourseManagementService.ExternalServices;
using Microsoft.EntityFrameworkCore;
using CourseManagementService.Services;
using CourseManagementService.Services.Implementations;
using CourseManagementService.Services.Interfaces; // Ton espace de noms pour les modèles

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient<AuthHttpClientService>();


// Configuration du DbContext pour PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IFiliereService, FiliereService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<INoteService,    NoteService>();   // 🟢 ← à ajouter


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // ou ton URL réelle
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionFormation API V1");
        // Optionnellement : c.RoutePrefix = "swagger"; // c’est déjà la valeur par défaut
    });
}

// Ajouter le middleware global d'exception
app.UseMiddleware<ExceptionMiddleware>();

app.UseStaticFiles();

app.UseCors("AllowFrontend");


app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

ApplyMigration();
app.Run();

// --------------------------------------------------
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();
}