using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.DtosExt.User;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public partial interface IUserService : IBaseService<object>
    {
        ServiceResult<BindAccountDto> GetUserWechatOpenIdByOrgCode(OrgInfoDto orgInfoDto);

        ServiceResult<BindAccountDto> GetUserWechatOpenIdByYeHu(QueryWechatOpenIdByYeHuDto yeHuDto);
    }
}
