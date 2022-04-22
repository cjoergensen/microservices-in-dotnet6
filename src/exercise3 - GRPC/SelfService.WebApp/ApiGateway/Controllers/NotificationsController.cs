using Microsoft.AspNetCore.Mvc;
using SelfService.WebApp.ApiGateway.ApiClients;

namespace SelfService.WebApp.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : Controller
{
    private readonly IConsumptionNotificationSubscriptionServiceClient consumptionNotificationSubscriptionServiceClient;
    private readonly ICustomerProfileServiceClient customerProfileServiceClient;

    public NotificationsController(IConsumptionNotificationSubscriptionServiceClient consumptionNotificationSubscriptionServiceClient, ICustomerProfileServiceClient customerProfileServiceClient)
    {
        this.consumptionNotificationSubscriptionServiceClient = consumptionNotificationSubscriptionServiceClient;
        this.customerProfileServiceClient = customerProfileServiceClient;
    }

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> Index(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        var result = new Dictionary<string, bool>
        {
            ["powerconsumption.abnormalconsumption"] = await consumptionNotificationSubscriptionServiceClient.GetAbnormalConsumptionSubscriptionStatus(id.Value),
            ["powerconsumption.outage"] = false,
            ["billing.newinvoice"] = false,
            ["billing.paymentoverdue"] = false
        };

        return new OkObjectResult(result);
    }

    [HttpPut]
    [Route("{id?}/abnormalconsumption")]
    public async Task CreateAbnormalConsumptionNotification(int id)
    {
        var profile = await customerProfileServiceClient.GetProfile(id);
        var communicationChannel = profile.PreferedCommunicationChannel;

        await consumptionNotificationSubscriptionServiceClient.CreateAbnormalConsumptionSubscription(id, communicationChannel);
    }

    [HttpDelete]
    [Route("{id?}/abnormalconsumption")]
    public async Task DeleteAbnormalConsumptionNotification(int id)
    {
        await consumptionNotificationSubscriptionServiceClient.DeleteAbnormalConsumptionSubscription(id);
    }

    //public async Task CreateSubscription(int profileId, string subscriptionName, CommunicationChannel communicationChannel)
    //{
    //    var content = new StringContent(JsonSerializer.Serialize(communicationChannel), Encoding.UTF8, "application/json");
    //    var httpResponse = await httpClient.PutAsync($"{subscriptionName}/{profileId}", content);
    //    httpResponse.EnsureSuccessStatusCode();
    //}

    //public async Task DeleteSubscription(int profileId, string subscriptionName)
    //{
    //    var httpResponse = await httpClient.DeleteAsync($"{subscriptionName}/{profileId}");
    //    httpResponse.EnsureSuccessStatusCode();
    //}
}
