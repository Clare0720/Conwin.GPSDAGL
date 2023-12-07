using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IVideoZhongDuanService : IBaseService<object>
    {
        ServiceResult<VideoZhongDuanXinXiExDto> Get(Guid id, UserInfoDto userInfo);

        ServiceResult<bool> Confirm(CheLiangVideoZhongDuanConfirmDto dto,UserInfoDto userInfo);
    }
}
