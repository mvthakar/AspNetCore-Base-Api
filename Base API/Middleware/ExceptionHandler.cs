using Microsoft.AspNetCore.Diagnostics;

namespace BaseAPI.Middleware;

public static class ExceptionHandler
{
    public static void UseExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(LogException));
    }

    public static async Task LogException(HttpContext httpContext)
    {
        var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is BadHttpRequestException)
        {
            await Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Request was not formatted property").ExecuteAsync(httpContext);
        }
        else
        {
            await Results.Problem().ExecuteAsync(httpContext);
        }
    }
}
