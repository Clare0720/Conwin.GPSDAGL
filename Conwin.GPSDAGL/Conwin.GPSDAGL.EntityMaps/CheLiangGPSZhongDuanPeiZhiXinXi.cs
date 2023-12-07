using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangGPSZhongDuanPeiZhiXinXi : EntityMetadata 
    {
        public string ZhongDuanAnZhuangId { get; set; }
        public Nullable<int> ZhongDuanLeiXing { get; set; }
        public string ShengChanChangJia { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string SheBeiXingHao { get; set; }
        public string ShiYongCheXing { get; set; }
        public string DingWeiMoKuai { get; set; }
        public string TongXunMoShi { get; set; }
        public Nullable<int> GuoJianPiCi { get; set; }
        public string ZhongDuanBianMa { get; set; }
        public Nullable<System.DateTime> GongGaoRiQi { get; set; }
        public string CheJiXuHao { get; set; }
        public string CheJiPeiJian { get; set; }
        public string ZhuZhongXinHao { get; set; }
        public string FuZhongXinHao { get; set; }
        public string MoRenTongDao { get; set; }
        public string APN { get; set; }
        public string ZuiGaoSuDu { get; set; }
        public string ZuiDiSuDu { get; set; }
        public string ShangXianIP { get; set; }
        public string ShangXianPort { get; set; }
        public string GPSShangXianIP { get; set; }
        public string GPSShangXianPort { get; set; }
        public Nullable<int> ShiFouZhuanWang { get; set; }
        public Nullable<int> ShiFouShangChuanYunZheng { get; set; }
        public string SIMKaHao { get; set; }
        public string SIMXuHao { get; set; }
        public string ZhongDuanMDT { get; set; }
        public string M1 { get; set; }
        public string IA1 { get; set; }
        public string IC1 { get; set; }
        public Nullable<int> ShiFouAnZhuangShiPinZhongDuan { get; set; }
        public string ShiPinTouAnZhuangXuanZe { get; set; }
        public Nullable<int> ShiPingChangShangLeiXing { get; set; }
        public Nullable<int> ShiPinTouGeShu { get; set; }
        public string Remark { get; set; }
    }
}
