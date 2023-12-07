using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.GPSCheLiangJianKong;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{  
    /// <summary>
    /// 车辆监控依赖接口
    /// </summary>
    [ApiPrefix(typeof(GPSCheLiangJianKongController))]
    public class GPSCheLiangJianKongController : BaseApiController
    {
        private IGPSCheLiangJianKongService _service;

        public GPSCheLiangJianKongController(IGPSCheLiangJianKongService service)
        {
            _service = service;
        }
        /// <summary>
        /// 车辆监控看板查询车辆企业监控(006600200245)
        /// </summary>
        /// <param name="requestString"></param>s
        /// <returns></returns>
        [Route("QueryCheLiangQiYeShiPing")]
        [HttpPost]
        public object QueryCheLiangQiYeShiPing([FromBody] string requestString)
        {
            return _service.QueryVehicleByUser(CWRequestParam.GetBody<CheLiangQiYeShiPingQueryDto>());
        }
    }
}
