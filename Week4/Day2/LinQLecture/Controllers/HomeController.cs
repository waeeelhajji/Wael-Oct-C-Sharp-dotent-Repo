using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LinQLecture.Models;

namespace LinQLecture.Controllers;

public class HomeController : Controller
{
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
        new Game{Title="Doki Doki Literature Club Plus!", Price=10.00, Genre="Casual", Rating="E", Platform="PC"},
        new Game {Title="Red Dead Redemption", Price=40.00, Genre="Action adventure", Rating="M", Platform="All"},
        new Game {Title="My Little Pony A Maretime Bay Adventure", Price=39.99, Genre="Adventure", Rating="E",Platform="All"},
        new Game {Title="Fallout New Vegas", Price=10.00, Genre="Open World RPG", Rating="M", Platform="PC"}
    };
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // this will get us all the data and in Order
        List<Game> allGamesFromData = AllGames.ToList();
        List<Game> allGamesInOrder = AllGames.OrderBy(s => s.Title).ToList();
        ViewBag.OrderByGames = allGamesInOrder;
        ViewBag.AllGames = allGamesFromData;
        // Get only the games available on all platforms or the Rating T
        List<Game> allPlatforms = AllGames.Where(a => a.Platform == "All" || a.Rating == "T").ToList();
        ViewBag.AllPlatforms = allPlatforms;

        // Get First Three Games With The Rating M
        List<Game> firstThreeGames = AllGames.Where(s => s.Rating == "M").Take(3).ToList();
        ViewBag.FirstThreeGames = firstThreeGames;

        // First three Games With The Rating M with Lowest Price
        List<Game> firstThreeGameOrderByPrice = AllGames.Where(b => b.Rating == "M").OrderBy(q => q.Price).Take(3).ToList();
        ViewBag.FirstThreeGameOrderByPrice = firstThreeGameOrderByPrice;

        // Getting One Single Game 
        Game singleGame = AllGames.FirstOrDefault(d => d.Title == "God of War");
        ViewBag.SingleGame = singleGame;


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
