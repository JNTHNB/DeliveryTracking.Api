using DeliveryTracking.Api;
using DeliveryTracking.Application;
using DeliveryTracking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Seed Data (for convenience)
app.SeedData();

// API Endpoints
app.MapEndpoints();

app.Run();

public abstract partial class Program;