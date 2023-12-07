using Conwin.GPSDAGL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang
{
    public class FuWuShangCheLiangZhongDuanUpdateDto
    {
        /// <summary>
        /// 服务商车辆ID，编辑时不能为空
        /// </summary>
        public Guid FuWuShangCheLiangId { get; set; }
        /// <summary>
        /// 服务商车辆GPS终端信息
        /// </summary>
        public FuWuShangCLGpsInfoDto FuWuShangGpsInfo { get; set; }
        /// <summary>
        /// 服务商车辆智能视频终端信息
        /// </summary>
        public FuWuShangCLVideoInfoDto FuWuShangVideoInfo { get; set; }
        /// <summary>
        /// 服务商车辆gps终端数据通讯配置信息
        /// </summary>
        public FuWuShangZhongDuanShuJuTongXunPeiZhiXinXiDto PeiZhiInfo { get; set; }
    }

    /// <summary>
    /// 服务商车辆GPS终端信息
    /// </summary>
    public class FuWuShangCLGpsInfoDto
    {
        public int? ZhongDuanLeiXing { get; set; }
        public string ShengChanChangJia { get; set; }
        public string ChangJiaBianHao { get; set; }
        public string SheBeiXingHao { get; set; }
        public string ZhongDuanBianMa { get; set; }
        public string SIMKaHao { get; set; }
        public string ZhongDuanMDT { get; set; }
        public long? M1 { get; set; }
        public long? IA1 { get; set; }
        public long? IC1 { get; set; }
        public int? ShiFouAnZhuangShiPinZhongDuan { get; set; }
        public string ShiPinTouAnZhuangXuanZe { get; set; }
        public int? ShiPingChangShangLeiXing { get; set; }
        public int? ShiPinTouGeShu { get; set; }
        public string Remark { get; set; }
    }

    /// <summary>
    /// 服务商车辆智能视频终端信息
    /// </summary>
    public class FuWuShangCLVideoInfoDto
    {
        public string VideoDeviceMDT { get; set; }
        public string VideoSheBeiXingHao { get; set; }
        public int? VideoSheBeiJiShenLeiXing { get; set; }
        public string VideoSheBeiGouCheng { get; set; }
        public string VideoShengChanChangJia { get; set; }
        public string VideoChangJiaBianHao { get; set; }
        public DateTime? VideoAnZhuangShiJian { get; set; }
        public string XianShiPingZhaoPianId { get; set; }
        public string ShengGuangBaoJingXiTongZhaoPianId { get; set; }
        public string ZhuJiCunChuQiZhaoPianId { get; set; }
        public List<FuWuShangVideoZhongDuanImgDto> FileList { get; set; }
    }

    /// <summary>
    /// 服务商车辆智能视频终端附件图片信息
    /// </summary>
    public class FuWuShangVideoZhongDuanImgDto
    {
        public string FileId { get; set; }
        public int? FileType { get; set; }
    }


    public class FuWuShangZhongDuanShuJuTongXunPeiZhiXinXiDto
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

        public static FuWuShangZhongDuanShuJuTongXunPeiZhiXinXiDto MapFromEntity(FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi entity)
        {
            return new FuWuShangZhongDuanShuJuTongXunPeiZhiXinXiDto
            {
                Id = entity.Id,
                BanBenHao = entity.BanBenHao,
                CheLiangID = entity.CheLiangID,
                XieYiLeiXing = entity.XieYiLeiXing,
                ZhongDuanID = entity.ZhongDuanID,
                ZhuaBaoLaiYuan = entity.ZhuaBaoLaiYuan,
            };
        }
        public FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi MapToEntity(FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi entity)
        {
            entity.BanBenHao = this.BanBenHao;
            if (this.CheLiangID.HasValue)
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

}
