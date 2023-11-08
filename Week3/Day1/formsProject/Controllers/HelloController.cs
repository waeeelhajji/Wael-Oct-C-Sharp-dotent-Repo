using Microsoft.AspNetCore.Mvc;
namespace formProject.Controllers;

public class HelloController : Controller
{
    [HttpGet("")]
    public ViewResult Index()
    {
        return View();
    }

    [HttpPost("process")]
    public IActionResult Process(string Name, string Species, int Age)
    {
        Console.WriteLine($"Name:  {Name}");
        Console.WriteLine($"Species:  {Species}");
        Console.WriteLine($"Age:  {Age}");
        ViewBag.Name = Name;
        ViewBag.Species = Species;
        ViewBag.Age = Age;
        return View("Success");
    }

    [HttpGet("success")]
    public ViewResult Success()
    {
        return View();
    }



}


