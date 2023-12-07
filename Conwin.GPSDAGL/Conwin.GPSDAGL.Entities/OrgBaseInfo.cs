using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class OrgBaseInfo : EntityMetadata 
    {
        public string OrgCode { get; set; }
        public Nullable<int> OrgType { get; set; }
        public string OrgShortName { get; set; }
        public string OrgName { get; set; }
        public Nullable<System.Guid> ParentOrgId { get; set; }
        public string JingYingFanWei { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string DiZhi { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public string Remark { get; set; }
        public string Street { get; set; }
        public string YeWuJingYingFanWei { get; set; }
    }
}
