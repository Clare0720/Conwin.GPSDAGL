using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi : EntityMetadata 
    {
        public Nullable<System.Guid> CheLiangID { get; set; }
        public Nullable<System.Guid> ZhongDuanID { get; set; }
        public Nullable<int> XieYiLeiXing { get; set; }
        public Nullable<int> ZhuaBaoLaiYuan { get; set; }
        public Nullable<int> BanBenHao { get; set; }
    }
}
