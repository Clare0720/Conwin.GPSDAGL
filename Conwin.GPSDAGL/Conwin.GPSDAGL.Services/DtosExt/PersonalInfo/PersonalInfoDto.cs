using Conwin.GPSDAGL.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.PersonalInfo
{
    public class PersonalInfoDto : Conwin.GPSDAGL.Entities.PersonalInfo.PersonalInfo
    {

        /// <summary>
        /// 省运政人员图片Base64数据
        /// </summary>
        public string IDPhotoBase64 { get; set; }
        public string OrgCode { get; set; }
    }

    public class PersonalExtInfoDto
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public string IDCard { get; set; }
        public string DriverLicense { get; set; }
        public string CongYeRenYuanDianNaoBianHao { get; set; }
        public string YeHuMingCheng { get; set; }
        public string YeHuDaiMa { get; set; }
        public Nullable<Guid> YeHuId { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string WeChatNo { get; set; }
        public Nullable<int> OrgType { get; set; }
    }

    /// <summary>
    /// 邀请同事
    /// </summary>
    public class PersonalInfoInviteWorkmateDto
    {
        // 邀请同事标志 0：是 1：否
        public Nullable<int> IsInvite { get; set; }
        public GeRenZhuCeRenZhengXinXiExDto GeRenZhuCeRenZhengXinXi { get; set; }
        public GeRenDiSanFangZhangHaoXinXiDto GeRenDiSanFangZhangHaoXinXi { get; set; }
        public PersonalInfoDto PersonalInfo{ get; set; }
        public string SysId { get; set; }
        public string TargetSysId { get; set; }
        public string IsLogin { get; set; }
    }
}
