using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.DAL.Context
{
    public class EmployeesDB: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        
        public EmployeesDB(DbContextOptions<EmployeesDB> options) : base(options) { }
    }
}
