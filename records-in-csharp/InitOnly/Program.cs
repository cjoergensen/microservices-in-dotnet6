var person = new Person() { Name = "John doe", Age = 35};
Console.WriteLine(person);

//person.Name = "Jane Doe";
Console.WriteLine(person);

public record Person
{
    public string Name { get; init; }
    public int Age { get; init; }
}
