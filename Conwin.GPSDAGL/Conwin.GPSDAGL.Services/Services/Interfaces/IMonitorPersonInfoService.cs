using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IMonitorPersonInfoService : IBaseService<MonitorPersonInfo>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ServiceResult<bool> Create(MonitorPersonInfo model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ServiceResult<bool> Delete(Guid[] ids);
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<object> Get(Guid id);
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> Query(QueryData queryData);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ServiceResult<bool> Update(MonitorPersonInfoDto model);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>

        ServiceResult<ExportResponseInfoDto> ExportMonitorPersonInfo(QueryData queryData);


    }
}
