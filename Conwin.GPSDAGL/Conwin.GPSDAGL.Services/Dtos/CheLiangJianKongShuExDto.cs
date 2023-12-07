
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangJianKongShuExDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiYanSe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string NodeId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string NodeName { get; set; }

}

}
