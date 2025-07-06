using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParseTheParcel.Application.Interfaces;
using ParseTheParcel.Application.Services;
using ParseTheParcel.Infrastructure.Providers;
using ParseTheParcel.Application.Models;
using ParseTheParcel.Application.DTOs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("shippingConfig.json", optional: false, reloadOnChange: true);
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();
    })
    .ConfigureServices((context, services) =>
    {
        // Bind List<ParcelRule> from config
        services.Configure<List<ParcelRule>>(context.Configuration.GetSection("ParcelRules"));

        // Register services
        services.AddSingleton<IParcelRuleProvider, ConfigParcelRuleProvider>();
        services.AddScoped<IParcelRuleEvaluator, ParcelRuleEvaluator>();
        services.AddScoped<IParcelService, ParcelService>();
    })
    .Build();

// Resolve the service and run logic
using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IParcelService>();

Console.WriteLine("Enter Length (mm):");
if (!long.TryParse(Console.ReadLine(), out var length))
{
    Console.WriteLine("Invalid input for length.");
    return;
}

Console.WriteLine("Enter Breadth (mm):");
if (!long.TryParse(Console.ReadLine(), out var breadth))
{
    Console.WriteLine("Invalid input for breadth.");
    return;
}

Console.WriteLine("Enter Height (mm):");
if (!long.TryParse(Console.ReadLine(), out var height))
{
    Console.WriteLine("Invalid input for height.");
    return;
}

Console.WriteLine("Enter Weight (kg):");
if (!double.TryParse(Console.ReadLine(), out var weight))
{
    Console.WriteLine("Invalid input for weight.");
    return;
}

var request = new ParcelRequest
{
    Length = length,
    Breadth = breadth,
    Height = height,
    Weight = weight
};

var (type, cost, message) = service.GetParcelTypeAndCost(request);

Console.WriteLine("\n=== Evaluation Result ===\n");

Console.WriteLine(message);
Console.ReadLine();
