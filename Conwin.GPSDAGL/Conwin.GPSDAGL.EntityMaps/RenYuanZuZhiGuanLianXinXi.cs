using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class RenYuanZuZhiGuanLianXinXi : EntityMetadata 
    {
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string RenYuanId { get; set; }
        public string RenYuanCode { get; set; }
        public string Positions { get; set; }
    }
}
