using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BMSAPI.Exceptions;

public class ExceptionHandlerMiddleware {
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception error) {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch {
                ApiException e => (int) HttpStatusCode.BadRequest,
                DbUpdateException e => (int) HttpStatusCode.Conflict,
                KeyNotFoundException e => (int) HttpStatusCode.NotFound,
                _ => (int) HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new {message = error?.Message});
            // Console.WriteLine("----------------------------------------------------");
            // Console.WriteLine("ExceptionHandlerMiddleware");
            // Console.WriteLine("error message: " + error.Message);
            // Console.WriteLine("response status code: " + response.StatusCode);
            // Console.WriteLine("stack trace: " + error);
            // Console.WriteLine("----------------------------------------------------");

            // await response.WriteAsync(result);
            await response.WriteAsync("Internal server error");
        }
    }
}