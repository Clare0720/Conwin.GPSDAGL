using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(OrganizationController))]
    public class OrganizationController : BaseApiController
    {

        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        #region 新增
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            //var dto = base.CWRequestParam.GetBody<PingTaiDaiLiShangExDto>();
            //return _pingTaiDaiLiShangXinXiService.Create(CWRequestParam.publicrequest.reqid, dto);
            return null;
        }
        #endregion

        #region 修改
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            //var dto = base.CWRequestParam.GetBody<PingTaiDaiLiShangExDto>();
            //return _pingTaiDaiLiShangXinXiService.Update(CWRequestParam.publicrequest.reqid, dto);
            return null;
        }
        #endregion

        #region 删除
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            string sysid = CWRequestParam.publicrequest.sysid;
            return _organizationService.Delete(sysid,CWRequestParam.publicrequest.reqid, ids, base.UserInfo);
        }
        #endregion

        #region 停用
        [HttpPost]
        [Route("CancelStatus")]
        public object Canel([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _organizationService.Cancel(CWRequestParam.publicrequest.reqid, ids);
        }
        #endregion

        #region 启用
        [HttpPost]
        [Route("NormalStatus")]
        public object Normal([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _organizationService.Normal(CWRequestParam.publicrequest.reqid, ids);
        }
        #endregion

        #region 查看
        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            //Guid id;
            //if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            //{
            //    return _pingTaiDaiLiShangXinXiService.Get(id);
            //}
            //else
            //{
            //    return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            //}
            return null;
        }
        #endregion

        #region 列表
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            //return _pingTaiDaiLiShangXinXiService.Query(CWRequestParam.GetBody<QueryData>());
            return null;
        }
        #endregion

    }
}