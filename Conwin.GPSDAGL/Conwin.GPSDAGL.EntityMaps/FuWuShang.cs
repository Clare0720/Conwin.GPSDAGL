using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShang : EntityMetadata 
    {
        public string BaseId { get; set; }
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string OrgShortName { get; set; }
        public string OrgName { get; set; }
        public string YingYeZhiZhaoHao { get; set; }
        public string TongYiSheHuiXinYongDaiMa { get; set; }
        public string YouBian { get; set; }
    }
}
