using Conwin.Framework.Log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Conwin.GPSDAGL.Framework
{
    /// <summary>
    /// 类型转换类 
    /// </summary>
    public sealed class TypeHelper
    {
        #region 数据类型转换

        public static int ToInt(object o)
        {
            return ToInt(o, default(int));
        }

        public static int ToInt(object o, int defaultVal)
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            int retVal;
            if (!int.TryParse(o.ToString(), out retVal))
            {
                return defaultVal;
            }
            return retVal;
        }

        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, default(decimal));
        }

        public static decimal ToDecimal(object o, decimal defaultVal)
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            decimal retVal;
            if (!decimal.TryParse(o.ToString(), out retVal))
            {
                return defaultVal;
            }
            return retVal;
        }

        public static string ToString(object o)
        {
            return ToString(o, "");
        }

        public static string ToString(object o, string defaultVal)
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            return o.ToString();
        }

        public static bool ToBoolean(object o)
        {
            return ToBoolean(o, default(bool));
        }

        public static bool ToBoolean(object o, bool defaultVal)
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            try
            {
                return Convert.ToBoolean(o);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static DateTime ToDateTime(object o)
        {
            return ToDateTime(o, default(DateTime));
        }

        public static DateTime ToDateTime(object o, DateTime defaultVal)
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            DateTime retVal;
            if (!DateTime.TryParse(o.ToString(), out retVal))
            {
                return defaultVal;
            }
            return retVal;
        }

        #endregion

        #region 对象互转

        /// <summary>
        /// 适用于初始化新实体
        /// </summary>
        public static T RotationMapping<T, S>(S s)
        {
            T target = Activator.CreateInstance<T>();
            var originalObj = s.GetType();
            var targetObj = typeof(T);
            foreach (PropertyInfo original in originalObj.GetProperties())
            {
                foreach (PropertyInfo t in targetObj.GetProperties())
                {
                    if (t.Name == original.Name)
                    {
                        t.SetValue(target, original.GetValue(s, null), null);
                    }
                }
            }
            return target;
        }

        /// <summary>
        /// 适用于没有新建实体之间
        /// </summary>
        public static T RotationMapping<T, S>(T t, S s)
        {
            var originalObj = s.GetType();
            var targetObj = typeof(T);
            foreach (PropertyInfo sp in originalObj.GetProperties())
            {
                foreach (PropertyInfo dp in targetObj.GetProperties())
                {
                    if (dp.Name == sp.Name)
                    {
                        dp.SetValue(t, sp.GetValue(s, null), null);
                    }
                }
            }
            return t;
        }

        #endregion

    }
}
