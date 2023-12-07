using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class YongHuJianKongShu : EntityMetadata 
    {
        public string NodeId { get; set; }
        public string SysUserId { get; set; }
        public Nullable<int> Enabled { get; set; }
        public string Remark { get; set; }
    }
}
