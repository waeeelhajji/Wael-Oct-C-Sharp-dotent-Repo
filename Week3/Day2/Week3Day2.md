# Week 3 Day 2

## Objectives
* Make our first MVC project
* Add models to our project
* Add validations to our models

## Making an MVC project

Now that we've seen a bare bones MVC project, it's time to look at the full version. Go to your console and run this line:

```
    dotnet new mvc --no-https -o FirstMVC
```

Once the project is created, check out what you have. It's way more than what we had with a `dotnet new web`! In particular, you will notice there are now folders for Controllers, Views, and Models. We no longer have to create them ourselves! You will also notice that we already have bootstrap in our project for all your styling needs, but we can also use our own css as well.

### Neat new features
#### Layout.cs
The layout file acts as a frame that lays over all of our files. HTML and css that are applied to this file are applied to *all* your cshtml pages. This makes it super easy to import your css and to add overarching styles like a navbar across all your webpages in a snap! 

### Modifying for our project
Having said that we have everything we need to make an MVC project, it's also true that we have a little too much now. That layout file from before contains lots of boilderplate code we likely don't need for our project, so you can delete and edit that code as necessary. I often delete all mentions of the privacy page unless I need it or want to turn it into a different page. However this is all down to preference and what you're going for with your project so add and remove whatever you need!

## Adding Models
Now that we have our basic setup ready to go, it's time to build our first model.

In your model folder create your first model file named YourFileName.cs and set up the basic elements of your file and the attributes of the model:

```
    namespace YourProjectName.Models;
    #pragma warning disable CS8618
    public class User
    {
        public string Name {get;set;}
        public string FavColor {get;set;}
        public string FavNumber {get;set;}
    }
```

#### Why the {get;set;}?
Adding a get and set to our fields turn them into auto-implemented properties. This allows other areas of our code to have the control to read and write these attributes as needed. We will need this since going forward.

### What's with #pragma warning disable CS8618?
Dotnet 6 has a new feature that tries to warn us when we have models that could potentially take in null values. There are a few ways around this warning message but a quick way is just to disable the warning. Going forward we will be building out our models in such a way that we won't run in to the problem of a value taking in null so we can do this. 

## Form setup

From here, setting up our form is going to be very similar to how we did it in the past. However, now that we are making full use of MVC and ASP.Net Core, there are some extra features we can now take advantage of.

In your form on your cshtml, you are now able to use something called asp-for in your tags. This takes the place of the name attribute and the input type attribute so you no longer need to include them. 

```
    <form action="postForm" method="post">
        <div class="form-group">
            <label>Name</label>
            <input asp-for="Name" class="form-control">
        </div>
        <div class="form-group">
            <label>Favorite color</label>
            <input asp-for="FavColor" class="form-control">
        </div>
        <div class="form-group">
            <label>Favorite Number</label>
            <input asp-for="FavNumber" class="form-control">
        </div>
        <div class="form-group">
            <input type="submit" value="Submit" class="btn btn-primary">
        </div>
    </form>
```

And over in your controller for your post request you will find that you can pass a whole object along now instead of each piece individually, which saves us on writing code.

```
    [HttpPost("postForm")]
    public IActionResult postForm(User myUser)
    {
        Console.WriteLine($"Name: {myUser.Name}");
        Console.WriteLine($"Fav Color: {myUser.FavColor}");
        Console.WriteLine($"Fav Number: {myUser.FavNumber}");
        return RedirectToAction("Index");
    }
```

We still have the same issue from before of not actually having a way to save our data, but that's okay for now. What's important is understanding how we can get data to the backend.

Now that we're all set with our model, we can take advantage of one more useful feature: validations.

## Adding Validations
Validations are what ensure that we don't send data into our database that doesn't meet our specifications. Thankfully, dotnet has made setting up validations extremely easy!

In our model we'll add the following using statement to bring in our validations:
```
    using System.ComponentModel.DataAnnotations;
```

The ComponentModel.DataAnnotations library comes with a variety of built in validations. For the most part we will use their built-ins, however we can create our own custom validations if needed. 

```
    namespace YourProjectName.Models;
    #pragma warning disable CS8618
    public class User
    {
        [Required(ErrorMessage="Name is required!")]
        public string Name {get;set;}
        
        [Required(ErrorMessage="Favorite color is required")]
        [MinLength(3, ErrorMessage="Color name must be at least 3 characters in length")]
        public string FavColor {get;set;}
        
        [Required(ErrorMessage="Favorite number is required")]
        [Range(-2000,2000)]
        public string FavNumber {get;set;}
    }
```

These are just some of the options available to us. Look up what else we can do with data annotations!

The next step is to update our controller to handle these validations. We need to check if our form passed validations and react accordingly.

```
    [HttpPost("postForm")]
    public IActionResult postForm(User myUser)
    {
        // If we successfully pass validations, we enter the if
        if(ModelState.IsValid)
        {
            // Where we redirect to Index and see our results as before
            Console.WriteLine($"Name: {myUser.Name}");
            Console.WriteLine($"Fav Color: {myUser.FavColor}");
            Console.WriteLine($"Fav Number: {myUser.FavNumber}");
            return RedirectToAction("Index");
        } else {
            // Otherwise we do not pass validations and should stay where we were
            return View("Index");
        }
        
    }
```

If you test it now by trying to fail validations, you will find that when you hit submit nothing happens and nothing is logged to the console. That is because we failed validations. The final step is to render the errors so our users know what about the form they got wrong.

Adding validation error messages is as easy as, well, telling them to render. ASP handles knowing that there are errors and provides us with a simple way to render them using span tags.

```
    <form action="postForm" method="post">
        <div class="form-group">
            <label>Name</label>
            <input asp-for="Name" class="form-control">
            <span asp-validation-for="Name"></span>
        </div>
        <div class="form-group">
            <label>Favorite color</label>
            <input asp-for="FavColor" class="form-control">
            <span asp-validation-for="FavColor"></span>
        </div>
        <div class="form-group">
            <label>Favorite Number</label>
            <input asp-for="FavNumber" class="form-control">
            <span asp-validation-for="FavNumber"></span>
        </div>
        <div class="form-group">
            <input type="submit" value="Submit" class="btn btn-primary">
        </div>
    </form>
```

Now when you test it the validation messages display on the screen! That's it! We're done! You now have a working form with validation errors. Try correctly filling out the form now and see it working as intended.

## Debugging
#### An expression tree may not contain a dynamic operation
If you get this error it means you need to add @model YourModel to the top of your cshtml page. Remember that asp-net works with models and it needs to know which model it's working with!