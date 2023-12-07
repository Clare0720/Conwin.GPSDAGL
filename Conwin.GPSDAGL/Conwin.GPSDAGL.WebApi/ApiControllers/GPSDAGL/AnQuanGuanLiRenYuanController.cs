using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Web.Http;


namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(AnQuanGuanLiRenYuanController))]
    public class AnQuanGuanLiRenYuanController : BaseApiController
    {
        private IAnQuanRenYuanGuanLiService _anQuanRenYuanGuanLiService;
        public AnQuanGuanLiRenYuanController(IAnQuanRenYuanGuanLiService anQuanRenYuanGuanLiService)
        {
            _anQuanRenYuanGuanLiService = anQuanRenYuanGuanLiService;
        }

        /// <summary>
        /// 获取安全员列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _anQuanRenYuanGuanLiService.Query(CWRequestParam.GetBody<QueryData>());

        }

        /// <summary>
        /// 创建安全员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<AnQuanGuanLiRenYuanDto>();
            return _anQuanRenYuanGuanLiService.Create(dto);

        }

        /// <summary>
        /// 获取单条记录详情
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out Guid id))
            {
                return _anQuanRenYuanGuanLiService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }

        /// <summary>
        /// 更新安全员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<AnQuanGuanLiRenYuanDto>();
            return _anQuanRenYuanGuanLiService.Update(dto);
        }

        /// <summary>
        /// 更新安全员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _anQuanRenYuanGuanLiService.Delete(ids);
        }


        /// <summary>
        /// 导出安全人员档案
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportQiYeAnQuanRenYuanInfo")]
        public object ExportQiYeAnQuanRenYuanInfo([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _anQuanRenYuanGuanLiService.ExportQiYeAnQuanRenYuanInfo(dto);
        }

    }
}