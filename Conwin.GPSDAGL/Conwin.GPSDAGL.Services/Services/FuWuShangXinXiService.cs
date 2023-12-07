using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Conwin.Framework.Log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Conwin.Framework.FileAgent;
using System.Configuration;
using System.IO;
using Conwin.FileModule.ServiceAgent;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Conwin.Framework.Redis;
using System.Net.Mail;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using AutoMapper;
using System.Reflection;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class FuWuShangXinXiService : ApiServiceBase, IFuWuShangXinXiService
    {
        #region 构造函数

        private readonly string sysId = string.Empty;
        private readonly IFuWuShangRepository _fuWuShangRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly IMaterialListOfServiceProviderRepository _materialListOfServiceProviderRepository;

        public FuWuShangXinXiService(
            IFuWuShangRepository fuWuShangRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            IMaterialListOfServiceProviderRepository materialListOfServiceProviderRepository,
            IBussinessLogger bussinessLogger) : base(bussinessLogger)
        {
            _fuWuShangRepository = fuWuShangRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _materialListOfServiceProviderRepository = materialListOfServiceProviderRepository;
            sysId = base.SysId;
        }

        #endregion

        #region 新增

        public ServiceResult<bool> Create(string reqid, FuWuShangExDto model)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                // 组织基本信息Id
                var baseId = Guid.NewGuid();
                // 组织信息Id
                var orgId = Guid.NewGuid();
                //组织基本信息
                OrgBaseInfo orgBaseInfo = new OrgBaseInfo
                {
                    Id = baseId,
                    OrgType = (int) OrganizationType.本地服务商,
                    OrgShortName = model.OrgShortName,
                    OrgName = model.OrgName,
                    ParentOrgId = Guid.Parse(userInfo.ExtOrganizationId),
                    JingYingFanWei = model.JingYingQuYu,
                    XiaQuSheng = userInfo.OrganProvince,
                    XiaQuShi = userInfo.OrganCity,
                    XiaQuXian = userInfo.OrganDistrict,
                    DiZhi = model.GongSiDiZhi,
                    Remark = model.BeiZhu,
                    ZhuangTai = (int) ZhuangTaiEnum.正常营业,
                    ChuangJianRenOrgCode = userInfo.OrganizationCode,
                    SYS_ChuangJianRenID = userInfo.Id,
                    SYS_ChuangJianRen = userInfo.UserName,
                    SYS_ChuangJianShiJian = DateTime.Now,
                    SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常,
                };
                //服务商信息
                FuWuShang fuWuShang = new FuWuShang
                {
                    Id = orgId,
                    BaseId = baseId.ToString(),
                    //组织编号
                    OrgType = (int) OrganizationType.本地服务商,
                    OrgShortName = model.OrgShortName,
                    OrgName = model.OrgName,
                    YingYeZhiZhaoHao = model.YingYeZhiZhaoHao,
                    TongYiSheHuiXinYongDaiMa = model.TongYiSheHuiXinYongDaiMa,
                    YouBian = model.YouBian,
                    SYS_ChuangJianRenID = userInfo.Id,
                    SYS_ChuangJianRen = userInfo.UserName,
                    SYS_ChuangJianShiJian = DateTime.Now,
                    SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常
                };

                var validRsult = Vaild(fuWuShang);
                if (!validRsult.Data)
                {
                    return validRsult;
                }
                else
                {
                    //校验机构名称是否重复
                    var isExitModel = _orgBaseInfoRepository.Count(x =>
                        x.OrgName == model.OrgName && x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常);
                    if (isExitModel > 0)
                    {
                        return new ServiceResult<bool>()
                        {
                            Data = false, StatusCode = 2,
                            ErrorMessage = string.Format("已经存在名称为{0}的服务商档案，请使用其它名称", model.OrgName)
                        };
                    }

                    using (var uow = new UnitOfWork())
                    {
                        uow.BeginTransaction();

                        var getSNoResponse = GetInvokeRequest("00000020013", "1.0", new
                        {
                            SysId = sysId,
                            Module = "00330021",
                            Type = 2
                        });
                        if (getSNoResponse.publicresponse.statuscode != 0)
                        {
                            return new ServiceResult<bool>()
                                {Data = false, StatusCode = 2, ErrorMessage = getSNoResponse.publicresponse.message};
                        }
                        else
                        {
                            string OrgCode = "fws" + getSNoResponse.body.SNo.ToString().PadLeft(4, '0');

                            // 组织基本信息
                            orgBaseInfo.OrgCode = OrgCode;
                            _orgBaseInfoRepository.Add(orgBaseInfo);

                            // 平台代理商信息
                            fuWuShang.OrgCode = OrgCode;
                            _fuWuShangRepository.Add(fuWuShang);

                            var addResult = uow.CommitTransaction() > 0;
                            if (addResult)
                            {
                                AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                                {
                                    ReqId = reqid,
                                    YeWuDuiXiangLeiXing = "企业GPS基础档案",
                                    YeWuDuiXiangZiLei = "服务商档案",
                                    YeWuDuiXiangID = orgBaseInfo.Id,
                                    YeWuDuiXiangBiaoZhi = model.OrgName,
                                    YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(model),
                                    YeWuLeiXing = "新增企业GPS基础档案",
                                    YeWuZiLei = "新增服务商档案档案",
                                    YeWuChangJingLeiXing = "基础档案业务",
                                    MoKuaiMingCheng = "企业GPS基础档案管理",
                                    XiTongMingCheng = "企业GPS基础档案系统",
                                    YingYongMingCheng = "服务商档案档案系统",
                                    YeWuGaiYaoXinXi = string.Format("新增档案：{0}", model.OrgName),
                                }, userInfo);
                                return new ServiceResult<bool>() {Data = true};
                            }
                            else
                            {
                                return new ServiceResult<bool>()
                                    {Data = false, StatusCode = 2, ErrorMessage = "新增服务商出错了"};
                            }
                        }
                    }
                }
            });
        }

        #endregion

        #region 查看

        public ServiceResult<FuWuShangExDto> Get(Guid id)
        {
            var result = new ServiceResult<FuWuShangExDto>();

            UserInfoDtoNew userInfo = GetUserInfo();
            int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;

            var orgInfo =
                (from org in _orgBaseInfoRepository.GetQuery(x => x.Id == id && x.SYS_XiTongZhuangTai == sysZhengChang)
                    join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                        on org.Id.ToString() equals fws.BaseId
                    select new FuWuShangExDto
                    {
                        Id = org.Id.ToString(),
                        OrgName = org.OrgName,
                        OrgShortName = org.OrgShortName,
                        YingYeZhiZhaoHao = fws.YingYeZhiZhaoHao,
                        TongYiSheHuiXinYongDaiMa = fws.TongYiSheHuiXinYongDaiMa,
                        JingYingQuYu = org.JingYingFanWei,
                        YouXiaoZhuangTai = org.ZhuangTai,
                        YouBian = fws.YouBian,
                        GongSiDiZhi = org.DiZhi,
                        BeiZhu = org.Remark,
                        XiaQuSheng = org.XiaQuSheng,
                        XiaQuShi = org.XiaQuShi,
                        XiaQuXian = org.XiaQuXian
                    }).FirstOrDefault();


            orgInfo.FuZheRen = "暂未添加负责人";
            orgInfo.FuZheRenDianHua = "暂未添加负责人";
            result.Data = orgInfo;
            return result;
        }

        #endregion

        public ServiceResult<ServiceProviderSetDto> GetProviderName(string providerName)
        {
            var result = new ServiceResult<ServiceProviderSetDto>();
            var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
            var thirdPartyInformation =
                (from org in _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                    join fws in _fuWuShangRepository.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgName == providerName)
                        on org.Id.ToString() equals fws.BaseId
                    join mlsop in _materialListOfServiceProviderRepository.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == sysZhengChang)
                        on fws.OrgCode equals mlsop.OrgCode
                    select new ServiceProviderSetDto
                    {
                        Id = fws.BaseId,
                        CompanyTypes = (int)mlsop.CompanyType,
                        SetOrganizationName = fws.OrgName,
                        SetUnifiedSocialCreditCode = fws.TongYiSheHuiXinYongDaiMa,
                        SetIndustrialACBLicenseId = mlsop.IndustrialACBLicenseId,
                        SetIndustrialACBLOTOfficeId = mlsop.IndustrialACBLOTOfficeId,
                        SetJingYingFanWei = org.JingYingFanWei,
                        SetContactName = fws.LianXiRenName,
                        SetContactIDNumber = mlsop.IDCard,
                        SetContactTelephone = fws.LianXiRenPhone,
                        SetLandline = mlsop.Landline,
                        SetMailbox = mlsop.Mailbox,
                        SetFilingApplicationFormId = mlsop.FilingApplicationFormId,
                        SetStandardTOServiceId = mlsop.StandardTOServiceId,
                        SetSiteMaterialsId = mlsop.SiteMaterialsId,
                        SetPhotosOBPremisesId = mlsop.PhotosOBPremisesId,
                        SetPersonnelPDMaterialsId = mlsop.PersonnelPDMaterialsId,
                        SetPersonnelSSCertificateId = mlsop.PersonnelSSCertificateId,
                        SetEquipmentIMaterialsId = mlsop.EquipmentIMaterialsId,
                        SetSupervisorMaterialsId = mlsop.SupervisorMaterialsId,
                        SetViolationsASMaterialsId = mlsop.ViolationsASMaterialsId,
                        SetMonitoringSystemMaterialsId = mlsop.MonitoringSystemMaterialsId,
                        SetSafetyGradeMaterialsId = mlsop.SafetyGradeMaterialsId,
                        SetRemark = org.Remark,
                        RegistrationStatus = mlsop.ApprovalStatus,
                        FinalRejection = mlsop.ApprovalRemark,
                        OrgCode = mlsop.OrgCode
                    }).FirstOrDefault();
            if (thirdPartyInformation == null) return result;
            result.Data = thirdPartyInformation;
            return result;
        }

        public ServiceResult<GetEmailDto> GetVerificationCode(GetEmailDto emailParameter)
        {
            var result = new ServiceResult<GetEmailDto>();
            var key = $"{ConfigurationManager.AppSettings["APPCODE"].ToString()}-CLBASH:{emailParameter.EmailCode}";
            var code = string.Empty;
            string emailCode;
            var registrationType = emailParameter.Type == 1 ? "企业" : "第三方机构";
            //判断验证码是否过期
            if (RedisManager.HasKey(key))
            {
                var value = RedisManager.Get(key).ToString();
                code = value;
                emailCode = emailParameter.EmailCode;
            }
            else
            {
                var verificationId = Guid.NewGuid();
                emailCode = verificationId.ToString();
                key = $"{ConfigurationManager.AppSettings["APPCODE"].ToString()}-CLBASH:{emailCode}";
                //生成随机数字
                var rand = new Random();
                for (var i = 0; i < 6; i++)
                {
                    code += rand.Next(0, 9).ToString();
                }
                RedisManager.Set(key, code, TimeSpan.FromMinutes(30));
            }

            //发送人邮箱
            var senderEmail = ConfigurationManager.AppSettings["EmailCode"].ToString();
            //发送人邮箱授权码
            var senderEmailCode = ConfigurationManager.AppSettings["EmailNumber"].ToString();
            var argCode = senderEmail.Split('@');
            try
            {
                //实例化一个发送邮件类。
                var mailMessage = new MailMessage {From = new MailAddress(senderEmail)};
                //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                //收件人邮箱地址。
                mailMessage.To.Add(new MailAddress(emailParameter.Email));
                //邮件标题。
                mailMessage.Subject = "平台注册验证码";
                //邮件内容。
                mailMessage.Body =
                    $@"您于{DateTime.Now.ToLongDateString().ToString()}在清远市交通运输局两客一危一重营运车辆主动安全防控平台进行{registrationType}账号注册，注册邮箱为{emailParameter.Email}，请使用验证码进行验证校验。
验证码：{code}

如果不是你本人进行注册，请忽略本信息。

请注意验证码保密，谢谢！
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

            var emailList = new GetEmailDto
            {
                VerificationCodeId = emailCode
            };
            result.Data = emailList;
            return result;
        }

        public ServiceResult<bool> GetMechanism(string providerName)
        {
            try
            {
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var enterpriseThirdParty = _fuWuShangRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgName == providerName)
                    .FirstOrDefault();
                return enterpriseThirdParty == null ? new ServiceResult<bool> { Data = true } 
                    : new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "当前服务商已注册，请使用账号密码登录平台。" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("服务商信息获取失败" + ex.Message);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "操作失败，请稍后重试" };
            }
        }

        #region 列表

        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                FuWuShangSearchDto search =
                    JsonConvert.DeserializeObject<FuWuShangSearchDto>(queryData.data.ToString());
                UserInfoDtoNew userInfoDto = GetUserInfo();
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;

                Expression<Func<OrgBaseInfo, bool>> orgBaseExp = x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgType == (int) OrganizationType.本地服务商;
                Expression<Func<FuWuShang, bool>> fuWuShangExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;

                // 机构名称
                if (!string.IsNullOrWhiteSpace(search.JiGouMingCheng))
                {
                    orgBaseExp = orgBaseExp.And(x => x.OrgName.Contains(search.JiGouMingCheng.Trim()));
                }

                // 营业执照号
                if (!string.IsNullOrWhiteSpace(search.YingYeZhiZhaoHao))
                {
                    fuWuShangExp = fuWuShangExp.And(x => x.YingYeZhiZhaoHao.Contains(search.YingYeZhiZhaoHao.Trim()));
                }

                if (userInfoDto.OrganizationType == (int) OrganizationType.市政府)
                {
                    orgBaseExp = orgBaseExp.And(x => x.XiaQuShi == userInfoDto.OrganCity);
                }

                if (userInfoDto.OrganizationType == (int) OrganizationType.县政府)
                {
                    orgBaseExp = orgBaseExp.And(x =>
                        x.XiaQuXian == userInfoDto.OrganDistrict && x.XiaQuShi == userInfoDto.OrganCity);
                }

                if (userInfoDto.OrganizationType == (int) OrganizationType.本地服务商)
                {
                    orgBaseExp = orgBaseExp.And(x => x.OrgCode == userInfoDto.OrganizationCode);
                }

                if (userInfoDto.OrganizationType == (int) OrganizationType.企业)
                {
                    orgBaseExp = orgBaseExp.And(x => x.XiaQuShi == userInfoDto.OrganCity);
                }

                //有效状态 正常营业   合约到期
                if (!string.IsNullOrWhiteSpace(search.YouXiaoZhuangTai))
                {
                    int Status = Convert.ToInt32(search.YouXiaoZhuangTai);
                    orgBaseExp = orgBaseExp.And(x => x.ZhuangTai == Status);
                }

                var query = from p in _orgBaseInfoRepository.GetQuery(orgBaseExp)
                    join q in _fuWuShangRepository.GetQuery(fuWuShangExp)
                        on p.Id.ToString() equals q.BaseId
                    select new
                    {
                        p.Id,
                        p.OrgCode,
                        p.OrgName,
                        p.OrgShortName,
                        q.YingYeZhiZhaoHao,
                        JingYingQuYu = p.JingYingFanWei,
                        YouXiaoZhuangTai = p.ZhuangTai,
                        p.SYS_ChuangJianShiJian,
                        TpNameId = q.Id
                    };
                QueryResult queryResult = new QueryResult();
                queryResult.totalcount = query.Count();
                queryResult.items = query.OrderByDescending(x => x.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                return new ServiceResult<QueryResult>() {Data = queryResult};
            });
        }

        #endregion

        public ServiceResult<QueryResult> ThirdPartyQuery(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                FuWuShangGetDto search =
                    JsonConvert.DeserializeObject<FuWuShangGetDto>(queryData.data.ToString());
                UserInfoDtoNew userInfoDto = GetUserInfo();

                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;

                Expression<Func<OrgBaseInfo, bool>> orgBaseExp = x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgType == (int) OrganizationType.本地服务商;

                Expression<Func<FuWuShang, bool>> fuWuShangExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<MaterialListOfServiceProvider, bool>> materialListOfServiceProvider = q => q.SYS_XiTongZhuangTai == sysZhengChang; 
                if (search.BeiAnTongGuoBeginTime.HasValue)
                {
                    fuWuShangExp = fuWuShangExp.And(p => p.SYS_ChuangJianShiJian >= search.BeiAnTongGuoBeginTime);
                }

                if (search.BeiAnTongGuoEndTime.HasValue)
                {
                    fuWuShangExp = fuWuShangExp.And(p => p.SYS_ChuangJianShiJian <= search.BeiAnTongGuoEndTime);
                }

                // 机构名称
                if (!string.IsNullOrWhiteSpace(search.OrgName))
                {
                    fuWuShangExp = fuWuShangExp.And(x => x.OrgName.Contains(search.OrgName.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(search.RegistrationStatus))
                {
                    int registrationStatus = int.Parse(search.RegistrationStatus);
                    materialListOfServiceProvider = materialListOfServiceProvider.And(x => x.ApprovalStatus == registrationStatus);
                }
                
                //为1时 为服务商端
                if (search.Type == "1")
                {
                    orgBaseExp = orgBaseExp.And(x => x.OrgCode == userInfoDto.OrganizationCode);
                }

                var query = from p in _orgBaseInfoRepository.GetQuery(orgBaseExp)
                    join q in _fuWuShangRepository.GetQuery(fuWuShangExp)
                        on p.Id.ToString() equals q.BaseId
                        join mospr in _materialListOfServiceProviderRepository.GetQuery(materialListOfServiceProvider)
                            on q.OrgCode equals mospr.OrgCode
                    select new FuWuShangGetDto
                    {
                        Id = p.Id,
                        LianXiRenPhone = q.LianXiRenPhone,
                        LianXiRenName = q.LianXiRenName,
                        JingYingQuYu = p.JingYingFanWei,
                        OrgName = q.OrgName,
                        OrgCode = q.OrgCode,
                        BeginTime = q.SYS_ChuangJianShiJian,
                        RegistrationStatus = mospr.ApprovalStatus.ToString()
                    };
                var queryResult = new QueryResult {totalcount = query.Count()};
                var list = query.OrderByDescending(x => x.BeginTime)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                queryResult.items = list;
                return new ServiceResult<QueryResult>() {Data = queryResult};
            });
        }

        public ServiceResult<ExportResponseInfoDto> ExportThirdParty(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2};
                }

                var list = GetThirdPartyList(queryData);
                var tableTitle = "第三方机构备案信息审核列表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Any())
                {
                    try
                    {
                        string FileId = string.Empty;
                        Guid? fileUploadId = CreateQiYeDangAnExcelAndUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            FileId = fileUploadId.ToString();
                        }

                        return new ServiceResult<ExportResponseInfoDto>
                            {Data = new ExportResponseInfoDto {FileId = FileId}};
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出第三方端出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "导出出错", StatusCode = 2};
                    }
                }
                return new ServiceResult<ExportResponseInfoDto> {StatusCode = 2, ErrorMessage = "没有需要导出的数据"};
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出第三方端出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "导出出错", StatusCode = 2};
            }
        }

        #region 导出

        private static Guid? CreateQiYeDangAnExcelAndUpload(List<FuWuShangGetDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "第三方机构备案信息审核列表";
            string[] cellTitleArry = {"备案机构名称", "拟经营区域", "联系人", "联系电话", "创建时间", "审核状态"};
            var workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet) workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
                HSSFRow row = (HSSFRow) sheet.CreateRow(0);

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
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow) sheet.CreateRow(1);

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
                    row = (HSSFRow) sheet.CreateRow(content_index + 2);
                    int index = 0;

                    row.CreateCell(index++).SetCellValue(item.OrgName);
                    row.CreateCell(index++).SetCellValue(item.JingYingQuYu);
                    row.CreateCell(index++).SetCellValue(item.LianXiRenName);
                    row.CreateCell(index++).SetCellValue(item.LianXiRenPhone);
                    var beginTime = string.Empty;
                    if (item.BeginTime.HasValue)
                    {
                        beginTime = item.BeginTime.Value.ToString("yyyy-MM-dd");
                    }
                    row.CreateCell(index++).SetCellValue(beginTime);
                    var selectThirdParty = typeof(ThirdPartyRegistrationStatus).GetEnumName(int.Parse(item.RegistrationStatus));
                    row.CreateCell(index++).SetCellValue(selectThirdParty);
                    for (int contInx = 0; contInx < index; contInx++)
                    {
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

        #region 修改

        public ServiceResult<bool> Update(string reqid, FuWuShangExDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                if (string.IsNullOrWhiteSpace(dto.Id))
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = string.Format("参数错误,Id为空")};
                }

                // 组织基本信息
                OrgBaseInfo orgBaseInfo = _orgBaseInfoRepository
                    .GetQuery(x => x.Id == new Guid(dto.Id) && x.SYS_XiTongZhuangTai == sysZhengChang).FirstOrDefault();
                //服务商信息
                FuWuShang fuWuShang = _fuWuShangRepository
                    .GetQuery(x => x.BaseId == dto.Id && x.SYS_XiTongZhuangTai == sysZhengChang).FirstOrDefault();

                if (orgBaseInfo == null || fuWuShang == null)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = string.Format("找不到组织基本信息或服务商信息")};
                }

                //组织基本信息 更新的信息
                orgBaseInfo.OrgName = dto.OrgName;
                orgBaseInfo.OrgShortName = dto.OrgShortName;
                orgBaseInfo.JingYingFanWei = dto.JingYingQuYu;
                orgBaseInfo.DiZhi = dto.GongSiDiZhi;
                orgBaseInfo.Remark = dto.BeiZhu;
                orgBaseInfo.XiaQuSheng = dto.XiaQuSheng;
                orgBaseInfo.XiaQuShi = dto.XiaQuShi;
                orgBaseInfo.XiaQuXian = dto.XiaQuXian;
                orgBaseInfo.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                orgBaseInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                orgBaseInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                orgBaseInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;

                //平台代理商信息 更新的信息
                fuWuShang.OrgName = dto.OrgName;
                fuWuShang.OrgShortName = dto.OrgShortName;
                fuWuShang.YingYeZhiZhaoHao = dto.YingYeZhiZhaoHao;
                fuWuShang.TongYiSheHuiXinYongDaiMa = dto.TongYiSheHuiXinYongDaiMa;
                fuWuShang.YouBian = dto.YouBian;
                fuWuShang.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                fuWuShang.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                fuWuShang.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;

                // 校验 校验 统一社会信用代码和营业执照号 唯一性
                var validRsult = Vaild(fuWuShang);
                if (!validRsult.Data)
                {
                    return validRsult;
                }
                else
                {
                    //检测名称是否已经使用
                    var isExitModel = _orgBaseInfoRepository.Count(x =>
                        x.OrgName == dto.OrgName && x.Id != new Guid(dto.Id) && x.SYS_XiTongZhuangTai == sysZhengChang);
                    if (isExitModel > 0)
                    {
                        return new ServiceResult<bool>()
                        {
                            Data = false, StatusCode = 2,
                            ErrorMessage = string.Format("已经存在名称为{0}的服务商，请使用其它名称", dto.OrgName)
                        };
                    }

                    var preEntity = _orgBaseInfoRepository.GetByKey(new Guid(dto.Id));

                    var systemOrganizationInfoDto = new
                    {
                        OrganizationCode = preEntity.OrgCode,
                        OrganizationName = dto.OrgName,
                        ManageArea = dto.JingYingQuYu,
                        OrgProvince = dto.XiaQuSheng,
                        OrgCity = dto.XiaQuShi,
                        OrgDistrict = dto.XiaQuXian,
                        SysId = sysId
                    };
                    var updateSystemOrgResult = GetInvokeRequest("00000030037", "1.0", systemOrganizationInfoDto);
                    if (updateSystemOrgResult.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = updateSystemOrgResult.publicresponse.message};
                    }

                    if (!(bool) updateSystemOrgResult.body.success)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = updateSystemOrgResult.body.msg};
                    }

                    using (var uow = new UnitOfWork())
                    {
                        uow.BeginTransaction();

                        //更新组织基本信息
                        _orgBaseInfoRepository.Update(orgBaseInfo);
                        //更新服务商信息
                        _fuWuShangRepository.Update(fuWuShang);

                        var updateResult = uow.CommitTransaction() > 0;
                        if (updateResult)
                        {
                            AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                            {
                                ReqId = reqid,
                                YeWuDuiXiangLeiXing = "企业GPS基础档案",
                                YeWuDuiXiangZiLei = "服务商档案",
                                YeWuDuiXiangID = new Guid(dto.Id),
                                YeWuDuiXiangBiaoZhi = dto.OrgName,
                                YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(dto),
                                YeWuLeiXing = "修改企业GPS基础档案",
                                YeWuZiLei = "修改服务商档案",
                                YeWuChangJingLeiXing = "基础档案业务",
                                MoKuaiMingCheng = "企业GPS基础档案管理",
                                XiTongMingCheng = "企业GPS基础档案系统",
                                YingYongMingCheng = "服务商档案系统",
                                YeWuGaiYaoXinXi = string.Format("修改档案：{0}", dto.OrgName),
                            }, userInfo);
                            return new ServiceResult<bool>() {Data = true};
                        }
                        else
                        {
                            return new ServiceResult<bool>()
                                {Data = false, StatusCode = 2, ErrorMessage = "修改服务商信息出错了"};
                        }
                    }
                }
            });
        }

        #endregion

        public ServiceResult<bool> FilingMaterials(ServiceProviderDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                //判断邮箱验证码是否正确
                var key = $"{ConfigurationManager.AppSettings["APPCODE"].ToString()}-CLBASH:{dto.RedisId}";
                var value = RedisManager.Get(key).ToString();
                if (value != dto.EmailVerificationCode)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "邮箱验证码错误"};
                }

                // 组织基本信息Id
                var baseId = Guid.NewGuid();
                // 服务商信息Id
                var orgId = Guid.NewGuid();
                //查看 第三方机构是否存在
                var fuWuShang = _fuWuShangRepository
                    .GetQuery(x => x.OrgName == dto.OrganizationName && x.SYS_XiTongZhuangTai == sysZhengChang)
                    .FirstOrDefault();
                if (fuWuShang != null)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "当前服务商已注册，请使用账号密码登录平台。"};
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    #region 注册第三方机构

                    string orgCode;
                    var getSNoResponse = GetInvokeRequest("00000020013", "1.0", new
                    {
                        SysId = sysId,
                        Module = "00330021",
                        Type = 2
                    });

                    if (getSNoResponse.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getSNoResponse.publicresponse.message};
                    }
                    else
                    {
                        orgCode = "fws" + getSNoResponse.body.SNo.ToString().PadLeft(4, '0');
                    }

                    //新增服务商
                    var fuWuShangEntity = new FuWuShang
                    {
                        Id = orgId,
                        BaseId = baseId.ToString(),
                        LianXiRenName = dto.ContactName,
                        OrgCode = orgCode,
                        LianXiRenPhone = dto.ContactTelephone,
                        //组织编号
                        OrgType = (int) OrganizationType.本地服务商,
                        OrgShortName = dto.OrganizationName,
                        OrgName = dto.OrganizationName,
                        TongYiSheHuiXinYongDaiMa = dto.UnifiedSocialCreditCode,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常
                    };
                    _fuWuShangRepository.Add(fuWuShangEntity);

                    var orgBaseInfoEntity = new OrgBaseInfo
                    {
                        Id = baseId,
                        OrgType = (int) OrganizationType.本地服务商,
                        OrgShortName = dto.OrganizationName,
                        OrgName = dto.OrganizationName,
                        ParentOrgId = baseId,
                        OrgCode = orgCode,
                        JingYingFanWei = dto.JingYingFanWei,
                        XiaQuSheng = "广东",
                        XiaQuShi = "清远",
                        XiaQuXian = string.Empty,
                        DiZhi = string.Empty,
                        Remark = dto.Remark,
                        ZhuangTai = (int) ZhuangTaiEnum.正常营业,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常,
                    };
                    _orgBaseInfoRepository.Add(orgBaseInfoEntity);


                    var materialListOfServiceProviderEntity = new MaterialListOfServiceProvider
                    {
                        Id = Guid.NewGuid(),
                        OrgCode = orgCode,
                        ApprovalStatus = (int) ThirdPartyRegistrationStatus.市级审核中,
                        CompanyType = dto.CompanyType,
                        Landline = dto.Landline,
                        Mailbox = dto.Mailbox,
                        IDCard = dto.ContactIDNumber,
                        IndustrialACBLicenseId = dto.IndustrialACBLicenseId,
                        IndustrialACBLOTOfficeId = dto.IndustrialACBLOTOfficeId,
                        FilingApplicationFormId = dto.FilingApplicationFormId,
                        StandardTOServiceId = dto.StandardTOServiceId,
                        SiteMaterialsId = dto.SiteMaterialsId,
                        PhotosOBPremisesId = dto.PhotosOBPremisesId,
                        PersonnelPDMaterialsId = dto.PersonnelPDMaterialsId,
                        PersonnelSSCertificateId = dto.PersonnelSSCertificateId,
                        EquipmentIMaterialsId = dto.EquipmentIMaterialsId,
                        SupervisorMaterialsId = dto.SupervisorMaterialsId,
                        ViolationsASMaterialsId = dto.ViolationsASMaterialsId,
                        MonitoringSystemMaterialsId = dto.MonitoringSystemMaterialsId,
                        SafetyGradeMaterialsId = dto.SafetyGradeMaterialsId,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常
                    };
                    _materialListOfServiceProviderRepository.Add(materialListOfServiceProviderEntity);

                    #endregion

                    #region 账号分配与邮件发送

                    //服务商角色
                    var roleCodes = $"{(int) ZuZhiJueSe.第三方机构审核填报用户:000}";

                    var syId = ConfigurationManager.AppSettings["WEBAPISYSID"];
                    //许可证号后10位
                    var userName = GetLastStr(dto.UnifiedSocialCreditCode, 10);
                    var passwordStatus = false;
                    //账号信息
                    AccountInformation accountInformation;
                    using (IDbConnection conn =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["DC_YHQXDS"].ConnectionString))
                    {

                        var sql =
                            $@"SELECT * FROM  dbo.T_User WHERE SYS_XiTongZhuangTai=0 AND UserName='{userName}'";

                        accountInformation = conn.Query<AccountInformation>(sql).ToList().FirstOrDefault();
                    }

                    //不存在账号时才进行添加
                    if (accountInformation == null)
                    {
                        //服务商用户注册
                        var queryRequest = GetInvokeRequest("00000030057", "1.0", new
                        {
                            SysId = syId,
                            SysOrgId = "C6380E44-F83F-A921-7174-5B6A8565BB4E",
                            OrgName = dto.OrganizationName,
                            OrgType = (int) OrganizationType.本地服务商,
                            OrgCode = orgCode,
                            UserName = userName, //TODO:生成序列号
                            RoleCode = roleCodes,
                            OrganizationProvince = "广东",
                            OrganizationCity = "清远",
                            OrganizationDistrict = "",
                            ManageArea = "广东清远",
                            SYS_XiTongBeiZhu = "",
                            Password = "DSF" + GetLastStr(userName, 6)
                        });
                        if (queryRequest.body == false)
                        {
                            return new ServiceResult<bool>()
                                {Data = false, StatusCode = 2, ErrorMessage = queryRequest.body.msg};
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

                    var resendEmailDto = new ResendEmailDto
                    {
                        BusinessLicense = dto.UnifiedSocialCreditCode,
                        Email = dto.Mailbox,
                        PasswordStatus = passwordStatus
                    };
                    ResendEmail(resendEmailDto);

                    #endregion

                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = "注册第三方机构出错了"};
                    }
                }
            });
        }


        public ServiceResult<ExportResponseInfoDto> BatchDownload(string material)
        {
            var materialList = _materialListOfServiceProviderRepository.GetQuery(x =>
                x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常 && x.OrgCode == material).FirstOrDefault();
            if (materialList == null)
            {
                return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "该第三方机构,材料信息不存在", StatusCode = 2};
            }

            var fileName = material;

            var thirdPartyEntity = _fuWuShangRepository.GetQuery(x =>
                x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常
                && x.OrgCode == material).FirstOrDefault();
            if (thirdPartyEntity != null)
            {
                fileName = thirdPartyEntity.OrgName;
            }
            try
            {
                var fileAddress = "D:/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "第三方机构材料/";
                Directory.CreateDirectory(fileAddress);
                //第一步初始化
                Mapper.Initialize(x => { x.CreateMap<MaterialListOfServiceProvider, MaterialDocuments>(); });

                //开始搬运
                var materialEntity = Mapper.Map<MaterialListOfServiceProvider, MaterialDocuments>(materialList);
                var propertyInfos = typeof(MaterialDocuments).GetProperties();
                var i = 0;
                foreach (var p in propertyInfos)
                {
                    var materialValue = p.GetValue(materialEntity).ToString();
                    if (!string.IsNullOrEmpty(materialValue))
                    {
                        if(p.Name == "IndustrialACBLOTOfficeId" && materialList.CompanyType != (int)CompanyType.分公司)
                            continue;
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
                var zipAddress = @"D:\" +material + "附件.zip";
                var fileId = CompressDirectory(fileAddress, "", 6, true, out materialName, zipAddress, fileName);
                return new ServiceResult<ExportResponseInfoDto> {Data = new ExportResponseInfoDto {FileId = fileId}};
            }
            catch (Exception e)
            {
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "该第三方机构,材料信息不存在", StatusCode = 2 };
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
        public static string CompressDirectory(string dirPath, string GzipFileName, int CompressionLevel, bool deleteDir, out string GzipPath,string zipAddress,string material)
        {
            string result = string.Empty;
            try
            {
                //压缩文件为空时默认与压缩文件夹同一级目录
                if (GzipFileName == string.Empty)
                {
                    GzipFileName = dirPath+"材料附件.zip";
                }
                var file = File.Create(GzipFileName);
                var zipBool = @"第三方机构材料\材料附件.zip";
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
            var fileId= fileDtoResult.FileId;
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

        public ServiceResult<bool> FilingReview(ServiceProviderDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //1）	备注状态通过=》  分配新的权限 ，市级审核通过
                //3）	备案状态未通过 =》  市级审核不通过
                //2）	通过后 驳回=》 收回权限    市级审核驳回
                var userInfo = GetUserInfo();
                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var fuWuShang = _fuWuShangRepository.
                        GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == dto.OperatorCode).ToList();
                    var entity = fuWuShang.FirstOrDefault();
                    if (entity == null)
                    {
                        return new ServiceResult<bool>()
                            { Data = false, StatusCode = 2, ErrorMessage = "第三方机构存在数据无法找到" };

                    }
                    var materialList = _materialListOfServiceProviderRepository.GetQuery
                        (x => x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == dto.OperatorCode).FirstOrDefault();
                    if (materialList != null)
                    {
                        //通过
                        if (dto.RegistrationStatus == (int) ThirdPartyRegistrationStatus.市级审核通过)
                        {
                            if (materialList.ApprovalStatus != (int) ThirdPartyRegistrationStatus.市级审核中)
                            {
                                return new ServiceResult<bool>()
                                    {Data = false, StatusCode = 2, ErrorMessage = "只能对市级审核中的数据进行审核"};
                            }
                            materialList.ApprovalStatus = (int) ThirdPartyRegistrationStatus.市级审核通过;
                            var queryParam = new
                            {
                                OrgCode = fuWuShang.Select(x => x.OrgCode).ToList(),
                                RoleCodeList = new List<string> { "004" },
                                SysId = sysId,
                            };
                            var queryRequest = GetInvokeRequest("00000030060", "1.0", queryParam);
                            if (queryRequest != null)
                            {
                                if (queryRequest.publicresponse.statuscode != 0)
                                {
                                    LogHelper.Error($"主管部门审核第三方机构备案信息调用接口00000030060响应异常{JsonConvert.SerializeObject(queryRequest)}");
                                }
                            }
                            else
                            {
                                LogHelper.Error("主管部门审核第三方机构备案调用接口00000030060返回结果为空");
                            }
                        }
                        else if(dto.RegistrationStatus == (int)ThirdPartyRegistrationStatus.市级审核不通过)
                        {
                            if (materialList.ApprovalStatus == (int) ThirdPartyRegistrationStatus.市级审核不通过
                                || materialList.ApprovalStatus == (int) ThirdPartyRegistrationStatus.市级审核驳回)
                            {
                                return new ServiceResult<bool>()
                                    { Data = false, StatusCode = 2, ErrorMessage = "只有市级审核中，市级审核通过的数据才行执行此操作" };
                            }

                            if (materialList.ApprovalStatus == (int) ThirdPartyRegistrationStatus.市级审核中)
                            {
                                materialList.ApprovalStatus = (int) ThirdPartyRegistrationStatus.市级审核不通过;
                            }else if 
                                (materialList.ApprovalStatus == (int)ThirdPartyRegistrationStatus.市级审核通过)
                            {
                                materialList.ApprovalStatus = (int) ThirdPartyRegistrationStatus.市级审核驳回;
                            }

                            materialList.ApprovalRemark = dto.FinalRejection;
                            var queryParam = new
                            {
                                OrgCode = fuWuShang.Select(x => x.OrgCode).ToList(),
                                RoleCodeList = new List<string> { "004" },
                                SysId = sysId,
                            };
                            var queryRequest = GetInvokeRequest("00000030061", "1.0", queryParam);
                            if (queryRequest != null)
                            {
                                if (queryRequest.publicresponse.statuscode != 0)
                                {
                                    LogHelper.Error($"主管部门审核第三方机构备案信息调用接口00000030061响应异常{JsonConvert.SerializeObject(queryRequest)}");
                                }
                            }
                            else
                            {
                                LogHelper.Error("主管部门审核第三方机构备案信息调用接口00000030061返回结果为空");
                            }
                        }
                        materialList.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        materialList.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        materialList.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _materialListOfServiceProviderRepository.Update(materialList);
                    }
                    var updateResult = uow.CommitTransaction() > 0;
                    if (updateResult)
                    {
                        return new ServiceResult<bool>() { Data = true };

                    }
                    return new ServiceResult<bool>()
                        { Data = false, StatusCode = 2, ErrorMessage = "备案失败" };
                }

            });
        }

        public static string GetChineseSpell(string strText)
        {
            var len = strText.Length;
            var myStr = "";
            for (var i = 0; i < len; i++)
            {
                myStr += GetSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        public static string GetSpell(string cnChar)
        {
            byte[] arrCn = Encoding.Default.GetBytes(cnChar);
            if (arrCn.Length <= 1) return cnChar;
            int area = arrCn[0];
            int pos = arrCn[1];
            int code = (area << 8) + pos;
            int[] areacode =
            {
                45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896,
                50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481
            };
            for (var i = 0; i < 26; i++)
            {
                var max = 55290;
                if (i != 25) max = areacode[i + 1];
                if (areacode[i] <= code && code < max)
                {
                    return Encoding.Default.GetString(new[] { (byte)(65 + i) });
                }
            }
            return "*";
        }

        #region 校验 统一社会信用代码和营业执照号 唯一性
        private ServiceResult<bool> Vaild(FuWuShang model)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            if (string.IsNullOrEmpty(model.TongYiSheHuiXinYongDaiMa) && string.IsNullOrEmpty(model.YingYeZhiZhaoHao))
            {
                return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "统一社会信用代码和营业执照号不能同时为空" };
            }
            if (!string.IsNullOrEmpty(model.YingYeZhiZhaoHao))
            {
                var isReplyNumber = _fuWuShangRepository.Count(x => x.YingYeZhiZhaoHao == model.YingYeZhiZhaoHao && x.BaseId != model.BaseId && x.SYS_XiTongZhuangTai == sysZhengChang);
                if (isReplyNumber > 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("已存在营业执照号为{0}的平台代理商，请使用其它营业执照号", model.YingYeZhiZhaoHao) };
                }
            }
            if (!string.IsNullOrEmpty(model.TongYiSheHuiXinYongDaiMa))
            {
                var isReplyNumber = _fuWuShangRepository.Count(x => x.TongYiSheHuiXinYongDaiMa == model.TongYiSheHuiXinYongDaiMa && x.BaseId != model.BaseId && x.SYS_XiTongZhuangTai == sysZhengChang);
                if (isReplyNumber > 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("已存在统一社会信用代码为{0}的平台代理商，请使用其它统一社会信用代码", model.TongYiSheHuiXinYongDaiMa) };
                }
            }

            return new ServiceResult<bool>() { Data = true };
        }
        #endregion
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
                //许可证号后10位
                var userName = GetLastStr(dto.BusinessLicense, 10);
                var senderEmail = ConfigurationManager.AppSettings["EmailCode"];
                //发送人邮箱授权码
                var senderEmailCode = ConfigurationManager.AppSettings["EmailNumber"];
                var argCode = senderEmail.Split('@');
                var password = $"DSF{GetLastStr(dto.BusinessLicense, 6)}";
                if (dto.PasswordStatus)
                {
                    password = "123456";
                }
                try
                {
                    //实例化一个发送邮件类。
                    var mailMessage = new MailMessage { From = new MailAddress(senderEmail) };
                    //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                    //收件人邮箱地址。
                    mailMessage.To.Add(new MailAddress(dto.Email));
                    //邮件标题。
                    mailMessage.Subject = "第三方机构账号注册";
                    //邮件内容。
                    mailMessage.Body =
                        $@"您于{DateTime.Now.ToLongDateString().ToString()}在清远市交通运输局两客一危一重营运车辆主动安全防控平台进行第三方机构账号注册，注册邮箱为{dto.Email}，现已通过账号注册申请，请使用账号密码进行登录。并及时修改密码。
账号：{userName}
密码：{password}

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

        public ServiceResult<bool> FilingAgainMaterials(ServiceProviderDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                //查看 第三方机构是否存在
                var fuWuShang = _fuWuShangRepository
                    .GetQuery(x => x.OrgCode == dto.OperatorCode && x.SYS_XiTongZhuangTai == sysZhengChang)
                    .FirstOrDefault();
                var userInfo = GetUserInfo();
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //新增服务商
                    if (fuWuShang == null)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = "第三方机构不存在"};
                    }

                    var materialList = _materialListOfServiceProviderRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang
                        && x.OrgCode == dto.OperatorCode).FirstOrDefault();
                    if (materialList == null)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = "第三方机构备案信息不存在"};
                    }


                    fuWuShang.LianXiRenPhone = dto.ContactTelephone;
                    fuWuShang.LianXiRenName = dto.ContactName;
                    fuWuShang.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    fuWuShang.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                    fuWuShang.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                    _fuWuShangRepository.Update(fuWuShang);

                    if (materialList.ApprovalStatus == (int) ThirdPartyRegistrationStatus.市级审核不通过
                        || materialList.ApprovalStatus == (int)ThirdPartyRegistrationStatus.市级审核驳回)
                    {
                        materialList.ApprovalStatus = (int) ThirdPartyRegistrationStatus.市级审核中;
                    }
                    if (dto.CompanyType == (int) CompanyType.总公司)
                    {
                        dto.IndustrialACBLOTOfficeId = string.Empty;
                    }
                    materialList.CompanyType = dto.CompanyType;
                    materialList.Landline = dto.Landline;
                    materialList.IDCard = dto.ContactIDNumber;
                    materialList.IndustrialACBLicenseId = dto.IndustrialACBLicenseId;
                    materialList.IndustrialACBLOTOfficeId = dto.IndustrialACBLOTOfficeId;
                    materialList.FilingApplicationFormId = dto.FilingApplicationFormId;
                    materialList.StandardTOServiceId = dto.StandardTOServiceId;
                    materialList.SiteMaterialsId = dto.SiteMaterialsId;
                    materialList.PhotosOBPremisesId = dto.PhotosOBPremisesId;
                    materialList.PersonnelPDMaterialsId = dto.PersonnelPDMaterialsId;
                    materialList.PersonnelSSCertificateId = dto.PersonnelSSCertificateId;
                    materialList.EquipmentIMaterialsId = dto.EquipmentIMaterialsId;
                    materialList.SupervisorMaterialsId = dto.SupervisorMaterialsId;
                    materialList.ViolationsASMaterialsId = dto.ViolationsASMaterialsId;
                    materialList.MonitoringSystemMaterialsId = dto.MonitoringSystemMaterialsId;
                    materialList.SafetyGradeMaterialsId = dto.SafetyGradeMaterialsId;
                    materialList.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    materialList.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                    materialList.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                    _materialListOfServiceProviderRepository.Update(materialList);
                    var updateResult = uow.CommitTransaction() > 0;

                    if (updateResult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = "第三方机构重新备案出错了"};
                    }
                }
            });
        }

        #region 根据编号获取第三方机构信息
        public ServiceResult<ServiceProviderSetDto> GetDataDyNumber(string providerName)
        {
            try
            {
                var result = new ServiceResult<ServiceProviderSetDto>();
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var thirdPartyInformation =
                (from org in _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                 join fws in _fuWuShangRepository.GetQuery(x =>
                         x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == providerName)
                     on org.Id.ToString() equals fws.BaseId
                 join mlsop in _materialListOfServiceProviderRepository.GetQuery(x =>
                         x.SYS_XiTongZhuangTai == sysZhengChang)
                     on fws.OrgCode equals mlsop.OrgCode
                 select new ServiceProviderSetDto
                 {
                     Id = fws.BaseId,
                     CompanyTypes = (int)mlsop.CompanyType,
                     SetOrganizationName = fws.OrgName,
                     SetUnifiedSocialCreditCode = fws.TongYiSheHuiXinYongDaiMa,
                     SetIndustrialACBLicenseId = mlsop.IndustrialACBLicenseId,
                     SetIndustrialACBLOTOfficeId = mlsop.IndustrialACBLOTOfficeId,
                     SetJingYingFanWei = org.JingYingFanWei,
                     SetContactName = fws.LianXiRenName,
                     SetContactIDNumber = mlsop.IDCard,
                     SetContactTelephone = fws.LianXiRenPhone,
                     SetLandline = mlsop.Landline,
                     SetMailbox = mlsop.Mailbox,
                     SetFilingApplicationFormId = mlsop.FilingApplicationFormId,
                     SetStandardTOServiceId = mlsop.StandardTOServiceId,
                     SetSiteMaterialsId = mlsop.SiteMaterialsId,
                     SetPhotosOBPremisesId = mlsop.PhotosOBPremisesId,
                     SetPersonnelPDMaterialsId = mlsop.PersonnelPDMaterialsId,
                     SetPersonnelSSCertificateId = mlsop.PersonnelSSCertificateId,
                     SetEquipmentIMaterialsId = mlsop.EquipmentIMaterialsId,
                     SetSupervisorMaterialsId = mlsop.SupervisorMaterialsId,
                     SetViolationsASMaterialsId = mlsop.ViolationsASMaterialsId,
                     SetMonitoringSystemMaterialsId = mlsop.MonitoringSystemMaterialsId,
                     SetSafetyGradeMaterialsId = mlsop.SafetyGradeMaterialsId,
                     SetRemark = org.Remark,
                     RegistrationStatus = mlsop.ApprovalStatus,
                     FinalRejection = mlsop.ApprovalRemark,
                     OrgCode = mlsop.OrgCode
                 }).FirstOrDefault();
            if (thirdPartyInformation == null) return result;
            result.Data = thirdPartyInformation;
            return result;
            }
            catch (Exception e)
            {
                LogHelper.Error("服务商编号获取详细信息出错" + e.Message, e);
                return null;
                
            }
        }
        #endregion

        public List<FuWuShangGetDto> GetThirdPartyList(QueryData queryData)
        {
            try
            {
                FuWuShangGetDto search =
                JsonConvert.DeserializeObject<FuWuShangGetDto>(queryData.data.ToString());
            var userInfoDto = GetUserInfo();
            var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;

            Expression<Func<OrgBaseInfo, bool>> orgBaseExp = x =>
                x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgType == (int) OrganizationType.本地服务商;

            Expression<Func<FuWuShang, bool>> fuWuShangExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<MaterialListOfServiceProvider, bool>> materialListOfServiceProvider =
                q => q.SYS_XiTongZhuangTai == sysZhengChang;
            if (search.BeiAnTongGuoBeginTime.HasValue)
            {
                fuWuShangExp = fuWuShangExp.And(p => p.SYS_ChuangJianShiJian >= search.BeiAnTongGuoBeginTime);
            }

            if (search.BeiAnTongGuoEndTime.HasValue)
            {
                fuWuShangExp = fuWuShangExp.And(p => p.SYS_ChuangJianShiJian <= search.BeiAnTongGuoEndTime);
            }

            // 机构名称
            if (!string.IsNullOrWhiteSpace(search.OrgName))
            {
                fuWuShangExp = fuWuShangExp.And(x => x.OrgName.Contains(search.OrgName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(search.RegistrationStatus))
            {
                int registrationStatus = int.Parse(search.RegistrationStatus);
                materialListOfServiceProvider =
                    materialListOfServiceProvider.And(x => x.ApprovalStatus == registrationStatus);
            }
            //为1时 为服务商端
            if (search.Type == "1")
            {
                orgBaseExp = orgBaseExp.And(x => x.OrgCode == userInfoDto.OrganizationCode);
            }
            var query = from p in _orgBaseInfoRepository.GetQuery(orgBaseExp)
                join q in _fuWuShangRepository.GetQuery(fuWuShangExp)
                    on p.Id.ToString() equals q.BaseId
                join mospr in _materialListOfServiceProviderRepository.GetQuery(materialListOfServiceProvider)
                    on q.OrgCode equals mospr.OrgCode
                select new FuWuShangGetDto
                {
                    Id = p.Id,
                    LianXiRenPhone = q.LianXiRenPhone,
                    LianXiRenName = q.LianXiRenName,
                    JingYingQuYu = p.JingYingFanWei,
                    OrgName = q.OrgName,
                    OrgCode = q.OrgCode,
                    BeginTime = q.SYS_ChuangJianShiJian,
                    RegistrationStatus = mospr.ApprovalStatus.ToString()
                };
            return query.OrderByDescending(x => x.BeginTime).ToList();
            }
            catch (Exception e)
            {
                LogHelper.Error("导出查询第三方机构列表失败"+e.Message,e);
                return new List<FuWuShangGetDto>();
            }
        }

        public override void Dispose()
        {
            _fuWuShangRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
            _materialListOfServiceProviderRepository.Dispose();
        }
    }
}
