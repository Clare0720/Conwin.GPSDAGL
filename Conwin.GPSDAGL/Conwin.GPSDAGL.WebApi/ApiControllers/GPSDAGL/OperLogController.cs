using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
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
    [ApiPrefix(typeof(OperLogController))]
    public class OperLogController : BaseApiController
    {
        private IOperLogService _service;
        public OperLogController(IOperLogService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询操作日志列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryOperLogList")]
        public object QueryOperLogList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _service.QueryOperLogList(dto);
        }

        /// <summary>
        /// 获取操作日志详情
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOperLogDetails")]
        public object GetOperLogDetails([FromBody] string requestString)
        {

            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetOperLogDetails(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误", StatusCode = 2 };
            }
        }
    }
}
