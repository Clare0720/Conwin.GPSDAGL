using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.User;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(UserController))]
    public class UserController : BaseApiController
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("GetUserWechatOpenIdByOrgCode")]
        public object GetUserWechatOpenIdByOrgCode([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<OrgInfoDto>();
            return _userService.GetUserWechatOpenIdByOrgCode(dto);
        }

        [HttpPost]
        [Route("GetUserWechatOpenIdByYeHu")]
        public object GetUserWechatOpenIdByYeHu([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryWechatOpenIdByYeHuDto>();
            return _userService.GetUserWechatOpenIdByYeHu(dto);
        }
    }
}
