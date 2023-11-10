using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstMVC.Models;

namespace FirstMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost("process")]
    public IActionResult process(User newUser)
    {
        // Console.WriteLine($"Name:  {Name}");
        // Console.WriteLine($"FavColor:  {FavColor}");
        // Console.WriteLine($"FavNumber:  {FavNumber}");
        // ViewBag.Name = Name;
        // ViewBag.FavColor = FavColor;
        // ViewBag.FavNumber = FavNumber;

        if (ModelState.IsValid)
        {
            // this means we passed our validation
            // then would you redirect to success
            return RedirectToAction("Success");
        }
        else
        {
            // ViewBag.User = newUser;
            return View("Index");
        }
    }

    [HttpGet("Success")]
    public IActionResult Success(User newUser)
    {
        return View(newUser);
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
