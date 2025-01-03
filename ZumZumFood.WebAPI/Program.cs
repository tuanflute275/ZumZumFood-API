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
app.Run();