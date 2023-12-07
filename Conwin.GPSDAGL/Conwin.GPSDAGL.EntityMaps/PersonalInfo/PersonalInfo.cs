using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Conwin.GPSDAGL.Entities.PersonalInfo
{
    public class PersonalInfo
    {
        public Nullable<Guid>  Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 固定值： 男 / 女
        /// </summary>
        public string Sex { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 企业法人 = 1,
        /// 管理人员 = 2,
        /// 工作人员 = 3,
        /// 司机 = 4
        /// 99=其他
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 人员职务
        /// </summary>
        public string[] Positions { get; set; }

        public string Cellphone { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> IDCardType { get; set; }
        public string IDPhoto { get; set; }

        public string NativePlace { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }

        /// <summary>
        /// 1=待确认，
        /// 2=在职，
        /// 3=离职
        /// </summary>
        public Nullable<int> WorkingStatus { get; set; }

        /// <summary>
        /// 正常，
        /// 异常，
        /// 注销，
        /// 作废
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///  附件对象数组 json字符串
        /// </summary>
        public string Attachments { get; set; }

        /// <summary>
        /// 扩展对象，数据为从业人员时记录从业人员信息 json字符串
        /// </summary>
        public string ExtInfo { get; set; }

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