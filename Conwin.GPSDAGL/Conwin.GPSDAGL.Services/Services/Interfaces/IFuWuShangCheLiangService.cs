using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang;
using System;

namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface IFuWuShangCheLiangService : IBaseService<FuWuShangCheLiangDto>
    {
        /// <summary>
        /// 服务商终端信息审核
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> ZhongDuanXinXiShengHe(FuWuShangSubmitReviewDto dto);

        /// <summary>
        /// 服务商车辆档案列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> Query(QueryData queryData);

        /// <summary>
        /// 服务商车辆档案列表Excel导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<Guid?> QueryToExcel(QueryData queryData);

        /// <summary>
        /// 添加服务商车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<Guid> Create(FuWuShangCheLiangUpdateDto dto);

        /// <summary>
        /// 修改服务商车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> Update(FuWuShangCheLiangUpdateDto dto);

        /// <summary>
        /// 获取服务商车辆详情
        /// </summary>
        /// <param name="fwsCheLiangId"></param>
        /// <returns></returns>
        ServiceResult<FuWuShangCheLiangUpdateDto> Detail(Guid fwsCheLiangId);

        /// <summary>
        /// 更新服务商车辆终端(新增/修改)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateZhongDuan(FuWuShangCheLiangZhongDuanUpdateDto dto);

        /// <summary>
        /// 获取服务商车辆终端详情
        /// </summary>
        /// <param name="fwsCheLiangId"></param>
        /// <returns></returns>
        ServiceResult<FuWuShangCheLiangZhongDuanUpdateDto> ZhongDuanDetail(Guid fwsCheLiangId);

        /// <summary>
        /// 删除服务商车辆档案
        /// </summary>
        /// <param name="fwsCheLiangId"></param>
        /// <returns></returns>
        ServiceResult<bool> Delete(Guid fwsCheLiangId);

        /// <summary>
        /// 更新服务商车辆保险信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> AddOrUpdateFuWuShangCheLiangBaoXianXinXi(UpdateFuWuShangCheLiangBaoXianXinXiDto dto);
        /// <summary>
        /// 获取服务商车辆保险信息
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        ServiceResult<FuWuShangCheLiangBaoXianXinXiResponseDto> GetFuWuShangCheLiangBaoXianXinXi(Guid? cheLiangId);
        /// <summary>
        /// 更新服务商车辆业户联系信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateFuWuShangYeHuLianXiXinXi(UpdateFuWuShangYeHuBaoXianXinXiRequestDto dto);
        /// <summary>
        /// 获取服务商车辆业户联系信息
        /// </summary>
        /// <param name="CheLiangId"></param>
        /// <returns></returns>
        ServiceResult<UpdateFuWuShangYeHuBaoXianXinXiResponseDto> GetFuWuShangYeHuLianXiXinXi(Guid CheLiangId);


    }
}
