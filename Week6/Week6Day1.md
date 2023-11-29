# Week 6 Day 1

## One to Many Relationships

One to Many Relationships are one of the common types of relationships between data we encounter in a relational database. They connect tables wherein items from one table have connections to many items from another table, but that other table's items can only ever connect to one item from the adjoining table. Some real life examples of these types of relationships include users and messages (1 user can send many messages, but 1 message can only be written by 1 user) and artists and songs (1 artist can write many songs, but 1 song can only be written by 1 artist.)

Establishing a one to many relationship between two models is a straightforward process. Just like in MySQL where you have to define a foreign key in the many table of the one (ex: a message would have a foreign key connecting it to one user) we must establish the same relationship here, but with a couple additions.

### On the one side

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace yourProjectName.Models;

public class Owner
{
    [Key]
    public int OwnerId {get;set;}
    [Required]
    public string Name {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public List<Pet> PetsOwned {get;set;} = new List<Pet>();
}

```

### On the many side

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace yourProjectName.Models;

public class Pet
{
    [Key]
    public int PetId {get;set;}
    [Required]
    public string Name {get;set;}
    [Required]
    public string Species {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    [Required]
    public int OwnerId {get;set;}
    // Navigation property that lets us see the whole owner
    // Make sure to put Owner? or you won't pass validations!
    public Owner? Owner {get;set;}
}
```

Once these models are set up, we can make sure MyContext is updated with the models and migrate our database. You will notice in the database now that we have an id on the many table from the one table, but no reference to the navigation properties. We will be using these properties soon to query our data.

### Adding a many item

Adding a one item like a user or an owner is the same steps as before. But now when we go to add an item that belongs to the many table we need to consider this: what instances of the 1 table do we have available? There needs to be instances in order to create instances for the many table. Think of it this way: a comment can't exist without a user who wrote it, therefore, we need to make some users before we can make some comments so that each comment has an author.

Our database is expecting back an id from the adjoining 1 table, so an easy way to pass this in is through our form. Depending on how the relationship is established this might be done another way, but here is one method to make the connection:

```
    In your controller, so you can loop through all owners: 
    ViewBag.AllOwners = _context.Owners.ToList();
    
    In your cshtml:
    <form action="/pet/add" method="post">
        <div class="form-group">
            <label for="Name">Name</label>
            <input asp-for="Name" class="form-control">
            <span asp-validation-for="Name" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label for="Species">Species</label>
            <input asp-for="Species" class="form-control">
            <span asp-validation-for="Species" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label for="OwnerId">Owner</label>
            <select asp-for="OwnerId" class="form-control">
                <option value=null>--Select--</option>
                @foreach(Owner o in ViewBag.AllOwners)
                {
                    // The value must be the ID or this won't work!
                    <option value="@o.OwnerId">@o.Name</option>
                }
            </select>
            <span asp-validation-for="OwnerId" class="text-danger"></span>
        </div>
        <div class="form-group">
            <input type="submit" value="Add Pet" class="btn btn-danger">
        </div>
    </form>
```

After this, adding the item to our database is the same steps as before. Post the data, validate it with ModelState, and then add it to our database. We're done! Check your MySQL workbench to see now that the data is there. If it is not for some reason, go back through your models and forms and ensure everything is set up correctly.

### Pulling data from across tables with Include

Now that we have our relationships established and some data in our database, we're able to start pulling data from across tables. This allows us for example to get all the messages a user wrote, or who is the owner of a certain pet, and so on. Doing so requires us to use that navigation property from before and a new query called Include.

Here are a couple examples of Include at work:
This example allows us to access the data about the one person who owns a particular pet

```
Query:
ViewBag.AllPets = _context.Pets.Include(a => a.Owner).ToList();

Front end:
<ul class="col-4">
    @foreach(Pet p in ViewBag.AllPets)
    {
        <li>@p.Name the @p.Species is owned by: @p.Owner.Name</li>
    }
</ul>
```

This example allows us to access the data on the list of pets that a person owns

```
Query:
ViewBag.AllOwners = _context.Owners.Include(a => a.PetsOwned).ToList();

Front end:
<ul>
    @foreach(Owner o in ViewBag.AllOwners)
    {
        <li class="text-start">@o.Name</li>
        <ul>
            @foreach(Pet p in o.PetsOwned)
            {
                <li class="text-start">@p.Name the @p.Species</li>
            }
        </ul>
    }
</ul>
```

How exactly we get data is all dependent on what we're trying to get. And sometimes doing a query is easier going one way than another. Play around with Include a while until it makes sense.

*Important:* Remember that a navigation property is just a placeholder that points us toward data. Without a .Include it is just an empty variable! If you ever run into an error saying the navigation property is not defined, you probably forgot to include the data in your initial query!
