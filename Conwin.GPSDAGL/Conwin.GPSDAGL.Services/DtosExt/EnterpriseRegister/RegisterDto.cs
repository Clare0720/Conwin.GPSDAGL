using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister
{
    /// <summary>
    /// 第三方机构注册提交内容
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 业户代码
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
        /// 经营许可证
        /// </summary>
        public string JingYingXuKeZheng { get; set; }
        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人身份证号
        /// </summary>
        public string ContactIDCard { get; set; }
        /// <summary>
        /// 联系人联系方式
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 联系人邮件地址
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
        /// 邮箱验证码
        /// </summary>
        public string MailVerificationCode { get; set; }

        /// <summary>
        /// 验证码redis
        /// </summary>
        public string RedisId { get; set; }
    }


    public class QueryQiYeListDto
    {
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 组织代码
        /// </summary>
        public string OrgCode { get; set; }

    }

    /// <summary>
    /// 查询清远地市企业响应视图模型
    /// </summary>
    public class EnterpriseInfoResponseDto
    {

        /// <summary>
        /// 业户代码
        /// </summary>
        public string YeHuDaiMa { get; set; }
        /// <summary>
        /// 业户简称
        /// </summary>
        public string YeHuJianCheng { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string YeHuMingCheng { get; set; }
        /// <summary>
        /// 经营许可证字
        /// </summary>
        public string JingYingXuKeZhengZi { get; set; }
        /// <summary>
        /// 经营许可证号
        /// </summary>
        public string JingYingXuKeZhengHao { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string JiGouDaiMa { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public string GongSiLeiXing { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 通信地址
        /// </summary>
        public string TongXinDiZhi { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 经济类型
        /// </summary>
        public string JingJiLeiXing { get; set; }
        /// <summary>
        /// 法定代表人
        /// </summary>
        public string FaDingDaiBiaoRen { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LianXiRen { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LianXiDianHua { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string YiDongDianHua { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string ChuanZhen { get; set; }
        /// <summary>
        /// 公司负责人
        /// </summary>
        public string GongSiFuZeRen { get; set; }
        /// <summary>
        /// 货运企业类型
        /// </summary>
        public string HuoYunQiYeLeiXing { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 辖区镇
        /// </summary>
        public string XiaQuZhen { get; set; }

        public DateTime ChuangJianShiJian { get; set; }
        /// <summary>
        /// 企业性质
        /// </summary>
        public string QiYeXingZhi { get; set; }
    }

    /// <summary>
    /// 查询审核资料响应模型
    /// </summary>
    public class EnterpriseRegisterInfoDto
    {
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 业户代码
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
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string YouXiang { get; set; }
        /// <summary>
        /// 是否选择第三方机构
        /// </summary>
        public bool? ShiFouXuanZeDiSanFang { get; set; }

    }


    public class QueryEnterpriseResponseDto
    {
        public List<EnterpriseInfoResponseDto> EnterpriseList { get; set; }

        public List<EnterpriseRegisterInfoDto> RegisterInfoList { get; set; }
    }

}
