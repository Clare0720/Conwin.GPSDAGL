
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class DangYuanDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string Name { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Sex { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string NativePlace { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Nation { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Education { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Degree { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactNumber { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Position { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> EntryDate { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DangZuZhiSuoZaiDi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LiuDongDangYuan { get; set; }

}

}
