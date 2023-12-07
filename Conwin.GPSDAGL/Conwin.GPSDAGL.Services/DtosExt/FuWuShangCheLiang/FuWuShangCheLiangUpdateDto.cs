using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang
{
    public class FuWuShangCheLiangUpdateDto
    {
        /// <summary>
        /// 主键ID，修改时必填
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public Nullable<int> CheLiangZhongLei { get; set; }
        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 业户类型：2=企业；8=个体户；
        /// </summary>
        public int? YeHuOrgType { get; set; }
        /// <summary>
        /// 业户代码
        /// </summary>
        public string YeHuOrgCode { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string YeHuOrgName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 服务商组织代码
        /// </summary>
        public string FuWuShangOrgCode { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string CheJiaHao { get; set; }

        /// <summary>
        /// 最后GPS定位时间
        /// </summary>
        public Nullable<DateTime> LatestGpsTime { get; set; }
    }
}
