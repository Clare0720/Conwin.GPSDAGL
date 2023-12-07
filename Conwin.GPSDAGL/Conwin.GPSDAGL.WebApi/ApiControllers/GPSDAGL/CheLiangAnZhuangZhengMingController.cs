using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing;
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
    [ApiPrefix(typeof(CheLiangAnZhuangZhengMingController))]
    public class CheLiangAnZhuangZhengMingController : BaseApiController
    {
        private ICheLiangAnZhuangZhengMingService _cheLiangAnZhuangZhengMingService;
        public CheLiangAnZhuangZhengMingController(ICheLiangAnZhuangZhengMingService cheLiangAnZhuangZhengMingService)
        {
            _cheLiangAnZhuangZhengMingService = cheLiangAnZhuangZhengMingService;
        }

        /// <summary>
        /// 查询安装证明数据列表 
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _cheLiangAnZhuangZhengMingService.QueryList(CWRequestParam.GetBody<QueryData>());
        }
        /// <summary>
        /// 生成重型货车智能视频安装证明承诺函
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateVideoDeviceInstallCert")]
        public object GenerateVideoDeviceInstallCert([FromBody] string requestString)
        {
            var id = new Guid(base.CWRequestParam.body.ToString());
            return _cheLiangAnZhuangZhengMingService.GenerateVideoDeviceInstallCert(id, base.UserInfo);
        }

        /// <summary>
        /// 生成重型货车智能视频安装证明承诺函
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ZhiNengZhiPinChengNuoHanByBaoXian")]
        public object ZhiNengZhiPinChengNuoHanByBaoXian([FromBody] string requestString)
        {
            var id = new Guid(base.CWRequestParam.body.ToString());
            return _cheLiangAnZhuangZhengMingService.ZhiNengZhiPinChengNuoHanByBaoXian(id, base.UserInfo);
        }


        /// <summary>
        /// 导出安装证明数据列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportAnZhuangZhengMing")]
        public object ExportAnZhuangZhengMing([FromBody] string requestString)
        {
            var dto = JsonConvert.DeserializeObject<QueryListRequestDto>(Convert.ToString(base.CWRequestParam.body));
            return _cheLiangAnZhuangZhengMingService.ExportAnZhuangZhengMing(dto);
        }

    }
}
