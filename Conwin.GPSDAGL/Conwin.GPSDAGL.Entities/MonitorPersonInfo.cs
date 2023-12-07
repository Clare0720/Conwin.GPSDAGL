using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class MonitorPersonInfo : EntityMetadata 
    {
        public string Name { get; set; }
        public string OrgCode { get; set; }
        public string IDCard { get; set; }
        public string Tel { get; set; }
        public string LaborContractFileId { get; set; }
        public string CertificatePassingExaminationFileId { get; set; }
        public string SocialSecurityContractFileId { get; set; }
        public string IDCardFrontId { get; set; }
        public string IDCardBackId { get; set; }
    }
}
