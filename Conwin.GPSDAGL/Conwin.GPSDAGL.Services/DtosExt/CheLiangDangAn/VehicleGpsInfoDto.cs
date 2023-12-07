using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    [DataContract(IsReference = true)]
    public partial class VehicleGpsInfoDto
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string RegistrationNo { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string RegistrationNoColor { get; set; }

        /// <summary>
        /// 车辆种类
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string CheLiangZhongLei { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string CheJiaHao { get; set; }
        /// <summary>
        /// 终端MDT
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MDT { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long? M1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long? IA1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long? IC1 { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string DeviceMode { get; set; }
        /// <summary>
        /// SIM卡号
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string SimNo { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int? TerminalType { get; set; }

        /// <summary>
        /// 业户名称
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string YeHuMingCheng { get; set; }

        /// <summary>
        /// 辖区市
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string XiaQuShi { get; set; }

        /// <summary>
        /// 辖区县
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string XiaQuXian { get; set; }


        /// <summary>
        /// 终端型号
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string TerminalTypeName { get; set; }


        /// <summary>
        /// 定位时间
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime? GpsTime { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public double? Longtitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public double? Latitude { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int? Speed { get; set; }

        /// <summary>
        /// 上传频率
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int? UploadFrequency { get; set; }
        /// <summary>
        /// GPS审核状态
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FiledState { get; set; }

        /// <summary>
        /// 备案指标
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FiledComment { get; set; }
        /// <summary>
        /// 备案结果详情
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ResultComment { get; set; }

    }
}
