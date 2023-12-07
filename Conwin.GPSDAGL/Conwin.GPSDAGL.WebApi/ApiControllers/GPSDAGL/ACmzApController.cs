using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services;
using System;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Conwin.GPSDAGL.Services.Services.Interfaces;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(ACmzApController))]
    public class ACmzApController : BaseApiController
    {

        private IACmzApService _aCmzApController;

        public ACmzApController(IACmzApService cmzApService)
        {
            _aCmzApController = cmzApService;
        }





        /// <summary>
        /// 获取安全员列表
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _aCmzApController.Query(CWRequestParam.GetBody<QueryData>());


        }






    }
}

