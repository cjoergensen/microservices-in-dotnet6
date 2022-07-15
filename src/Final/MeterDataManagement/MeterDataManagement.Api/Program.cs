using Shared.Logging;
using Shared.Telemetry;
using AcmePowerSolutions.MeterDataManagement.Api.Grpc.v1_0;
using Marten;
using AcmePowerSolutions.MeterDataManagement.Api.Model;
using AcmePowerSolutions.MeterDataManagement.Api.Infrastructure.Repositories;
using Weasel.Core;
using AcmePowerSolutions.MeterDataManagement.Api.Queries;


const string ServiceName = "AcmePowerSolutions.MeterReadingService.WebApi";

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogging(ServiceName);
//builder.Host.UseNServiceBus("MeterReadingService.WebApi");

// Add services to the container.
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("marten"));
    options.AutoCreateSchemaObjects = AutoCreate.All;
});
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddTelemetry(ServiceName, "1.0.0");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMeterReadingRepository, MartenMeterReadingRepository>();
builder.Services.AddScoped<IConsumptionQueries, ConsumptionQueries>();
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