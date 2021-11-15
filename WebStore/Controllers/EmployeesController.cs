using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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

        public IActionResult Create() => View();

        public IActionResult Edit(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee is null) return NotFound();

            var view_model = new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };
            return View(view_model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.Name,
                Patronymic = Model.Patronymic,
                Age = Model.Age,
            };

            _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) => View();
                       
    }
}
