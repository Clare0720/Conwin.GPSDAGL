using Conwin.EntityFramework;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface ICheLiangService : IBaseService<CheLiangDto>
    {
        ServiceResult<QueryResult> Query(QueryData queryData,string SysId);
        ServiceResult<bool> Update(CheLiangAddDto model);
        /// <summary>
        /// 车辆档案管理人工审核
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> ModifyApproval(CheLiangAddDto dto);
        ServiceResult<object> Create(CheLiangAddDto dto);
        ServiceResult<bool> Delete(List<Guid> ids, UserInfoDto userInfo);
        ServiceResult<bool> UpdateState(CheLiang model, UserInfoDto userInfo);
        ServiceResult<object> GetVehicleBasicInfo(Guid CheLiangId);
        ServiceResult<QueryResult> GetVehicleBasicInfoNew(QueryData queryData);
        ServiceResult<object> GetVehicleDetailedInfo(string CheLiangId);
        ServiceResult<object> AddVehicleDetailedInfo(CheLiangEx dto, UserInfoDto userInfo);
        ServiceResult<object> UpDateVehicleDetailedInfo(CheLiangEx dto, UserInfoDto userInfo);

        ServiceResult<QueryResult> QueryVehicleBasicInfoList(QueryData queryData);

        /// <summary>
        /// 查询没有被当前用户订阅的车辆信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryNotSubscribeVehicleBasicInfoList(QueryData queryData);

        ServiceResult<ZhongDuanXinXiDto> GetZhongDuanXinXi(string cheLiangId);
        ServiceResult<bool> UpdateZhongDuanXinXi(UpdateZhongDuanXinXiDto dto);

        ServiceResult<bool> UpdateZhongDuanBeiAnZhuangTai(UpdateZhongDuanBeiAnZhuangTaiDto dto);
        /// <summary>
        /// 车辆档案导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportCheliangXinXiDto> ExportCheliangXinXi(QueryData queryData);
        /// <summary>
        /// 更新车辆保险信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> AddOrUpdateCheLiangBaoXianXinXi(UpdateCheLiangBaoXianXinXiDto dto);
        /// <summary>
        /// 获取车辆保险信息
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        ServiceResult<CheLiangBaoXianXinXiResponseDto> GetCheLiangBaoXianXinXi(Guid? cheLiangId);
        /// <summary>
        /// 更新车辆业户联系信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateYeHuLianXiXinXi(UpdateYeHuBaoXianXinXiRequestDto dto);
        /// <summary>
        /// 获取车辆业户联系信息详情
        /// </summary>
        /// <param name="CheLiangId"></param>
        /// <returns></returns>
        ServiceResult<UpdateYeHuBaoXianXinXiResponseDto> GetYeHuLianXiXinXi(Guid CheLiangId);

        /// <summary>
        /// 车辆提交备案申请
        /// </summary>
        /// <param name="cheLiangID"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateVehicleRecordState(Guid? cheLiangID);

        /// <summary>
        /// 取消备案车辆管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetCancelRecordVehicleList(QueryData dto);

        /// <summary>
        /// 取消备案车辆导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportCheliangXinXiDto> ExportCancelRecordVehicle(QueryData queryData);
        /// <summary>
        /// 获取车辆年审人工审核参考信息
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        ServiceResult<VehicleAnnualReviewResultDto> GetVehicelAnnualReview(Guid cheLiangId);

        /// <summary>
        /// GPS导入接入证明
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        ServiceResult<GpsAccessInformation> GpsImportAccessCertificate(string cheLiangId);
        /// <summary>
        /// GPS自动审核
        /// </summary>
        /// <returns></returns>
        ServiceResult<bool> AuditAuto();

        /// <summary>
        /// 获取车辆年审人工审核参考信息
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        ServiceResult<GpsAuditInformation> GetAuditResult(Guid cheLiangId);
        /// <summary>
        /// 车辆业务资质查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryVehicleQualification(QueryData queryData);

        /// <summary>
        ///  车辆业务资质导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportCheliangXinXiDto> ExportVehicleQualification(QueryData queryData);

        /// <summary>
        /// 车辆业务资质查询公共
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetOpenVehicleQualification(QueryData queryData);

        /// <summary>
        /// 批量提交备案
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ServiceResult<bool> BatchFiling(List<Guid> ids);


        /// <summary>
        /// 车辆凌晨营运天数统计
        /// </summary>
        /// <returns></returns>
        ServiceResult<List<VehicleOperatingDays>> VehicleOperatingDays(QueryData queryData);
    }
}
