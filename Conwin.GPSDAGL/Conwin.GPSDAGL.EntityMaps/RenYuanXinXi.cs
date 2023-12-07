using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class RenYuanXinXi : EntityMetadata 
    {
        public string Name { get; set; }
        public string RenYuanCode { get; set; }
        public string Icon { get; set; }
        public string IDPhoto { get; set; }
        public Nullable<int> IDCardType { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Cellphone { get; set; }
        public string NativePlace { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<int> WorkingStatus { get; set; }
        public string Attachments { get; set; }
        public Nullable<int> IsCreateAccount { get; set; }
        public string SysUserId { get; set; }
    }
}
