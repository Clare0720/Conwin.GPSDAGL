using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(JiaShiYuanController))]
    public class JiaShiYuanController : BaseApiController
    {
        private readonly IJiaShiYuanService _jiaShiYuanService;

        public JiaShiYuanController(IJiaShiYuanService jiaShiYuanService)
        {
            _jiaShiYuanService = jiaShiYuanService;
        }

        /// <summary>
        /// 驾驶员列表查询（006600200030）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _jiaShiYuanService.Query(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 驾驶员报表导出（006600200035）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Export")]
        public object Export([FromBody] string requestString)
        {
            return _jiaShiYuanService.Export(CWRequestParam.GetBody<JiaShiYuanSearchDto>());
        }

        /// <summary>
        /// 驾驶员详情（006600200031）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Detail")]
        public object Detail([FromBody] string requestString)
        {
            return _jiaShiYuanService.Detail(Convert.ToString(CWRequestParam.body));
        }

        /// <summary>
        /// 创建驾驶员信息（006600200029）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            return _jiaShiYuanService.Create(CWRequestParam.GetBody<JiaShiYuanCreateReqDto>(), base.UserInfo);
        }

        /// <summary>
        /// 修改驾驶员信息（006600200032）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            return _jiaShiYuanService.Update(CWRequestParam.GetBody<JiaShiYuanUpdateReqDto>(), base.UserInfo);
        }

        /// <summary>
        /// 删除驾驶员信息（006600200033）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            return _jiaShiYuanService.Delete(CWRequestParam.GetBody<string[]>(), base.UserInfo);
        }

        /// <summary>
        /// 聘用/解聘驾驶员（006600200034）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HireOrDismissal")]
        public object HireOrDismissal([FromBody] string requestString)
        {
            return _jiaShiYuanService.HireOrDismissal(CWRequestParam.GetBody<JiaShiYuanHireReqDto>(), base.UserInfo);
        }

        /// <summary>
        /// 绑定车辆（006600200036）
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindVehicle")]
        public object BindVehicle([FromBody] string requestString)
        {
            return _jiaShiYuanService.BindVehicle(CWRequestParam.GetBody<JiaShiYuanVehicleReqDto>(), base.UserInfo);
        }

    }
}
