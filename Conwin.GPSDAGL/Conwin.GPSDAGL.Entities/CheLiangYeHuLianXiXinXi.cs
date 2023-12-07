using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangYeHuLianXiXinXi : EntityMetadata 
    {
        public System.Guid CheLiangId { get; set; }
        public string YeHuPrincipalName { get; set; }
        public string YeHuPrincipalPhone { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string CongYeZiGeZhengHao { get; set; }
        public string JiZhongAnZhuangDianMingCheng { get; set; }
        public string SheBeiAnZhuangRenYuanXingMing { get; set; }
        public string SheBeiAnZhuangDanWei { get; set; }
        public string SheBeiAnZhuangRenYuanDianHua { get; set; }
    }
}
