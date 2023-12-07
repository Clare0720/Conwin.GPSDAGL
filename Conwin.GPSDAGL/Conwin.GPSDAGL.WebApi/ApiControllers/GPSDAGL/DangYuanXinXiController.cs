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
    [ApiPrefix(typeof(DangYuanXinXiController))]
    public class DangYuanXinXiController : BaseApiController
    {
        private IDangYuanXinXiService _DangYuanXinXiService;
        public DangYuanXinXiController(IDangYuanXinXiService DangYuanXinXiService) {
            _DangYuanXinXiService = DangYuanXinXiService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody]string requestString)
        {
            return _DangYuanXinXiService.Query(CWRequestParam.GetBody<QueryData>(), UserInfo);
            
        }

        /// <summary>
        /// 创建人员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<DangYuanDto>();
             return _DangYuanXinXiService.Create(dto,base.UserInfo);
     
        }



        /// <summary>
        /// 获取单条记录详情
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Get")]
        public object Get([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<DangYuanDto>();
             return _DangYuanXinXiService.Get(dto);

        }

        /// <summary>
        /// 更新人员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<DangYuanDto>();
            return _DangYuanXinXiService.Update(dto, UserInfo);
        }

        /// <summary>
        /// 更新人员档案信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
             return _DangYuanXinXiService.Delete(ids, base.UserInfo);
        }

    }
}