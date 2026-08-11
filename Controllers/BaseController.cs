using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

public abstract class BaseController : Controller
{
    protected void ShowAlert(string title, string message, string type)
    {
        TempData["AlertTitle"] = title;
        TempData["AlertMessage"] = message;
        TempData["AlertType"] = type;
    }
}
