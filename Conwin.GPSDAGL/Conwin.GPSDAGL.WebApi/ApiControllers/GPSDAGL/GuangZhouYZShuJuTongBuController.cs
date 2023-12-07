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
    [ApiPrefix(typeof(GuangZhouYZShuJuTongBuController))]
    public class GuangZhouYZShuJuTongBuController : BaseApiController
    {
        private IGuangZhouYZShuJuTongBuService _guangZhouYZShuJuTongBuService;

        public GuangZhouYZShuJuTongBuController(IGuangZhouYZShuJuTongBuService guangZhouYZShuJuTongBuService)
        {
            _guangZhouYZShuJuTongBuService = guangZhouYZShuJuTongBuService;
        }

        [HttpPost]
        [Route("GetYunZhengVehicleInfo")]
        public object GetVehicleInformation([FromBody] string requestString)
        {
            var result = _guangZhouYZShuJuTongBuService.GetYunZhengVehicleInfo(CWRequestParam.GetBody<QueryData>());
            return result;
        }

    }
}
