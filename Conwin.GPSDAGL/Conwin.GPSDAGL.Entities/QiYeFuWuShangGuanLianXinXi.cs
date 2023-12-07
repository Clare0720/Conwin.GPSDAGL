using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class QiYeFuWuShangGuanLianXinXi : EntityMetadata 
    {
        public string QiYeCode { get; set; }
        public string FuWuShangCode { get; set; }
        public string ZhuLianLuIP { get; set; }
        public Nullable<int> ZhuLianLuDuanKou { get; set; }
        public string CongLianLuIP { get; set; }
        public Nullable<int> CongLianLuDuanKou { get; set; }
        public string XiaQuSheng { get; set; }
        public string PingTaiJieRuMa { get; set; }
        public string LoginName { get; set; }
        public string LoginPassWord { get; set; }
        public Nullable<long> M1 { get; set; }
        public Nullable<long> IA1 { get; set; }
        public Nullable<long> IC1 { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
    }
}
