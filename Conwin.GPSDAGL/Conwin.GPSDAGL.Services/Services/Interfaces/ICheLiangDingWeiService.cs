using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDingWei;
using Conwin.GPSDAGL.Services.Interfaces;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public partial interface ICheLiangDingWeiService : IBaseService<CheLiangDingWeiXinXiDto>
    {
        /// <summary>
        /// 更新车辆定位信息（有则更新，无则添加）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateGPSInfo(CheLiangDingWeiAddReqDto dto);

        ServiceResult<bool> ZaiXianZhuangTaiChange(ZaiXianZhuangTaiChangeDto dto);

    }
}
