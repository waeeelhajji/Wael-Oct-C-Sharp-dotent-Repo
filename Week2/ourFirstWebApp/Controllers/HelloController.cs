using Microsoft.AspNetCore.Mvc; // this is a service that brings in your file MVC functionality
namespace ourFirstWebApp.Controllers;     //be sure to use your own project's namespace!
public class HelloController : Controller   //remember inheritance??
{
    //for each route this controller is to handle:
    [HttpGet]       //type of request :   
    [Route("")]     //associated route string (exclude the leading /)
    public ViewResult Index()
    {

        return View("Index");
    }

    [HttpGet("second")]
    public ViewResult Second()
    {
        // We don't need to put the name of the cshtml file in the View if it matches the action name 
        return View();
    }
    [HttpGet("third/{name}")]
    public ViewResult NameRoute(string name)
    {
        ViewBag.Name = name;
        ViewBag.MyArray = new int[5] { 1, 2, 3, 4, 5 };
        return View("Third");
    }

}


// GET ===> Reading Getting Something (?)
// POST ===> Expecting thing coming from this REQ === Data == Form 

