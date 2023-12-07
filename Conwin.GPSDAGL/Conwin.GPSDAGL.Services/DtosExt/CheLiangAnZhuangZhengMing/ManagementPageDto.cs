using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing
{
    /// <summary>
    /// 查询安装证明列表查询参数
    /// </summary>
    public class QueryListRequestDto
    {
        /// <summary>
        /// 业户名称
        /// </summary>
        public string YeHuMingCheng { get; set; }
        /// <summary>
        /// 证明编号
        /// </summary>
        public string ZhengMingBianHao { get; set; }
        /// <summary>
        /// 证明类型
        /// </summary>
        public int? ZhengMingLeiXin { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
    }

    /// <summary>
    /// 查询安装证明列表响应参数
    /// </summary>
    public class QueryListResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 证明编号
        /// </summary>
        public string ZhengMingBianHao { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ChuangJianShiJian { get; set; }
        /// <summary>
        /// 证明类型
        /// </summary>
        public int? ZhengMingLeiXin { get; set; }
        /// <summary>
        /// 证明文件附件ID(无章版本)
        /// </summary>
        public Guid? ZhengMingFileId { get; set; }
        /// <summary>
        /// 证明文件附件ID(有章版本)
        /// </summary>
        public string ShuiYinPDFFileId { get; set; }
        /// <summary>
        /// 车辆辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 车辆辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 车辆业户名称
        /// </summary>
        public string YeHuMingCheng { get; set; }
    }

    public class ExportResponseDto
    {
        public string File { get; set; }
    }
}
