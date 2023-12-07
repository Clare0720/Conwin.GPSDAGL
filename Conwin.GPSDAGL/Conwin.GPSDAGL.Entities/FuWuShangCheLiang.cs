using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShangCheLiang : EntityMetadata 
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<int> CheLiangZhongLei { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string YeHuOrgCode { get; set; }
        public Nullable<int> YeHuOrgType { get; set; }
        public string FuWuShangOrgCode { get; set; }
        public string Remark { get; set; }
        public Nullable<int> BeiAnZhuangTai { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public string CheJiaHao { get; set; }
    }
}
