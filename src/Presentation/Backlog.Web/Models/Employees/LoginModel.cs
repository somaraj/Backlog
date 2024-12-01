using System.ComponentModel.DataAnnotations;

namespace Backlog.Web.Models.Employees
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}