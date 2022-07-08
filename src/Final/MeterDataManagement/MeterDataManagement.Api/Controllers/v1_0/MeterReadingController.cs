using AcmePowerSolutions.MeterDataManagement.Api.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AcmePowerSolutions.MeterDataManagement.Api.Controllers.v1_0;

[ApiController]
[Route("[controller]")]
public class MeterReadingController : Controller
{
    private readonly IConsumptionQueries consumptionQueries;

    public MeterReadingController(IConsumptionQueries consumptionQueries)
    {
        this.consumptionQueries = consumptionQueries;
    }

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> Get(int id, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
    {
        var viewModel = await consumptionQueries.GetConsumptionInPeriode(id, from, to);
        return new ObjectResult(viewModel);
    }
}