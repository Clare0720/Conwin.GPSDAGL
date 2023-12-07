using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShangCheLiangGPSZhongDuanXinXi : EntityMetadata 
    {
        public string FuWuShangCheLiangId { get; set; }
        public Nullable<int> ZhongDuanLeiXing { get; set; }
        public string ShengChanChangJia { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string SheBeiXingHao { get; set; }
        public string ZhongDuanBianMa { get; set; }
        public string SIMKaHao { get; set; }
        public string ZhongDuanMDT { get; set; }
        public Nullable<long> M1 { get; set; }
        public Nullable<long> IA1 { get; set; }
        public Nullable<long> IC1 { get; set; }
        public int ShiFouAnZhuangShiPinZhongDuan { get; set; }
        public string ShiPinTouAnZhuangXuanZe { get; set; }
        public int ShiPingChangShangLeiXing { get; set; }
        public int ShiPinTouGeShu { get; set; }
        public string Remark { get; set; }
    }
}
