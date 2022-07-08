using AcmePowerSolutions.MeterDataManagement.Api.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AcmePowerSolutions.MeterDataManagement.Api.Controllers.v1_0;

[ApiController]
[Route("[controller]")]
public class ConsumptionController : Controller
{
    private readonly IConsumptionQueries consumptionQueries;

    public ConsumptionController(IConsumptionQueries consumptionQueries)
    {
        this.consumptionQueries = consumptionQueries;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int customerId, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
    {
        var viewModel = await consumptionQueries.GetConsumptionInPeriode(customerId, from, to);
        return new ObjectResult(viewModel);
    }
}