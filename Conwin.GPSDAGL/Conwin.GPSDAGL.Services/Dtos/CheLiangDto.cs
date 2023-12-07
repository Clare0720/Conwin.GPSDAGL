
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

public partial class CheLiangDto : EntityMetadataDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChePaiYanSe { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CheLiangLeiXing { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> CheLiangZhongLei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheZaiDianHua { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuSheng { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuShi { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string XiaQuXian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ZaiXianZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> ShiFouGuFei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YeHuOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> YeHuOrgType { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheDuiOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string FuWuShangOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ChuangJianRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string ZuiJinXiuGaiRenOrgCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SuoShuPingTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CheJiaHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YunYingZhengHao { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> NianShenZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string Remark { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YunZhengZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string YunZhengYingYunZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int ManualApprovalStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string CreateCompanyCode { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string JingYingFanWei { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int GPSAuditStatus { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int BusinessHandlingResults { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public int IsHavVideoAlarmAttachment { get; set; }

}

}
