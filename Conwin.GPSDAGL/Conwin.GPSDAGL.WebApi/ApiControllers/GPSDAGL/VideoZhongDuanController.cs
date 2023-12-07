using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(VideoZhongDuanController))]
    public class VideoZhongDuanController : BaseApiController
    {
        private readonly IVideoZhongDuanService _videoZhongDuanService;

        public VideoZhongDuanController(IVideoZhongDuanService videoZhongDuanService)
        {
            this._videoZhongDuanService = videoZhongDuanService;
        }

        [HttpPost]
        [Route("Get")]
        public object Get([FromBody] string requestString)
        {
            var id = new Guid(base.CWRequestParam.body.ToString());
            return _videoZhongDuanService.Get(id, base.UserInfo);
        }

        //006600200071
        [HttpPost]
        [Route("Confirm")]
        public object Confirm([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<CheLiangVideoZhongDuanConfirmDto>();
            return _videoZhongDuanService.Confirm(dto,base.UserInfo);
        }
    }
}