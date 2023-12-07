using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{


    public partial interface IACmzApService
    {
        ServiceResult<QueryResult> Query(QueryData queryData);
    }
}
