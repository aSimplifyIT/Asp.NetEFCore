using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    //[Route("[controller]/[action]")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
     

        public EmployeeController(IEmployeeRepository employeeRepository,
                                  IWebHostEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}


        //[Route("{id?}")]
        //[AllowAnonymous]
        public IActionResult Index(int? id)
        {
            //throw new Exception("Errors in details view.");
            Employee employee = _employeeRepository.GetEmployee(id.Value);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            EmployeeIndexViewModel employeeIndexViewModel = new EmployeeIndexViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Details"
            };

            //ViewData["Employee"] = model;

            //ViewBag.Employee = model;

            //return View(model);

            ViewData["Employee"] = employeeIndexViewModel.Employee;

            ViewBag.Employee = employeeIndexViewModel.Employee;

            return View(employeeIndexViewModel);
        }

        //[Route("")]
        //[Route("~/")]
        //[AllowAnonymous]
        [Authorize]
        public IActionResult AllEmployees()
        {
            var model = _employeeRepository.GetAllEmployeesList();
            return View(model);
        }
        public IActionResult Home()
        {
            @ViewBag.page = "Home Page";
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            @ViewBag.page = "Create User";
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                  
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Imgs");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(model.Photo.FileName);
                        string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                    
                }
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    PhotoPat = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("Index", new { id = newEmployee.Id });
            }
            return View();
        
        }

        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.Delete(id);
            return RedirectToAction("AllEmployees");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                TempData["AlertMessage"] = "Employee not exist.";
                return RedirectToAction("AllEmployees");
            }
            EditViewModel viewModel = new EditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPat
            };
            return View("Update", viewModel);
        }

        [HttpPost]
        public IActionResult Update(EditViewModel employeeChanges)
        {
            string uniqueFileName = null;
            if (employeeChanges.Photo != null)
            {

                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Imgs");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(employeeChanges.Photo.FileName);
                string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
                employeeChanges.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));

            }

            Employee newEmployee = _employeeRepository.GetEmployee(employeeChanges.Id);
            newEmployee.Name = employeeChanges.Name;
            newEmployee.Department = employeeChanges.Department;
            newEmployee.PhotoPat = uniqueFileName;
       
            _employeeRepository.Update(newEmployee);
            return RedirectToAction("AllEmployees");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
