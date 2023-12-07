using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class UpdateFuWuShangYeHuBaoXianXinXiRequestDto
    {
        public Guid? Id { get; set; }
        public Guid? CheLiangId { get; set; }
        /// <summary>
        /// 业户负责人姓名
        /// </summary>
        public string YeHuPrincipalName { get; set; }
        /// <summary>
        /// 业户负责人联系方式
        /// </summary>
        public string YeHuPrincipalPhone { get; set; }
        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 司机手机号码
        /// </summary>
        public string DriverPhone { get; set; }
        /// <summary>
        /// 从业资格证号
        /// </summary>
        public string CongYeZiGeZhengHao { get; set; }
        /// <summary>
        /// 集中安装点名称
        /// </summary>
        public string JiZhongAnZhuangDianMingCheng { get; set; }
        /// <summary>
        /// 设备安装人员姓名
        /// </summary>
        public string SheBeiAnZhuangRenYuanXingMing { get; set; }
        /// <summary>
        /// 设备安装单位
        /// </summary>
        public string SheBeiAnZhuangDanWei { get; set; }
        /// <summary>
        /// 设备安装人员电话
        /// </summary>
        public string SheBeiAnZhuangRenYuanDianHua { get; set; }
    }


    public class UpdateFuWuShangYeHuBaoXianXinXiResponseDto
    {
        /// <summary>
        /// 业户负责人姓名
        /// </summary>
        public string YeHuPrincipalName { get; set; }
        /// <summary>
        /// 业户负责人联系方式
        /// </summary>
        public string YeHuPrincipalPhone { get; set; }
        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 司机手机号码
        /// </summary>
        public string DriverPhone { get; set; }
        /// <summary>
        /// 从业资格证号
        /// </summary>
        public string CongYeZiGeZhengHao { get; set; }
        /// <summary>
        /// 集中安装点名称
        /// </summary>
        public string JiZhongAnZhuangDianMingCheng { get; set; }
        /// <summary>
        /// 设备安装人员姓名
        /// </summary>
        public string SheBeiAnZhuangRenYuanXingMing { get; set; }
        /// <summary>
        /// 设备安装单位
        /// </summary>
        public string SheBeiAnZhuangDanWei { get; set; }
        /// <summary>
        /// 设备安装人员电话
        /// </summary>
        public string SheBeiAnZhuangRenYuanDianHua { get; set; }

    }
}
