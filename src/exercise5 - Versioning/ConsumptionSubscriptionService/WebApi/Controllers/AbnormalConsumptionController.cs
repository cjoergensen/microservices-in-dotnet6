using ConsumptionNotificationSubscriptionService.Data;
using ConsumptionSubscriptionService.Contracts.Commands.v1_0;
using ConsumptionSubscriptionService.Contracts.Queries.v1_0;

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
    public IActionResult Subscribe([FromBody] SubscribeToAbnormalConsumptionNotifications command)
    {
        if (command == null)
            return new BadRequestResult();

        var subscription = repository.Get(command.CustomerId);
        if (subscription == null)
        {
            repository.Add(command.CustomerId, command.CommunicationChannel);
            return new CreatedResult("", null);
        }

        repository.Update(command.CustomerId, command.CommunicationChannel);
        return new NoContentResult();
    }

    [HttpDelete]
    [Route("{id?}")]
    public IActionResult Unsubscribe(int? id)
    {
        if (!id.HasValue)
            return new BadRequestResult();

        repository.Delete(id.Value);
        return new NoContentResult();
    }
}