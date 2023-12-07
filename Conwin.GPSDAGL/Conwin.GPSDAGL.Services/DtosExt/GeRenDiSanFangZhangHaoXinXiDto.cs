using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{
    [DataContract(IsReference = true)]
    public partial class GeRenDiSanFangZhangHaoXinXiDto : EntityMetadataDto 
    {
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<System.Guid> GeRenID { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string WeiYiBiaoShi { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string OpenId { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string WeiXinHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string TouXiang { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string NiCheng { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> XingBie { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string GuoJia { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string XiaQuSheng { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string XiaQuShi { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string XiaQuXian { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> LeiBie { get; set; }
    }
}
