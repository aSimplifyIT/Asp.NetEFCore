using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        void Save(Employee employee);

        public IEnumerable<Employee> GetAllEmployees();
        public List<Employee> GetAllEmployeesList();

        Employee Add(Employee employee);

        Employee Update(Employee employeeChanges);

        Employee Delete(int id);
    }
}
