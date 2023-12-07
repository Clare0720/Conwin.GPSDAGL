using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    /// <summary>
    /// 车辆年审指标模型
    /// </summary>
   public class VehicleAnnualReviewResultDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 是否有智能视频报警附件
        /// </summary>
        public bool IsHavVideoAlarmAttachment { get; set; }
        /// <summary>
        /// 是否有车辆轨迹信息
        /// </summary>
        public bool IsHavVehicleTrack { get; set; }
        /// <summary>
        /// 车辆最新定位时间
        /// </summary>
        public DateTime? VehicelLastPositioningTime { get; set; }
        /// <summary>
        /// 提交备案时间--不展示
        /// </summary>
        public DateTime? TiJiaoBeiAnShiJian { get; set; }

    }


    public class EventInfoDto
    {
        /// <summary>
        /// 报警内容
        /// </summary>
        public Dictionary<string, object> Content { get; set; }
    }

    public class GpsInformation
    {
        public int IDVal { get; set; }

    }
    public class EnterpriseInformation
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string QiYeMingCheng { set; get; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { set; get; }
    }


    [Serializable]
    public class KeyValueItemValid
    {
        public string Key { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 审核结果说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 审校指标
        /// </summary>
        public string FiledStander { get; set; }

    }



    /// <summary>
    /// 车辆动态数据DTO
    /// </summary>
    [Serializable]
    public class VehicleDynamicInfoDto
    {
        /// <summary>
        /// 车辆电脑编号
        /// </summary>
        public Int64? BianHao { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePai { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string YanSe { get; set; }
        /// <summary>
        /// 运营商编码
        /// </summary>
        public string OperatorCode { get; set; }
        /// <summary>
        /// 运营商名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// Sim卡号
        /// </summary>
        public string SimNo { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 厂商编码
        /// </summary>
        public string FactoryCode { get; set; }
        /// <summary>
        /// 终端标准(GPS安装状态)
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 最后上传时间(如果未安装，最后上传时间为空)
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 定位时间是否在一个月以内
        /// </summary>
        public string IsGpsTimeInRecentMonth { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// 频率(高于15秒/条，低于15秒/条，当上次Gps时间为空时，这里为空)
        /// </summary>
        public string UploadFrequency { get; set; }
        /// <summary>
        /// 是否上传道路运输电子证件IC卡数据
        /// </summary>
        public string HasUploadICData { get; set; }
        /// <summary>
        /// 道路运输证IC卡数据
        /// </summary>
        public string DLYSZICData { get; set; }
        /// <summary>
        /// 从业资格证IC卡数据
        /// </summary>
        public string CYZGZICData { get; set; }
        /// <summary>
        /// 卫星定位数据是否符合要求
        /// </summary>
        public string IsGpsDataQualified { get; set; }

    }




    class GpsInfoDto
    {
        public string RegistrationNo { get; set; }

        public string RegistrationNoColor { get; set; }

        public DateTime? GpsTime { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int Speed { get; set; }
        public string Frequency { get; set; }
    }

}
