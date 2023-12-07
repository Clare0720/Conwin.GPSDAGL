using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(GPSTongXunPeiZhiController))]
    public class GPSTongXunPeiZhiController : BaseApiController
    {

        private readonly IGPSTongXunPeiZhiService _gpsTongXunService  ;
        public GPSTongXunPeiZhiController(IGPSTongXunPeiZhiService  gpsTongXunService)
        {
            _gpsTongXunService = gpsTongXunService;
        }
        //  006600200072	
        #region 获取车辆GPS终端数据通讯配置信息  
        [HttpPost]
        [Route("GetGPSPeiZhiXinXi")]
        public object GetGPSPeiZhiXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<GPSTongXunPeiZhiReqDto>();
            return _gpsTongXunService.GetGPSPeiZhiXinXi(dto);
        }
        #endregion
        //  006600200073
        #region 添加或更新车辆GPS终端数据通讯配置信息
        [HttpPost]
        [Route("SetGPSPeiZhiXinXi")]
        public object SetGPSPeiZhiXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<GPSTongXunPeiZhiDto>();
            return _gpsTongXunService.SetGPSPeiZhiXinXi(dto);
        }
        #endregion

        //  006600200074	
        #region 获取车辆GPS终端数据通讯配置信息  
        [HttpPost]
        [Route("GetGPSPeiZhiXinXiByChePaiHao")]
        public object GetGPSPeiZhiXinXiByChePaiHao([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<GPSTongXunPeiZhiReqDto>();
            return _gpsTongXunService.GetGPSPeiZhiXinXiByChePaiHao(dto);
        }
        #endregion

    }
}
