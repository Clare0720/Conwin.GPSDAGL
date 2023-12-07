using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu
{
    public class TongBuOutput<TData>
    {
        /// <summary>
        /// 起始值，可能是id，也可能是时间
        /// </summary>
        public TData Data { get; set; }
    }
}
