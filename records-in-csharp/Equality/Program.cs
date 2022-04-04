var person1 = new Person("John Doe", 45);
var person2 = new Person("John Doe", 45);

var samePerson = person1.Equals(person2);
Console.WriteLine($"Are this the same person? {samePerson}");

var pet1 = new Pet("Max", 5);
var pet2 = new Pet("Max", 5);

var samePet = pet1.Equals(pet2);
Console.WriteLine($"Are this the same pet? {samePet}");


public record Person(string Name, int Age);

public class Pet 
{
    public Pet(string Name, int Age)
    {
        this.Name = Name;
        this.Age = Age;
    }

    public string Name { get; set; }
    public int Age { get; set; }
}