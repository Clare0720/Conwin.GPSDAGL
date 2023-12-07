using System;
using System.Web;
using System.Web.Caching;
namespace Conwin.GPSDAGL.Framework
{
    /// <summary>
    /// Cache的辅助类
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// 缓存时间(天)
        /// </summary>
        public const int CACHETIME = 30;
        #region Add
        /// <summary>
        /// 无过期的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, object value)
        {
            HttpContext.Current.Cache[key] = value;
        }
        /// <summary>
        /// 绝对时间的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteTime"></param>
        public static void Add(string key, object value, DateTime absoluteTime)
        {
            HttpContext.Current.Cache.Insert(key, value, null, absoluteTime, Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 相对时间的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingTime"></param>
        public static void Add(string key, object value, TimeSpan slidingTime)
        {
            HttpContext.Current.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, slidingTime);
        }
        #endregion
       /// <summary>
        /// 从 HttpContext.Current.Cache 中取回缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存对象</returns>
        public static object Get(string key)
        {
            return HttpContext.Current.Cache[key];
        }
        public static object Get(string key, Func<object> func)
        {
            Add(key, func());
            return Get(key);
        }
        public static bool Contain(string key)
        {
            return Get(key) != null;
        }
        public static void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
