using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services
{
    /// <summary>
    /// Adds entension methods for <see cref="IQueryable"/>
    /// </summary>
    public static class IQueryableExtension2
    {
        public static IQueryable<TSource> Include<TSource>(this IQueryable<TSource> source, string path)
        {
            var objectQuery = source as ObjectQuery<TSource>;
            if (objectQuery != null)
            {
                return objectQuery.Include(path);
            }
            return source;
        }
        public static IQueryable<T> Include<T>(this IQueryable<T> mainQuery, Expression<Func<T, object>> subSelector)
        {
            //var objectQuery = mainQuery as ObjectQuery<T>;
            //if (objectQuery != null)
            //{
            //    return objectQuery.Include(subSelector);
            //}
            //return mainQuery;
            if (subSelector.Body is MethodCallExpression)
            {
                var arguments = (subSelector.Body as MethodCallExpression).Arguments;
                foreach (var item in arguments)
                {
                    //var pExp = (new System.Linq.Expressions.Expression.LambdaExpressionProxy(item as System.Linq.Expressions.Expression<System.Func<Conwin.GPSDAGL.Entities.Permission, System.Guid>>)).Body;
                    //var itemBody = (item as LambdaExpression).Body;
                    if (item is MemberExpression)
                    {
                        mainQuery.Include(((item as MemberExpression).Member as PropertyInfo).Name);
                    }
                    else if (item is LambdaExpression)
                    {
                        var itemBody = (item as LambdaExpression).Body;
                        mainQuery.Include(((itemBody as MemberExpression).Member as PropertyInfo).Name);
                    }
                }
                return mainQuery;
            }
            else if (subSelector.Body is MemberExpression)
            {
                return mainQuery.Include(((subSelector.Body as MemberExpression).Member as PropertyInfo).Name);
            }
            else
            {
                return mainQuery;
            }
        }
    }
}
