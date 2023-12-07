
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class VehiclePartnershipBindingDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string EnterpriseCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ServiceProviderCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LicensePlateNumber { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LicensePlateColor { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remarks { get; set; }

}

}
