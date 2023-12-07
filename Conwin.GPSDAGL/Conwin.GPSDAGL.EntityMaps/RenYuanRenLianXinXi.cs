using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class RenYuanRenLianXinXi : EntityMetadata 
    {
        public string RenYuanId { get; set; }
        public string RenYuanCode { get; set; }
        public Nullable<int> RenLianPlatformType { get; set; }
        public string RenLianId { get; set; }
    }
}
