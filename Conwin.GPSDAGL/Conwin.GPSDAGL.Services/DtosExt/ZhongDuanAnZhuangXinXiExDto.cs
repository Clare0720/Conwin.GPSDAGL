using Conwin.GPSDAGL.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class ZhongDuanAnZhuangXinXiSearchDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
    public class ZhongDuanAnZhuangXinXiExDto
    {
        
        /// <summary>
        /// 车辆Id
        /// </summary>
        public string CheLiangId { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }

        /// <summary>
        /// 代理商Id
        /// </summary>
        public string DaiLiShangId { get; set; }
        /// <summary>
        /// 代理商名称
        /// </summary>
        public string DaiLiShangOrgName { get; set; }

        /// <summary>
        /// 代理商 组织代码
        /// </summary>
        public string DaiLiShangOrgCode { get; set; }

        /// <summary>
        /// 服务商Id
        /// </summary>
        public string FuWuShangId { get; set; }
        /// <summary>
        /// 服务商 名称
        /// </summary>
        public string FuWuShangOrgName { get; set; }

        /// <summary>
        /// 服务商 组织代码
        /// </summary>
        public string FuWuShangOrgCode { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string SheBeiId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string SheBeiXingHao { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public int? ZhongDuanLeiXing { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string ShengChanChangJia { get; set; }

        /// <summary>
        /// 厂家编号
        /// </summary>
        public string ChangJiaBianHao { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string XingHaoBianMa { get; set; }

        /// <summary>
        /// 适用车型
        /// </summary>
        public string ShiYongCheXing { get; set; }

        /// <summary>
        /// 定位模块
        /// </summary>
        public string DingWeiMoKuai { get; set; }

        /// <summary>
        /// 通讯模块
        /// </summary>
        public string TongXunMoShi { get; set; }

        /// <summary>
        /// 过检批次
        /// </summary>
        public int? GuoJianPiCi { get; set; }

        /// <summary>
        /// 终端编码
        /// </summary>
        public string ZhongDuanBianMa { get; set; }

        /// <summary>
        /// 公告日期
        /// </summary>
        public DateTime? GongGaoRiQi { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }

        /// <summary>
        /// 车机序号
        /// </summary>
        public string CheJiXuHao { get; set; }

        /// <summary>
        /// 车机配件
        /// </summary>
        public string CheJiPeiJian { get; set; }

        /// <summary>
        /// 主中心号
        /// </summary>
        public string ZhuZhongXinHao { get; set; }

        /// <summary>
        /// 副中心号
        /// </summary>
        public string FuZhongXinHao { get; set; }

        /// <summary>
        /// 默认通道
        /// </summary>
        public string MoRenTongDao { get; set; }

        /// <summary>
        /// APN参数
        /// </summary>
        public string APNCanShu { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        public string ZuiGaoSuDu { get; set; }

        /// <summary>
        /// 最低速度
        /// </summary>
        public string ZuiDiSuDu { get; set; }

        /// <summary>
        /// 上线IP
        /// </summary>
        public string ShangXianIP { get; set; }

        /// <summary>
        /// 上线端口
        /// </summary>
        public string ShangXianDuanKou { get; set; }

        /// <summary>
        /// 终端号MDT
        /// </summary>
        public string ZhongDuanHao { get; set; }

        /// <summary>
        /// M1
        /// </summary>
        public string M1 { get; set; }

        /// <summary>
        /// IA1
        /// </summary>
        public string IA1 { get; set; }

        /// <summary>
        /// IC1
        /// </summary>
        public string IC1 { get; set; }

        /// <summary>
        /// SIM卡号
        /// </summary>
        public string SIMKaHao { get; set; }

        /// <summary>
        /// SIM序号
        /// </summary>
        public string SIMXuLieHao { get; set; }

        /// <summary>
        /// 服务期限 开始时间
        /// </summary>
        public DateTime? DeviceInstallCertDate { get; set; }

        /// <summary>
        /// 服务期限 结束时间
        /// </summary>
        public DateTime? DeviceInstallCertExpired { get; set; }

        /// <summary>
        /// 是否安装视频终端
        /// </summary>
        public int? ShiFouAnZhuangShiPingZhongDuan { get; set; }

        /// <summary>
        /// 视频头个数
        /// </summary>
        public int? ShiPingTouGeShu { get; set; }

        /// <summary>
        /// 厂商类型
        /// </summary>
        public int? VideoServiceKind { get; set; }

        /// <summary>
        /// 视频头安装选择
        /// </summary>
        public string CameraSelected { get; set; }

        /// <summary>
        /// 设备IMEI号
        /// </summary>
        public string SheBeiIMEI { get; set; }

    }
}
