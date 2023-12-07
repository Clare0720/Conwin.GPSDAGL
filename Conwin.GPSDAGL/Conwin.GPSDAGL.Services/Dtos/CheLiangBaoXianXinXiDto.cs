
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangBaoXianXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaoQiangXianOrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> JiaoQiangXianEndTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShangYeXianOrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> ShangYeXianEndTime { get; set; }

}

}
