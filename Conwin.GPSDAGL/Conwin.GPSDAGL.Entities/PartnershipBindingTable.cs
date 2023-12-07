using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class PartnershipBindingTable : EntityMetadata 
    {
        public string EnterpriseCode { get; set; }
        public string ServiceProviderCode { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string CooperativeContractId { get; set; }
        public string UnitOrganizationCode { get; set; }
        public Nullable<int> UnitType { get; set; }
        public string Remarks { get; set; }
    }
}
