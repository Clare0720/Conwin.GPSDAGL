using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangVideoZhongDuanConfirm : EntityMetadata 
    {
        public string CheLiangId { get; set; }
        public string ZhongDuanId { get; set; }
        public Nullable<bool> ShuJuJieRu { get; set; }
        public Nullable<bool> SheBeiWanZheng { get; set; }
        public string NeiRong { get; set; }
        public Nullable<int> BeiAnZhuangTai { get; set; }
        public Nullable<System.DateTime> TiJiaoBeiAnShiJian { get; set; }
        public Nullable<System.DateTime> ZuiJinShenHeShiJian { get; set; }
    }
}
