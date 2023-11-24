# Week 5 Day 2

## Objectives

* Create a simple login and registration website

## Setup

Setting up our website requires we follow the steps from yesterday to establish our connection to the database. To get started, we will build out the model for our User.

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Models;

public class User
{
    [Key]
    public int UserId {get;set;}
    
    [Required]
    public string Username {get;set;}
    
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password {get;set;}
    
    // Anything directly under the notMapped will not go in to the database
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PassConfirm {get;set;}
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}
```

Make sure to add your User model to your MyConetext file to ensure a User table is properly created.

## Register

Complete the steps to connect to the database and then it's time to set up the registration form:

```
@model User
<form action="/user/register" method="post">
    <div class="form-group">
        <label for="Username">Username</label>
        <input asp-for="Username" class="form-control">
        <span asp-validation-for="Username" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label for="Email">Email</label>
        <input asp-for="Email" class="form-control">
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label for="Password">Password</label>
        <input asp-for="Password" class="form-control">
        <span asp-validation-for="Password" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label for="PassConfirm">Confirm Password</label>
        <input asp-for="PassConfirm" class="form-control">
        <span asp-validation-for="PassConfirm" class="text-danger"></span>
    </div>
    <div class="form-group">
        <input type="submit" value="Register" class="btn btn-primary">
    </div>
</form>
```

And the corresponding post route:

```
[HttpPost("user/register")]
public IActionResult Register(User newUser)
{
    if(ModelState.IsValid)
    {
        // We passed validations
        return RedirectToAction("Success");
    } else {
        return View("Index");
    }
}
```

Create a quick Success page as well to get a visual confirmation your form worked. Now that the basic setup is done we can add some extra validations.

### Check for a unique email

We need to make sure the email the user gave us is unique before adding it to the database. Otherwise we run the risk of having issues pulling our data correctly later. We rely on the assumption that every email is unique in order to validate user login.

After we checked that our model is valid in our controller, we can add this check for the email:

```
// This statement will return true if an instance of the new user's email is found in the database and false if it is not
if(_context.Users.Any(a => a.Email == newUser.Email))
{
    // We have a problem. This email is already in the database
    ModelState.AddModelError("Email", "Email is already in use!");
    return View("Index");
}
```

### Hash the password

Most important of all is that we are hashing the password _before_ it goes into our database. In the case of a data breach we wouldn't want a hacker able to see the plain text of everyone's passwords!

To use a built-in password hasher, we will bring this using statement to the top of our controller:

```
using Microsoft.AspNetCore.Identity;
```

Now we have the PasswordHasher, a really neat tool that hashes our passwords. We use it like so after we have validated that the email is unique:

```
PasswordHasher<User> Hasher = new PasswordHasher<User>();
newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
```

Now with that done, we can safely add the information to our database and save our changes. We are now done with registering!

## Login

Login is going to require a few modified steps from our register. We can make use of most of the register form and just remove the parts we don't need like so:

```
<form action="/user/login" method="post">
    <div class="form-group">
        <label for="Email">Email</label>
        <input asp-for="Email" class="form-control">
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label for="Password">Password</label>
        <input asp-for="Password" class="form-control">
        <span asp-validation-for="Password" class="text-danger"></span>
    </div>
    <div class="form-group">
        <input type="submit" value="Login" class="btn btn-primary">
    </div>
</form>
```

And create an accompanying post request like so:

```
[HttpPost("user/login")]
public IActionResult Login(LogUser loginUser)
{
    if(ModelState.IsValid)
    {
        return RedirectToAction("Success");
    } else {
        return View("Index");
    }
}
```

If your login and registration forms are on the same page and you tried to test your login now, you would get an issue where all the validations for register would also trigger and login doesn't work no matter what you put in the form. This is because the two forms are still using the same model. And while this model works for registering, it does not work for logging in. So we need to make some changes.

First, we'll make a new model specifically for the login user (don't worry this doesn't need to be added to your MyContext file. This will not be saved in the database, it's just for our usage.)

```
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Models;

public class LogUser
{
    [EmailAddress]
    [Required]
    public string LogEmail {get;set;}
    
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string LogPassword {get;set;}
}
```

It will be helpful to change the names to prevent confusion between models as well. Make sure to update your form accordingly to match the new variable names.

Now to officially break the models apart, we need to bring the login form into a partial so we can use @model LogUser on it. Making a partial is as easy as making a new file in your Views > Home folder and calling it _NameOfChoice.cshtml (the_ is common practice when naming a partial. See the _Layout file for an example!)

Cut and paste your form from your Index page here and add the necessary model to the top. Now add the partial into your Index.cshtml in the place you want like so:

```
<partial name="_Login"/>
```

Now that that problem is resolved, we can get to the proper login validations.

### Checking the email

Phase one of successfully logging in is to check to see if the email provided by the user is an existing email in our database. First we have to search for the email, and then depending on if we found it or not give the proper response:

```
User userInDb = _context.Users.FirstOrDefault(a => a.Email == loginUser.LogEmail);
if(userInDb == null)
{
    // There was no email in the database so throw an error
    ModelState.AddModelError("LogEmail", "Invalid Login Attempt");
    return View("Index");
}
```

And that's it!

### Checking the password

Checking the password requires a little extra work. We need to bring our password hasher back in to do the comparisons for us. That will return us a response that will alert us if the password given does not match the password on record. It will look something like this:

```
PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();

var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);

if(result == 0)
{
    // This is a problem, we did not put in the correct password
    ModelState.AddModelError("LogEmail", "Invalid Login Attempt");
    return View("Index");
} else {
    return RedirectToAction("Success");
}
```

With that done if you test the page now everything should work! You have a functioning login and registration page!

### Adding Session

Now that we can successfully login and register, the final step is to remember our logged in user with session. Adding Session to our project is as easy as following the setup instructions and then placing the session set up where needed.

#### In Register post request

```
_context.Add(newUser);
_context.SaveChanges();
HttpContext.Session.SetInt32("user", newUser.UserId);
return RedirectToAction("Success");
```

#### In Login post request

```
if(result == 0)
{
    // This is a problem, we did not put in the correct password
    ModelState.AddModelError("LogEmail", "Invalid Login Attempt");
    return View("Index");
} else {
    HttpContext.Session.SetInt32("user", userInDb.UserId);
    return RedirectToAction("Success");
}
```

Then on your Success page you can use session to grab the user:

```
User loggedInUser = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("user"));
return View(loggedInUser);
```

And you're done! From here you can add extra features like verifying if there is a session before allowing users to access the success page and clearing session upon logout.
