using Polly;
using Polly.Extensions.Http;
using SelfService.WebApp.ApiGateway.ApiClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ICustomerProfileServiceClient, CustomerProfileServiceClient>(client =>
    client.BaseAddress = new Uri("https://localhost:8001"))
    .AddPolicyHandler((services, request) => GetRetryPolicy<MeterReadingServiceClient>(services))
    .AddPolicyHandler(GetCircuitBreakerPolicy<MeterReadingServiceClient>());



builder.Services.AddHttpClient<IConsumptionNotificationSubscriptionServiceClient, ConsumptionNotificationSubscriptionServiceClient>(client =>
    client.BaseAddress = new Uri("https://localhost:8002/api/v1.0/"))
    .AddPolicyHandler((services, request) => GetRetryPolicy<MeterReadingServiceClient>(services))
    .AddPolicyHandler(GetCircuitBreakerPolicy<MeterReadingServiceClient>());



builder.Services.AddHttpClient<IMeterReadingServiceClient, MeterReadingServiceClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:8003");
        client.DefaultRequestVersion = new Version(2, 0);
    })
    .AddPolicyHandler((services, request) => GetRetryPolicy<MeterReadingServiceClient>(services))
    .AddPolicyHandler(GetCircuitBreakerPolicy<MeterReadingServiceClient>());
    

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


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(IServiceProvider services)
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            3, 
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)), 
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                var logger = services.GetService<ILogger<T>>();
                if(logger != null)
                    logger.LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
            });
}


static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy<T>()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(10),
            onBreak: (outcome, timespan) =>
            {
                Console.WriteLine("Circuit Breaker tripped and is temporarily disallowing requests");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit Breaker closed and is allowing requests");
            },
            onHalfOpen: () =>
            {
                Console.WriteLine("Circuit Breaker is half-opened and will test the service with the next request");
            });
}