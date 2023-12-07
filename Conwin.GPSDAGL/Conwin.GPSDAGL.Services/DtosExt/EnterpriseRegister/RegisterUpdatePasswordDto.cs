using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister
{
    public class RegisterUpdatePasswordDto
    {
        public string OldPassWord { get; set; }

        public string Password { get;set;}

        public string SysId { get; set; }
    }


    public class UpdatePassWordParamDto
    {
        public string SysId { get; set; }
        public string AccountName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }

        public Guid SYS_ZuiJinXiuGaiRenId { get; set; }

        public string SYS_ZuiJinXiuGaiRen { get; set; }
    }
}
