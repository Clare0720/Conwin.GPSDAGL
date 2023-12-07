
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangGPSZhongDuanXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhongDuanLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShengChanChangJia { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChangJiaBianHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiXingHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhongDuanBianMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SIMKaHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhongDuanMDT { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> M1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> IA1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> IC1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int ShiFouAnZhuangShiPinZhongDuan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShiPinTouAnZhuangXuanZe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int ShiPingChangShangLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int ShiPinTouGeShu { get; set; }

}

}
