using Conwin.EntityFramework;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Attributes;
using Conwin.Framework.ServiceAgent.BaseClasses;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Conwin.Framework.Log4net;

namespace Conwin.GPSDAGL.WebApi.ApiControllers.GPSDAGL
{
    [ApiPrefix(typeof(CheLiangController))]
    public class CheLiangController : BaseApiController
    {
        private ICheLiangService _service;
        public CheLiangController(ICheLiangService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Query")]
        public object Query([FromBody] string requestString)
        {
            return _service.Query(CWRequestParam.GetBody<QueryData>(),CWRequestParam.publicrequest.sysid);
        }

        [HttpPost]
        [Route("Create")]
        public object Create([FromBody] string requestString)
        {
            return _service.Create(CWRequestParam.GetBody<CheLiangAddDto>());
        }

        [HttpPost]
        [Route("Update")]
        public object Update([FromBody] string requestString)
        {
            return _service.Update(CWRequestParam.GetBody<CheLiangAddDto>());
        }

        //006600200154
        [HttpPost]
        [Route("ModifyApproval")]
        public object ModifyApproval([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<CheLiangAddDto>();
            return _service.ModifyApproval(dto);
        }
        [HttpPost]
        [Route("Delete")]
        public object Delete([FromBody] string requestString)
        {
            return _service.Delete(CWRequestParam.GetBody<List<Guid>>(), UserInfo);
        }

        [HttpPost]
        [Route("UpdateState")]
        public object UpdateState([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiang>();
            return _service.UpdateState(dto, base.UserInfo);
        }

        [HttpPost]
        [Route("GetVehicleBasicInfo")]
        public object GetVehicleBasicInfo([FromBody] string requestString)
        {
            var id = new Guid(base.CWRequestParam.body.ToString());
            return _service.GetVehicleBasicInfo(id);
        }

        [HttpPost]
        [Route("GetVehicleBasicInfoNew")]
        public object GetVehicleBasicInfoNew([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<QueryData>();
            return _service.GetVehicleBasicInfoNew(dto);
        }

        [HttpPost]
        [Route("QueryVehicleBasicInfoList")]
        public object QueryVehicleBasicInfoList([FromBody] string requestString)
        {
            return _service.QueryVehicleBasicInfoList(CWRequestParam.GetBody<QueryData>());
        }

        /// <summary>
        /// 查询没有被当前用户订阅的车辆信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryNotSubscribeVehicleBasicInfoList")]
        public object QueryNotSubscribeVehicleBasicInfoList([FromBody] string requestString)
        {
	        return _service.QueryNotSubscribeVehicleBasicInfoList(CWRequestParam.GetBody<QueryData>());
        }

        [HttpPost]
        [Route("GetVehicleDetailedInfo")]
        public object GetVehicleDetailedInfo([FromBody] string requestString)
        {
            var id = base.CWRequestParam.body.ToString();
            return _service.GetVehicleDetailedInfo(id);
        }

        [HttpPost]
        [Route("AddVehicleDetailedInfo")]
        public object AddVehicleDetailedInfo([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiangEx>();
            return _service.AddVehicleDetailedInfo(dto, base.UserInfo);
        }

        [HttpPost]
        [Route("UpDateVehicleDetailedInfo")]
        public object UpDateVehicleDetailedInfo([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<CheLiangEx>();
            return _service.UpDateVehicleDetailedInfo(dto, base.UserInfo);
        }

        [HttpPost]
        [Route("GetZhongDuanXinXi")]
        public object GetZhongDuanXinXi([FromBody] string requestString)
        {
            var cheLiangId = base.CWRequestParam.body.ToString();
            return _service.GetZhongDuanXinXi(cheLiangId);
        }

        [HttpPost]
        [Route("UpdateZhongDuanXinXi")]
        public object UpdateZhongDuanXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateZhongDuanXinXiDto>();
            return _service.UpdateZhongDuanXinXi(dto);
        }


        /// <summary>
        /// 修改车辆终端备案状态
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateZhongDuanBeiAnZhuangTai")]
        public object UpdateZhongDuanBeiAnZhuangTai([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateZhongDuanBeiAnZhuangTaiDto>();
            return _service.UpdateZhongDuanBeiAnZhuangTai(dto);
        }

        [HttpPost]
        [Route("ExportCheliangXinXi")]
        public object ExportCheliangXinXi([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.ExportCheliangXinXi(dto);
        }


        /// <summary>
        /// 更新车辆保险信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrUpdateCheLiangBaoXianXinXi")]
        public object AddOrUpdateCheLiangBaoXianXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateCheLiangBaoXianXinXiDto>();
            return _service.AddOrUpdateCheLiangBaoXianXinXi(dto);
        }

        /// <summary>
        /// 获取车辆保险信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCheLiangBaoXianXinXi")]
        public object GetCheLiangBaoXianXinXi([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetCheLiangBaoXianXinXi(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }

        /// <summary>
        /// 更新车辆业户联系信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateYeHuLianXiXinXi")]
        public object UpdateYeHuLianXiXinXi([FromBody] string requestString)
        {
            var dto = base.CWRequestParam.GetBody<UpdateYeHuBaoXianXinXiRequestDto>();
            return _service.UpdateYeHuLianXiXinXi(dto);
        }
        /// <summary>
        /// 获取车辆业户联系信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetYeHuLianXiXinXi")]
        public object GetYeHuLianXiXinXi([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetYeHuLianXiXinXi(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }

        /// <summary>
        /// 车辆提交备案申请 006600200129-1.0
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateVehicleRecordState")]
        public object UpdateVehicleRecordState([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.UpdateVehicleRecordState(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }
        }


        [HttpPost]
        [Route("GetCancelRecordVehicleList")]
        public object GetCancelRecordVehicleList([FromBody] string requestString)
        {
            return _service.GetCancelRecordVehicleList(CWRequestParam.GetBody<QueryData>());
        }


        [HttpPost]
        [Route("ExportCancelRecordVehicle")]
        public object ExportCancelRecordVehicle([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.ExportCancelRecordVehicle(dto);
        }

        /// <summary>
        /// 获取车辆年审人工审核参考信息
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVehicelAnnualReview")]
        public object GetVehicelAnnualReview([FromBody] string requestString)
        {
            Guid id;
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out id))
            {
                return _service.GetVehicelAnnualReview(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }

        /// <summary>
        /// GPS导入接入证明
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GpsImportAccessCertificate")]
        public object GpsImportAccessCertificate([FromBody] string requestString)
        {
            var cheLiangId = base.CWRequestParam.body.ToString();
            return _service.GpsImportAccessCertificate(cheLiangId);
        }

        /// <summary>
        /// GPS自动审核
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AuditAuto")]
        public object AuditAuto([FromBody] string requestString)
        {
            return _service.AuditAuto();
        }

        /// <summary>
        /// 获取GPS审核结果明细
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAuditResult")]
        public object GetAuditResult([FromBody] string requestString)
        {
            if (Guid.TryParse(Convert.ToString(base.CWRequestParam.body), out Guid id))
            {
                return _service.GetAuditResult(id);
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, ErrorMessage = "参数有误" };
            }

        }
        /// <summary>
        /// 006600200258 车辆业务资质查询
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryVehicleQualification")]
        public object QueryVehicleQualification([FromBody] string requestString)
        {
            return _service.QueryVehicleQualification(CWRequestParam.GetBody<QueryData>());
        }
        /// <summary>
        /// 006600200259 车辆业务资质导出
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExportVehicleQualification")]
        public object ExportVehicleQualification([FromBody] string requestString)
        {
            var dto = CWRequestParam.GetBody<QueryData>();
            return _service.ExportVehicleQualification(dto);
        }

        /// <summary>
        /// 006600200260 车辆业务资质查询
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOpenVehicleQualification")]
        public object GetOpenVehicleQualification([FromBody] string requestString)
        {
            return _service.GetOpenVehicleQualification(CWRequestParam.GetBody<QueryData>());
        }


        [HttpPost]
        [Route("BatchFiling")]
        public object BatchFiling([FromBody] string requestString)
        {
            return _service.BatchFiling(CWRequestParam.GetBody<List<Guid>>());
        }


        /// <summary>
        /// 车辆凌晨营运天数
        /// 006600200266
        /// </summary>
        /// <param name="requestString"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleOperatingDays")]
        public object VehicleOperatingDays([FromBody] string requestString)
        {
            return _service.VehicleOperatingDays(CWRequestParam.GetBody<QueryData>());
        }
    }

}