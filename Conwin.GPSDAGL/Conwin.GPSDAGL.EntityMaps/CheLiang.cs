using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiang : EntityMetadata 
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<int> CheLiangLeiXing { get; set; }
        public Nullable<int> CheLiangZhongLei { get; set; }
        public string CheZaiDianHua { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public Nullable<int> YeWuBanLiZhuangTai { get; set; }
        public Nullable<int> ShiFouGuFei { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public string YeHuOrgCode { get; set; }
        public Nullable<int> YeHuOrgType { get; set; }
        public string CheDuiOrgCode { get; set; }
        public Nullable<int> ZaiXianZhuangTai { get; set; }
        public Nullable<int> YingYunZhuangTai { get; set; }
        public Nullable<int> FuWuZhuangTai { get; set; }
    }
}
