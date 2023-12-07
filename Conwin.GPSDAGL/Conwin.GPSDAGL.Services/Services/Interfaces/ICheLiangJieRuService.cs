using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangJieRu;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface ICheLiangJieRuService : IBaseService<object>
    {
        /// <summary>
        /// 智能视频接入统计列表查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetZhiNengShiPinJieRuXinXi(QueryData dto);
        /// <summary>
        /// 智能视频接入统计列表导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportZhiNengShiPingJieRuDto> ExportZhiNengShiPingJieRuXinXi(QueryData queryData);

        /// <summary>
        /// 查询服务商智能视频接入信息列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetFuWuShangJieRuXinXi(QueryData dto);
        /// <summary>
        /// 导出服务商智能视频接入信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportZhiNengShiPingJieRuDto> ExportFuWuShangJieRuXinXi(QueryData queryData);
        /// <summary>
        /// 查询辖区智能视频接入信息列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetXiaQuJieRuXinXi(QueryData dto);
        /// <summary>
        /// 导出辖区智能视频接入信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportZhiNengShiPingJieRuDto> ExportXiaQuJieRuXinXi(QueryData queryData);
    }
}
