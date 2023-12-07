
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class ZiDingYiCheLiangJianKongShuDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string NodeName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> ParentNodeId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Order { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<bool> IsEnabled { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> NodeType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string NodeData { get; set; }

}

}
