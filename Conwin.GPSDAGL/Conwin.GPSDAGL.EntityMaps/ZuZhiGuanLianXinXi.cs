using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class ZuZhiGuanLianXinXi : EntityMetadata 
    {
        public Nullable<int> RelationOrgType { get; set; }
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string RelationOrgCode { get; set; }
    }
}
