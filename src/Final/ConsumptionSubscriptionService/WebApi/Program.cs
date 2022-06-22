using ConsumptionNotificationSubscriptionService.Data;
using NServiceBus;
using Serilog;
using Shared.Telemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Shared.Logging.LogFactory.BuildLogger());
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("ConsumptionNotificationSubscriptionService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});
// Add services to the container.
builder.Services.AddTelemetry("ConsumptionNotificationSubscriptionService", "1.0.0");
builder.Services.AddSingleton<IAbnormalConsumptionSubscriptionRepository, AbnormalConsumptionSubscriptionRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();

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

app.MapControllers();
app.UseCors("CorsPolicy");
app.UseMiddleware<Shared.Logging.ErrorHandlerMiddleware>();

app.Run();
