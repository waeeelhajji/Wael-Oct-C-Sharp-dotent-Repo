// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// First instance of an animal

Animal Dog = new Animal("Dog", 60.2, "Black", true);
Animal Cat = new Animal("Cat", 2.2, "White", false);
Animal Bird = new Animal("Bird", 0.2, "Yellow", true);
Animal Bear = new Animal("Grizly", 200.20, "Black and White", true);
Animal Horse = new Animal("Horse", 800.5, "Brown", true);
Animal fish = new Animal(22.22, "Blue", true);

Console.WriteLine(fish.Skin);
Console.WriteLine(fish.Species);
Console.WriteLine("-------------------");

Console.WriteLine(Cat.Weight);
Console.WriteLine(Cat.Behaviour);
Console.WriteLine("-------------------");

Console.WriteLine(Bird.Skin);
Console.WriteLine(Bird.Species);
Console.WriteLine("-------------------");

Console.WriteLine(Bear.Behaviour);
Console.WriteLine(Bear.Species);
Console.WriteLine("-------------------");

Console.WriteLine(Horse.Skin);
Console.WriteLine(Horse.Weight);



Dog.makeNoise("Miaou");
Cat.makeNoise("Bark");

