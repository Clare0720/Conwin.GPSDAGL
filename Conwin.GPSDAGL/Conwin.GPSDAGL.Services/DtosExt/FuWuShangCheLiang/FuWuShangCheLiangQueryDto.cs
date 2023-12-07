using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang
{
    public class FuWuShangCheLiangQueryDto
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<int> CheLiangZhongLei { get; set; }
        public string YeHuMingCheng { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public Nullable<int> BeiAnZhuangTai { get; set; }

        public DateTime? ChuangJianShiJianStart { get; set; }
        public DateTime? ChuangJianShiJianEnd { get; set; }
        /// <summary>
        /// 数据通讯版本号
        /// </summary>
        public ZhongDuanShuJuTongXunBanBenHao? ShuJuTongXunBanBenHao { get; set; }
    }

    public class FuWuShangCheLiangQueryResultDto
    {
        public string Id { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<int> CheLiangZhongLei { get; set; }
        public string YeHuMingCheng { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public Nullable<int> BeiAnZhuangTai { get; set; }
        
        public DateTime? ChuangJianShiJian { get; set; }

        /// <summary>
        /// 数据通讯版本号
        /// </summary>
        public int? ShuJuTongXunBanBenHao { get; set; }
    }
}
