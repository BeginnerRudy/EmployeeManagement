using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models {
    public class MockEmployeeRepository : IEmployeeRepository{
        private List<Employee> _emplyeeList;

        public MockEmployeeRepository() {
            _emplyeeList = new List<Employee>() {
                new Employee(1, "Mary",  "marg@qq.com", Dept.HR),
                new Employee(2, "John", "john@qq.com", Dept.IT),
                new Employee(3, "Sam", "sam@qq.com", Dept.IT),
            };
        }

        public Employee Add(Employee employee) {
            employee.Id = _emplyeeList.Max(e => e.Id) + 1;
            _emplyeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int Id) {
            Employee employee = _emplyeeList.FirstOrDefault(e => e.Id == Id);
            if (employee != null) {
                _emplyeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee() {
            return _emplyeeList;
        }

        public Employee GetEmployee(int Id) {
            return _emplyeeList.Find(e => e.Id == Id);
        }

        public Employee Update(Employee employeeChanges) {
            Employee employee = _emplyeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null) {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
