# Week 5 Day1

## Objectives

* Establish a connection to our MySQL Database
* Learn the basic CRUD commands

## Steps to connect to a database

Setting up our connection to MySQL is a minorly tedious task but once it is done it does not need to be touched again.

### Step 1: Install packages

We need these packages to start using MySQL and Entity Framework Core. Pop these into your terminal after you have made and cd-ed into your project to get them downloaded.

```
    dotnet add package Pomelo.EntityFrameworkCore.MySql --version 6.0.1
    dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.3
```

### Step 2: Make your model

We can't add anything to the database if it doesn't have a model to work off of. This step runs just the same as always except now we reference a [Key] and ID in order to match up with what MySQL expect.

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace YourProjectName.Models;
public class Item
{
    [Key]
    public int ItemId { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
```

*Important: you MUST name your key NameOfClassId otherwise you may run in to errors down the line!*

### Step 3: Set up your MyContext file

MyContext is what tells our program what tables we want in our database. The standard naming convention is to take the plural version of your model. (Since this is a place where you will store many of these items.)

Create a MyContext.cs file in your Models folder and add the following code (edit it to match your needs):

```
#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace YourProjectName.Models;
public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<Item> Items { get; set; } 
}
```

### Step 4: Setting up our connection string

We need a connection string to grant us access to our MySQL database on our local machine. Which means we need to give it the user credentials it needs to log in.
*Note: If you did not set root as your password into MySQL then be very careful about sharing your project otherwise you risk exposing personal data to other people!*

The best place to store this connection is in the already created file appsettings.json. This is an ideal file as it can be gitignored without affecting the performance of our project and protects your private information as a bonus!

When all is said and done, your file will look something like this. Make sure to update the connection string with your proper login credentials and to change the name of the database to something relevant to your project. If you forget to do this you may merge two projects into the same database and make a big mess!

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
    "ConnectionStrings":
    {
        "DefaultConnection": "Server=localhost;port=3306;userid=root;password=root;database=mydbnamedb;"
    }
}
```

### Step 5: Setting up our connection in Program.cs

Now that everything is in place, it's time to hook things up in your Program.cs.

```
// Additional using statements around here
using Microsoft.EntityFrameworkCore;
using ProjectName.Models;

// This should look familiar
var builder = WebApplication.CreateBuilder(args)

//  Creates the db connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Adds database connection - must be before app.Build();
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
```

Now that that is done, there is one more task to do within our code.

## Step 6: Bringing our database to our controller for use

The final step requires us to establish a connection in our controller that will allow us to grab anything we need from the database.

Your code will look something like this:

```
using System.Diagnostics
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;
namespace YourProjectName.Controllers;
    
public class HomeController : Controller
{
    private MyContext _context;
    // This is already in your controller
    private readonly ILogger<HomeController> _logger;
     
    // here we can "inject" our context service into the constructor
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        // This becomes the name we use to reference our database
        _context = context;
    }
     
    [HttpGet"")]
    public IActionResult Index()
    {
        // Note this will not work yet until we migrate our tables and have actual data in the database
        List<Item> AllItems = _context.Items.OrderBy(a => a.Name).ToList();
            
        return View();
    }
 }
```

## Step 7: Migrating our tables

The final step is to migrate over to our MySQL database so we can create the tables we need and finally start storing data in them.

First you must stage the changes:

```
    dotnet ef migrations add YourMigrationName
```

Finally we push the changes to our database:

```
    dotnet ef database update
```

And now if we go to SQL Workbench we should see our tables in the database we created! We're done!

## CRUD commands

It's not enough to just have a connection to a database however. We also need to be able to Create, Read, Update, and Delete data from our databases. These are the basic CRUD commands we can expect to do in many websites.

### Creating Data

Creating data will require we have a form to fill out, just as we practiced before. Only now when we send data to our post request we can actually do something with that data. Namely, save it to our database like so:

```
[HttpPost("item/add")]
public IActionResult AddItem(Item newItem)
{
    // We add this to the database so long as it's correct
    if(ModelState.IsValid)
    {
        // We can add to the database
        _context.Add(newItem);
        _context.SaveChanges();
        return RedirectToAction("Index");
    } else {
        return View("Index");
    }
}
```

### Reading Data

We spent some time yesterday learning how to grab data using LINQ queries. Now the only addition we can make is how we reference the database we're calling on. First we need _context to hook up to the database, then we need the name of the table (as defined in MyContext) we're trying to access, like so:

```
    Item oneItem = _context.Items.FirstOrDefault(a => a.ItemId == ItemId);
```

Beyond that, refer to what you learned about LINQ to find out what other queries you can do.

### Updating Data

Updating data requires that we have data to overwrite old data. It also requires that we know what piece of data we're overwriting. All this information typically comes in from a form we created that passes in the id of the item we're trying to update.

```
[HttpPost("item/update/{itemId}")]
public IActionResult UpdateItem(int itemId, Item newVersionOfItem)
{
    Item oldItem = _context.Items.FirstOrDefault(a => a.ItemId == itemId);
    // This is NOT a valid way to update!
    // oldItem = newVersionOfItem;
    // This IS a valid way
    oldItem.Name = newVersionOfItem.Name;
    oldItem.Description = newVersionOfItem.Description;
    oldItem.UpdatedAt = DateTime.Now;
    _context.SaveChanges();
    return RedirectToAction("Index");
}
```

### Deleting Data

Deleting data is similar to updating in that we need to know what we're affecting. But then we don't have to worry about overwriting data because we're getting rid of all of it! Deleting is as easy as:

```
[HttpGet("item/delete/{itemId}")]
public IActionResult DeleteItem(int itemId)
{
    // Find the item
    Item itemToDelete = _context.Items.SingleOrDefault(a => a.ItemId == itemId);
    // Delete the item
    _context.Items.Remove(itemToDelete);
    // Save changes
    _context.SaveChanges();
    // Send yourself somewhere
    return RedirectToAction("Index");
}
```

And that's it! Once you feel comfortable with the 4 basic CRUD commands your ability to manipulate a website is now vastly improved!
