using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Range(22, 60)]
        public int? Age { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Create At")]
        public DateTime CreateAt { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }

    }
}
