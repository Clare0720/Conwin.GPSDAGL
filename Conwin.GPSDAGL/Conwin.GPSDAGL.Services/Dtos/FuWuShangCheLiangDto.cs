
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class FuWuShangCheLiangDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiYanSe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CheLiangZhongLei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuSheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuXian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YeHuOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> YeHuOrgType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FuWuShangOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> BeiAnZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuiJinXiuGaiRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheJiaHao { get; set; }

}

}
