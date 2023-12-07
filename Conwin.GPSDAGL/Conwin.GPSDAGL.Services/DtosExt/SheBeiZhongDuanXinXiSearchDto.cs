using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class SheBeiZhongDuanXinXiSearchDto
    {
    }
    public class SheBeiZhongDuanXinXiExDto
    {
        public string ChuangJianDanWeiOrgCode { get; set; }
        public Nullable<System.Guid> ZhongDuanBiaoZhiId { get; set; }
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
        public string BeiZhu { get; set; }
        public Nullable<int> ZhuangTai { get; set; }

        public string GongGaoPiWenFuJianId { get; set; }
    }

}
