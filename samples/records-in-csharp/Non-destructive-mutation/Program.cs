var person1 = new Person { Name = "John Doe", Age = 45 };
Console.WriteLine(person1);

var person2 = person1 with
{
    Name = "Jane Doe"
};
Console.WriteLine(person2);

public record Person
{
    public string Name { get; init; }
    public int Age { get; init; }
}