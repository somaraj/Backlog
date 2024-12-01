using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Employees
{
    public class EmployeeProjectModel : BaseModel
    {
        public EmployeeProjectModel()
        {
            AvailableEmployees = [];
        }

        public int ProjectId { get; set; }

        [LocalizedDisplayName("EmployeeProjectModel.Employee")]
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        [LocalizedDisplayName("EmployeeProjectModel.CanReport")]
        public bool CanReport { get; set; }

        [LocalizedDisplayName("EmployeeProjectModel.CanClose")]
        public bool CanClose { get; set; }

        [LocalizedDisplayName("EmployeeProjectModel.CanReOpen")]
        public bool CanReOpen { get; set; }

        [LocalizedDisplayName("EmployeeProjectModel.CanComment")]
        public bool CanComment { get; set; }

        public IList<SelectListItem> AvailableEmployees { get; set; }
    }
}
