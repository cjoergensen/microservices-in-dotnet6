var person = new Person("John Doe", 45);

string name;
int age;

(name, age) = person;

Console.WriteLine($"{name} - {age}");

public record Person(string Name, int Age);