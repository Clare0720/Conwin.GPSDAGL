using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(SheBeiZhongDuanXinXiController))]

    public class SheBeiZhongDuanXinXiController : BaseApiController
    {
        private ISheBeiZhongDuanXinXiService  _sheBeiZhongDuanXinXiService;

        public SheBeiZhongDuanXinXiController(ISheBeiZhongDuanXinXiService sheBeiZhongDuanXinXiService)
        {
            _sheBeiZhongDuanXinXiService = sheBeiZhongDuanXinXiService;
        }

        /// <summary>
        /// 查询终端设备列表 
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QuerySheBeiZhongDuanList")]
        public object QuerySheBeiZhongDuanList([FromBody] string requestString)
        {
            return _sheBeiZhongDuanXinXiService.QuerySheBeiZhongDuanList(CWRequestParam.GetBody<QueryData>());
        }

    }
}
