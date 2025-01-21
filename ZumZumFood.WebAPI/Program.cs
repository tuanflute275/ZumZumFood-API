using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

var builder         = WebApplication.CreateBuilder(args);
var services        = builder.Services;
var configuration   = builder.Configuration;

// Add Infrastructure
services.AddDerivativeTradeServices(configuration);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("YourServiceName")) // Replace with your service name
            .AddAspNetCoreInstrumentation() // Trace incoming HTTP requests
            .AddHttpClientInstrumentation() // Trace outgoing HTTP requests
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://otel-collector:4317"); // OpenTelemetry Collector endpoint
            });
    })
    .WithMetrics(metricsProviderBuilder =>
    {
        metricsProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("YourServiceName")) // Replace with your service name
            .AddAspNetCoreInstrumentation() // Collect ASP.NET Core metrics
            .AddRuntimeInstrumentation() // Collect .NET runtime metrics
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://otel-collector:4317"); // OpenTelemetry Collector endpoint
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapMetrics();
await app.AutoMigration();
app.UseRouting();
app.ConfigureMiddleware();
app.MapControllers();
// Cấu hình endpoint cho health check
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message,
                duration = e.Value.Duration.TotalSeconds
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.Run();