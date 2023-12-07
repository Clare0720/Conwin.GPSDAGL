using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Services;

namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface IAnZhuangZhongDuanService : IBaseService<CheLiangGPSZhongDuanXinXiDto>
    {
        /// <summary>
        /// 车辆终端安装列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> Query(QueryData queryData);
        /// <summary>
        /// 导出车辆终端安装列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportZhongDuanAnZhuangInfo(QueryData queryData);

    }
}
