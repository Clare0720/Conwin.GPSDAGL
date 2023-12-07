using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Interfaces;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    /// <summary>
    /// 车辆终端安装信息
    /// </summary>
    [ApiPrefix(typeof(AnZhuangZhongDuanController))]
    public class AnZhuangZhongDuanController : BaseApiController
    {
        private IAnZhuangZhongDuanService _service;
        public AnZhuangZhongDuanController(IAnZhuangZhongDuanService service)
        {
            _service = service;
        }


        //车辆安装终端信息列表  006600200094-1.0
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.Query(dto);
        }

        /// <summary>
        /// 导出安装终端信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportZhongDuanAnZhuangInfo")]
        public object ExportZhongDuanAnZhuangInfo([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.ExportZhongDuanAnZhuangInfo(dto);
        }


    }

}