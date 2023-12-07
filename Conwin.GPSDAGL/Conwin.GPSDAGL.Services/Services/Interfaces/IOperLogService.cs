using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.DtosExt.OperLog;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IOperLogService: IBaseService<object>
    {
        /// <summary>
        /// 获取操作日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryOperLogList(QueryData dto);
        /// <summary>
        /// 获取操作日志详情
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        ServiceResult<LogDetailsInfoDto> GetOperLogDetails(Guid? logID);

    }
}
