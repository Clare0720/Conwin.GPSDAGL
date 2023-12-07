using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    /// <summary>
    /// 操作日志-业务操作类型-枚举
    /// </summary>
    public enum OperLogBizOperType
    {
        /// <summary>
        /// 其他操作
        /// </summary>
        [Description("其他")]
        OTHER = 0,
        /// <summary>
        /// 新增操作
        /// </summary>
        [Description("新增")]
        ADD = 1,
        /// <summary>
        /// 删除操作
        /// </summary>
        [Description("删除")]
        DELETE = 2,
        /// <summary>
        /// 更新操作
        /// </summary>
        [Description("更新")]
        UPDATE = 3,
        /// <summary>
        /// 更新操作
        /// </summary>
        [Description("查询")]
        Query = 4,
        /// <summary>
        /// 导出操作
        /// </summary>
        [Description("导出")]
        Export = 5
    }


    /// <summary>
    ///  枚举扩展类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举值的描述文本
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDesc(this System.Enum en)
        {
            string desc = "-";
            try
            {
                Type type = en.GetType();
                MemberInfo[] memInfo = type.GetMember(en.ToString());
                if (memInfo != null && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                    {
                        desc = ((DescriptionAttribute)attrs[0]).Description;
                    }
                }
                return desc;
            }
            catch
            {
                return desc;
            }
        }
    }
}
