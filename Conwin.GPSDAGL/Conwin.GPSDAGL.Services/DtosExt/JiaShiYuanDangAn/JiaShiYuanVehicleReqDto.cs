using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案 驾驶员绑定车辆 请求数据
    /// </summary>
    public class JiaShiYuanVehicleReqDto
    {
        /// <summary>
        /// 驾驶员ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 车辆Id
        /// </summary>
        public string CheLiangId { get; set; }
    }
}
