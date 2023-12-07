using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class CheLiangFenPeiJianKongSearchDto
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }

        /// <summary>
        /// 用户ID  车辆监控使用
        /// </summary>
        public Guid? SysUserID { get; set; }

        /// <summary>
        /// 企业组织代码
        /// </summary>
        public string YeHuOrgCode { get; set; }
        /// <summary>
        /// 车队组织代码
        /// </summary>
        public string CheDuiOrgCode { get; set; }
        /// <summary>
        /// 是否已分配监控
        /// </summary>
        public bool? IsFPJK { get; set; }
    }
}
