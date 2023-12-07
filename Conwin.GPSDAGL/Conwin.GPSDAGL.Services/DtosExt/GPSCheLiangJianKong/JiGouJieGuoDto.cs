using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.DtosExt
{

    /// <summary>
    /// 机构结果DTO
    /// </summary>
    public class JiGouResponseDto
    {
        public string OrgId { get; set; }
        public string OrgType { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string ParentOrgId { get; set; }
        public string ParentOrgCode { get; set; }
        public int OrgZaiXianCheLiangShu { get; set; }
        public int OrgZongCheLiangShu { get; set; }
    }


    /// <summary>
    /// 机构结果DTO
    /// </summary>
    public class JiGouJieGuoDto
    {
        public string OrgCode { get; set; }

        public int OrgType { get; set; }
        public string OrgName { get; set; }
        public int OrgZaiXianCheLiangShu { get; set; }
        public int OrgZongCheLiangShu { get; set; }
        public string ParentOrgCode { get; set; }

        public List<CheLiangItem> Items { get; set; }

    }

    public class OrgItem
    {
        public string OrgCode { get; set; }
        public int OrgType { get; set; }
        public string OrgName { get; set; }
        public int OrgZaiXianCheLiangShu { get; set; }
        public int OrgZongCheLiangShu { get; set; }
        public string ParentOrgCode { get; set; }
    }

    public class CheLiangItem
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public int? CheLiangZhongLei { get; set; }
        public bool ShiFouZaiXian { get; set; }
    }


    public class JianKongShuResult
    {
        public int NodeType { get; set; }
        public dynamic Items { get; set; }
    }
}
