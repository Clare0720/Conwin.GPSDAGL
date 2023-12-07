using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(EnterpriseRegisterController))]
    public class EnterpriseRegisterController : BaseApiController
    {
        private IEnterpriseRegisterService _enterpriseRegisterService;

        public EnterpriseRegisterController(IEnterpriseRegisterService enterpriseRegisterService)
        {
            _enterpriseRegisterService = enterpriseRegisterService;
        }

        /// <summary>
        /// 企业注册提交注册申请资料
        /// 006600200182
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseRegister")]
        public object EnterpriseRegister([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<RegisterRequestDto>();
            return _enterpriseRegisterService.EnterpriseRegister(dto);
        }

        /// <summary>
        /// 查询企业注册信息 
        /// 006600200181
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryEnterpriseList")]
        public object QueryEnterpriseList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.QueryEnterpriseList(dto);
        }

        /// <summary>
        /// 企业用户获取审核注册信息 
        /// 006600200183
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEnterpriseRegisterInfo")]
        public object GetEnterpriseRegisterInfo([FromBody] string requestString)
        {
            return _enterpriseRegisterService.GetEnterpriseRegisterInfo();
        }

        /// <summary>
        /// 企业提交完善审核资料提交审核 
        /// 006600200184
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SubmitEnterpriseApprovalInfo")]
        public object SubmitEnterpriseApprovalInfo([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<EnterpriseApprovalSubmitInfoDto>();
            return _enterpriseRegisterService.SubmitEnterpriseApprovalInfo(dto);
        }



        /// <summary>
        /// 主管部门查询企业备案审核信息列表 
        /// 006600200185
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryEnterpriseApprovalInfoList")]
        public object QueryEnterpriseApprovalInfoList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.QueryEnterpriseApprovalInfoList(dto);
        }


        /// <summary>
        /// 主管部门查看企业备案审核信息详情 
        /// 006600200186
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEnterpriseApprovalInfoDetail")]
        public object GetEnterpriseApprovalInfoDetail([FromBody] string requestString)
        {
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out Guid id))
            {
                return _enterpriseRegisterService.GetEnterpriseApprovalInfoDetail(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }


        /// <summary>
        /// 主管部门审核企业备案信息 
        /// 006600200187
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ApprovalEnterpriseInfo")]
        public object ApprovalEnterpriseInfo([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ApprovalEnterpriseDto>();
            return _enterpriseRegisterService.ApprovalEnterpriseInfo(dto);
        }

        /// <summary>
        /// 企业修改密码限制 
        /// 006600200194
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePassword")]
        public object UpdatePassword([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<RegisterUpdatePasswordDto>();
            return _enterpriseRegisterService.UpdatePassword(dto);
        }

        /// <summary>
        /// 查询企业注册可选企业列表 
        /// 006600200203
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryResisterEnterpriseList")]
        public object QueryResisterEnterpriseList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.QueryResisterEnterpriseList(dto);
        }

        /// <summary>
        /// 启用账号是否启用
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseAccountStatus")]
        public object EnterpriseAccountStatus([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<EnterpriseDataManagementDto>();
            return _enterpriseRegisterService.EnterpriseAccountStatus(CWRequestParam.publicrequest.sysid, dto);
        }
        /// <summary>
        /// 账号密码邮件发送
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResendEmail")]
        public object ResendEmail([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<ResendEmailDto>();
            return _enterpriseRegisterService.ResendEmail(dto);
        }


        /// <summary>
        /// 合作关系绑定列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PartnershipBindingList")]
        public object PartnershipBindingList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.PartnershipBindingList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }
        /// <summary>
        /// 新增合作关系绑定
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("NewPartnershipBinding")]
        public object NewPartnershipBinding([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<PartnershipBindingDto>();
            return _enterpriseRegisterService.NewPartnershipBinding(CWRequestParam.publicrequest.sysid, dto);
        }
        /// <summary>
        /// 审批不通过
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ApprovalFailed")]
        public object ApprovalFailed([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<PartnershipBindingDto>();
            return _enterpriseRegisterService.ApprovalFailed(CWRequestParam.publicrequest.sysid, dto);
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExaminationPassed")]
        public object ExaminationPassed([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<PartnershipBindingDto>();
            return _enterpriseRegisterService.ExaminationPassed(CWRequestParam.publicrequest.sysid, dto);
        }

        /// <summary>
        /// 车辆第三方机构监控列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VThirdPartyMonitoringList")]
        public object VThirdPartyMonitoringList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VThirdPartyMonitoringList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }
        /// <summary>
        /// 发起解除绑定关系
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnbindOperation")]
        public object UnbindOperation([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<PartnershipBindingDto>();
            return _enterpriseRegisterService.UnbindOperation(CWRequestParam.publicrequest.sysid, dto);
        }
        /// <summary>
        /// 取消绑定申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnbindApplication")]
        public object UnbindApplication([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<PartnershipBindingDto>();
            return _enterpriseRegisterService.UnbindApplication(CWRequestParam.publicrequest.sysid, dto);
        }

        /// <summary>
        /// 车辆未绑定列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleNotBoundList")]
        public object VehicleNotBoundList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleNotBoundList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }
        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseList")]
        public object EnterpriseList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 车辆申请绑定列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleApplyBindingList")]
        public object VehicleApplyBindingList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleApplyBindingList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }

        /// <summary>
        /// 车辆绑定列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleBindingList")]
        public object VehicleBindingList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleBindingList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }
        /// <summary>
        /// 车辆解绑申请列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleUApplicationList")]
        public object VehicleUApplicationList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleUApplicationList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }

        /// <summary>
        /// 车辆绑定申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleBindingApplication")]
        public object VehicleBindingApplication([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleBindingApplication(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }

        /// <summary>
        /// 撤销车辆绑定申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleRevokeBinding")]
        public object VehicleRevokeBinding([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleRevokeBinding(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }
        /// <summary>
        /// 车辆第三方机构解绑申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleUnbindingApplication")]
        public object VehicleUnbindingApplication([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleUnbindingApplication(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }
        /// <summary>
        /// 撤销车辆第三方机构解绑申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleRevokeUnbinding")]
        public object VehicleRevokeUnbinding([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleRevokeUnbinding(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }

        /// <summary>
        /// 车辆第三方机构绑定关系列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleBindingRelationshipsList")]
        public object VehicleBindingRelationshipsList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleBindingRelationshipsList(CWRequestParam.GetBody<QueryData>(), base.UserInfo);
        }

        /// <summary>
        /// 车辆第三方机构审核通过
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleBindingToExamine")]
        public object VehicleBindingToExamine([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleBindingToExamine(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }

        /// <summary>
        /// 车辆第三方机构审核不通过
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleBindingFailToExamine")]
        public object VehicleBindingFailToExamine([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehicleBindingFailToExamine(CWRequestParam.GetBody<List<VehicleBindingDto>>());
        }
        /// <summary>
        /// 企业统计分析列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("StatisticalAnalysis")]
        public object StatisticalAnalysis([FromBody] string requestString)
        {
            var result = _enterpriseRegisterService.StatisticalAnalysis(CWRequestParam.GetBody<QueryData>());
            return result;
        }
        /// <summary>
        /// 企业监控人员详细
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseMonitoringList")]
        public object EnterpriseMonitoringList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseMonitoringList(CWRequestParam.GetBody<QueryData>());
        }
        /// <summary>
        /// 企业安全人员详细
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseSecurityList")]
        public object EnterpriseSecurityList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseSecurityList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 企业车辆绑定关系详细
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseVehicleBindingList")]
        public object EnterpriseVehicleBindingList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseVehicleBindingList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 统计分析企业详细信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseDetails")]
        public object EnterpriseDetails([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<EnterpriseDetailsDto>();
            return _enterpriseRegisterService.EnterpriseDetails(dto);
        }

        /// <summary>
        /// 统计分析 企业绑定第三方机构列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseBindingList")]
        public object EnterpriseBindingList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseBindingList(CWRequestParam.GetBody<QueryData>());
        }
        /// <summary>
        /// 企业审核通过
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseApproved")]
        public object EnterpriseApproved([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseApproved(CWRequestParam.GetBody<QueryData>());
        }



        /// <summary>
        /// 导出企业委托监控统计
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportEnterpriseMonitoring")]
        public object ExportEnterpriseMonitoring([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportEnterpriseMonitoring(dto);
        }

        /// <summary>
        /// 企业第三方机构车辆关系
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnterpriseVehicleRelationshipList")]
        public object EnterpriseVehicleRelationshipList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.EnterpriseVehicleRelationshipList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 导出组织监控委托申请
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportPartnershipBinding")]
        public object ExportPartnershipBinding([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportPartnershipBinding(dto);
        }
        /// <summary>
        /// 车辆委托监控审批导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleBindingRelationships")]
        public object ExportVehicleBindingRelationships([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportVehicleBindingRelationships(dto);
        }

        /// <summary>
        /// 车辆未绑定列表导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleNotBound")]
        public object ExportVehicleNotBound([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportVehicleNotBound(dto);
        }
        /// <summary>
        /// 车辆绑定申请列表导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleApplyBinding")]
        public object ExportVehicleApplyBinding([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportVehicleApplyBinding(dto);
        }

        /// <summary>
        /// 车辆绑定列表导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleBinding")]
        public object ExportVehicleBinding([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportVehicleBinding(dto);
        }
        /// <summary>
        /// 车辆解绑申请列表导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleUApplication")]
        public object ExportVehicleUApplication([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportVehicleUApplication(dto);
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
            return _enterpriseRegisterService.BatchDownload(providerName);
        }



        /// <summary>
        /// 电话语言统计列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TelephoneLanguageStatistics")]
        public object TelephoneLanguageStatistics([FromBody] string requestString)
        {
            return _enterpriseRegisterService.TelephoneLanguageStatistics(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 电话语言统计导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportTelephoneLanguageStatistics")]
        public object ExportTelephoneLanguageStatistics([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _enterpriseRegisterService.ExportTelephoneLanguageStatistics(dto);
        }


        /// <summary>
        /// 风险未拨打记录
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RiskDialingFailedList")]
        public object RiskDialingFailedList([FromBody] string requestString)
        {
            return _enterpriseRegisterService.RiskDialingFailedList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 属于第三方机构监测的车
        /// 006600200261
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehiclesMonitoredThirdParty")]
        public object VehiclesMonitoredThirdParty([FromBody] string requestString)
        {
            return _enterpriseRegisterService.VehiclesMonitoredThirdParty();
        }

    }
}
