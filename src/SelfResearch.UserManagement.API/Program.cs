using SelfResearch.UserManagement.API.Core.Data;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SelfResearch.UserManagement.API.Mapping;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
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
