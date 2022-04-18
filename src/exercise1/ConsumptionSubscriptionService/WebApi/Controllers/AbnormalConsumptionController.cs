using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Data;

namespace ConsumptionSubscriptionService.Controllers;

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