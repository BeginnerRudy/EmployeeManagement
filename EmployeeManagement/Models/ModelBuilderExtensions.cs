using System;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models {
    public static class ModelBuilderExtensions {
        public static void Seed(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<Employee>().HasData(
                new Employee {
                    Id = 1,
                    Name = "Mary",
                    Department = Dept.HR,
                    Email = "mary@qq.com"
                },
                new Employee {
                    Id = 2,
                    Name = "Mark",
                    Department = Dept.IT,
                    Email = "mark@qq.com"
                }
            );
        }
    }
}