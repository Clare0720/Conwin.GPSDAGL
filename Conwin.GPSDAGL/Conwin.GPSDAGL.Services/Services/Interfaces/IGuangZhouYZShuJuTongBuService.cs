using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities.QingYuanSync.CheLiang;
using Conwin.GPSDAGL.Entities.QingYuanSync.YeHu;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IGuangZhouYZShuJuTongBuService : IBaseService<object>
    {
        ServiceResult<QueryResult> GetYunZhengVehicleInfo(QueryData dto);


    }
}
