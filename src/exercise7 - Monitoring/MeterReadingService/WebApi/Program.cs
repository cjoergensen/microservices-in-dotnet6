using MeterReadingService.Data;
using MeterReadingService.WebApi.Services;
using NServiceBus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Shared.Logging.LogFactory.BuildLogger());

builder.Host.UseNServiceBus(context => 
{
    var endpointConfiguration = new EndpointConfiguration("MeterReadingService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});


// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddSingleton<IMeterReadingRepository, MeterReadingRepository>();

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

// Configure the HTTP request pipeline.
app.MapGrpcService<PowerMeterReadingService>();
app.MapControllers();
app.UseCors("CorsPolicy");



app.Run();
