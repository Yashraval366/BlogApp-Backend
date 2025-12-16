using System.Net;
using System.Text.Json;
using BlogApp.API.DTOs.Response;
using Microsoft.AspNetCore.Diagnostics;

namespace BlogApp.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";

            var response = new Response<object>
            {
                Success = false,
                Message = "Unexpected Error occured",
                Errors = new List<string> { exception.Message }
            };

            var json = JsonSerializer.Serialize(response);

            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}
