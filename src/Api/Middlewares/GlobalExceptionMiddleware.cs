using System.Net;
using ProductsCRUD.Domain.Exceptions;

namespace ProductsCRUD.Api.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de domínio");
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "DOMAIN_ERROR", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado");
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "NOT_FOUND", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "Erro interno inesperado.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new
        {
            traceId = context.TraceIdentifier,
            error = new
            {
                code,
                message
            }
        };

        await context.Response.WriteAsJsonAsync(payload);
    }
}