
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangDingWeiXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string RegistrationNo { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string RegistrationNoColor { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> LatestGpsTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> FirstUploadTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> LatestRecvTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> LatestLongtitude { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> LatestLatitude { get; set; }

}

}
