using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.Elasticsearch
{
    public class ESFilter : Base.ESBase
    {

        private IDictionary<string,object> @bool;

        public IDictionary<string, object> filter;

        public ESFilter()
        {
            minimum_should_match = 1;
            must = new List<dynamic>();
            must_not = new List<dynamic>();
            should = new List<dynamic>();
            @bool = new Dictionary<string, object>();
            filter = new Dictionary<string, object>();
        }

        public void SetInBool(string property, object obj)
        {
            if (typeof(ESFilter) == obj.GetType())
            {
                @bool[property] = (obj as ESFilter).getFilter();
            }
            else
            {
                @bool[property] = obj;
            }
        }

        public IDictionary<string, object> Bool
        {
            get
            {
                if (must.Count > 0)
                {
                    @bool["must"] = must;
                }
                if (must_not.Count > 0)
                {
                    @bool["must_not"] = must_not;
                }
                if (should.Count > 0)
                {
                    @bool["should"] = should;
                    @bool["minimum_should_match"] = minimum_should_match;
                }
                return @bool;
            }
        }

        public object getFilter()
        {
            filter["bool"] = this.Bool;
            return filter;
        }

        public void Set_minimum_should_match(int i)
        {
            minimum_should_match = i;
        }

        public void SetEx(string property, object obj)
        {
            filter[property] = obj;
        }






    }
}
