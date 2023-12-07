
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangYeHuDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string BaseId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> OrgType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgShortName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgName { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SuoShuJianKongPingTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JingYingXuKeZhengHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> JingYingXuKeZhengYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<bool> JingYingXuKeZhengChangQiYouXiao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string GongShangYingYeZhiZhaoHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> GongShangYingYeZhiZhaoYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<bool> GongShangYingYeZhiZhaoChangQiYouXiao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string QiYeXingZhi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LianXiRen { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuanZhenHaoMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LianXiFangShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ShiFouGeTiHu { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ShenHeZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JingJiLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SuoShuQiYe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.Guid> QiYeBiaoZhiId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int IsConfirmInfo { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheHuiXinYongDaiMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string GeTiHuShenFenZhengHaoMa { get; set; }

}

}
