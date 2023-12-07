using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.GpsKaoHeShuJuTongBu
{
    public class GetGpsKaoHeShuJuInput
    {
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 表类型
        /// 1： VehicleGpsDataQualifiedDetail,2 ：VehicleGpsDataDriftDetail,3 ：VehicleTrackDetail 
        /// </summary>
        public int Type { get; set; }
        public int StartNum { get; set; }
        public int PageIndex { get; set; }
        public int  PageSize { get; set; }
    }
}
