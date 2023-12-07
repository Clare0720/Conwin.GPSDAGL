
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class FuWuShangCheLiangVideoZhongDuanXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string FuWuShangCheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhongDuanMDT { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiXingHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> SheBeiJiShenLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiGouCheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChangJiaBianHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShengChanChangJia { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> AnZhuangShiJian { get; set; }

}

}
