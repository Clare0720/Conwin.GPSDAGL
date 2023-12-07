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
    [ApiPrefix(typeof(GpsKaoHeShuJuTongBuController))]
    public class GpsKaoHeShuJuTongBuController : BaseApiController
    {
        private IGpsKaoHeShuJuTongBuService _gpsKaoHeShuJuTongBuService;

        public GpsKaoHeShuJuTongBuController(IGpsKaoHeShuJuTongBuService gpsKaoHeShuJuTongBuService)
        {
            _gpsKaoHeShuJuTongBuService = gpsKaoHeShuJuTongBuService;
        }

        [HttpPost]
        [Route("GetList")]
        public object GetNewYeHu([FromBody] string requestString)
        {
            var result = _gpsKaoHeShuJuTongBuService.GetGpsKaoHeShuJu(CWRequestParam.GetBody<Services.DtosExt.GpsKaoHeShuJuTongBu.GetGpsKaoHeShuJuInput>());

            return result;
        }

    }
}
