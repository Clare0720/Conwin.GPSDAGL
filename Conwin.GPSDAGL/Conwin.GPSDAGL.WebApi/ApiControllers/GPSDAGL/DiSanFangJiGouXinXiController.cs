using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(DiSanFangJiGouXinXiController))]
    public class DiSanFangJiGouXinXiController : BaseApiController
    {

        private readonly IDiSanFangXinXiService _diSanFangXinXiService;
        public DiSanFangJiGouXinXiController(IDiSanFangXinXiService diSanFangXinXiService)
        {
            _diSanFangXinXiService = diSanFangXinXiService;
        }

        #region 新增
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<DiSanFangExDto>();
            string sysId = CWRequestParam.publicrequest.sysid;
            return _diSanFangXinXiService.Create(sysId, dto);
        }
        #endregion

        #region 修改
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<DiSanFangExDto>();
            string sysId = CWRequestParam.publicrequest.sysid;
            return _diSanFangXinXiService.Update(sysId, dto);
        }
        #endregion

        #region 查看
        [HttpPost]
        [Route("Get")]
        public object Get([FromBody]string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _diSanFangXinXiService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }
        #endregion

        #region 列表
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody]string requestString)
        {
            return _diSanFangXinXiService.Query(CWRequestParam.GetBody<QueryData>());
        }
        #endregion

    }
}