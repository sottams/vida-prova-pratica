using Microsoft.AspNetCore.Http;
using SistemaCompra.Domain.Core;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaCompra.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessRuleException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (ArgumentNullException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = "Ocorreu um erro interno." }));
            }
        }
    }
}
