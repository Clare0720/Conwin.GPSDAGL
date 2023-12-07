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
    [ApiPrefix(typeof(FuWuShangXinXiController))]
    public class FuWuShangXinXiController : BaseApiController
    {

        private readonly IFuWuShangXinXiService _fuWuShangXinXiService;
        public FuWuShangXinXiController(IFuWuShangXinXiService fuWuShangXinXiService)
        {
            _fuWuShangXinXiService = fuWuShangXinXiService;
        }

        #region 新增
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangExDto>();
            return _fuWuShangXinXiService.Create(CWRequestParam.publicrequest.reqid, dto);
        }
        #endregion

        #region 修改
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody]string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangExDto>();
            return _fuWuShangXinXiService.Update(CWRequestParam.publicrequest.reqid, dto);
        }
        #endregion
        #region 第三方备案审核
        [HttpPost]
        [Route("FilingMaterials")]
        public object FilingMaterials([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ServiceProviderDto>();
            return _fuWuShangXinXiService.FilingMaterials(dto);
        }
        #endregion

        #region 市政府备案审核
        [HttpPost]
        [Route("FilingReview")]
        public object FilingReview([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ServiceProviderDto>();
            return _fuWuShangXinXiService.FilingReview(dto);
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
                return _fuWuShangXinXiService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }
        #endregion
        #region 根据服务商名称获取相应数据
        [HttpPost]
        [Route("GetProviderName")]
        public object GetProviderName([FromBody] string requestString)
        {
            var providerName = Convert.ToString(base.CWRequestParam.body);
            return _fuWuShangXinXiService.GetProviderName(providerName);
        }
        #endregion


        [HttpPost]
        [Route("GetMechanism")]
        public object GetMechanism([FromBody] string requestString)
        {
            var providerName = Convert.ToString(base.CWRequestParam.body);
            return _fuWuShangXinXiService.GetMechanism(providerName);
        }

        #region 列表
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody]string requestString)
        {
            return _fuWuShangXinXiService.Query(CWRequestParam.GetBody<QueryData>());
        }
        #endregion
        #region 第三方端列表
        [HttpPost]
        [Route("ThirdPartyQuery")]
        public object ThirdPartyQuery([FromBody] string requestString)
        {
            return _fuWuShangXinXiService.ThirdPartyQuery(CWRequestParam.GetBody<QueryData>());
        }
        #endregion

        /// <summary>
        /// 导出第三方端列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportThirdParty")]
        public object ExportThirdParty([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _fuWuShangXinXiService.ExportThirdParty(dto);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVerificationCode")]
        public object GetVerificationCode([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<GetEmailDto>();
            return _fuWuShangXinXiService.GetVerificationCode(dto);
        }

        /// <summary>
        /// 批量下载
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchDownload")]
        public object BatchDownload([FromBody] string requestString)
        {
            var providerName = Convert.ToString(base.CWRequestParam.body);
            return _fuWuShangXinXiService.BatchDownload(providerName);
        }

        /// <summary>
        /// 第三方机构重新提交备案
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FilingAgainMaterials")]
        public object FilingAgainMaterials([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ServiceProviderDto>();
            return _fuWuShangXinXiService.FilingAgainMaterials(dto);
        }

        /// <summary>
        /// 根据服务商编号获取详细信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDataDyNumber")]
        public object GetDataDyNumber([FromBody] string requestString)
        {
            var providerName = Convert.ToString(base.CWRequestParam.body);
            return _fuWuShangXinXiService.GetDataDyNumber(providerName);
        }

    }
}