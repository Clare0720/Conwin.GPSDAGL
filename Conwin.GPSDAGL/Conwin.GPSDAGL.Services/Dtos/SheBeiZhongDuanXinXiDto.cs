
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class SheBeiZhongDuanXinXiDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> SheBeiLeiBie { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhongDuanLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SheBeiXingHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShengChanChangJia { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChangJiaBianHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XingHaoBianMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ShiYongCheXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string DingWeiMoKuai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string TongXunMoShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> GuoJianPiCi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZhongDuanBianMa { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> GongGaoRiQi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string GongGaoPiWenFuJianId { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuiJinXiuGaiRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZhuangTai { get; set; }

}

}
