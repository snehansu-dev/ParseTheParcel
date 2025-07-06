using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ParseTheParcel.Application.DTOs;

namespace ParseTheParcel.Application.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request error.");
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "Invalid request.", ex.Message);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "JSON parsing error.");
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "Invalid JSON payload.", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.", ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message, string? detail)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var error = new ErrorResponse
            {
                Message = message,
                Detail = detail,
                TraceId = context.TraceIdentifier
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(error, options);

            await context.Response.WriteAsync(json);
        }
    }
}
