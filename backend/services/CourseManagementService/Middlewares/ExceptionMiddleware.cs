// /Middlewares/ExceptionMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // continue le pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Une erreur est survenue.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException      => (int)HttpStatusCode.NotFound,
                _                         => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                status = context.Response.StatusCode,
                message = ex.Message
            });

            await context.Response.WriteAsync(result);
        }
    }
}