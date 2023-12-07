
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangVideoZhongDuanConfirmDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhongDuanId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<bool> ShuJuJieRu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<bool> SheBeiWanZheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string NeiRong { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> BeiAnZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> TiJiaoBeiAnShiJian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> ZuiJinShenHeShiJian { get; set; }

}

}
