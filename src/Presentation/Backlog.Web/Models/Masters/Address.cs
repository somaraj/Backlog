using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class AddressModel : BaseModel
    {
        [LocalizedDisplayName("AddressModel.Address1")]
        public string Address1 { get; set; }

        [LocalizedDisplayName("AddressModel.Address2")]
        public string? Address2 { get; set; }

        [LocalizedDisplayName("AddressModel.Country")]
        public int? CountryId { get; set; }

        [LocalizedDisplayName("AddressModel.StateProvince")]
        public int? StateProvinceId { get; set; }

        [LocalizedDisplayName("AddressModel.City")]
        public string? City { get; set; }

        [LocalizedDisplayName("AddressModel.ZipPostalCode")]
        public string? ZipPostalCode { get; set; }
    }
}
