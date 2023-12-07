using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class ExportCheliangXinXiDto
    {
        public string FileId { get; set; }
    }


    public class ExportCheliangXinXiRequestDto
    {
        public Dictionary<string, string> Cols { get; set; }
    }
}
