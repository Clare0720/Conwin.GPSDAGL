using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.VehicleOnNetStatistics
{
    public class VehicleOnNetInfoDto
    {
        public List<VehicleStatisticsDto> YunZhengCheLiangStatistics { get; set; }

        public List<VehicleStatisticsDto> ZaiXianCheLiangStatistics { get; set; }
    }
    public class VehicleStatisticsDto
    {
        public string XiaQuXian { get; set; }

        public int? VehicelCount { get; set; }

        public int? OnNetVehicleCount { get; set; }

    }
    public class VehicleAreaBaseInfo
    {
        public string ChePaiHao { get; set; }

        public string ChePaiYanSe { get; set; }

        public string XiaQuShi { get; set; }

        public string XiaQuXian { get; set; }

        public int? ZaiXianZhuangTai { get; set; }

        public string YunZhengZhuangTai { get; set; }
    }
}
