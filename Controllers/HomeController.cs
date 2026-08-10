using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        
        return View();
    }
}