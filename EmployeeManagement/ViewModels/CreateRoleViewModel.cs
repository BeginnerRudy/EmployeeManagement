using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels {
    public class CreateRoleViewModel {
        public CreateRoleViewModel() {


        }

        [Required]
        public string RoleName { get; set; }

    }
}