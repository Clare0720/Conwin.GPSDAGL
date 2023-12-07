using Conwin.GPSDAGL.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class CheDuiSearchDto
    {
        public string JiGouMingCheng { get; set; }
        public string CheDuiDaiMa { get; set; }
        public string LianXiRen { get; set; }
        public string ShouJiHaoMa { get; set; }
        public string ShenHeZhuangTai { get; set; }
        public string YouXiaoZhuangTai { get; set; }
    }

    public class CheDuiExDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string JiGouMingCheng { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string JiGouJianCheng { get; set; }

        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }

        /// <summary>
        /// 有效状态
        /// </summary>
        public int? YouXiaoZhuangTai { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ShenHeZhuangTai { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }

        /// <summary>
        /// 车队代码
        /// </summary>
        public string CheDuiDaiMa { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string FuZheRen { get; set; }
        /// <summary>
        /// 负责人 电话
        /// </summary>
        public string FuZheRenDianHua { get; set; }
    }

    public class SystemOrgDetailDto
    {
        public string Id { get; set; }
        public string SysID { get; set; }
        public string OrganizationName { get; set; }
        public int? OrganizationType { get; set; }
        public string OrganizationCode { get; set; }
        public string ManageArea { get; set; }
    }
}
