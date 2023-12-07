using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using System;
namespace Conwin.GPSDAGL.Services.Interfaces
{
    public partial interface IDiSanFangXinXiService : IBaseService<OrgBaseInfoDto>
    {
        ServiceResult<bool> Create(string sysId, DiSanFangExDto dto);
        ServiceResult<bool> Update(string sysId, DiSanFangExDto dto);
        ServiceResult<DiSanFangExDto> Get(Guid id);
        ServiceResult<QueryResult> Query(QueryData queryData);

    }
}
