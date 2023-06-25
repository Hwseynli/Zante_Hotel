﻿
using Microsoft.AspNetCore.Mvc;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View();
            //return View();
        }
    }
}

