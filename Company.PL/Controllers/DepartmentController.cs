using Company.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    // MVC Controller 
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentController(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET: Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
    }
}
