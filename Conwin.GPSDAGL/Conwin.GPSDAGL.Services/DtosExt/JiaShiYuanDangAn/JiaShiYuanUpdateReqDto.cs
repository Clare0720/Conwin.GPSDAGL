using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案 修改驾驶员 请求数据
    /// </summary>
    public class JiaShiYuanUpdateReqDto : JiaShiYuanCreateReqDto
    {
        /// <summary>
        /// 驾驶员ID，必填项
        /// </summary>
        public string Id { get; set; }
    }
}
