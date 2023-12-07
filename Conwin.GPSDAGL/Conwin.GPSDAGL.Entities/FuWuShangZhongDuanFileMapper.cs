using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShangZhongDuanFileMapper : EntityMetadata 
    {
        public Nullable<System.Guid> FuWuShangZhongDuanId { get; set; }
        public string FileId { get; set; }
        public Nullable<int> FileType { get; set; }
    }
}
