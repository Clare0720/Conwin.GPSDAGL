using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu
{
    public class GetQingYuanYZCheLiangInput
    {
        /// <summary>
        /// 最新更新 到的车辆 ID，如果为null，则查全部新增，
        /// 否则从最新更新的车辆ID开始获取
        /// </summary>
        public long QiShiID { get; set; }
        public int PageSize { get; set; }
    }
}
