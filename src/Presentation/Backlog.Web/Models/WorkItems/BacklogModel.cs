namespace Backlog.Web.Models.WorkItems
{
    public class BacklogModel
    {
        public int ProjectId { get; set; }

        public bool CanReport { get; set; }

        public bool CanClose { get; set; }

        public bool CanReOpen { get; set; }

        public bool CanComment { get; set; }
    }
}
