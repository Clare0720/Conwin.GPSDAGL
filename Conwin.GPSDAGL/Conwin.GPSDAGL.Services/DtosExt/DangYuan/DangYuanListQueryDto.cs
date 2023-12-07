using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Services.Dtos;
using System.Runtime.Serialization;

namespace Conwin.GPSDAGL.Services.DtosExt.RenYuan
{
    public class DangYuanListQueryDto
    {
        public string QiYeMingCheng { get; set; }
        public string Name { get; set; }
        public string IDCard { get; set; }
    }

}
