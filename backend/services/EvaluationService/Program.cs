using System.Text.Json.Serialization;
using AutoMapper;
using EvaluationService.Data;
using EvaluationService.DTOs;
using EvaluationService.Repositories;
using EvaluationService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL
builder.Services.AddDbContext<EvaluationsDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

IMapper mapper = MappingConfig.RegisterMappings().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority            = builder.Configuration["Auth:Authority"];
        opt.Audience             = builder.Configuration["Auth:Audience"];
        opt.RequireHttpsMetadata = true;
    });

// HttpClient + Polly

/*
builder.Services.AddHttpClient<IExternalValidationService, ExternalValidationService>()
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1)));
    // Repeating the http request up to 3 times and waiting after each 
*/

// Register Repositories
builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IQuestionsStandardRepository, QuestionsStandardRepository>();

// Business services
builder.Services.AddScoped<IExternalValidationService, ExternalValidationService>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IAIAnalyzer, AIAnalyzer>();
builder.Services.AddScoped<IEvaluationService, EvaluationService.Services.EvaluationService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionsStandardService, QuestionsStandardService>();

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/*
=> HttpClient is a high-level abstraction, but under the hood, it uses a HttpMessageHandler that:

     1. opens and reuses TCP connections (important for performance)

     2. caches DNS results

=> If you recreate it too often:

     1. You lose connection reuse (bad for performance)

     2. You may hit socket exhaustion

     3. You miss DNS updates if you keep it alive forever
*/
    
builder.Services.AddHttpClient<IExternalValidationService, ExternalValidationService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(3))
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(2));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// to serialize or map enum values to string values in (swagger, post_man, with axios ...) whoever consumes the API

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EvaluationService", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Description  = "Bearer {token}",
        In           = ParameterLocation.Header,
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        Reference    = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id   = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EvaluationService v1");
        c.RoutePrefix = string.Empty;          // Swagger at root /
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

ApplyMigration();
app.Run();

// --------------------------------------------------
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EvaluationsDbContext>();
    if (db.Database.GetPendingMigrations().Any())
        db.Database.Migrate();
}
