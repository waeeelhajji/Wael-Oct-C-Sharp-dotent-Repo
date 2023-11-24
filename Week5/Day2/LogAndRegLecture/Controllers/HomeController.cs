using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LogAndRegLecture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LogAndRegLecture.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpPost("user/register")]
    public IActionResult Regiter(User newUser)
    {
        if (ModelState.IsValid)
        {
            // We pass the validation
            // need to check if the email is unique
            if (_context.Users.Any(a => a.Email == newUser.Email))
            {
                // we have a problem . this user already has this email in database 
                ModelState.AddModelError("Email", "Email is already in use !");
                return View("Index");
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpPost("user/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (ModelState.IsValid)
        {
            // step 1 : find their email and if we can't find it throw an error
            User userInDB = _context.Users.FirstOrDefault(a => a.Email == loginUser.LogEmail);
            if (userInDB == null)
            {
                // there was no Email in the database
                ModelState.AddModelError("LogEmail", "Invalid Login attempt");
                return View("Index");
            }
            PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
            var result = Hasher.VerifyHashedPassword(loginUser, userInDB.Password, loginUser.LogPassword);
            // aaaaa ===> 1 
            // aaaaa
            if (result == 0)
            {
                // this is a problem , we did not put in the database
                ModelState.AddModelError("LogEmail", "Invalid Login attempt");
                return View("Index");
            }
            else
            {
                return RedirectToAction("Success");
            }
        }
        else
        {
            return View("Index");
        }
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
