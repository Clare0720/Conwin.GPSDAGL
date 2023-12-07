using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    /// <summary>
    /// 服务商车辆信息
    /// </summary>
    [ApiPrefix(typeof(FuWuShangCheLiangController))]
    public class FuWuShangCheLiangController : BaseApiController
    {
        private IFuWuShangCheLiangService _service;
        public FuWuShangCheLiangController(IFuWuShangCheLiangService service)
        {
            _service = service;
        }

        /// <summary>
        /// 服务商提交终端信息审核  006600200078-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ZhongDuanXinXiShengHe")]
        public object ZhongDuanXinXiShengHe([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangSubmitReviewDto>();
            return _service.ZhongDuanXinXiShengHe(dto);
        }

        //服务商车辆档案列表  006600200078-1.0
        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.Query(dto);
        }

        //服务商车辆档案列表Excel导出  006600200079-1.0
        [HttpPost]
        [Route("QueryToExcel")]
        public object QueryToExcel([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.QueryToExcel(dto);
        }

        //添加服务商车辆  006600200080-1.0
        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangCheLiangUpdateDto>();
            return _service.Create(dto);
        }

        //修改服务商车辆  006600200081-1.0
        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangCheLiangUpdateDto>();
            return _service.Update(dto);
        }

        //获取服务商车辆详情  006600200082-1.0
        [HttpPost]
        [Route("Detail")]
        public object Detail([FromBody] string requestString)
        {
            Guid id = Guid.Parse(base.CWRequestParam.body.ToString());
            return _service.Detail(id);
        }

        //更新服务商车辆终端  006600200083-1.0
        [HttpPost]
        [Route("UpdateZhongDuan")]
        public object UpdateZhongDuan([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<FuWuShangCheLiangZhongDuanUpdateDto>();
            return _service.UpdateZhongDuan(dto);
        }

        //获取服务商车辆终端详情  006600200084-1.0
        [HttpPost]
        [Route("ZhongDuanDetail")]
        public object ZhongDuanDetail([FromBody] string requestString)
        {
            Guid id = Guid.Parse(base.CWRequestParam.body.ToString());
            return _service.ZhongDuanDetail(id);
        }

        //删除服务商车辆信息  006600200093-1.0
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            Guid id = Guid.Parse(base.CWRequestParam.body.ToString());
            return _service.Delete(id);
        }


        /// <summary>
        /// 更新车辆保险信息 006600200106-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrUpdateFuWuShangCheLiangBaoXianXinXi")]
        public object AddOrUpdateFuWuShangCheLiangBaoXianXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateFuWuShangCheLiangBaoXianXinXiDto>();
            return _service.AddOrUpdateFuWuShangCheLiangBaoXianXinXi(dto);
        }

        /// <summary>
        /// 获取服务商车辆保险信息 006600200107-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFuWuShangCheLiangBaoXianXinXi")]
        public object GetFuWuShangCheLiangBaoXianXinXi([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetFuWuShangCheLiangBaoXianXinXi(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }

        /// <summary>
        /// 更新服务商车辆业户联系信息 006600200108-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateFuWuShangYeHuLianXiXinXi")]
        public object UpdateYeHuLianXiXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateFuWuShangYeHuBaoXianXinXiRequestDto>();
            return _service.UpdateFuWuShangYeHuLianXiXinXi(dto);
        }
        /// <summary>
        /// 获取服务商车辆业户联系信息 006600200109-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFuWuShangYeHuLianXiXinXi")]
        public object GetYeHuLianXiXinXi([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetFuWuShangYeHuLianXiXinXi(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }


    }

}