using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Country : BaseEntity
    {
        private ICollection<StateProvince> _stateProvinces;

        public string Name { get; set; }

        public string TwoLetterIsoCode { get; set; }

        public string ThreeLetterIsoCode { get; set; }

        public int DisplayOrder { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<StateProvince> StateProvinces
        {
            get => _stateProvinces ?? (_stateProvinces = new List<StateProvince>());
            protected set => _stateProvinces = value;
        }
    }
}