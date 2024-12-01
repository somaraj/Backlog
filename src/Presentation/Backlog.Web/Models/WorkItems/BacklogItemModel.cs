using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.WorkItems
{
    public class BacklogItemModel : BaseModel
    {
        public BacklogItemModel()
        {
            AvailableTaskTypes = [];
            AvailableReporters = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableSeverities = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableModules = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableSubModules = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableSprints = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableAssignees = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableStatus = [];
        }

        public Guid Code { get; set; }

        [LocalizedDisplayName("BacklogItem.Title")]
        public string Title { get; set; }

        [LocalizedDisplayName("BacklogItem.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("BacklogItem.Parent")]
        public int? ParentId { get; set; }

        [LocalizedDisplayName("BacklogItem.TaskType")]
        public int TaskTypeId { get; set; }

        [LocalizedDisplayName("BacklogItem.Reporter")]
        public int? ReporterId { get; set; }

        [LocalizedDisplayName("BacklogItem.Severity")]
        public int SeverityId { get; set; }

        [LocalizedDisplayName("BacklogItem.DueDate")]
        public DateOnly? DueDate { get; set; }

        [LocalizedDisplayName("BacklogItem.Project")]
        public int ProjectId { get; set; }

        [LocalizedDisplayName("BacklogItem.Module")]
        public int? ModuleId { get; set; }

        [LocalizedDisplayName("BacklogItem.SubModule")]
        public int? SubModuleId { get; set; }

        [LocalizedDisplayName("BacklogItem.Sprint")]
        public int? SprintId { get; set; }

        [LocalizedDisplayName("BacklogItem.Assignee")]
        public int? AssigneeId { get; set; }

        [LocalizedDisplayName("BacklogItem.Status")]
        public int StatusId { get; set; }

        public int ReOpenCount { get; set; }

        public int SubTaskCount { get; set; }

        public string Token { get; set; }

        public int EditMode { get; set; }

        public IList<SelectListItem> AvailableTaskTypes { get; set; }

        public IList<SelectListItem> AvailableReporters { get; set; }

        public IList<SelectListItem> AvailableSeverities { get; set; }

        public IList<SelectListItem> AvailableModules { get; set; }

        public IList<SelectListItem> AvailableSubModules { get; set; }

        public IList<SelectListItem> AvailableSprints { get; set; }

        public IList<SelectListItem> AvailableAssignees { get; set; }

        public IList<SelectListItem> AvailableStatus { get; set; }
    }

    public class BacklogItemGridModel : BaseModel
    {
        public string Title { get; set; }

        public DateOnly? DueDate { get; set; }

        public string Project { get; set; }

        public string? Module { get; set; }

        public string? SubModule { get; set; }

        public string? Sprint { get; set; }

        public string? Assignee { get; set; }

        public int ReOpenCount { get; set; }

        public int SubTaskCount { get; set; }

        public StatusModel Status { get; set; }

        public SeverityModel Severity { get; set; }

        public TaskTypeModel TaskType { get; set; }
    }
}
