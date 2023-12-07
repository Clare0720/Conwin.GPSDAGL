using NPOI.SS.Formula.Functions;
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
    public class FuWuShangJieRuRequestDto
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
        /// 车辆种类
        /// </summary>
        public int? CheLiangZhongLei { get; set; }
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
    }

    public class QueryFuWuShangJieRuDto
    {
        public Guid? CheLiangId { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int? CheliangZhongLei { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 服务商代码
        /// </summary>
        public string FuWuShangCode { get; set; }
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string FuWuShangOrgName { get; set; }
        /// <summary>
        /// 业户代码
        /// </summary>
        public string YeHuOrgCode { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string YeHuOrgName { get; set; }
        /// <summary>
        /// 备案状态
        /// </summary>
        public int? BeiAnZhuangTai { get; set; }
    }


    /// <summary>
    /// 列表查询响应
    /// </summary>
    public class FuWuShangResponseDto
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
        /// 设备服务商
        /// </summary>
        public string FuWuShangOrgName { get; set; }
        /// <summary>
        /// 服务商代码
        /// </summary>
        public string FuWuShangCode { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int? CheliangZhongLei { get; set; }
        /// <summary>
        /// 业户数量
        /// </summary>
        public int? YeHuNumber { get; set; }
        /// <summary>
        /// 车辆总数
        /// </summary>
        public int? CheLiangNumber { get; set; }
        /// <summary>
        /// 车辆接入率
        /// </summary>
        public decimal? CheLiangJieRuLv { get; set; }
        /// <summary>
        /// 已接入车辆数
        /// </summary>
        public int? JieRuCheLiangShu { get; set; }
        /// <summary>
        /// 通过备案车辆数
        /// </summary>
        public int? TongGuoBeiAnCheLiangShu { get; set; }
        /// <summary>
        /// 未通过备案车辆数
        /// </summary>
        public int? WeiTongGuoBeiAnCheLiangShu { get; set; }
        /// <summary>
        /// 未审核车辆数
        /// </summary>
        public int? WeiShengHeCheLiangShu { get; set; }
        /// <summary>
        /// 取消备案车辆数
        /// </summary>
        public int? QuXiaoBeiAnShu { get; set; }

    }

    public class GetFuWuShangJieRuXinXiDto
    {
        public int Count { get; set; }

        public List<FuWuShangResponseDto> list { get; set; }
    }

}
