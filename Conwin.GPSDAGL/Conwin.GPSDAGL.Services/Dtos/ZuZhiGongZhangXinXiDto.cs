
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class ZuZhiGongZhangXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianDanWeiOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> GongZhangZhaoPianId { get; set; }

}

}
