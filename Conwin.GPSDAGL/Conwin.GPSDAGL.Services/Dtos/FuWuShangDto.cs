
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class FuWuShangDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string BaseId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> OrgType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgShortName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YingYeZhiZhaoHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string TongYiSheHuiXinYongDaiMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YouBian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LianXiRenName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LianXiRenPhone { get; set; }

}

}
