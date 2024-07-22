using Microsoft.AspNetCore.Diagnostics;
using TodoApp.Core.DTOs;
using TodoApp.Service.Exceptions;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace TodoApp.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {

        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {

                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundExcepiton=> 404,
                        _ => 500
                    };
                    string message = statusCode switch
                    {
                        400 => exceptionFeature.Error.Message,
                        404 => exceptionFeature.Error.Message,
                        // 500 => "An unexpected error occurred",
                        500 => exceptionFeature.Error.Message,
                    };

                    context.Response.StatusCode = statusCode;
                    
                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, message);
                
                    var options = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
                    
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
                });
            });
        }
    }
}
