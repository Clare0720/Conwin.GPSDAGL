using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IGPSTongXunPeiZhiService: IBaseService<GPSTongXunPeiZhiDto>
    {
        ServiceResult<GPSTongXunPeiZhiDto> GetGPSPeiZhiXinXi(GPSTongXunPeiZhiReqDto dto);
        ServiceResult<bool> SetGPSPeiZhiXinXi(GPSTongXunPeiZhiDto dto);
        ServiceResult<GPSTongXunPeiZhiDto> GetGPSPeiZhiXinXiByChePaiHao(GPSTongXunPeiZhiReqDto dto);
    }
}
