using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Interfaces
{
   public partial interface IAnQuanRenYuanGuanLiService:IBaseService<Dtos.AnQuanGuanLiRenYuanDto>
    {
        ServiceResult<QueryResult> Query(QueryData queryData);

        ServiceResult<object> Get(Guid id);
        ServiceResult<bool> Create(AnQuanGuanLiRenYuanDto model);
        ServiceResult<bool> Update(AnQuanGuanLiRenYuanDto model);
        ServiceResult<bool> Delete(Guid[] ids);
        ServiceResult<ExportResponseInfoDto> ExportQiYeAnQuanRenYuanInfo(QueryData queryData);
    }
}
