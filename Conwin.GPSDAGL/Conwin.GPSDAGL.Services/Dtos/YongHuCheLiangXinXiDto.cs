
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class YongHuCheLiangXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string SysUserId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }

}

}
