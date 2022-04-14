using CustomerProfileService.Data;
using Microsoft.AspNetCore.Mvc;


namespace CustomerProfileService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly ICustomerProfileRepository repository;
    private readonly ILogger<ProfileController> logger;

    public ProfileController(ICustomerProfileRepository repository, ILogger<ProfileController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Index([FromRoute]int? id)
    {
        if (id is null)
            return new BadRequestResult();

        var customerProfile = repository.Get(id.Value);
        return new ObjectResult(new GetCustomerProfileResponse(customerProfile.CustomerId, customerProfile.Name, customerProfile.DateOfBirth));
    }
}
