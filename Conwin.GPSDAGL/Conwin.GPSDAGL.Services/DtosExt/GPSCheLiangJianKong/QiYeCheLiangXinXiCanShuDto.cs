using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 查询车队、企业及其所属车辆参数DTO
    /// </summary>
    [DataContract(IsReference = true)]
    public class QiYeCheLiangXinXiCanShuDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string SysUserId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? OrgType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OrgName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string RoleCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OrgId { get; set; }
    }
}
