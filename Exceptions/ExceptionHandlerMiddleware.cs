using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
            await response.WriteAsync(result);
        }
    }
}