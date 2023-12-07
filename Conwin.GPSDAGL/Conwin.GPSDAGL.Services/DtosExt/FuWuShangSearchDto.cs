using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class FuWuShangSearchDto
    {
        public string JiGouMingCheng { get; set; }
        public string YingYeZhiZhaoHao { get; set; }
        public string YouXiaoZhuangTai { get; set; }
        public string LianXiRen { get; set; }
        public string ShouJiHaoMa { get; set; }
        public string HeZuoFangShi { get; set; }
    }

    public class FuWuShangGetDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string RegistrationStatus { get; set; }
        /// <summary>
        /// 开始提交时间
        /// </summary>
        public DateTime? BeiAnTongGuoBeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? BeiAnTongGuoEndTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public  string Type { get; set; }

        public DateTime? BeginTime  { get; set; }
        public string ChuangJianShiJian { get; set; }

        public  string LianXiRenName { get; set; }
        public string LianXiRenPhone { get; set; }

        public string OrgCode { get; set; }
    }
    public class GetFuWuShangDto
    {
        public int Count { get; set; }

        public List<FuWuShangGetDto> List { get; set; }
    }
    public class FuWuShangExDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string OrgShortName { get; set; }

        /// <summary>
        /// 营业执照号
        /// </summary>
        public string YingYeZhiZhaoHao { get; set; }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string TongYiSheHuiXinYongDaiMa { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public string QiYeLeiXing { get; set; }

        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }

        /// <summary>
        /// 有效状态
        /// </summary>
        public int? YouXiaoZhuangTai { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string YouBian { get; set; }

        /// <summary>
        ///  平台编号
        /// </summary>
        public string PingTaiBianHao { get; set; }

        /// <summary>
        /// 过检批次
        /// </summary>
        public string PingTaiGuoJianPiCi { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string GongSiDiZhi { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }

        /// <summary>
        /// 营业执照副本Id
        /// </summary>
        public string YingYeZhiZhaoFuBenId { get; set; }

        /// <summary>
        /// 法人代表身份证复印件 Id
        /// </summary>
        public string FaRenShenFenZhengFuYingJianId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string FuZheRen { get; set; }

        /// <summary>
        /// 负责人 电话
        /// </summary>
        public string FuZheRenDianHua { get; set; }

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
        /// 服务商审核状态
        /// </summary>
        public string RegistrationStatus { get; set; }
    }


    public class ServiceProviderDto
    {
        public  string Id { get; set; }
        /// <summary>
        /// 第三方机构编号
        /// </summary>
        public  string OperatorCode { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public int CompanyType { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string UnifiedSocialCreditCode { get; set; }
        /// <summary>
        /// 工商营业执照
        /// </summary>
        public string IndustrialACBLicenseId { get; set; }
        /// <summary>
        /// 总公司工商营业执照
        /// </summary>
        public string IndustrialACBLOTOfficeId { get; set; }
        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人身份证号码
        /// </summary>
        public string ContactIDNumber { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactTelephone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Landline { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mailbox { get; set; }
        /// <summary>
        /// 邮箱验证码
        /// </summary>
        public string EmailVerificationCode { get; set; }
        /// <summary>
        /// 备案申请表
        /// </summary>
        public string FilingApplicationFormId { get; set; }
        /// <summary>
        /// 服务格式条款
        /// </summary>
        public string StandardTOServiceId { get; set; }
        /// <summary>
        /// 经营场所产权证明
        /// </summary>
        public string SiteMaterialsId { get; set; }
        /// <summary>
        /// 经营场所照片
        /// </summary>
        public string PhotosOBPremisesId { get; set; }
        /// <summary>
        /// 人员岗位分工明细
        /// </summary>
        public string PersonnelPDMaterialsId { get; set; }
        /// <summary>
        /// 人员社保证明
        /// </summary>
        public string PersonnelSSCertificateId { get; set; }
        /// <summary>
        /// 设备安装维护制度
        /// </summary>
        public string EquipmentIMaterialsId { get; set; }
        /// <summary>
        /// 监护人员管理制度
        /// </summary>
        public string SupervisorMaterialsId { get; set; }
        /// <summary>
        /// 违规处理和统计分析制度
        /// </summary>
        public string ViolationsASMaterialsId { get; set; }
        /// <summary>
        /// 监控系统符合相关标准的声明
        /// </summary>
        public string MonitoringSystemMaterialsId { get; set; }
        /// <summary>
        /// 监控系统的安全等级证明材料
        /// </summary>
        public string SafetyGradeMaterialsId { get; set; }
        /// <summary>
        /// 服务商备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public  int? RegistrationStatus { get; set; }
        /// <summary>
        /// 驳回意见
        /// </summary>
        public  string FinalRejection { get; set; }
        /// <summary>
        /// 验证码redis
        /// </summary>
        public string RedisId { get; set; }

    }

    public class ServiceProviderSetDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string SetOrganizationName { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string SetUnifiedSocialCreditCode { get; set; }
        /// <summary>
        /// 工商营业执照
        /// </summary>
        public string SetIndustrialACBLicenseId { get; set; }
        /// <summary>
        /// 总公司工商营业执照
        /// </summary>
        public string SetIndustrialACBLOTOfficeId { get; set; }
        /// <summary>
        /// 经营区域
        /// </summary>
        public string SetJingYingFanWei { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string SetContactName { get; set; }
        /// <summary>
        /// 联系人身份证号码
        /// </summary>
        public string SetContactIDNumber { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string SetContactTelephone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string SetLandline { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string SetMailbox { get; set; }
        /// <summary>
        /// 邮箱验证码
        /// </summary>
        public string SetEmailVerificationCode { get; set; }
        /// <summary>
        /// 备案申请表
        /// </summary>
        public string SetFilingApplicationFormId { get; set; }
        /// <summary>
        /// 服务格式条款
        /// </summary>
        public string SetStandardTOServiceId { get; set; }
        /// <summary>
        /// 经营场所产权证明
        /// </summary>
        public string SetSiteMaterialsId { get; set; }
        /// <summary>
        /// 经营场所照片
        /// </summary>
        public string SetPhotosOBPremisesId { get; set; }
        /// <summary>
        /// 人员岗位分工明细
        /// </summary>
        public string SetPersonnelPDMaterialsId { get; set; }
        /// <summary>
        /// 人员社保证明
        /// </summary>
        public string SetPersonnelSSCertificateId { get; set; }
        /// <summary>
        /// 设备安装维护制度
        /// </summary>
        public string SetEquipmentIMaterialsId { get; set; }
        /// <summary>
        /// 监护人员管理制度
        /// </summary>
        public string SetSupervisorMaterialsId { get; set; }
        /// <summary>
        /// 违规处理和统计分析制度
        /// </summary>
        public string SetViolationsASMaterialsId { get; set; }
        /// <summary>
        /// 监控系统符合相关标准的声明
        /// </summary>
        public string SetMonitoringSystemMaterialsId { get; set; }
        /// <summary>
        /// 监控系统的安全等级证明材料
        /// </summary>
        public string SetSafetyGradeMaterialsId { get; set; }
        /// <summary>
        /// 服务商备注
        /// </summary>
        public string SetRemark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? RegistrationStatus { get; set; }
        /// <summary>
        /// 驳回意见
        /// </summary>
        public string FinalRejection { get; set; }

        public int CompanyTypes { get; set; }

        public string OrgCode { get; set; }

    }


    public class GetEmailDto
    {
        //邮箱
        public string Email { get; set; }
        //验证码id
        public string VerificationCodeId { get; set; }
        //Redis id
        public string RedisId { get; set; }
        /// <summary>
        /// 注册类型
        /// </summary>
        public int Type { get; set; }

        public string EmailCode { get; set; }
    }


    public class MaterialDocuments
    {  
        /// <summary>
        /// 工商营业执照
        /// </summary>
        public string IndustrialACBLicenseId { get; set; }

        public string IndustrialACBLOTOfficeId { get; set; }

        /// <summary>
        /// 备案申请表
        /// </summary>
        public string FilingApplicationFormId { get; set; }
        /// <summary>
        /// 服务格式条款
        /// </summary>
        public string StandardTOServiceId { get; set; }
        /// <summary>
        /// 经营场所产权证明
        /// </summary>
        public string SiteMaterialsId { get; set; }
        /// <summary>
        /// 经营场所照片
        /// </summary>
        public string PhotosOBPremisesId { get; set; }
        /// <summary>
        /// 人员岗位分工明细
        /// </summary>
        public string PersonnelPDMaterialsId { get; set; }
        /// <summary>
        /// 人员社保证明
        /// </summary>
        public string PersonnelSSCertificateId { get; set; }
        /// <summary>
        /// 设备安装维护制度
        /// </summary>
        public string EquipmentIMaterialsId { get; set; }
        /// <summary>
        /// 监护人员管理制度
        /// </summary>
        public string SupervisorMaterialsId { get; set; }
        /// <summary>
        /// 违规处理和统计分析制度
        /// </summary>
        public string ViolationsASMaterialsId { get; set; }
        /// <summary>
        /// 监控系统符合相关标准的声明
        /// </summary>
        public string MonitoringSystemMaterialsId { get; set; }
        /// <summary>
        /// 监控系统的安全等级证明材料
        /// </summary>
        public string SafetyGradeMaterialsId { get; set; }
    }
}
