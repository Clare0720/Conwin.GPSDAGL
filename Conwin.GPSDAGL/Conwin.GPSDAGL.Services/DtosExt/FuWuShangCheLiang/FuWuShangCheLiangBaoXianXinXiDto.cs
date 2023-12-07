using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class UpdateFuWuShangCheLiangBaoXianXinXiDto
    {

        public Guid? CheLiangId { get; set; }
        /// <summary>
        /// 交强险保险机构名称
        /// </summary>
        public string JiaoQiangXianOrgName { get; set; }
        /// <summary>
        /// 交强险保险到期日期
        /// </summary>
        public DateTime? JiaoQiangXianEndTime { get; set; }
        /// <summary>
        /// 商业险保险机构名称
        /// </summary>
        public string ShangYeXianOrgName { get; set; }
        /// <summary>
        /// 商业险保险到期日期
        /// </summary>
        public DateTime? ShangYeXianEndTime { get; set; }

    }

    public class FuWuShangCheLiangBaoXianXinXiResponseDto
    {
        /// <summary>
        /// 交强险保险机构名称
        /// </summary>
        public string JiaoQiangXianOrgName { get; set; }
        /// <summary>
        /// 交强险保险到期日期
        /// </summary>
        public DateTime? JiaoQiangXianEndTime { get; set; }
        /// <summary>
        /// 商业险保险机构名称
        /// </summary>
        public string ShangYeXianOrgName { get; set; }
        /// <summary>
        /// 商业险保险到期日期
        /// </summary>
        public DateTime? ShangYeXianEndTime { get; set; }
    }

}
