using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Todo.Api.Configurations;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Could not process the request with TraceId: {TraceId}\n" +
                "Exception: {Exception}\n" +
                "TargetSite: {TargetSite}",
                Activity.Current?.Id,
                ex.Message,
                ex.TargetSite);

            await Results.Problem(
                    title: "An error occurred while processing your request.",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?>
                    {
                        {"traceId", Activity.Current?.Id},
                        {"message", ex.Message}
                    }
                )
                .ExecuteAsync(httpContext);
        }
    }
}