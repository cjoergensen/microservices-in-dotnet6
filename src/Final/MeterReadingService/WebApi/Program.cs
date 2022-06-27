using MeterReadingService.Data;
using MeterReadingService.WebApi.Services.v1_0;
using NServiceBus;
using Shared.Logging;
using Shared.Messaging;
using Shared.Telemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogging();
builder.Host.UseNServiceBus("MeterReadingService.v1_0");

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddSingleton<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddTelemetry("MeterReadingService.WebApi", "1.0.0");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGrpcService<PowerMeterReadingService>();
app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();