
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class ZhongDuanFileMapperDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> ZhongDuanId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> FileType { get; set; }

}

}
