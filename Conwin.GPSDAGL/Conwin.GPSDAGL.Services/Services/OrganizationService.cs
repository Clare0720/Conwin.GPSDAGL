using Conwin.EntityFramework;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class OrganizationService : ApiServiceBase, IOrganizationService
    {
        #region 构造函数
        private readonly string sysId = string.Empty;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly ICheLiangRepository _cheLiangRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        private readonly IQiYeFuWuShangGuanLianXinXiRepository _qiYeFuWuShangGuanLianXinXiRepository;
        public OrganizationService(
            IOrgBaseInfoRepository orgBaseInfoRepository,
            ICheLiangRepository cheLiangRepository,
            IQiYeFuWuShangGuanLianXinXiRepository qiYeFuWuShangGuanLianXinXiRepository,
            ICheLiangYeHuRepository cheLiangYeHuRepository,
        IBussinessLogger bussinessLogger) : base(bussinessLogger)
        {
            sysId = base.SysId;
            _cheLiangRepository = cheLiangRepository;
            _qiYeFuWuShangGuanLianXinXiRepository = qiYeFuWuShangGuanLianXinXiRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }
        #endregion

        #region 删除组织

        /// <summary>
        /// 删除组织信息
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="ids">组织基本信息 id</param>
        /// <returns></returns>
        public ServiceResult<bool> Delete(string sysid,string reqid, Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "不存在要删除的组织记录，请重新选择" };
                }
                if(string.IsNullOrWhiteSpace(sysid))
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "系统ID不能为空" };
                }
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                var idArr = new List<string>();
                foreach (Guid id in ids)
                {
                    idArr.Add(id.ToString());
                }
                var orgBaseInfos = _orgBaseInfoRepository.GetQuery(u => ids.Contains(u.Id) && u.SYS_XiTongZhuangTai == sysZhengChang);
                var baseIdList = orgBaseInfos.Select(x => x.Id.ToString()).ToList();
                var yhList = _cheLiangYeHuRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0).Where(x => baseIdList.Contains(x.BaseId)).ToList();
                if (orgBaseInfos.Count() <= 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "不存在要删除的组织记录，请重新选择" };
                }

                var systemOrgInfoList = from p in orgBaseInfos
                                        select new Dtos.SystemOrgInfoDto
                                        {
                                            SysId = sysid,
                                            OrganizationName = p.OrgName,
                                            OrganizationType = p.OrgType,
                                            OrganizationCode = p.OrgCode,
                                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                                            SYS_ZuiJinXiuGaiRenID = userInfo.Id
                                        };
                //是否删除系统组织关联账户
                bool IsDeleteSysOrgInfo = true;
                //删除运输企业
                if (systemOrgInfoList.FirstOrDefault()?.OrganizationType == (int)OrganizationType.企业 || systemOrgInfoList.FirstOrDefault()?.OrganizationType == (int)OrganizationType.个体户)
                {
                    var cheLiangQuery = from a in systemOrgInfoList
                                        join b in _cheLiangRepository.GetQuery(u => u.SYS_XiTongZhuangTai == sysZhengChang)
                                        on a.OrganizationCode equals b.YeHuOrgCode
                                        select b.Id;
                    if (cheLiangQuery.Count() > 0)
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "选择的企业存在车辆信息，无法删除" };
                    }
                }
                //删除平台代理商档案
                if (systemOrgInfoList.FirstOrDefault()?.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    //创建时未注册系统组织信息
                    IsDeleteSysOrgInfo = false;

                    var guanLianXinXi = from a in systemOrgInfoList
                                        join b in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                                        on a.OrganizationCode equals b.FuWuShangCode
                                        select b.Id;
                    if (guanLianXinXi.Count() > 0)
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "选择的服务商存在与企业的关联信息，无法删除" };
                    }
                }
                #region 删除相关组织的用户帐号
                if (IsDeleteSysOrgInfo)
                {
                    var getShanChuResponse = GetInvokeRequest("00000030035", "1.0", new
                    {
                        SystemOrganizationInfo = systemOrgInfoList.ToList()
                    });
                    if (getShanChuResponse.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = getShanChuResponse.publicresponse.message };
                    }
                    if (IsPropertyExist(getShanChuResponse.body))
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "删除相关组织的用户帐号出错" };
                    }
                }
                #endregion
                //var yhInfos = _cheLiangYeHuRepository.GetQuery(x => idArr.Contains(x.BaseId) && x.SYS_XiTongZhuangTai == 0);
                #region 删除组织基本信息
                bool updateResult = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    foreach (OrgBaseInfo d in orgBaseInfos)
                    {
                        d.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _orgBaseInfoRepository.Update(d);
                    }
                    foreach (CheLiangYeHu d in yhList)
                    {
                        d.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _cheLiangYeHuRepository.Update(d);
                    }
                    updateResult = uow.CommitTransaction() > 0;
                }
                if (!updateResult)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "删除组织基本信息失败" };
                }
                #endregion


                #region 业务日志
                UserInfoDtoNew UserInfo = GetUserInfo();
                var logList = orgBaseInfos.ToList();
                foreach (var model in logList)
                {
                    AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                    {
                        ReqId = reqid,
                        YeWuDuiXiangLeiXing = "企业GPS基础档案",
                        YeWuDuiXiangZiLei = string.Format("{0}", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuDuiXiangID = model.Id,
                        YeWuDuiXiangBiaoZhi = model.OrgName,
                        YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(model),
                        YeWuLeiXing = "删除企业GPS基础档案",
                        YeWuZiLei = string.Format("删除{0}档案", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuChangJingLeiXing = "基础档案业务",
                        MoKuaiMingCheng = "企业GPS基础档案管理",
                        XiTongMingCheng = "企业GPS基础档案系统",
                        YingYongMingCheng = string.Format("{0}档案系统", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuGaiYaoXinXi = string.Format("删除档案：{0}", model.OrgName),
                    }, UserInfo);
                }
                #endregion

                return new ServiceResult<bool>() { Data = true };
            });
        }
        #endregion

        #region 停用
        public ServiceResult<bool> Cancel(string reqid, Guid[] ids)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "不存在要注销的组织记录，请重新选择" };
                }

                var idArr = new List<string>();
                foreach (Guid id in ids)
                {
                    idArr.Add(id.ToString());
                }

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                var orgBaseInfos = _orgBaseInfoRepository.GetQuery(u => ids.Contains(u.Id) && u.SYS_XiTongZhuangTai == sysZhengChang);
                var systemOrgInfoList = from p in orgBaseInfos
                                        select new Dtos.SystemOrgInfoDto
                                        {
                                            SysId = sysId,
                                            OrganizationName = p.OrgName,
                                            OrganizationType = p.OrgType,
                                            OrganizationCode = p.OrgCode,
                                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                                            SYS_ZuiJinXiuGaiRenID = userInfo.Id
                                        };

                #region 账户禁用
                var getZhuXiaoResponse = GetInvokeRequest("00000030034", "1.0", new
                {
                    IsActive = false,
                    SystemOrganizationInfo = systemOrgInfoList.ToList()
                });
                if (getZhuXiaoResponse.publicresponse.statuscode != 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.publicresponse.message };
                }
                if (getZhuXiaoResponse.body.success == false)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "禁用相关组织的用户帐号出错" };
                }
                #endregion

                #region 修改有效状态
                bool updateResult = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    foreach (OrgBaseInfo d in orgBaseInfos)
                    {
                        d.ZhuangTai = (int)ZhuangTaiEnum.合约到期;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _orgBaseInfoRepository.Update(d);
                    }
                    updateResult = uow.CommitTransaction() > 0;
                }
                if (!updateResult)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "修改组织有效状态失败" };
                }
                #endregion

                //业务日志
                var logList = _orgBaseInfoRepository.GetQuery(x => ids.Contains(x.Id)).ToList();

                foreach (var model in logList)
                {
                    AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                    {
                        ReqId = reqid,
                        YeWuDuiXiangLeiXing = "企业GPS基础档案",
                        YeWuDuiXiangZiLei = string.Format("{0}", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuDuiXiangID = model.Id,
                        YeWuDuiXiangBiaoZhi = model.OrgName,
                        YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(model),
                        YeWuLeiXing = "注销企业GPS基础档案",
                        YeWuZiLei = string.Format("注销{0}档案", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuChangJingLeiXing = "基础档案业务",
                        MoKuaiMingCheng = "企业GPS基础档案管理",
                        XiTongMingCheng = "企业GPS基础档案系统",
                        YingYongMingCheng = string.Format("{0}档案系统", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuGaiYaoXinXi = string.Format("注销档案：{0}", model.OrgName),
                    }, userInfo);
                }
                return new ServiceResult<bool>() { Data = true };

            });
        }
        #endregion

        #region 启用
        public ServiceResult<bool> Normal(string reqid, Guid[] ids)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "不存在要启用的组织记录，请重新选择" };
                }
                var idArr = new List<string>();
                foreach (Guid id in ids)
                {
                    idArr.Add(id.ToString());
                }

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                var orgBaseInfos = _orgBaseInfoRepository.GetQuery(u => ids.Contains(u.Id) && u.SYS_XiTongZhuangTai == sysZhengChang);
                var systemOrgInfoList = from p in orgBaseInfos
                                        select new Dtos.SystemOrgInfoDto
                                        {
                                            SysId = sysId,
                                            OrganizationName = p.OrgName,
                                            OrganizationType = p.OrgType,
                                            OrganizationCode = p.OrgCode,
                                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                                            SYS_ZuiJinXiuGaiRenID = userInfo.Id
                                        };

                //启用账户
                var getZhuXiaoResponse = GetInvokeRequest("00000030034", "1.0", new
                {
                    IsActive = true,
                    SystemOrganizationInfo = systemOrgInfoList.ToList()
                });
                if (getZhuXiaoResponse.publicresponse.statuscode != 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.publicresponse.message };
                }
                if (getZhuXiaoResponse.body.success == false)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "启用相关组织的用户帐号出错" };
                }

                #region 修改有效状态
                bool updateResult = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    foreach (OrgBaseInfo d in orgBaseInfos)
                    {
                        d.ZhuangTai = (int)ZhuangTaiEnum.正常营业;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _orgBaseInfoRepository.Update(d);
                    }
                    updateResult = uow.CommitTransaction() > 0;
                }
                if (!updateResult)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "修改组织有效状态失败" };
                }
                #endregion

                //业务日志
                var logList = _orgBaseInfoRepository.GetQuery(x => ids.Contains(x.Id)).ToList();

                foreach (var model in logList)
                {
                    AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                    {
                        ReqId = reqid,
                        YeWuDuiXiangLeiXing = "企业GPS基础档案",
                        YeWuDuiXiangZiLei = string.Format("{0}", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuDuiXiangID = model.Id,
                        YeWuDuiXiangBiaoZhi = model.OrgName,
                        YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(model),
                        YeWuLeiXing = "启用企业GPS基础档案",
                        YeWuZiLei = string.Format("启用{0}档案", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuChangJingLeiXing = "基础档案业务",
                        MoKuaiMingCheng = "企业GPS基础档案管理",
                        XiTongMingCheng = "企业GPS基础档案系统",
                        YingYongMingCheng = string.Format("{0}档案系统", Enum.GetName(typeof(OrganizationType), model.OrgType)),
                        YeWuGaiYaoXinXi = string.Format("启用档案：{0}", model.OrgName),
                    }, userInfo);
                }
                return new ServiceResult<bool>() { Data = true };
            });
        }
        #endregion

        public bool IsPropertyExist(dynamic data)
        {
            try
            {
                if (data.success == false)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public override void Dispose()
        {
            _orgBaseInfoRepository.Dispose();
        }
    }
}
