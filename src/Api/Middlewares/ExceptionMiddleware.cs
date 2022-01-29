using Microsoft.AspNetCore.Diagnostics;
using QaCoders_Net.Response;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QaCoders_Net.Middlewares;

public static class ExceptionMiddleware
{
    public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
    {
        builder.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

                var errorDetails = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";

                var statusCode = HttpStatusCode.InternalServerError.GetHashCode();

                var rootId = Guid.Parse(System.Diagnostics.Activity.Current.RootId);

                var problemDetails = new ErrorResponse(new List<ErrorMessageResponse>
                {
                    new ErrorMessageResponse()
                    {
                        Id = rootId,
                        ErrorCode = "500",
                        Message = errorDetails
                    }
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, GetJsonSerializerOptions()));
            });
        });

        return builder;
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var serializer = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        serializer.Converters.Add(new JsonStringEnumConverter());

        return serializer;
    }
}
