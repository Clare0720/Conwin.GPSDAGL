using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Framework.Elasticsearch;

namespace Conwin.GPSDAGL.Framework.Elasticsearch.Base
{
    public abstract class ESBase
    {

        protected List<dynamic> must = new List<dynamic>();

        protected List<dynamic> must_not = new List<dynamic>();

        protected List<dynamic> should = new List<dynamic>();

        public int minimum_should_match { get; set; }

        public ESBase()
        {
            minimum_should_match = 1;
        }



        #region 添加查询条件
        public void Must_Term(string key, object value)
        {
            AddTerm(key, value, must);
        }

        public void MustNot_Term(string key, object value)
        {
            AddTerm(key, value, must_not);
        }

        public void Should_Term(string key, object value)
        {
            AddTerm(key, value, should);
        }

        public void Must_Terms(string key, IEnumerable<dynamic> value)
        {
            AddTerms(key, value, must);
        }

        public void MustNot_Terms(string key, IEnumerable<dynamic> value)
        {
            AddTerms(key, value, must_not);
        }

        public void Should_Terms(string key, IEnumerable<dynamic> value)
        {
            AddTerms(key, value, should);
        }

        #region range 范围查询
        public void Must_Range(string key, ESQueryRange esG, ESQueryRange esL, object g, object l)
        {
            AddRange(key, esG, esL, g, l, must);
        }
        public void MustNot_Range(string key, ESQueryRange esG, ESQueryRange esL, object g, object l)
        {
            AddRange(key, esG, esL, g, l, must_not);
        }

        public void Should_Range(string key, ESQueryRange esG, ESQueryRange esL, object g, object l)
        {
            AddRange(key, esG, esL, g, l, should);
        }

        public void Must_Range(string key, ESQueryRange es, object v)
        {
            AddRange(key, es, v, must);
        }
        public void MustNot_Range(string key, ESQueryRange es, object v)
        {
            AddRange(key, es, v, must_not);
        }

        public void Should_Range(string key, ESQueryRange es, object v)
        {
            AddRange(key, es, v, should);
        }

        #endregion

        #region match 
        public void Must_Match(string key, object match, ESOperator @operator = ESOperator.OR)
        {
            AddMatch(key, match, @operator, must);
        }

        public void MustNot_Match(string key, object match, ESOperator @operator = ESOperator.OR)
        {
            AddMatch(key, match, @operator, must_not);
        }

        public void Should_Match(string key, object match, ESOperator @operator = ESOperator.OR)
        {
            AddMatch(key, match, @operator, should);
        }

        #endregion

        #region wildcard 模糊查询 

        // 通配符查询，是简化的正则表达式查询，包括下面两类通配符：

        //* 代表任意（包括0个）多个字符
        //? 代表任意一个字符

        public void Must_Wildcard(string key, object wildcard)
        {
            AddWildcard(key, wildcard, must);
        }

        public void MustNot_Wildcard(string key, object wildcard)
        {
            AddWildcard(key, wildcard, must_not);
        }

        public void Should_Wildcard(string key, object wildcard)
        {
            AddWildcard(key, wildcard, should);
        }
        #endregion

        #region pirefix 前缀查询

        public void Must_Prefix(string key, object wildcard)
        {
            AddPrefix(key, wildcard, must);
        }

        public void MustNot_Prefix(string key, object wildcard)
        {
            AddPrefix(key, wildcard, must_not);
        }

        public void Should_Prefix(string key, object wildcard)
        {
            AddPrefix(key, wildcard, should);
        }
        #endregion



        #region nested

        public void Must_Nested(string path, ESQueryBody query)
        {
            AddNested(path, query, must);
        }

        public void MustNot_Nested(string path, ESQueryBody query)
        {
            AddNested(path, query, must_not);
        }

        public void Should_Nested(string path, ESQueryBody query)
        {
            AddNested(path, query, should);
        }

        public void Must_Nested(string path, ESFilter query)
        {
            AddNested(path, query, must);
        }

        public void MustNot_Nested(string path, ESFilter query)
        {
            AddNested(path, query, must_not);
        }

        public void Should_Nested(string path, ESFilter query)
        {
            AddNested(path, query, should);
        }

        #endregion



        #region tool
        private void AddTerm(string key, object value, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = value;
            list.Add(new { term = dic });
        }

        private void AddTerms(string key, IEnumerable<dynamic> value, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = value.ToList();
            list.Add(new { terms = dic });
        }

        private void AddRange(string key, ESQueryRange esG, ESQueryRange esL, object g, object l, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;

            if (esG == ESQueryRange.GT)
            {
                if (esL == ESQueryRange.LT)
                {
                    dic[key] = new { gt = g, lt = l };
                }
                else if (esL == ESQueryRange.LTE)
                {
                    dic[key] = new { gt = g, lte = l };
                }
            }
            else if (esG == ESQueryRange.GTE)
            {
                if (esL == ESQueryRange.LT)
                {
                    dic[key] = new { gte = g, lt = l };
                }
                else if (esL == ESQueryRange.LTE)
                {
                    dic[key] = new { gte = g, lte = l };
                }
            }
            list.Add(new { range = dic });
        }

        private void AddRange(string key, ESQueryRange es, object v, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;

            if (es == ESQueryRange.GT)
            {
                dic[key] = new { gt = v };
            }
            else if (es == ESQueryRange.GTE)
            {
                dic[key] = new { gte = v };
            }
            else if (es == ESQueryRange.LT)
            {
                dic[key] = new { lt = v };
            }
            else if (es == ESQueryRange.LTE)
            {
                dic[key] = new { lte = v };
            }
            list.Add(new { range = dic });
        }

        private void AddMatch(string key, object value, ESOperator sOperator, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            string op;
            switch (sOperator)
            {
                case ESOperator.OR:
                    op = "or";
                    break;
                case ESOperator.AND:
                    op = "and";
                    break;
                default:
                    op = "or";
                    break;
            }
            dic[key] = new { query = value, @operator = op };
            list.Add(new { match = dic });
        }

        private void AddWildcard(string key, object value, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = value;
            list.Add(new { wildcard = dic });
        }

        private void AddPrefix(string key, object value, List<dynamic> list)
        {
            dynamic dobj = new System.Dynamic.ExpandoObject();
            var dic = (IDictionary<string, object>)dobj;
            dic[key] = value;
            list.Add(new { prefix = dic });
        }

        private void AddNested(string path, ESQueryBody query, List<dynamic> list)
        {
            var n = new { path = path, query = query.getBoolBody() };
            list.Add(new { nested = n });
        }

        private void AddNested(string path, ESFilter filter, List<dynamic> list)
        {
            var n = new { path = path, query = filter.getFilter() };
            list.Add(new { nested = n });
        }

        #endregion

        #endregion

        public List<dynamic> Should
        {
            get => should;
        }
        public List<dynamic> Must
        {
            get => must;
        }

        public List<dynamic> MustNot
        {
            get => must_not;
        }


        public void setMust(IEnumerable<object> must)
        {
            this.must = must.ToList();
        }

        public void setMustNot(IEnumerable<object> mustNot)
        {
            this.must_not = mustNot.ToList();
        }

        public void setShould(IEnumerable<object> should)
        {
            this.should = should.ToList();
        }
    }
}
