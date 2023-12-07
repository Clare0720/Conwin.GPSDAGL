using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class ZhongDuanFileMapper : EntityMetadata 
    {
        public Nullable<System.Guid> ZhongDuanId { get; set; }
        public string FileId { get; set; }
        public Nullable<int> FileType { get; set; }
    }
}
