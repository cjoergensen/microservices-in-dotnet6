# Exercise 6 - Robustness and logging

In this exercise we will implement Robustness patterns and logging.

For logging we will use SeriLog ASP.Net. The log configuration has been consolidated in the _Shared / Logging_ project.

For simplicity of this exercise we will log to the console. Serilog provides sinks for writing log events to storage in various formats: https://github.com/serilog/serilog/wiki/Provided-Sinks

Logging has been implemented in the _Data_ projects of each service. However there is no logging in the WebApi's.

## Step 1 -  Logging as Middleware

Implemented logging in each of the methods in WebApi's are very tedious. Therefore we will be implementing this a [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0) component.

In the _Shared / Logging_ project add a new class and name it _ErrorHandlerMiddleware.cs_.

Add this code:

```
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared.Logging;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlerMiddleware> logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in path '{requestPath}'. Request = '{request}'", context.Request.Path.Value, context.Request);
            throw;
        }
    }
}
```

Then in each of the WebApi's add this to the _Program.cs_ just above the ``app.Run();``

```
app.UseMiddleware<Shared.Logging.ErrorHandlerMiddleware>();
```

## Step 2 - Robustness pattern with Polly

Open the _MeterReadingService / WebApi / MeterReadingController.cs_ and inspect the _Get_ method. You will notice that we have added some flickering simulate an unstable service.

Lets use the Retry and Circuit Breaker patterns to handle this in our ApiGateway.

Right-Click the  _SelfServiceWebApp / ApiGateway_ > _Manage Nuget Packages_ 

Right-Click the In the _SelfServiceWebApp / ApiGateway_ project and select _Manage nuget packages_. Under the Browse tap search for _Microsoft.Extensions.Http.Polly_ and install the latest version.

Then open the _SelfServiceWebApp / ApiGateway / Program.cs_ and these methods:

```
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
```
Also add these using statements:

```
using Polly;
using Polly.Extensions.Http;
```

Next we can apply the policies on each of the HttpClient used in the Gateway:

```
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
```

Press F5 and navigate to the Consumption page. Monitor the _ApiGateway_ console window and notice how the instability is handled. When the circuit breaker trips, wait and then try reloading the page.