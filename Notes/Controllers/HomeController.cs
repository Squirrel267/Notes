using System;
using Microsoft.AspNetCore.Mvc;


namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
