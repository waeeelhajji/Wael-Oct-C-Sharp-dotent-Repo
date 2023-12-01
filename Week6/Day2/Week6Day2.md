# Week6 Day 2

## Objectives

* Create a many to many relationship
* Pull data from a many to many relationship using .ThenInclude()

## Many to Many Relationships

A many to many relationship is one in which one item can have many connections to another table of items and vice versa. Some examples of many to many relationships include:

* Users and Games (1 user can own many games, and 1 game can be owned by many users)
* Actors and Movies (1 actor can act in many movies, and 1 movie has many actors in it)
* Store and Products (1 store sells many products, and 1 product can be sold at many stores)

### Creating our Many to Many models

The important thing to remember about creating a Many to Many over a One to Many is that when we make a Many to Many we create a third table -- the association table -- that will link up the two adjoining tables and hold all the different instances in which they have connected.

So when it's time to make our models we need to create:

```
    ModelA - The first object we're trying to create for our website
    ModelB - The second object we're trying to create for our website
    ModelC - The association model that links ModelA and ModelB
```

### Example project

ModelA, an Actor model:

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace YourProjectName.Models;

public class Actor
{
    [Key]
    public int ActorId {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}
```

ModelB, a Movie model:

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace YourProjectName.Models;

public class Movie
{
    [Key]
    public int MovieId {get;set;}
    public string Title {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}
```

ModelC, the Association model:

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace YourProjectName.Models;

public class Castlist
{
    [Key]
    public int CastlistId {get;set;}
    // The connection to the actor table
    public int ActorId {get;set;}
    public Actor? Actor {get;set;}
    // The connection to the movie table
    public int MovieId {get;set;}
    public Movie? Movie {get;set;}
}
```

Now that we have our association set up we can add these lines to our first models:

#### In the Actor model

```
public List<Castlist> MoviesActedIn {get;set;} = new List<Castlist>();
```

#### In the Movie model

```
public List<Castlist> ActorsInMovie {get;set;} = new List<Castlist>();
```

With that done don't forget to add these 3 models to your MyContext.cs and set up everything else needed for connecting to a database then you're ready to migrate!

### Adding Data

Adding data for Movies and Actors works the same way as it ever has. For a review on how to add simple data, refer to past lecture notes. Our focus today is on how to add an association row to our database. If you look at the Castlist model, you will see this requires us to pass in an ActorId and a MovieId so we know exactly which actor and which movie we are trying to connect.

Assuming we have Actors and Movies to call on now, we can make a form specifically for adding to our association model:

#### AddToCast.cshtml

```
<form action="/castlist/add" method="post">
    <div class="form-group">
        <label for="ActorId">Actor: </label>
        <select asp-for="ActorId" class="form-control">
            @foreach(Actor a in ViewBag.AllActors)
            {
                <option value="@a.ActorId">@a.FirstName @a.LastName</option>
            }
        </select>
    </div>
    <div class="form-group">
        <label for="MovieId">was in: </label>
        <select asp-for="MovieId" class="form-control">
            @foreach(Movie a in ViewBag.AllMovies)
            {
                <option value="@a.MovieId">@a.Title</option>
            }
        </select>
    </div>
    <input type="submit" value="Add role" class="btn btn-primary">
</form>
```

Note this form requires that on our controller side we have 2 ViewBags passing data about all Actors and all Movies to work.

Once this form posts, we have all the data we need and can add the association to the database the same way we would add any type of data:

```
[HttpPost("castlist/add")]
public IActionResult AddCast(Castlist newCasting)
{
    if(ModelState.IsValid)
    {
        _context.Add(newCasting);
        _context.SaveChanges();
        return RedirectToAction("AddToCast");
    } else {
        return View("AddToCast");
    }
}
```

Now if you test the form and check out your database you will start seeing associations being made!

### Rendering association data

On it's own the association model is just a model for holding IDs. There isn't much we can use it for on it's own, which is why most of our queries will come from Model A or B and will reach across the association model to grab what they need. This means we need to jump from ModelA to ModelC to ModelB in order for a full scope of data to be obtained.

Double jumping models means we need a new query term. We learned about how .Include allows us to model jump into a connected model, but in order for us to jump from one model into another and into another we need to Include...and THEN Include another model.

For example, if we were trying to pull data on all the movies that a certain actor has acted in, we would do something like this:

```
_context.Actors.Include(s => s.MoviesActedIn).ThenInclude(d => d.Movie).FirstOrDefault(f => f.ActorId == actId);
```

Or if we were trying to get the actors in all movies:

```
_context.Movies.Include(s => s.ActorsInMovie).ThenInclude(d => d.Actor).ToList();
```

Pay attention to how we had to jump from one model into the association model and then into the third model. Without this, we won't be able to get any information so we need to do it this way!

Finally, if we wanted to render the data we gathered we could do it like so:

#### AllMovies.cshtml

```
<h2>All Movies</h2>
@foreach(Movie a in ViewBag.AllMovies)
{
    <h3>@a.Title</h3>
    <ul>
        @foreach(Castlist c in a.ActorsInMovie)
        {
            <li>@c.Actor.FirstName @c.Actor.LastName</li>
        }
    </ul>
}
```

And we're done! As long as the data was pulled and passed along correctly you should now be seeing a list of all movies and the actors in each movie.

The important thing to remember here is that data comes in many forms. Depending on your query you may be getting back lists with lists inside of them or single objects with lists or lists with single items in them. Pay close attention to WHAT you are pulling before you try to render anything. Read your queries and become familiar with what to expect from each query and you will soon find it a breeze to pull and render data using Entity Framework Core!
