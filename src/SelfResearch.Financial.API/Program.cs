using System.Text.Json.Serialization;
using SelfResearch.Financial.API.Core.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SelfResearch.Financial.API.Feature.Wallet;
using SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;
using Microsoft.Extensions.Azure;
using SelfResearch.Financial.API.Feature.Propagate;
using SelfResearch.Financial.API.Feature.Wallet.CreateWalet;
using SelfResearch.Core.Infraestructure;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true) // only for local development proposes
    .AddEnvironmentVariables();


builder.Services.ConfigureHttpJsonOptions(options => 
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.PropertyNamingPolicy = null;
});

// Add Cors policy to reach backend from Vue site.
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnablesFrontend", builder =>
    {
        builder.SetIsOriginAllowed(origin => true) // Allow any origin in development
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<WalletMappingProfiles>();
});

builder.Services.AddScoped<IRetrieveWalletService, RetrieveWalletService>();
builder.Services.AddScoped<IRetrieveWalletRepository, RetrieveWalletRepository>();
builder.Services.AddScoped<IPropagatedEntityService<PropagatedUser>, PropagatedUserService>();
builder.Services.AddScoped<IPropagatedEntityRepository<PropagatedUser>, PropagatedUserRepository>();
builder.Services.AddScoped<ICreateWalletService, CreateWalletService>();
builder.Services.AddScoped<ICreateWalletRepository, CreateWalletRepository>();

var azureServiceBusConnectionString = builder.Configuration.GetSection("Azure:ServiceBus:ServiceBusConnectionString").Value ?? throw new InvalidOperationException("ServiceBusConnectionString is not configured.");

builder.Host.UseNServiceBus(context =>
{
    var serviceBusEndpoint = builder.Configuration.GetSection("Azure:ServiceBus:NServiceBusEndpointName").Value ?? throw new InvalidOperationException("NServiceBusEndpointName is not configured.");
    var serviceBusKey = azureServiceBusConnectionString;

    var endpointConfiguration = new EndpointConfiguration(serviceBusEndpoint);

    var transport = endpointConfiguration.UseTransport(new AzureServiceBusTransport(serviceBusKey, TopicTopology.Default));

    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

builder.Services.AddAzureClients(builder =>
{
    builder.AddServiceBusClient(azureServiceBusConnectionString);
});

FluentResultConfiguration.AddCustomErrorHandling();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // CORS middleware to allow Vue frontend to access the API in development in development environment
    app.UseCors("EnablesFrontend");

    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
