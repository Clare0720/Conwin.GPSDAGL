using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Entities
{
    public partial class DaoLuYunShuCongYeRenYuanTaiZhang : EntityMetadata
    {
        public string XingMing { get; set; }
        public string XingBie { get; set; }
        public string ShenFenZhengHaoMa { get; set; }
        public DateTime? CongYeZiGeZhengYouXiaoRiQi { get; set; }
        public string ZhaoPian { get; set; }
        public string ZhuangTai { get; set; }
    }
}
