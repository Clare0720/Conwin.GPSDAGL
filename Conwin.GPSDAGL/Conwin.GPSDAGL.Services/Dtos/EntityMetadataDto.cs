
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{


[DataContract(IsReference = true)]

[KnownType(typeof(FuWuShangDto))]

[KnownType(typeof(OrgBaseInfoDto))]

[KnownType(typeof(CheLiangJianKongShuExDto))]

[KnownType(typeof(CheLiangZuZhiXinXiDto))]

[KnownType(typeof(YongHuCheLiangXinXiDto))]

[KnownType(typeof(ZiDingYiCheLiangJianKongShuDto))]

[KnownType(typeof(YongHuJianKongShuDto))]

[KnownType(typeof(JiaShiYuanDto))]

[KnownType(typeof(CheLiangYeHuDto))]

[KnownType(typeof(CheLiangGPSZhongDuanXinXiDto))]

[KnownType(typeof(CheLiangDingWeiXinXiDto))]

[KnownType(typeof(DangYuanDto))]

[KnownType(typeof(CheLiangExDto))]

[KnownType(typeof(QiYeFuWuShangGuanLianXinXiDto))]

[KnownType(typeof(CheLiangVideoZhongDuanXinXiDto))]

[KnownType(typeof(UserJianKongShuDto))]

[KnownType(typeof(ZhongDuanFileMapperDto))]

[KnownType(typeof(CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto))]

[KnownType(typeof(CheLiangAnZhuangZhengMingDto))]

[KnownType(typeof(ZuZhiGongZhangXinXiDto))]

[KnownType(typeof(FuWuShangCheLiangDto))]

[KnownType(typeof(FuWuShangCheLiangGPSZhongDuanXinXiDto))]

[KnownType(typeof(FuWuShangCheLiangVideoZhongDuanXinXiDto))]

[KnownType(typeof(FuWuShangZhongDuanFileMapperDto))]

[KnownType(typeof(CheLiangVideoZhongDuanConfirmDto))]

[KnownType(typeof(CheLiangBaoXianXinXiDto))]

[KnownType(typeof(CheLiangYeHuLianXiXinXiDto))]

[KnownType(typeof(FuWuShangCheLiangBaoXianXinXiDto))]

[KnownType(typeof(FuWuShangCheLiangYeHuLianXiXinXiDto))]

[KnownType(typeof(SheBeiZhongDuanXinXiDto))]

[KnownType(typeof(FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto))]

[KnownType(typeof(EnterpriseRegisterInfoDto))]

[KnownType(typeof(PartnershipBindingTableDto))]

[KnownType(typeof(VehiclePartnershipBindingDto))]

[KnownType(typeof(AnQuanGuanLiRenYuanDto))]

[KnownType(typeof(MonitorPersonInfoDto))]

[KnownType(typeof(MaterialListOfServiceProviderDto))]

[KnownType(typeof(CheLiangDto))]

[KnownType(typeof(GPSAuditRecordDto))]

public partial class EntityMetadataDto : BaseDto 
{


	[DataMember(EmitDefaultValue = false)]
    public string SYS_ShuJuLaiYuan { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SYS_ChuangJianRenID { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SYS_ChuangJianRen { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SYS_ZuiJinXiuGaiRenID { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SYS_ZuiJinXiuGaiRen { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public Nullable<int> SYS_XiTongZhuangTai { get; set; }


	[DataMember(EmitDefaultValue = false)]
    public string SYS_XiTongBeiZhu { get; set; }

}

}
