using Microsoft.OpenApi.Models;
using ParseTheParcel.Application.DTOs;
using ParseTheParcel.Application.Interfaces;
using ParseTheParcel.Application.Models;
using ParseTheParcel.Application.Services;
using ParseTheParcel.Infrastructure.Providers;
using ParseTheParcel.Application.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trademe Parcel Shipping API",
        Version = "v1",
        Description = "API to calculate shipping cost based on package size and weight"
    });
});

// Configuration
builder.Configuration.AddJsonFile("shippingConfig.json", optional: false, reloadOnChange: true);
builder.Services.Configure<List<ParcelRule>>(builder.Configuration.GetSection("ParcelRules"));

// Services
builder.Services.AddSingleton<IParcelRuleProvider, ConfigParcelRuleProvider>();
builder.Services.AddScoped<IParcelRuleEvaluator, ParcelRuleEvaluator>();
builder.Services.AddScoped<IParcelService, ParcelService>();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapPost("/api/v1/parcel/calculate", (ParcelRequest request, IParcelService parcelService, HttpContext context) =>
{
    var (type, cost, message) = parcelService.GetParcelTypeAndCost(request);
    if (type == null)
    {
        return Results.BadRequest(new ErrorResponse
        {
            Message = message ?? string.Empty,
            ErrorCode = "RULE_EVALUATION_FAILED",
            TraceId = context.TraceIdentifier
        });
    }

    var response = new ParcelResponse
    {
        ParcelType = type,
        Cost = cost,
        Message = message
    };
    return Results.Ok(response);
})
.WithName("CalculateParcel")
.WithOpenApi();

app.Run();
