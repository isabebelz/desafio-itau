using CompraProgramada.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using ValidationException = CompraProgramada.Application.Common.Exceptions.ValidationException;

namespace CompraProgramada.Api.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    errors = ex.Errors
                });

                await httpContext.Response.WriteAsync(result);
            }
            catch (DomainException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var result = JsonSerializer.Serialize(new
                {
                    error = ex.Message
                });

                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new
                {
                    error = "Erro interno do servidor."
                });

                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}
