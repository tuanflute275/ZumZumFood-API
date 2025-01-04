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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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