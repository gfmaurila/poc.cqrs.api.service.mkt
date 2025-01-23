using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkt.Api.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Config.ConfigCommandValidator(builder.Services);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
Config.ConfigDbContext(builder.Services, builder.Configuration);
Config.ConfigRepository(builder.Services);
Config.ConfigService(builder.Services);
Config.ConfigCommand(builder.Services);
Config.ConfigRequestHandler(builder.Services);
builder.Services.AddHttpClient();
Config.ConfigBusService(builder.Services);
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// app.ApplyMigrations();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();