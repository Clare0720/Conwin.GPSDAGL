using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities.QingYuanSync.CheLiang;
using Conwin.GPSDAGL.Entities.QingYuanSync.YeHu;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface IQingYuanYZShuJuTongBuService : IBaseService<object>
    {
        ServiceResult<object>  GetNewYeHu(GetNewYeHuInput input);
        ServiceResult<object> GetQingYuanYZCheLiang(GetQingYuanYZCheLiangInput input);
        #region 新版同步接口，暂与旧接口并存，防止回滚丢失
        ServiceResult<object> GetYeHuList(GetNewYeHuInput input);
        ServiceResult<object> GetCheLiangList(GetNewCheLiangInput input);
        #endregion

        #region 新的业户同步规则以及同步车辆业户状态
        /// <summary>
        /// 同步业户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetShengGpsYeHuList(QueryData dto);
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetShengGpsVehicleInfo(QueryData dto);
        /// <summary>
        /// 同步车辆营运状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetShengGpsVehicleInfoNew(QueryData dto);
        /// <summary>
        /// 同步车和业户的关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetShengGpsVehicleBaseInfo(QueryData dto);
        /// <summary>
        /// 清远企业注册功能查询企业用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryShengGpsYeHuList(QueryData dto);



        /// <summary>
        /// 获取地市车辆信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetVehicleInformation(QueryData dto);


        /// <summary>
        /// 获取省gps车辆经营范围
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetVehicleConfiguration(QueryData dto);

        /// <summary>
        /// 同步省gps两客业户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> GetYeHuTwoGuestsList(QueryData dto);
        #endregion
    }
}
