using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Models;

namespace Zante_Hotel.Controllers;

public class HomeController : Controller
{
   

    public IActionResult Index()
    {
        return View();
    }

   
}

