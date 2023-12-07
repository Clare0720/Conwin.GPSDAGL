using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing
{

    public class CarInfoHuiZong
    {
        public CarBaseInfoDto CheLiangXinXi { get; set; }
        public CarVideoZhongDuanInfoDto VideoZhongDuanXinXi { get; set; }
        public ZhongDuanConfirmInfo ZhongDuanBeiAnXinXi { get; set; }
        public CarGPSZhongDuanInfoDto GPSZhongDuanXinXi { get; set; }
        public CheLiangYeHuXinXi CheLiangYeHuXinXi { get; set; }
        public CheLiangBaoXianInfo CheLiangBaoXianXinXi { get; set; }
    }

    public class CarBaseInfoDto
    {
        public Guid? Id { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string FuWuShangOrgCode { get; set; }
        public string CheliangYeHuOrgCode { get; set; }
        public int? CheliangZhongLei { get; set; }
        public string CheJiaHao { get; set; }
    }
    public class CarVideoZhongDuanInfoDto
    {
        public Guid? Id { get; set; }
        public DateTime? AnZhuangShiJian { get; set; }
        public string SheBeiXingHao { get; set; }
        public string ShengChanChangJia { get; set; }
    }

    public class CarGPSZhongDuanInfoDto
    {
        public Guid? Id { get; set; }
        public string SIMKaHao { get; set; }

    }
    public class ZhongDuanConfirmInfo
    {
        public int? BeiAnZhuangTai { get; set; }
    }
    public class CheLiangYeHuXinXi
    {
        public Guid? Id { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }

    }

    public class CheLiangBaoXianInfo
    {
        public Guid? Id { get; set; }
        public string JiaoQiangXianOrgName { get; set; }
        public DateTime? JiaoQiangXianEndTime { get; set; }
        public string ShangYeXianOrgName { get; set; }
        public DateTime? ShangYeXianEndTime { get; set; }

    }

}
