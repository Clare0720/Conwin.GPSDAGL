
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class JiaShiYuanDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string Name { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Cellphone { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> IDCardType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string IDCard { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> WorkingStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string OrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheLiangId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> EntryDate { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> DismissalDate { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Certification { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> Sex { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string GuoJi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string HuKouDiZhi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> Birthday { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> CertificationEndTime { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FaZhengJiGou { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string LianXiDiZhi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShenFenZhengZhengMian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShenFenZhengFanMian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaShiYuanZhengMian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> JiaZhaoChuCiShenLing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhunJiaCheXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaZhaoHaoMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaZhaoBianHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> NianJianRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> JiaZhaoYouXiaoQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaShiZhengZhengMian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JiaShiZhengFanMian { get; set; }

}

}
