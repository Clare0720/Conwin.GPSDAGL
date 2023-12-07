using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public partial interface IJiaShiYuanService : IBaseService<JiaShiYuanDto>
    {
        /// <summary>
        /// 驾驶员列表信息查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> Query(QueryData queryData);

        /// <summary>
        /// 驾驶员信息导出
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        ServiceResult<JiaShiYuanExportResDto> Export(JiaShiYuanSearchDto searchDto);

        /// <summary>
        /// 驾驶员详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResult<JiaShiYuanDetailResDto> Detail(string id);

        /// <summary>
        /// 创建驾驶员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> Create(JiaShiYuanCreateReqDto dto, UserInfoDto userInfo);

        /// <summary>
        /// 修改驾驶员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> Update(JiaShiYuanUpdateReqDto dto, UserInfoDto userInfo);

        /// <summary>
        /// 删除驾驶员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> Delete(string[] ids, UserInfoDto userInfo);

        /// <summary>
        /// 聘用/解聘驾驶员
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> HireOrDismissal(JiaShiYuanHireReqDto dto, UserInfoDto userInfo);

        /// <summary>
        /// 驾驶员绑定车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> BindVehicle(JiaShiYuanVehicleReqDto dto, UserInfoDto userInfo);

    }
}
