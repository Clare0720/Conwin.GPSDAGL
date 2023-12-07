using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class VideoZhongDuanXinXiExDto
    {
        public string ChePaiHao { get; set; }

        public string ChePaiYanSe { get; set; }

        public string OrgName { get; set; }

        public string SheBeiXingHao { get; set; }

        public int? SheBeiJiShenLeiXing { get; set; }

        public string SheBeiGouCheng { get; set; }

        public string ChangJiaBianHao { get; set; }

        public string ShengChanChangJia { get; set; }

        public DateTime? AnZhuangShiJian { get; set; }

        public int ShiPinTouGeShu { get; set; }

        public int ShiPingChangShangLeiXing { get; set; }


        public string ShiPinTouAnZhuangXuanZe { get; set; }

        public Guid ZhongDuanId { get; set; }

        public Guid CheLiangId { get; set; }

        public bool? ShuJuJieRu { get; set; }

        public bool? SheBeiWanZheng { get; set; }

        public string NeiRong { get; set; }

        public List<ZhongDuanFileMapper> FileList { get; set; }

        public int? CheLiangZhongLei { get; set; }

        public string SIMKaHao { get; set; }

        public string ZhongDuanMDT { get; set; }

        public string FuWuShangMingCheng { get; set; }

        public int? BeiAnZhuangTai { get; set; }

        public Nullable<System.DateTime> LatestGpsTime { get; set; }
    }

    
}
