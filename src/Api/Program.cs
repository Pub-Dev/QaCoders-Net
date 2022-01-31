using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using QaCoders_Net;
using QaCoders_Net.Interfaces.Presenters;
using QaCoders_Net.Middlewares;
using QaCoders_Net.Presenters;
using QaCoders_Net.Providers;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    options.CustomOperationIds(operation =>
    {
        var actionDescription = (ControllerActionDescriptor)operation.ActionDescriptor;

        var actionName = actionDescription.ActionName;

        return $"{actionName}";
    });
    options.MapType<decimal>(() => new OpenApiSchema
    {
        Type = "number",
        Format = "decimal"
    });
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScoped<NotificationContext>();
builder.Services.AddScoped<IPresenter, Presenter>();

var app = builder.Build();

if (args.Any(x => x.Contains("swagger")))
{
    Console.Write("Generating Swagger file");

    var swaggerProvider = (ISwaggerProvider)app.Services.GetService(typeof(ISwaggerProvider));

    var doc = swaggerProvider.GetSwagger("v1");

    File.WriteAllText("swagger-generated.json", doc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0));

    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
