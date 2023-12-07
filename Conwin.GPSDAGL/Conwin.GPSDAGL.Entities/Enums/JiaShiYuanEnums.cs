using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Entities.Enums
{
    /// <summary>
    /// 驾驶员证件类型
    /// </summary>
    public enum JiaShiYuanIDCardType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        IDCard = 0
    }

    /// <summary>
    /// 驾驶员工作状态
    /// </summary>
    public enum JiaShiYuanWorkStatus
    {
        /// <summary>
        /// 聘用，在职
        /// </summary>
        [Description("聘用")]
        Hire = 1,
        /// <summary>
        /// 解聘，离职
        /// </summary>
        [Description("解聘")]
        Dismissal = 2
    }
}
