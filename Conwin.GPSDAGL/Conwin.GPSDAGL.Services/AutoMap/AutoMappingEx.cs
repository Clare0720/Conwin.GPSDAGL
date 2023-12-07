using AutoMapper;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
//using Conwin.GPSDAGL.Services.Dtos;
namespace Conwin.GPSDAGL.Services.AutoMap
{
    public partial class Profiles
    {
        private void ConfigureMappingCustom()
        {
            //Mapper.CreateMap<Member, MemberDto>()
            //    .ForMember(p => p.EnterpriseName, u => u.MapFrom(s => s.Enterprise.Name));
            //      Mapper.CreateMap<GeRenZhuCeRenZhengXinXi, GeRenZhuCeRenZhengXinXiExDto>()
            //    .ForAllMembers(p=>p.ExplicitExpansion());
            //Mapper.CreateMap<GeRenZhuCeRenZhengXinXiExDto, GeRenZhuCeRenZhengXinXi>()
            //    .ForAllMembers(p=>p.ExplicitExpansion());
            Mapper.CreateMap<CheLiangYeHuDto, OrgBaseInfo>();
            Mapper.CreateMap<CheLiangYeHuDto, CheLiangYeHu > ();
            Mapper.CreateMap<CheLiangYeHu, CheLiangYeHuDto>();
            Mapper.CreateMap<CheLiangAddDto, CheLiang>();
            Mapper.CreateMap<OrgBaseInfo, CheLiangYeHuDto>();
        }
    }
}