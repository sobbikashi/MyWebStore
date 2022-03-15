using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SecondAction()
        {
            return View("Index");
        }             
        
        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();        
        public IActionResult ProductDetails() => View();
        public IActionResult Shop() => View();
        public IActionResult NotFound404() => View();
        public IActionResult Redir()
        {
            return Redirect("http://yandex.ru");
        }       
    }
}
