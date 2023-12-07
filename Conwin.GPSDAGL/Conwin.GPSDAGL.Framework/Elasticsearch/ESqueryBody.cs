using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.Elasticsearch
{
    public class ESQueryBody : Base.ESBase
    {
        private List<dynamic> sort = new List<dynamic>();

        private object filter;

        private IDictionary<string, object> requsetBody;

        public ESQueryBody()
        {
            requsetBody = new Dictionary<string, object>();
            requsetBody["query"] = new { match_all = new { } };
            minimum_should_match = 1;
        }



        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="size">size</param>
        public void SetFromSize(int from ,int size)
        {
            requsetBody["from"] = from;
            requsetBody["size"] = size;
        }

        /// <summary>
        /// 设置返回个数
        /// </summary>
        /// <param name="size">size</param>
        public void SetSize(int size)
        {
            requsetBody["size"] = size;
        }


        /// <summary>
        /// 设置所使用的filter
        /// </summary>
        /// <param name="filter"></param>
        public void SetQueryFilter(ESFilter filter)
        {
            this.filter = filter.getFilter();
        }

        public void SetQueryFilter(object filter)
        {
            this.filter = filter;
        }

        #region sort 排序
        /// <summary>
        /// 添加排序
        /// 格式 { "sort":["key1","key2"] }
        /// </summary>
        /// <param name="key"></param>
        public void AddSort(string key)
        {
            sort.Add(key);
        }

        /// <summary>
        /// 添加排序
        /// 格式 { "sort":[{ "date": { "order" : "desc" } }] }
        /// </summary>
        /// <param name="key"></param>
        /// <param name="e">枚举</param>
        public void AddSort(string key, ESQuerySort e)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = new { order = Enum.GetName(typeof(ESQuerySort), e).ToLower() };
            sort.Add(dic);
        }

        /// <summary>
        /// 添加排序
        /// 格式： { "sort":[{ "key.keyword": { "order" : "desc" , "mode" : "min"} }] }
        /// </summary>
        /// <param name="key"></param>
        /// <param name="e">枚举</param>
        /// <param name="m">枚举</param>
        public void AddSort(string key, ESQuerySort e, ESQuerySortMode m)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = new
            {
                order = Enum.GetName(typeof(ESQuerySort), e).ToLower() ,
                mode = Enum.GetName(typeof(ESQuerySortMode), m).ToLower(),
            };
            sort.Add(dic);
        }


        #endregion



        #region _source

        public void Set_SourceIncludes(IEnumerable<string> includes)
        {
            requsetBody["_source"] = new { includes=includes };
        }

        public void Set_SourceExcludes(IEnumerable<string> excludes)
        {
            requsetBody["_source"] = new { excludes = excludes };
        }

        public void Set_Source(IEnumerable<string> includes, IEnumerable<string> excludes)
        {
            requsetBody["_source"] = new { includes = includes, excludes = excludes };
        }

        public void Set_Source(bool b)
        {
            requsetBody["_source"] = b;
        }

        #endregion


        /// <summary>
        /// 自主扩展
        /// </summary>
        /// <param name="property"></param>
        /// <param name="obj"></param>
        public void SetEx(string property ,object obj)
        {
            requsetBody[property] = obj;
        }



        public object getBoolBody()
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            if (must.Count > 0)
            {
                dic["must"] = must;
            }
            if (must_not.Count > 0)
            {
                dic["must_not"] = must_not;
            }
            if (should.Count > 0)
            {
                dic["should"] = should;
                dic["minimum_should_match"] = minimum_should_match;
            }
            if (filter != null)
            {
                dic["filter"] = filter;
            }
            if (dic.Count > 0)
            {
                var b = new { @bool = dic };
                return b;
            }
            return null;
        }



        #region 出口

        /// <summary>
        /// 获取查询Json
        /// </summary>
        /// <returns>Json字符串</returns>
        public string GetQuery()
        {
            var b = getBoolBody();
            if (b != null)
            {
                requsetBody["query"] = b;
            }
            if (sort.Count > 0)
            {
                requsetBody["sort"] = sort;
            }
            return JsonConvert.SerializeObject(requsetBody);
        }

        /// <summary>
        /// 获取查询Json 格式化的
        /// </summary>
        /// <returns>Json字符串</returns>
        public string GetQueryJson()
        {
            var b = getBoolBody();
            if (b != null)
            {
                requsetBody["query"] = b;
            }
            if (sort.Count > 0)
            {
                requsetBody["sort"] = sort;
            }
            return JsonConvert.DeserializeObject<JObject>(GetQuery()).ToString();
        }


        #endregion




    }
}

