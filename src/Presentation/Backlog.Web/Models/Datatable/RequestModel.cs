using Newtonsoft.Json;

namespace Backlog.Web.Models.Datatable
{
    public class RequestModel
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("sortColumn")]
        public int SortColumn { get; set; }
    }
}