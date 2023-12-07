using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Dtos
{

    public class CheLiangXinXiInput
    {

        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public string CheZaiDianHua { get; set; }
        public int? ZaiXianZhuangTai { get; set; }
        public int? ZhuangTai { get; set; }
        public string SouYouRen { get; set; }
        public string JingYingXuKeZhengHao { get; set; }
        public string IsZhongDuan { get; set; }
        public int? CheLiangZhongLei { get; set; }

        /// <summary>
        /// 创建单位组织代码 车辆监控使用
        /// </summary>
        public string ChuangJianDanWeiOrgCode { get; set; }

        /// <summary>
        /// 车辆ID  车辆监控使用
        /// </summary>
        public string CheLiangID { get; set; }

        /// <summary>
        /// 用户ID  车辆监控使用
        /// </summary>
        public Guid? SysUserID { get; set; }

        /// <summary>
        /// 企业组织代码
        /// </summary>
        public string QiYeOrgCode { get; set; }

        public string OrgCode { get; set; }

        /// <summary>
        /// 车队组织代码
        /// </summary>
        public string CheDuiOrgCode { get; set; }


        /// <summary>
        /// 是否已分配监控
        /// </summary>
        public bool? IsFPJK { get; set; }
        
        public DateTime? TongJiRiQiQi { get; set; }
    
        public DateTime? TongJiRiQiZhi { get; set; }
        public string SIMKaHao { get; set; }

        public string ZhongDuanHao { get; set; }
        public Nullable<System.DateTime> ShouFeiYouXiaoQi { get; set; }
        public string ShouFeiZhuangTai { get; set; }
        public string FuWuZhuangTai { get; set; }
        public Nullable<Int32> YeWuBanLiZhuangTai { get; set; }
        public string XuFeiQiXian { get; set; }
        public string IMEIKaHao { get; set; }
        public string YunYingShangMingCheng { get; set; }

    }
}
