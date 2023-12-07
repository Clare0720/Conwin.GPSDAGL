using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Services;
using System;
namespace Conwin.GPSDAGL.Services.Interfaces
{
    public partial interface IFuWuShangXinXiService : IBaseService<FuWuShangDto>
    {
        ServiceResult<bool> Create(string reqid, FuWuShangExDto dto);
        ServiceResult<bool> Update(string reqid, FuWuShangExDto dto);
        /// <summary>
        /// 备案审核添加服务商
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingMaterials(ServiceProviderDto dto);

        /// <summary>
        /// 下载所有附属文件
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> BatchDownload(string material);
        /// <summary>
        /// 市政府备案审核
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingReview(ServiceProviderDto dto);
        ServiceResult<FuWuShangExDto> Get(Guid id);
        /// <summary>
        /// 获取服务商信息
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<ServiceProviderSetDto> GetProviderName(string providerName);


        /// <summary>
        ///获取验证码
        /// </summary>
        /// <param name="emailParameter"></param>
        /// <returns></returns>
        ServiceResult<GetEmailDto> GetVerificationCode(GetEmailDto emailParameter);
        /// <summary>
        /// 机构重新审核获取服务商信息
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<bool> GetMechanism(string providerName);
        ServiceResult<QueryResult> Query(QueryData queryData);
        /// <summary>
        /// 第三方端查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> ThirdPartyQuery(QueryData queryData);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportThirdParty(QueryData queryData);

        /// <summary>
        /// 第三方机构重新备案
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingAgainMaterials(ServiceProviderDto dto);

        /// <summary>
        /// 获取服务商信息
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<ServiceProviderSetDto> GetDataDyNumber(string providerName);
    }
}
