using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister
{
    /// <summary>
    /// 获取企业注册填写信息视图模型
    /// </summary>
    public class ApprovalInfoDto
    {
        /// <summary>
        /// 业户基本信息
        /// </summary>
        public EnterpriseBaseInfoDto EnterpriseBaseInfo { get; set; }
        /// <summary>
        /// 业户联系信息
        /// </summary>
        public EnterpriseContactDto EnterpriseContactInfo { get; set; }
        /// <summary>
        /// 业户责任人信息
        /// </summary>
        public EnterprisePrincipalDto EnterprisePrincipalInfo { get; set; }
        /// <summary>
        /// /业户营业执照信息
        /// </summary>
        public EnterpriseBusinessLicenseDto EnterpriseBusinessLicenseInfo { get; set; }
        /// <summary>
        /// 业户经营许可证信息
        /// </summary>
        public EnterpriseBusinessPermitDto EnterpriseBusinessPermitInfo { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ApprovalStatus { get; set; }
        /// <summary>
        /// 审核备注信息
        /// </summary>
        public string ApprovalRemark { get; set; }
    }
    /// <summary>
    /// 基本信息
    /// </summary>
    public class EnterpriseBaseInfoDto
    {

        /// <summary>
        /// 企业名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
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
        /// <summary>
        /// 经营范围
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 业务范围
        /// </summary>
        public string YeWuJingYingFanWei { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 业户类型
        /// </summary>
        public int? EnterpriseType { get; set; }
        /// <summary>
        /// 监控类型
        /// </summary>
        public int? MonitorType { get; set; }

    }

    /// <summary>
    /// 业户联系信息
    /// </summary>
    public class EnterpriseContactDto
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人身份证号
        /// </summary>
        public string ContactIDCard { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 联系人邮箱地址
        /// </summary>
        public string ContactEMail { get; set; }
        /// <summary>
        /// 联系人身份证正面照片
        /// </summary>
        public string ContactIDCardFrontId { get; set; }
        /// <summary>
        /// 联系人身份证反面照片
        /// </summary>
        public string ContactIDCardBackId { get; set; }
    }
    /// <summary>
    /// 业户责任人信息
    /// </summary>
    public class EnterprisePrincipalDto
    {
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 责任人身份证号码
        /// </summary>
        public string PrincipalIDCard { get; set; }
        /// <summary>
        /// 责任人手机号码
        /// </summary>
        public string PrincipalTel { get; set; }
        /// <summary>
        /// 责任人身份证正面照片
        /// </summary>
        public string PrincipalIDCardFrontId { get; set; }
        /// <summary>
        /// 责任人身份证反面照片
        /// </summary>
        public string PrincipalIDCardBackId { get; set; }

    }
    /// <summary>
    /// 业户营业执照信息
    /// </summary>
    public class EnterpriseBusinessLicenseDto
    {
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string UniformSocialCreditCode { get; set; }
        /// <summary>
        /// 营业执照照片
        /// </summary>
        public string BusinessLicenseFileId { get; set; }
    }
    /// <summary>
    /// 业户经营许可证信息
    /// </summary>
    public class EnterpriseBusinessPermitDto
    {
        /// <summary>
        /// 经营许可证号码
        /// </summary>
        public string BusinessPermitNumber { get; set; }
        /// <summary>
        /// 经营许可证有效期开始时间
        /// </summary>
        public DateTime? BusinessPermitStartDateTime { get; set; }
        /// <summary>
        /// 经营许可证有效期结束时间
        /// </summary>
        public DateTime? BusinessPermitEndDateTime { get; set; }
        /// <summary>
        /// 经营许可证核发单位
        /// </summary>
        public string BusinessPermitIssuingUnit { get; set; }
        /// <summary>
        /// 经营许可证照片
        /// </summary>
        public string BusinessPermitPhotoFileId { get; set; }
    }

    /// <summary>
    /// 企业提交注册完善信息视图模型
    /// </summary>
    public class EnterpriseApprovalSubmitInfoDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 业务范围
        /// </summary>
        public string YeWuJingYingFanWei { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 业户类型
        /// </summary>
        public int? EnterpriseType { get; set; }
        /// <summary>
        /// 监控方式
        /// </summary>
        public int? MonitorType { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人身份证号
        /// </summary>
        public string ContactIDCard { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 联系人邮箱地址
        /// </summary>
        public string ContactEMail { get; set; }
        /// <summary>
        /// 联系人身份证正面照片
        /// </summary>
        public string ContactIDCardFrontId { get; set; }
        /// <summary>
        /// 联系人身份证反面照片
        /// </summary>
        public string ContactIDCardBackId { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 责任人身份证号码
        /// </summary>
        public string PrincipalIDCard { get; set; }
        /// <summary>
        /// 责任人手机号码
        /// </summary>
        public string PrincipalTel { get; set; }
        /// <summary>
        /// 责任人身份证正面照片
        /// </summary>
        public string PrincipalIDCardFrontId { get; set; }
        /// <summary>
        /// 责任人身份证反面照片
        /// </summary>
        public string PrincipalIDCardBackId { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string UniformSocialCreditCode { get; set; }
        /// <summary>
        /// 营业执照照片
        /// </summary>
        public string BusinessLicenseFileId { get; set; }
        /// <summary>
        /// 经营许可证号码
        /// </summary>
        public string BusinessPermitNumber { get; set; }
        /// <summary>
        /// 经营许可证有效期开始时间
        /// </summary>
        public DateTime? BusinessPermitStartDateTime { get; set; }
        /// <summary>
        /// 经营许可证有效期结束时间
        /// </summary>
        public DateTime? BusinessPermitEndDateTime { get; set; }
        /// <summary>
        /// 经营许可证核发单位
        /// </summary>
        public string BusinessPermitIssuingUnit { get; set; }
        /// <summary>
        /// 经营许可证照片
        /// </summary>
        public string BusinessPermitPhotoFileId { get; set; }
    }

    /// <summary>
    /// 企业备案审核记录列表查询参数
    /// </summary>
    public class QueryEnterpriseApprovalListParamDto
    {
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
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
        /// 业户类型
        /// </summary>
        public int? EnterpriseType { get; set; }
        /// <summary>
        /// 监控方式
        /// </summary>
        public int? MonitorType { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 责任人手机号码
        /// </summary>
        public string PrincipalTel { get; set; }
        /// <summary>
        /// 经营许可证号
        /// </summary>
        public string BusinessPermitNumber { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ApprovalStatus { get; set; }
    }


    /// <summary>
    /// 主管部门查询企业审核信息列表视图模型
    /// </summary>
    public class EnterpriseApprovalListDto
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
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
        /// 业户类型
        /// </summary>
        public int? EnterpriseType { get; set; }
        /// <summary>
        /// 监控方式
        /// </summary>
        public int? MonitorType { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string PrincipalName { get; set; }
        /// <summary>
        /// 责任人手机号码
        /// </summary>
        public string PrincipalTel { get; set; }
        /// <summary>
        /// 经营许可证号
        /// </summary>
        public string BusinessPermitNumber { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ApprovalStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDateTime { get; set; }
    }

    public class ApprovalEnterpriseDto
    {
        public List<Guid> IdList { get; set; }
        public int ApprovalStatus { get; set; }

        public string ApprovalRemark { get; set; }


    }


}
