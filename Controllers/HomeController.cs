using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace GeneticImages.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}