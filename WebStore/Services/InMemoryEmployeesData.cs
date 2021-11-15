using WebStore.Models;
using WebStore.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {

        private readonly List<Employee> _Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },

        };
        private int _CurrentMaxId;

        public InMemoryEmployeesData() => _CurrentMaxId = _Employees.Max(i => i.Id);
        public IEnumerable<Employee> GetAll() => _Employees;

        public Employee Get(int id) => _Employees.SingleOrDefault(employee => employee.Id == id);        
        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee)) return employee.Id; //только для inMemory, для БД удалить, не учитывается наличие полных тёзок одного возраста

            employee.Id = ++_CurrentMaxId;

            _Employees.Add(employee);

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee)) return; //тоже только для inMemory

            var db_item = Get(employee.Id);

            if (db_item is null) return;

            //db_item.Id = employee.Id; ID РЕДАКТИРОВАТЬ НЕЛЬЗЯ!!!!! а в БД еще и невозможно

            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;

            

        }

        public bool Delete(int id)
        {
            var db_item = Get(id);

            if (db_item is null) return false;

            return _Employees.Remove(db_item);
        }     
 
    }
}
