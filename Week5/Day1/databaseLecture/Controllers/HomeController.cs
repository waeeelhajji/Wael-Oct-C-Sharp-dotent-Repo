using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using databaseLecture.Models;

namespace databaseLecture.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.AllItems = _context.Items.OrderBy(a => a.Name).ToList();
        return View();
    }
    [HttpPost("item/add")]
    public IActionResult AddItem(Item newItem)
    {
        ViewBag.AllItems = _context.Items.OrderBy(a => a.Name).ToList();
        // We Add this to the database so long it's correct
        if (ModelState.IsValid)
        {
            // we can add to the database
            // And pass this object to the .Add method
            _context.Add(newItem);
            // OR _context.Items.Add(newUser)
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpGet("item/delete/{ItemId}")]
    public IActionResult DeleteItem(int ItemId)
    {
        Item itemToDelete = _context.Items.SingleOrDefault(a => a.ItemId == ItemId);
        _context.Items.Remove(itemToDelete);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    [HttpGet("item/edit/{ItemId}")]
    public IActionResult EditItem(int ItemId)
    {
        // We need To find the item
        Item itemToBeEdit = _context.Items.SingleOrDefault(a => a.ItemId == ItemId);
        return View(itemToBeEdit);
    }

    [HttpPost("/item/update/{ItemId}")]
    public IActionResult UpdateItem(int itemId, Item newVersionOfItem)
    {
        Item oldItem = _context.Items.FirstOrDefault(b => b.ItemId == itemId);

        oldItem.Name = newVersionOfItem.Name;
        oldItem.Description = newVersionOfItem.Description;
        oldItem.UpdatedAt = newVersionOfItem.UpdatedAt;
        _context.SaveChanges();


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
