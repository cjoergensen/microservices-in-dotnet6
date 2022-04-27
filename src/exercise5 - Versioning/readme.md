# Exercise 5 - Versioning

## Namespace versioning of Contracts

In this exercise the _Contract_ projects uses various versioning strategies:

| Project  | Pattern  |
|---|---|
| **ConsumptionSubscriptionService.Contracts** | ConsumptionSubscriptionService.Contracts.v1_0.Commands <br />(Multiple versions in Assembly) |
| **CustomerProfile.Contracts** | CustomerProfileService.Contracts.v1_0.Commands <br />(One version per Assembly)  |
| **MeterReadingService.Contracts** | MeterReadingService.Contracts.Events.v1_0 |

## Versioning - Using URI

Right-Click the In the _ConsumptionSubscriptionService / WebApi_ project and select _Manage nuget packages_. Under the Browse tap search for _Microsoft.AspNetCore.Mvc.Versioning_ and install the latest version.

In the _Program.cs_ add this line:

```
builder.Services.AddApiVersioning();
```

Then updated the _Controllers / AbnormalConsumptionController.cs_ with these attributes:

```
[ApiController]
[Route("api/v{version:apiVersion}/[Controller]")]
[ApiVersion("1.0")]
```

Last step is to update the API Gateway to use the new versioning scheme.

Open the _SelfServiceWebApp / ApiGateWay / Program.cs_ file. And updated the uri for the _ConsumptionNotificationSubscriptionServiceClient_:

```
builder.Services.AddHttpClient<IConsumptionNotificationSubscriptionServiceClient, ConsumptionNotificationSubscriptionServiceClient>(client => client.BaseAddress = new Uri("https://localhost:8002/api/v1.0/"));
```

## Versioning - Using QueryString/

Right-Click the In the _CustomerProfileService / WebApi_ project and select _Manage nuget packages_. Under the Browse tap search for _Microsoft.AspNetCore.Mvc.Versioning_ and install the latest version.

In the _Program.cs_ add this line:

```
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});
```

And this using statement:

```
using Microsoft.AspNetCore.Mvc.Versioning;
```

Since we are using the ``AssumeDefaultVersionWhenUnspecified = true`` and setting the default version 1.0, no changes are required for our consumers.

For further details see (Api Versioning Wiki)[https://github.com/dotnet/aspnet-api-versioning/wiki]