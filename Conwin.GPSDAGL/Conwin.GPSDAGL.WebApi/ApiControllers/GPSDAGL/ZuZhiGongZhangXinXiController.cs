using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(ZuZhiGongZhangXinXiController))]
    public class ZuZhiGongZhangXinXiController : BaseApiController
    {
        private IZuZhiGongZhangXinXiService  _zuZhiGongZhangXinXiService;
        public ZuZhiGongZhangXinXiController(
            IZuZhiGongZhangXinXiService zuZhiGongZhangXinXiService
            )
        {
            _zuZhiGongZhangXinXiService = zuZhiGongZhangXinXiService;
        }


        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ZuZhiGongZhangXinXiDto>();
            return _zuZhiGongZhangXinXiService.Create(CWRequestParam.publicrequest.reqid, dto, base.UserInfo);
        }

        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ZuZhiGongZhangXinXiDto>();
            return _zuZhiGongZhangXinXiService.Update(CWRequestParam.publicrequest.reqid, dto, base.UserInfo);
        }

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _zuZhiGongZhangXinXiService.Delete(CWRequestParam.publicrequest.reqid, ids, base.UserInfo);
        }


        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _zuZhiGongZhangXinXiService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }

        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            QueryData queryData = base.CWRequestParam.GetBody<QueryData>();
            var dto = JsonConvert.DeserializeObject<ZuZhiGongZhangXinXiDto>(Convert.ToString(queryData.data));
            return _zuZhiGongZhangXinXiService.Query(dto, base.UserInfo, queryData.page, queryData.rows);
        }
    }
}
