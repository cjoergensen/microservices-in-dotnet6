using CustomerProfileService.Contracts.v1_0.Events;
using NServiceBus;

namespace SelfService.WebApp.ApiGateway.Services;

public class CustomerProfileService : 
    IHandleMessages<CustomerNameUpdated>, 
    IHandleMessages<CustomerEmailUpdated>, 
    IHandleMessages<CustomerPhoneNumberUpdated>,
    IHandleMessages<PreferedCommunicationChannelChanged>
{
    private readonly SelfService.WebApp.Data.ICustomerProfileRepository repository;

    public CustomerProfileService(SelfService.WebApp.Data.ICustomerProfileRepository repository)
    {
        this.repository = repository;
    }

    public Task Handle(CustomerNameUpdated message, IMessageHandlerContext context)
    {
        repository.UpdateName(message.CustomerId, message.NewName);
        return Task.CompletedTask;
    }

    public Task Handle(PreferedCommunicationChannelChanged message, IMessageHandlerContext context)
    {
        repository.UpdateCommunicationChannel(message.CustomerId, message.NewCommunicationChannel);
        return Task.CompletedTask;
    }

    public Task Handle(CustomerPhoneNumberUpdated message, IMessageHandlerContext context)
    {
        repository.UpdatePhoneNumber(message.CustomerId, message.NewPhoneNumber);
        return Task.CompletedTask;
    }

    public Task Handle(CustomerEmailUpdated message, IMessageHandlerContext context)
    {
        repository.UpdateEmail(message.CustomerId, message.NewEmail);
        return Task.CompletedTask;
    }
}
