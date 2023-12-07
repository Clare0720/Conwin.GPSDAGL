
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class MaterialListOfServiceProviderDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string IndustrialACBLicenseId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IndustrialACBLOTOfficeId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FilingApplicationFormId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string StandardTOServiceId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SiteMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PhotosOBPremisesId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PersonnelPDMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PersonnelSSCertificateId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string EquipmentIMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SupervisorMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ViolationsASMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string MonitoringSystemMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SafetyGradeMaterialsId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Landline { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Mailbox { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ApprovalRemark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int ApprovalStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CompanyType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }

}

}
