using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Data;
using NServiceBus;

namespace CustomerProfileService.WebApi.Services
{
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
        }
    }
}
