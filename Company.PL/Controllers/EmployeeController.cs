using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeetRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository, 
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeetRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // GET: Employee/Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);
            }
            // Dictionary : 3 Property
            // 1. ViewData : Transfer Extra Information from controller (Action) to View
            //ViewData["Message"] = "Hello From ViewData";

            // 2. ViewBag: Transfer Extra Information from controller to view
            //ViewBag.Message = "Hello from viewBag";
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                // Manual Mapping
                //var employee = new Employee()
                //{
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    DepartmentId = model.DepartmentId

                //};

                if(model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = "employee Not Found" });
            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee Not found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest();
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,

                //};
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                if(model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest("Invalid Id");
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                 _unitOfWork.EmployeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    if(model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}
