using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeRegistrationModel
    {
        public EmployeeRegistrationModel()
        {
            Employee = new EmployeeModel();
            Address = new AddressModel();
            AvailableCountries = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableStates = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableDepartments = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableDesignations = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableLanguages = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableRoles = [new SelectListItem { Text = "Select", Value = "" }];
        }

        public EmployeeModel Employee { get; set; }

        public AddressModel Address { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }

        public List<SelectListItem> AvailableDepartments { get; set; }

        public List<SelectListItem> AvailableDesignations { get; set; }

        public List<SelectListItem> AvailableLanguages { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; }
    }
}
