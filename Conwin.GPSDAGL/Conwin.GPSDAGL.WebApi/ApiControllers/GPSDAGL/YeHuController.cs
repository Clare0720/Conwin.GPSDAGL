using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;

using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Web.Http;
using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.DtosExt;
using Newtonsoft.Json;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.Framework.ServiceAgent.Utilities;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(YeHuController))]
    public class YeHuController : BaseApiController
    {
        private IYeHuService _yeHuService;
        public YeHuController(IYeHuService yeHuService)
        {
            _yeHuService = yeHuService;
        }

        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiangYeHuDto>();
            return _yeHuService.Create(CWRequestParam.publicrequest.sysid, dto, base.UserInfo);
        }

        [HttpPost]
        [Route("EnterpriseAudit")]
        public object EnterpriseAudit([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiangYeHuDto>();
            return _yeHuService.EnterpriseAudit(dto, base.UserInfo);
        }


        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiangYeHuDto>();
            return _yeHuService.Update(CWRequestParam.publicrequest.sysid, dto, base.UserInfo);
        }

        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _yeHuService.Delete(ids, base.UserInfo);
        }

        //合约过期
        [HttpPost]
        [Route("CancelStatus")]
        public object Canel([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _yeHuService.Cancel(ids, base.UserInfo);
        }

        //正常
        [HttpPost]
        [Route("NormalStatus")]
        public object Normal([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _yeHuService.Normal(ids, base.UserInfo);
        }


        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _yeHuService.Get(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }

        [HttpPost]
        [Route("GetByOrgCode")]
        public object GetByOrgCode([FromBody] string requestString)
        {
            var orgCode = CWRequestParam.body;
            var result = "";
            return _yeHuService.GetByOrgCode(orgCode);
        }
     
        [HttpPost]
        [Route("GetYeHuConfirmInfoStatus")]
        public object GetYeHuConfirmInfoStatus([FromBody] string requestString)
        {
            string orgCode = CWRequestParam.body ?? null;
            return _yeHuService.GetYeHuConfirmInfoStatus(orgCode);
        }

        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _yeHuService.Query(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }

        [HttpPost]
        [Route("QueryAll")]
        public object QueryAll([FromBody] string requestString)
        {
            return _yeHuService.QueryAll(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 导出企业档案
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportQiYeXinXi")]
        public object ExportQiYeXinXi([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _yeHuService.ExportQiYeXinXi(dto);
        }

        [HttpPost]
        [Route("GetQiYeXinXiByYingYeZhiZhaoHao")]
        public object GetQiYeXinXiByYingYeZhiZhaoHao([FromBody] string requestString)
        {
            string yingYeZhiZhaoHao = CWRequestParam.body.JingYingXuKeZhengHao ?? null;
            return _yeHuService.GetQiYeXinXiByYingYeZhiZhaoHao(yingYeZhiZhaoHao);
        }

        [HttpPost]
        [Route("QueryForPersonalInfoMobile")]
        public object QueryForPersonalInfoMobile([FromBody] string requestString)
        {
            return _yeHuService.QueryForPersonalInfoMobile(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }

        [HttpPost]
        [Route("AddFuWuShangGuanLianXinXi")]
        public object AddFuWuShangGuanLianXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QiYeFuWuShangGuanLianXinXiDto>();
            return _yeHuService.AddFuWuShangGuanLianXinXi(dto);
        }
        [HttpPost]
        [Route("EditFuWuShangGuanLianXinXi")]
        public object EditFuWuShangGuanLianXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QiYeFuWuShangGuanLianXinXiDto>();
            return _yeHuService.EditFuWuShangGuanLianXinXi(dto);
        }

        [HttpPost]
        [Route("DeleteFuWuShangGuanLianXinXi")]
        public object DeleteFuWuShangGuanLianXinXi([FromBody] string requestString)
        {
            Guid[] ids = base.CWRequestParam.GetBody<Guid[]>();
            return _yeHuService.DeleteFuWuShangGuanLianXinXi(ids, base.UserInfo);
        }


        [HttpPost]
        [Route("GetFuWuShangGuanLianXinXi")]
        public object GetFuWuShangGuanLianXinXi([FromBody] string requestString)
        {

            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _yeHuService.GetFuWuShangGuanLianXinXi(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }

        [HttpPost]
        [Route("QueryFuWuShangGuanLianXinXi")]
        public object QueryFuWuShangGuanLianXinXi([FromBody] string requestString)
        {

            return _yeHuService.QueryFuWuShangGuanLianXinXi(CWRequestParam.GetBody<QueryData>());
        }

        [HttpPost]
        [Route("QueryForJianKongPingTaiXinXi")]
        public object QueryForJianKongPingTaiXinXi([FromBody] string requestString)
        {

            return _yeHuService.QueryForJianKongPingTaiXinXi(CWRequestParam.GetBody<QueryData>());
        }

        [HttpPost]
        [Route("ConditionQueryFuWuShangGuanLianXinXi")]
        public object ConditionQueryFuWuShangGuanLianXinXi([FromBody] string requestString)
        {

            return _yeHuService.ConditionQueryFuWuShangGuanLianXinXi(CWRequestParam.GetBody<QueryData>());
        }

        [HttpPost]
        [Route("GetJiaShiYuanXinXi")]
        public object GetJiaShiYuanXinXi([FromBody] string requestString)
        {
            var idCard = Convert.ToString(base.CWRequestParam.body);
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
            return _yeHuService.GetJiaShiYuanXinXi(idCard);

        }



        [HttpPost]
        [Route("QiYeSynchronization")]
        public object QiYeSynchronization([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QiYeDataSynDto>();
            return _yeHuService.QiYeSynchronization(dto);
        }



        /// <summary>
        /// 导入企业信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportUpdate")]
        public ServiceResult<bool> ImportUpdate([FromBody] string requestString)
        {
            var result = new ServiceResult<bool>() { };
            ImportFuWu model;
            try
            {
                model = CWRequestParam.GetBody<ImportFuWu>();
            }
            catch (Exception e)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "反序列化失败";
                return result;
            }
            return _yeHuService.ImportUpdate(model);
        }


        /// <summary>
        /// 导入网约车企业
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportAppointmentEnterprise")]
        public ServiceResult<bool> ImportAppointmentEnterprise([FromBody] string requestString)
        {
            var result = new ServiceResult<bool>() { };
            ImportFuWu model;
            try
            {
                model = CWRequestParam.GetBody<ImportFuWu>();
            }
            catch (Exception e)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "反序列化失败";
                return result;
            }
            return _yeHuService.ImportAppointmentEnterprise(model);
        }

    }
}
