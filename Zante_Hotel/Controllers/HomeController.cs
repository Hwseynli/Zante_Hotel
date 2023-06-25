using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

