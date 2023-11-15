using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SessionLecture.Models;
using Microsoft.AspNetCore.Http;

namespace SessionLecture.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HttpContext.Session.SetString("User", "Wael");
        string? user = HttpContext.Session.GetString("User");

        Console.WriteLine(user);


        if (HttpContext.Session.GetInt32("Num") == null)
        {
            HttpContext.Session.SetInt32("Num", 0);
        }


        return View();
    }

    [HttpPost("SetName")]
    public IActionResult SetName(string Name, int Number)
    {
        HttpContext.Session.SetString("NewUser", Name);
        // HttpContext.Session.SetInt32("Num", Number);
        int? original = HttpContext.Session.GetInt32("Num"); // 0
        HttpContext.Session.SetInt32("Num", (int)original + Number); //22

        return RedirectToAction("Index");
    }

    [HttpGet("delete")]
    public IActionResult deleteSession()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
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
