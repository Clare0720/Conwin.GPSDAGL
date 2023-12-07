using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Conwin.GPSDAGL.Services.Dtos
{
    [DataContract(IsReference = true)]
    public class CheLiangAddDto : EntityMetadataDto 
    {

        [DataMember(EmitDefaultValue = false)]
        public string JingYingFanWei { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Nullable<int> YeWuBanLiZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Nullable<int> ShiFouGuFei { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ChuangJianRenOrgCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ZuiJinXiuGaiRenOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? YeHuOrgType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CheDuiOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? CheDuiOrgType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FuWuShangOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Description("车牌号")]
        public string ChePaiHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("车牌颜色")]
        public string ChePaiYanSe { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("车辆类型")]
        public Nullable<int> CheLiangLeiXing { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("车辆种类")]
        public Nullable<int> CheLiangZhongLei { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("车载电话")]
        public string CheZaiDianHua { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("营运状态")]
        public string YunZhengZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("所属平台")]
        public string SuoShuPingTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("车架号")]
        public string CheJiaHao { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("营运证号")]
        public string YunYingZhengHao { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("年审状态")]
        public int NianShenZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("辖区省")]
        public string XiaQuSheng { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("辖区市")]
        public string XiaQuShi { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("辖区县")]
        public string XiaQuXian { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("业户代码")]
        public string YeHuOrgCode { get; set; }
        [Description("备注")]
        [DataMember(EmitDefaultValue = false)]
        public string Remark { get; set; }

        [Description("人工审核状态")]
        [DataMember(EmitDefaultValue = false)]
        public  int ManualApprovalStatus { get; set; }













    }


   public class CheLiangExInfoDto
    {
        public string CheLiangId { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public Nullable<System.Guid> CheLiangBiaoZhiId { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public Nullable<int> CheZhouShu { get; set; }

        [Description("车身颜色")]
        public Nullable<int> CheShenYanSe { get; set; }
        [Description("车辆能力")]
        public string CheLiangNengLi { get; set; }
        [Description("车辆品牌")]
        public string CheLiangPinPai { get; set; }
        [Description("车辆型号")]
        public string XingHao { get; set; }
        [Description("道路运输证号")]
        public string DaoLuYunShuZhengHao { get; set; }
        [Description("车辆道路运输证年审时间")]
        public Nullable<System.DateTime> DaoLuYunShuZhengNianShenRiQi { get; set; }
        [Description("车辆道路运输证号有效期")]
        public Nullable<System.DateTime> DaoLuYunShuZhengYouXiaoQi { get; set; }
        [Description("车辆道路运输证提醒天数")]
        public Nullable<int> DaoLuYunShuZhengTiXingTianShu { get; set; }
        [Description("行驶证号")]
        public string XingShiZhengHao { get; set; }
        [Description("行驶证年审时间")]
        public Nullable<System.DateTime> XingShiZhengNianShenRiQi { get; set; }
        [Description("行驶证年审有效期")]
        public Nullable<System.DateTime> XingShiZhengYouXiaoQi { get; set; }
        [Description("行驶证年审提醒天数")]
        public Nullable<int> XingShiZhengTiXingTianShu { get; set; }
        [Description("行驶证登记日期")]
        public Nullable<System.DateTime> XingShiZhengDengJiRiQi { get; set; }
        [Description("行驶证扫描件照片")]
        public string XingShiZHengSaoMiaoJianId { get; set; }
        [Description("行驶证地址")]
        public string XingShiZhengDiZhi { get; set; }
        [Description("二维日期")]
        public Nullable<System.DateTime> ErWeiRiQi { get; set; }
        [Description("下次二维日期")]
        public Nullable<System.DateTime> XiaCiErWeiRiQi { get; set; }
        [Description("审验有效期")]
        public Nullable<System.DateTime> ShenYanYouXiaoQi { get; set; }
        [Description("车辆保险种类")]
        public string CheLiangBaoXiaoZhongLei { get; set; }
        [Description("车辆保险到期时间")]
        public Nullable<System.DateTime> CheLiangBaoXiaoDaoJieZhiRiQi { get; set; }
        [Description("出厂日期")]
        public Nullable<System.DateTime> ChuChangRiQi { get; set; }
        [Description("发动机编号")]
        public string FaDongJiHao { get; set; }
        [Description("总质量(kg)")]
        public string ZongZhiLiang { get; set; }
        [Description("车高(mm)")]
        public Nullable<decimal> CheGao { get; set; }
        [Description("车长(mm)")]
        public Nullable<decimal> CheChang { get; set; }
        [Description("车宽(mm)")]
        public Nullable<decimal> CheKuan { get; set; }
        [Description("吨位")]
        public Nullable<double> DunWei { get; set; }
        [Description("准牵引总质量")]
        public Nullable<double> ZhunQianYinZongZhiLiang { get; set; }
        [Description("整备质量(Kg)")]
        public Nullable<double> ZhengBeiZhiLiang { get; set; }
        [Description("核载质量(Kg)")]
        public Nullable<double> HeZaiZhiLiang { get; set; }
        [Description("排气量")]
        public string PaiQiLiang { get; set; }
        [Description("燃料")]
        public Nullable<int> RanLiao { get; set; }
        [Description("座型")]
        public string ZuoXing { get; set; }
        [Description("座位")]
        public Nullable<int> ZuoWei { get; set; }
        [Description("技术等级")]
        public string JiShuDengJi { get; set; }
        [Description("装备等级")]
        public string AnZhuangDengJi { get; set; }
        [Description("轴数")]
        public Nullable<int> ZhouShu { get; set; }

        [Description("经营范围")]
        public string JingYingFanWei { get; set; }
        [Description("货物名称")]
        public string HuoWuMingCheng { get; set; }
        [Description("货物吨位")]
        public Nullable<double> HuoWuDunWei { get; set; }
        [Description("始发地")]
        public string ShiFaDi { get; set; }

        [Description("始发站")]
        public string ShiFaZhan { get; set; }
        [Description("讫发地")]
        public string QiFaDi { get; set; }
        [Description("讫点站")]
        public string QiDianZhan { get; set; }
        [Description("接驳车辆")]
        public string JieBoCheLiang { get; set; }

    }
}
