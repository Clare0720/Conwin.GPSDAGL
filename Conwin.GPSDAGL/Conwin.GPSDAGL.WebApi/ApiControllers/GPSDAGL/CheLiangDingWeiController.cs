using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDingWei;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(CheLiangDingWeiController))]
    public class CheLiangDingWeiController : BaseApiController
    {
        private ICheLiangDingWeiService _service;
        public CheLiangDingWeiController(ICheLiangDingWeiService service)
        {
            _service = service;
        }

        /// <summary>
        /// 更新车辆定位信息（有则更新，无则添加）（006600200037）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGPS")]
        public object UpdateGPS([FromBody] string requestString)
        {
            return _service.UpdateGPSInfo(CWRequestParam.GetBody<CheLiangDingWeiAddReqDto>());
        }
        //ZaiXianZhuangTaiChange
        /// <summary>
        /// 车辆在线状态更新006600200075-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheLiangZaiXianZhuangTaiChange")]
        public object CheLiangZaiXianZhuangTaiChange([FromBody] string requestString)
        {
            return _service.ZaiXianZhuangTaiChange(CWRequestParam.GetBody<ZaiXianZhuangTaiChangeDto>());
        }
    }
}