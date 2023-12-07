using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiangDingWeiXinXi : EntityMetadata 
    {
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }
        public Nullable<System.DateTime> LatestGpsTime { get; set; }
        public Nullable<System.DateTime> FirstUploadTime { get; set; }
        public Nullable<System.DateTime> LatestRecvTime { get; set; }
        public Nullable<double> LatestLongtitude { get; set; }
        public Nullable<double> LatestLatitude { get; set; }
    }
}
