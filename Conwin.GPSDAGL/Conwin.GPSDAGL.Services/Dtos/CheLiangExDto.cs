
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangExDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JingYingFanWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> CheLiangBiaoZhiId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CheShenYanSe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangNengLi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XingShiZhengHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XingShiZhengDiZhi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> XingShiZhengYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> XingShiZhengNianShenRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangBaoXiaoZhongLei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> CheLiangBaoXiaoDaoJieZhiRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> XingShiZhengTiXingTianShu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> XingShiZhengDengJiRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XingShiZHengSaoMiaoJianId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> RanLiao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string PaiQiLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZongZhiLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> ZhengBeiZhiLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> HeZaiZhiLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FaDongJiHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<decimal> CheGao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<decimal> CheChang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<decimal> CheKuan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XingHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiShuDengJi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string AnZhuangDengJi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> ErWeiRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> XiaCiErWeiRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> ShenYanYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DaoLuYunShuZhengHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> DaoLuYunShuZhengYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> DaoLuYunShuZhengNianShenRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> DaoLuYunShuZhengTiXingTianShu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuoXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZuoWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> DunWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> ChuChangRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CheZhouShu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JieBoCheLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string HuoWuMingCheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShiFaDi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string QiFaDi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShiFaZhan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string QiDianZhan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> HuoWuDunWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuiJinXiuGaiRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhouShu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<double> ZhunQianYinZongZhiLiang { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangPinPai { get; set; }

}

}
