using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Contracts.v1_0.Events;
using CustomerProfileService.Data;
using NServiceBus;

namespace CustomerProfileService.WebApi.Services;

public class CustomerProfileService : IHandleMessages<UpdateProfile>
{
    private readonly ICustomerProfileRepository repository;

    public CustomerProfileService(ICustomerProfileRepository repository)
    {
        this.repository = repository;
    }

    public async Task Handle(UpdateProfile updateCommand, IMessageHandlerContext context)
    {
        var customerProfile = repository.Get(updateCommand.CustomerId);
        if (customerProfile == null)
            return;

        await repository.Update(updateCommand.CustomerId, updateCommand.Name, updateCommand.PhoneNumber, updateCommand.Email);

        if (customerProfile.Name != updateCommand.Name)
            await  context.Publish(new CustomerNameUpdated(updateCommand.CustomerId, updateCommand.Name, DateTimeOffset.Now));

        if (customerProfile.Email != updateCommand.Email)
            await context.Publish(new CustomerEmailUpdated(updateCommand.CustomerId, updateCommand.Email, DateTimeOffset.Now));

        if (customerProfile.PhoneNumber != updateCommand.PhoneNumber)
            await context.Publish(new CustomerPhoneNumberUpdated(updateCommand.CustomerId, updateCommand.PhoneNumber, DateTimeOffset.Now));
    }
}