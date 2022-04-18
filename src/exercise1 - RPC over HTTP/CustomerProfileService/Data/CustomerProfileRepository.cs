using CustomerProfileService.Models;
using LiteDB;

namespace CustomerProfileService.Data;

public class CustomerProfileRepository : ICustomerProfileRepository
{
    private const string DatabaseFile = "CustomerProfileService.db";
    private const string CollectionName = "customerprofiles";

    public CustomerProfile Get(int customerId)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var profiles = db.GetCollection<CustomerProfile>(CollectionName);
        return Get(customerId, profiles);
    }

    public void Update(int customerId, string name, string phone, string email)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var profiles = db.GetCollection<CustomerProfile>(CollectionName);

        var profile = Get(customerId, profiles);
        if (profile == null)
            return;

        profile.Name = name;
        profile.Email = email;
        profile.PhoneNumber = phone;
        profiles.Update(profile);
    }

    private static CustomerProfile Get(int customerId, ILiteCollection<CustomerProfile> profiles)
    {
        profiles.EnsureIndex(subscription => subscription.CustomerId);
        return profiles.FindOne(subscription => subscription.CustomerId == customerId);
    }
}