using CustomerProfileService.Contracts.v1_0.Events;
using CustomerProfileService.Models;
using LiteDB;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    private const string DatabaseFile = "CustomerProfileService.db";
    private const string CollectionName = "customerprofiles";
    private readonly ILogger<CustomerProfileRepository> logger;
    private readonly IMessageSession messageSession;

    public CustomerProfileRepository(ILogger<CustomerProfileRepository> logger, IMessageSession messageSession)
    {
        this.logger = logger;
        this.messageSession = messageSession;
    }

    public CustomerProfile Get(int customerId)
    {
        try
        {
            logger.LogDebug("Retrieving '{typeName}'. Customer Id = {customerId}", nameof(CustomerProfile), customerId);

            using var db = new LiteDatabase($"Filename={DatabaseFile};connection=shared");
            var profiles = db.GetCollection<CustomerProfile>(CollectionName);
            return Get(customerId, profiles);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrive '{typeName}'.", nameof(CustomerProfile));
            throw;
        }
    }

    public async Task Update(int customerId, string name, string phone, string email)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, Name = {name}, Phone = {phone}, Email = {email}", nameof(CustomerProfile), customerId, name, phone, email);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<CustomerProfile>(CollectionName);
            bool nameUpdated, emailUpdated, phonenumberUpdated;
            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(CustomerProfile), customerId);
                return;
            }

            nameUpdated = profile.Name != name;
            profile.Name = name;

            emailUpdated = profile.Email!= email;
            profile.Email = email;

            phonenumberUpdated = profile.PhoneNumber != phone;
            profile.PhoneNumber = phone;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(CustomerProfile), profile);


            var publishTasks = new List<Task>();
            if (nameUpdated)
                publishTasks.Add(messageSession.Publish(new CustomerNameUpdated(customerId, profile.Name, DateTimeOffset.Now)));

            if (emailUpdated)
                publishTasks.Add(messageSession.Publish(new CustomerEmailUpdated(customerId, profile.Email, DateTimeOffset.Now)));

            if (phonenumberUpdated)
                publishTasks.Add(messageSession.Publish(new CustomerPhoneNumberUpdated(customerId, profile.PhoneNumber, DateTimeOffset.Now)));

            await Task.WhenAll(publishTasks);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update 'typeName'.", nameof(CustomerProfile));
            throw;
        }
    }

    private static CustomerProfile Get(int customerId, ILiteCollection<CustomerProfile> profiles)
    {
        profiles.EnsureIndex(subscription => subscription.CustomerId);
        return profiles.FindOne(subscription => subscription.CustomerId == customerId);
    }
}