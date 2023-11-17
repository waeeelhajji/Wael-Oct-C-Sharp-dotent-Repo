# Week 4 Day 2

## Objectives

* Use LINQ queries to pull data from data sets

## Using LINQ

LINQ is a library of methods that allows us to pull information from a data set. This will be particularly useful as we start integrating databases into our projects so we can grab anything we need from our databases.

All we need to get started with LINQ is a set of data to draw from. This can be from a database or it can be something you made on your own.

```
    public static Game[] AllGames = new Game[] {
        new Game {Title="Elden Ring", Price=59.99, Genre="Action RPG", Rating="M", Platform="PC"},
        new Game {Title="League of Legends", Price=0.00, Genre="MOBA", Rating="T", Platform="PC"},
        new Game {Title="World of Warcraft", Price=39.99, Genre="MMORPG", Rating="T", Platform="PC"},
        new Game {Title="Elder Scrolls Online", Price=14.99, Genre="Action RPG", Rating="M", Platform="PC"},
        new Game {Title="Smite", Price=0.00, Genre="MOBA", Rating="T", Platform="All"},
        new Game {Title="Overwatch", Price=39.00, Genre="First-person Shooter", Rating="T", Platform="PC"},
        new Game {Title="Scarlet Nexus", Price=59.99, Genre="Action JRPG", Rating="T", Platform="All"},
        new Game {Title="Wonderlands", Price=59.99, Genre="RPG FPS", Rating="M", Platform="All"},
        new Game {Title="Rocket League", Price=0.00, Genre="Sports", Rating="E", Platform="All"},
        new Game {Title="StarCraft", Price=0.00, Genre="RTS", Rating="T", Platform="PC"},
        new Game {Title="God of War", Price=29.99, Genre="Action-adventure ", Rating="M", Platform="PC"},
        new Game {Title="Red Dead Redemption", Price=40.00, Genre="Action adventure", Rating="M", Platform="All"},
        new Game {Title="My Little Pony A Maretime Bay Adventure", Price=39.99, Genre="Adventure", Rating="E",Platform="All"},
        new Game {Title="Fallout New Vegas", Price=10.00, Genre="Open World RPG", Rating="M", Platform="PC"}
    };
```

Now that we have a set of data to pull from, it's time to write some LINQ queries. The most important thing to notice as you write these queries is there there is a pattern to how the queries look. Understand the pattern and you will be able to pull data from anything.

Below are just a few examples of some common LINQ queries you may perform.

#### Note: We add .ToList() to the end of each query that will return a list to allow us to use the datatype List<Game> at the front

### Getting all data

```
    List<Game> allGamesFromData = AllGames.ToList();
```

### Organizing data alphabetically by title

```
    List<Game> allGamesFromData = AllGames.OrderBy(s => s.Title).ToList();
```

### Getting all data that matches a certain criteria

```
    List<Game> allPlatforms = AllGames.Where(f => f.Platform == "All").ToList();
```

### Getting all data that matches 2 criteria (you can substitute || where needed)

```
    List<Game> allPlatforms = AllGames.Where(f => f.Platform == "All" && f.Rating == "T").ToList();
```

### Finding the first instance of matching data

```
    Game singleGame = AllGames.FirstOrDefault(d => d.Title == "Rocket League");
```

### Taking a select number of items

```
    List<Game> topMGames = AllGames.Where(a => a.Rating == "M").Take(3).ToList();
```

Noticed the patterns yet?

Now that we have our data, sending it to our front end to be rendered is as easy as putting the data into a ViewBag or ViewModel and telling it to render! Going forward we will start working with data from actual databases, but today your focus should be on familiarizing yourself with how LINQ queries look and some of the common ones you will use.
