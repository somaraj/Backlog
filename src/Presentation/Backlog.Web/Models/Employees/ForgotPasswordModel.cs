using System.ComponentModel.DataAnnotations;

namespace Backlog.Web.Models.Employees
{
    public class ForgotPasswordModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "You can't leave this blank!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Provide a valid email")]
        public string Email { get; set; }
    }
}
