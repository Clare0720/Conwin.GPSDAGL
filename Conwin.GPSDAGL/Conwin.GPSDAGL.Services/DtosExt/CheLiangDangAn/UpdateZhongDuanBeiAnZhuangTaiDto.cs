using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
   public class UpdateZhongDuanBeiAnZhuangTaiDto
    {
        public Guid? cheliangId { get; set; }

        [Description("备案状态")]
        public int? beiAnZhuangTai { get; set; }
        [Description("备注信息")]
        public string CancelRecordRemark { get; set; }
    }
}
