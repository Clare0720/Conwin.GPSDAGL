using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
   public interface ISheBeiZhongDuanXinXiService: IBaseService<SheBeiZhongDuanXinXiDto>
    {
        /// <summary>
        /// 获取设备终端信息列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QuerySheBeiZhongDuanList(QueryData dto);

    }
}
