using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.Framework.Redis;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.GPSDAGL.Services.Enums;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Dapper;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Conwin.FileModule.ServiceAgent;
using Conwin.GPSDAGL.Framework.OperationLog;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;

namespace Conwin.GPSDAGL.Services.Services
{
    /// <summary>
    /// 企业注册
    /// </summary>
    public class EnterpriseRegisterService : ApiServiceBase, IEnterpriseRegisterService
    {
        //企业注册信息
        private readonly IEnterpriseRegisterInfoRepository _enterpriseRegisterInfoRepository;

        //企业信息
        private readonly ICheLiangYeHuRepository _yeHuRepository;

        //组织信息
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;

        //企业服务商关联信息
        private readonly IQiYeFuWuShangGuanLianXinXiRepository _qiYeFuWuShangGuanLianXinXiRepository;

        //系统ID
        private readonly string sysId = string.Empty;

        //行业云网关地址
        private static readonly string hangYeYunServiceGatewayUrl =
            ConfigurationManager.AppSettings["HangYeYunServiceGateway"];

        private static ServiceHttpHelper _helper = new ServiceHttpHelper();

        //第三方机构
        private readonly IFuWuShangRepository _fuWuShangRepository;

        //安全人员
        private readonly IAnQuanGuanLiRenYuanRepository _anQuanGuanLiRenYuan;

        //合作关系绑定表
        private readonly IPartnershipBindingTableRepository _partnershipBindingTable;

        //车辆合作关系绑定表
        private readonly IVehiclePartnershipBindingRepository _vehiclePartnershipBinding;

        //服务商审核信息表
        private readonly IMaterialListOfServiceProviderRepository _materialListOfServiceProviderRepository;

        //车辆表
        private readonly ICheLiangRepository _cheLiangRepository;

        /// <summary>
        /// 监控人员
        /// </summary>
        private readonly IMonitorPersonInfoRepository _monitorPersonInfo;
        /// <summary>
        /// 车辆视频终端
        /// </summary>
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        public EnterpriseRegisterService(
            IEnterpriseRegisterInfoRepository enterpriseRegisterInfoRepository,
            ICheLiangYeHuRepository qiYeDangAnRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            IQiYeFuWuShangGuanLianXinXiRepository qiYeFuWuShangGuanLianXinXiRepository,
            IBussinessLogger _bussinessLogger,
            IFuWuShangRepository fuWuShangRepository,
            IAnQuanGuanLiRenYuanRepository anQuanGuanLiRenYuanRepository,
            IPartnershipBindingTableRepository partnershipBindingTableRepository,
            IVehiclePartnershipBindingRepository vehiclePartnershipBindingRepository,
            ICheLiangRepository cheLiangRepository,
            IMonitorPersonInfoRepository monitorPersonInfoRepository,
            IMaterialListOfServiceProviderRepository materialListOfServiceProviderRepository,
            ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository
        ) : base(_bussinessLogger)
        {
            _enterpriseRegisterInfoRepository = enterpriseRegisterInfoRepository;

            _yeHuRepository = qiYeDangAnRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _qiYeFuWuShangGuanLianXinXiRepository = qiYeFuWuShangGuanLianXinXiRepository;
            sysId = base.SysId;
            _fuWuShangRepository = fuWuShangRepository;
            _anQuanGuanLiRenYuan = anQuanGuanLiRenYuanRepository;
            _partnershipBindingTable = partnershipBindingTableRepository;
            _vehiclePartnershipBinding = vehiclePartnershipBindingRepository;
            _cheLiangRepository = cheLiangRepository;
            _monitorPersonInfo = monitorPersonInfoRepository;
            _materialListOfServiceProviderRepository = materialListOfServiceProviderRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
        }

        /// <summary>
        /// 查询企业账号注册企业名单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryEnterpriseList(QueryData dto)
        {
            try
            {
                QueryQiYeListDto search =
                    JsonConvert.DeserializeObject<QueryQiYeListDto>(JsonConvert.SerializeObject(dto.data));

                QueryResult result = new QueryResult();
                QueryEnterpriseResponseDto resultModel = new QueryEnterpriseResponseDto();
                var queryQiYeParam = new
                {
                    Page = dto.page,
                    Rows = dto.rows,
                    data = new
                    {
                        OrgName = search.OrgName,
                        OrgCode = search.OrgCode
                    }
                };
                CWRequest notRequest = CWHelper.GenerateRequest(sysId, "006600200180", "1.0", queryQiYeParam);
                string notResString = _helper.Post(hangYeYunServiceGatewayUrl, notRequest);
                if (!string.IsNullOrWhiteSpace(notResString))
                {
                    CWResponse notResponse =
                        ServiceAgentUtility.DeserializeResponse(JsonConvert.DeserializeObject(notResString).ToString());
                    if (notResponse?.publicresponse?.statuscode == 0)
                    {
                        result.totalcount = notResponse?.body?.totalcount;
                        if (notResponse?.body?.totalcount > 0)
                        {
                            resultModel.EnterpriseList =
                                JsonConvert.DeserializeObject<List<EnterpriseInfoResponseDto>>(
                                    JsonConvert.SerializeObject(notResponse?.body?.items));
                        }
                    }

                }

                result.items = resultModel;

                return new ServiceResult<QueryResult> { Data = result };

            }
            catch (Exception ex)
            {
                LogHelper.Error("企业注册查询企业名单出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }

        }

        /// <summary>
        /// 企业注册资料提交&账号下发邮箱
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> EnterpriseRegister(RegisterRequestDto dto)
        {
            try
            {
                #region 属性非空校验

                if (string.IsNullOrWhiteSpace(dto.OrgCode))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "企业代码不能为空" };
                }
                if (string.IsNullOrWhiteSpace(dto.ContactEMail))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "联系邮箱不能为空" };
                }
                #endregion
                #region 数据有效性校验

