using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class ZiDingYiCheLiangJianKongShu : EntityMetadata 
    {
        public string NodeName { get; set; }
        public Nullable<System.Guid> ParentNodeId { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public Nullable<int> NodeType { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string Remark { get; set; }
        public string NodeData { get; set; }
    }
}
