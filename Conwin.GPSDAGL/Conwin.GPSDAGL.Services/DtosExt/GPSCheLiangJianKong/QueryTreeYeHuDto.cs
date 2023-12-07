using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 获取车辆监控树企业搜索列表 请求参数
    /// </summary>
    public class QueryTreeYeHuReqDto
    {
        public string OrgName { get; set; }
    }

    public class JianKongTreeYeHuListQueryDto
    {
        public int? OrgType { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public int? ZaiXianZhuangTai { get; set; }
    }

    public class QueryTreeYeHuListDto
    {
        public int? OrgType { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public int OrgZaiXianCheLiangShu { get; set; }
        public int OrgZongCheLiangShu { get; set; }
        public string ParentOrgCode { get; set; }
    }
}
