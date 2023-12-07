using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.VehicleOnNetStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface ICheLiangJianKongService : IBaseService<CheLiangDto>
    {
        ServiceResult<object> GetVehicleInfoYeHu(UserInfoDto userInfo);
        ServiceResult<object> GetVehicleInfoCheDui(UserInfoDto userInfo);
        ServiceResult<object> GetVehicleInfoByYeHu(QueryData qr, UserInfoDto userInfo);
        ServiceResult<object> AddVehicleMonitoring(List<CheLiangXinXiInput> dto, UserInfoDto userInfo);
        ServiceResult<object> DelVehicleMonitoring(List<CheLiangXinXiInput> dto, UserInfoDto userInfo);
        ServiceResult<JiGouJieGuoDto> QueryDangQianZuZhiCheLiangZaiXianShu(CheLiangJianKongShuQueryDto queryData);
        ServiceResult<List<JiGouJieGuoDto>> QueryXiaJiZuZhi(CheLiangJianKongShuQueryDto queryData);
        ServiceResult<QueryResult> QueryXiaJiCheLiang(QueryData queryData);
        ServiceResult<QueryResult> QueryCheLiangXinXi(QueryData queryData);
        ServiceResult<List<JiGouResponseDto>> QueryQiYeHuoCheDui(QiYeCheLiangXinXiCanShuDto queryData);

        ServiceResult<List<JiGouResponseDto>> QueryQiYeHuoCheDuiV2(QiYeCheLiangXinXiCanShuDto queryData);
        ServiceResult<JianKongShuResult> QueryJianKongShu(CheLiangJianKongShuQueryDto queryData);

        ServiceResult<QueryResult> QueryJianKongShuCheLiang(QueryData queryData);

        ServiceResult<CheLiangOrgsOutputDto> QueryCheLiangOrgAndUser(CheLiangOrgsInputDto dto);

        /// <summary>
        /// 查询车辆视频监控树内容（下级组织或车辆列表）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<JianKongShuResult> QueryVideoJianKongShu(VideoJianKongShuQueryDto dto);

        /// <summary>
        /// 获取车辆监控企业及车辆数列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<List<QueryTreeYeHuListDto>> QueryJianKongYeHuAndCheLiangShu(QueryTreeYeHuReqDto queryData);

        /// <summary>
        /// 查询车辆视频监控树内容（下级组织或车辆列表）V2
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<JianKongShuResult> QueryVideoJianKongShuV2(VideoJianKongShuQueryDto dto);

        /// <summary>
        /// 获取企业车辆监控树企业搜索列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<JianKongShuResult> QueryQiYeVideoJianKongShu(VideoJianKongShuQueryDto dto);

        ServiceResult<QueryResult> QueryJianKongShuCheLiangV2(QueryData queryData);

        /// <summary>
        /// 获取车辆数统计
        /// </summary>
        /// <returns></returns>
        ServiceResult<VehicleOnNetInfoDto> GetVehicelNumber();
    }
}
