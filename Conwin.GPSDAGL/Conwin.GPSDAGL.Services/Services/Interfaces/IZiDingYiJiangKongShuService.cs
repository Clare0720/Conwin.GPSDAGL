using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface IZiDingYiJiangKongShuService : IBaseService<object>
    {
        ServiceResult<string> AddTree(CustomMonitorTreeDto dto, UserInfoDto userinfo);

        ServiceResult<QueryResult> GetTreeList(QueryData queryData, UserInfoDto userinfo);

        ServiceResult<bool> DeleteTree(List<string> ids, UserInfoDto userinfo, string mark = null);

        ServiceResult<bool> EnabledTree(string id, UserInfoDto userinfo);

        ServiceResult<JSTreeDTO> GetTree(string id);

        ServiceResult<JSTreeDTO> GetEnabledTreeByUser(UserInfoDto userinfo);

        ServiceResult<string> UpdateTree(CustomMonitorTreeDto dto, UserInfoDto userinfo);
    }
}
