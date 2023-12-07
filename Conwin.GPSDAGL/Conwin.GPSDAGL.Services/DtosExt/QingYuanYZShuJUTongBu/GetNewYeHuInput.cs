using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu
{
    public class GetNewYeHuInput
    {
        /// <summary>
        /// 起始最新更新时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 截至最新更新时间
        /// </summary>
        public DateTime EndTime { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
