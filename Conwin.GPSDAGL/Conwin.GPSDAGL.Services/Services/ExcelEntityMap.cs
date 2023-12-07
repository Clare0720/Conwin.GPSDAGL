using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class ExcelEntityMap
    {
        private bool _isNeedOrder = true;
        /// <summary>
        /// 列名
        /// </summary>
        public string DestinaName { get; set; }
        /// <summary>
        /// 数据源名
        /// </summary>
        public string SourceName { get; set; }
    }
}
