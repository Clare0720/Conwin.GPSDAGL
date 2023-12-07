using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.GpsKaoHeShuJuTongBu;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IGpsKaoHeShuJuTongBuService : IBaseService<object>
    {
        ServiceResult<TongBuOutput>  GetGpsKaoHeShuJu(GetGpsKaoHeShuJuInput input);
    }
}