                //验证邮箱验证码有效性
                var key = $"{ConfigurationManager.AppSettings["APPCODE"].ToString()}-CLBASH:{dto.RedisId}";
                var value = RedisManager.Get(key).ToString();
                if (value != dto.MailVerificationCode)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "邮箱验证码错误" };
                }
                //验证是否存在历史注册资料
                var registerModel = _enterpriseRegisterInfoRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == dto.OrgCode.Trim()).FirstOrDefault();
                if (registerModel != null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "当前企业已有账号，无需再次注册，请使用账号密码进行登录。" };
                }
                var yhModel = _yeHuRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == dto.OrgCode.Trim()).FirstOrDefault();
                if(yhModel==null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "基础档案不存在这家企业信息！无法注册" };
                }
                #endregion
                //新增注册审核信息
                var newEnterpriseRegisterModel = new EnterpriseRegisterInfo()
                {
                    Id = Guid.NewGuid(),
                    OrgCode = dto.OrgCode.Trim(),
                    ContactName = dto.ContactName.Trim(),
                    ContactIDCard = dto.ContactIDCard.Trim(),
                    ContactTel = dto.ContactTel.Trim(),
                    ContactEMail = dto.ContactEMail.Trim(),
                    ContactIDCardFrontId = dto.ContactIDCardFrontId,
                    ContactIDCardBackId = dto.ContactIDCardBackId,
                    BusinessPermitNumber = dto.JingYingXuKeZheng,
                    SYS_ChuangJianShiJian = DateTime.Now,
                    SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                    ApprovalStatus = (int)RegisterInfoApprovalStatus.注册账号,
                    SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                };
                #region 账号分配与邮件发送
                #region 账号分配
                //企业角色
                var roleCodes = $"{(int)ZuZhiJueSe.企业审核填报用户:000}";
                //系统id
                var syId = ConfigurationManager.AppSettings["WEBAPISYSID"];

                //SPT+00001(十进制方式进位)
                var userName = "SPT"+AccountNumber();
                //账号信息
                AccountInformation accountInformation;
                using (IDbConnection conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DC_YHQXDS"].ConnectionString))
                {
                    var sql =
                        $@"SELECT * FROM  dbo.T_User WHERE SYS_XiTongZhuangTai=0 AND UserName='{userName}'";
                    accountInformation = conn.Query<AccountInformation>(sql).ToList().FirstOrDefault();
                }

                var passwordStatus = false;
                var number = string.Empty;
                var rand = new Random();
                for (var i = 0; i < 6; i++)
                {
                    number += rand.Next(0, 9).ToString();
                }
                //不存在账号时才进行添加
                if (accountInformation == null)
                {
                    //用户注册
                    var queryRequest = GetInvokeRequest("00000030057", "1.0", new
                    {
                        SysId = syId,
                        SysOrgId = "C6380E44-F83F-A921-7174-5B6A8565BB4E",
                        OrgName = dto.OrgName,
                        OrgType = (int)OrganizationType.企业,
                        OrgCode = dto.OrgCode.Trim(),
                        UserName = userName, //TODO:生成序列号
                        RoleCode = roleCodes,
                        OrganizationProvince = dto.XiaQuSheng,
                        OrganizationCity = dto.XiaQuShi,
                        OrganizationDistrict = dto.XiaQuXian,
                        ManageArea = "广东清远",
                        SYS_XiTongBeiZhu = string.Empty,
                        Password = "SPT" + number
                    });
                    if (queryRequest.body == false)
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = queryRequest.body.msg };
                    }
                }
                else
                {
                    passwordStatus = true;
                    //初始化账号
                    using (IDbConnection conn =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["DC_RZZXDS"].ConnectionString))
                    {

                        var sql =
                            $@" UPDATE   dbo.T_UserAccount
                                         SET  [Password]='CJpNI8yLB5o='
                                           WHERE AccountName='{userName}'";
                        conn.ExecuteScalar<int>(sql, null);
                    }
                }

                #endregion

                #region 邮件发送

                var resendEmailDto = new ResendEmailDto
                {
                    BusinessLicense = userName,
                    Email = dto.ContactEMail,
                    PasswordStatus = passwordStatus,
                    Password = "SPT" + number
                };
                ResendEmail(resendEmailDto);

                #endregion

                #endregion

                #region 持久化

                bool isSuccess;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //新增企业注册信息
                    _enterpriseRegisterInfoRepository.Add(newEnterpriseRegisterModel);

                    isSuccess = uow.CommitTransaction() > 0;
                }

                if (isSuccess)
                {

                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                        ActionName = nameof(EnterpriseRegister),
                        BizOperType = OperLogBizOperType.ADD,
                        ShortDescription = $"{dto.OrgName}（{dto.OrgCode}）自主注册账号：{userName}]",
                        OperatorName = "",
                        OldBizContent = "",
                        OperatorID = "",
                        NewBizContent = JsonConvert.SerializeObject(newEnterpriseRegisterModel),
                        OperatorOrgCode = dto.OrgCode,
                        OperatorOrgName = dto.OrgName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    });
                }

                #endregion

                return new ServiceResult<bool> { Data = isSuccess };

            }catch (Exception ex)
            {
                LogHelper.Error($"企业提交注册资料出错{ex.Message},请求参数{JsonConvert.SerializeObject(dto)}", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "保存失败" };
            }
        }

        public string AccountNumber()
        {
            string account;
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DC_YHQXDS"].ConnectionString))
                {
                    var state = false;
                    var sql =
                        $@"
SELECT *  FROM DC_YHQXDS.dbo.T_UserAccountInformation 
";
                    var userAccount = conn.Query<UserAccountModel>(sql).ToList().FirstOrDefault();
                    account = userAccount == null ? "00001" : userAccount.AccountName;
                    account = Convert.ToString(Convert.ToInt16(account) + 1).PadLeft(5, '0');
                    while (!state)
                    {
                        sql = $@" SELECT COUNT(*)  FROM DC_RZZXDS.dbo.T_UserAccount
                    WHERE AccountName = 'SPT{account}'";
                        var count = conn.ExecuteScalar<int>(sql, null);
                        if (count <= 0)
                        {
                            state = true;
                        }
                        else
                        {
                            var size = Convert.ToInt16(account) + 1;
                            account = Convert.ToString(size).PadLeft(5, '0');
                        }
                    }
                    sql = $@" UPDATE DC_YHQXDS.dbo.T_UserAccountInformation
	  SET AccountName='{account}'";
                    conn.ExecuteScalar<int>(sql, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return account;
        }

        /// <summary>
        /// 仅企业用户：获取当前用户注册审核信息
        /// </summary>
        /// <returns></returns>
        public ServiceResult<ApprovalInfoDto> GetEnterpriseRegisterInfo()
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ApprovalInfoDto> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
                }

                var registerModel = (from a in _enterpriseRegisterInfoRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == 0 && x.OrgCode == userInfo.OrganizationCode)
                                     join b in _orgBaseInfoRepository.GetQuery(x =>
                                         x.SYS_XiTongZhuangTai == 0 && x.OrgCode == userInfo.OrganizationCode) on a.OrgCode equals b
                                         .OrgCode
                                     select new ApprovalInfoDto
                                     {
                                         EnterpriseBaseInfo = new EnterpriseBaseInfoDto
                                         {
                                             OrgName = b.OrgName,
                                             OrgCode = b.OrgCode,
                                             XiaQuSheng = b.XiaQuSheng,
                                             XiaQuShi = b.XiaQuShi,
                                             XiaQuXian = b.XiaQuXian,
                                             JingYingFanWei = b.JingYingFanWei,
                                             YeWuJingYingFanWei = b.YeWuJingYingFanWei,
                                             DiZhi = b.DiZhi,
                                             EnterpriseType = a.EnterpriseType,
                                             MonitorType = a.MonitorType,
                                         },
                                         EnterpriseContactInfo = new EnterpriseContactDto
                                         {
                                             ContactName = a.ContactName,
                                             ContactIDCard = a.ContactIDCard,
                                             ContactTel = a.ContactTel,
                                             ContactEMail = a.ContactEMail,
                                             ContactIDCardBackId = a.ContactIDCardBackId,
                                             ContactIDCardFrontId = a.ContactIDCardFrontId
                                         },
                                         EnterprisePrincipalInfo = new EnterprisePrincipalDto
                                         {
                                             PrincipalName = a.PrincipalName,
                                             PrincipalIDCard = a.PrincipalIDCard,
                                             PrincipalTel = a.PrincipalTel,
                                             PrincipalIDCardFrontId = a.PrincipalIDCardFrontId,
                                             PrincipalIDCardBackId = a.PrincipalIDCardBackId,

                                         },
                                         EnterpriseBusinessLicenseInfo = new EnterpriseBusinessLicenseDto
                                         {
                                             UniformSocialCreditCode = a.UniformSocialCreditCode,
                                             BusinessLicenseFileId = a.BusinessLicenseFileId,
                                         },
                                         EnterpriseBusinessPermitInfo = new EnterpriseBusinessPermitDto
                                         {
                                             BusinessPermitNumber = a.BusinessPermitNumber,
                                             BusinessPermitStartDateTime = a.BusinessPermitStartDateTime,
                                             BusinessPermitEndDateTime = a.BusinessPermitEndDateTime,
                                             BusinessPermitIssuingUnit = a.BusinessPermitIssuingUnit,
                                             BusinessPermitPhotoFileId = a.BusinessPermitPhotoFIleId
                                         },
                                         ApprovalStatus = a.ApprovalStatus,
                                         ApprovalRemark = a.ApprovalRemark
                                     }).FirstOrDefault();

                return new ServiceResult<ApprovalInfoDto> { Data = registerModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业注册审核信息出错" + ex.Message, ex);
                return new ServiceResult<ApprovalInfoDto> { StatusCode = 2, ErrorMessage = "查询出错，请稍后重试。" };
            }
        }

        /// <summary>
        /// 企业用户完善审核信息并提交审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<bool> SubmitEnterpriseApprovalInfo(EnterpriseApprovalSubmitInfoDto model)
        {
            try
            {
                #region 信息验证

                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取用户信息失败，请重新登录" };
                }

                if (userInfo.OrganizationCode != model.OrgCode)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "账号与提交信息不一致，请重新登录" };
                }

                var enterpriseApprovalInfoModel = _enterpriseRegisterInfoRepository
                    .GetQuery(x => x.OrgCode == userInfo.OrganizationCode && x.SYS_XiTongZhuangTai == 0)
                    .FirstOrDefault();

                if (enterpriseApprovalInfoModel == null)
                {
                    enterpriseApprovalInfoModel = new EnterpriseRegisterInfo
                    {
                        Id = Guid.NewGuid(),
                        SYS_XiTongZhuangTai = 0,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        OrgCode = userInfo.OrganizationCode,
                        SYS_ChuangJianRen = userInfo.UserName,
                        SYS_ChuangJianRenID = userInfo.Id
                    };
                }

                if (enterpriseApprovalInfoModel.ApprovalStatus == (int)RegisterInfoApprovalStatus.审核通过)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "组织备案信息已审核通过，当前不能修改信息。" };
                }

                #endregion

                Mapper.CreateMap<EnterpriseApprovalSubmitInfoDto, EnterpriseRegisterInfo>();
                var enterpriseModel = Mapper.Map<EnterpriseRegisterInfo>(model);
                enterpriseModel.Id = enterpriseApprovalInfoModel.Id;
                enterpriseModel.SYS_ChuangJianShiJian = enterpriseApprovalInfoModel.SYS_ChuangJianShiJian;
                enterpriseModel.SYS_ChuangJianRen = enterpriseApprovalInfoModel.SYS_ChuangJianRen;
                enterpriseModel.SYS_ChuangJianRenID = enterpriseApprovalInfoModel.SYS_ChuangJianRenID;
                enterpriseModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                enterpriseModel.SYS_ChuangJianRen = userInfo.UserName;
                enterpriseModel.SYS_ChuangJianRenID = userInfo.Id;
                enterpriseModel.SYS_XiTongZhuangTai = 0;
                enterpriseModel.ApprovalStatus = (int)RegisterInfoApprovalStatus.提交审核;


                #region 持久化

                bool isSuccess;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //新增企业注册信息
                    _enterpriseRegisterInfoRepository.Update(enterpriseModel);

                    isSuccess = uow.CommitTransaction() > 0;
                }

                if (isSuccess)
                {

                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                        ActionName = nameof(SubmitEnterpriseApprovalInfo),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = $"{userInfo.OrganizationName}（{userInfo.OrganizationCode}）提交修改备案信息",
                        OperatorName = "",
                        OldBizContent = JsonConvert.SerializeObject(enterpriseApprovalInfoModel),
                        OperatorID = "",
                        NewBizContent = JsonConvert.SerializeObject(enterpriseModel),
                        OperatorOrgCode = userInfo.OrganizationCode,
                        OperatorOrgName = userInfo.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    });
                }

                #endregion

                return new ServiceResult<bool> { Data = isSuccess };

            }
            catch (Exception ex)
            {
                LogHelper.Error("企业提交审核信息出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "保存失败，请稍后重试" };
            }


        }

        /// <summary>
        /// 主管部门查询企业备案审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryEnterpriseApprovalInfoList(QueryData data)
        {
            try
            {
                QueryEnterpriseApprovalListParamDto param =
                    JsonConvert.DeserializeObject<QueryEnterpriseApprovalListParamDto>(
                        JsonConvert.SerializeObject(data.data));

                Expression<Func<EnterpriseRegisterInfo, bool>> enExp = x => x.SYS_XiTongZhuangTai == 0;
                Expression<Func<OrgBaseInfo, bool>> orgExp = x => x.SYS_XiTongZhuangTai == 0;

                //审核状态过滤
                List<int> approvalStatusList = new List<int>
                {
                    (int) RegisterInfoApprovalStatus.审核通过,
                    (int) RegisterInfoApprovalStatus.审核不通过,
                    (int) RegisterInfoApprovalStatus.提交审核,
                    (int) RegisterInfoApprovalStatus.驳回
                };
                enExp = enExp.And(x => approvalStatusList.Contains(x.ApprovalStatus ?? 0));

                if (!string.IsNullOrWhiteSpace(param.OrgName))
                {
                    orgExp = orgExp.And(x => x.OrgName.Contains(param.OrgName.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(param.XiaQuXian))
                {
                    orgExp = orgExp.And(x => x.XiaQuXian == param.XiaQuXian);
                }

                if (param.EnterpriseType.HasValue)
                {
                    enExp = enExp.And(x => x.EnterpriseType == param.EnterpriseType);
                }

                if (param.MonitorType.HasValue)
                {
                    enExp = enExp.And(x => x.MonitorType == param.MonitorType);
                }

                if (!string.IsNullOrWhiteSpace(param.ContactName))
                {
                    enExp = enExp.And(x => x.ContactName.Contains(param.ContactName.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(param.ContactTel))
                {
                    enExp = enExp.And(x => x.ContactTel.Contains(param.ContactTel.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(param.PrincipalName))
                {
                    enExp = enExp.And(x => x.PrincipalName.Contains(param.PrincipalName.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(param.PrincipalTel))
                {
                    enExp = enExp.And(x => x.PrincipalTel.Contains(param.PrincipalTel.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(param.BusinessPermitNumber))
                {
                    enExp = enExp.And(x => x.BusinessPermitNumber.Contains(param.BusinessPermitNumber.Trim()));
                }

                if (param.ApprovalStatus.HasValue)
                {
                    enExp = enExp.And(x => x.ApprovalStatus == param.ApprovalStatus);
                }


                var list = from en in _enterpriseRegisterInfoRepository.GetQuery(enExp)
                           join org in _orgBaseInfoRepository.GetQuery(orgExp) on en.OrgCode equals org.OrgCode
                           select new EnterpriseApprovalListDto
                           {
                               Id = en.Id,
                               OrgCode = en.OrgCode,
                               OrgName = org.OrgName,
                               XiaQuSheng = org.XiaQuSheng,
                               XiaQuShi = org.XiaQuShi,
                               XiaQuXian = org.XiaQuXian,
                               EnterpriseType = en.EnterpriseType,
                               MonitorType = en.MonitorType,
                               ContactName = en.ContactName,
                               ContactTel = en.ContactTel,
                               PrincipalName = en.PrincipalName,
                               PrincipalTel = en.PrincipalTel,
                               BusinessPermitNumber = en.BusinessPermitNumber,
                               ApprovalStatus = en.ApprovalStatus,
                               CreateDateTime = en.SYS_ChuangJianShiJian,
                           };


                QueryResult result = new QueryResult();
                result.totalcount = list.Count();

                if (result.totalcount > 0)
                {
                    result.items = list.OrderByDescending(x => x.CreateDateTime).Skip((data.page - 1) * data.rows)
                        .Take(data.rows).ToList();
                }

                return new ServiceResult<QueryResult> { Data = result };


            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业备案审核信息列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "" };
            }
        }

        /// <summary>
        /// 主管部门查看企业备案审核信息详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ServiceResult<EnterpriseApprovalSubmitInfoDto> GetEnterpriseApprovalInfoDetail(Guid Id)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<EnterpriseApprovalSubmitInfoDto>
                    { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
                }


                var approvalModel =
                    (from a in _enterpriseRegisterInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == Id)
                     join b in _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on a.OrgCode equals b
                         .OrgCode
                     select new
                         EnterpriseApprovalSubmitInfoDto
                     {
                         Id = a.Id,
                         OrgCode = b.OrgCode,
                         OrgName = b.OrgName,
                         XiaQuSheng = b.XiaQuSheng,
                         XiaQuShi = b.XiaQuShi,
                         XiaQuXian = b.XiaQuXian,
                         DiZhi = b.DiZhi,
                         JingYingFanWei = b.JingYingFanWei,
                         YeWuJingYingFanWei = b.YeWuJingYingFanWei,
                         EnterpriseType = a.EnterpriseType,
                         MonitorType = a.MonitorType,
                         ContactName = a.ContactName,
                         ContactIDCard = a.ContactIDCard,
                         ContactTel = a.ContactTel,
                         ContactEMail = a.ContactEMail,
                         ContactIDCardFrontId = a.ContactIDCardFrontId,
                         ContactIDCardBackId = a.ContactIDCardBackId,
                         PrincipalName = a.PrincipalName,
                         PrincipalIDCard = a.PrincipalIDCard,
                         PrincipalTel = a.PrincipalTel,
                         PrincipalIDCardFrontId = a.PrincipalIDCardFrontId,
                         PrincipalIDCardBackId = a.PrincipalIDCardBackId,
                         UniformSocialCreditCode = a.UniformSocialCreditCode,
                         BusinessLicenseFileId = a.BusinessLicenseFileId,
                         BusinessPermitNumber = a.BusinessPermitNumber,
                         BusinessPermitStartDateTime = a.BusinessPermitStartDateTime,
                         BusinessPermitEndDateTime = a.BusinessPermitEndDateTime,
                         BusinessPermitIssuingUnit = a.BusinessPermitIssuingUnit,
                         BusinessPermitPhotoFileId = a.BusinessPermitPhotoFIleId
                     }).FirstOrDefault();

                return new ServiceResult<EnterpriseApprovalSubmitInfoDto> { Data = approvalModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取企业备案审核信息详情出错" + ex.Message, ex);
                return new ServiceResult<EnterpriseApprovalSubmitInfoDto> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        /// <summary>
        /// 主管部门审核企业备案信息
        /// </summary>
        /// <param name="approvalDto"></param>
        /// <returns></returns>
        public ServiceResult<bool> ApprovalEnterpriseInfo(ApprovalEnterpriseDto approvalDto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
                }

                if (approvalDto?.IdList == null || approvalDto.IdList.Count() == 0)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "请选择需要审核的记录" };
                }

                var approvalList = _enterpriseRegisterInfoRepository
                    .GetQuery(x => approvalDto.IdList.Contains(x.Id) && x.SYS_XiTongZhuangTai == 0).ToList();

                var oldInfoList = approvalList.ToList();

                switch (approvalDto.ApprovalStatus)
                {
                    case (int)RegisterInfoApprovalStatus.审核通过:
                        if (approvalList.Where(x => x.ApprovalStatus != (int)RegisterInfoApprovalStatus.提交审核).Count() >
                            0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只能对待审核的申请进行审核通过操作！" };
                        }

                        break;
                    case (int)RegisterInfoApprovalStatus.审核不通过:
                        if (approvalList.Where(x => x.ApprovalStatus != (int)RegisterInfoApprovalStatus.提交审核).Count() >
                            0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只能对待审核的申请进行审核不通过操作！" };
                        }

                        break;
                    case (int)RegisterInfoApprovalStatus.驳回:
                        if (approvalList.Where(x => x.ApprovalStatus != (int)RegisterInfoApprovalStatus.审核通过).Count() >
                            0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只能对审核通过的申请进行驳回操作" };
                        }

                        break;
                    default:
                        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "请选择需要进行的操作" };
                }


                bool isSuccess;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    approvalList.ForEach(x =>
                    {
                        x.ApprovalStatus = approvalDto.ApprovalStatus;
                        x.ApprovalRemark = approvalDto.ApprovalRemark;
                        x.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        x.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        x.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _enterpriseRegisterInfoRepository.Update(x);

                        var orgBaseInfo = _orgBaseInfoRepository
                            .GetQuery(q => q.SYS_XiTongZhuangTai == 0 && q.OrgCode == x.OrgCode).FirstOrDefault();
                        var oldModel = oldInfoList.Where(q => q.OrgCode == x.OrgCode).FirstOrDefault();
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                            ActionName = nameof(ApprovalEnterpriseInfo),
                            BizOperType = OperLogBizOperType.UPDATE,
                            ShortDescription =
                                $"主管部门审核{orgBaseInfo?.OrgName}（{orgBaseInfo?.OrgCode}）备案信息:{typeof(RegisterInfoApprovalStatus).GetEnumName(approvalDto.ApprovalStatus)}",
                            OperatorName = "",
                            OldBizContent = JsonConvert.SerializeObject(oldModel),
                            OperatorID = "",
                            NewBizContent = JsonConvert.SerializeObject(x),
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });

                    });
                    isSuccess = uow.CommitTransaction() > 0;

                }

                //组织权限分配
                if (isSuccess)
                {
                    switch (approvalDto.ApprovalStatus)
                    {
                        case (int)RegisterInfoApprovalStatus.审核通过:
                            {
                                var queryParam = new
                                {
                                    OrgCode = approvalList.Select(x => x.OrgCode).ToList(),
                                    RoleCodeList = new List<string> { "006" },
                                    SysId = sysId,
                                };
                                var queryRequest = GetInvokeRequest("00000030060", "1.0", queryParam);
                                if (queryRequest != null)
                                {
                                    if (queryRequest.publicresponse.statuscode != 0)
                                    {
                                        LogHelper.Error(
                                            $"主管部门审核企业备案信息调用接口00000030060响应异常{JsonConvert.SerializeObject(queryRequest)}");
                                    }
                                }
                                else
                                {
                                    LogHelper.Error("主管部门审核企业备案信息调用接口00000030060返回结果为空");
                                }
                            }
                            break;
                        case (int)RegisterInfoApprovalStatus.审核不通过:
                        case (int)RegisterInfoApprovalStatus.驳回:
                            {
                                var queryParam = new
                                {
                                    OrgCode = approvalList.Select(x => x.OrgCode).ToList(),
                                    RoleCodeList = new List<string> { "006" },
                                    SysId = sysId,
                                };
                                var queryRequest = GetInvokeRequest("00000030061", "1.0", queryParam);
                                if (queryRequest != null)
                                {
                                    if (queryRequest.publicresponse.statuscode != 0)
                                    {
                                        LogHelper.Error(
                                            $"主管部门审核企业备案信息调用接口00000030061响应异常{JsonConvert.SerializeObject(queryRequest)}");
                                    }
                                }
                                else
                                {
                                    LogHelper.Error("主管部门审核企业备案信息调用接口00000030061返回结果为空");
                                }
                            }
                            break;
                    }
                }

                return new ServiceResult<bool> { Data = isSuccess };
            }
            catch (Exception ex)
            {
                LogHelper.Error("通过企业备案审核出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "审核失败，请稍后再试" };
            }

        }

        /// <summary>
        ///修改密码限制
        /// </summary>
        /// <returns></returns>
        public ServiceResult<bool> UpdatePassword(RegisterUpdatePasswordDto dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
                }

                switch (userInfo.OrganizationType)
                {
                    case (int)OrganizationType.企业:
                        var enModel = _enterpriseRegisterInfoRepository.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == 0 && x.OrgCode == userInfo.OrganizationCode &&
                            x.ApprovalStatus == (int)RegisterInfoApprovalStatus.审核通过).FirstOrDefault();
                        if (enModel == null)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "账号未通过主管部门审核，暂时不能修改密码。" };
                        }

                        break;
                    case (int)OrganizationType.本地服务商:

                        break;

                }

                UpdatePassWordParamDto param = new UpdatePassWordParamDto
                {
                    SysId = base.SysId,
                    SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                    SYS_ZuiJinXiuGaiRenId = Guid.Parse(userInfo.Id),
                    AccountName = userInfo.AccountName,
                    OldPassword = dto.OldPassWord,
                    Password = dto.Password,
                };
                //调用修改密码接口
                var getSNoResponse = GetInvokeRequest("00000010004", "1.0", param);
                if (getSNoResponse?.publicresponse?.statuscode != 0)
                {
                    LogHelper.Error("修改密码调用00000010004返回失败,响应报文:" + JsonConvert.SerializeObject(getSNoResponse));
                    return new ServiceResult<bool>
                    { Data = false, StatusCode = 2, ErrorMessage = "修改失败," + getSNoResponse.publicresponse.message };
                }
                else
                {
                    return new ServiceResult<bool> { Data = true };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error($"修改密码出错{ex.Message}", ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "操作出错" };
            }
        }


        /// <summary>
        /// 获取旧运政 企业信息（调整 直接拿取基础档案企业信息）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryResisterEnterpriseList(QueryData dto)
        {
            try
            {
                QueryQiYeListDto search =
                    JsonConvert.DeserializeObject<QueryQiYeListDto>(JsonConvert.SerializeObject(dto.data));
                QueryResult result = new QueryResult();
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<CheLiangYeHu, bool>> YeHuExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
                if (!string.IsNullOrEmpty(search.OrgName))
                {
                    OrgBaseExp = OrgBaseExp.And(u => u.OrgName.Contains(search.OrgName.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(search.OrgCode))
                {
                    OrgBaseExp = OrgBaseExp.And(u => u.OrgCode == search.OrgCode.Trim());
                }

                var query = from a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                    join b in _yeHuRepository.GetQuery(YeHuExp)
                        on a.Id.ToString() equals b.BaseId
                    join epr in _enterpriseRegisterInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                        on b.OrgCode equals epr.OrgCode
                        into temp1
                    from te1 in temp1.DefaultIfEmpty()
                    select new EnterpriseInfoResponseDto
                    {
                        YeHuDaiMa= b.OrgCode,
                        YeHuJianCheng = a.OrgShortName,
                        YeHuMingCheng = b.OrgName,
                        JingYingFanWei = a.YeWuJingYingFanWei,
                        JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                        DiZhi = a.DiZhi,
                        XiaQuShi = a.XiaQuShi,
                        XiaQuXian = a.XiaQuXian,
                        LianXiRen=b.LianXiRen,
                        LianXiDianHua=b.LianXiFangShi,
                        ChuangJianShiJian = a.SYS_ChuangJianShiJian.Value,
                        QiYeXingZhi=b.QiYeXingZhi
                    };
                result.totalcount = query.Count();
                //分页
                result.items = query.OrderByDescending(x => x.ChuangJianShiJian)
                    .Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList();
                return new ServiceResult<QueryResult> { Data = result };

                //List<EnterpriseInfoResponseDto> resultList = new List<EnterpriseInfoResponseDto>();
                //var queryQiYeParam = new
                //{
                //    Page = dto.page,
                //    Rows = dto.rows,
                //    data = new
                //    {
                //        OrgName = search.OrgName,
                //        OrgCode = search.OrgCode
                //    }
                //};
                //CWRequest notRequest = CWHelper.GenerateRequest(sysId, "006600200180", "1.0", queryQiYeParam);
                //string notResString = _helper.Post(hangYeYunServiceGatewayUrl, notRequest);
                //if (!string.IsNullOrWhiteSpace(notResString))
                //{
                //    CWResponse notResponse =
                //        ServiceAgentUtility.DeserializeResponse(JsonConvert.DeserializeObject(notResString).ToString());
                //    if (notResponse?.publicresponse?.statuscode == 0)
                //    {
                //        result.totalcount = notResponse?.body?.totalcount;
                //        if (notResponse?.body?.totalcount > 0)
                //        {
                //            resultList =
                //                JsonConvert.DeserializeObject<List<EnterpriseInfoResponseDto>>(
                //                    JsonConvert.SerializeObject(notResponse?.body?.items));
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("企业注册查询企业名单出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }


        #region 企业资料管理相关接口


        #region 获取后几位数

        /// <summary>
        /// 获取后几位数
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public string GetLastStr(string str, int num)
        {
            var count = 0;
            if (str.Length <= num) return str;
            count = str.Length - num;
            str = str.Substring(count, num);
            return str;
        }

        #endregion

        public ServiceResult<bool> EnterpriseAccountStatus(string sysid, EnterpriseDataManagementDto model)
        {
            try
            {
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var enterpriseThirdParty = _enterpriseRegisterInfoRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == model.QiYeCode)
                    .FirstOrDefault();
                if (enterpriseThirdParty != null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "当前企业已注册，请使用账号密码登录平台。" };
                }

                return new ServiceResult<bool> { Data = true };
            }
            catch (Exception ex)
            {
                LogHelper.Error("企业注册选择企业确认企业注册信息出错" + ex.Message);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "操作失败，请稍后重试" };
            }

        }

        /// <summary>
        /// 账号密码邮件发送
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> ResendEmail(ResendEmailDto dto)
        {
            try
            {
                #region 邮件发送
                //发送邮箱
                //发送人邮箱
                if (string.IsNullOrEmpty(dto.Password))
                {
                 var userAccount=  UserAccount(dto.BusinessLicense);
                 dto.Password = new TripleDESEncode().Decode(userAccount.Password);
                 dto.BusinessLicense = userAccount.AccountName;
                }
                var senderEmail = ConfigurationManager.AppSettings["EmailCode"];
                //发送人邮箱授权码
                var senderEmailCode = ConfigurationManager.AppSettings["EmailNumber"];
                var argCode = senderEmail.Split('@');
                if (dto.PasswordStatus)
                {
                    dto.Password = "123456";
                }
                try
                {
                    //实例化一个发送邮件类。
                    var mailMessage = new MailMessage { From = new MailAddress(senderEmail) };
                    //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                    //收件人邮箱地址。
                    mailMessage.To.Add(new MailAddress(dto.Email));
                    //邮件标题。
                    mailMessage.Subject = "企业账号注册成功";
                    //邮件内容。
                    mailMessage.Body =
                        $@"您于{DateTime.Now.ToLongDateString().ToString()}在清远市交通运输局两客一危一重营运车辆主动安全防控平台进行企业账号注册，注册邮箱为{dto.Email}，现已通过账号注册申请，请使用账号密码进行登录。并及时修改密码。
账号：{ dto.BusinessLicense}
密码：{ dto.Password}

如果不是你本人进行注册，则你的账户信息受到威胁，请登录平台密码修改。登录地址为：
http://125.89.139.202:10100/Modules/Home/Index.html

请注意账号密码保密，谢谢！
清远市交通运输局两客一危一重营运车辆主动安全防控平台";
                    //实例化一个SmtpClient类。
                    //在这里我使用163邮箱，所以是kk_youzi@163.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                    //EnableSsl 使用安全加密连接。
                    //UseDefaultCredentials 不和请求一块发送。
                    // Credentials 验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                    //前缀是指@之前的字符
                    var client = new SmtpClient
                    {
                        Host = "smtp.163.com",
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(argCode[0], senderEmailCode)
                    };
                    //发送
                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    LogHelper.Error("发送邮箱失败" + e.Message, e);
                }

                #endregion

                return new ServiceResult<bool> { Data = true };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"账号密码发送失败{ex.Message}");
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "邮件发送失败" };
            }
        }


        public UserAccountModel UserAccount(string orgCode)
        {
            var userAccount = new UserAccountModel();
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DC_YHQXDS"].ConnectionString))
                {
                    var sql =
                        $@"USE DC_YHQXDS
SELECT T_UserAccount.*
FROM DC_RZZXDS.dbo.T_UserAccount
    JOIN dbo.T_User
        ON DC_RZZXDS.dbo.T_UserAccount.UserID = T_User.Id
    JOIN T_SysUser
        ON T_User.Id = T_SysUser.User_Id
    JOIN T_SystemOrganization
        ON T_SysUser.SysID = T_SystemOrganization.SysID
    JOIN T_Organization
        ON T_Organization.Id = T_SystemOrganization.Organization_Id
    JOIN T_SystemOrganizationSysUserMapper
        ON T_SystemOrganizationSysUserMapper.SystemOrganizationList_Id = T_SystemOrganization.Id
WHERE T_SystemOrganizationSysUserMapper.SysUserList_Id = T_SysUser.Id
      AND T_Organization.OrganizationType IN ( '2')
	  AND T_Organization.OrganizationCode='{orgCode}'
      AND DC_RZZXDS.dbo.T_UserAccount.SYS_XiTongZhuangTai = 0
";
                    userAccount = conn.Query<UserAccountModel>(sql).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询账号失败" + ex.Message, ex);
            }
            return userAccount;
        }

        #endregion

        #region 企业与第三方机构合作关系绑定模块相关功能

        #region  合作关系绑定列表

        public ServiceResult<QueryResult> PartnershipBindingList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetPartnershipBindingList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询合作关系绑定列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        /// <summary>
        /// 合作关系绑定列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        private GetPartnershipBindingDto GetPartnershipBindingList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetPartnershipBindingDto();
            PartnershipBindingDto searchDto =
                JsonConvert.DeserializeObject<PartnershipBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            //合作关系绑定列表
            Expression<Func<PartnershipBindingTable, bool>> partnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<FuWuShang, bool>> FuWuShang =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiangYeHu, bool>> CheLiangYeHu =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            //企业

            #region  数据限制条件

            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                partnershipBinding =
                    partnershipBinding.And(x => x.EnterpriseCode.Contains(userInfoDto.OrganizationCode));
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                partnershipBinding =
                    partnershipBinding.And(x => x.ServiceProviderCode.Contains(userInfoDto.OrganizationCode));
            }

            //默认加载待审核   发起取消合作的数据
            if (searchDto.ZhuangTai == 0)
            {
                partnershipBinding =
                    partnershipBinding.And(x =>
                        x.ZhuangTai == (int)CooperationStatus.企业发起待审核 || x.ZhuangTai ==
                                                                       (int)CooperationStatus.第三方发起待审核
                                                                       || x.ZhuangTai ==
                                                                       (int)CooperationStatus.企业发起取消合作 ||
                                                                       x.ZhuangTai ==
                                                                       (int)CooperationStatus.第三方发起取消合作);
            }
            else
            {
                partnershipBinding =
                    partnershipBinding.And(x =>
                        x.ZhuangTai == searchDto.ZhuangTai);
            }

            #endregion

            var query = from etp in _partnershipBindingTable.GetQuery(partnershipBinding)
                        join fws in _fuWuShangRepository.GetQuery(FuWuShang) on etp
                                .ServiceProviderCode equals fws.OrgCode
                            into temp1
                        from temp in temp1.DefaultIfEmpty()
                        join yh in _yeHuRepository.GetQuery(CheLiangYeHu)
                            on etp.EnterpriseCode equals yh.OrgCode into yhList
                        from yhEntity in yhList.DefaultIfEmpty()
                        where string.IsNullOrEmpty(searchDto.EnterpriseName) ||
                              yhEntity.OrgName.Contains(searchDto.EnterpriseName)
                        where string.IsNullOrEmpty(searchDto.ServiceProviderName) ||
                              temp.OrgName.Contains(searchDto.ServiceProviderName)
                        select new PartnershipBindingDto
                        {
                            Id = etp.Id,
                            EnterpriseCode = etp.EnterpriseCode,
                            ServiceProviderCode = etp.ServiceProviderCode,
                            UnitOrganizationCode = etp.UnitOrganizationCode,
                            UnitType = etp.UnitType ?? 0,
                            ZhuangTai = etp.ZhuangTai ?? 0,
                            SYS_ChuangJianShiJian = etp.SYS_ChuangJianShiJian,
                            EnterpriseName = yhEntity.OrgName,
                            ServiceProviderName = temp.OrgName,
                            CooperativeContractId = etp.CooperativeContractId
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region  新增合作关系绑定

        public ServiceResult<bool> NewPartnershipBinding(string sysid, PartnershipBindingDto model)
        {
            //1 通过用户信息来确定 是企业发起绑定关系还是第三方机构发起
            //2 企业发起：UnitType：2（企业），[ZhuangTai]：1（企业发起待审核）
            //3 第三方机构发起： UnitType：5（第三方机构），[ZhuangTai]：2（第三方发起待审核）
            //4  状态为4.审批不通过 7.取消合作  才能进行重新绑定
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                #region 逻辑验证

                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                //验证 当前企业，第三方机构是否状态正常
                var enterpriseRegisterInfo = _enterpriseRegisterInfoRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang
                    && x.OrgCode == model.EnterpriseCode
                    && x.ApprovalStatus ==
                    (int)RegisterInfoApprovalStatus.审核通过).ToList();
                if (!enterpriseRegisterInfo.Any())
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "所选企业未通过备案！" };
                }

                var materialListOfServiceProvider = _materialListOfServiceProviderRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang
                    && x.OrgCode == model.ServiceProviderCode
                    && x.ApprovalStatus == (int)ThirdPartyRegistrationStatus.市级审核通过).ToList();
                if (!materialListOfServiceProvider.Any())
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "所选第三方未通过备案！" };
                }

                #endregion

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var partnershipBinding = _partnershipBindingTable
                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                       && x.EnterpriseCode == model.EnterpriseCode &&
                                       x.ServiceProviderCode == model.ServiceProviderCode)
                        .FirstOrDefault();

                    if (partnershipBinding != null)
                    {
                        if (partnershipBinding.ZhuangTai != (int) CooperationStatus.审批不通过 &&
                            partnershipBinding.ZhuangTai != (int) CooperationStatus.取消合作)
                            return new ServiceResult<bool>
                            {
                                ErrorMessage = "该企业与第三方机构已经绑定关系！" +
                                               "只有审批不通过或取消合作才能重新绑定",
                                StatusCode = 2
                            };
                        switch (userInfo.OrganizationType)
                        {
                            case (int)OrganizationType.企业:
                                partnershipBinding.UnitOrganizationCode = model.EnterpriseCode;
                                partnershipBinding.UnitType = (int)OrganizationType.企业;
                                partnershipBinding.ZhuangTai = (int)CooperationStatus.企业发起待审核;
                                break;
                            case (int)OrganizationType.本地服务商:
                                partnershipBinding.UnitOrganizationCode = model.ServiceProviderCode;
                                partnershipBinding.UnitType = (int)OrganizationType.本地服务商;
                                partnershipBinding.ZhuangTai = (int)CooperationStatus.第三方发起待审核;
                                break;
                            default:
                                return new ServiceResult<bool>()
                                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                        }

                        partnershipBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        partnershipBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        partnershipBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        partnershipBinding.CooperativeContractId = model.CooperativeContractId;
                        _partnershipBindingTable.Update(partnershipBinding);
                        var updateResults = uow.CommitTransaction() > 0;
                        return updateResults
                            ? new ServiceResult<bool>() { Data = true }
                            : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "新增合作关系绑定失败" };

                    }

                    var entity = new PartnershipBindingTable
                    {
                        Id = Guid.NewGuid(),
                        EnterpriseCode = model.EnterpriseCode,
                        ServiceProviderCode = model.ServiceProviderCode,
                        CooperativeContractId = model.CooperativeContractId,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        SYS_ChuangJianRen = userInfo.UserName,
                        SYS_ChuangJianRenID = userInfo.Id,
                        SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                    };
                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.企业:
                            entity.ZhuangTai = (int)CooperationStatus.企业发起待审核;
                            entity.UnitOrganizationCode = model.EnterpriseCode;
                            entity.UnitType = (int)OrganizationType.企业;
                            model.UnitType = (int)OrganizationType.企业;
                            break;
                        case (int)OrganizationType.本地服务商:
                            entity.ZhuangTai = (int)CooperationStatus.第三方发起待审核;
                            entity.UnitOrganizationCode = model.ServiceProviderCode;
                            entity.UnitType = (int)OrganizationType.本地服务商;
                            model.UnitType = (int)OrganizationType.本地服务商;
                            break;
                        default:
                            return new ServiceResult<bool>()
                                { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                    }

                    _partnershipBindingTable.Add(entity);
                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        string sendOrgCode = "";
                        switch (userInfo.OrganizationType)
                        {
                            case (int)OrganizationType.企业:
                                sendOrgCode = model.ServiceProviderCode;
                                break;
                            case (int)OrganizationType.本地服务商:
                                sendOrgCode = model.EnterpriseCode;
                                break;
                        }


                        var orgBaseInfoModel = _orgBaseInfoRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == sendOrgCode).FirstOrDefault();

                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                            ActionName = nameof(NewPartnershipBinding),
                            BizOperType = OperLogBizOperType.ADD,
                            ShortDescription =
                                $"{userInfo?.OrganizationName}（{userInfo?.OrganizationCode}）申请与{orgBaseInfoModel?.OrgName}（{orgBaseInfoModel?.OrgCode}）委托关系绑定。",
                            OperatorName = "",
                            OldBizContent = "",
                            OperatorID = "",
                            NewBizContent = JsonConvert.SerializeObject(entity),
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                    }

                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "新增合作关系绑定失败" };
                }
            });
        }

        #endregion

        #region 审批不通过

        public ServiceResult<bool> ApprovalFailed(string sysid, PartnershipBindingDto model)
        {
            //1 通过用户信息来确定 是企业确定审批不通过还是第三方机构确定
            //2 只对待审核的审批为 审核不通过
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var partnershipBinding = _partnershipBindingTable
                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                       && x.EnterpriseCode == model.EnterpriseCode &&
                                       x.ServiceProviderCode == model.ServiceProviderCode)
                        .FirstOrDefault();

                    if (partnershipBinding != null)
                    {
                        #region 逻辑判断

                        switch (partnershipBinding.ZhuangTai)
                        {
                            case (int) CooperationStatus.第三方发起待审核 when userInfo.OrganizationType == (int) OrganizationType.本地服务商:
                                //只能企业执行此操作
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能企业执行此操作！",
                                    StatusCode = 2
                                };
                            //只能第三方机构执行此操作
                            case (int)CooperationStatus.企业发起待审核 when userInfo.OrganizationType == (int)OrganizationType.企业:
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能第三方机构执行此操作！",
                                    StatusCode = 2
                                };
                        }

                        #endregion

                        if (partnershipBinding.ZhuangTai == (int)CooperationStatus.第三方发起待审核
                            || partnershipBinding.ZhuangTai == (int)CooperationStatus.企业发起待审核)
                        {
                            partnershipBinding.ZhuangTai = (int)CooperationStatus.审批不通过;
                            partnershipBinding.Remarks = model.Remarks;
                            partnershipBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            partnershipBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            partnershipBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            _partnershipBindingTable.Update(partnershipBinding);
                        }
                        else
                        {
                            return new ServiceResult<bool>
                            {
                                ErrorMessage = "只有待审核的数据才能进行操作",
                                StatusCode = 2
                            };
                        }
                    }

                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        string sendOrgCode = "";
                        switch (userInfo.OrganizationType)
                        {
                            case (int)OrganizationType.企业:
                                sendOrgCode = model.EnterpriseCode;
                                break;
                            case (int)OrganizationType.本地服务商:
                                sendOrgCode = model.ServiceProviderCode;
                                break;
                        }

                        var orgBaseInfoModel = _orgBaseInfoRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == sendOrgCode).FirstOrDefault();
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                            ActionName = nameof(NewPartnershipBinding),
                            BizOperType = OperLogBizOperType.ADD,
                            ShortDescription =
                                $"{userInfo?.OrganizationName}（{userInfo?.OrganizationCode}）审批{orgBaseInfoModel?.OrgName}（{orgBaseInfoModel?.OrgCode}）委托关系绑定申请：{typeof(CooperationStatus).GetEnumName(CooperationStatus.审批不通过)}。",
                            OperatorName = "",
                            OldBizContent = "",
                            OperatorID = "",
                            NewBizContent = "",
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                    }

                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "审核不通过操作失败！" };
                }
            });
        }

        #endregion

        #region 审批通过

        public ServiceResult<bool> ExaminationPassed(string sysid, PartnershipBindingDto model)
        {
            //1 状态：企业发起待审核 第三方发起审核 =》  审批通过     监控模式=》第三方监控
            //2 状态： 企业发起取消合作   第三方发起取消合作=》 取消合作   当无一家第三方机构有绑定关系时 监控模式=》自主监控
            //下所有车 全部取消合作   车辆表服务商编号 变为空
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                #region 逻辑判断

                var partnershipBinding = _partnershipBindingTable
                    .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                   && x.EnterpriseCode == model.EnterpriseCode &&
                                   x.ServiceProviderCode == model.ServiceProviderCode)
                    .FirstOrDefault();
                //用于记录申请单原状态，记录操作日志用
                var oldPartnershipBindingStatus = partnershipBinding.ZhuangTai;

                if (partnershipBinding != null)
                {
                    switch (partnershipBinding.ZhuangTai)
                    {
                        case (int)CooperationStatus.企业发起待审核:
                        case (int)CooperationStatus.企业发起取消合作:
                        {
                            if (userInfo.OrganizationType == (int)OrganizationType.企业)
                            {
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能第三方机构执行此操作！",
                                    StatusCode = 2
                                };
                            }

                            break;
                        }
                        case (int)CooperationStatus.第三方发起待审核:
                        case (int)CooperationStatus.第三方发起取消合作:
                        {
                            if (userInfo.OrganizationType == (int)OrganizationType.本地服务商)
                            {
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能企业执行此操作！",
                                    StatusCode = 2
                                };
                            }

                            break;
                        }
                    }

                    #endregion

                    using (var uow = new UnitOfWork())
                    {
                        uow.BeginTransaction();
                        var enterpriseRegisterInfo =
                            _enterpriseRegisterInfoRepository.GetQuery(x =>
                                x.SYS_XiTongZhuangTai == sysZhengChang &&
                                x.OrgCode == model.EnterpriseCode).FirstOrDefault();
                        switch (partnershipBinding.ZhuangTai)
                        {
                            case (int)CooperationStatus.企业发起待审核:
                            case (int)CooperationStatus.第三方发起待审核:
                                partnershipBinding.ZhuangTai = (int)CooperationStatus.审批通过;
                                //企业备案信息表
                                if (enterpriseRegisterInfo != null)
                                {
                                    enterpriseRegisterInfo.MonitorType = (int)MonitorType.第三方监控;
                                    enterpriseRegisterInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                    enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                    enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                    _enterpriseRegisterInfoRepository.Update(enterpriseRegisterInfo);
                                }

                                break;
                            case (int)CooperationStatus.企业发起取消合作:
                            case (int)CooperationStatus.第三方发起取消合作:
                                partnershipBinding.ZhuangTai = (int)CooperationStatus.取消合作;
                                //车辆第三方机构解绑 
                                var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(x =>
                                    x.SYS_XiTongZhuangTai == sysZhengChang &&
                                    x.EnterpriseCode == model.EnterpriseCode
                                    && x.ServiceProviderCode == model.ServiceProviderCode).ToList();

                                foreach (var entity in vehicleBindingList)
                                {
                                    entity.ZhuangTai = (int)VehicleCooperationStatus.取消合作;
                                    entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                    entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                    entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                    _vehiclePartnershipBinding.Update(entity);
                                }

                                //车辆恢复企业自主监控
                                var cheLiangList = _cheLiangRepository
                                    .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                                   && x.YeHuOrgCode == model.EnterpriseCode
                                                   && x.FuWuShangOrgCode == model.ServiceProviderCode).ToList();
                                foreach (var item in cheLiangList)
                                {
                                    item.FuWuShangOrgCode = string.Empty;
                                    item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                    item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                    item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                    _cheLiangRepository.Update(item);
                                }

                                //企业恢复自主监控
                                var partnershipBindingList = _partnershipBindingTable.GetQuery(x =>
                                    x.SYS_XiTongZhuangTai == 0
                                    && (x.ZhuangTai == (int)CooperationStatus.审批通过 ||
                                        x.ZhuangTai == (int)CooperationStatus.企业发起取消合作 ||
                                        x.ZhuangTai == (int)CooperationStatus.第三方发起取消合作)
                                    && x.EnterpriseCode == model.EnterpriseCode
                                    && !x.ServiceProviderCode.Contains(model.ServiceProviderCode));
                                if (!partnershipBindingList.Any())
                                {
                                    if (enterpriseRegisterInfo != null)
                                    {
                                        enterpriseRegisterInfo.MonitorType = (int)MonitorType.自主监控;
                                        enterpriseRegisterInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                        enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                        enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                        _enterpriseRegisterInfoRepository.Update(enterpriseRegisterInfo);
                                    }
                                }

                                break;
                            default:
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "操作失败！",
                                    StatusCode = 2
                                };
                        }

                        partnershipBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        partnershipBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        partnershipBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _partnershipBindingTable.Update(partnershipBinding);
                        var updateResult = uow.CommitTransaction() > 0;

                        if (updateResult)
                        {
                            string sendOrgCode = "";
                            switch (userInfo.OrganizationType)
                            {
                                case (int)OrganizationType.企业:
                                    sendOrgCode = model.EnterpriseCode;
                                    break;
                                case (int)OrganizationType.本地服务商:
                                    sendOrgCode = model.ServiceProviderCode;
                                    break;
                            }

                            var orgBaseInfoModel = _orgBaseInfoRepository
                                .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == sendOrgCode).FirstOrDefault();

                            OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                            {
                                SystemName = OprateLogHelper.GetSysTemName(),
                                ModuleName = OperLogModuleName.委托监控.GetDesc(),
                                ActionName = nameof(ExaminationPassed),
                                BizOperType = OperLogBizOperType.UPDATE,
                                ShortDescription =
                                    $"{userInfo?.OrganizationName}（{userInfo?.OrganizationCode}）审批{orgBaseInfoModel?.OrgName}（{orgBaseInfoModel?.OrgCode}）{typeof(CooperationStatus).GetEnumName(oldPartnershipBindingStatus)}:审批通过。",
                                OperatorName = "",
                                OldBizContent = "",
                                OperatorID = "",
                                NewBizContent = "",
                                OperatorOrgCode = userInfo.OrganizationCode,
                                OperatorOrgName = userInfo.OrganizationName,
                                SysID = SysId,
                                AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                            });
                        }


                        return updateResult
                            ? new ServiceResult<bool>() { Data = true }
                            : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "审核通过操作失败！" };
                    }
                }

                return new ServiceResult<bool>()
                { Data = false, StatusCode = 2, ErrorMessage = "数据存在异常！" };
            });
        }

        #endregion

        #region 车辆第三方机构监控列表

        public ServiceResult<QueryResult> VThirdPartyMonitoringList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVThirdPartyMonitoringList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆第三方机构监控列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        /// <summary>
        /// 车辆第三方机构监控列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        private GetPartnershipBindingDto GetVThirdPartyMonitoringList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetPartnershipBindingDto();
            PartnershipBindingDto searchDto =
                JsonConvert.DeserializeObject<PartnershipBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            //合作关系表
            Expression<Func<PartnershipBindingTable, bool>> partnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            //企业
            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                partnershipBinding =
                    partnershipBinding.And(x => x.EnterpriseCode.Contains(userInfoDto.OrganizationCode));
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                partnershipBinding =
                    partnershipBinding.And(x => x.ServiceProviderCode.Contains(userInfoDto.OrganizationCode));
            }

            partnershipBinding =
                partnershipBinding.And(x =>
                    x.ZhuangTai == (int)CooperationStatus.审批通过 || x.ZhuangTai == (int)CooperationStatus.企业发起取消合作
                                                                || x.ZhuangTai == (int)CooperationStatus.第三方发起取消合作);
            var query = from etp in _partnershipBindingTable.GetQuery(partnershipBinding)
                        join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                            on etp.ServiceProviderCode equals fws.OrgCode
                        join yh in _yeHuRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                            on etp.EnterpriseCode equals yh.OrgCode
                        where string.IsNullOrEmpty(searchDto.EnterpriseName) ||
                              yh.OrgName.Contains(searchDto.EnterpriseName)
                        where string.IsNullOrEmpty(searchDto.ServiceProviderName) ||
                              fws.OrgName.Contains(searchDto.ServiceProviderName)
                        select new PartnershipBindingDto
                        {
                            Id = etp.Id,
                            EnterpriseCode = etp.EnterpriseCode,
                            EnterpriseName = yh.OrgName,
                            ServiceProviderName = fws.OrgName,
                            ServiceProviderCode = etp.ServiceProviderCode,
                            UnitOrganizationCode = etp.UnitOrganizationCode,
                            UnitType = etp.UnitType ?? 0,
                            ZhuangTai = etp.ZhuangTai ?? 0,
                            SYS_ChuangJianShiJian = etp.SYS_ChuangJianShiJian
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 发起解除绑定关系

        public ServiceResult<bool> UnbindOperation(string sysid, PartnershipBindingDto model)
        {
            //1 企业 状态：审批通过  =》  取消合作
            //企业发起解绑  企业和第三方机构直接 取消合作   企业下的车变成自主监控
            //2 第三方机构 状态： 审批通过   =》 第三方发起取消合作
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> {ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2};
                }

                if (userInfo.OrganizationType != (int) OrganizationType.企业
                    && userInfo.OrganizationType != (int) OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作"};
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                    var partnershipBinding = _partnershipBindingTable
                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                       && x.EnterpriseCode == model.EnterpriseCode &&
                                       x.ServiceProviderCode == model.ServiceProviderCode)
                        .FirstOrDefault();
                    if (partnershipBinding != null)
                    {
                        if (partnershipBinding.ZhuangTai == (int) CooperationStatus.审批通过)
                        {
                            switch (userInfo.OrganizationType)
                            {
                                case (int) OrganizationType.企业:
                                {
                                    var enterpriseRegisterInfo =
                                        _enterpriseRegisterInfoRepository.GetQuery(x =>
                                            x.SYS_XiTongZhuangTai == sysZhengChang &&
                                            x.OrgCode == model.EnterpriseCode).FirstOrDefault();

                                    partnershipBinding.ZhuangTai = (int) CooperationStatus.取消合作;
                                    //车辆第三方机构解绑 
                                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(x =>
                                        x.SYS_XiTongZhuangTai == sysZhengChang &&
                                        x.EnterpriseCode == model.EnterpriseCode
                                        && x.ServiceProviderCode == model.ServiceProviderCode).ToList();
                                    foreach (var entity in vehicleBindingList)
                                    {
                                        entity.ZhuangTai = (int) VehicleCooperationStatus.取消合作;
                                        entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                        entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                        entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                        _vehiclePartnershipBinding.Update(entity);
                                    }

                                    //车辆恢复企业自主监控
                                    var cheLiangList = _cheLiangRepository
                                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                                       && x.YeHuOrgCode == model.EnterpriseCode
                                                       && x.FuWuShangOrgCode == model.ServiceProviderCode).ToList();
                                    foreach (var item in cheLiangList)
                                    {
                                        item.FuWuShangOrgCode = string.Empty;
                                        item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                        item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                        item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                        _cheLiangRepository.Update(item);
                                    }

                                    //企业恢复自主监控
                                    var partnershipBindingList = _partnershipBindingTable.GetQuery(x =>
                                        x.SYS_XiTongZhuangTai == 0
                                        && (x.ZhuangTai == (int) CooperationStatus.审批通过 ||
                                            x.ZhuangTai == (int) CooperationStatus.企业发起取消合作 ||
                                            x.ZhuangTai == (int) CooperationStatus.第三方发起取消合作)
                                        && x.EnterpriseCode == model.EnterpriseCode
                                        && !x.ServiceProviderCode.Contains(model.ServiceProviderCode));
                                    if (!partnershipBindingList.Any())
                                    {
                                        if (enterpriseRegisterInfo != null)
                                        {
                                            enterpriseRegisterInfo.MonitorType = (int) MonitorType.自主监控;
                                            enterpriseRegisterInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                            enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                            enterpriseRegisterInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                            _enterpriseRegisterInfoRepository.Update(enterpriseRegisterInfo);
                                        }
                                    }

                                    break;
                                }

                                case (int) OrganizationType.本地服务商:
                                    partnershipBinding.ZhuangTai = (int) CooperationStatus.第三方发起取消合作;
                                    break;
                            }
                        }
                        else
                        {
                            return new ServiceResult<bool>
                            {
                                ErrorMessage = "操作失败！",
                                StatusCode = 2
                            };
                        }

                        partnershipBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        partnershipBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        partnershipBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _partnershipBindingTable.Update(partnershipBinding);
                    }

                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        string sendOrgCode = "";
                        switch (userInfo.OrganizationType)
                        {
                            case (int) OrganizationType.企业:
                                sendOrgCode = model.ServiceProviderCode;
                                break;
                            case (int) OrganizationType.本地服务商:
                                sendOrgCode = model.EnterpriseCode;
                                break;
                        }

                        var orgBaseInfoModel = _orgBaseInfoRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == sendOrgCode).FirstOrDefault();
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                            ActionName = nameof(NewPartnershipBinding),
                            BizOperType = OperLogBizOperType.ADD,
                            ShortDescription =
                                $"{userInfo?.OrganizationName}（{userInfo?.OrganizationCode}）发起{orgBaseInfoModel?.OrgName}（{orgBaseInfoModel?.OrgCode}）委托关系解绑申请。",
                            OperatorName = "",
                            OldBizContent = "",
                            OperatorID = "",
                            NewBizContent = "",
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                    }
                    return updateResult
                        ? new ServiceResult<bool>() {Data = true}
                        : new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "审核通过操作失败！"};
                }
            });
        }

        #endregion

        #region 取消绑定申请

        public ServiceResult<bool> UnbindApplication(string sysid, PartnershipBindingDto model)
        {
            //1 企业 状态：取消申请
            //2 第三方机构 状态： 取消申请
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var partnershipBinding = _partnershipBindingTable
                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                       && x.EnterpriseCode == model.EnterpriseCode &&
                                       x.ServiceProviderCode == model.ServiceProviderCode)
                        .FirstOrDefault();

                    if (partnershipBinding != null)
                    {
                        #region 逻辑处理

                        switch (partnershipBinding.ZhuangTai)
                        {
                            case (int)CooperationStatus.第三方发起待审核 when userInfo.OrganizationType == (int)OrganizationType.企业:
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能第三方执行此操作！",
                                    StatusCode = 2
                                };
                            case (int)CooperationStatus.企业发起待审核 when userInfo.OrganizationType == (int)OrganizationType.本地服务商:
                                return new ServiceResult<bool>
                                {
                                    ErrorMessage = "只能企业执行此操作！",
                                    StatusCode = 2
                                };
                        }

                        #endregion

                        if (partnershipBinding.ZhuangTai == (int)CooperationStatus.第三方发起待审核
                            || partnershipBinding.ZhuangTai == (int)CooperationStatus.企业发起待审核)
                        {
                            partnershipBinding.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                            partnershipBinding.ZhuangTai = (int)CooperationStatus.取消申请;
                        }
                        else
                        {
                            return new ServiceResult<bool>
                            {
                                ErrorMessage = "操作失败！",
                                StatusCode = 2
                            };
                        }

                        partnershipBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        partnershipBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        partnershipBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _partnershipBindingTable.Update(partnershipBinding);
                    }

                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        string sendOrgCode = "";
                        switch (userInfo.OrganizationType)
                        {
                            case (int)OrganizationType.企业:
                                sendOrgCode = model.ServiceProviderCode;
                                break;
                            case (int)OrganizationType.本地服务商:
                                sendOrgCode = model.EnterpriseCode;
                                break;
                        }

                        var orgBaseInfoModel = _orgBaseInfoRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == sendOrgCode).FirstOrDefault();
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.企业注册备案.GetDesc(),
                            ActionName = nameof(NewPartnershipBinding),
                            BizOperType = OperLogBizOperType.ADD,
                            ShortDescription =
                                $"{userInfo?.OrganizationName}（{userInfo?.OrganizationCode}）取消对{orgBaseInfoModel?.OrgName}（{orgBaseInfoModel?.OrgCode}）委托关系解绑申请。",
                            OperatorName = "",
                            OldBizContent = "",
                            OperatorID = "",
                            NewBizContent = "",
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                    }

                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "取消申请操作失败！" };
                }
            });
        }

        #endregion

        #endregion

        #region 车辆，企业，第三方机构关系绑定相关功能

        #region 车辆未绑定列表

        public ServiceResult<QueryResult> VehicleNotBoundList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVehicleNotBoundList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆未绑定列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleBindingDto GetVehicleNotBoundList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            if (userInfoDto.OrganizationType != (int)OrganizationType.企业
                && userInfoDto.OrganizationType != (int)OrganizationType.本地服务商)
            {
                return returnData;
            }

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            cheliangexp = cheliangexp.And(x => x.FuWuShangOrgCode == string.Empty);
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                x.ZhuangTai == (int)VehicleCooperationStatus.企业发起取消合作
                || x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起待审核
                || x.ZhuangTai == (int)VehicleCooperationStatus.审批通过
                || x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起取消合作
                || x.ZhuangTai == (int)VehicleCooperationStatus.企业发起待审核);
            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                cheliangexp = cheliangexp.And(x => x.YeHuOrgCode == userInfoDto.OrganizationCode);
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                cheliangexp = cheliangexp.And(x => x.FuWuShangOrgCode == userInfoDto.OrganizationCode);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                cheliangexp = cheliangexp.And(p => p.ChePaiHao.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                cheliangexp = cheliangexp.And(p => p.ChePaiYanSe == searchDto.LicensePlateColor);
            }

            var query = from car in _cheLiangRepository.GetQuery(cheliangexp)
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = car.ChePaiHao,
                            LicensePlateColor = car.ChePaiYanSe,
                            VehicleType = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = car.YeHuOrgCode,
                            SYS_ChuangJianShiJian = car.SYS_ChuangJianShiJian
                        };
            var query2 = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding
            ).Select(s =>
                new VehicleBindingDto
                {
                    LicensePlateNumber = s.LicensePlateNumber,
                    LicensePlateColor = s.LicensePlateColor
                });
            //移出相同的车牌号
            var comp = new RewriteEqualityComparer();
            var rewriteQuery = query.ToList().Except(query2.ToList(), comp);
            var vehicleBindingDto = rewriteQuery.ToList();
            returnData.Count = vehicleBindingDto.Count();
            //分页
            returnData.List = vehicleBindingDto.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 车辆绑定申请列表

        public ServiceResult<QueryResult> VehicleApplyBindingList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVehicleApplyBindingList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆绑定申请列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleBindingDto GetVehicleApplyBindingList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            if (userInfoDto.OrganizationType != (int)OrganizationType.企业
                && userInfoDto.OrganizationType != (int)OrganizationType.本地服务商)
            {
                return returnData;
            }

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.企业发起待审核);
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfoDto.OrganizationCode);
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起待审核);
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfoDto.OrganizationCode);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                vehiclePartnershipBinding = vehiclePartnershipBinding.And(p =>
                    p.LicensePlateNumber.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(p => p.LicensePlateColor == searchDto.LicensePlateColor);
            }

            var query = from vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                        join car in _cheLiangRepository.GetQuery(cheliangexp)
                            on vpb.LicensePlateNumber equals car.ChePaiHao
                        where vpb.LicensePlateColor == car.ChePaiYanSe
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = car.ChePaiHao,
                            LicensePlateColor = car.ChePaiYanSe,
                            VehicleType = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = vpb.EnterpriseCode,
                            ServiceProviderCode = vpb.ServiceProviderCode,
                            SYS_ChuangJianShiJian = vpb.SYS_ChuangJianShiJian
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 车辆绑定列表

        public ServiceResult<QueryResult> VehicleBindingList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVehicleBindingList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆绑定列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleBindingDto GetVehicleBindingList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            if (userInfoDto.OrganizationType != (int)OrganizationType.企业
                && userInfoDto.OrganizationType != (int)OrganizationType.本地服务商)
            {
                return returnData;
            }

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            vehiclePartnershipBinding =
                vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.审批通过);
            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                vehiclePartnershipBinding = vehiclePartnershipBinding.And(p =>
                    p.LicensePlateNumber.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(p => p.LicensePlateColor == searchDto.LicensePlateColor);
            }

            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfoDto.OrganizationCode);
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfoDto.OrganizationCode);
            }

            var query = from vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                        join car in _cheLiangRepository.GetQuery(cheliangexp)
                            on vpb.LicensePlateNumber equals car.ChePaiHao
                        where vpb.LicensePlateColor == car.ChePaiYanSe
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = car.ChePaiHao,
                            LicensePlateColor = car.ChePaiYanSe,
                            VehicleType = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = vpb.EnterpriseCode,
                            ServiceProviderCode = vpb.ServiceProviderCode,
                            SYS_ChuangJianShiJian = vpb.SYS_ChuangJianShiJian
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 车辆解绑申请列表

        public ServiceResult<QueryResult> VehicleUApplicationList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVehicleUApplicationList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆解绑申请列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleBindingDto GetVehicleUApplicationList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            if (userInfoDto.OrganizationType != (int)OrganizationType.企业
                && userInfoDto.OrganizationType != (int)OrganizationType.本地服务商)
            {
                return returnData;
            }

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            switch (userInfoDto.OrganizationType)
            {
                case (int)OrganizationType.企业:
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.企业发起取消合作);
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfoDto.OrganizationCode);
                    break;
                case (int)OrganizationType.本地服务商:
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起取消合作);
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfoDto.OrganizationCode);
                    break;
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                vehiclePartnershipBinding = vehiclePartnershipBinding.And(p =>
                    p.LicensePlateNumber.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(p => p.LicensePlateColor == searchDto.LicensePlateColor);
            }

            var query = from vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                        join car in _cheLiangRepository.GetQuery(cheliangexp)
                            on vpb.LicensePlateNumber equals car.ChePaiHao
                        where vpb.LicensePlateColor == car.ChePaiYanSe
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = car.ChePaiHao,
                            LicensePlateColor = car.ChePaiYanSe,
                            VehicleType = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = vpb.EnterpriseCode,
                            ServiceProviderCode = vpb.ServiceProviderCode,
                            SYS_ChuangJianShiJian = vpb.SYS_ChuangJianShiJian
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 车辆绑定申请

        public ServiceResult<bool> VehicleBindingApplication(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：  =》  企业发起待审核
            //2 第三方机构 状态：    =》 第三方发起审核

            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return new ServiceResult<bool> {ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2};
            }

            var key = userInfo.Id;
            return ExecuteCommandStruct<bool>(() =>
            {
                lock (CWHelper.GetStringLock(key, "AddVehicle"))
                {
                    var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                    if (userInfo.OrganizationType == (int) OrganizationType.企业)
                    {
                        var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == sysZhengChang
                            && x.OrgCode == userInfo.OrganizationCode);
                        if (!anQuanLiRenYuaneneity.Any())
                        {
                            return new ServiceResult<bool>()
                                {Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！"};
                        }
                    }

                    if (userInfo.OrganizationType != (int) OrganizationType.企业
                        && userInfo.OrganizationType != (int) OrganizationType.本地服务商)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作"};
                    }

                    var vehicleList = _cheLiangRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常)
                        .ToList();
                    var status = false;
                    var abnormal = string.Empty;
                    var vehicleVideo = _cheLiangVideoZhongDuanConfirmRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常)
                        .ToList();
                    foreach (var entity in dto)
                    {
                        var vehicle = vehicleList.Find(x =>
                            x.ChePaiHao == entity.LicensePlateNumber && x.ChePaiYanSe == entity.LicensePlateColor);
                        if (vehicle == null)
                        {
                            return new ServiceResult<bool>()
                                {Data = false, StatusCode = 2, ErrorMessage = entity.LicensePlateNumber + "存在异常"};
                        }

                        if (vehicle.CheLiangZhongLei == (int) CheLiangZhongLei.旅游包车 || vehicle.CheLiangZhongLei ==
                                                                                    (int) CheLiangZhongLei.客运班车
                                                                                    || vehicle.CheLiangZhongLei ==
                                                                                    (int) CheLiangZhongLei.危险货运
                                                                                    || vehicle.CheLiangZhongLei ==
                                                                                    (int) CheLiangZhongLei.重型货车)
                        {
                            var vehicleId = vehicle.Id.ToString();
                            var vehicleVideoEntity = vehicleVideo.Find(x => x.CheLiangId == vehicleId);
                            if (vehicleVideoEntity != null &&
                                vehicleVideoEntity.BeiAnZhuangTai == (int) ZhongDuanBeiAnZhuangTai.通过备案) continue;
                            status = true;
                            abnormal += vehicle.ChePaiHao + " 备案未通过,";
                        }
                        else
                        {
                            if (vehicle.GPSAuditStatus == (int) GPSAuditStatus.通过备案) continue;
                            status = true;
                            abnormal += vehicle.ChePaiHao + " GPS审核未通过,";
                        }
                    }

                    if (status)
                    {
                        abnormal = abnormal.TrimEnd(',');
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = abnormal};

                    }

                    #region 业务处理

                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;
                    var vehicleBindingList =
                        _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding).ToList();

                    var vehiclePartnershipBindingAdd = new List<VehiclePartnershipBinding>();
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity != null)
                        {
                            if (userInfo.OrganizationType == (int) OrganizationType.企业)
                            {
                                entity.ZhuangTai = (int) VehicleCooperationStatus.企业发起待审核;
                            }
                            else if (userInfo.OrganizationType == (int) OrganizationType.本地服务商)
                            {
                                entity.ZhuangTai = (int) VehicleCooperationStatus.第三方发起待审核;
                            }
                            entity.ServiceProviderCode = item.ServiceProviderCode;
                            entity.Remarks = string.Empty;
                            entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            vehiclePartnershipBindingUpdate.Add(entity);
                        }
                        else
                        {
                            var dataTime = new VehiclePartnershipBinding
                            {
                                Id = Guid.NewGuid(),
                                EnterpriseCode = item.EnterpriseCode,
                                ServiceProviderCode = item.ServiceProviderCode,
                                SYS_ChuangJianShiJian = DateTime.Now,
                                SYS_ChuangJianRen = userInfo.UserName,
                                SYS_ChuangJianRenID = userInfo.Id,
                                LicensePlateNumber = item.LicensePlateNumber,
                                LicensePlateColor = item.LicensePlateColor,
                                SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常
                            };
                            if (userInfo.OrganizationType == (int) OrganizationType.企业)
                            {
                                dataTime.ZhuangTai = (int) CooperationStatus.企业发起待审核;
                            }
                            else if (userInfo.OrganizationType == (int) OrganizationType.本地服务商)
                            {
                                dataTime.ZhuangTai = (int) CooperationStatus.第三方发起待审核;
                            }

                            vehiclePartnershipBindingAdd.Add(dataTime);
                        }
                    }

                    #endregion

                    using (var uow = new UnitOfWork())
                    {
                        uow.BeginTransaction();

                        vehiclePartnershipBindingAdd.ForEach(x => { _vehiclePartnershipBinding.Add(x); });
                        vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                        var updateResult = uow.CommitTransaction() > 0;
                        return updateResult
                            ? new ServiceResult<bool>() {Data = true}
                            : new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "车辆绑定申请失败！"};
                    }
                }
            });
        }

        #endregion

        #region 撤销车辆绑定申请

        public ServiceResult<bool> VehicleRevokeBinding(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：企业发起待审核  =》  取消申请
            //2 第三方机构 状态： 第三方发起审核   =》 取消申请
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 业务处理

                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;
                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.企业:
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.企业发起待审核);
                            break;
                        case (int)OrganizationType.本地服务商:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起待审核);
                            break;
                    }

                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding);
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity != null)
                        {
                            entity.ZhuangTai = (int)VehicleCooperationStatus.取消申请;
                            entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            vehiclePartnershipBindingUpdate.Add(entity);
                        }
                    }

                    #endregion

                    vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                    var updateResult = uow.CommitTransaction() > 0;
                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "撤销车辆绑定申请失败！" };
                }
            });
        }

        #endregion

        #region 车辆第三方机构解绑申请

        public ServiceResult<bool> VehicleUnbindingApplication(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：审核通过  =》  取消合作
            //企业 对自己车解绑不需要第三方机构审核
            //2 第三方机构 状态： 第三方发起审核   =》 第三方发起取消合作
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 业务处理

                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.审批通过);
                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding);
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    var cheLiangUpdate = new List<CheLiang>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity == null) continue;
                        {
                            switch (userInfo.OrganizationType)
                            {
                                case (int)OrganizationType.企业:
                                {
                                    entity.ZhuangTai = (int)VehicleCooperationStatus.取消合作;

                                    var vehicleEntity = _cheLiangRepository
                                        .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                                       && x.ChePaiHao == item.LicensePlateNumber &&
                                                       x.ChePaiYanSe == item.LicensePlateColor)
                                        .FirstOrDefault();
                                    if (vehicleEntity != null)
                                    {
                                        vehicleEntity.FuWuShangOrgCode = string.Empty;
                                        vehicleEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                        vehicleEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                        vehicleEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                        cheLiangUpdate.Add(vehicleEntity);
                                    }

                                    break;
                                }

                                case (int)OrganizationType.本地服务商:
                                    entity.ZhuangTai = (int)VehicleCooperationStatus.第三方发起取消合作;
                                    break;
                            }

                            entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            vehiclePartnershipBindingUpdate.Add(entity);
                        }
                    }

                    #endregion

                    vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                    cheLiangUpdate.ForEach(x => { _cheLiangRepository.Update(x); });
                    var updateResult = uow.CommitTransaction() > 0;
                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "车辆第三方机构解绑申请失败！" };
                }
            });
        }

        #endregion

        #region 撤销车辆第三方机构解绑申请

        public ServiceResult<bool> VehicleRevokeUnbinding(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：企业发起取消合作  =》  审核通过
            //2 第三方机构 状态： 第三方发起取消合作   =》 审核通过
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (userInfo.OrganizationType == (int)OrganizationType.企业)
                {
                    var anQuanLiRenYuaneneity = _anQuanGuanLiRenYuan.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == userInfo.OrganizationCode);
                    if (!anQuanLiRenYuaneneity.Any())
                    {
                        return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "当前企业无安全人员，无法执行此操作！" };
                    }
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 业务处理

                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;
                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.企业:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.企业发起取消合作);
                            break;
                        case (int)OrganizationType.本地服务商:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起取消合作);
                            break;
                    }

                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding);
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity == null) continue;
                        entity.ZhuangTai = (int)VehicleCooperationStatus.审批通过;
                        entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        vehiclePartnershipBindingUpdate.Add(entity);
                    }

                    #endregion

                    vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                    var updateResult = uow.CommitTransaction() > 0;
                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "撤销车辆第三方机构解绑申请失败！" };
                }
            });
        }

        #endregion

        #region 车辆第三方机构绑定关系列表

        public ServiceResult<QueryResult> VehicleBindingRelationshipsList(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetVehicleBindingRelationshipsList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆第三方机构绑定关系列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleBindingDto GetVehicleBindingRelationshipsList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            if (userInfoDto.OrganizationType == (int)OrganizationType.企业)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfoDto.OrganizationCode);
            }
            else if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfoDto.OrganizationCode);
            }

            //审核状态
            if (searchDto.ZhuangTai != null)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ZhuangTai == searchDto.ZhuangTai);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                vehiclePartnershipBinding = vehiclePartnershipBinding.And(p =>
                    p.LicensePlateNumber.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(p => p.LicensePlateColor == searchDto.LicensePlateColor);
            }

            var query = from vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                        join car in _cheLiangRepository.GetQuery(cheliangexp)
                            on vpb.LicensePlateNumber equals car.ChePaiHao
                        where vpb.LicensePlateColor == car.ChePaiYanSe
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                            on vpb.ServiceProviderCode equals fws.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = vpb.LicensePlateNumber,
                            LicensePlateColor = vpb.LicensePlateColor,
                            VehicleType = car.CheLiangZhongLei,
                            ServiceProviderName = fws.OrgName,
                            EnterpriseName = org.OrgName,
                            ZhuangTai = vpb.ZhuangTai,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = vpb.EnterpriseCode,
                            ServiceProviderCode = vpb.ServiceProviderCode,
                            SYS_ChuangJianShiJian = vpb.SYS_ChuangJianShiJian
                        };
            returnData.Count = query.Count();
            //分页
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 车辆第三方机构审核通过

        public ServiceResult<bool> VehicleBindingToExamine(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：企业发起待审核  =》  审核通过
            //企业发起取消合作=》 取消合作
            //2 第三方机构 状态： 第三方发起审核   =》 审核通过
            //第三方发起取消合作=》 取消合作
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 业务处理

                    var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;

                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.企业:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起待审核
                                || x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起取消合作);
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfo.OrganizationCode);
                            break;
                        case (int)OrganizationType.本地服务商:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.企业发起取消合作
                                || x.ZhuangTai == (int)VehicleCooperationStatus.企业发起待审核);
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfo.OrganizationCode);
                            break;
                    }

                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding);
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    var cheLiangUpdate = new List<CheLiang>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity == null) continue;
                        {
                            var fuWuShangOrgCode = string.Empty;
                            switch (entity.ZhuangTai)
                            {
                                case (int)VehicleCooperationStatus.企业发起待审核:
                                case (int)VehicleCooperationStatus.第三方发起待审核:
                                    entity.ZhuangTai = (int)VehicleCooperationStatus.审批通过;
                                    fuWuShangOrgCode = item.ServiceProviderCode;
                                    break;
                                case (int)VehicleCooperationStatus.企业发起取消合作:
                                case (int)VehicleCooperationStatus.第三方发起取消合作:
                                    entity.ZhuangTai = (int)VehicleCooperationStatus.取消合作;
                                    break;
                            }

                            var vehicleEntity = _cheLiangRepository
                                .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang
                                               && x.ChePaiHao == item.LicensePlateNumber &&
                                               x.ChePaiYanSe == item.LicensePlateColor)
                                .FirstOrDefault();
                            if (vehicleEntity != null)
                            {
                                vehicleEntity.FuWuShangOrgCode = fuWuShangOrgCode;
                                vehicleEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                vehicleEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                vehicleEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                cheLiangUpdate.Add(vehicleEntity);
                            }

                            entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            vehiclePartnershipBindingUpdate.Add(entity);
                        }
                    }

                    #endregion

                    vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                    cheLiangUpdate.ForEach(x => { _cheLiangRepository.Update(x); });
                    var updateResult = uow.CommitTransaction() > 0;
                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "车辆第三方机构审核通过失败！" };
                }
            });
        }

        #endregion

        #region 车辆第三方机构审核不通过

        public ServiceResult<bool> VehicleBindingFailToExamine(List<VehicleBindingDto> dto)
        {
            //1 企业 状态：企业发起待审核  =》  审核不通过
            //2 第三方机构 状态： 第三方发起审核   =》 审核不通过
            return ExecuteCommandStruct<bool>(() =>
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                if (userInfo.OrganizationType != (int)OrganizationType.企业
                    && userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool>()
                    { Data = false, StatusCode = 2, ErrorMessage = "只有企业，与第三方才能执行此操作" };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 业务处理

                    var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                    Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                        q => q.SYS_XiTongZhuangTai == sysZhengChang;

                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.企业:
                            vehiclePartnershipBinding = vehiclePartnershipBinding.And(x =>
                                x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起待审核);
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfo.OrganizationCode);
                            break;
                        case (int)OrganizationType.本地服务商:
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.ZhuangTai == (int)VehicleCooperationStatus.企业发起待审核);
                            vehiclePartnershipBinding =
                                vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfo.OrganizationCode);
                            break;
                    }

                    var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding);
                    var vehiclePartnershipBindingUpdate = new List<VehiclePartnershipBinding>();
                    foreach (var item in dto)
                    {
                        var entity = vehicleBindingList.FirstOrDefault(x =>
                            x.LicensePlateColor == item.LicensePlateColor &&
                            x.LicensePlateNumber == item.LicensePlateNumber);
                        if (entity == null) continue;
                        entity.ZhuangTai = (int)VehicleCooperationStatus.审批不通过;
                        entity.Remarks = item.Remarks;
                        entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        vehiclePartnershipBindingUpdate.Add(entity);
                    }

                    #endregion

                    vehiclePartnershipBindingUpdate.ForEach(x => { _vehiclePartnershipBinding.Update(x); });
                    var updateResult = uow.CommitTransaction() > 0;
                    return updateResult
                        ? new ServiceResult<bool>() { Data = true }
                        : new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "车辆第三方机构审核不通过失败！" };
                }
            });
        }

        #endregion

        #region 统计分析

        public ServiceResult<QueryResult> StatisticalAnalysis(QueryData queryData)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetStatisticalAnalysisList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询统计分析列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetVehicleStatisticalDto GetStatisticalAnalysisList(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleStatisticalDto();
            VehicleStatisticalDto searchDto =
                JsonConvert.DeserializeObject<VehicleStatisticalDto>(JsonConvert.SerializeObject(queryData.data));
            var whereSql = "where 1=1";
            if (!string.IsNullOrWhiteSpace(searchDto?.OrgName))
            {
                whereSql += $"  AND OrgName like '%{searchDto.OrgName.Trim()}%'";
            }

            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                string sql = $@"SELECT *
FROM
(
    SELECT a.OrgCode,
           a.OrgName,
           a.XiaQuXian,
           a.PrincipalName,
a.Id,
           ISNULL(c.MonitorPersonCount, 0) MonitorPersonCount,
           ISNULL(b.SecurityPersonnelCount, 0) SecurityPersonnelCount,
           ISNULL(c.PersonnelSocialCount, 0) PersonnelSocialCount,
           ISNULL(c.ServiceContractCount, 0) ServiceContractCount,
           ISNULL(c.PersonnelTrainingCount, 0) PersonnelTrainingCount,
           ISNULL(d.BindTPartyCount, 0) BindTPartyCount,
           ISNULL(e.UnboundVehicleCount, 0) UnboundVehicleCount,
           ISNULL(e.BoundVehicleCount, 0) BoundVehicleCount,
		   ROW_NUMBER() over(order by a.SYS_ChuangJianShiJian desc) as 'RowNumber',
		   a.SYS_ChuangJianShiJian
    FROM
    (
        SELECT yh.OrgName,
               oif.XiaQuXian,
               eri.PrincipalName,
               eri.OrgCode,
			   eri.SYS_ChuangJianShiJian,
       eri.Id
        FROM T_EnterpriseRegisterInfo eri
            JOIN dbo.T_CheLiangYeHu yh
                ON yh.OrgCode = eri.OrgCode
            JOIN dbo.T_OrgBaseInfo oif
                ON oif.Id = yh.BaseId
        WHERE eri.SYS_XiTongZhuangTai = 0
              AND yh.SYS_XiTongZhuangTai = 0
              AND oif.SYS_XiTongZhuangTai = 0
    ) a
        LEFT JOIN
        (
            SELECT COUNT(*) SecurityPersonnelCount,
                   OrgCode
            FROM T_AnQuanGuanLiRenYuan
            WHERE SYS_XiTongZhuangTai = 0
            GROUP BY OrgCode
        ) b
            ON b.OrgCode = a.OrgCode
        LEFT JOIN
        (
            SELECT SUM(   CASE
                              WHEN SocialSecurityContractFileId IS NOT NULL
                                   AND SocialSecurityContractFileId <> '' THEN
                                  1
                              ELSE
                                  0
                          END
                      ) PersonnelSocialCount,   --人员社保证明数量
                   SUM(   CASE
                              WHEN LaborContractFileId IS NOT NULL
                                   AND LaborContractFileId <> '' THEN
                                  1
                              ELSE
                                  0
                          END
                      ) ServiceContractCount,   --人员劳务合同证明材料数量
                   SUM(   CASE
                              WHEN CertificatePassingExaminationFileId IS NOT NULL
                                   AND CertificatePassingExaminationFileId <> '' THEN
                                  1
                              ELSE
                                  0
                          END
                      ) PersonnelTrainingCount, --监控人员培训证明材料数量
                   COUNT(*) MonitorPersonCount,
                   OrgCode
            FROM T_MonitorPersonInfo
            WHERE SYS_XiTongZhuangTai = 0
            GROUP BY OrgCode
        ) c
            ON c.OrgCode = a.OrgCode
        LEFT JOIN
        (
            SELECT COUNT(*) BindTPartyCount,
                   EnterpriseCode
            FROM T_PartnershipBindingTable
            WHERE SYS_XiTongZhuangTai = 0
                  AND
                  (
                      ZhuangTai = 3
                      OR ZhuangTai = 5
                      OR ZhuangTai = 6
                  )
            GROUP BY EnterpriseCode
        ) d
            ON d.EnterpriseCode = a.OrgCode
        LEFT JOIN
        (
            SELECT SUM(   CASE
                              WHEN FuWuShangOrgCode IS NULL
                                   OR FuWuShangOrgCode = '' THEN
                                  1
                              ELSE
                                  0
                          END
                      ) UnboundVehicleCount, --未绑定第三方车辆数
                   SUM(   CASE
                              WHEN FuWuShangOrgCode IS NOT NULL
                                   AND FuWuShangOrgCode <> '' THEN
                                  1
                              ELSE
                                  0
                          END
                      ) BoundVehicleCount,   --已绑定第三方车辆数
                   YeHuOrgCode
            FROM dbo.T_CheLiang
            WHERE SYS_XiTongZhuangTai = 0
            GROUP BY YeHuOrgCode
        ) e
            ON e.YeHuOrgCode = a.OrgCode
) t
 {whereSql}  ";

                var rowStart = (queryData.page - 1) * queryData.rows + 1;
                var rowEnd = rowStart + queryData.rows - 1;
                var queryCount = $@"select count(*) from ({sql} ) countT";
                var count = conn.ExecuteScalar<int>(queryCount);
                sql += $" and t.rowNumber between {rowStart} and {rowEnd}    ORDER BY SYS_ChuangJianShiJian DESC";
                var vehicleStatisticalList = conn.Query<VehicleStatisticalDto>(sql).ToList();
                returnData.Count = count;
                returnData.List = vehicleStatisticalList;
            }

            return returnData;
        }

        #endregion

        #endregion

        #region

        public class RewriteEqualityComparer : EqualityComparer<VehicleBindingDto>
        {
            public override bool Equals(VehicleBindingDto compare, VehicleBindingDto compared)
            {
                return compare.LicensePlateNumber == compared.LicensePlateNumber &&
                       compare.LicensePlateColor == compared.LicensePlateColor;
            }

            public override int GetHashCode(VehicleBindingDto obj)
            {
                return obj.LicensePlateNumber.GetHashCode() ^ obj.LicensePlateColor.GetHashCode();
            }
        }


        public ServiceResult<QueryResult> EnterpriseList(QueryData queryData)
        {

            try
            {
                UserInfoDtoNew userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                GetQiYeInfoDto returnData = GetQiYeQueryInfoList(queryData, userInfoDto);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }


        }

        private GetQiYeInfoDto GetQiYeQueryInfoList(QueryData queryData, UserInfoDtoNew userInfoDto)
        {
            GetQiYeInfoDto returnData = new GetQiYeInfoDto();
            QiyeSearch searchDto =
                JsonConvert.DeserializeObject<QiyeSearch>(JsonConvert.SerializeObject(queryData.data));
            int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiangYeHu, bool>> YeHuExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<PartnershipBindingTable, bool>> partnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;

            partnershipBinding =
                partnershipBinding.And(x =>
                    x.ZhuangTai == (int)CooperationStatus.审批通过 || x.ZhuangTai == (int)CooperationStatus.企业发起取消合作
                                                                || x.ZhuangTai == (int)CooperationStatus.第三方发起取消合作);
            partnershipBinding =
                partnershipBinding.And(x => x.ServiceProviderCode.Contains(userInfoDto.OrganizationCode));
            //企业名称
            if (!string.IsNullOrEmpty(searchDto.YeHuMingCheng))
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgName.Contains(searchDto.YeHuMingCheng.Trim()));
            }

            //企业代码
            if (!string.IsNullOrWhiteSpace(searchDto.OrgCode))
            {
                partnershipBinding = partnershipBinding.And(u => u.EnterpriseCode == searchDto.OrgCode.Trim());
            }

            if (searchDto.OrgType != null)
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgType == searchDto.OrgType);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.JingYingXuKeZhengHao))
            {
                YeHuExp = YeHuExp.And(x => x.JingYingXuKeZhengHao.Contains(searchDto.JingYingXuKeZhengHao.Trim()));
            }


            var query = from par in _partnershipBindingTable.GetQuery(partnershipBinding)
                        join b in _yeHuRepository.GetQuery(YeHuExp)
                            on par.EnterpriseCode equals b.OrgCode
                        join a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                            on b.BaseId equals a.Id.ToString()
                        select new QueryQiYeResponseDto
                        {
                            OrgName = b.OrgName,
                            OrgCode = b.OrgCode,
                            Id = a.Id,
                            ChuangJianShiJian = par.SYS_ChuangJianShiJian,
                            XiaQuSheng = a.XiaQuSheng,
                            XiaQuShi = a.XiaQuShi,
                            XiaQuXian = a.XiaQuXian,
                            JingYingXuKeZhengHao = b.JingYingXuKeZhengHao
                        };
            returnData.Count = query.Count();
            //分页
            returnData.list = query.OrderByDescending(x => x.ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
            return returnData;
        }

        #endregion

        #region 企业监控人员详细

        public ServiceResult<QueryResult> EnterpriseMonitoringList(QueryData queryData)
        {

            try
            {
                var returnData = new GetPersonnelInformationDto();
                PersonnelInformation searchDto =
                    JsonConvert.DeserializeObject<PersonnelInformation>(JsonConvert.SerializeObject(queryData.data));
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //监控人员
                Expression<Func<MonitorPersonInfo, bool>> monitorPersonInfo =
                    t => t.SYS_XiTongZhuangTai == sysZhengChang;
                monitorPersonInfo = monitorPersonInfo.And(x => x.OrgCode == searchDto.OrgCode);
                var query = _monitorPersonInfo.GetQuery(monitorPersonInfo).Select(x => new PersonnelInformation
                {
                    Name = x.Name,
                    SYS_ChuangJianShiJian = (DateTime)x.SYS_ChuangJianShiJian,
                    IDCard = x.IDCard,
                    Tel = x.Tel,
                    LaborContractFileId = x.LaborContractFileId,
                    CertificatePassingExaminationFileId = x.CertificatePassingExaminationFileId,
                    SocialSecurityContractFileId = x.SocialSecurityContractFileId
                });
                returnData.Count = query.Count();
                //分页
                returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业监控人员详细列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion

        #region 企业安全人员详细

        public ServiceResult<QueryResult> EnterpriseSecurityList(QueryData queryData)
        {

            try
            {
                var returnData = new GetPersonnelInformationDto();
                PersonnelInformation searchDto =
                    JsonConvert.DeserializeObject<PersonnelInformation>(JsonConvert.SerializeObject(queryData.data));
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //安全人员
                Expression<Func<AnQuanGuanLiRenYuan, bool>> anQuanGuanLiRenYuan =
                    t => t.SYS_XiTongZhuangTai == sysZhengChang;
                anQuanGuanLiRenYuan = anQuanGuanLiRenYuan.And(x => x.OrgCode == searchDto.OrgCode);
                var query = _anQuanGuanLiRenYuan.GetQuery(anQuanGuanLiRenYuan).Select(x => new PersonnelInformation
                {
                    Name = x.Name,
                    SYS_ChuangJianShiJian = (DateTime)x.SYS_ChuangJianShiJian,
                    IDCard = x.IDCard,
                    Tel = x.Tel,
                    LaborContractFileId = x.LaborContractFileId,
                    CertificatePassingExaminationFileId = x.CertificatePassingExaminationFileId,
                    SocialSecurityContractFileId = x.SocialSecurityContractFileId
                });
                returnData.Count = query.Count();
                //分页
                returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业安全人员详细列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion

        #region 企业车辆绑定详细信息

        public ServiceResult<QueryResult> EnterpriseVehicleBindingList(QueryData queryData)
        {

            try
            {
                var returnData = new GetVehicleBindingDto();
                VehicleBindingDto searchDto =
                    JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //车辆基础档案列表
                Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                //组织表
                Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                cheliangexp = cheliangexp.And(x => x.YeHuOrgCode == searchDto.OrgCode);
                //绑定车辆
                if (searchDto.Type == 1)
                {
                    cheliangexp = cheliangexp.And(x => x.FuWuShangOrgCode != null && x.FuWuShangOrgCode != "");
                }
                //未绑定车辆
                else if (searchDto.Type == 2)
                {
                    cheliangexp = cheliangexp.And(x => x.FuWuShangOrgCode == null || x.FuWuShangOrgCode == "");
                }

                var query = from car in _cheLiangRepository.GetQuery(cheliangexp)
                            join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                                on car.YeHuOrgCode equals org.OrgCode
                            join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                                on car.FuWuShangOrgCode equals fws.OrgCode
                                into fwsList
                            from item in fwsList.DefaultIfEmpty()
                            select new VehicleBindingDto
                            {
                                LicensePlateNumber = car.ChePaiHao,
                                LicensePlateColor = car.ChePaiYanSe,
                                VehicleType = car.CheLiangZhongLei,
                                XiaQuShi = car.XiaQuShi,
                                XiaQuXian = car.XiaQuXian,
                                XiaQuSheng = car.XiaQuSheng,
                                SYS_ChuangJianShiJian = car.SYS_ChuangJianShiJian,
                                ServiceProviderName = item.OrgName
                            };
                returnData.Count = query.Count();
                //分页
                returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业车辆绑定出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion

        #region 企业详细信息

        public ServiceResult<EnterpriseDetailsDto> EnterpriseDetails(EnterpriseDetailsDto queryData)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return null;
                }

                var result = new ServiceResult<EnterpriseDetailsDto>();

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //企业备案信息表
                Expression<Func<EnterpriseRegisterInfo, bool>> enterpriseRegisterInfo =
                    t => t.SYS_XiTongZhuangTai == sysZhengChang;
                enterpriseRegisterInfo = enterpriseRegisterInfo.And(x => x.OrgCode == queryData.OrgCode);
                //组织表
                Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                var query = from car in _enterpriseRegisterInfoRepository.GetQuery(enterpriseRegisterInfo)
                            join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                                on car.OrgCode equals org.OrgCode
                            select new EnterpriseDetailsDto
                            {
                                EnterpriseName = org.OrgName,
                                XiaQuXian = org.XiaQuXian,
                                PrincipalName = car.PrincipalName,
                                BusinessPermitNumber = car.BusinessPermitNumber,
                                PrincipalIDCard = car.PrincipalIDCard,
                                PrincipalTel = car.PrincipalTel,
                                MonitorType = car.MonitorType,
                                EnterpriseType = car.EnterpriseType,
                                BusinessLicenseFileId = car.BusinessLicenseFileId
                            };
                var model = query.FirstOrDefault();
                if (model == null) return result;
                result.Data = model;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("企业详细信息出错" + ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region 企业绑定第三放机构

        public ServiceResult<QueryResult> EnterpriseBindingList(QueryData queryData)
        {
            try
            {
                var returnData = new GetPartnershipBindingDto();
                PartnershipBindingDto searchDto =
                    JsonConvert.DeserializeObject<PartnershipBindingDto>(JsonConvert.SerializeObject(queryData.data));
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //合作关系表
                Expression<Func<PartnershipBindingTable, bool>> partnershipBinding =
                    q => q.SYS_XiTongZhuangTai == sysZhengChang;

                Expression<Func<FuWuShang, bool>> fuWuShang =
                    q => q.SYS_XiTongZhuangTai == sysZhengChang;
                //组织表
                Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                partnershipBinding =
                    partnershipBinding.And(x => x.EnterpriseCode == searchDto.OrgCode);
                partnershipBinding =
                    partnershipBinding.And(x =>
                        x.ZhuangTai == (int)CooperationStatus.审批通过 || x.ZhuangTai == (int)CooperationStatus.企业发起取消合作
                                                                    || x.ZhuangTai ==
                                                                    (int)CooperationStatus.第三方发起取消合作);
                var query = from etp in _partnershipBindingTable.GetQuery(partnershipBinding)
                            join fws in _fuWuShangRepository.GetQuery(fuWuShang)
                                on etp.ServiceProviderCode equals fws.OrgCode
                            join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                                on fws.OrgCode equals org.OrgCode
                            select new PartnershipBindingDto
                            {
                                UnitType = etp.UnitType ?? 0,
                                ServiceProviderName = fws.OrgName,
                                SYS_ChuangJianShiJian = etp.SYS_ChuangJianShiJian,
                                ServiceProviderCode = etp.ServiceProviderCode,
                                XiaQuShi = org.XiaQuShi,
                                XiaQuXian = org.XiaQuXian,
                                XiaQuSheng = org.XiaQuSheng
                            };
                returnData.Count = query.Count();
                //分页
                returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };

            }
            catch (Exception e)
            {
                LogHelper.Error("企业第三方机构错误" + e.Message, e);
                return null;
            }
        }

        #endregion

        #region  企业审核通过列表

        public ServiceResult<QueryResult> EnterpriseApproved(QueryData queryData)
        {
            try
            {
                GetQiYeInfoDto returnData = new GetQiYeInfoDto();
                QiyeSearch searchDto =
                    JsonConvert.DeserializeObject<QiyeSearch>(JsonConvert.SerializeObject(queryData.data));

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<CheLiangYeHu, bool>> YeHuExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<EnterpriseRegisterInfo, bool>> enterpriseRegisterInfo =
                    q => q.SYS_XiTongZhuangTai == sysZhengChang;
                enterpriseRegisterInfo =
                    enterpriseRegisterInfo.And(x =>
                        x.ApprovalStatus == (int)RegisterInfoApprovalStatus.审核通过);
                //企业名称
                if (!string.IsNullOrEmpty(searchDto.YeHuMingCheng))
                {
                    OrgBaseExp = OrgBaseExp.And(u => u.OrgName.Contains(searchDto.YeHuMingCheng.Trim()));
                }

                if (searchDto.OrgType != null)
                {
                    OrgBaseExp = OrgBaseExp.And(u => u.OrgType == searchDto.OrgType);
                }

                if (!string.IsNullOrWhiteSpace(searchDto.JingYingXuKeZhengHao))
                {
                    YeHuExp = YeHuExp.And(x => x.JingYingXuKeZhengHao.Contains(searchDto.JingYingXuKeZhengHao.Trim()));
                }

                var query = from par in _enterpriseRegisterInfoRepository.GetQuery(enterpriseRegisterInfo)
                            join b in _yeHuRepository.GetQuery(YeHuExp)
                                on par.OrgCode equals b.OrgCode
                            join a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                                on b.BaseId equals a.Id.ToString()
                            select new QueryQiYeResponseDto
                            {
                                OrgName = b.OrgName,
                                OrgCode = b.OrgCode,
                                Id = a.Id,
                                ChuangJianShiJian = par.SYS_ChuangJianShiJian,
                                XiaQuSheng = a.XiaQuSheng,
                                XiaQuShi = a.XiaQuShi,
                                XiaQuXian = a.XiaQuXian,
                                JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                                QiYeXingZhi=b.QiYeXingZhi
                            };
                returnData.Count = query.Count();
                //分页
                returnData.list = query.OrderByDescending(x => x.ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.list };
                return new ServiceResult<QueryResult> { Data = result };

            }
            catch (Exception e)
            {
                LogHelper.Error("企业审核通过列表错误" + e.Message, e);
                return null;
            }
        }

        #endregion

        public ServiceResult<ExportResponseInfoDto> ExportEnterpriseMonitoring(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetStatisticalAnalysisList(queryData, userInfo).List;
                var tableTitle = "企业委托监控统计" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var FileId = string.Empty;
                        var fileUploadId = CreateQiYeDangAnExcelAndUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            FileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出企业委托监控统计出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出企业委托监控统计出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        #region 导出企业委托监控统计

        private static Guid? CreateQiYeDangAnExcelAndUpload(List<VehicleStatisticalDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "企业委托监控统计";
            string[] cellTitleArry =
            {
                "企业名称", "辖区县", "安全责任人", "监控员数量", "安全人员数量", "人员社保证明数量", "人员劳动合同数量",
                "监控人员培训数量", "绑定第三方数量", "未绑定第三方车辆数", "已绑定第三方车辆数", "创建时间"
            };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;

                    row.CreateCell(index++).SetCellValue(item.OrgName);
                    row.CreateCell(index++).SetCellValue(item.XiaQuXian);
                    row.CreateCell(index++).SetCellValue(item.PrincipalName);
                    row.CreateCell(index++).SetCellValue(item.MonitorPersonCount);
                    row.CreateCell(index++).SetCellValue(item.SecurityPersonnelCount);
                    row.CreateCell(index++).SetCellValue(item.PersonnelSocialCount);
                    row.CreateCell(index++).SetCellValue(item.ServiceContractCount);
                    row.CreateCell(index++).SetCellValue(item.PersonnelTrainingCount);
                    row.CreateCell(index++).SetCellValue(item.BindTPartyCount);
                    row.CreateCell(index++).SetCellValue(item.UnboundVehicleCount);
                    row.CreateCell(index++).SetCellValue(item.BoundVehicleCount);
                    row.CreateCell(index++).SetCellValue(item.SYS_ChuangJianShiJian.ToString("yyyy-MM-dd"));
                    for (int contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 企业第三方机构车辆关系

        public ServiceResult<QueryResult> EnterpriseVehicleRelationshipList(QueryData queryData)
        {

            try
            {
                var returnData = new GetVehicleBindingDto();
                VehicleBindingDto searchDto =
                    JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //车辆基础档案列表
                Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                //组织表
                Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;


                Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                    t => t.SYS_XiTongZhuangTai == sysZhengChang;
                if (!string.IsNullOrEmpty(searchDto.ServiceProviderCode))
                {
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ServiceProviderCode == searchDto.ServiceProviderCode);
                }

                if (!string.IsNullOrEmpty(searchDto.EnterpriseCode))
                {
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.EnterpriseCode == searchDto.EnterpriseCode);
                }

                if (!string.IsNullOrEmpty(searchDto.LicensePlateNumber))
                {
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.LicensePlateNumber == searchDto.LicensePlateNumber);
                }

                //绑定车辆
                cheliangexp = cheliangexp.And(x => x.FuWuShangOrgCode != null && x.FuWuShangOrgCode != "");
                var query = from car in _cheLiangRepository.GetQuery(cheliangexp)
                            join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                                on car.YeHuOrgCode equals org.OrgCode
                            join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                                on car.FuWuShangOrgCode equals fws.OrgCode
                            join vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                                on car.ChePaiHao equals vpb.LicensePlateNumber
                            where car.ChePaiYanSe == vpb.LicensePlateColor
                            where string.IsNullOrEmpty(searchDto.EnterpriseName) ||
                                  org.OrgName.Contains(searchDto.EnterpriseName)
                            select new VehicleBindingDto
                            {
                                LicensePlateNumber = car.ChePaiHao,
                                LicensePlateColor = car.ChePaiYanSe,
                                VehicleType = car.CheLiangZhongLei,
                                XiaQuShi = car.XiaQuShi,
                                XiaQuXian = car.XiaQuXian,
                                XiaQuSheng = car.XiaQuSheng,
                                SYS_ChuangJianShiJian = car.SYS_ChuangJianShiJian,
                                ServiceProviderName = fws.OrgName,
                                EnterpriseName = org.OrgName
                            };
                returnData.Count = query.Count();
                //分页
                returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                var result = new QueryResult { totalcount = query.Count(), items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业车辆绑定出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion

        #region 组织监控关系导出

        public ServiceResult<ExportResponseInfoDto> ExportPartnershipBinding(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetPartnershipBindingList(queryData, userInfo).List;
                var tableTitle = "组织监控关系" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = PartnershipBindingExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出组织监控关系出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出组织监控关系出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? PartnershipBindingExcelUpload(List<PartnershipBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "组织监控关系";
            string[] cellTitleArry = { "企业名称", "企业代码", "第三方机构名称", "第三方机构代码", "审核状态", "创建时间" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.EnterpriseName);
                    row.CreateCell(index++).SetCellValue(item.EnterpriseCode);
                    row.CreateCell(index++).SetCellValue(item.ServiceProviderName);
                    row.CreateCell(index++).SetCellValue(item.ServiceProviderCode);
                    row.CreateCell(index++).SetCellValue(typeof(ExportCooperationStatus).GetEnumName(item.ZhuangTai));
                    row.CreateCell(index++).SetCellValue(item.SYS_ChuangJianShiJian != null
                        ? item.SYS_ChuangJianShiJian.Value.ToString("yyyy-MM-dd")
                        : string.Empty);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 车辆委托监控导出

        public ServiceResult<ExportResponseInfoDto> ExportVehicleBindingRelationships(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = VehicleBindingRelationshipsList(queryData, userInfo).List;
                var tableTitle = "车辆委托监控" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = VehicleBRelationExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆委托监控出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆委托监控出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }
        private GetVehicleBindingDto VehicleBindingRelationshipsList(QueryData queryData,
          UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetVehicleBindingDto();
            VehicleBindingDto searchDto =
                JsonConvert.DeserializeObject<VehicleBindingDto>(JsonConvert.SerializeObject(queryData.data));
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

            //车辆基础档案列表
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            //组织表
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<VehiclePartnershipBinding, bool>> vehiclePartnershipBinding =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            switch (userInfoDto.OrganizationType)
            {
                case (int)OrganizationType.企业:
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.EnterpriseCode == userInfoDto.OrganizationCode);
                    break;
                case (int)OrganizationType.本地服务商:
                    vehiclePartnershipBinding =
                        vehiclePartnershipBinding.And(x => x.ServiceProviderCode == userInfoDto.OrganizationCode);
                    break;
            }

            //审核状态
            if (searchDto.ZhuangTai != null)
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(x => x.ZhuangTai == searchDto.ZhuangTai);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateNumber))
            {
                searchDto.LicensePlateNumber = Regex.Replace(searchDto.LicensePlateNumber, @"\s", "");
                vehiclePartnershipBinding = vehiclePartnershipBinding.And(p =>
                    p.LicensePlateNumber.Contains(searchDto.LicensePlateNumber.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(searchDto.LicensePlateColor))
            {
                vehiclePartnershipBinding =
                    vehiclePartnershipBinding.And(p => p.LicensePlateColor == searchDto.LicensePlateColor);
            }

            var query = from vpb in _vehiclePartnershipBinding.GetQuery(vehiclePartnershipBinding)
                        join car in _cheLiangRepository.GetQuery(cheliangexp)
                            on vpb.LicensePlateNumber equals car.ChePaiHao
                        where vpb.LicensePlateColor == car.ChePaiYanSe
                        join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                            on car.YeHuOrgCode equals org.OrgCode
                        join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                            on vpb.ServiceProviderCode equals fws.OrgCode
                        select new VehicleBindingDto
                        {
                            LicensePlateNumber = vpb.LicensePlateNumber,
                            LicensePlateColor = vpb.LicensePlateColor,
                            VehicleType = car.CheLiangZhongLei,
                            ServiceProviderName = fws.OrgName,
                            EnterpriseName = org.OrgName,
                            ZhuangTai = vpb.ZhuangTai,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            XiaQuSheng = car.XiaQuSheng,
                            EnterpriseCode = vpb.EnterpriseCode,
                            ServiceProviderCode = vpb.ServiceProviderCode,
                            SYS_ChuangJianShiJian = vpb.SYS_ChuangJianShiJian
                        };
            returnData.Count =0;
            returnData.List = query.OrderByDescending(x => x.SYS_ChuangJianShiJian).ToList();
            return returnData;
        }
        private static Guid? VehicleBRelationExcelUpload(List<VehicleBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆委托监控";
            string[] cellTitleArry = { "车牌号", "车牌颜色", "车辆种类", "所属区域", "企业名称", "第三方机构名称", "审核状态", "创建时间" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.LicensePlateNumber);
                    row.CreateCell(index++).SetCellValue(item.LicensePlateColor);
                    //车辆种类
                    var chelaingzhongleiStr = string.Empty;
                    if (item.VehicleType.HasValue)
                    {
                        chelaingzhongleiStr = typeof(CheLiangZhongLei).GetEnumName(item.VehicleType);
                        
                    }
                    row.CreateCell(index++).SetCellValue(chelaingzhongleiStr);
                    row.CreateCell(index++)
                        .SetCellValue(item.XiaQuSheng + "  " + item.XiaQuShi + "  " + item.XiaQuXian);
                    row.CreateCell(index++).SetCellValue(item.EnterpriseName);
                    row.CreateCell(index++).SetCellValue(item.ServiceProviderName);
                    row.CreateCell(index++).SetCellValue(typeof(VehicleCooperationStatus).GetEnumName(item.ZhuangTai));
                    row.CreateCell(index++).SetCellValue(item.SYS_ChuangJianShiJian != null
                        ? item.SYS_ChuangJianShiJian.Value.ToString("yyyy-MM-dd")
                        : string.Empty);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 车辆未绑定列表导出

        public ServiceResult<ExportResponseInfoDto> ExportVehicleNotBound(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetVehicleNotBoundList(queryData, userInfo).List;
                var tableTitle = "车辆未绑定" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = VehicleNotBoundExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆未绑定出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆未绑定出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? VehicleNotBoundExcelUpload(List<VehicleBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆未绑定";
            string[] cellTitleArry = { "车牌号", "车牌颜色", "车辆种类", "所属区域" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.LicensePlateNumber);
                    row.CreateCell(index++).SetCellValue(item.LicensePlateColor);
                    row.CreateCell(index++).SetCellValue(item.VehicleType != null
                        ? typeof(CheLiangZhongLei).GetEnumName(item.VehicleType)
                        : typeof(CheLiangZhongLei).GetEnumName(string.Empty));
                    row.CreateCell(index++)
                        .SetCellValue(item.XiaQuSheng + "  " + item.XiaQuShi + "  " + item.XiaQuXian);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 车辆绑定申请列表导出

        public ServiceResult<ExportResponseInfoDto> ExportVehicleApplyBinding(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetVehicleApplyBindingList(queryData, userInfo).List;
                var tableTitle = "车辆绑定申请列表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = GetVehicleApplyBindingExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆绑定申请列表出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆绑定申请列表出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? GetVehicleApplyBindingExcelUpload(List<VehicleBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆绑定申请列表";
            string[] cellTitleArry = { "车牌号", "车牌颜色", "车辆种类", "所属区域" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.LicensePlateNumber);
                    row.CreateCell(index++).SetCellValue(item.LicensePlateColor);
                    row.CreateCell(index++).SetCellValue(item.VehicleType != null
                        ? typeof(CheLiangZhongLei).GetEnumName(item.VehicleType)
                        : typeof(CheLiangZhongLei).GetEnumName(string.Empty));
                    row.CreateCell(index++)
                        .SetCellValue(item.XiaQuSheng + "  " + item.XiaQuShi + "  " + item.XiaQuXian);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 车辆绑定列表导出

        public ServiceResult<ExportResponseInfoDto> ExportVehicleBinding(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetVehicleBindingList(queryData, userInfo).List;
                var tableTitle = "车辆绑定列表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = GetVehicleBindingExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆绑定列表出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆绑定列表出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? GetVehicleBindingExcelUpload(List<VehicleBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆绑定列表";
            string[] cellTitleArry = { "车牌号", "车牌颜色", "车辆种类", "所属区域" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.LicensePlateNumber);
                    row.CreateCell(index++).SetCellValue(item.LicensePlateColor);
                    row.CreateCell(index++).SetCellValue(item.VehicleType != null
                        ? typeof(CheLiangZhongLei).GetEnumName(item.VehicleType)
                        : typeof(CheLiangZhongLei).GetEnumName(string.Empty));
                    row.CreateCell(index++)
                        .SetCellValue(item.XiaQuSheng + "  " + item.XiaQuShi + "  " + item.XiaQuXian);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 车辆解绑申请列表导出

        public ServiceResult<ExportResponseInfoDto> ExportVehicleUApplication(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetVehicleUApplicationList(queryData, userInfo).List;
                var tableTitle = "车辆解绑申请列表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = GetVehicleUApplicationExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆解绑申请列表出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆解绑申请列表出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? GetVehicleUApplicationExcelUpload(List<VehicleBindingDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆解绑申请列表";
            string[] cellTitleArry = { "车牌号", "车牌颜色", "车辆种类", "所属区域" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.LicensePlateNumber);
                    row.CreateCell(index++).SetCellValue(item.LicensePlateColor);
                    row.CreateCell(index++).SetCellValue(item.VehicleType != null
                        ? typeof(CheLiangZhongLei).GetEnumName(item.VehicleType)
                        : typeof(CheLiangZhongLei).GetEnumName(string.Empty));
                    row.CreateCell(index++)
                        .SetCellValue(item.XiaQuSheng + "  " + item.XiaQuShi + "  " + item.XiaQuXian);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 批量导出企业附件

        public ServiceResult<ExportResponseInfoDto> BatchDownload(string material)
        {
            var materialList = _enterpriseRegisterInfoRepository.GetQuery(x =>
                x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.OrgCode == material).FirstOrDefault();
            if (materialList == null)
            {
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "该企业,材料信息不存在", StatusCode = 2 };
            }

            var fileName = material;
            var thirdPartyEntity = _yeHuRepository.GetQuery(x =>
                x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常
                && x.OrgCode == material).FirstOrDefault();
            if (thirdPartyEntity != null)
            {
                fileName = thirdPartyEntity.OrgName;
            }

            try
            {
                var fileAddress = "D:/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "企业附件材料/";
                Directory.CreateDirectory(fileAddress);
                Mapper.Initialize(x => { x.CreateMap<EnterpriseRegisterInfo, EnterpriseAccessories>(); });
                //开始搬运
                var materialEntity = Mapper.Map<EnterpriseRegisterInfo, EnterpriseAccessories>(materialList);
                var propertyInfos = typeof(EnterpriseAccessories).GetProperties();
                //第一步初始化
                var i = 0;
                foreach (var p in propertyInfos)
                {
                    var materialValue = p.GetValue(materialEntity).ToString();
                    if (!string.IsNullOrEmpty(materialValue))
                    {
                        i++;
                        var id = Guid.Parse(materialValue);
                        var array = FileAgentUtility.GetFileData(id);
                        var information = FileAgentUtility.GetFileById(id);
                        var path = fileAddress + information.DisplayName + i + "." + information.FileExtension;
                        var fs = new FileStream(path, FileMode.OpenOrCreate);
                        //将byte数组写入文件中
                        fs.Write(array, 0, array.Length);
                        fs.Close();
                    }
                }

                var materialName = "";
                var zipAddress = @"D:\" + material + "附件.zip";
                var fileId = CompressDirectory(fileAddress, "", 6, true, out materialName, zipAddress, fileName);
                return new ServiceResult<ExportResponseInfoDto> { Data = new ExportResponseInfoDto { FileId = fileId } };
            }
            catch (Exception e)
            {
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "该企业,材料信息不存在", StatusCode = 2 };
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dirPath">要打包的文件夹</param>
        /// <param name="GzipFileName">目标文件名</param>
        /// <param name="CompressionLevel">压缩品质级别（0~9）</param>
        /// <param name="deleteDir">是否删除原文件夹</param>
        /// <param name="GzipPath">压缩文件的路径</param>
        ///  <param name="zipAddress">zip地址</param>
        ///  <param name="material">文件名称</param>
        public static string CompressDirectory(string dirPath, string GzipFileName, int CompressionLevel,
            bool deleteDir, out string GzipPath, string zipAddress, string material)
        {
            try
            {
                //压缩文件为空时默认与压缩文件夹同一级目录
                if (GzipFileName == string.Empty)
                {
                    GzipFileName = dirPath + "材料附件.zip";
                }

                var file = File.Create(GzipFileName);
                var zipBool = @"企业附件材料\材料附件.zip";
                using (var zipoutputstream = new ZipOutputStream(file))
                {
                    //设置压缩文件级别
                    zipoutputstream.SetLevel(CompressionLevel);
                    var fileList = GetAllFies(dirPath);
                    foreach (var item in fileList)
                    {
                        if (item.Key.Contains(zipBool)) continue;
                        //将文件数据读到流里面
                        var fs = File.OpenRead(item.Key.ToString());
                        var buffer = new byte[fs.Length];
                        //从流里读出来赋值给缓冲区
                        fs.Read(buffer, 0, buffer.Length);
                        var entry = new ZipEntry(item.Key.Substring(dirPath.Length + 1))
                        {
                            DateTime = item.Value,
                            Size = fs.Length
                        };
                        fs.Close();
                        zipoutputstream.PutNextEntry(entry);
                        zipoutputstream.Write(buffer, 0, buffer.Length);
                    }

                    zipoutputstream.Finish();
                    zipoutputstream.Close();
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("压缩失败" + ex.Message, ex);
            }

            GzipPath = GzipFileName;
            //上传
            var fileDto = new FileDTO
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = material + "材料附件." + "zip",
                FileExtension = "zip",
                DisplayName = material + "材料附件." + "zip",
                Remark = "",
                Data = File.ReadAllBytes(GzipFileName)
            };
            var fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult == null) return null;
            var fileId = fileDtoResult.FileId;
            Directory.Delete(dirPath, true);
            if (File.Exists(zipAddress))
            {
                File.Delete(zipAddress);
            }

            return fileId.ToString();
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, DateTime> GetAllFies(string dir)
        {
            var filesList = new Dictionary<string, DateTime>();
            var fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            GetAllDirFiles(fileDire, filesList);
            GetAllDirsFiles(fileDire.GetDirectories(), filesList);
            return filesList;
        }

        /// <summary>
        /// 获取一个文件夹下的文件
        /// </summary>
        /// <param name="dir">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>
        private static void GetAllDirFiles(DirectoryInfo dir, Dictionary<string, DateTime> filesList)
        {
            foreach (var file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        /// <summary>
        /// 获取一个文件夹下的所有文件夹里的文件
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="filesList"></param>
        private static void GetAllDirsFiles(DirectoryInfo[] dirs, Dictionary<string, DateTime> filesList)
        {
            foreach (var dir in dirs)
            {
                foreach (var file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }

                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }

        #endregion

        public ServiceResult<QueryResult> TelephoneLanguageStatistics(QueryData queryData)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetTelephoneLanguageStatistics(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询电话语言统计列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetTelephoneLanguageStatisticsDto GetTelephoneLanguageStatistics(QueryData queryData,
            UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetTelephoneLanguageStatisticsDto();
            TelephoneLanguageStatisticsDto searchDto =
                JsonConvert.DeserializeObject<TelephoneLanguageStatisticsDto>(
                    JsonConvert.SerializeObject(queryData.data));
            var whereSql = string.Empty;
            var whereSqlTheFirst = string.Empty;
            if (userInfoDto.OrganizationType == (int)OrganizationType.市政府 || userInfoDto.OrganizationType == (int)OrganizationType.县政府)
            {
                switch (userInfoDto.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        whereSqlTheFirst += $"  AND XiaQuShi like '%{userInfoDto.OrganCity.Trim()}%'";
                        if (!string.IsNullOrWhiteSpace(searchDto?.XiaQuXian))
                        {
                            whereSqlTheFirst += $"  AND XiaQuXian like '%{searchDto.XiaQuXian.Trim()}%'";
                        }
                        break;
                    case (int)OrganizationType.县政府:
                        whereSqlTheFirst += $"  AND XiaQuXian like '%{userInfoDto.OrganDistrict.Trim()}%'";
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(searchDto?.YeHuMingCheng))
            {
                whereSql += $"  AND YeHuMingCheng like '%{searchDto.YeHuMingCheng.Trim()}%'";
            }

            var timeRange = string.Empty;
            var timeRangeSecond = string.Empty;
            if (searchDto.StartTime.HasValue)
            {
                timeRange += $"  AND CallStartTime >= '{searchDto.StartTime}'";
                timeRangeSecond += $"  AND SendTime >= '{searchDto.StartTime}'";
            }
            if (searchDto.EndTime.HasValue)
            {
                searchDto.EndTime = searchDto.EndTime.Value.AddDays(1);
                timeRange += $"  AND CallStartTime < '{searchDto.EndTime}'";
                timeRangeSecond += $"  AND SendTime < '{searchDto.EndTime}'";
            }
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DC_DLYSZHGLPT"].ConnectionString))
            {
                var sql = $@"SELECT *
FROM
(
    SELECT tem1.YeHuMingCheng,
           NORMAL_CLEARING,
           POWER_OFF,
           OUT_OF_SERVICE,
           HOLD,
           NOT_CONVENIENT,
           DOES_NOT_EXIST,
           NOT_REACHABLE,
           NOT_ANSWER,
           BUSY,
           DEFAULTING,
           NO_USER_RESPONSE,
           NO_ANSWER,
           USER_BUSY,
           TRUNK_LINE_FAULT,
           FACILITY_FAULT,
           AI_ROBOT_FAULT,
           BUSINESS_RESTRICT,
           UNKNOWN_ERROR,
           VoiceCombined,
           TextCombined,
           ROW_NUMBER() OVER (ORDER BY VoiceCombined DESC) AS 'RowNumber'
    FROM
    (
            SELECT YeHuMingCheng,
               SUM(tcrd.NORMAL_CLEARING) NORMAL_CLEARING,
               SUM(tcrd.POWER_OFF) POWER_OFF,
               SUM(tcrd.OUT_OF_SERVICE) OUT_OF_SERVICE,
               SUM(tcrd.HOLD) HOLD,
               SUM(tcrd.NOT_CONVENIENT) NOT_CONVENIENT,
               SUM(tcrd.DOES_NOT_EXIST) DOES_NOT_EXIST,
               SUM(tcrd.NOT_REACHABLE) NOT_REACHABLE,
               SUM(tcrd.NOT_ANSWER) NOT_ANSWER,
               SUM(BUSY) BUSY,
               SUM(DEFAULTING) DEFAULTING,
               SUM(NO_USER_RESPONSE) NO_USER_RESPONSE,
               SUM(NO_ANSWER) NO_ANSWER,
               SUM(USER_BUSY) USER_BUSY,
               SUM(TRUNK_LINE_FAULT) TRUNK_LINE_FAULT,
               SUM(FACILITY_FAULT) FACILITY_FAULT,
               SUM(AI_ROBOT_FAULT) AI_ROBOT_FAULT,
               SUM(BUSINESS_RESTRICT) BUSINESS_RESTRICT,
               SUM(UNKNOWN_ERROR) UNKNOWN_ERROR
        FROM
        (
            SELECT YeHuMingCheng,
                   CustomerPhone
            FROM T_CallEnterpriseRecord
            WHERE 1 = 1
                  AND CallStatus = '0'
				  AND SYS_XiTongZhuangTai=0
{whereSqlTheFirst} {timeRange}
            GROUP BY YeHuMingCheng,
                     CustomerPhone
        ) tcer
            JOIN
            (
                SELECT customerPhone,
                       SUM(   CASE responseCode
                                  WHEN '200' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NORMAL_CLEARING,   --呼叫正常结束
                       SUM(   CASE responseCode
                                  WHEN '37' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS POWER_OFF,         --关机
                       SUM(   CASE responseCode
                                  WHEN '39' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS OUT_OF_SERVICE,    --停机
                       SUM(   CASE responseCode
                                  WHEN '40' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS HOLD,              --通话保持
                       SUM(   CASE responseCode
                                  WHEN '46' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NOT_CONVENIENT,    --拒接
                       SUM(   CASE responseCode
                                  WHEN '36' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS DOES_NOT_EXIST,    --空号
                       SUM(   CASE responseCode
                                  WHEN '38' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NOT_REACHABLE,     --无法接通
                       SUM(   CASE responseCode
                                  WHEN '43' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NOT_ANSWER,        --无人接听
                       SUM(   CASE responseCode
                                  WHEN '41' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS BUSY,              --呼叫正忙
                       SUM(   CASE responseCode
                                  WHEN '56' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS DEFAULTING,        --⽋费
                       SUM(   CASE responseCode
                                  WHEN '408' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NO_USER_RESPONSE,  --中继线路⽆响应
                       SUM(   CASE responseCode
                                  WHEN '480' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS NO_ANSWER,         --超时⽆⼈应答
                       SUM(   CASE responseCode
                                  WHEN '486' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS USER_BUSY,         --中继线路忙
                       SUM(   CASE responseCode
                                  WHEN '4XX' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS TRUNK_LINE_FAULT,  --主叫线路故障
                       SUM(   CASE responseCode
                                  WHEN '5XX' THEN
                                      1
                                  WHEN '6XX' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS FACILITY_FAULT,    --主叫设备故障
                       SUM(   CASE responseCode
                                  WHEN '9XX' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS AI_ROBOT_FAULT,    --机器⼈故障
                       SUM(   CASE responseCode
                                  WHEN '16XX' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS BUSINESS_RESTRICT, --业务限制
                       SUM(   CASE responseCode
                                  WHEN '500' THEN
                                      1
                                  ELSE
                                      0
                              END
                          ) AS UNKNOWN_ERROR      --其他故障
                FROM T_CallRecordDetail
                GROUP BY customerPhone
            ) tcrd
                ON tcrd.customerPhone = tcer.CustomerPhone
        WHERE 1 = 1
        GROUP BY YeHuMingCheng
    ) tem1
        LEFT JOIN
        (
            SELECT COUNT(*) VoiceCombined,
                   YeHuMingCheng
            FROM T_CallEnterpriseRecord
            WHERE SYS_XiTongZhuangTai = 0
                 AND CallStatus='0'
             {timeRange}
            GROUP BY YeHuMingCheng
        ) tem2
            ON tem1.YeHuMingCheng = tem2.YeHuMingCheng
        LEFT JOIN
        (
          SELECT YeHuMingCheng,SUM(TextCombined) TextCombined
        FROM
        (SELECT YeHuMingCheng,
                   CustomerPhone
            FROM T_CallEnterpriseRecord
            WHERE 1 = 1
                  AND CallStatus = '0'
				  AND SYS_XiTongZhuangTai=0
{whereSqlTheFirst} {timeRange}
            GROUP BY YeHuMingCheng,
                     CustomerPhone
        ) tcer
            JOIN
            (
                SELECT  count (*) AS TextCombined,
			     CustomerPhone
                FROM T_CallEnterpriseMsgRecord
				  WHERE SYS_XiTongZhuangTai = 0
	        {timeRangeSecond}
                GROUP BY CustomerPhone
            ) tcrd
                ON tcrd.CustomerPhone = tcer.CustomerPhone
        WHERE 1 = 1
        GROUP BY YeHuMingCheng
        ) tem3
            ON tem1.YeHuMingCheng = tem3.YeHuMingCheng
) t  
where 1=1  {whereSql} ";
                var rowStart = (queryData.page - 1) * queryData.rows + 1;
                var rowEnd = rowStart + queryData.rows - 1;
                var queryCount = $@"select count(*) from ({sql} ) countT";
                var count = conn.ExecuteScalar<int>(queryCount);
                sql += $" and t.rowNumber between {rowStart} and {rowEnd}    ORDER BY VoiceCombined DESC";
                var telephoneLanguageList = conn.Query<TelephoneLanguageStatisticsDto>(sql).ToList();
                returnData.Count = count;
                returnData.List = telephoneLanguageList;
            }
            return returnData;
        }


        public ServiceResult<ExportResponseInfoDto> ExportTelephoneLanguageStatistics(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetTelephoneLanguageStatistics(queryData, userInfo).List;
                var tableTitle = "电话语言统计" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count > 0)
                {
                    try
                    {
                        var fileId = string.Empty;
                        var fileUploadId = GetTelephoneLanguageExcelUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            fileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                        { Data = new ExportResponseInfoDto { FileId = fileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出电话语言统计出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出电话语言统计列表出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? GetTelephoneLanguageExcelUpload(List<TelephoneLanguageStatisticsDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "电话语言统计";
            string[] cellTitleArry =
            {
                "企业名称", "呼叫完成", "关机", "停机", "拒听"
                , "空号", "无人接通", "无人接听", "呼叫正忙", "欠费", "中继线路无响应", "超时无人应答"
                , "中继线路忙", "主叫线路障碍", "主叫设备故障", "机器人故障","业务限制"
                ,"其他故障","短信发送数","语音合计"
            };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow)sheet.CreateRow(0);

                #region 单元格样式

                //标题样式
                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = workbook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = workbook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                //内容样式
                ICellStyle contentStyle_Center = workbook.CreateCellStyle();
                contentStyle_Center.Alignment = HorizontalAlignment.Center;
                contentStyle.SetFont(contentFont);

                #endregion

                //合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 20));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

                for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                {
                    row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                    //附加表头样式
                    row.Cells[cell_index].CellStyle = cellStyle;
                }

                //内容
                var loop_list = list.Skip(sheet_index * (sheetRowCount - 2)).Take(sheetRowCount - 2).ToList();
                for (int content_index = 0; content_index < loop_list.Count; content_index++)
                {
                    var item = loop_list[content_index];
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;
                    row.CreateCell(index++).SetCellValue(item.YeHuMingCheng);
                    row.CreateCell(index++).SetCellValue(item.NORMAL_CLEARING);
                    row.CreateCell(index++).SetCellValue(item.POWER_OFF);
                    row.CreateCell(index++).SetCellValue(item.OUT_OF_SERVICE);
                    row.CreateCell(index++).SetCellValue(item.NOT_CONVENIENT);
                    row.CreateCell(index++).SetCellValue(item.DOES_NOT_EXIST);
                    row.CreateCell(index++).SetCellValue(item.NOT_REACHABLE);
                    row.CreateCell(index++).SetCellValue(item.NOT_ANSWER);
                    row.CreateCell(index++).SetCellValue(item.BUSY);
                    row.CreateCell(index++).SetCellValue(item.DEFAULTING);
                    row.CreateCell(index++).SetCellValue(item.NO_USER_RESPONSE);
                    row.CreateCell(index++).SetCellValue(item.NO_ANSWER);
                    row.CreateCell(index++).SetCellValue(item.USER_BUSY);
                    row.CreateCell(index++).SetCellValue(item.TRUNK_LINE_FAULT);
                    row.CreateCell(index++).SetCellValue(item.FACILITY_FAULT);
                    row.CreateCell(index++).SetCellValue(item.AI_ROBOT_FAULT);
                    row.CreateCell(index++).SetCellValue(item.BUSINESS_RESTRICT);
                    row.CreateCell(index++).SetCellValue(item.UNKNOWN_ERROR);
                    row.CreateCell(index++).SetCellValue(item.VoiceCombined);
                    row.CreateCell(index++).SetCellValue(item.TextCombined);
                    for (var contInx = 0; contInx < index; contInx++)
                    {
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
                }
            }

            //上传
            string extension = "xls";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "",
                BusinessType = "",
                BusinessId = "",
                FileName = fileName + "." + extension,
                FileExtension = extension,
                DisplayName = fileName,
                Remark = ""
            };
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                fileDto.Data = ms.ToArray();
            }

            FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
            if (fileDtoResult != null)
            {
                return fileDtoResult.FileId;
            }
            else
            {
                return null;
            }
        }


        public ServiceResult<QueryResult> RiskDialingFailedList(QueryData queryData)
        {

            try
            {
                var userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2 };
                }

                var returnData = GetRiskDialingFailedList(queryData, userInfoDto);
                var result = new QueryResult { totalcount = returnData.Count, items = returnData.List };
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询电话语言统计列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private GetRiskDialingFailedDtoDto GetRiskDialingFailedList(QueryData queryData,
        UserInfoDtoNew userInfoDto)
        {
            var returnData = new GetRiskDialingFailedDtoDto();
            GetRiskDialingFailedDto searchDto =
                JsonConvert.DeserializeObject<GetRiskDialingFailedDto>(
                    JsonConvert.SerializeObject(queryData.data));
            var whereSql = string.Empty;
            var whereSqlTheFirst = string.Empty;
            if (userInfoDto.OrganizationType == (int)OrganizationType.市政府 || userInfoDto.OrganizationType == (int)OrganizationType.县政府)
            {
                switch (userInfoDto.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        whereSqlTheFirst += $"  AND XiaQuShi like '%{userInfoDto.OrganCity.Trim()}%'";
                        break;
                    case (int)OrganizationType.县政府:
                        whereSqlTheFirst += $"  AND XiaQuXian like '%{userInfoDto.OrganDistrict.Trim()}%'";
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(searchDto?.YeHuMingCheng))
            {
                whereSql += $"  AND YeHuMingCheng like '%{searchDto.YeHuMingCheng.Trim()}%'";
            }
            var timeRange = string.Empty;
            if (searchDto.RiskStartTime.HasValue)
            {
                timeRange += $"  AND StartTime >= '{searchDto.RiskStartTime}'";
            }
            if (searchDto.RiskEndTime.HasValue)
            {
                searchDto.RiskEndTime = searchDto.RiskEndTime.Value.AddDays(1);
                timeRange += $"  AND StartTime < '{searchDto.RiskEndTime}'";
            }
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DC_DLYSZHGLPT"].ConnectionString))
            {
                var sql = $@"
SELECT *FROM(
SELECT *, ROW_NUMBER() OVER (ORDER BY StartTime DESC) AS 'RowNumber'
FROM  T_CallEnterpriseRecord
WHERE SYS_XiTongZhuangTai=0
AND  CallStatus='1'  {whereSql} {timeRange}  {whereSqlTheFirst} )t
where 1=1 ";
                var rowStart = (queryData.page - 1) * queryData.rows + 1;
                var rowEnd = rowStart + queryData.rows - 1;
                var queryCount = $@"select count(*) from ({sql} ) countT";
                var count = conn.ExecuteScalar<int>(queryCount);
                sql += $" and t.rowNumber between {rowStart} and {rowEnd}    ORDER BY StartTime DESC";
                var telephoneLanguageList = conn.Query<GetRiskDialingFailedDto>(sql).ToList();
                returnData.Count = count;
                returnData.List = telephoneLanguageList;
            }
            return returnData;
        }



        /// <summary>
        /// 属于第三方机构监测的车
        /// </summary>
        /// <returns></returns>
        public ServiceResult<List<ThirdPartyVehicles>> VehiclesMonitoredThirdParty()
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<List<ThirdPartyVehicles>>
                        {StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。"};
                }
                var approvalModel =
                    (from cl in _cheLiangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.FuWuShangOrgCode == userInfo.OrganizationCode)
                        select new
                            ThirdPartyVehicles
                            {
                                ChePaiHao = cl.ChePaiHao,
                                ChePaiYanSe = cl.ChePaiYanSe
                     }).ToList();
                return new ServiceResult<List<ThirdPartyVehicles>> {Data = approvalModel};
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取属于第三方机构监测的车出错" + ex.Message, ex);
                return new ServiceResult<List<ThirdPartyVehicles>> {StatusCode = 2, ErrorMessage = "查询出错"};
            }
        }

      

        public override void Dispose()
        {
            _yeHuRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
            _qiYeFuWuShangGuanLianXinXiRepository.Dispose();
            _fuWuShangRepository.Dispose();
            _anQuanGuanLiRenYuan.Dispose();
            _partnershipBindingTable.Dispose();
            _vehiclePartnershipBinding.Dispose();
            _cheLiangRepository.Dispose();
            _monitorPersonInfo.Dispose();
            _enterpriseRegisterInfoRepository.Dispose();
            _materialListOfServiceProviderRepository.Dispose();
        }
    }
}
