using Ecommerce.Core.src.Common;
using System.Net;
using System.Text.Json;

namespace Ecommerce.WebAPI.src.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            string message;

            // Check if the exception is of type AppException
            if (ex is AppException appException)
            {
                statusCode = appException.StatusCode;
                message = appException.Message;
            }
            // If it's not an AppException, handle it as a generic internal server error
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "Internal Server Error - " + ex.Message;
            }

            _logger.LogError($"An error occurred: {ex}");

            // Set the HTTP status code and response body
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
