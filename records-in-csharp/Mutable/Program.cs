var person = new Person();
person.Name = "John doe";
person.Age = 55;
Console.WriteLine(person);

person.Name = "Jane Doe";
Console.WriteLine(person);

public record Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}