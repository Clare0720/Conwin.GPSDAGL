using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Entities.PersonalInfo
{
    public class PersonalFaceInfo
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<Guid> PersonalId { get; set; }
        public string FaceID { get; set; }

        /// <summary>
        ///  1 = 地平线
        /// </summary>
        public Nullable<int> FaceIDPlatformType { get; set; }

        public string SYS_ZuiJinXiuGaiRen { get; set; }
        public string SYS_ZuiJinXiuGaiRenID { get; set; }
        public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }
        public string SYS_ChuangJianRen { get; set; }
        public string SYS_ChuangJianRenID { get; set; }
        public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }
        public string SYS_ShuJuLaiYuan { get; set; }
        public Nullable<int> SYS_XiTongZhuangTai { get; set; }
        public string SYS_XiTongBeiZhu { get; set; }
    }
}
