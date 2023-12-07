using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    public enum OperLogModuleName
    {
        /// <summary>
        /// 车辆档案       
        /// </summary>
        [Description("车辆档案")]
        车辆档案 = 1,
        /// <summary>
        /// 企业注册备案     
        /// </summary>
        [Description("企业注册备案")]
        企业注册备案 = 2,
        /// <summary>
        /// 第三方注册备案       
        /// </summary>
        [Description("第三方注册备案")]
        第三方注册备案 = 3,
        /// <summary>
        /// 委托监控       
        /// </summary>
        [Description("委托监控")]
        委托监控 = 4,
    }
}
