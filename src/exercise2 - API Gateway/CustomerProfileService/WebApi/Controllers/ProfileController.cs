using CustomerProfileService.Contracts.Commands;
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
}