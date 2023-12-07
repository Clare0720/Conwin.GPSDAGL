using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class PingTaiDaiLiShang : EntityMetadata 
    {
        public string BaseId { get; set; }
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string OrgShortName { get; set; }
        public string OrgName { get; set; }
        public Nullable<int> QiYeLeiXing { get; set; }
        public string YingYeZhiZhaoHao { get; set; }
        public string JingYingQuYu { get; set; }
        public string TongYiSheHuiXinYongDaiMa { get; set; }
        public string YouBian { get; set; }
        public string YingYeZhiZhaoFuBenId { get; set; }
        public string FaRenShenFenZhengFuYingJianId { get; set; }
        public string PingTaiBianHao { get; set; }
        public string PingTaiGuoJianPiCi { get; set; }
    }
}
