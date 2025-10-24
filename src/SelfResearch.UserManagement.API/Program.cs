using SelfResearch.UserManagement.API.Core.Data;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SelfResearch.UserManagement.API.Mapping;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Azure;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using SelfResearch.Core.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<MappingProfiles>();
});

builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();

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
