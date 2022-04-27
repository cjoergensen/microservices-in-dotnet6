using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared.Logging;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlerMiddleware> logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in path '{requestPath}'. Request = '{request}'", context.Request.Path.Value, context.Request);
            throw;
        }
    }
}
