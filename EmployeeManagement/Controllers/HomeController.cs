using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public ViewResult Index()
        {
            var modal = _employeeRepository.GetAllEmployees();
            return View(modal);
        }

        [Route("Home/Details/{id}")]
        public ViewResult Details(int id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employee=_employeeRepository.GetEmployee(id),
                pageTitle= "Employee Details"
            };
            //Employee modal = _employeeRepository.GetEmployee(1);
            //ViewData["Employee"] = modal;
            //ViewData["PageDetails"] = "Employee Details";
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee=_employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.ID,
                Name=employee.Name,
                Department=employee.Department,
                Email=employee.Email,
                ExistingPhotoPath=employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                //string uniqueFileName = null;
                //if (model.Photos != null && model.Photos.Count>0)
                if (model.Photo != null)
                {
                    if(model.ExistingPhotoPath!=null)
                    {
                        string filePath=Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    //string uniqueFileName = ProcessUploadedFile(model);
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                //Employee newEmployee = new Employee
                //{
                //    Name = model.Name,
                //    Email = model.Email,
                //    Department = model.Department,
                //    PhotoPath = uniqueFileName
                //};

                _employeeRepository.Update(employee);
                //return RedirectToAction("Details", newEmployee); 
                return RedirectToAction("Index");
            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                //List of Employees

                //foreach (IFormFile photo in model.Photos)
                //{
                //    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //}

                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        //[HttpPost]
        //public IActionResult Create(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Employee newEmployee = _employeeRepository.Add(employee);
        //        //return RedirectToAction("Details", newEmployee); 
        //        return RedirectToAction("Details", new { newEmployee.ID });
        //    }

        //    return View();
        //}

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = uniqueFileName = ProcessUploadedFile(model); 
                //if (model.Photos != null && model.Photos.Count>0)
                
                //if (model.Photo!=null)
                //{
                //    //List of Employees

                //    //foreach (IFormFile photo in model.Photos)
                //    //{
                //    //    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //    //    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                //    //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    //    photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //    //}

                //    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //}

                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(employee);
                //return RedirectToAction("Details", newEmployee); 
                return RedirectToAction("Details", new { employee.ID });
            }

            return View();
        }
    }
}
