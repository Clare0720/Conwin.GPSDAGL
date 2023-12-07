using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangJianKongShuEx : EntityMetadata 
    {
        public string CheLiangId { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public string NodeId { get; set; }
        public string NodeName { get; set; }
    }
}
