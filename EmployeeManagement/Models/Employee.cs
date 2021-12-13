using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models {
    public class Employee {
        [NotMapped]
        public string EncryptedId { get; set; }

        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        public Employee(int Id, string Name, string Email, Dept Department) {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Department = Department;
        }

        public string PhotoPath { get; set; }

        public Employee() { }
    }
}
