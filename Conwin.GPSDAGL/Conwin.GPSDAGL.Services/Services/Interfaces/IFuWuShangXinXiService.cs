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
        /// ���������ӷ�����
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingMaterials(ServiceProviderDto dto);

        /// <summary>
        /// �������и����ļ�
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> BatchDownload(string material);
        /// <summary>
        /// �������������
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingReview(ServiceProviderDto dto);
        ServiceResult<FuWuShangExDto> Get(Guid id);
        /// <summary>
        /// ��ȡ��������Ϣ
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<ServiceProviderSetDto> GetProviderName(string providerName);


        /// <summary>
        ///��ȡ��֤��
        /// </summary>
        /// <param name="emailParameter"></param>
        /// <returns></returns>
        ServiceResult<GetEmailDto> GetVerificationCode(GetEmailDto emailParameter);
        /// <summary>
        /// ����������˻�ȡ��������Ϣ
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<bool> GetMechanism(string providerName);
        ServiceResult<QueryResult> Query(QueryData queryData);
        /// <summary>
        /// �������˲�ѯ
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<QueryResult> ThirdPartyQuery(QueryData queryData);
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        ServiceResult<ExportResponseInfoDto> ExportThirdParty(QueryData queryData);

        /// <summary>
        /// �������������±���
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ServiceResult<bool> FilingAgainMaterials(ServiceProviderDto dto);

        /// <summary>
        /// ��ȡ��������Ϣ
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        ServiceResult<ServiceProviderSetDto> GetDataDyNumber(string providerName);
    }
}
