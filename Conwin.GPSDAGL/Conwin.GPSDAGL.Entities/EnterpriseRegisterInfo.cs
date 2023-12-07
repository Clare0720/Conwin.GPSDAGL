using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class EnterpriseRegisterInfo : EntityMetadata 
    {
        public string OrgCode { get; set; }
        public string ContactName { get; set; }
        public string ContactIDCard { get; set; }
        public string ContactTel { get; set; }
        public string ContactEMail { get; set; }
        public string PrincipalName { get; set; }
        public string PrincipalIDCard { get; set; }
        public string PrincipalTel { get; set; }
        public string UniformSocialCreditCode { get; set; }
        public string BusinessLicenseFileId { get; set; }
        public string BusinessPermitNumber { get; set; }
        public Nullable<System.DateTime> BusinessPermitStartDateTime { get; set; }
        public Nullable<System.DateTime> BusinessPermitEndDateTime { get; set; }
        public string BusinessPermitIssuingUnit { get; set; }
        public string BusinessPermitPhotoFIleId { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public string ApprovalRemark { get; set; }
        public Nullable<int> EnterpriseType { get; set; }
        public Nullable<int> MonitorType { get; set; }
        public string ContactIDCardFrontId { get; set; }
        public string ContactIDCardBackId { get; set; }
        public string PrincipalIDCardFrontId { get; set; }
        public string PrincipalIDCardBackId { get; set; }
    }
}
