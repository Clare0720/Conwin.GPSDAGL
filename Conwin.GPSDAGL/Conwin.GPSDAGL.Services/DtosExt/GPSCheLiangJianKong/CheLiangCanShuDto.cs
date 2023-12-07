using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 用户信息查询车辆信息传入DTO
    /// </summary>
    public class CheLiangCanShuDto
    {
        public string ChePaiHao { get; set; }

        public string ChePaiYanSe { get; set; }

        public string SysUserId { get; set; }

        public string OrgCode { get; set; }

        public string RoleCode { get; set; }

        public bool ShiFouAnZhuangShiPingZhongDuan { get; set; }
        public string OrgId { get; set; }

    }
}
