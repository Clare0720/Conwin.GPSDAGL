
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class QiYeFuWuShangGuanLianXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string QiYeCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FuWuShangCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhuLianLuIP { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuLianLuDuanKou { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CongLianLuIP { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CongLianLuDuanKou { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuSheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PingTaiJieRuMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LoginName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LoginPassWord { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> M1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> IA1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<long> IC1 { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuXian { get; set; }

}

}
