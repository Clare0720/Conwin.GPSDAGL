using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 用户信息查询车辆信息返回结果DTO
    /// </summary>
    public class CheLiangJieGuoDto
    {
        public string ChePaiHao { get; set; }

        public string ChePaiYanSe { get; set; }

        public string CheLiangZhongLei { get; set; }

        public bool ShiFouZaiXian { get; set; }

        public int? ShiPingTouGeShu { get; set; }

    }
}
