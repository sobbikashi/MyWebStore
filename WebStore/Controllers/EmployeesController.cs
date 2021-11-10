using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData; 
        
        
        //[Route("All")]
        public IActionResult Index() => View(_EmployeesData.GetAll());
        //срабатывает всегда первый, но доступен и второй тоже
        //[Route("info/{id}")]
        //[Route("info-id-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
       
    }
}
