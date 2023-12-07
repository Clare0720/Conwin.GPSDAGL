using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.User
{
    public class OrgInfoDto
    {
        public List<string> OrgCodes { get; set; }
        public string SysId { get; set; }
        public List<string> WechatSysIds { get; set; }

    }

    public class QueryWechatOpenIdByYeHuDto
    {
        public string YeHuDaiMa { get; set; }
        public string SysId { get; set; }
        public List<string> WechatSysIds { get; set; }

    }


    public class BindAccountDto
    {
        //public string SysUserId { get; set; }
        //public string AccountName { get; set; }
        //public Guid SysId { get; set; }
        public List<WeiXinBindAccountDto> WeiXinBangDing { get; set; }
    }


    public class WeiXinBindAccountDto
    {
        public Guid SysId { get; set; }
        public string OpenId { get; set; }
        public string SysUserId { get; set; }
    }
}
