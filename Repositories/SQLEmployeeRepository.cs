using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WebApplication1.DB;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext context;

        public SQLEmployeeRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employees;
        }

        public List<Employee> GetAllEmployeesList()
        {
            return context.Employees.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return context.Employees.Find(id);
        }

        public void Save(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
