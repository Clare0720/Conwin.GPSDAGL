



using AutoMapper;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
namespace Conwin.GPSDAGL.Services.AutoMap
{

public partial class Profiles : Profile
{
    protected override void Configure()
	{

		Mapper.CreateMap<AnQuanGuanLiRenYuan, AnQuanGuanLiRenYuanDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<AnQuanGuanLiRenYuanDto, AnQuanGuanLiRenYuan>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiang, CheLiangDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangDto, CheLiang>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangAnZhuangZhengMing, CheLiangAnZhuangZhengMingDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangAnZhuangZhengMingDto, CheLiangAnZhuangZhengMing>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangBaoXianXinXi, CheLiangBaoXianXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangBaoXianXinXiDto, CheLiangBaoXianXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangDingWeiXinXi, CheLiangDingWeiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangDingWeiXinXiDto, CheLiangDingWeiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangEx, CheLiangExDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangExDto, CheLiangEx>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi, CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto, CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangGPSZhongDuanXinXi, CheLiangGPSZhongDuanXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangGPSZhongDuanXinXiDto, CheLiangGPSZhongDuanXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangJianKongShuEx, CheLiangJianKongShuExDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangJianKongShuExDto, CheLiangJianKongShuEx>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangVideoZhongDuanConfirm, CheLiangVideoZhongDuanConfirmDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangVideoZhongDuanConfirmDto, CheLiangVideoZhongDuanConfirm>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangVideoZhongDuanXinXi, CheLiangVideoZhongDuanXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangVideoZhongDuanXinXiDto, CheLiangVideoZhongDuanXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangYeHu, CheLiangYeHuDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangYeHuDto, CheLiangYeHu>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangYeHuLianXiXinXi, CheLiangYeHuLianXiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangYeHuLianXiXinXiDto, CheLiangYeHuLianXiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<CheLiangZuZhiXinXi, CheLiangZuZhiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<CheLiangZuZhiXinXiDto, CheLiangZuZhiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<DangYuan, DangYuanDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<DangYuanDto, DangYuan>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<EnterpriseRegisterInfo, EnterpriseRegisterInfoDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<EnterpriseRegisterInfoDto, EnterpriseRegisterInfo>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<EntityMetadata, EntityMetadataDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<EntityMetadataDto, EntityMetadata>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShang, FuWuShangDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangDto, FuWuShang>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiang, FuWuShangCheLiangDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangDto, FuWuShangCheLiang>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiangBaoXianXinXi, FuWuShangCheLiangBaoXianXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangBaoXianXinXiDto, FuWuShangCheLiangBaoXianXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi, FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiDto, FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanXinXi, FuWuShangCheLiangGPSZhongDuanXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanXinXiDto, FuWuShangCheLiangGPSZhongDuanXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiangVideoZhongDuanXinXi, FuWuShangCheLiangVideoZhongDuanXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangVideoZhongDuanXinXiDto, FuWuShangCheLiangVideoZhongDuanXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangCheLiangYeHuLianXiXinXi, FuWuShangCheLiangYeHuLianXiXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangCheLiangYeHuLianXiXinXiDto, FuWuShangCheLiangYeHuLianXiXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<FuWuShangZhongDuanFileMapper, FuWuShangZhongDuanFileMapperDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<FuWuShangZhongDuanFileMapperDto, FuWuShangZhongDuanFileMapper>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<GPSAuditRecord, GPSAuditRecordDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<GPSAuditRecordDto, GPSAuditRecord>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<JiaShiYuan, JiaShiYuanDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<JiaShiYuanDto, JiaShiYuan>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<MaterialListOfServiceProvider, MaterialListOfServiceProviderDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<MaterialListOfServiceProviderDto, MaterialListOfServiceProvider>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<MonitorPersonInfo, MonitorPersonInfoDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<MonitorPersonInfoDto, MonitorPersonInfo>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<OrgBaseInfo, OrgBaseInfoDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<OrgBaseInfoDto, OrgBaseInfo>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<PartnershipBindingTable, PartnershipBindingTableDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<PartnershipBindingTableDto, PartnershipBindingTable>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<QiYeFuWuShangGuanLianXinXi, QiYeFuWuShangGuanLianXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<QiYeFuWuShangGuanLianXinXiDto, QiYeFuWuShangGuanLianXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<SheBeiZhongDuanXinXi, SheBeiZhongDuanXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<SheBeiZhongDuanXinXiDto, SheBeiZhongDuanXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<UserJianKongShu, UserJianKongShuDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<UserJianKongShuDto, UserJianKongShu>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<VehiclePartnershipBinding, VehiclePartnershipBindingDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<VehiclePartnershipBindingDto, VehiclePartnershipBinding>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<YongHuCheLiangXinXi, YongHuCheLiangXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<YongHuCheLiangXinXiDto, YongHuCheLiangXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<YongHuJianKongShu, YongHuJianKongShuDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<YongHuJianKongShuDto, YongHuJianKongShu>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<ZhongDuanFileMapper, ZhongDuanFileMapperDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<ZhongDuanFileMapperDto, ZhongDuanFileMapper>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<ZiDingYiCheLiangJianKongShu, ZiDingYiCheLiangJianKongShuDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<ZiDingYiCheLiangJianKongShuDto, ZiDingYiCheLiangJianKongShu>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		Mapper.CreateMap<ZuZhiGongZhangXinXi, ZuZhiGongZhangXinXiDto>()
		.ForAllMembers(p=>p.ExplicitExpansion());
		Mapper.CreateMap<ZuZhiGongZhangXinXiDto, ZuZhiGongZhangXinXi>()
				.ForAllMembers(p=>p.ExplicitExpansion());

		ConfigureMappingCustom();
	}
}

}

