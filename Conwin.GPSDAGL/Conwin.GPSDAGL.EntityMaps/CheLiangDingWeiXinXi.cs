using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangDingWeiXinXi : EntityMetadata 
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<System.DateTime> ZuiJinDingWeiShiJian { get; set; }
        public Nullable<double> ZuiJinJingDu { get; set; }
        public Nullable<double> ZuiJinWeiDu { get; set; }
        public Nullable<int> ZuiJinSuDu { get; set; }
    }
}
