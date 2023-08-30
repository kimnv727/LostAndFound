using LostAndFound.API.ResponseWrapper;
using LostAndFound.Core.Exceptions.common;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LostAndFound.API.Middlewares
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            object response;

            switch (ex)
            {
                case ModelStateException msEx:
                    statusCode = 400;
                    response = new ApiBadRequestResponse(msEx.PropertyName, msEx.ErrorMessage);
                    break;
                case HandledException hEx:
                    statusCode = hEx.StatusCode;
                    response = new ApiResponse(hEx.StatusCode, isError: true, message: ex.Message);
                    break;
                default:
                    Log.Error(ex.Message);
                    Log.Error(ex.ToString());
                    statusCode = StatusCodes.Status500InternalServerError;
                    response = new ApiInternalServerErrorResponse(ex.ToString());
                    break;
            }
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}
