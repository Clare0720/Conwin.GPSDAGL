using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class ZuZhiGongZhangXinXi : EntityMetadata 
    {
        public string ChuangJianDanWeiOrgCode { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }
        public Nullable<System.Guid> GongZhangZhaoPianId { get; set; }
    }
}
