using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class SheBeiZhongDuanXinXi : EntityMetadata 
    {
        public Nullable<int> SheBeiLeiBie { get; set; }
        public Nullable<int> ZhongDuanLeiXing { get; set; }
        public string SheBeiXingHao { get; set; }
        public string ShengChanChangJia { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string XingHaoBianMa { get; set; }
        public string ShiYongCheXing { get; set; }
        public string DingWeiMoKuai { get; set; }
        public string TongXunMoShi { get; set; }
        public Nullable<int> GuoJianPiCi { get; set; }
        public string ZhongDuanBianMa { get; set; }
        public Nullable<System.DateTime> GongGaoRiQi { get; set; }
        public string GongGaoPiWenFuJianId { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string Remark { get; set; }
    }
}
