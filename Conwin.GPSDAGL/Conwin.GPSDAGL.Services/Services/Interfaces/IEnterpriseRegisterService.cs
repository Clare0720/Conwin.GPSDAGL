using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IEnterpriseRegisterService : IBaseService<EnterpriseRegisterInfo>
    {
        /// <summary>
        /// 企业注册资料提交
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> EnterpriseRegister(RegisterRequestDto dto);
        /// <summary>
        /// 企业注册信息查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryEnterpriseList(QueryData dto);
        /// <summary>
        /// 企业用户获取注册审核信息
        /// </summary>
        /// <returns></returns>
        ServiceResult<ApprovalInfoDto> GetEnterpriseRegisterInfo();
        /// <summary>
        /// 企业提交注册审核信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ServiceResult<bool> SubmitEnterpriseApprovalInfo(EnterpriseApprovalSubmitInfoDto model);
        /// <summary>
        /// 主管部门查询企业备案审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryEnterpriseApprovalInfoList(QueryData data);
        /// <summary>
        /// 主管部门查看企业备案审核信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ServiceResult<EnterpriseApprovalSubmitInfoDto> GetEnterpriseApprovalInfoDetail(Guid Id);
        /// <summary>
        /// 主管部门审核企业备案申请
        /// </summary>
        /// <param name="approvalDto"></param>
        /// <returns></returns>
        ServiceResult<bool> ApprovalEnterpriseInfo(ApprovalEnterpriseDto approvalDto);
        /// <summary>
        /// 限制企业账号审核通过才能修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdatePassword(RegisterUpdatePasswordDto dto);












        /// <summary>
        /// 企业注册可选企业列表查询接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryResisterEnterpriseList(QueryData dto);

        /// <summary>
        /// 企业账号是否启用
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> EnterpriseAccountStatus(string sysid, EnterpriseDataManagementDto modelDto);

        /// <summary>
        /// 账号密码邮件发送
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> ResendEmail(ResendEmailDto dto);

        /// <summary>
        /// 合作关系绑定列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> PartnershipBindingList(QueryData queryData, UserInfoDto userInfo);

        /// <summary>
        /// 市级企业备案
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> NewPartnershipBinding(string sysid, PartnershipBindingDto modelDto);
        /// <summary>
        /// 审批不通过
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> ApprovalFailed(string sysid, PartnershipBindingDto modelDto);
        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> ExaminationPassed(string sysid, PartnershipBindingDto modelDto);

        /// <summary>
        /// 车辆第三方机构监控列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VThirdPartyMonitoringList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 发起解除绑定关系
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> UnbindOperation(string sysid, PartnershipBindingDto modelDto);
        /// <summary>
        /// 取消绑定申请
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        ServiceResult<bool> UnbindApplication(string sysid, PartnershipBindingDto modelDto);

        /// <summary>
        /// 车辆未绑定列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VehicleNotBoundList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseList(QueryData queryData);

        /// <summary>
        /// 车辆申请绑定
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VehicleApplyBindingList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 车辆绑定列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VehicleBindingList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 车辆解绑申请列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VehicleUApplicationList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 车辆绑定申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleBindingApplication(List<VehicleBindingDto> dto);

        /// <summary>
        /// 撤销车辆绑定申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleRevokeBinding(List<VehicleBindingDto> dto);
        /// <summary>
        /// 车辆第三方机构解绑申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleUnbindingApplication(List<VehicleBindingDto> dto);
        /// <summary>
        /// 撤销车辆第三方机构解绑申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleRevokeUnbinding(List<VehicleBindingDto> dto);
        /// <summary>
        /// 车辆解绑申请列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> VehicleBindingRelationshipsList(QueryData queryData, UserInfoDto userInfo);
        /// <summary>
        /// 车辆第三方机构审核通过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleBindingToExamine(List<VehicleBindingDto> dto);
        /// <summary>
        /// 车辆第三方机构审核不通过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> VehicleBindingFailToExamine(List<VehicleBindingDto> dto);

        /// <summary>
        /// 统计分析
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> StatisticalAnalysis(QueryData queryData);
        /// <summary>
        /// 企业监控人员详细
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseMonitoringList(QueryData queryData);
        /// <summary>
        /// 企业监控人员详细
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseSecurityList(QueryData queryData);

        /// <summary>
        /// 企业车辆绑定列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseVehicleBindingList(QueryData queryData);
        /// <summary>
        /// 统计分析企业详细信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<EnterpriseDetailsDto> EnterpriseDetails(EnterpriseDetailsDto queryData);
        /// <summary>
        /// 企业绑定第三方列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseBindingList(QueryData queryData);

        /// <summary>
        /// 企业审核通过列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseApproved(QueryData queryData);

        /// <summary>
        /// 导出企业委托监控统计
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportEnterpriseMonitoring(QueryData queryData);
        /// <summary>
        /// 企业第三方机构车辆关系
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> EnterpriseVehicleRelationshipList(QueryData queryData);

        /// <summary>
        /// 导出组织监控委托申请
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportPartnershipBinding(QueryData queryData);

        /// <summary>
        /// 车辆委托监控审批导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportVehicleBindingRelationships(QueryData queryData);

        /// <summary>
        /// 车辆未绑定列表导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportVehicleNotBound(QueryData queryData);
        /// <summary>
        /// 车辆绑定列表申请列表导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportVehicleApplyBinding(QueryData queryData);
        /// <summary>
        /// 车辆绑定列表导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportVehicleBinding(QueryData queryData);
        /// <summary>
        /// 车辆解绑申请列表导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportVehicleUApplication(QueryData queryData);
        /// <summary>
        /// 下载所有附属文件
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> BatchDownload(string material);

        /// <summary>
        /// 风险拨打电话短信记录
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> TelephoneLanguageStatistics(QueryData queryData);
        /// <summary>
        /// 风险拨打电话短信记录导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportTelephoneLanguageStatistics(QueryData queryData);


        /// <summary>
        /// 风险未拨打记录
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> RiskDialingFailedList(QueryData queryData);

        /// <summary>
        /// 属于第三方机构监测的车
        /// </summary>
        /// <returns></returns>
        ServiceResult<List<ThirdPartyVehicles>> VehiclesMonitoredThirdParty();
    }
}
