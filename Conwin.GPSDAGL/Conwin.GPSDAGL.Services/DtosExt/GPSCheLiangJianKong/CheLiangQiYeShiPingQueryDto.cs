using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.GPSCheLiangJianKong
{
    public class CheLiangQiYeShiPingDto
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public int CheLiangZhongLei { get; set; }
        public string XiaQuShi { get; set; }
        public string QiYeMingCheng { get; set; }
        public string ParentOrgName { get; set; }
        public Nullable<int> ShiPingTouGeShu { get; set; }

        public string CameraSelected { get; set; }
        public Nullable<int> VideoServiceKind { get; set; }

        public string GeRenCheZhuMingCheng { get; set; }
    }

    public class CheLiangQiYeShiPingQueryDto
    {
        public bool GovermentFlag { get; set; }
        public string QueryType { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public Nullable<int> OrganizationType { get; set; }
        public string QiYeMingCheng { get; set; }
        public string YunYingShangMingCheng { get; set; }
        public string YunYingShangBianHao { get; set; }
        public string QiYeOrgCode { get; set; }
        public string OrgCode { get; set; }
        public string SysUserId { get; set; }
        public string RoleCode { get; set; }
        public string ChePaiHao { get; set; }
        public string OrgDistrict { get; set; }
        public string OrgId { get; set; }
        public Nullable<int> IsMobile { get; set; }
    }
}
