using CustomerProfileService.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    private const string DatabaseFile = "CustomerProfileService.db";
    private const string CollectionName = "customerprofiles";
    private readonly ILogger<CustomerProfileRepository> logger;

    public CustomerProfileRepository(ILogger<CustomerProfileRepository> logger)
    {
        this.logger = logger;
    }

    public CustomerProfile Get(int customerId)
    {
        try
        {
            logger.LogDebug("Retrieving '{typeName}'. Customer Id = {customerId}", nameof(CustomerProfile), customerId);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<CustomerProfile>(CollectionName);
            return Get(customerId, profiles);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrive '{typeName}'.", nameof(CustomerProfile));
            throw;
        }
    }

    public void Update(int customerId, string name, string phone, string email)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, Name = {name}, Phone = {phone}, Email = {email}", nameof(CustomerProfile), customerId, name, phone, email);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<CustomerProfile>(CollectionName);

            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(CustomerProfile), customerId);
                return;
            }

            profile.Name = name;
            profile.Email = email;
            profile.PhoneNumber = phone;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(CustomerProfile), profile);

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