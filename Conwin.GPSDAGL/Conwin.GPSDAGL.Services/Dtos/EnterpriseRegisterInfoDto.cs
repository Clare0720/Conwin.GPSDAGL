
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class EnterpriseRegisterInfoDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactIDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactTel { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactEMail { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PrincipalName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PrincipalIDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PrincipalTel { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string UniformSocialCreditCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string BusinessLicenseFileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string BusinessPermitNumber { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> BusinessPermitStartDateTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> BusinessPermitEndDateTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string BusinessPermitIssuingUnit { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string BusinessPermitPhotoFIleId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ApprovalStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ApprovalRemark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> EnterpriseType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> MonitorType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactIDCardFrontId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ContactIDCardBackId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PrincipalIDCardFrontId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PrincipalIDCardBackId { get; set; }

}

}
