
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class GPSAuditRecordDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string VehicleId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ResultComment { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FiledComment { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> GPSAuditStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> FiledDate { get; set; }

}

}
