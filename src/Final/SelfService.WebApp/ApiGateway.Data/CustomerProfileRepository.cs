using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using LiteDB;
using Microsoft.Extensions.Logging;
using SelfService.WebApp.Shared.Models;

namespace SelfService.WebApp.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    private const string DatabaseFile = "CustomerProfile-ViewModel.db";
    private const string CollectionName = "customerprofiles";
    private readonly ILogger<CustomerProfileRepository> logger;

    public CustomerProfileRepository(ILogger<CustomerProfileRepository> logger)
    {
        this.logger = logger;
    }

    public Profile Get(int customerId)
    {
        try
        {
            logger.LogDebug("Retrieving '{typeName}'. Customer Id = '{customerId}'", nameof(Profile), customerId);
            using var db = new LiteDatabase($"Filename={DatabaseFile};connection=shared");
            var profiles = db.GetCollection<Profile>(CollectionName);

            return Get(customerId, profiles);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrive '{typeName}'.", nameof(Profile));
            throw;
        }
    }

    public void UpdateName(int customerId, string name)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, Name = {name}", nameof(Profile), customerId, name);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<Profile>(CollectionName);

            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(Profile), customerId);
                return;
            }

            profile.Name = name;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(Profile), profile);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update 'typeName'.", nameof(Profile));
            throw;
        }
    }

    public void UpdateEmail(int customerId, string email)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, Email = {email}", nameof(Profile), customerId, email);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<Profile>(CollectionName);

            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(Profile), customerId);
                return;
            }

            profile.Email = email;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(Profile), profile);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update 'typeName'.", nameof(Profile));
            throw;
        }
    }

    public void UpdatePhoneNumber(int customerId, string phonenumber)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, Phone = {phone}", nameof(Profile), customerId, phonenumber);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<Profile>(CollectionName);

            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(Profile), customerId);
                return;
            }

            profile.PhoneNumber = phonenumber;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(Profile), profile);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update 'typeName'.", nameof(Profile));
            throw;
        }
    }

    public void UpdateCommunicationChannel(int customerId, CommunicationChannel communicationChannel)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, PreferedCommunicationChanne = {communicationChannel}", nameof(Profile), customerId, communicationChannel);

            using var db = new LiteDatabase(DatabaseFile);
            var profiles = db.GetCollection<Profile>(CollectionName);
            var profile = Get(customerId, profiles);
            if (profile == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Record with Customer Id = '{customerId}' was not found", nameof(Profile), customerId);
                return;
            }

            profile.PreferedCommunicationChannel = communicationChannel;
            profiles.Update(profile);
            logger.LogInformation("Updated '{typeName}': {customerProfile}.", nameof(Profile), profile);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update 'typeName'.", nameof(Profile));
            throw;
        }
    }

    private static Profile Get(int customerId, ILiteCollection<Profile> profiles)
    {
        profiles.EnsureIndex(profile => profile.CustomerId);
        return profiles.FindOne(profile => profile.CustomerId == customerId);
    }
}
