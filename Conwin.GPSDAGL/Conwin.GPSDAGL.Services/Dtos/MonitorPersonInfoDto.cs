
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class MonitorPersonInfoDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string Name { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Tel { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LaborContractFileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CertificatePassingExaminationFileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SocialSecurityContractFileId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCardFrontId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCardBackId { get; set; }

}

}
