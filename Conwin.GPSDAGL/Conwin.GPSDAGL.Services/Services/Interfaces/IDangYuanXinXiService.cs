using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.RenYuan;
using Conwin.GPSDAGL.Services.Enums;
using System;

namespace Conwin.GPSDAGL.Services.Interfaces
{
    public partial interface IDangYuanXinXiService : IBaseService<Dtos.DangYuanDto>
    {
        ServiceResult<QueryResult> Query(QueryData queryData, UserInfoDto userInfo);
        ServiceResult<object> Get(DangYuanDto dto);
        ServiceResult<bool> Create(DangYuanDto model, UserInfoDto userInfo);
        ServiceResult<bool> Update(DangYuanDto model, UserInfoDto userInfo);
        ServiceResult<bool> Delete(Guid[] ids, UserInfoDto userInfo);



    }
}
