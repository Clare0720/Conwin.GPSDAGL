using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class FuWuShangCheLiangVideoZhongDuanXinXi : EntityMetadata 
    {
        public string FuWuShangCheLiangId { get; set; }
        public string ZhongDuanMDT { get; set; }
        public string SheBeiXingHao { get; set; }
        public Nullable<int> SheBeiJiShenLeiXing { get; set; }
        public string SheBeiGouCheng { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string ShengChanChangJia { get; set; }
        public Nullable<System.DateTime> AnZhuangShiJian { get; set; }
    }
}
