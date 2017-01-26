using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception == null) return;

        //TODO log exception to some logfile

        var code = HttpStatusCode.InternalServerError;

     await WriteExceptionAsync(context, exception, code).ConfigureAwait(false);
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)code;
        await response.WriteAsync(JsonConvert.SerializeObject(new
        {
            error = new
            {
                message = exception.Message,
                exception = exception.GetType().Name,
                stackTrace = exception.StackTrace,
                innerException = exception.InnerException
            }
        })).ConfigureAwait(false);
    }
}