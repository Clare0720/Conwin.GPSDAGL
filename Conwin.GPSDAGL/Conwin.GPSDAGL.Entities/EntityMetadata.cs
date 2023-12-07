using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class EntityMetadata : BaseEntity 
    {
        public string SYS_ShuJuLaiYuan { get; set; }
        public string SYS_ChuangJianRenID { get; set; }
        public string SYS_ChuangJianRen { get; set; }
        public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }
        public string SYS_ZuiJinXiuGaiRenID { get; set; }
        public string SYS_ZuiJinXiuGaiRen { get; set; }
        public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }
        public Nullable<int> SYS_XiTongZhuangTai { get; set; }
        public string SYS_XiTongBeiZhu { get; set; }
    }
}
