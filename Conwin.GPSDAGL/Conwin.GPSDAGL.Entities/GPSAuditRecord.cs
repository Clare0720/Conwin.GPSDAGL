using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class GPSAuditRecord : EntityMetadata 
    {
        public string VehicleId { get; set; }
        public string ResultComment { get; set; }
        public string FiledComment { get; set; }
        public Nullable<int> GPSAuditStatus { get; set; }
        public Nullable<System.DateTime> FiledDate { get; set; }
    }
}
