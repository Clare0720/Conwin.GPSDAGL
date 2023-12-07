using System;

namespace Conwin.GPSDAGL.JobWindowsService.Common
{
    /// <summary>
    /// 类型转换类 
    /// </summary>
    public sealed class TypeHelper
    {
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


        public static string ToDateTime(object o, string defaultVal, string formart = "yyyy-MM-dd HH:mm:ss")
        {
            if (o == null || o == DBNull.Value)
                return defaultVal;

            DateTime retVal;
            if (!DateTime.TryParse(o.ToString(), out retVal))
            {
                return defaultVal;
            }
            return retVal.ToString(formart);
        }
    }
}
