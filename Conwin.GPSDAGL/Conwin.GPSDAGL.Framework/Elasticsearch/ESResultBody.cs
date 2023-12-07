using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.Elasticsearch
{
    /// <summary>
    /// 有且仅当 请求成功时使用
    /// </summary>
    public class ESQueryResultBody
    {
        public string _scroll_id { get; set; }
        public int took { get; set; }
        public bool timed_out { get; set; }
        public Hits hits { get; set; }
        public _Shards _shards { get; set; }

    }
    public class Hits
    {
        /// <summary>
        /// 文档总数
        /// </summary>
        public int total { get; set; }
        public double? max_score { get; set; }
        public List<HitsObject> hits { get; set; }
    }


    public class HitsObject
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public double? _score { get; set; }
        public JObject _source { get; set; }
        public JObject fields { get; set; }
    }

    public class _Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
    }




    public class ESQueryResultBody<T> : ESQueryResultBody
    {
        public new List<HitsObject<T>> hits { get; set; }
    }

    public class HitsObject<T>
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public double? _score { get; set; }
        public T _source { get; set; }
    }



}
