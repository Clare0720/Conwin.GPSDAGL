using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Dtos
{
    public class SystemOrgInfoDto
    {
        public string SysId { get; set; }
        public string OrganizationName { get; set; }
        public int? OrganizationType { get; set; }
        public string OrganizationCode { get; set; }
        public string SYS_ZuiJinXiuGaiRen { get; set; }
        public string SYS_ZuiJinXiuGaiRenID { get; set; }
    }
}
