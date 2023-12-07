using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers
{
    [ApiPrefix(typeof(MonitorPersonInfoController))]
    public class MonitorPersonInfoController : BaseApiController
    {
        private IMonitorPersonInfoService _monitorPersonInfoService;

        public MonitorPersonInfoController(IMonitorPersonInfoService monitorPersonInfoService)
        {
            _monitorPersonInfoService = monitorPersonInfoService;
        }


        /// <summary>
        /// 列表 006600200192
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _monitorPersonInfoService.Query(CWRequestParam.GetBody<QueryData>());

        }

        /// <summary>
        /// 创建 006600200188
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<MonitorPersonInfo>();
            return _monitorPersonInfoService.Create(dto);

        }

        /// <summary>
        /// 详情 006600200191
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out Guid id))
            {
                return _monitorPersonInfoService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }

        /// <summary>
        /// 更新 006600200189
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<MonitorPersonInfoDto>();
            return _monitorPersonInfoService.Update(dto);
        }

        /// <summary>
        /// 删除 006600200190
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _monitorPersonInfoService.Delete(ids);
        }


        /// <summary>
        /// 导出 006600200193
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportMonitorPersonInfo")]
        public object ExportMonitorPersonInfo([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _monitorPersonInfoService.ExportMonitorPersonInfo(dto);
        }

    }
}
