using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Enums
{
    /// <summary>
    /// 组织注册审核状态
    /// </summary>
    public enum OrgRegisterEnum
    {
        /// <summary>
        /// 待第三方审核
        /// </summary>
        待第三方审核 = 1,
        /// <summary>
        /// 第三方审核不通过
        /// </summary>
        第三方审核不通过 = 2,
        /// <summary>
        /// 待市政府审核
        /// </summary>
        待市政府审核 = 3,
        /// <summary>
        /// 市政府审核不通过
        /// </summary>
        市政府审核不通过 = 4,
        /// <summary>
        /// 审核不通过
        /// </summary>
        审核不通过 = 5,
        /// <summary>
        /// 驳回
        /// </summary>
        通过后驳回 = 6,
    }
}
