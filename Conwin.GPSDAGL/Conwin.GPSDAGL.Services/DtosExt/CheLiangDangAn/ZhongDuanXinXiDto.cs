using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    public class GpsZhongDuanXinXiDto
    {
        public Guid Id { get; set; }


        [Description("GPS终端类型")]
        public int? ZhongDuanLeiXing { get; set; }
        [Description("GPS设备型号")]
        public string SheBeiXingHao { get; set; }
        [Description("GPS终端生产厂家")]
        public string ShengChanChangJia { get; set; }
        [Description("GPS终端厂家编号")]
        public string ChangJiaBianHao { get; set; }
        [Description("GPS终端编码")]
        public string ZhongDuanBianMa { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
        [Description("GPS终端MDT")]
        public string ZhongDuanMDT { get; set; }
        [Description("GPS终端SIM卡号")]
        public string SIMKaHao { get; set; }
        [Description("M1")]
        public long? M1 { get; set; }
        [Description("IA1")]
        public long? IA1 { get; set; }
        [Description("IC1")]
        public long? IC1 { get; set; }
        [Description("是否安装视频终端")]
        public int? ShiFouAnZhuangShiPinZhongDuan { get; set; }
        [Description("视频头个数")]
        public int? ShiPinTouGeShu { get; set; }
        [Description("视频厂商类型")]
        public int? ShiPingChangShangLeiXing { get; set; }
        [Description("视频头安装选择")]
        public string ShiPinTouAnZhuangXuanZe { get; set; }
    }

    public class VideoZhongDuanImgDto
    {
        public string FileId { get; set; }
        public int? FileType { get; set; }
    }

    public class VideoZhongDuanXinXiDto
    {
        public Guid Id { get; set; }


        [Description("智能视频设备MDT")]
        public string VideoDeviceMDT { get; set; }
        [Description("智能视频设备型号")]
        public string VideoSheBeiXingHao { get; set; }
        [Description("智能视频设备机身类型")]
        public int? VideoSheBeiJiShenLeiXing { get; set; }
        [Description("智能视频设备构成")]
        public string VideoSheBeiGouCheng { get; set; }
        [Description("智能视频生产厂家")]
        public string VideoShengChanChangJia { get; set; }
        [Description("智能视频厂家编号")]
        public string VideoChangJiaBianHao { get; set; }
        [Description("智能视频安装时间")]
        public DateTime? VideoAnZhuangShiJian { get; set; }
        [Description("DMS设备照片")]
        public string XianShiPingZhaoPianId { get; set; }
        [Description("ADAS设备照片")]
        public string ShengGuangBaoJingXiTongZhaoPianId { get; set; }
        [Description("主机设备照片")]
        public string ZhuJiCunChuQiZhaoPianId { get; set; }
        public List<VideoZhongDuanImgDto> FileList { get; set; }

       
    }

    public class ZhongDuanShuJuTongXunPeiZhiXinXiDto
    {
        public Guid Id { get; set; }
        [Description("终端")]
        public Guid? ZhongDuanID { get; set; }
        [Description("车辆")]
        public Guid? CheLiangID { get; set; }
        public int? XieYiLeiXing { get; set; }
        [Description("抓包来源")]
        public int? ZhuaBaoLaiYuan { get; set; }
        /// <summary>
        /// 数据通讯版本号
        /// </summary>  
        [Description("终端数据通讯版本号")]
        public int? BanBenHao { get; set; }

        public static ZhongDuanShuJuTongXunPeiZhiXinXiDto MapFromEntity(CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi entity)
        {
            return new ZhongDuanShuJuTongXunPeiZhiXinXiDto
            {
                Id = entity.Id,
                BanBenHao = entity.BanBenHao,
                CheLiangID = entity.CheLiangID,
                XieYiLeiXing = entity.XieYiLeiXing,
                ZhongDuanID = entity.ZhongDuanID,
                ZhuaBaoLaiYuan = entity.ZhuaBaoLaiYuan,
            };
        }
        public CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi MapToEntity(CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi entity)
        {
            entity.BanBenHao = this.BanBenHao;
            if(this.CheLiangID.HasValue)
                entity.CheLiangID = this.CheLiangID;
            if (this.XieYiLeiXing.HasValue)
                entity.XieYiLeiXing = this.XieYiLeiXing;
            if (this.ZhongDuanID.HasValue)
                entity.ZhongDuanID = this.ZhongDuanID;
            if (this.ZhuaBaoLaiYuan.HasValue)
                entity.ZhuaBaoLaiYuan = this.ZhuaBaoLaiYuan;
            return entity;
        }

    }

    public class ZhongDuanXinXiDto
    {
        public GpsZhongDuanXinXiDto GpsInfo { get; set; }
        public VideoZhongDuanXinXiDto VideoInfo { get; set; }
        public ZhongDuanShuJuTongXunPeiZhiXinXiDto PeiZhiInfo { get; set; }
    }

    public class UpdateZhongDuanXinXiDto
    {
        public string CheLiangId { get; set; }
        public GpsZhongDuanXinXiDto GpsInfo { get; set; }

        public VideoZhongDuanXinXiDto VideoInfo { get; set; }

        public ZhongDuanShuJuTongXunPeiZhiXinXiDto PeiZhiInfo { get; set; }
    }


    public class CheLiangVideoZhongDuanConfirmInfoDto 
    {
        [DataMember(EmitDefaultValue = false)]
        public string CheLiangId { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ZhongDuanId { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("数据接入")]
        public Nullable<bool> ShuJuJieRu { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("设备完整")]
        public Nullable<bool> SheBeiWanZheng { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("核查情况说明")]
        public string NeiRong { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("备案状态")]
        public Nullable<int> BeiAnZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Nullable<System.DateTime> TiJiaoBeiAnShiJian { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Nullable<System.DateTime> ZuiJinShenHeShiJian { get; set; }
    }

}
