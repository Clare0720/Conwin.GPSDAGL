using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang
{
    /// <summary>
    /// 车辆报警事件查询请求
    /// </summary>
    public class CheLiangBaoJingQueryDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 查询指定的事件类型，多个以英文逗号分割
        /// </summary>
        public string EventTypeCode { get; set; }
        /// <summary>
        /// 事件开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
    }

    /// <summary>
    /// 车辆报警事件查询响应
    /// </summary>
    public class CheLiangBaoJingResDto
    {
        /// <summary>
        /// 事件类型编码
        /// </summary>
        public string EventTypeCode { get; set; }
        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; set; }
        /// <summary>
        /// 事件开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
    }
}
