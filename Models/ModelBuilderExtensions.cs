using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace WebApplication1.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "A",
                    Department = Dept.IT
                },
                new Employee
                {
                    Id = 2,
                    Name = "B",
                    Department = Dept.IT
                }
                );
        }

    }
}
