using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class DangYuan : EntityMetadata 
    {
        public string Name { get; set; }
        public Nullable<int> Sex { get; set; }
        public string IDCard { get; set; }
        public string NativePlace { get; set; }
        public Nullable<int> Nation { get; set; }
        public Nullable<int> Education { get; set; }
        public string Degree { get; set; }
        public string ContactNumber { get; set; }
        public string OrgCode { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string DangZuZhiSuoZaiDi { get; set; }
        public string LiuDongDangYuan { get; set; }
    }
}
