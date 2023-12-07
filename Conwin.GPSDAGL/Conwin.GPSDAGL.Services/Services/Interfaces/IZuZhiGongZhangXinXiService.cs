using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
   public interface IZuZhiGongZhangXinXiService : IBaseService<ZuZhiGongZhangXinXiDto>
    {
        ServiceResult<bool> Create(string reqid, ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo);
        ServiceResult<bool> Update(string reqid, ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo);
        ServiceResult<bool> Delete(string reqid, Guid[] ids, UserInfoDto userInfo);
        ServiceResult<ZuZhiGongZhangXinXiDto> Get(Guid id);
        ServiceResult<QueryResult> Query(ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo, int page, int rows);
    }
}
