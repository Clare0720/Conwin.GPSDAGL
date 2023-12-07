using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangZuZhiXinXi : EntityMetadata 
    {
        public string CheLiangId { get; set; }
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
    }
}
