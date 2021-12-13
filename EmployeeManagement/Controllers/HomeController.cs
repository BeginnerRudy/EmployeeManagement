using System;
using System.IO;
using System.Linq;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers {
    [Route("[controller]/[action]")]
    [Authorize]
    public class HomeController : Controller{
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IDataProtector protector;

        // Constructor injection
        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [Route("~/")]
        [Route("")]
        [AllowAnonymous]
        public ViewResult Index() {
            var model = _employeeRepository.GetAllEmployee().Select(e => {
                e.EncryptedId = protector.Protect(e.Id.ToString());
                return e;
            });
            return View(model);
        }

        [Route("{id?}")]
        [AllowAnonymous]
        public ViewResult Details(string id) {
            int decryptedId = Convert.ToInt32(protector.Unprotect(id));
            Employee employee = _employeeRepository.GetEmployee(decryptedId);
            if (employee == null) {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", decryptedId);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() {
                Employee = employee,
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id) {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model) {
            if (ModelState.IsValid) {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null) {
                    if (model.ExistingPhotoPath != null) {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);

                }

                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }

            return View();
        }

        [HttpGet]
        public ViewResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model) {
            if (ModelState.IsValid) {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee newEmployee = new Employee {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);
            return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View(); 
        }

        [HttpDelete]
        public IActionResult Delete(int id) {
            Employee employee = _employeeRepository.GetEmployee(id);
            if (employee != null) {
                _employeeRepository.Delete(id);
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model) {
            string uniqueFileName = null;
            if (model.Photo != null) {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
