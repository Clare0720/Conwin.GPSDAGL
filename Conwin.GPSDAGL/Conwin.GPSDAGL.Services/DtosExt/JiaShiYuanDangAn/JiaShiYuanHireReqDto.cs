using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案 驾驶员聘用/解聘 请求数据
    /// </summary>
    public class JiaShiYuanHireReqDto 
    {
        /// <summary>
        /// 驾驶员ID，可多选，必填项
        /// </summary>
        public string[] Id { get; set; }
        /// <summary>
        /// 操作类型：1=聘用；2=解聘；
        /// </summary>
        public int OperationType { get; set; }
    }
}
