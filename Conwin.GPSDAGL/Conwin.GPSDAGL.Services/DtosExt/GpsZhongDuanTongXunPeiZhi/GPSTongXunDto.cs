using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// GPS通讯配置详情
    /// </summary>
    public class GPSTongXunPeiZhiDto
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public Guid? CheLiangID { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public Guid? ZhongDuanID { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        public int? XieYiLeiXing { get; set; }
        /// <summary>
        /// 抓包来源
        /// </summary>
        public int? ZhuaBaoLaiYuan { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int? BanBenHao { get; set; }
    }

    /// <summary>
    /// 车辆GPS终端通讯配置信息 请求数据
    /// </summary>
    public class GPSTongXunPeiZhiReqDto
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public Guid CheLiangID { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public Guid ZhongDuanID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
    }
}
