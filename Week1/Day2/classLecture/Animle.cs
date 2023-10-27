
class Animal
{
    // Establish the attributes of the class
    // Words you used to describe an item 
    public string Species;
    public double Weight;
    public string Skin;
    public bool Behaviour;

    // this is our constructor 
    public Animal(string species, double weight, string skin, bool behaviour)
    {
        Species = species;
        Weight = weight;
        Skin = skin;
        Behaviour = behaviour;
    }

    // Seconde constructor that takes only 2 attributes and auto fills the first one
    public Animal(double weight, string skin, bool behaviour)
    {
        Species = "Dog";
        Weight = weight;
        Skin = skin;
        Behaviour = behaviour;
    }

    // Methods (function) these are the things an animal can do


    public void makeNoise(string sound)
    {
        Console.WriteLine($"the {Species} made the sound {sound}");
    }

}