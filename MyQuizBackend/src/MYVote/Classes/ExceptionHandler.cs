﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyQuizBackend;
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

        var code = HttpStatusCode.InternalServerError;

        writeExceptionToLogFile(exception);

        await WriteExceptionAsync(context, exception, code).ConfigureAwait(false);
    }

    public static void writeExceptionToLogFile(Exception exception)
    {
        var stringToLog = exception.Message + Environment.NewLine + exception.GetType().Name + Environment.NewLine +
                          exception.StackTrace + Environment.NewLine + exception.InnerException;
        Logger.writeToLogFile(stringToLog);
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