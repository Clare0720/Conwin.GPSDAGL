using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.GPSDAGL.Services.Services;
using System;
using System.Collections.Generic;

namespace Conwin.GPSDAGL.Services.Interfaces
{
    public partial interface IYeHuService : IBaseService<Dtos.CheLiangYeHuDto>
    {
        ServiceResult<bool> Create(string sysid, CheLiangYeHuDto modelDto, UserInfoDto userInfo);
        /// <summary>
        /// 企业审核是否通过，企业审核驳回
        /// </summary>
        /// <param name="modelDto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> EnterpriseAudit(CheLiangYeHuDto modelDto, UserInfoDto userInfo);
        ServiceResult<bool> Update(string sysid, CheLiangYeHuDto modelDto, UserInfoDto userInfo);
        ServiceResult<bool> Delete(Guid[] ids, UserInfoDto userInfo);
        ServiceResult<bool> Cancel(Guid[] ids, UserInfoDto userInfo);
        ServiceResult<bool> Normal(Guid[] ids, UserInfoDto userInfo);
        ServiceResult<CheLiangYeHuDto> Get(Guid id);
        ServiceResult<QueryResult> Query(QueryData queryData, UserInfoDto UserInfo);
        ServiceResult<QueryResult> QueryAll(QueryData queryData);
        ServiceResult<object> GetQiYeXinXiByYingYeZhiZhaoHao(string yingYeZhiZhaoHao);
        ServiceResult<QueryResult> QueryForPersonalInfoMobile(QueryData queryData, UserInfoDto userInfo);

        ServiceResult<bool> AddFuWuShangGuanLianXinXi(QiYeFuWuShangGuanLianXinXiDto modelDto);
        ServiceResult<bool> EditFuWuShangGuanLianXinXi(QiYeFuWuShangGuanLianXinXiDto modelDto);
        ServiceResult<bool> DeleteFuWuShangGuanLianXinXi(Guid[] ids, UserInfoDto userInfo);
        ServiceResult<QueryResult> QueryFuWuShangGuanLianXinXi(QueryData queryData);
        ServiceResult<object> GetFuWuShangGuanLianXinXi(Guid id);

        ServiceResult<QueryResult> QueryForJianKongPingTaiXinXi(QueryData queryData);
        ServiceResult<QueryResult> ConditionQueryFuWuShangGuanLianXinXi(QueryData queryData);

        ServiceResult<CheLiangYeHuDto> GetByOrgCode(string orgCode);
        ServiceResult<int> GetYeHuConfirmInfoStatus(string orgCode);
        ServiceResult<ExportResponseInfoDto> ExportQiYeXinXi(QueryData queryData);
        ServiceResult<object> GetJiaShiYuanXinXi(string idCard);
        ServiceResult<bool> QiYeSynchronization(QiYeDataSynDto dto);

        /// <summary>
        /// 导入企业信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        ServiceResult<bool> ImportUpdate(ImportFuWu body);


        /// <summary>
        /// 导入网约车企业
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        ServiceResult<bool> ImportAppointmentEnterprise(ImportFuWu body);
    }
}
