
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class FuWuShangCheLiangYeHuLianXiXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YeHuPrincipalName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YeHuPrincipalPhone { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DriverName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DriverPhone { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CongYeZiGeZhengHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiZhongAnZhuangDianMingCheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiAnZhuangRenYuanXingMing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiAnZhuangDanWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiAnZhuangRenYuanDianHua { get; set; }

}

}
