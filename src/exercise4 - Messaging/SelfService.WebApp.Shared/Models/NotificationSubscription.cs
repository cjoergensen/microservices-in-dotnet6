namespace SelfService.WebApp.Shared.Models;

public class NotificationSubscription
{
    public string Name { get; set; }
    public bool Active { get; set; }

    public NotificationSubscription(string name, bool active)
    {
        this.Name = name;
        this.Active = active;
    }
}
