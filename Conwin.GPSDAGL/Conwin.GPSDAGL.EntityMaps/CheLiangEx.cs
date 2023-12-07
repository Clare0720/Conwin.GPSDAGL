using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangEx : EntityMetadata 
    {
        public string CheLiangId { get; set; }
        public Nullable<System.Guid> CheLiangBiaoZhiId { get; set; }
        public Nullable<int> CheShenYanSe { get; set; }
        public string JingYingFanWei { get; set; }
        public string XingShiZhengDiZhi { get; set; }
        public Nullable<System.DateTime> XingShiZhengDengJiRiQi { get; set; }
        public string XingShiZHengSaoMiaoJianId { get; set; }
        public Nullable<int> RanLiao { get; set; }
        public string PaiQiLiang { get; set; }
        public string ZongZhiLiang { get; set; }
        public string FaDongJiHao { get; set; }
        public string CheJiaHao { get; set; }
        public Nullable<decimal> CheGao { get; set; }
        public Nullable<decimal> CheChang { get; set; }
        public Nullable<decimal> CheKuan { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string XingHao { get; set; }
        public string JiShuDengJi { get; set; }
        public string AnZhuangDengJi { get; set; }
        public Nullable<System.DateTime> ErWeiRiQi { get; set; }
        public Nullable<System.DateTime> XiaCiErWeiRiQi { get; set; }
        public Nullable<System.DateTime> ShenYanYouXiaoQi { get; set; }
        public string DaoLuYunShuZhengHao { get; set; }
        public string ZuoXing { get; set; }
        public Nullable<int> ZuoWei { get; set; }
        public Nullable<double> DunWei { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
    }
}
