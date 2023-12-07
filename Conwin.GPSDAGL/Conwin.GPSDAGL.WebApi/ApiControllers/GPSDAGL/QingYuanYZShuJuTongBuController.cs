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
    [ApiPrefix(typeof(QingYuanYZShuJuTongBuController))]
    public class QingYuanYZShuJuTongBuController : BaseApiController
    {
        private IQingYuanYZShuJuTongBuService _qingYuanYZShuJuTongBuService;

        public QingYuanYZShuJuTongBuController(IQingYuanYZShuJuTongBuService qingYuanYZShuJuTongBuService)
        {
            _qingYuanYZShuJuTongBuService = qingYuanYZShuJuTongBuService;
        }

        [HttpPost]
        [Route("NewYeHu")]
        public object GetNewYeHu([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetYeHuList(CWRequestParam.GetBody<Services.DtosExt.QingYuanYZShuJUTongBu.GetNewYeHuInput>() );
            return result;
        }
        [HttpPost]
        [Route("QingYuanYZCheLiang")]       
        public object GetQingYuanYZCheLiang([FromBody] string requestString)
        {
            //var result = _qingYuanYZShuJuTongBuService.GetQingYuanYZCheLiang(CWRequestParam.GetBody<Services.DtosExt.QingYuanYZShuJUTongBu.GetQingYuanYZCheLiangInput >());

            var result = _qingYuanYZShuJuTongBuService.GetCheLiangList(CWRequestParam.GetBody<Services.DtosExt.QingYuanYZShuJUTongBu.GetNewCheLiangInput>());

            return result;
        }



        [HttpPost]
        [Route("GetShengGpsYeHuList")]
        public object GetShengGpsYeHuList([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetShengGpsYeHuList(CWRequestParam.GetBody<QueryData>());
            return result;
        }

        [HttpPost]
        [Route("QueryShengGpsYeHuList")]
        public object QueryShengGpsYeHuList([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.QueryShengGpsYeHuList(CWRequestParam.GetBody<QueryData>());
            return result;
        }


        [HttpPost]
        [Route("GetShengGpsVehicleInfo")]
        public object GetShengGpsVehicleInfo([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetShengGpsVehicleInfo(CWRequestParam.GetBody<QueryData>());
            return result;
        }
        /// <summary>
        /// 获取车辆营运状态  006600200132
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShengGpsVehicleInfoNew")]
        public object GetShengGpsVehicleInfoNew([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetShengGpsVehicleInfoNew(CWRequestParam.GetBody<QueryData>());
            return result;
        }
        [HttpPost]
        [Route("GetShengGpsVehicleBaseInfo")]
        public object GetShengGpsVehicleBaseInfo([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetShengGpsVehicleBaseInfo(CWRequestParam.GetBody<QueryData>());
            return result;
        }

        [HttpPost]
        [Route("GetVehicleInformation")]
        public object GetVehicleInformation([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetVehicleInformation(CWRequestParam.GetBody<QueryData>());
            return result;
        }


        [HttpPost]
        [Route("GetVehicleConfiguration")]
        public object GetVehicleConfiguration([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetVehicleConfiguration(CWRequestParam.GetBody<QueryData>());
            return result;
        }



        [HttpPost]
        [Route("GetYeHuTwoGuestsList")]
        public object GetYeHuTwoGuestsList([FromBody] string requestString)
        {
            var result = _qingYuanYZShuJuTongBuService.GetYeHuTwoGuestsList(CWRequestParam.GetBody<QueryData>());
            return result;
        }
    }
}
