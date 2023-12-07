using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangYeHu : EntityMetadata 
    {
        public string BaseId { get; set; }
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string OrgShortName { get; set; }
        public string OrgName { get; set; }
        public string SuoShuJianKongPingTai { get; set; }
        public string JingYingXuKeZhengHao { get; set; }
        public Nullable<System.DateTime> JingYingXuKeZhengYouXiaoQi { get; set; }
        public Nullable<bool> JingYingXuKeZhengChangQiYouXiao { get; set; }
        public string GongShangYingYeZhiZhaoHao { get; set; }
        public Nullable<System.DateTime> GongShangYingYeZhiZhaoYouXiaoQi { get; set; }
        public Nullable<bool> GongShangYingYeZhiZhaoChangQiYouXiao { get; set; }
        public string QiYeXingZhi { get; set; }
        public string LianXiRen { get; set; }
        public string ChuanZhenHaoMa { get; set; }
        public string LianXiFangShi { get; set; }
        public Nullable<int> ShiFouGeTiHu { get; set; }
        public Nullable<int> ShenHeZhuangTai { get; set; }
        public string JingJiLeiXing { get; set; }
        public string SuoShuQiYe { get; set; }
        public Nullable<System.Guid> QiYeBiaoZhiId { get; set; }
        public int IsConfirmInfo { get; set; }
        public string SheHuiXinYongDaiMa { get; set; }
        public string GeTiHuShenFenZhengHaoMa { get; set; }
    }
}
