using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.BaseData;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(BaseDataController))]
    public class BaseDataController : BaseApiController
    {
        private IBaseDataService _service;
        public BaseDataController(IBaseDataService service)
        {
            _service = service;
        }

        //获取辖区县列表
        [HttpPost]
        [Route("GetDistrictsList")]
        public object GetDistrictsList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryDistrictsDto>();        
            return _service.GetDistrictsList(dto);
        }



    }
}
