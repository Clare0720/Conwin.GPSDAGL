using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class QiYeDataSynDto
    {
        public string OrgCode { get; set; }
    }


    [DataContract(IsReference = true)]
    public class ImportFuWu
    {
        [DataMember(EmitDefaultValue = true)]
        public string Field { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string Applicant { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string Reason { get; set; }
    }

    [DataContract(IsReference = true)]
    public class ImportVehicleContactInfoDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string YeHuMingCheng { get; set; }
        /// <summary>
        /// 企业代码
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string YeHuDaiMa { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string DiZhi { get; set; }
        /// <summary>
        /// 经营许可证
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string JingYingXuKeZhengHao { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string LianXiRen { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string LianXiDianHua { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string ChuanZhen { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string SYS_XiTongZhuangTai { get; set; }

    }

    /// <summary>
    /// 运政企业编号数据表
    /// </summary>
    public class EnterpriseSynchronizationTable
    {
        public Guid Id { get; set; }
        public string OrgCode { get; set; }
        public string EnterpriseId { get; set; }
        public string OrgName { get; set; }
    }
    public class AddOrUpdateQiYeDangAn
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 组织代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public string OrgShortName { get; set; }
        /// <summary>
        /// 经营范围(经营区域)
        /// </summary>
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string YeWuJingYingFanWei { get; set; }
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
        /// 地址
        /// </summary>
        public string DiZhi { get; set; }
        /// <summary>
        /// 营运状态
        /// </summary>
        public string ZhuangTai { get; set; }
        /// <summary>
        /// 经营许可证号
        /// </summary>
        public string JingYingXuKeZhengHao { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LianXiRen { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LianXiDianHua { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string ChuanZhen { get; set; }
        /// <summary>
        /// 经济类型
        /// </summary>
        public string JingJiLeiXing { get; set; }

        public string QiYeXingZhi { get; set; }
    }




    [DataContract(IsReference = true)]
    public class ImportVehicleAppointmentDto
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string YeHuMingCheng { get; set; }
        /// <summary>
        /// 辖区省
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string JingYingFanWei { get; set; }
        /// <summary>
        /// 企业性质
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string QiYeXingZhi { get; set; }
    }
}
