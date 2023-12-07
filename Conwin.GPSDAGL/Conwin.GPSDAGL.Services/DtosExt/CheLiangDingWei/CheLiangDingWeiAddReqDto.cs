using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDingWei
{
    /// <summary>
    /// 车辆定位信息 创建请求数据
    /// </summary>
    public class CheLiangDingWeiAddReqDto
    {
        public double? LatestLongtitude { get; set; }
        public double? LatestLatitude { get; set; }
        public DateTime? LatestGpsTime { get; set; }
        public DateTime? LatestRecvTime { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }
    }
}
