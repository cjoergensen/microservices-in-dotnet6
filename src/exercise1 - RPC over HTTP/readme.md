
# Exercise 1 - RPC over HTTP

## Introduction to the solution

The solution contains two services and the web frontend:

### SelfService.WebApp.Client
Blazor WebAssembly app with a simple UI. The UI contains two pages to edit the Profile and Notifications.

### CustomerProfileService

This service is responsible for managing the Customers Profile information. Data is store in a LiteDB database and it exposes a REST Api for clients to retrieve and update data.

The _CustomerProfileService_ consists of these projects:

| Project  | Description  |
|---|---|
| **Contracts** | The Commands, Event and Queries integration DTO's. These are the messages that the service publishes og consumes |
| **Data** | Data Repositories and entities used internaly by the service  |
| **Models** | Internal models |
| **WebApi** | The WebApi that the service exposes to its clients  |

### ConsumptionNotificationSubscriptionService

The _ConsumptionNotificationSubscriptionService_ uses the same structure as descriped in [CustomerProfileService](#CustomerProfileService) and is responsible for managing the notifications that customer wants to recieve and the communication channel to use.



## Step 1 - Retrieve the Customer Profile
The first objective is to be able to get the Customer Profile information from the  _CustomerProfileService_ and use display it in the _SelfService.WebApp.Client_

In the _CustomerProfileService_/_WebApi_ add a new class _ProfileController.cs_ to the _Controllers_ folder.

Make sure that the controller inherits from the ControllerBase class and is decorated with ApiController and Route attributes:

```
namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
}
```

In order to access the profile data we need a reference to the _CustomerProfileRepository_ from the Data project. Lets inject this via the constructor like this:

```
using CustomerProfileService.Data;
namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ICustomerProfileRepository repository;

    public ProfileController(ICustomerProfileRepository repository)
    {
        this.repository = repository;
    }
}
```

The actual implementation of _ICustomerProfileRepository_ registered in the startup.cs in the _WebApi_ project.

Let's go ahead and implement the HTTP Get method for retrieving the Profile:

```
[HttpGet]
[Route("{id?}")]
public IActionResult Get([FromRoute] int? id)
{
    if (!id.HasValue)
        return new BadRequestResult();

    var customerProfile = repository.Get(id.Value);
    if (customerProfile == null)
        return new NotFoundResult();

    return new ObjectResult(new GetCustomerProfileResponse(customerProfile.CustomerId, customerProfile.Name, customerProfile.PhoneNumber, customerProfile.Email));
}
```

Build the solution and start the _CustomerProfileService/WebApi_ project. Open a browser, navigate to ``https://localhost:8001/Profile/1`` and you see a response simular to this:

```
{"customerId":1,"name":"Jane Doe","phoneNumber":"12345678","email":"john@test.com"}
```

## Step 2 - Consume the CustomerProfile WebAPI

With the CustomerProfile WebAPI up and running it's time to consume it from the WebApp.

Open the _SelfService.WebApp / Client / ApiClients / CustomerProfileServiceClient.cs_

Replace the mock implementation in the _GetProfile_ method with this:

```
public async Task<Profile> GetProfile(int profileId)
{
    var httpResponse = await httpClient.GetAsync($"profile/{profileId}");
    httpResponse.EnsureSuccessStatusCode();

    var content = await httpResponse.Content.ReadAsStringAsync();
    if (string.IsNullOrWhiteSpace(content))
        throw new InvalidOperationException("Unable to load 'Profile'");

    var profile = JsonSerializer.Deserialize<Profile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    if (profile is null)
        throw new InvalidOperationException("Unable to load 'Profile'");

    return profile;
}
```

Build the solution. Right-Click the Solution in VS and select Properties. Select the 'Multiple startup projects' option and set _SelfService.WebApp.Client_ and _WebApi (CustomerProfileService\WebApi)_ as startup projects. Press F5 to start the project.

Navigate to the 'Profile' page and you should now see that the data is retrieved via the REST api.

## Step 3 - Update the Customer Profile

Open the _CustomerProfileService / WebApi / Controllers / ProfileController.cs _ and add this method:

```
[HttpPut]
public IActionResult Update([FromBody] UpdateProfile? updateCommand)
{
    if (updateCommand == null)
        return new BadRequestResult();

    var customerProfile = repository.Get(updateCommand.CustomerId);
    if (customerProfile == null)
        return new NotFoundResult();

    repository.Update(updateCommand.CustomerId, updateCommand.Name, updateCommand.PhoneNumber, updateCommand.Email);

    return new NoContentResult();
}
```

And add this using statement: 
```
using CustomerProfileService.Contracts.Commands;
```

Then open the _SelfService.WebApp / Client / ApiClients / CustomerProfileServiceClient.cs_ and updated the _UpdateProfile_ method with this:

```
public async Task UpdateProfile(Profile profile)
{
    var updateCommand = new UpdateProfile(profile.CustomerId, profile.Name, profile.PhoneNumber, profile.Email);
    var content = new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json");
    var httpResponse = await httpClient.PutAsync($"profile", content);
    httpResponse.EnsureSuccessStatusCode();
}
```

Press F5 to run the application. Open the Profile page, edit the data and press update. Restart the application and verify that the data has been updated.

## Notification Settings

In the _CustomerProfileService_/_WebApi_ add a new class NotificationSettingsController.cs_ to the _Controllers_ folder.

Replace the file with this:

```
using CustomerProfileService.Contracts.Commands;
using CustomerProfileService.Data;

namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationSettingsController : ControllerBase
{
    private readonly INotificationSettingsRepository notificationSettingsRepository;

    public NotificationSettingsController(INotificationSettingsRepository notificationSettingsRepository)
    {
        this.notificationSettingsRepository = notificationSettingsRepository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Get([FromRoute] int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var settings = notificationSettingsRepository.Get(id.Value);
        if(settings == null)
            return new ObjectResult(new GetNotificationSettingsResponse(id.Value, ConsumptionNotificationSubscriptionService.Contracts.CommunicationChannel.Email));

        return new ObjectResult(new GetNotificationSettingsResponse(settings.CustomerId, settings.PreferedCommunicationChannel));
    }

    [HttpPut]
    public IActionResult Update([FromBody] UpdateNotificationSettings updateCommand)
    {
        if (updateCommand == null)
            return new BadRequestResult();

        var settings = notificationSettingsRepository.Get(updateCommand.CustomerId);
        if (settings == null)
            return new NotFoundResult();

        notificationSettingsRepository.Update(updateCommand.CustomerId, updateCommand.PreferedCommunicationChannel);
        return new NoContentResult();
    }
}
```
Next open the _SelfService.WebApp / Client / ApiClients / CustomerProfileServiceClient.cs_ and replace the _GetNotificationSettings_ and _UpdateNotificationSettings_ with this:

```
public async Task<NotificationSettings> GetNotificationSettings(int profileId)
{
    var httpResponse = await httpClient.GetAsync($"notificationsettings/{profileId}");
    httpResponse.EnsureSuccessStatusCode();

    if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        return new NotificationSettings(profileId, ConsumptionNotificationSubscriptionService.Contracts.CommunicationChannel.Email);

    var content = await httpResponse.Content.ReadAsStringAsync();
    if (string.IsNullOrWhiteSpace(content))
        throw new InvalidOperationException("Unable to load 'Notification Settings'");

    var notificationSettings = JsonSerializer.Deserialize<NotificationSettings>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    if (notificationSettings is null)
        throw new InvalidOperationException("Unable to load 'Notification Settings'");

    return notificationSettings;
}

public async Task UpdateNotificationSettings(NotificationSettings notificationSettings)
{
    var updateCommand = new UpdateNotificationSettings(notificationSettings.CustomerId, notificationSettings.PreferedCommunicationChannel);
    var content = new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json");
    var httpResponse = await httpClient.PutAsync($"notificationsettings", content);
    httpResponse.EnsureSuccessStatusCode();
}
```

Press F5 to run the application. Open the Profile page, edit the 'Prefered Communication Channel 'and press update. Restart the application and verify that the data has been updated.

This concludes the CustomerProfileService.WebApi.

## ConsumptionSubscriptionService Web API

In the _ConsumptionNotificationSubscriptionService_/_WebApi_ add a new class AbnormalConsumptionController.cs_ to the _Controllers_ folder.

Replace the code with this:

```
using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Data;

namespace ConsumptionNotificationSubscriptionService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AbnormalConsumptionController : ControllerBase
{
    private readonly IAbnormalConsumptionSubscriptionRepository repository;

    public AbnormalConsumptionController(IAbnormalConsumptionSubscriptionRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Get(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var subscription = repository.Get(id.Value);
        if (subscription == null)
            return new NotFoundResult();

        return new ObjectResult(new GetAbnormalConsumptionSubscriptionResponse(subscription.CustomerId, subscription.CommunicationChannel, subscription.CreatedOn));
    }

    [HttpPut]
    [Route("{id?}")]
    public IActionResult Subscribe(int? id, [FromBody] CommunicationChannel communicationChannel)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var subscription = repository.Get(id.Value);
        if (subscription == null)
        {
            repository.Add(id!.Value, communicationChannel);
            return new CreatedResult("", null);
        }

        repository.Update(id!.Value, communicationChannel);
        return new NoContentResult();
    }

    [HttpDelete]
    [Route("{id?}")]
    public IActionResult Unsubscribe(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        repository.Delete(id!.Value);
        return new NoContentResult();
    }
}
```

Note how this controller use the HttpDelete and HttpPut verbs to implement the Subscribe/Unsubscribe feature.


Open the _SelfService.WebApp / Client / ApiClients / NotificationSubscriptionServiceClient.cs_ and update it with this:

```
using ConsumptionNotificationSubscriptionService.Contracts;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.ApiClients;

public class NotificationSubscriptionServiceClient
{
    private readonly HttpClient httpClient;

    public NotificationSubscriptionServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dictionary<string, bool>> GetSubscriptions(int profileId)
    {
        var result = new Dictionary<string, bool>
        {
            ["abnormalconsumption"] = false
        };

        var httpResponse = await httpClient.GetAsync($"AbnormalConsumption/{profileId}");
        if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            result["abnormalconsumption"] = true;

        return result;
    }

    public async Task SubcribeToNotification(int profileId, string subscriptionName, CommunicationChannel communicationChannel)
    {
        var content = new StringContent(JsonSerializer.Serialize(communicationChannel), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"{subscriptionName}/{profileId}", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task UnsubscribeFromNotification(int profileId, string subscriptionName)
    {
        var httpResponse = await httpClient.DeleteAsync($"{subscriptionName}/{profileId}");
        httpResponse.EnsureSuccessStatusCode();
    }
}
```

Build the solution. Right-Click the Solution in VS and select Properties. Select the 'Multiple startup projects' option and add  _WebApi (ConsumptionNotificationSubscriptionService\WebApi)_ to the list of startup projects. Press F5 to start the project.

Navigate to the Notifications page and flip the 'Consumption alerts' switch and confirm that the change is persisted. test