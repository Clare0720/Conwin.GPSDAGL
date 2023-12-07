using Conwin.EntityFramework;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(CheLiangJianKongController))]
    public class CheLiangJianKongController : BaseApiController
    {
        private ICheLiangJianKongService _service;
        public CheLiangJianKongController(ICheLiangJianKongService service)
        {
            _service = service;
        }

        //企业GPS-分配监控获取业户列表
        [HttpPost]
        [Route("GetVehicleInfoYeHu")]
        public object GetVehicleInfoYeHu([FromBody] string requestString)
        {
            return _service.GetVehicleInfoYeHu(base.UserInfo);
        }

        //企业GPS-分配监控获取车队列表
        [HttpPost]
        [Route("GetVehicleInfoCheDui")]
        public object GetVehicleInfoCheDui([FromBody] string requestString)
        {
            return _service.GetVehicleInfoCheDui(base.UserInfo);
        }

        //企业GPS-分配监控获取车辆列表
        [HttpPost]
        [Route("GetVehicleInfoByYeHu")]
        public object GetVehicleInfoByYeHu([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _service.GetVehicleInfoByYeHu(dto, base.UserInfo);
        }

        //添加监控权限
        [HttpPost]
        [Route("AddVehicleMonitoring")]
        public object AddVehicleMonitoring([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<List<CheLiangXinXiInput>>();
            return _service.AddVehicleMonitoring(dto, base.UserInfo);
        }

        //移除监控权限
        [HttpPost]
        [Route("DelVehicleMonitoring")]
        public object DelVehicleMonitoring([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<List<CheLiangXinXiInput>>();
            return _service.DelVehicleMonitoring(dto, base.UserInfo);
        }

        //获取当前组织与车辆在线数
        [HttpPost]
        [Route("QueryJianKongShu")]
        public object QueryJianKongShu([FromBody] string requestString)
        {
            return _service.QueryJianKongShu(CWRequestParam.GetBody<CheLiangJianKongShuQueryDto>());
        }

        //获取组织车辆
        [HttpPost]
        [Route("QueryJianKongShuCheLiang")]
        public object QueryJianKongShuCheLiang([FromBody] string requestString)
        {
            return _service.QueryJianKongShuCheLiang(CWRequestParam.GetBody<QueryData>());
        }

        //获取当前组织与车辆在线数
        [HttpPost]
        [Route("QueryDangQianZuZhiCheLiangZaiXianShu")]
        public object QueryDangQianZuZhiCheLiangZaiXianShu([FromBody] string requestString)
        {
            return _service.QueryDangQianZuZhiCheLiangZaiXianShu(CWRequestParam.GetBody<CheLiangJianKongShuQueryDto>());
        }

        //获取下级组织
        [HttpPost]
        [Route("QueryXiaJiZuZhi")]
        public object QueryXiaJiZuZhi([FromBody] string requestString)
        {
            return _service.QueryXiaJiZuZhi(CWRequestParam.GetBody<CheLiangJianKongShuQueryDto>());
        }

        //查询关联车辆
        [HttpPost]
        [Route("QueryXiaJiCheLiangBySysUserId")]
        public object QueryXiaJiCheLiangBySysUserId([FromBody] string requestString)
        {
            return _service.QueryXiaJiCheLiang(CWRequestParam.GetBody<QueryData>());
        }

        //根据用户信息查询车辆信息
        [HttpPost]
        [Route("QueryCheLiangXinXi")]
        public object QueryCheLiangXinXi([FromBody] string requestString)
        {
            return _service.QueryCheLiangXinXi(CWRequestParam.GetBody<QueryData>());
        }

        //获取车队、企业和运营商列表
        [HttpPost]
        [Route("QueryQiYeHuoCheDui")]
        public object QueryQiYeHuoCheDui([FromBody] string requestString)
        {
            return _service.QueryQiYeHuoCheDui(CWRequestParam.GetBody<QiYeCheLiangXinXiCanShuDto>());
        }

        //获取车辆相关的组织列表和用户列表
        [HttpPost]
        [Route("QueryCheLiangOrgAndUser")]
        public object QueryCheLiangOrgAndUser([FromBody] string requestString)
        {
            return _service.QueryCheLiangOrgAndUser(CWRequestParam.GetBody<CheLiangOrgsInputDto>());
        }

        //车辆视频监控树  006600200053-1.0
        [HttpPost]
        [Route("VehicleVideoMonitorTree")]
        public object VehicleVideoMonitorTree([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<VideoJianKongShuQueryDto>();
            return _service.QueryVideoJianKongShu(dto);
        }

        //获取车辆监控树企业搜索列表  006600200120-1.0
        [HttpPost]
        [Route("QueryTreeYeHuList")]
        public object QueryTreeYeHuList([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryTreeYeHuReqDto>();
            return _service.QueryJianKongYeHuAndCheLiangShu(dto);
        }

        //获取车辆监控树企业搜索列表 
        [HttpPost]
        [Route("VehicleVideoMonitorTreeV2")]
        public object VehicleVideoMonitorTreeV2([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<VideoJianKongShuQueryDto>();
            return _service.QueryVideoJianKongShuV2(dto);
        }

        //获取企业车辆监控树企业搜索列表 
        [HttpPost]
        [Route("QueryQiYeVideoJianKongShu")]
        public object QueryQiYeVideoJianKongShu([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<VideoJianKongShuQueryDto>();
            return _service.QueryQiYeVideoJianKongShu(dto);
        }

        //获取监控树车辆查询列表
        [HttpPost]
        [Route("QueryJianKongShuCheLiangV2")]
        public object QueryJianKongShuCheLiangV2([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _service.QueryJianKongShuCheLiangV2(dto);
        }

        //获取车队、企业和运营商列表 V2
        [HttpPost]
        [Route("QueryQiYeHuoCheDuiV2")]
        public object QueryQiYeHuoCheDuiV2([FromBody] string requestString)
        {
            return _service.QueryQiYeHuoCheDuiV2(CWRequestParam.GetBody<QiYeCheLiangXinXiCanShuDto>());
        }


        //获取车辆数统计
        [HttpPost]
        [Route("GetVehicelNumber")]
        public object GetVehicelNumber([FromBody] string requestString)
        {
            return _service.GetVehicelNumber();
        }

    }
}