using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.DtosExt.GPSCheLiangJianKong;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    /// <summary>
    /// 企业GPS车辆监控依赖接口
    /// </summary>
    public interface IGPSCheLiangJianKongService : IBaseService<object>
    {
        ServiceResult<QueryResult> QueryVehicleByUser(CheLiangQiYeShiPingQueryDto queryData);
    }
}
