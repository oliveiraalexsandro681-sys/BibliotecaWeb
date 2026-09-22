using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    public class TestandoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
