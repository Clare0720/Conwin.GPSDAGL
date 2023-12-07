using Conwin.GPSDAGL.Services.DtosExt.BaseData;
using Conwin.GPSDAGL.Services.Interfaces;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IBaseDataService : IBaseService<object>
    {
        List<GetAreaInfoDto> GetDistrictsList(QueryDistrictsDto dto);
    }
}
