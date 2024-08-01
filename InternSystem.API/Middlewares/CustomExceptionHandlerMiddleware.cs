using System.Text.Json;
using FluentValidation;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Domain.BaseException;

namespace InternSystem.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            try
            {
                var check = context.User.Identity.IsAuthenticated;
                await _next(context);
            }
            catch (CoreException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = ex.StatusCode;
                var result = JsonSerializer.Serialize(new { ex.Code, ex.Message, ex.AdditionalData });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (ErrorException ex)
            {
                _logger.LogError(ex, ex.ErrorDetail.ErrorMessage.ToString());
                context.Response.StatusCode = ex.StatusCode;
                var result = JsonSerializer.Serialize(ex.ErrorDetail);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errorResponse = new
                {
                    errorCode = "Validation Error",
                    errorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage
                };
                var result = JsonSerializer.Serialize(errorResponse);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var result = JsonSerializer.Serialize(new { error = $"An unexpected error occurred. Detail{ex.Message}" });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        }
    }
}
