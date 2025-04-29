using SalesDomain.Abstractions.Response;
using System.Net;
using System.Text.Json;

namespace SalesApi.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = Result.CreateErrorResponse("An unexpected error occurs. Please try again later!", EStatusResponse.Error, exception.Message);

        var payload = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(payload);
    }
}