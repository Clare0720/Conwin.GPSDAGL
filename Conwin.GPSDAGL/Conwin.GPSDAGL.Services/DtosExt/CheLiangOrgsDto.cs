using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class CheLiangOrgsInputDto
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
    }

    public class CheLiangOrgsOutputDto
    {
        public List<string> OrgCode { get; set; }
        public List<string> SysUserId { get; set; }
    }
}
