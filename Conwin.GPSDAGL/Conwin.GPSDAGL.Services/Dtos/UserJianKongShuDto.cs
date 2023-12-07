
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class UserJianKongShuDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string NodeId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SysUserId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Enabled { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string BeiZhu { get; set; }

}

}
