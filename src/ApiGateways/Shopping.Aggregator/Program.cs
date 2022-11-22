using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Serilog;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ICatalogService, CatalogService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
    .AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
    .AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(x =>
    x.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
    .AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
                .AddUrlGroup(new Uri($"{builder.Configuration["ApiSetings:CatalogUrl"]}/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{builder.Configuration["ApiSetings:BasketUrl"]}/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{builder.Configuration["ApiSetings:OrderingUrl"]}/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
