using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.AnZhuangZhongDuan
{
    public class AnZhuangZhongDuanQueryReqDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// GPS终端号
        /// </summary>
        public string GPSZhongDuanMDT { get; set; }
        /// <summary>
        /// GPS终端SIM卡号
        /// </summary>
        public string GPSSIMKaHao { get; set; }
        /// <summary>
        /// 智能视频终端号
        /// </summary>
        public string VideoZhongDuanMDT { get; set; }

    }


    public class GetCheLiangJieRuXinXiDto
    {
        public int Count { get; set; }

        public List<AnZhuangZhongDuanListResDto> list { get; set; }
    }


    public class AnZhuangZhongDuanListResDto
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 车辆创建时间
        /// </summary>
        public DateTime? ChuangJianShiJian { get; set; }
        /// <summary>
        /// GPS终端号
        /// </summary>
        public string GPSZhongDuanMDT { get; set; }
        /// <summary>
        /// GPS终端SIM卡号
        /// </summary>
        public string GPSSIMKaHao { get; set; }
        /// <summary>
        /// 智能视频终端号
        /// </summary>
        public string VideoZhongDuanMDT { get; set; }
        /// <summary>
        /// 智能视频终端安装时间
        /// </summary>
        public DateTime? VideoAnZhuangShiJian { get; set; }
    }
}
