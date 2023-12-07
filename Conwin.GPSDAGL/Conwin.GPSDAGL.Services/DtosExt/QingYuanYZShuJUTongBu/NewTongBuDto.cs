using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu
{
    public class QiYeInfoResponseDto
    {
        /// <summary>
        /// 业户电脑编号
        /// </summary>
        public string YeHuDianNaoBianHao { get; set; }
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
        /// 邮编
        /// </summary>
        public string YouBian { get; set; }
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
        /// <summary>
        /// 状态
        /// </summary>
        public string ZhuangTai { get; set; }
        /// <summary>
        /// 创建人员
        /// </summary>
        public string Sys_XinZengRen { get; set; }
        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime? Sys_XinZengShiJian { get; set; }
        /// <summary>
        /// 最近修改人
        /// </summary>
        public string Sys_ZuiJinXiuGaiRen { get; set; }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? Sys_ZuiJinXiuGaiShiJian { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public string ShuJuZhuangTai { get; set; }

    }

    public class QiYeInfoQueryDto
    {
        /// <summary>
        /// 业户代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
    }


    public class VehicleQueryDto
    {

        public List<VheicleQueryInfoDto> VehicleList { get; set; }
    }

    public class VheicleQueryInfoDto
    {
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }

    }


    public class VeheiclQueryResponseDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string RegistrationNo { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string RegistrationNoColor { get; set; }
        /// <summary>
        /// 营运状态
        /// </summary>
        public string ZhuangTai { get; set; }
    }

    public class VeheiclBaseInfoQueryResponseDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string RegistrationNo { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string RegistrationNoColor { get; set; }
        /// <summary>
        /// 营运状态
        /// </summary>
        public string ZhuangTai { get; set; }
        /// <summary>
        /// 业户代码
        /// </summary>
        public string YeHuDaiMa { get; set; }
        /// <summary>
        /// XiaQuXian
        /// </summary>
        public string XiaQuXian { get; set; }

    }


    public class VehicleInformation
    {
        public int? CheLiangDianNaoBianHao { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }
        public string CheLiangZhongLei { get; set; }
        public bool IsYZ { get; set; }
        public string MDT { get; set; }
        public long M1 { get; set; }
        public long IA1 { get; set; }
        public long IC1 { get; set; }
        public string DeviceCode { get; set; }
        public string FactoryCode { get; set; }
        public string SimNo { get; set; }
        public int? TerminalType { get; set; }
        public string YeHuDaiMa { get; set; }
        public string YeHuMingCheng { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorName { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string XiaQuZhen { get; set; }
        public DateTime? FirstUploadTime { get; set; }
        public double LatestLongtitude { get; set; }
        public double LatestLatitude { get; set; }
        public DateTime LatestGpsTime { get; set; }
        public DateTime LatestRecvTime { get; set; }
        public bool HasRecvGps { get; set; }
        public string ZhuangTai { get; set; }
        public string CheJiaHao { get; set; }
        public string DeviceModel { get; set; }
        public string IMEINo { get; set; }
        public string IMEIFactoryName { get; set; }
    }

    public class VehicleConfiguration
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public string JingYingFanWei { get; set; }
    }
}
