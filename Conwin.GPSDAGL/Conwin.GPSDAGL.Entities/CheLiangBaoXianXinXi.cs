using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangBaoXianXinXi : EntityMetadata 
    {
        public Nullable<System.Guid> CheLiangId { get; set; }
        public string JiaoQiangXianOrgName { get; set; }
        public Nullable<System.DateTime> JiaoQiangXianEndTime { get; set; }
        public string ShangYeXianOrgName { get; set; }
        public Nullable<System.DateTime> ShangYeXianEndTime { get; set; }
    }
}
