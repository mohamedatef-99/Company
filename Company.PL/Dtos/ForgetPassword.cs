using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
