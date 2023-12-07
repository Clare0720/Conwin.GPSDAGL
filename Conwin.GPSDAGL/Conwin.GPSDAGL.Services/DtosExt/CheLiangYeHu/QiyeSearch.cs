using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class QiyeSearch
    {
        public string YeHuMingCheng { get; set; }
        public string OrgCode { get; set; }
        public string YouXiaoZhuangTai { get; set; }
        public string LianXiRen { get; set; }
        public string ShouJiHaoMa { get; set; }
        public string ShenHeZhuangTai { get; set; }
        public int? OrgType { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string JingYingXuKeZhengHao { get; set; }

        public string RegistrationStatus { get; set; }

    }


    public class QueryEnterpriseManagement
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 状态 第三方审核中   1 第三方审核失败  2 市级审核中  3 市级审核失败 4 市级审核成功 5 市级驳回   6
        /// </summary>
        public string ZhuangTai { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

    }
}
