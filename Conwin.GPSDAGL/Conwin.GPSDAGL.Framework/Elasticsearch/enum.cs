using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.Elasticsearch
{
    /// <summary>
    /// 排序
    /// </summary>
    public enum ESQuerySort
    {
        DESC, ASC
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ESQuerySortMode { MIN, MAX, SUM, AVG, MEDIAN }

    /// <summary>
    /// 范围
    /// </summary>
    public enum ESQueryRange { GT, LT, GTE, LTE }

    public enum ESOperator { OR, AND }


}
