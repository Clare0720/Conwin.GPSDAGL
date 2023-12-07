using Conwin.EntityFramework;
using System;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Entities
{
    public partial class CheLiang : EntityMetadata 
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public Nullable<int> CheLiangLeiXing { get; set; }
        public Nullable<int> CheLiangZhongLei { get; set; }
        public string CheZaiDianHua { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public Nullable<int> ZaiXianZhuangTai { get; set; }
        public Nullable<int> ShiFouGuFei { get; set; }
        public string YeHuOrgCode { get; set; }
        public Nullable<int> YeHuOrgType { get; set; }
        public string CheDuiOrgCode { get; set; }
        public string FuWuShangOrgCode { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public string SuoShuPingTai { get; set; }
        public string CheJiaHao { get; set; }
        public string YunYingZhengHao { get; set; }
        public Nullable<int> NianShenZhuangTai { get; set; }
        public string Remark { get; set; }
        public string YunZhengZhuangTai { get; set; }
        public string YunZhengYingYunZhuangTai { get; set; }
        public int ManualApprovalStatus { get; set; }
        public string CreateCompanyCode { get; set; }
        public string JingYingFanWei { get; set; }
        public int GPSAuditStatus { get; set; }
        public int BusinessHandlingResults { get; set; }
        public int IsHavVideoAlarmAttachment { get; set; }
    }
}
