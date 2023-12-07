using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class JiaShiYuan : EntityMetadata 
    {
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public Nullable<int> IDCardType { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> WorkingStatus { get; set; }
        public string OrgCode { get; set; }
        public string CheLiangId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> DismissalDate { get; set; }
        public string Certification { get; set; }
        public Nullable<int> Sex { get; set; }
        public string GuoJi { get; set; }
        public string HuKouDiZhi { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> CertificationEndTime { get; set; }
        public string FaZhengJiGou { get; set; }
        public string LianXiDiZhi { get; set; }
        public string ShenFenZhengZhengMian { get; set; }
        public string ShenFenZhengFanMian { get; set; }
        public string JiaShiYuanZhengMian { get; set; }
        public Nullable<System.DateTime> JiaZhaoChuCiShenLing { get; set; }
        public string ZhunJiaCheXing { get; set; }
        public string JiaZhaoHaoMa { get; set; }
        public string JiaZhaoBianHao { get; set; }
        public Nullable<System.DateTime> NianJianRiQi { get; set; }
        public Nullable<System.DateTime> JiaZhaoYouXiaoQi { get; set; }
        public string JiaShiZhengZhengMian { get; set; }
        public string JiaShiZhengFanMian { get; set; }
    }
}
