namespace CustomerProfileService.Models;

public class CustomerProfile
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
}