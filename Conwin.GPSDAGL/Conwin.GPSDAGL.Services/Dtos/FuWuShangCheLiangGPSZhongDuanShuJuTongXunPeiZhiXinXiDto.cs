
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> CheLiangID { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> ZhongDuanID { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> XieYiLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuaBaoLaiYuan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> BanBenHao { get; set; }

}

}
