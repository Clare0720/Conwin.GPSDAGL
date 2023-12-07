using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{
    [DataContract(IsReference = true)]
    public partial class T_RenYuanXinXiDto : EntityMetadataDto 
    {
    	[DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string RenYuanCode { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string Icon { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string IDPhoto { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> IDCardType { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string IDCard { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> Sex { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string Cellphone { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string NativePlace { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<System.DateTime> EntryDate { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> WorkingStatus { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string Attachments { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public Nullable<int> IsCreateAccount { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        public string SysUserId { get; set; }
    }
}
