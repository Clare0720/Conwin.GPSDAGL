
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangAnZhuangZhengMingDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> CheLiangID { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhengMingBianHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> GongZhangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhengMingLeiXin { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> ZhengMingFileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShuiYinPDFFileId { get; set; }

}

}
