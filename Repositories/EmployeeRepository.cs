using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public EmployeeRepository() {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Department = Dept.CM, Name = "A" },
                new Employee() { Id = 2, Department = Dept.IT, Name = "B" }
            };
        }

        public IEnumerable<Employee> GetAllEmployees() 
        { 
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id ) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee changeEmployee)
        {

            Employee employee = _employeeList.FirstOrDefault(e => e.Id == changeEmployee.Id);
            if (employee != null)
            {
                employee.Name = changeEmployee.Name;
                employee.Department = changeEmployee.Department;
            }
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
            
        }

        public void Save(Employee employee)
        {
            Console.Write(employee.Id);
            Console.Write(employee.Name);
        }

        public List<Employee> GetAllEmployeesList()
        {
            throw new NotImplementedException();
        }
    }
}
