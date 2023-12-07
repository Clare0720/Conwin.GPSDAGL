using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangJieRu
{
    /// <summary>
    /// 列表查询请求
    /// </summary>
    public class ZhiNengShiPingJieRuRequestDto
    {
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 设备服务商名称
        /// </summary>
        public string FuWuShangName { get; set; }
        /// <summary>
        /// 设备安装日期起
        /// </summary>
        public DateTime? AnZhuangRiQiBegin { get; set; }
        /// <summary>
        /// 设备安装日期止
        /// </summary>
        public DateTime? AnZhuangRiQiEnd { get; set; }
        /// <summary>
        /// 交强险保险公司名称
        /// </summary>
        public string JiaoQiangXianOrgName { get; set; }
        /// <summary>
        /// 商业险保险公司名称
        /// </summary>
        public string ShangYeXianOrgName { get; set; }
    }

    /// <summary>
    /// 列表查询响应
    /// </summary>
    public class ZhiNengShiPingJieRuResponseDto
    {
        public Guid? CheLiangId { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string CheJiaHao { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
        public string FaDongJiHao { get; set; }
        /// <summary>
        /// 个体户身份证号
        /// </summary>
        public string GeTiHuIDCard { get; set; }
        /// <summary>
        /// 业户统一社会信用代码
        /// </summary>
        public string SheHuiXinYongDaiMa { get; set; }
        /// <summary>
        /// 业户负责人姓名
        /// </summary>
        public string YeHuPrincipalName { get; set; }

        /// <summary>
        /// 业户负责人联系方式
        /// </summary>
        public string YeHuPrincipalPhone { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 司机联系方式
        /// </summary>
        public string DriverPhone { get; set; }

        /// <summary>
        /// 从业资格证号
        /// </summary>
        public string CongYeZiGeZhengHao { get; set; }

        /// <summary>
        /// 智能视频生产厂家
        /// </summary>
        public string VideoShengChanChangJia { get; set; }
        /// <summary>
        /// 智能视频设备型号
        /// </summary>
        public string VideoSheBeiXingHao { get; set; }
        /// <summary>
        /// SIM卡号
        /// </summary>
        public string SimKaHao { get; set; }
        /// <summary>
        /// 设备服务商
        /// </summary>
        public string FuWuShangOrgName { get; set; }
        /// <summary>
        /// 设备安装人员
        /// </summary>
        public string SheBeiAnZhuangRenYuanName { get; set; }
        /// <summary>
        /// 设备安装人员联系电话
        /// </summary>
        public string SheBeiAnZhuangRenYuanPhone { get; set; }
        /// <summary>
        /// 终端安装时间
        /// </summary>
        public DateTime? AnZhuangShiJian { get; set; }

        /// <summary>
        /// 集中安装点名称
        /// </summary>
        public string JiZhongAnZhuangDianMingCheng { get; set; }
        /// <summary>
        /// 交强险保险公司名称
        /// </summary>
        public string JiaoQiangXianOrgName { get; set; }
        /// <summary>
        /// 商业险保险公司名称
        /// </summary>
        public string ShangYeXianOrgName { get; set; }
    }

    /// <summary>
    /// 列表导出响应
    /// </summary>
    public class ExportZhiNengShiPingJieRuDto
    {
        public string FileId { get; set; }
    }
    /// <summary>
    /// 列表导出请求
    /// </summary>
    public class ExportZhiNengShiPingJieRuRequestDto
    {
        public Dictionary<string, string> Cols { get; set; }
    }

    public class GetCheLiangJieRuXinXiDto
    {
        public int Count { get; set; }

        public List<ZhiNengShiPingJieRuResponseDto> list { get; set; }
    }
}
