# Day 2

## Objectives
* Create our first classes in C#
* Use inheritance to make creating related classes even easier

## Classes

### Creating a class
Make a file in your project called NameOfClass.cs

The start of your file will look something like this:
```
class NameOfClass
{
    // Your code here
}
```

### Fields
All classes are comprised of fields (also known as attributes) that make up the "what" of your object. These are the elements you would use to describe the thing. For example, if we were making a class of drinks, we may describe a drink by its name, color, whether it's carbonated, its calorie count, etc...

Add all your fields into your class

```
    public string name;
    public int calories;
    public bool isCarbonated;
```

Remember to include public at the start so that you can access these variables in other files (like your program.cs)

### Constructors

After establishing your fields, it's time to make the constructor. You only need one constructor, but you have the option to create other constructors based on your needs. In general, a constructor will contain an input for all fields that could vary when you create an instance of that class.

```
    public Drink(string n, bool isC, int cal)
    {
        name = n;
        isCarbonated = isC;
        calories = c;
    }
```

You do *not* need to add every field you created to the constructor, but if you planned to use default values for any fields make sure to include those in the constructor.

This is where having multiple constructors could come in handy.

```
    public Drink(string n, int cal)
    {
        name = n;
        isCarbonated = true;
        calories = c;
    }
```

### Methods

Methods allow us to create the functionality of a class. These are the things an instance can "do" when it is created. Examples include the ability to add sugar to a drink and increase the calorie count, to make a character lose health, or reset a game to its default state.

Methods are written after your fields and constructors.

```
    public void addSugar(int amount)
    {
        this.calories+=amount;
    }
    
    public bool checkIfFlat()
    {
        return this.isCarbonated;
    }
```

Set these methods to public if you plan to call them in another file. Some methods exist only to be used within the class though so keep that in mind when making your methods. 

### Creating an Instance

In your program.cs (or wherever you're making your instance) you can now call on your class like so:

```
    // Creates a new drink instance of coffee
    Drink d1 = new Drink("Coffee", false, 180);
    
    // Prints out the name of the drink
    Console.WriteLine(d1.name);
    
    // This would add 100 calories to your drink
    d1.addSugar(100);
    
    Console.WriteLine(d1.calories); // this should now be 100 calories higher
```

## Debugging

### [item] is inaccessible due to its protection level
You need to make the variable/class you're working with public

## Inheritance

In order for us to use inheritance, we first need a base class from where we are going to inherit. 

```
class Animal 
{
    public string Species;
    public double Weight;
    public string Diet;
    public bool isFriendly;

    public Animal(string species, double weight, string diet, bool isFr)
    {
        Species = species;
        Weight = weight;
        Diet = diet;
        isFriendly = isFr;
    }
}
```

Once that is established, setting up inheritance is as easy as: 
```
    public class Mammal : Animal
```

Once that is done, we can start using the base class to build out the constructor for our child class.

```
    public Mammal(string species, double weight, string diet, bool isFriendly) : base(species, weight, diet, isFriendly) {}
```

Base refers to the base constructor from our parent class. We need a constructor in our parent class for us to create a "base" on which to build our new characters. 

Now in Program.cs, making a new child instance is as easy as:

```
    Mammal lion = new Mammal("Lion", 120.5, "Carnivore", false);
```

If we want to create something more diverse where we can add in our own data to the fields you will need to build out a constructor in the parent class that can fit what you need.