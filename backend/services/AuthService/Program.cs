using AuthService.Data;
using AuthService.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using AuthService.Models;
using AuthService.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


builder.Services
    .AddIdentityCore<ApplicationUser>()           // base utilisateurs
    .AddRoles<IdentityRole>()                     // gestion des rôles
    .AddEntityFrameworkStores<AuthDbContext>()    // EF store
    .AddDefaultTokenProviders();       

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(); // endpoints (login, refresh, etc.)

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Program.cs (après builder.Services.AddDbContext…)
builder.Services.AddScoped<IEnseignantService, EnseignantService>();
// Program.cs – juste après les autres services
builder.Services.AddScoped<IEtudiantService, EtudiantService>();
// Program.cs (après les autres AddScoped)
builder.Services.AddScoped<IExternalEvaluatorService, ExternalEvaluatorService>();


builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "Auth Demo",
                Version = "v1"
            });
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Please enter a token",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer",
        });
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    });

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

// Migrations + seed rôles
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    db.Database.Migrate();                         // applique TPT
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}


app.MapIdentityApi<ApplicationUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();

// --------------------------------------------------
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();
}
