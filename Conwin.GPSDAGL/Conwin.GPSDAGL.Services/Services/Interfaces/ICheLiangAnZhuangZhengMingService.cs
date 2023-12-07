using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing;
using Conwin.GPSDAGL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services.Interfaces
{
    public interface ICheLiangAnZhuangZhengMingService: IBaseService<CheLiangAnZhuangZhengMingDto>
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> QueryList(QueryData dto);
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseDto> ExportAnZhuangZhengMing(QueryListRequestDto search);
        /// <summary>
        /// 生成重型货车智能视频安装承诺函
        /// </summary>
        /// <param name="CheLiangId"></param>
        /// <param name="userInfo"></param>
        ServiceResult<bool> GenerateVideoDeviceInstallCert(Guid CheLiangId, UserInfoDto userInfo);
        /// <summary>
        /// 生成智能视频监控报警装置安装承诺函(保险)
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        ServiceResult<bool> ZhiNengZhiPinChengNuoHanByBaoXian(Guid cheLiangId, UserInfoDto userInfo);
    }
}
