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
    [ApiPrefix(typeof(CheLiangJieRuXinXiController))]
    public class CheLiangJieRuXinXiController : BaseApiController
    {
        private ICheLiangJieRuService _cheLiangJieRuService;
        public CheLiangJieRuXinXiController(ICheLiangJieRuService cheLiangJieRuService)
        {
            _cheLiangJieRuService = cheLiangJieRuService;
        }

        /// <summary>
        /// 查询智能视频接入信息列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryZhiNengShiPinJieRuList")]
        public object QueryZhiNengShiPinJieRuList([FromBody] string requestString)
        {
            return _cheLiangJieRuService.GetZhiNengShiPinJieRuXinXi(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 导出智能视频接入信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportZhiNengShiPingJieRuXinXi")]
        public object ExportZhiNengShiPingJieRuXinXi([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _cheLiangJieRuService.ExportZhiNengShiPingJieRuXinXi(dto);
        }


        /// <summary>
        /// 查询服务商智能视频接入信息列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFuWuShangJieRuXinXi")]
        public object GetFuWuShangJieRuXinXi([FromBody] string requestString)
        {
            return _cheLiangJieRuService.GetFuWuShangJieRuXinXi(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 导出服务商智能视频接入信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportFuWuShangJieRuXinXi")]
        public object ExportFuWuShangJieRuXinXi([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _cheLiangJieRuService.ExportFuWuShangJieRuXinXi(dto);
        }



        /// <summary>
        /// 查询辖区智能视频接入信息列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetXiaQuJieRuXinXi")]
        public object GetXiaQuJieRuXinXi([FromBody] string requestString)
        {
            return _cheLiangJieRuService.GetXiaQuJieRuXinXi(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 导出辖区智能视频接入信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportXiaQuJieRuXinXi")]
        public object ExportXiaQuJieRuXinXi([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _cheLiangJieRuService.ExportXiaQuJieRuXinXi(dto);
        }


    }
}
