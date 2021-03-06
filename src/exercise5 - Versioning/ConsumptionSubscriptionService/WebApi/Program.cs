using ConsumptionNotificationSubscriptionService.Data;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("ConsumptionNotificationSubscriptionService");
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;

    // RabbitMQ: https://docs.particular.net/transports/rabbitmq/connection-settings
    //var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
    //transport.ConnectionString("My custom connection string");

    // Azure Service Bus: https://docs.particular.net/transports/azure-service-bus/
    //var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
    //transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");
});
// Add services to the container.
builder.Services.AddSingleton<IAbnormalConsumptionSubscriptionRepository, AbnormalConsumptionSubscriptionRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();
