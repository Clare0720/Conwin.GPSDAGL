using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangEx : EntityMetadata 
    {
        public string CheLiangId { get; set; }
        public string JingYingFanWei { get; set; }
        public Nullable<System.Guid> CheLiangBiaoZhiId { get; set; }
        public Nullable<int> CheShenYanSe { get; set; }
        public string CheLiangNengLi { get; set; }
        public string XingShiZhengHao { get; set; }
        public string XingShiZhengDiZhi { get; set; }
        public Nullable<System.DateTime> XingShiZhengYouXiaoQi { get; set; }
        public Nullable<System.DateTime> XingShiZhengNianShenRiQi { get; set; }
        public string CheLiangBaoXiaoZhongLei { get; set; }
        public Nullable<System.DateTime> CheLiangBaoXiaoDaoJieZhiRiQi { get; set; }
        public Nullable<int> XingShiZhengTiXingTianShu { get; set; }
        public Nullable<System.DateTime> XingShiZhengDengJiRiQi { get; set; }
        public string XingShiZHengSaoMiaoJianId { get; set; }
        public Nullable<int> RanLiao { get; set; }
        public string PaiQiLiang { get; set; }
        public string ZongZhiLiang { get; set; }
        public Nullable<double> ZhengBeiZhiLiang { get; set; }
        public Nullable<double> HeZaiZhiLiang { get; set; }
        public string FaDongJiHao { get; set; }
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
        public Nullable<System.DateTime> DaoLuYunShuZhengYouXiaoQi { get; set; }
        public Nullable<System.DateTime> DaoLuYunShuZhengNianShenRiQi { get; set; }
        public Nullable<int> DaoLuYunShuZhengTiXingTianShu { get; set; }
        public string ZuoXing { get; set; }
        public Nullable<int> ZuoWei { get; set; }
        public Nullable<double> DunWei { get; set; }
        public Nullable<System.DateTime> ChuChangRiQi { get; set; }
        public Nullable<int> CheZhouShu { get; set; }
        public string JieBoCheLiang { get; set; }
        public string HuoWuMingCheng { get; set; }
        public string ShiFaDi { get; set; }
        public string QiFaDi { get; set; }
        public string ShiFaZhan { get; set; }
        public string QiDianZhan { get; set; }
        public Nullable<double> HuoWuDunWei { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public Nullable<int> ZhouShu { get; set; }
        public Nullable<double> ZhunQianYinZongZhiLiang { get; set; }
        public string CheLiangPinPai { get; set; }
    }
}
