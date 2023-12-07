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
    public interface IOrganizationService : IBaseService<OrgBaseInfoDto>
    {
        ServiceResult<bool> Delete(string sysid, string reqid, Guid[] ids, UserInfoDto userInfo);
        ServiceResult<bool> Cancel(string reqid, Guid[] ids);
        ServiceResult<bool> Normal(string reqid, Guid[] ids);
    }
}
