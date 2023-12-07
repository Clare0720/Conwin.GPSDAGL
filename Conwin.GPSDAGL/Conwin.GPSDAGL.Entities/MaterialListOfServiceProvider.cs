using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class MaterialListOfServiceProvider : EntityMetadata 
    {
        public string IndustrialACBLicenseId { get; set; }
        public string IndustrialACBLOTOfficeId { get; set; }
        public string FilingApplicationFormId { get; set; }
        public string StandardTOServiceId { get; set; }
        public string SiteMaterialsId { get; set; }
        public string PhotosOBPremisesId { get; set; }
        public string PersonnelPDMaterialsId { get; set; }
        public string PersonnelSSCertificateId { get; set; }
        public string EquipmentIMaterialsId { get; set; }
        public string SupervisorMaterialsId { get; set; }
        public string ViolationsASMaterialsId { get; set; }
        public string MonitoringSystemMaterialsId { get; set; }
        public string SafetyGradeMaterialsId { get; set; }
        public string Landline { get; set; }
        public string Mailbox { get; set; }
        public string IDCard { get; set; }
        public string ApprovalRemark { get; set; }
        public int ApprovalStatus { get; set; }
        public Nullable<int> CompanyType { get; set; }
        public string OrgCode { get; set; }
    }
}
