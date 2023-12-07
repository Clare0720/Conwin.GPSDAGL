using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Common
{
    /// <summary>
    /// 分页工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingUtil<T> : List<T>
    {
        public int DataCount { get; set; } //总记录数
        public int PageCount { get; set; } //总页数
        public int PageNo { get; set; } //当前页码
        public int PageSize { get; set; } //每页显示记录数
        public List<T> dataList { get; set; } // 集合
        //是否有上一页
        public bool HasPreviousPage
        {
            get { return PageNo > 1; }
        }

        //是否有下一页
        public bool HasNextPage
        {
            get { return PageNo < this.PageCount; }
        }
        public bool IsEffectivePage
        {
            get { return PageNo <= this.PageCount; }
        }
        //获取当前页
        public List<T> GetCurrentPage()
        {
            return dataList.Skip((this.PageNo - 1) * this.PageSize).Take(this.PageSize).ToList();
        }
        //跳转到下一页
        public void GotoNextPage()
        {
            this.PageNo++;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        public PagingUtil(List<T> dataList, int pageSize)
        {
            this.PageSize = pageSize;
            this.PageNo = 1;
            this.DataCount = dataList.Count;
            this.PageCount = (int)Math.Ceiling((decimal)this.DataCount / pageSize);
            this.dataList = dataList;
        }
    }
}
