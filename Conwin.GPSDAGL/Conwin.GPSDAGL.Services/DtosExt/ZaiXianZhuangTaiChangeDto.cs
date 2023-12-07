using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 车辆在线状态变更 请求
    /// </summary>
    public class ZaiXianZhuangTaiChangeDto
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
        /// 实时在线状态：0=离线；1=在线；
        /// </summary>
        public int ZaiXianZhuangTai { get; set; }
    }
}
