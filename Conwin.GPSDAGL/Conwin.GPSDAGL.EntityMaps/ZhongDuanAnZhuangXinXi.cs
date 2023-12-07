using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class ZhongDuanAnZhuangXinXi : EntityMetadata 
    {
        public string SheBeiId { get; set; }
        public string SheBeiXingHao { get; set; }
        public Nullable<int> ZhongDuanLeiXing { get; set; }
        public string CheLiangId { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public string DaiLiShangOrgCode { get; set; }
        public string FuWushangOrgCode { get; set; }
        public Nullable<int> AnZhuangZhuangTai { get; set; }
        public string AnZhuangZhengMingFuJianId { get; set; }
        public Nullable<System.DateTime> FuWuKaiShiShiJian { get; set; }
        public Nullable<System.DateTime> FuWuJieShuShiJian { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public string Remark { get; set; }
    }
}
