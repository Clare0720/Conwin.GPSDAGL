using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class VehiclePartnershipBinding : EntityMetadata 
    {
        public string EnterpriseCode { get; set; }
        public string ServiceProviderCode { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string LicensePlateNumber { get; set; }
        public string LicensePlateColor { get; set; }
        public string Remarks { get; set; }
    }
}
