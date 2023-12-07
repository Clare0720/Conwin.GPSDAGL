using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.Dtos
{
    [DataContract(IsReference = true)]
    public partial class GeRenZhuCeRenZhengXinXiExDto : EntityMetadataDto 
    {
        [DataMember(EmitDefaultValue = false)]
        public string OrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public OrgType JiGouLeiXing { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string YeHuID { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string YeHuMingCheng { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string XingMing { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string ShenFenZhengHaoMa { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string ShouJiHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string[] ZhiWu { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string GongHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string ShenFenZhengZhaoId { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string CongYeZiGeZhengHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string BeiZhu { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> RenZhengZhuangTai { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<System.DateTime> RenZhengShenQinShiJian { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<System.DateTime> RenZhengWanChengShiJian { get; set; }
    }
}
