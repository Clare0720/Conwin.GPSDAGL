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
        public Nullable<int> ShiFouGeTiHu { get; set; }
        public string YeHuDaiMa { get; set; }
        public string KongGuGongSiSuoZaiXiaQuSheng { get; set; }
        public string KongGuGongSiSuoZaiXiaQuShi { get; set; }
        public string KongGuGongSiMingCheng { get; set; }
        public Nullable<int> JingJiLeiXing { get; set; }
        public Nullable<int> ShenHeZhuangTai { get; set; }
        public string JingYingXuKeZhengZi { get; set; }
        public string JingYingXuKeZhengHao { get; set; }
        public Nullable<int> JingYingXuKeZhengYouXiaoZhuangTai { get; set; }
        public Nullable<System.DateTime> JingYingXuKeZhengYouXiaoQi { get; set; }
    }
}
