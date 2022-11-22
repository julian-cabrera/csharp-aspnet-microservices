using AspnetRunBasics.Extensions;
using AspnetRunBasics.Services;
using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.
	builder.Services.AddRazorPages();

	builder.Host.UseSerilog(SeriLogger.Configure);

	builder.Services.AddTransient<LoggingDelegatingHandler>();

	builder.Services.AddHttpClient<ICatalogService, CatalogService>(x =>
		x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
		.AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
		.AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());
	builder.Services.AddHttpClient<IBasketService, BasketService>(x =>
		x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
		.AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
		.AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());
    builder.Services.AddHttpClient<IOrderService, OrderService>(x =>
		x.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]))
		.AddHttpMessageHandler<LoggingDelegatingHandler>()
        .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
		.AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());

	builder.Services.AddHealthChecks()
					.AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:GatewayAddress"]}"), "Ocelot API Gateway", HealthStatus.Degraded);

    var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Error");
	}

	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthorization();

	app.MapRazorPages();
    app.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application ended with an unhandled exception");
}
finally
{
	Log.Information("Shut down complete");
	Log.CloseAndFlush();
}
