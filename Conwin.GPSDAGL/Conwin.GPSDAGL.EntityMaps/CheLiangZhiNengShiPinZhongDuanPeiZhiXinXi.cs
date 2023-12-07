using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangZhiNengShiPinZhongDuanPeiZhiXinXi : EntityMetadata 
    {
        public string ZhongDuanAnZhuangId { get; set; }
        public Nullable<int> ZhongDuanLeiXing { get; set; }
        public string ShengChanChangJia { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string SheBeiXingHao { get; set; }
        public string SheBeiMDT { get; set; }
        public string SheBeiIMEI { get; set; }
        public string SheBeiBianMa { get; set; }
        public string SIM { get; set; }
        public string ShangBaoIP { get; set; }
        public string ShangBaoPort { get; set; }
        public string Remark { get; set; }
    }
}
