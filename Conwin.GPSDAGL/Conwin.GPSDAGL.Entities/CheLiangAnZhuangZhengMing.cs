using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangAnZhuangZhengMing : EntityMetadata 
    {
        public Nullable<System.Guid> CheLiangID { get; set; }
        public string ZhengMingBianHao { get; set; }
        public Nullable<System.Guid> GongZhangId { get; set; }
        public Nullable<int> ZhengMingLeiXin { get; set; }
        public Nullable<System.Guid> ZhengMingFileId { get; set; }
        public string ShuiYinPDFFileId { get; set; }
    }
}
