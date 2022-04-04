var person = new Person("John Doe", 45);
Console.WriteLine(person);

//person.Name = "Jane Doe";
person = new Person("Jane Doe", 40);
Console.WriteLine(person);

public record Person(string Name, int Age);