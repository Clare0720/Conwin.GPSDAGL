
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class OrgBaseInfoDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> OrgType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgShortName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> ParentOrgId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JingYingFanWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuSheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuXian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DiZhi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuiJinXiuGaiRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Street { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YeWuJingYingFanWei { get; set; }

}

}
