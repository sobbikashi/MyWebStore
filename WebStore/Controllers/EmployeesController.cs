using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {        
       
        //[Route("All")]
        public IActionResult Index() => View(__Employees);
        //срабатывает всегда первый, но доступен и второй тоже
        //[Route("info/{id}")]
        //[Route("info-id-{id}")]
        public IActionResult Details(int id)
        {
            var employee = __Employees.FirstOrDefault(employee => employee.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
       
    }
}
