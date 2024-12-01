using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Address : BaseEntity
    {
        public string Address1 { get; set; }

        public string? Address2 { get; set; }

        public int? CountryId { get; set; }

        public int? StateProvinceId { get; set; }

        public string? City { get; set; }

        public string ZipPostalCode { get; set; }

        public virtual Country Country { get; set; }

        public virtual StateProvince StateProvince { get; set; }
    }
}
