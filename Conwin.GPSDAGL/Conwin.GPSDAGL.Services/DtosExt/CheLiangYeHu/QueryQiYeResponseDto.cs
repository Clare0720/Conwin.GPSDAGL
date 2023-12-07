using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class QueryQiYeResponseDto
    {
        public Guid? Id { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }
        public string JingYingFanWei { get; set; }
        public int? ZhuangTai { get; set; }
        public int? ShenHeZhuangTai { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string JingYingXuKeZhengHao { get; set; }
        public DateTime? ChuangJianShiJian { get; set; }
        public string LianXiRen { get; set; }
        public string LianXiFangShi { get; set; }
        public string QiYeGuanLiYuan { get; set; }
        public int? IsHavCar { get; set; }
        public string JingYingQuYu { get; set; }

        public int? RegistrationStatus { get; set; }

        /// <summary>
        /// 企业id
        /// </summary>
        public  string  EnterpriseId { get; set; }
        /// <summary>
        /// 企业性质
        /// </summary>
        public string QiYeXingZhi { get; set; }

    }

    public class GetQiYeInfoDto
    {
        public int Count { get; set; }

        public List<QueryQiYeResponseDto> list { get; set; }
    }



    public class EnterpriseDataManagementDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 企业注册审核记录id
        /// </summary>
        public Guid?  Id { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        public string QiYeCode { get; set; }
        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 是否选择第三方
        /// </summary>
        public bool? ShiFouXuanZeDiSanFang { get; set; }

        /// <summary>
        /// 所选第三方机构
        /// </summary>
        public  string NameOfServiceProvider { get; set; }
        /// <summary>
        /// 第三方机构编号
        /// </summary>
        public  string DiSanFangCode { get; set; }
        /// <summary>
        /// 安全员
        /// </summary>
        public string SafetyOfficer { get; set; }
        /// <summary>
        /// 监控员姓名
        /// </summary>
        public string MonitorName { get; set; }
        /// <summary>
        /// 提交时间：企业注册审核记录
        /// </summary>
        public DateTime? SYS_ChuangJianShiJian { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string YouXiang { get; set; }
        /// <summary>
        /// 经营许可证
        /// </summary>
        public string JingYingXuKeZheng { get; set; }
        /// <summary>
        /// 经营许可证有效期
        /// </summary>
        public DateTime? JingYingXuKeZhengYouXiaoQi { get; set; }
        /// <summary>
        /// 经营许可证是否长期有效
        /// </summary>
        public bool? JingYingXuKeZhengShiFouChangQiYouXiao { get; set; }
        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int ZhuangTai { get; set; }
        /// <summary>
        /// 第三方备注
        /// </summary>
        public string DiSanFangBeiZhu { get; set; }

        /// <summary>
        /// 市政府备注
        /// </summary>
        public string ZhengFuBeiZhu { get; set; }

        public string BatchParameters { get; set; }
    }

    public class GetEnterpriseDataManagementDto
    {
        public int Count { get; set; }

        public List<EnterpriseDataManagementDto> List { get; set; }
    }

    public class BatchApprovalParameters
    {
        /// <summary>
        /// 企业代码
        /// </summary>
        public  string QiYeCode { get; set; }
        /// <summary>
        /// 服务商编号
        /// </summary>
        public string DiSanFangCode { get; set; }
    }

    public class BatchApprovalParametersDto
    {
        public List<BatchApprovalParameters> BatchParameters { get; set; }
    }


    public class ResendEmailDto
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusinessLicense { get; set; }

        public bool PasswordStatus { get; set; }

        public string Password { get; set; }
    }


      public class GetPartnershipBindingDto
    {
        public int Count { get; set; }

        public List<PartnershipBindingDto> List { get; set; }
    }

    public class PartnershipBindingDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 合作关系绑定记录id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        public string EnterpriseCode { get; set; }
        /// <summary>
        /// 服务商代码
        /// </summary>
        public string ServiceProviderCode { get; set; }
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceProviderName { get; set; }
        /// <summary>
        /// 合作合同附件
        /// </summary>
        public string CooperativeContractId { get; set; }
        /// <summary>
        /// 创建单位组织代码
        /// </summary>
        public string UnitOrganizationCode { get; set; }
        /// <summary>
        /// 创建单位类型（2.企业 5.第三方机构）
        /// </summary>
        public int UnitType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int ZhuangTai { get; set; }
        /// <summary>
        /// 提交时间：企业注册审核记录
        /// </summary>
        public DateTime? SYS_ChuangJianShiJian { get; set; }

        //组织代码（查询）
        public string OrganizationCode { get; set; }

        public string OrgCode { get; set; }

        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
    }


    public class GetVehicleBindingDto
    {
        public int Count { get; set; }

        public List<VehicleBindingDto> List { get; set; }
    }

    public class VehicleBindingDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 车辆合作关系绑定记录id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        public string EnterpriseCode { get; set; }
        /// <summary>
        /// 服务商代码
        /// </summary>
        public string ServiceProviderCode { get; set; }
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string ServiceProviderName { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ZhuangTai { get; set; }
        /// <summary>
        /// 提交时间：企业注册审核记录
        /// </summary>
        public DateTime? SYS_ChuangJianShiJian { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlateNumber { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string LicensePlateColor { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int? VehicleType { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 企业编号(查询)
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 1代表绑定车辆 2代表未绑定车辆
        /// </summary>
        public int? Type{ get; set; }
}

    public class GetVehicleStatisticalDto
    {
        public int Count { get; set; }

        public List<VehicleStatisticalDto> List { get; set; }
    }

    public class VehicleStatisticalDto
    {
        /// <summary>
        /// 企业编号
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 安全责任人
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 监控员数量
        /// </summary>
        public int MonitorPersonCount { get; set; }
        /// <summary>
        /// 安全人员数量
        /// </summary>
        public int SecurityPersonnelCount { get; set; }
        /// <summary>
        /// 人员社保证明数量
        /// </summary>
        public int PersonnelSocialCount { get; set; }
        /// <summary>
        /// 人员劳务合同证明材料数量
        /// </summary>
        public int ServiceContractCount { get; set; }
        /// <summary>
        /// 监控人员培训证明材料数量
        /// </summary>
        public int PersonnelTrainingCount { get; set; }
        /// <summary>
        /// 绑定第三方数量
        /// </summary>
        public int BindTPartyCount { get; set; }
        /// <summary>
        /// 未绑定第三方车辆数
        /// </summary>
        public int UnboundVehicleCount { get; set; }
        /// <summary>
        /// 已绑定第三方车辆数
        /// </summary>
        public int BoundVehicleCount { get; set; }
        /// <summary>
        /// 行
        /// </summary>
        public int RowNumber { get; set; }
        public DateTime SYS_ChuangJianShiJian { get; set; }
    }
    public class GetPersonnelInformationDto
    {
        public int Count { get; set; }

        public List<PersonnelInformation> List { get; set; }
    }

    public class PersonnelInformation
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        public DateTime SYS_ChuangJianShiJian { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 劳动合同文件
        /// </summary>
        public string LaborContractFileId { get; set; }

        /// <summary>
        /// 人员考核通过证明
        /// </summary>
        public string CertificatePassingExaminationFileId { get; set; }

        /// <summary>
        /// 人员社保合同
        /// </summary>
        public string SocialSecurityContractFileId { get; set; }

        /// <summary>
        /// 企业编号
        /// </summary>
        public string OrgCode { get; set; }
    }

    public class EnterpriseDetailsDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 安全责任人
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 经营许可证号码
        /// </summary>
        public string BusinessPermitNumber { get; set; }
        /// <summary>
        /// 安全责任人身份证号码
        /// </summary>
        public string PrincipalIDCard { get; set; }
        /// <summary>
        /// 安全责任人联系电话
        /// </summary>
        public string PrincipalTel { get; set; }
        /// <summary>
        /// 监控类型
        /// </summary>
        public int? MonitorType { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        public int? EnterpriseType { get; set; }
        /// <summary>
        /// 营业执照照片
        /// </summary>
        public string BusinessLicenseFileId { get; set; }
        /// <summary>
        /// 企业编号（查询）
        /// </summary>
        public string OrgCode { get; set; }
    }

    /// <summary>
    /// 企业附件
    /// </summary>
    public class EnterpriseAccessories
    {    /// <summary>
        /// 联系人身份证正面照片
         /// </summary>
        public string ContactIDCardFrontId { get; set; }
        /// <summary>
        /// 联系人身份证反面照片
        /// </summary>
        public string ContactIDCardBackId { get; set; }
        /// <summary>
        /// 责任人身份证正面
        /// </summary>
        public string PrincipalIDCardFrontId { get; set; }
        /// <summary>
        /// 责任人身份证反面
        /// </summary>
        public string PrincipalIDCardBackId { get; set; }
        /// <summary>
        /// 经营许可证照片
        /// </summary>
        public string BusinessPermitPhotoFIleId { get; set; }
        /// <summary>
        /// 营业执照照片
        /// </summary>
        public string BusinessLicenseFileId { get; set; }
    }


    public class GetTelephoneLanguageStatisticsDto
    {
        public int Count { get; set; }

        public List<TelephoneLanguageStatisticsDto> List { get; set; }
    }


    public class TelephoneLanguageStatisticsDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public  string YeHuMingCheng { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 拨打时间
        /// </summary>
        public DateTime? CallStartTime { get; set; }
        /// <summary>
        /// 呼叫完成
        /// </summary>
        public int NORMAL_CLEARING { get; set; }
        /// <summary>
        /// 关机
        /// </summary>
        public int POWER_OFF { get; set; }
        /// <summary>
        /// 停机
        /// </summary>
        public int OUT_OF_SERVICE { get; set; }
        /// <summary>
        /// 拒听
        /// </summary>
        public int NOT_CONVENIENT { get; set; }
        /// <summary>
        /// 空号
        /// </summary>
        public int DOES_NOT_EXIST { get; set; }
        /// <summary>
        /// 无法接通
        /// </summary>
        public int NOT_REACHABLE { get; set; }
        /// <summary>
        /// 无人接听
        /// </summary>
        public int NOT_ANSWER { get; set; }
        /// <summary>
        /// 呼叫正忙
        /// </summary>
        public int BUSY { get; set; }
        /// <summary>
        /// 欠费
        /// </summary>
        public int DEFAULTING { get; set; }
        /// <summary>
        /// 中继线路无响应
        /// </summary>
        public int NO_USER_RESPONSE { get; set; }
        /// <summary>
        /// 超时无人应答
        /// </summary>
        public int NO_ANSWER { get; set; }
        /// <summary>
        /// 中继线路忙
        /// </summary>
        public int USER_BUSY { get; set; }
        /// <summary>
        /// 主叫线路故障
        /// </summary>
        public int TRUNK_LINE_FAULT { get; set; }
        /// <summary>
        /// 主叫设备故障
        /// </summary>
        public int FACILITY_FAULT { get; set; }
        /// <summary>
        /// 机器人故障
        /// </summary>
        public int AI_ROBOT_FAULT { get; set; }
        /// <summary>
        /// 业务限制
        /// </summary>
        public int BUSINESS_RESTRICT { get; set; }
        /// <summary>
        /// 其他故障
        /// </summary>
        public int UNKNOWN_ERROR { get; set; }
       /// <summary>
       /// 语音合计
       /// </summary>
        public int VoiceCombined { get; set; }
        /// <summary>
        /// 短信合计
        /// </summary>
       public int TextCombined { get; set; }
    }

    public class GetRiskDialingFailedDtoDto
    {
        public int Count { get; set; }

        public List<GetRiskDialingFailedDto> List { get; set; }
    }
    public class GetRiskDialingFailedDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string YeHuMingCheng { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 风险开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string SYS_XiTongBeiZhu { get; set; }
        /// <summary>
        /// 拨打场景
        /// </summary>
        public string CallType { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string RegistrationNo { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string RegistrationNoColor { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public string CheLiangZhongLei { get; set; }

        public DateTime? RiskStartTime { get; set; }
        public DateTime? RiskEndTime { get; set; }
    }


    public class UserAccountModel
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }



    public class EnterpriseCodeModel
    {
      
        public string OrgCode { get; set; }
    }

    public class VehicleSpotCheck
    {
        public Guid ID { get; set; }
    }
}

