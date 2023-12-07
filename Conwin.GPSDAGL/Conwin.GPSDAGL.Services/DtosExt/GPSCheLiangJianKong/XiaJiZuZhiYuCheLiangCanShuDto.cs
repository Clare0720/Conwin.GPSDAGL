using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 下级组织和车辆传入DTO
    /// </summary>
    public class CheLiangJianKongShuQueryDto
    {
        public string SysUserId { get; set; }

        public string OrgCode { get; set; }

        public OrganizationType OrgType { get; set; }

        public string RoleCode { get; set; }

        public string OrgId { get; set; }

        public string OrgName { get; set; }

    }
}
