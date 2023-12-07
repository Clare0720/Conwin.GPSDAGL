using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities.PersonalInfo;
using Conwin.GPSDAGL.Services.DtosExt.PersonalInfo;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Common;
using Gma.QrCodeNet.Encoding.DataEncodation;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Text;
using Conwin.GPSDAGL.Services.Services;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Conwin.Framework.FileAgent;
using System.IO;
using Conwin.FileModule.ServiceAgent;
using System.Net.Http;
using Conwin.GPSDAGL.Framework.Elasticsearch;
using System.Net;
using System.Drawing;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.EnterpriseRegister;
using Conwin.GPSDAGL.Services.Enums;
using System.Net.Mail;
using Conwin.Framework.ServiceAgent.Utilities;

namespace Conwin.GPSDAGL.Services
{
    public partial class YeHuService : ApiServiceBase, IYeHuService
    {
        //企业信息
        private readonly ICheLiangYeHuRepository _yeHuRepository;
        //组织信息
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        //车辆信息
        private readonly ICheLiangRepository _cheLiangRepository;
        //企业服务商关联信息
        private readonly IQiYeFuWuShangGuanLianXinXiRepository _qiYeFuWuShangGuanLianXinXiRepository;
        //车辆业户关联信息
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;

        //企业注册信息
        private readonly IEnterpriseRegisterInfoRepository _enterpriseRegisterInfoRepository;
        private readonly string sysId = string.Empty;
        public YeHuService(ICheLiangYeHuRepository qiYeDangAnRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            ICheLiangRepository cheLiangRepository,
            ICheLiangYeHuRepository cheLiangYeHuRepository,
            IQiYeFuWuShangGuanLianXinXiRepository qiYeFuWuShangGuanLianXinXiRepository,
            IBussinessLogger _bussinessLogger,
            IEnterpriseRegisterInfoRepository enterpriseRegisterInfoRepository
        ) : base(_bussinessLogger)
        {
            _yeHuRepository = qiYeDangAnRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _cheLiangRepository = cheLiangRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _qiYeFuWuShangGuanLianXinXiRepository = qiYeFuWuShangGuanLianXinXiRepository;
            sysId = base.SysId;
            _enterpriseRegisterInfoRepository = enterpriseRegisterInfoRepository;
        }

        public ServiceResult<bool> Create(string sysID, CheLiangYeHuDto model, UserInfoDto userInfo)
        {

            //1）	根据企业名称来判定重复；
            //2）	新增企业时，同步增加用户管理中的组织机构，同步生成管理员账号密码机构账号为qyadmin+四位流水号，密码Gdunis1234；
            //3     新增企业调整：只添加用户基础信息，账号分配由注册功能分配
            return ExecuteCommandStruct<bool>(() =>
            {
                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var isExitModel = _orgBaseInfoRepository
                    .Count(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgName.Trim() == model.OrgName.Trim()
                                                                       && x.XiaQuXian == model.XiaQuXian&&x.OrgType==2);

                var yeHuEntity = _yeHuRepository
                    .Count(x => x.OrgName == model.OrgName && x.SYS_XiTongZhuangTai == 0 &&
                                x.QiYeXingZhi == model.QiYeXingZhi);
                if (isExitModel > 0 && yeHuEntity>0)
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false, StatusCode = 2,
                        ErrorMessage = $"已经存在名称为{model.OrgName}的企业，请使用其它名称"
                    };
                }

                model.ZhuangTai = (int) ZhuangTaiEnum.正常营业;
                model.ShenHeZhuangTai = (int) ShenHeZhuangTai.审核通过;
                var str = "yh";
                var OrgCode = str + EnterpriseCode();
                var enterprise = _yeHuRepository
                    .GetQuery(x => x.OrgCode == OrgCode && x.SYS_XiTongZhuangTai == 0)
                    .FirstOrDefault();
                if (enterprise != null)
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false,
                        StatusCode = 2,
                        ErrorMessage = $"企业编号重复，请重新保存"
                    };
                }
                model.OrgCode = OrgCode;
                var BaseId = model.Id.ToLower();
                //添加组织基础表
                var orgBaseDto = model;
                orgBaseDto.Id = BaseId;
                orgBaseDto.OrgCode = OrgCode;
                orgBaseDto.ChuangJianRenOrgCode = userInfo.OrganizationCode;
                orgBaseDto.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                var orgBaseModel = Mapper.Map<OrgBaseInfo>(orgBaseDto);
                orgBaseModel.Remark = model.BeiZhu;
                SetCreateSYSInfo(orgBaseModel, userInfo);
                //添加企业表
                var qiYe = Mapper.Map<CheLiangYeHu>(model);
                qiYe.Id = Guid.NewGuid();
                qiYe.BaseId = BaseId;
                qiYe.IsConfirmInfo = 0;
                SetCreateSYSInfo(qiYe, userInfo);
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _orgBaseInfoRepository.Add(orgBaseModel);
                    _yeHuRepository.Add(qiYe);
                    var addResult = uow.CommitTransaction() > 0;
                    return addResult ? new ServiceResult<bool>() {Data = true} : new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "创建企业档案出错了"};
                }
            });
        }



        public ServiceResult<bool> EnterpriseAudit(CheLiangYeHuDto model, UserInfoDto userInfo)
        {

            //1）	修改企业表  审核状态 改为 已通过
            return ExecuteCommandStruct<bool>(() =>
            {
                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                //对应企业
                var preEntity = _yeHuRepository
                    .GetQuery(x => x.Id == new Guid(model.Id) && x.SYS_XiTongZhuangTai == sysZhengChang)
                    .FirstOrDefault();

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (preEntity != null)
                    {
                        preEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        preEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        preEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _yeHuRepository.Update(preEntity);

                    }

                    var addResult = uow.CommitTransaction() > 0;
                    return addResult
                        ? new ServiceResult<bool>() {Data = true}
                        : new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "操作失败"};
                }
            });
        }

        public ServiceResult<bool> Update(string sysid, CheLiangYeHuDto model, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {

                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                Guid qiYeID = Guid.Parse(model.Id);
                var isExitModel = _orgBaseInfoRepository.Count(x =>
                    x.OrgName.Trim() == model.OrgName.Trim() && x.Id != qiYeID &&
                    x.SYS_XiTongZhuangTai == sysZhengChang);
                if (isExitModel > 0)
                {
                    return new ServiceResult<bool>()
                    {
                        Data = false, StatusCode = 2,
                        ErrorMessage = string.Format("已经存在名称为{0}的企业，请使用其它名称", model.OrgName)
                    };
                }

                //UserInfoDtoNew userInfoDto = GetUserInfo();

                var systemOrganizationInfoDto = new
                {
                    OrganizationCode = model.OrgCode,
                    OrganizationName = model.OrgName,
                    ManageArea = model.JingYingFanWei,
                    SysId = sysid,
                    Province = model.XiaQuSheng,
                    City = model.XiaQuShi,
                    District = model.XiaQuXian
                    /*, SYS_XiTongBeiZhu = model.YeHuDaiMa*/
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

                var preEntity = _orgBaseInfoRepository
                    .GetQuery(x => x.Id == new Guid(model.Id) && x.SYS_XiTongZhuangTai == sysZhengChang)
                    .FirstOrDefault();
                preEntity.OrgName = model.OrgName;
                preEntity.OrgShortName = model.OrgShortName;
                preEntity.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                preEntity.Remark = model.BeiZhu;
                preEntity.JingYingFanWei = model.JingYingFanWei;
                preEntity.XiaQuSheng = model.XiaQuSheng;
                preEntity.XiaQuShi = model.XiaQuShi;
                preEntity.XiaQuXian = model.XiaQuXian;
                preEntity.DiZhi = model.DiZhi;
                preEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                preEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                preEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;

                var yh = _yeHuRepository.GetQuery(s => s.BaseId == model.Id && s.SYS_XiTongZhuangTai == sysZhengChang)
                    .FirstOrDefault();
                yh.OrgName = model.OrgName;
                yh.OrgShortName = model.OrgShortName;
                yh.JingJiLeiXing = model.JingJiLeiXing;
                yh.SuoShuJianKongPingTai = model.SuoShuJianKongPingTai;
                yh.SuoShuQiYe = model.SuoShuQiYe;
                yh.QiYeXingZhi = model.QiYeXingZhi;
                yh.LianXiRen = model.LianXiRen;
                if (model.QiYeBiaoZhiId != null)
                {
                    yh.QiYeBiaoZhiId = model.QiYeBiaoZhiId;
                }

                if (model.IsConfirmInfo == 1)
                {
                    yh.IsConfirmInfo = 1;
                }

                yh.LianXiFangShi = model.LianXiFangShi;
                yh.ChuanZhenHaoMa = model.ChuanZhenHaoMa;
                yh.GongShangYingYeZhiZhaoHao = model.GongShangYingYeZhiZhaoHao;
                yh.GongShangYingYeZhiZhaoYouXiaoQi = model.GongShangYingYeZhiZhaoYouXiaoQi;
                yh.GongShangYingYeZhiZhaoChangQiYouXiao = model.GongShangYingYeZhiZhaoChangQiYouXiao;
                yh.JingYingXuKeZhengHao = model.JingYingXuKeZhengHao;
                yh.JingYingXuKeZhengYouXiaoQi = model.JingYingXuKeZhengYouXiaoQi;
                yh.JingYingXuKeZhengChangQiYouXiao = model.JingYingXuKeZhengChangQiYouXiao;
                yh.SheHuiXinYongDaiMa = model.SheHuiXinYongDaiMa;
                yh.GeTiHuShenFenZhengHaoMa = model.GeTiHuShenFenZhengHaoMa;
                yh.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                yh.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                yh.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //组织基础表
                    _orgBaseInfoRepository.Update(preEntity);
                    //企业表
                    _yeHuRepository.Update(yh);

                    var updateRsult = uow.CommitTransaction() > 0;
                    if (updateRsult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "修改企业档案出错了"};
                    }
                }
            });
        }

        public ServiceResult<bool> Delete(Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "不存在要删除的企业记录，请重新选择"};
                }

                //存在车辆信息无法删除,车辆信息
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var cheLiangQuery = from a in _yeHuRepository.GetQuery(u => ids.Contains(u.Id))
                    join b in _cheLiangRepository.GetQuery(u => u.SYS_XiTongZhuangTai == sysZhengChang)
                        on a.OrgCode equals b.YeHuOrgCode
                    select a.Id;

                if (cheLiangQuery.Count() > 0)
                {
                    return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "选择的企业存在车辆信息，无法删除"};
                }

                var qiYeQuery =
                    from p in _yeHuRepository.GetQuery(
                        u => ids.Contains(u.Id) && u.SYS_XiTongZhuangTai == sysZhengChang)
                    select new Dtos.SystemOrgInfoDto
                    {
                        SysId = sysId,
                        OrganizationName = p.OrgName,
                        OrganizationType = p.OrgType,
                        OrganizationCode = p.OrgCode,
                        SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                        SYS_ZuiJinXiuGaiRenID = userInfo.Id
                    };
                var qiYeList = qiYeQuery.ToList();

                if (qiYeList != null && qiYeList.Count() > 0)
                {
                    var getShanChuResponse = GetInvokeRequest("00000030035", "1.0", new
                    {
                        SystemOrganizationInfo = qiYeList.ToList()
                    });
                    if (getShanChuResponse.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getShanChuResponse.publicresponse.message};
                    }

                    if (getShanChuResponse.body == false)
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "删除相关组织的用户帐号出错"};
                    }
                }

                var orgBaseIds = _yeHuRepository.GetQuery(s => ids.Contains(s.Id)).Select(a => a.BaseId).ToList();
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _yeHuRepository.Update(u => ids.Contains(u.Id), SetDelSYSInfo(new CheLiangYeHu(), userInfo));
                    _orgBaseInfoRepository.Update(u => orgBaseIds.Contains(u.Id.ToString()),
                        SetDelSYSInfo(new OrgBaseInfo(), userInfo));

                    var updateRsult = uow.CommitTransaction() > 0;
                    if (updateRsult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "修改企业档案出错了"};
                    }
                }
            });
        }

        public ServiceResult<bool> Cancel(Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "不存在要注销的企业记录，请重新选择"};
                }

                var queryNotYingYeCount =
                    _orgBaseInfoRepository.Count(u => ids.Contains(u.Id) && u.ZhuangTai != (int) ZhuangTaiEnum.正常营业);
                if (queryNotYingYeCount > 0)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "选择的记录中存在不是正常营业的，请重新选择"};
                }

                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var qiYeQuery =
                    from q in _yeHuRepository.GetQuery(
                        u => u.SYS_XiTongZhuangTai == sysZhengChang && ids.Contains(u.Id))
                    select new Dtos.SystemOrgInfoDto
                    {
                        SysId = sysId,
                        OrganizationName = q.OrgName,
                        OrganizationType = q.OrgType,
                        OrganizationCode = q.OrgCode,
                        SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                        SYS_ZuiJinXiuGaiRenID = userInfo.Id
                    };

                var qiYeList = qiYeQuery.ToList();
                if (qiYeList.Count > 0)
                {
                    //账户禁用
                    var getZhuXiaoResponse = GetInvokeRequest("00000030034", "1.0", new
                    {
                        IsActive = false,
                        SystemOrganizationInfo = qiYeList
                    });

                    if (getZhuXiaoResponse.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.body.msg};
                    }

                    if (getZhuXiaoResponse.body.success == false)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.body.msg};
                    }
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    _orgBaseInfoRepository.Update(u => ids.Contains(u.Id),
                        p => new OrgBaseInfo()
                        {
                            ZhuangTai = (int) ZhuangTaiEnum.合约到期,
                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                            SYS_ZuiJinXiuGaiRenID = userInfo.Id
                        });

                    var updateRsult = uow.CommitTransaction() > 0;
                    if (updateRsult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "修改企业档案出错了"};
                    }
                }
            });
        }

        public ServiceResult<CheLiangYeHuDto> GetByOrgCode(string orgCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orgCode))
                {
                    return new ServiceResult<CheLiangYeHuDto> {StatusCode = 2, ErrorMessage = "组织代码不能为空"};
                }

                var result = new ServiceResult<CheLiangYeHuDto>();
                var tm = from a in _orgBaseInfoRepository.GetQuery(m =>
                        m.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常 && m.OrgCode == orgCode &&
                        m.OrgType == (int) OrganizationType.企业)
                    join b in _yeHuRepository.GetQuery(m => m.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常)
                        on a.Id.ToString() equals b.BaseId
                    select new CheLiangYeHuDto
                    {
                        Id = a.Id.ToString(),
                        OrgName = a.OrgName,
                        OrgCode = a.OrgCode,
                        OrgShortName = a.OrgShortName,
                        JingYingFanWei = a.JingYingFanWei,
                        SuoShuJianKongPingTai = b.SuoShuJianKongPingTai,
                        XiaQuSheng = a.XiaQuSheng,
                        XiaQuShi = a.XiaQuShi,
                        XiaQuXian = a.XiaQuXian,
                        ZhuangTai = a.ZhuangTai,
                        QiYeXingZhi = b.QiYeXingZhi,
                        LianXiRen = b.LianXiRen,
                        LianXiFangShi = b.LianXiFangShi,
                        ChuanZhenHaoMa = b.ChuanZhenHaoMa,
                        DiZhi = a.DiZhi,
                        BeiZhu = a.Remark,
                        ShiFouGeTiHu = b.ShiFouGeTiHu,
                        QiYeBiaoZhiId = b.QiYeBiaoZhiId,
                        ShenHeZhuangTai = b.ShenHeZhuangTai,
                        JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                        JingYingXuKeZhengYouXiaoQi = b.JingYingXuKeZhengYouXiaoQi,
                        JingYingXuKeZhengChangQiYouXiao = b.JingYingXuKeZhengChangQiYouXiao,
                        GongShangYingYeZhiZhaoHao = b.GongShangYingYeZhiZhaoHao,
                        GongShangYingYeZhiZhaoYouXiaoQi = b.GongShangYingYeZhiZhaoYouXiaoQi,
                        GongShangYingYeZhiZhaoChangQiYouXiao = b.GongShangYingYeZhiZhaoChangQiYouXiao,
                        JingJiLeiXing = b.JingJiLeiXing,
                        SuoShuQiYe = b.SuoShuQiYe,
                    };
                var model = tm.FirstOrDefault();
                result.Data = model;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("根据组织代码获取组织信息出错" + ex.Message, ex);
                return new ServiceResult<CheLiangYeHuDto> {StatusCode = 2, ErrorMessage = "查询出错"};
            }
        }

        public ServiceResult<int> GetYeHuConfirmInfoStatus(string orgCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orgCode))
                {
                    return new ServiceResult<int> {StatusCode = 2, ErrorMessage = "组织代码不能为空"};
                }

                var status = _cheLiangYeHuRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常 && x.OrgCode == orgCode)
                    .FirstOrDefault()?.IsConfirmInfo;
                return new ServiceResult<int> {Data = Convert.ToInt32(status)};
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取企业确认信息状态出错" + ex.Message, ex);
                return new ServiceResult<int> {StatusCode = 2, ErrorMessage = "查询出错"};
            }
        }

        /// <summary>
        /// 启用企业
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="ids"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Normal(Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "不存在要启用的企业记录，请重新选择"};
                }

                var queryNotYingYeCount =
                    _orgBaseInfoRepository.Count(u => ids.Contains(u.Id) && u.ZhuangTai != (int) ZhuangTaiEnum.合约到期);
                if (queryNotYingYeCount > 0)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "选择的记录中存在不是合约到期的，请重新选择"};
                }

                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var qiYeQuery =
                    from q in _yeHuRepository.GetQuery(
                        u => u.SYS_XiTongZhuangTai == sysZhengChang && ids.Contains(u.Id))
                    select new Dtos.SystemOrgInfoDto
                    {
                        SysId = sysId,
                        OrganizationName = q.OrgName,
                        OrganizationType = q.OrgType,
                        OrganizationCode = q.OrgCode,
                        SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                        SYS_ZuiJinXiuGaiRenID = userInfo.Id
                    };

                var qiYeList = qiYeQuery.ToList();
                if (qiYeList.Count > 0)
                {
                    //账户禁用
                    var getZhuXiaoResponse = GetInvokeRequest("00000030034", "1.0", new
                    {
                        IsActive = true,
                        SystemOrganizationInfo = qiYeList
                    });

                    if (getZhuXiaoResponse.publicresponse.statuscode != 0)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.body.msg};
                    }

                    if (getZhuXiaoResponse.body.success == false)
                    {
                        return new ServiceResult<bool>()
                            {Data = false, StatusCode = 2, ErrorMessage = getZhuXiaoResponse.body.msg};
                    }
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    _orgBaseInfoRepository.Update(u => ids.Contains(u.Id),
                        p => new OrgBaseInfo()
                        {
                            ZhuangTai = (int) ZhuangTaiEnum.正常营业,
                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                            SYS_ZuiJinXiuGaiRenID = userInfo.Id
                        });

                    var updateRsult = uow.CommitTransaction() > 0;
                    if (updateRsult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "修改企业档案出错了"};
                    }
                }
            });
        }

        public ServiceResult<CheLiangYeHuDto> Get(Guid id)
        {
            var result = new ServiceResult<CheLiangYeHuDto>();
            var tm = from a in _orgBaseInfoRepository.GetQuery(m =>
                    m.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常 && m.Id == id)
                join b in _yeHuRepository.GetQuery(m => m.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常)
                    on a.Id.ToString() equals b.BaseId
                select new CheLiangYeHuDto
                {
                    Id = a.Id.ToString(),
                    OrgName = a.OrgName,
                    OrgCode = a.OrgCode,
                    OrgShortName = a.OrgShortName,
                    JingYingFanWei = a.JingYingFanWei,
                    //KongGuGongSiMingCheng = b.KongGuGongSiMingCheng,
                    //KongGuGongSiSuoZaiXiaQuSheng = b.KongGuGongSiSuoZaiXiaQuSheng,
                    //KongGuGongSiSuoZaiXiaQuShi = b.KongGuGongSiSuoZaiXiaQuShi,
                    //JingJiLeiXing = b.JingJiLeiXing,
                    SuoShuJianKongPingTai = b.SuoShuJianKongPingTai,
                    XiaQuSheng = a.XiaQuSheng,
                    XiaQuShi = a.XiaQuShi,
                    XiaQuXian = a.XiaQuXian,
                    ZhuangTai = a.ZhuangTai,
                    QiYeXingZhi = b.QiYeXingZhi,
                    LianXiRen = b.LianXiRen,
                    LianXiFangShi = b.LianXiFangShi,
                    ChuanZhenHaoMa = b.ChuanZhenHaoMa,
                    DiZhi = a.DiZhi,
                    //YeHuDaiMa = b.YeHuDaiMa,
                    BeiZhu = a.Remark,
                    ShiFouGeTiHu = b.ShiFouGeTiHu,
                    QiYeBiaoZhiId = b.QiYeBiaoZhiId,
                    ShenHeZhuangTai = b.ShenHeZhuangTai,
                    JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                    JingYingXuKeZhengYouXiaoQi = b.JingYingXuKeZhengYouXiaoQi,
                    JingYingXuKeZhengChangQiYouXiao = b.JingYingXuKeZhengChangQiYouXiao,
                    GongShangYingYeZhiZhaoHao = b.GongShangYingYeZhiZhaoHao,
                    GongShangYingYeZhiZhaoYouXiaoQi = b.GongShangYingYeZhiZhaoYouXiaoQi,
                    GongShangYingYeZhiZhaoChangQiYouXiao = b.GongShangYingYeZhiZhaoChangQiYouXiao,
                    JingJiLeiXing = b.JingJiLeiXing,
                    SuoShuQiYe = b.SuoShuQiYe,
                    SheHuiXinYongDaiMa = b.SheHuiXinYongDaiMa,
                    GeTiHuShenFenZhengHaoMa = b.GeTiHuShenFenZhengHaoMa
                    //JingYingXuKeZhengYouXiaoZhuangTai = b.JingYingXuKeZhengYouXiaoZhuangTai,
                    //JingYingXuKeZhengZi = b.JingYingXuKeZhengZi
                };
            var model = tm.FirstOrDefault();
            //var lianXiRenQuery = _lianXiRenXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && m.LeiBie == 2 && m.BenDanWeiOrgCode == mainList.BenDanWeiOrgCode).FirstOrDefault();
            //if (lianXiRenQuery != null)
            //{
            //    model.FuZheRen = lianXiRenQuery.LianXiRen;
            //    model.FuZheRenDianHua = lianXiRenQuery.ShouJiHaoMa;
            //}
            //else
            //{
            //    model.FuZheRen = "暂未添加负责人";
            //    model.FuZheRenDianHua = "暂未添加负责人";
            //}
            model.FuZheRen = "暂未添加负责人";
            model.FuZheRenDianHua = "暂未添加负责人";
            result.Data = model;
            return result;
        }

        public ServiceResult<QueryResult> Query(QueryData queryData, UserInfoDto userInfo)
        {

            try
            {
                UserInfoDtoNew userInfoDto = GetUserInfo();
                if (userInfoDto == null)
                {
                    return new ServiceResult<QueryResult> {ErrorMessage = "获取登录用户失败，请重新登录", StatusCode = 2};
                }

                GetQiYeInfoDto returnData = GetQiYeQueryInfoList(queryData, userInfoDto);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> {Data = result};
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业档案出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> {StatusCode = 2, ErrorMessage = "查询出错"};
            }


        }

        public ServiceResult<QueryResult> QueryAll(QueryData queryData)
        {

            try
            {
                GetQiYeInfoDto returnData = GetQiYeQueryInfoAllList(queryData);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> {Data = result};
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> {StatusCode = 2, ErrorMessage = "查询出错"};
            }
        }

        private GetQiYeInfoDto GetQiYeQueryInfoList(QueryData queryData, UserInfoDtoNew userInfoDto)
        {
            GetQiYeInfoDto returnData = new GetQiYeInfoDto();
            QiyeSearch searchDto =
                JsonConvert.DeserializeObject<QiyeSearch>(JsonConvert.SerializeObject(queryData.data));
            int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiangYeHu, bool>> YeHuExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            if (!string.IsNullOrEmpty(searchDto.YeHuMingCheng))
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgName.Contains(searchDto.YeHuMingCheng.Trim()));
            }

            if (!string.IsNullOrEmpty(searchDto.ShenHeZhuangTai))
            {
                int shenHeZhuangTai = Convert.ToInt32(searchDto.ShenHeZhuangTai);
                YeHuExp = YeHuExp.And(u => u.ShenHeZhuangTai == shenHeZhuangTai);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.OrgCode))
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgCode == searchDto.OrgCode.Trim());
            }

            if (!string.IsNullOrEmpty(searchDto.YouXiaoZhuangTai))
            {
                int youXiaoZhuangTai = Convert.ToInt32(searchDto.YouXiaoZhuangTai);
                OrgBaseExp = OrgBaseExp.And(u => u.ZhuangTai == youXiaoZhuangTai);
            }

            if (searchDto.OrgType != null)
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgType == searchDto.OrgType);
            }

            if (userInfoDto.OrganizationType == (int) OrganizationType.市政府)
            {
                OrgBaseExp = OrgBaseExp.And(x =>
                    x.XiaQuShi == userInfoDto.OrganCity || x.JingYingFanWei.Contains(userInfoDto.OrganCity));
            }

            if (userInfoDto.OrganizationType == (int) OrganizationType.县政府)
            {
                OrgBaseExp = OrgBaseExp.And(x =>
                    x.XiaQuXian == userInfoDto.OrganDistrict ||
                    x.JingYingFanWei.Contains(userInfoDto.OrganCity + userInfoDto.OrganDistrict));
            }

            if (userInfoDto.OrganizationType == (int) OrganizationType.企业)
            {
                OrgBaseExp = OrgBaseExp.And(x => x.OrgCode == userInfoDto.OrganizationCode);
            }

            if (userInfoDto.OrganizationType == (int) OrganizationType.本地服务商)
            {
                OrgBaseExp = OrgBaseExp.And(x =>
                    x.XiaQuShi == userInfoDto.OrganCity || x.JingYingFanWei.Contains(userInfoDto.OrganCity));
            }

            if (userInfoDto.OrganizationType == (int) OrganizationType.街道企业管理组)
            {
                OrgBaseExp = OrgBaseExp.And(x => x.Street == userInfoDto.OrganTown);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.XiaQuShi))
            {
                OrgBaseExp = OrgBaseExp.And(x => x.XiaQuShi == searchDto.XiaQuShi.Trim());
            }

            if (!string.IsNullOrWhiteSpace(searchDto.XiaQuXian))
            {
                OrgBaseExp = OrgBaseExp.And(x => x.XiaQuXian == searchDto.XiaQuXian.Trim());
            }

            if (!string.IsNullOrWhiteSpace(searchDto.JingYingXuKeZhengHao))
            {
                YeHuExp = YeHuExp.And(x => x.JingYingXuKeZhengHao.Contains(searchDto.JingYingXuKeZhengHao.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.LianXiRen))
            {
                YeHuExp = YeHuExp.And(x => x.LianXiRen == searchDto.LianXiRen);
            }

            int approvalStatus=0;
            if (!string.IsNullOrWhiteSpace(searchDto.RegistrationStatus))
            {
                approvalStatus = int.Parse(searchDto.RegistrationStatus);
            }
            var query = from a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                join b in _yeHuRepository.GetQuery(YeHuExp)
                    on a.Id.ToString() equals b.BaseId
                join epr in _enterpriseRegisterInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                    on b.OrgCode equals epr.OrgCode
                    into temp1
                from te1 in temp1.DefaultIfEmpty()
                where string.IsNullOrEmpty(searchDto.RegistrationStatus) || te1.ApprovalStatus== approvalStatus
                select new QueryQiYeResponseDto
                   {
                    Id = a.Id,
                    OrgName = b.OrgName,
                    OrgCode = b.OrgCode,
                    JingYingFanWei = a.JingYingFanWei,
                    ZhuangTai = a.ZhuangTai,
                    ShenHeZhuangTai = b.ShenHeZhuangTai,
                    XiaQuSheng = a.XiaQuSheng,
                    XiaQuShi = a.XiaQuShi,
                    XiaQuXian = a.XiaQuXian,
                    JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                    ChuangJianShiJian = a.SYS_ChuangJianShiJian,
                    LianXiRen = b.LianXiRen,
                    LianXiFangShi = b.LianXiFangShi,
                    QiYeGuanLiYuan = "",
                    IsHavCar = 0,
                    JingYingQuYu = a.JingYingFanWei,
                    RegistrationStatus = te1.ApprovalStatus,
                    EnterpriseId = b.Id.ToString(),
                    QiYeXingZhi=b.QiYeXingZhi
                };
            returnData.Count = query.Count();
            //分页
            returnData.list = query.OrderByDescending(x => x.ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();

            return returnData;
        }

        private GetQiYeInfoDto GetQiYeQueryInfoAllList(QueryData queryData)
        {
            GetQiYeInfoDto returnData = new GetQiYeInfoDto();
            QiyeSearch searchDto =
                JsonConvert.DeserializeObject<QiyeSearch>(JsonConvert.SerializeObject(queryData.data));
            int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiangYeHu, bool>> YeHuExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;
            if (!string.IsNullOrEmpty(searchDto.YeHuMingCheng))
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgName.Contains(searchDto.YeHuMingCheng.Trim()));
            }

            if (!string.IsNullOrEmpty(searchDto.ShenHeZhuangTai))
            {
                int shenHeZhuangTai = Convert.ToInt32(searchDto.ShenHeZhuangTai);
                YeHuExp = YeHuExp.And(u => u.ShenHeZhuangTai == shenHeZhuangTai);
            }

            if (!string.IsNullOrEmpty(searchDto.YouXiaoZhuangTai))
            {
                int youXiaoZhuangTai = Convert.ToInt32(searchDto.YouXiaoZhuangTai);
                OrgBaseExp = OrgBaseExp.And(u => u.ZhuangTai == youXiaoZhuangTai);
            }

            if (searchDto.OrgType != null)
            {
                OrgBaseExp = OrgBaseExp.And(u => u.OrgType == searchDto.OrgType);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.XiaQuShi))
            {
                OrgBaseExp = OrgBaseExp.And(x => x.XiaQuShi == searchDto.XiaQuShi.Trim());
            }

            if (!string.IsNullOrWhiteSpace(searchDto.XiaQuXian))
            {
                OrgBaseExp = OrgBaseExp.And(x => x.XiaQuXian == searchDto.XiaQuXian.Trim());
            }

            if (!string.IsNullOrWhiteSpace(searchDto.JingYingXuKeZhengHao))
            {
                YeHuExp = YeHuExp.And(x => x.JingYingXuKeZhengHao.Contains(searchDto.JingYingXuKeZhengHao.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.LianXiRen))
            {
                YeHuExp = YeHuExp.And(x => x.LianXiRen == searchDto.LianXiRen);
            }

            if (!string.IsNullOrWhiteSpace(searchDto.ShouJiHaoMa))
            {
                YeHuExp = YeHuExp.And(m => m.LianXiFangShi.Contains(searchDto.ShouJiHaoMa.Trim()));
            }

            var query = from a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                join b in _yeHuRepository.GetQuery(YeHuExp)
                    on a.Id.ToString() equals b.BaseId
                select new QueryQiYeResponseDto
                {
                    Id = a.Id,
                    OrgName = b.OrgName,
                    OrgCode = b.OrgCode,
                    JingYingFanWei = a.JingYingFanWei,
                    ZhuangTai = a.ZhuangTai,
                    ShenHeZhuangTai = b.ShenHeZhuangTai,
                    XiaQuSheng = a.XiaQuSheng,
                    XiaQuShi = a.XiaQuShi,
                    XiaQuXian = a.XiaQuXian,
                    JingYingXuKeZhengHao = b.JingYingXuKeZhengHao,
                    ChuangJianShiJian = a.SYS_ChuangJianShiJian,
                    LianXiRen = b.LianXiRen,
                    LianXiFangShi = b.LianXiFangShi,
                    QiYeGuanLiYuan = "",
                    IsHavCar = 0,
                };

            returnData.Count = query.Count();
            //分页
            returnData.list = query.OrderByDescending(x => x.ChuangJianShiJian)
                .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();

            return returnData;
        }

        public ServiceResult<ExportResponseInfoDto> ExportQiYeXinXi(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2};
                }

                var list = GetQiYeQueryInfoList(queryData, userInfo).list;
                string tableTitle = "企业档案" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count() > 0)
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
                        LogHelper.Error("导出企业档案出错" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "导出出错", StatusCode = 2};
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> {StatusCode = 2, ErrorMessage = "没有需要导出的数据"};

                }


            }
            catch (Exception ex)
            {
                LogHelper.Error("导出企业档案出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> {ErrorMessage = "导出出错", StatusCode = 2};
            }
        }

        #region 导出企业档案

        private static Guid? CreateQiYeDangAnExcelAndUpload(List<QueryQiYeResponseDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "企业档案";
            string[] cellTitleArry = {"企业名称", "经营许可证号", "辖区省", "辖区市", "辖区县", "联系人", "联系电话", "经营区域", "企业性质","审核状态" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
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
                    var index = 0;

                    row.CreateCell(index++).SetCellValue(item.OrgName);
                    row.CreateCell(index++).SetCellValue(item.JingYingXuKeZhengHao);
                    row.CreateCell(index++).SetCellValue(item.XiaQuSheng);
                    row.CreateCell(index++).SetCellValue(item.XiaQuShi);
                    row.CreateCell(index++).SetCellValue(item.XiaQuXian);
                    row.CreateCell(index++).SetCellValue(item.LianXiRen);
                    row.CreateCell(index++).SetCellValue(item.LianXiFangShi);
                    row.CreateCell(index++).SetCellValue(item.JingYingFanWei);
                    row.CreateCell(index++).SetCellValue(item.QiYeXingZhi);
                    //车辆种类
                    var registrationStatus = string.Empty;
                    if (item.RegistrationStatus.HasValue)
                    {
                        registrationStatus = typeof(RegisterInfoApprovalStatus).GetEnumName(item.RegistrationStatus);
                        ;
                    }
                    row.CreateCell(index++).SetCellValue(registrationStatus);
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



        public ServiceResult<object> GetQiYeXinXiByYingYeZhiZhaoHao(string yingYeZhiZhaoHao)
        {
            return ExecuteCommandClass<object>(() =>
            {
                var query = _yeHuRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常);
                if (!string.IsNullOrEmpty(yingYeZhiZhaoHao))
                {
                    query = query.Where(u => u.JingYingXuKeZhengHao == yingYeZhiZhaoHao);
                }

                var list = query.Select(u => new {u.OrgCode, u.OrgName}).ToList();
                return new ServiceResult<object>() {Data = list};
            });
        }

        // 人员档案 移动端 获取企业列表 2019-10-12 
        public ServiceResult<QueryResult> QueryForPersonalInfoMobile(QueryData queryData, UserInfoDto userInfo)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                QiyeSearch searchDto = JsonConvert.DeserializeObject<QiyeSearch>(queryData.data.ToString());
                Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q =>
                    q.ZhuangTai == (int) ZhuangTaiEnum.正常营业 && q.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常;
                Expression<Func<CheLiangYeHu, bool>> QiYeExp = q =>
                    q.ShenHeZhuangTai == (int) ShenHeZhuangTai.审核通过 &&
                    q.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常;
                ServiceResult<QueryResult> dataResult = new ServiceResult<QueryResult>();
                QueryResult result = new QueryResult();

                if (!string.IsNullOrWhiteSpace(searchDto.YeHuMingCheng))
                {
                    QiYeExp = QiYeExp.And(u => u.OrgName.Contains(searchDto.YeHuMingCheng.Trim()));
                }

                var list = from a in _orgBaseInfoRepository.GetQuery(OrgBaseExp)
                    join b in _yeHuRepository.GetQuery(QiYeExp)
                        on a.Id.ToString() equals b.BaseId
                    select new
                    {
                        Id = b.Id,
                        YeHuMingCheng = b.OrgName,
                        QiYeDaiMa = b.OrgCode,
                        XiaQuSheng = a.XiaQuSheng,
                        XiaQuShi = a.XiaQuShi,
                        a.XiaQuXian
                    };
                result.totalcount = list.Distinct().Count();
                result.items = list.Distinct().OrderBy(o => o.YeHuMingCheng).Skip((queryData.page - 1) * queryData.rows)
                    .Take(queryData.rows).ToList();
                dataResult.Data = result;
                return dataResult;
            });
        }

        // 企业档案管理，联系人，创建人员账号
        //public ServiceResult<bool> CreatePersonalInfoAccount(string[] ids, UserInfoDto userInfo)
        //{
        //    return ExecuteCommandStruct<bool>(() =>
        //    {
        //        ServiceResult<bool> result = new ServiceResult<bool>();
        //        result.Data = true;
        //        result.StatusCode = 0;
        //        if (userInfo == null)
        //        {
        //            result.Data = false;
        //            result.StatusCode = 2;
        //            result.ErrorMessage = "获取用户信息失败";
        //            return result;
        //        }
        //        try
        //        {
        //            if (ids != null && ids.Count() > 0)
        //            {
        //                using (var unitOfWork = new UnitOfWork("DefaultDb"))
        //                {
        //                    unitOfWork.BeginTransaction();
        //                    foreach (var id in ids)
        //                    {
        //                        if (_personalInfoService.Get(Guid.Parse(id)).Data == null)
        //                        {
        //                            LogHelper.Error("获取人员档案信息失败，数据ID：" + id);
        //                            result.StatusCode = 2;
        //                            result.ErrorMessage = "获取人员档案信息失败";
        //                            result.Data = false;
        //                            return result;
        //                        }
        //                        PersonalInfo personal = (PersonalInfo)_personalInfoService.Get(Guid.Parse(id)).Data;
        //                        PersonalExtInfoDto extInfo = new PersonalExtInfoDto();
        //                        if (!string.IsNullOrWhiteSpace(personal.ExtInfo))
        //                        {
        //                            extInfo = JsonConvert.DeserializeObject<PersonalExtInfoDto>(personal.ExtInfo);
        //                        }
        //                        PositionRolesHelper.Init();
        //                        var rolesMap = PositionRolesHelper.PositionRoles.RolesMap;
        //                        var roles = rolesMap[personal.Positions[0]];
        //                        if (string.IsNullOrWhiteSpace(extInfo.YeHuMingCheng) ||
        //                            string.IsNullOrWhiteSpace(extInfo.YeHuDaiMa) ||
        //                            string.IsNullOrWhiteSpace(personal.Cellphone) ||
        //                            string.IsNullOrWhiteSpace(roles))
        //                        {
        //                            LogHelper.Error($"创建用户账号失败，存在关键信息为空，业户名称：{extInfo.YeHuMingCheng},业户代码：{extInfo.YeHuDaiMa}，手机号码：{personal.Cellphone}，角色编码：{roles}");
        //                            result.StatusCode = 2;
        //                            result.ErrorMessage = "创建用户账号失败，企业名称、企业代码、手机号码、职务不可为空";
        //                            result.Data = false;
        //                            return result;
        //                        }
        //                        if (_geRenZhuCeRenZhengXinXiRepository.Count(u => u.SYS_XiTongZhuangTai == 0 && u.ShouJiHao == personal.Cellphone) > 0)
        //                        {
        //                            LogHelper.Error($"该用户已注册，注册手机号为{personal.Cellphone}");
        //                            result.StatusCode = 2;
        //                            result.ErrorMessage = $"{personal.Name}，手机号码：{personal.Cellphone}的账号已创建，无需重新创建！";
        //                            result.Data = false;
        //                            return result;
        //                        }
        //                        var YeHuMingCheng = extInfo.YeHuMingCheng;
        //                        Guid YeHuID = extInfo.YeHuId.HasValue ? extInfo.YeHuId.Value : Guid.Empty;
        //                        var OrgCode = extInfo.YeHuDaiMa;

        //                        #region 新增个人注册信息表
        //                        GeRenZhuCeRenZhengXinXi geRenZhuCeRenZhengXinXiNew = new GeRenZhuCeRenZhengXinXi
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            YeHuID = YeHuID,
        //                            YeHuMingCheng = YeHuMingCheng,
        //                            ShouJiHao = personal.Cellphone,
        //                            XingMing = personal.Name,
        //                            ShenFenZhengHaoMa = personal.IDCard,
        //                            ShenFenZhengZhaoId = personal.IDPhoto,
        //                            RenZhengZhuangTai = (int)QiYeRenZhengZhuangTai.已认证,
        //                            RenZhengWanChengShiJian = DateTime.Now,
        //                            RenZhengShenQinShiJian = DateTime.Now,
        //                            SYS_ChuangJianShiJian = DateTime.Now,
        //                            SYS_ShuJuLaiYuan = "企业档案联系人注册"
        //                        };
        //                        SetCreateSYSInfo(geRenZhuCeRenZhengXinXiNew, new UserInfoDto());
        //                        _geRenZhuCeRenZhengXinXiRepository.Add(geRenZhuCeRenZhengXinXiNew);
        //                        #endregion

        //                        #region 创建用户账号,账号为手机号，密码为手机号后六位
        //                        string errorMsg = string.Empty;
        //                        var organization = new { OrganizationName = extInfo.YeHuMingCheng, OrganizationCode = extInfo.YeHuDaiMa, OrganizationType = (int)OrgType.企业 };

        //                        var registerUserAccount = new
        //                        {
        //                            User = new { UserName = personal.Name, Telephone = string.Empty, Mobile = personal.Cellphone },
        //                            Organization = organization,
        //                            UserAccount = new { AccountName = personal.Cellphone, PassWord = personal.Cellphone.Substring(personal.Cellphone.Length - 6) },
        //                            RoleCode = roles,
        //                            SysId = sysId
        //                        };
        //                        var getRegisterResponse = GetInvokeRequest("00000030041", "1.0", registerUserAccount);
        //                        if (getRegisterResponse.publicresponse.statuscode != 0)
        //                        {
        //                            unitOfWork.RollBackTransaction();
        //                            LogHelper.Error("调用服务（00000030041-1.0）出错" + getRegisterResponse.publicresponse.message);
        //                            result.StatusCode = 2;
        //                            result.ErrorMessage = getRegisterResponse.publicresponse.message;
        //                            result.Data = false;
        //                            return result;
        //                        }
        //                        #endregion

        //                        #region 用户标签初始化
        //                        var initUserTagResponse = GetInvokeRequest("003300900007", "1.0", new { Name = personal.Name, Phone = personal.Cellphone, YeHuMingCheng = extInfo.YeHuMingCheng, ZhiWu = personal.Positions });
        //                        if (initUserTagResponse.publicresponse.statuscode != 0)
        //                        {
        //                            unitOfWork.RollBackTransaction();
        //                            LogHelper.Error("调用服务（003300900007-1.0）出错" + initUserTagResponse.publicresponse.message);
        //                            result.StatusCode = 2;
        //                            result.ErrorMessage = initUserTagResponse.publicresponse.message;
        //                            result.Data = false;
        //                            return result;
        //                        }
        //                        #endregion
        //                    }
        //                    result.Data = unitOfWork.CommitTransaction() > 0;
        //                    result.StatusCode = result.Data ? 0 : 2;
        //                    result.ErrorMessage = result.Data ? string.Empty : "创建失败";
        //                }
        //            }
        //            return result;
        //        }
        //        catch(Exception ex)
        //        {
        //            LogHelper.Error("创建人员账号失败：" + ex.ToString());
        //            result.Data = false;
        //            result.StatusCode = 2;
        //            result.ErrorMessage = "创建人员账号失败：" + ex.ToString();
        //            return result;
        //        }
        //    });
        //}

        public ServiceResult<QueryResult> InitializationOrgAndAdminUser()
        {
            ServiceResult<QueryResult> dataResult = new ServiceResult<QueryResult>();
            QueryResult result = new QueryResult();
            UserInfoDtoNew userInfoDto = GetUserInfo();

            var orgList = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0).ToList();

            foreach (var item in orgList)
            {
                var str = "qy";
                string OrgType = string.Format("{0}", item.OrgType);
                string UserName = str + "admin" + item.OrgCode.Substring(item.OrgCode.Length - 4, 4);
                string OrgCode = item.OrgCode;
                string RoleCode = string.Format("{0:000},{1:000}", (int) ZuZhiJueSe.GPS企业, (int) ZuZhiJueSe.车辆监控员);

                //用户模块服务00000030033新增系统组织、单位管理员和为管理员分配权限
                var getInitOrgAndSysUserResponse = GetInvokeRequest("00000030033", "1.0", new
                {
                    SysId = sysId,
                    SysOrgId = userInfoDto.OrganId,
                    item.OrgName,
                    OrgType,
                    OrgCode,
                    UserName, //TODO:生成序列号
                    RoleCode,
                    ManageArea = item.JingYingFanWei,
                    SYS_XiTongBeiZhu = OrgCode
                });

                //添加企业表
                var qiYe = Mapper.Map<CheLiangYeHu>(item);
                qiYe.Id = Guid.NewGuid();
                qiYe.BaseId = item.Id.ToString();
                qiYe.IsConfirmInfo = 0;
                SetCreateSYSInfo(qiYe, userInfoDto);


                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _yeHuRepository.Add(qiYe);
                    var addResultCount = uow.CommitTransaction();
                    result.totalcount = addResultCount;
                }
            }

            return dataResult;
        }


        public ServiceResult<bool> AddFuWuShangGuanLianXinXi(QiYeFuWuShangGuanLianXinXiDto model)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                UserInfoDtoNew userInfoDto = GetUserInfo();
                var qiye = _orgBaseInfoRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == model.QiYeCode &&
                    x.OrgType == (int) OrganizationType.企业).FirstOrDefault();
                if (qiye == null)
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "获取企业信息失败"};
                }

                var fuwushang = _orgBaseInfoRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgCode == model.FuWuShangCode &&
                    x.OrgType == (int) OrganizationType.本地服务商).FirstOrDefault();
                if (qiye == null)
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "获取服务商信息失败"};
                }

                var glxx = _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.QiYeCode == model.QiYeCode &&
                    x.FuWuShangCode == model.FuWuShangCode).FirstOrDefault();
                if (glxx != null)
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "已有该服务商关联信息"};
                }

                if (!IsNumber(model.LoginName))
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "登录名只能为纯数字"};
                }

                if (!string.IsNullOrWhiteSpace(model.LoginPassWord))
                {
                    if (model.LoginPassWord.Length > 8)
                    {
                        return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "密码长度不能大于8位"};
                    }
                }

                //获取平台接入码
                string jieRuMa = "";
                int jrmNumber = 0;
                using (IDbConnection connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
                {
                    string query =
                        $"INSERT INTO T_JianKongPingTaiSerialNumber(SYS_ChuangJianShiJian)VALUES('{DateTime.Now}');select Id= SCOPE_IDENTITY();";
                    jrmNumber = connection.ExecuteScalar<int>(query);
                }

                if (jrmNumber > 0)
                {
                    jieRuMa = "0066" + string.Format("{0:0000}", jrmNumber);
                }
                else
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "新增失败"};
                }

                QiYeFuWuShangGuanLianXinXi addModel = Mapper.Map<QiYeFuWuShangGuanLianXinXi>(model);
                addModel.Id = Guid.NewGuid();
                addModel.ZhuangTai = 0;
                addModel.PingTaiJieRuMa = jieRuMa;

                //政府单位添加主链路IP和端口后修改关联状态
                if (!string.IsNullOrWhiteSpace(addModel.ZhuLianLuIP) && addModel.ZhuLianLuDuanKou != null)
                {
                    addModel.ZhuangTai = 1;
                }

                SetCreateSYSInfo(addModel, userInfoDto);

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _qiYeFuWuShangGuanLianXinXiRepository.Add(addModel);

                    var addResult = uow.CommitTransaction() > 0;
                    if (addResult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "添加关联信息失败"};
                    }
                }
            });
        }

        //判断字符串是否为纯数字
        public static bool IsNumber(string str)
        {
            if (str == null || str.Length == 0) //验证这个参数是否为空
                return false;
            ASCIIEncoding ascii = new ASCIIEncoding(); //new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str); //把string类型的参数保存到数组里

            foreach (byte c in bytestr) //遍历这个数组里的内容
            {
                if (c < 48 || c > 57) //判断是否为数字
                {
                    return false;
                }
            }

            return true;
        }

        public ServiceResult<bool> EditFuWuShangGuanLianXinXi(QiYeFuWuShangGuanLianXinXiDto model)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                UserInfoDtoNew userInfoDto = GetUserInfo();
                Guid id = Guid.Parse(model.Id);
                var glxx = _qiYeFuWuShangGuanLianXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.Id == id).FirstOrDefault();
                if (glxx == null)
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "获取服务商关联信息失败"};
                }

                if (!IsNumber(model.LoginName))
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "登录名只能为纯数字"};
                }

                if (!string.IsNullOrWhiteSpace(model.LoginPassWord))
                {
                    if (model.LoginPassWord.Length > 8)
                    {
                        return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "密码长度不能大于8位"};
                    }
                }

                switch (userInfoDto.OrganizationType)
                {
                    case (int) OrganizationType.市政府:
                    case (int) OrganizationType.县政府:
                    case (int) OrganizationType.平台运营商:
                        glxx.ZhuLianLuIP = model.ZhuLianLuIP;
                        glxx.ZhuLianLuDuanKou = model.ZhuLianLuDuanKou;
                        break;

                }

                glxx.XiaQuSheng = model.XiaQuSheng;
                glxx.XiaQuShi = model.XiaQuShi;
                glxx.XiaQuXian = model.XiaQuXian;
                glxx.CongLianLuIP = model.CongLianLuIP;
                glxx.CongLianLuDuanKou = model.CongLianLuDuanKou;
                glxx.PingTaiJieRuMa = model.PingTaiJieRuMa;
                glxx.LoginName = model.LoginName;
                glxx.LoginPassWord = model.LoginPassWord;
                glxx.M1 = model.M1;
                glxx.IA1 = model.IA1;
                glxx.IC1 = model.IC1;
                glxx.SYS_ZuiJinXiuGaiRen = userInfoDto.UserName;
                glxx.SYS_ZuiJinXiuGaiRenID = userInfoDto.Id;
                glxx.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                //政府单位编辑主链路IP和端口后修改关联状态
                if (!string.IsNullOrWhiteSpace(glxx.ZhuLianLuIP) && glxx.ZhuLianLuDuanKou != null &&
                    glxx.ZhuangTai != 1)
                {
                    glxx.ZhuangTai = 1;
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _qiYeFuWuShangGuanLianXinXiRepository.Update(glxx);

                    var addResult = uow.CommitTransaction() > 0;
                    if (addResult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "修改关联信息失败"};
                    }
                }
            });
        }

        public ServiceResult<bool> DeleteFuWuShangGuanLianXinXi(Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;

                if (ids.Count() <= 0)
                {
                    return new ServiceResult<bool>()
                        {Data = false, StatusCode = 2, ErrorMessage = "不存在要删除的服务商关联信息，请重新选择"};
                }

                var glxxList = _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && ids.Contains(x.Id));
                if (glxxList.Count() <= 0)
                {
                    return new ServiceResult<bool>
                        {Data = false, StatusCode = 2, ErrorMessage = "不存在要删除的服务商关联信息，请重新选择"};
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    foreach (var item in glxxList)
                    {
                        item.SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.作废;
                        item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _qiYeFuWuShangGuanLianXinXiRepository.Update(item);
                    }

                    var updateRsult = uow.CommitTransaction() > 0;
                    if (updateRsult)
                    {
                        return new ServiceResult<bool>() {Data = true};
                    }
                    else
                    {
                        return new ServiceResult<bool>() {Data = false, StatusCode = 2, ErrorMessage = "删除的服务商关联信息出错了"};
                    }
                }
            });
        }

        public ServiceResult<QueryResult> QueryFuWuShangGuanLianXinXi(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                QiYeFuWuShangGuanLianXinXiDto search =
                    JsonConvert.DeserializeObject<QiYeFuWuShangGuanLianXinXiDto>(queryData.data.ToString());
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var list = from gl in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang && x.QiYeCode == search.QiYeCode)
                    join fws in _orgBaseInfoRepository.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgType == (int) OrganizationType.本地服务商)
                        on gl.FuWuShangCode equals fws.OrgCode
                    select new
                    {
                        gl.Id,
                        FuWuShangMingCheng = fws.OrgName,
                        gl.FuWuShangCode,
                        gl.ZhuLianLuIP,
                        gl.ZhuLianLuDuanKou,
                        gl.CongLianLuIP,
                        gl.CongLianLuDuanKou,
                        gl.XiaQuSheng,
                        gl.XiaQuShi,
                        gl.XiaQuXian,
                        gl.ZhuangTai,
                    };
                QueryResult result = new QueryResult();
                result.totalcount = list.Count();
                result.items = list.Distinct().OrderBy(o => o.FuWuShangMingCheng)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows);
                return new ServiceResult<QueryResult>() {Data = result};
            });
        }

        //根据条件查询对应的企业监控平台信息
        public ServiceResult<QueryResult> ConditionQueryFuWuShangGuanLianXinXi(QueryData queryData)
        {
            QiYeFuWuShangGuanLianXinXiDto search =
                JsonConvert.DeserializeObject<QiYeFuWuShangGuanLianXinXiDto>(queryData.data.ToString());
            Expression<Func<QiYeFuWuShangGuanLianXinXi, bool>> exp = x => x.SYS_XiTongZhuangTai == 0;
            //平台接入码
            if (!string.IsNullOrWhiteSpace(search.PingTaiJieRuMa.Trim()))
            {
                exp = exp.And(x => x.PingTaiJieRuMa.Contains(search.PingTaiJieRuMa.Trim()));
            }

            ;
            //企业代码
            if (!string.IsNullOrWhiteSpace(search.QiYeCode.Trim()))
            {
                exp = exp.And(x => x.QiYeCode.Contains(search.QiYeCode.Trim()));
            }

            ;
            //服务商代码
            if (!string.IsNullOrWhiteSpace(search.FuWuShangCode.Trim()))
            {
                exp = exp.And(x => x.FuWuShangCode.Contains(search.FuWuShangCode.Trim()));
            }

            ;
            //主链路IP
            if (!string.IsNullOrWhiteSpace(search.ZhuLianLuIP.Trim()))
            {
                exp = exp.And(x => x.ZhuLianLuIP.Contains(search.ZhuLianLuIP.Trim()));
            }

            ;
            //主链路端口
            if (search.ZhuLianLuDuanKou != null)
            {
                exp = exp.And(x => x.ZhuLianLuDuanKou == search.ZhuLianLuDuanKou);
            }

            ;
            //从链路IP
            if (!string.IsNullOrWhiteSpace(search.CongLianLuIP.Trim()))
            {
                exp = exp.And(x => x.CongLianLuIP.Contains(search.CongLianLuIP.Trim()));
            }

            ;
            //从链路端口
            if (search.CongLianLuDuanKou != null)
            {
                exp = exp.And(x => x.CongLianLuDuanKou == search.CongLianLuDuanKou);
            }

            ;
            //辖区省
            if (!string.IsNullOrWhiteSpace(search.XiaQuSheng.Trim()))
            {
                exp = exp.And(x => x.XiaQuSheng == search.XiaQuSheng.Trim());
            }

            ;
            //辖区市
            if (!string.IsNullOrWhiteSpace(search.XiaQuShi.Trim()))
            {
                exp = exp.And(x => x.XiaQuShi == search.XiaQuShi.Trim());
            }

            ;
            //辖区县
            if (!string.IsNullOrWhiteSpace(search.XiaQuXian.Trim()))
            {
                exp = exp.And(x => x.XiaQuXian == search.XiaQuXian.Trim());
            }

            ;
            //登录名
            if (!string.IsNullOrWhiteSpace(search.LoginName.Trim()))
            {
                exp = exp.And(x => x.LoginName.Contains(search.LoginName.Trim()));
            }

            ;
            //M1
            if (search.M1 != null)
            {
                exp = exp.And(x => x.M1 == search.M1);
            }

            ;
            //IA1
            if (search.IA1 != null)
            {
                exp = exp.And(x => x.IA1 == search.IA1);
            }

            ;
            //IC1
            if (search.IC1 != null)
            {
                exp = exp.And(x => x.IC1 == search.IC1);
            }

            ;
            //状态
            if (search.ZhuangTai != null)
            {
                exp = exp.And(x => x.ZhuangTai == search.ZhuangTai);
            }

            ;
            var list = from jk in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(exp)
                join org in _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0)
                    on jk.FuWuShangCode equals org.OrgCode
                select new
                {
                    jk.Id,
                    jk.QiYeCode,
                    jk.FuWuShangCode,
                    FuWuShangMingCheng = org.OrgName,
                    jk.ZhuLianLuIP,
                    jk.ZhuLianLuDuanKou,
                    jk.CongLianLuIP,
                    jk.CongLianLuDuanKou,
                    jk.XiaQuSheng,
                    jk.XiaQuShi,
                    jk.XiaQuXian,
                    jk.PingTaiJieRuMa,
                    jk.LoginName,
                    jk.LoginPassWord,
                    jk.M1,
                    jk.IA1,
                    jk.IC1,
                    jk.ZhuangTai,

                };
            QueryResult result = new QueryResult();
            result.totalcount = list.Count();
            if (result.totalcount > 0)
            {
                result.items = list.Distinct().OrderByDescending(o => o.PingTaiJieRuMa)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows);
            }

            return new ServiceResult<QueryResult>() {Data = result};
        }


        public ServiceResult<object> GetFuWuShangGuanLianXinXi(Guid id)
        {
            return ExecuteCommandClass<object>(() =>
            {
                int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var list = (from gl in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == sysZhengChang && x.Id == id)
                    join fws in _orgBaseInfoRepository.GetQuery(x =>
                            x.SYS_XiTongZhuangTai == sysZhengChang && x.OrgType == (int) OrganizationType.本地服务商)
                        on gl.FuWuShangCode equals fws.OrgCode
                    select new
                    {
                        gl.Id,
                        FuWuShangMingCheng = fws.OrgName,
                        gl.ZhuLianLuIP,
                        gl.ZhuLianLuDuanKou,
                        gl.CongLianLuIP,
                        gl.CongLianLuDuanKou,
                        gl.XiaQuSheng,
                        gl.XiaQuShi,
                        gl.XiaQuXian,
                        gl.LoginName,
                        gl.LoginPassWord,
                        gl.PingTaiJieRuMa,
                        gl.M1,
                        gl.IA1,
                        gl.IC1
                    }).FirstOrDefault();

                return new ServiceResult<object>() {Data = list};
            });
        }

        //根据辖区查询对应的企业关联信息
        public ServiceResult<QueryResult> QueryForJianKongPingTaiXinXi(QueryData queryData)
        {
            QueryForJianKongPingTaiXinXiDto search =
                JsonConvert.DeserializeObject<QueryForJianKongPingTaiXinXiDto>(queryData.data.ToString());
            int sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
            UserInfoDtoNew userInfoDto = GetUserInfo();
            Expression<Func<QiYeFuWuShangGuanLianXinXi, bool>> jkptExp = x =>
                x.SYS_XiTongZhuangTai == sysZhengChang && x.ZhuangTai == 1;
            Expression<Func<OrgBaseInfo, bool>> orgExp = x => x.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<OrgBaseInfo, bool>> qyExp = x => x.SYS_XiTongZhuangTai == sysZhengChang;

            if (!string.IsNullOrWhiteSpace(userInfoDto.OrganCity))
            {
                qyExp = qyExp.And(x => x.XiaQuShi == userInfoDto.OrganCity);
            }

            if (!string.IsNullOrWhiteSpace(userInfoDto.OrganDistrict) &&
                userInfoDto.OrganizationType != (int) OrganizationType.市政府)
            {
                qyExp = qyExp.And(x => x.XiaQuXian == userInfoDto.OrganDistrict);
            }

            if (!string.IsNullOrWhiteSpace(search.OrgName))
            {
                orgExp = orgExp.And(x => x.OrgName.Contains(search.OrgName));
            }

            //查询关联信息和服务商名称
            var glxxList =
                from gl in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == sysZhengChang && x.ZhuangTai == 1)
                join fws in _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                    on gl.FuWuShangCode equals fws.OrgCode
                select new
                {
                    gl.QiYeCode,
                    gl.FuWuShangCode,
                    FuWuShangMingCheng = fws.OrgName,
                    gl.ZhuLianLuIP,
                    gl.ZhuLianLuDuanKou,
                    gl.CongLianLuIP,
                    gl.CongLianLuDuanKou,
                    gl.XiaQuSheng,
                    gl.XiaQuShi,
                    gl.XiaQuXian,
                    gl.PingTaiJieRuMa,
                    gl.LoginName,
                    gl.LoginPassWord,
                    gl.M1,
                    gl.IA1,
                    gl.IC1,
                    gl.ZhuangTai,
                };
            //关联信息分组
            var qiYelist = from jk in _qiYeFuWuShangGuanLianXinXiRepository.GetQuery(jkptExp)
                join qy in _orgBaseInfoRepository.GetQuery(qyExp)
                    on jk.QiYeCode equals qy.OrgCode
                group new {qy.XiaQuShi, qy.XiaQuXian, qy.OrgCode, jk.ZhuangTai} by new
                    {qy.XiaQuShi, qy.XiaQuXian, qy.OrgCode, jk.ZhuangTai}
                into t1
                select t1;
            //返回分组后的信息
            var list = from qy in qiYelist.OrderByDescending(u => u.Key.OrgCode)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows)
                join org in _orgBaseInfoRepository.GetQuery(orgExp)
                    on qy.Key.OrgCode equals org.OrgCode
                join yh in _cheLiangYeHuRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                    on qy.Key.OrgCode equals yh.OrgCode
                select new
                {
                    org.OrgName,
                    org.OrgCode,
                    qy.Key.XiaQuShi,
                    qy.Key.XiaQuXian,
                    yh.JingYingXuKeZhengHao,
                    qy.Key.ZhuangTai,
                    JianKongPingTaiList = glxxList.Where(x => x.QiYeCode == qy.Key.OrgCode).Select(x => new
                    {
                        x.QiYeCode,
                        org.OrgName,
                        x.FuWuShangCode,
                        x.FuWuShangMingCheng,
                        x.ZhuLianLuIP,
                        x.ZhuLianLuDuanKou,
                        x.CongLianLuIP,
                        x.CongLianLuDuanKou,
                        x.XiaQuSheng,
                        x.XiaQuShi,
                        x.XiaQuXian,
                        x.PingTaiJieRuMa,
                        x.LoginName,
                        x.LoginPassWord,
                        x.M1,
                        x.IA1,
                        x.IC1,
                        x.ZhuangTai,
                    }),
                };

            QueryResult result = new QueryResult();
            result.totalcount = qiYelist.Count();
            result.items = list;
            return new ServiceResult<QueryResult> {Data = result};
        }


        public override void Dispose()
        {
            //_yeHuRepository.Dispose();
            //_orgBaseInfoRepository.Dispose();
            //_cheLiangZuZhiXinXiRepository.Dispose();
        }

        public ServiceResult<object> GetJiaShiYuanXinXi(string idCard)
        {
            return ExecuteCommandClass<object>(() =>
            {
                string sysZhengChang = XiTongZhuangTaiEnum.正常.ToString();
                CongYeRenYuanDto dto = null;
                using (IDbConnection connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["TMC"].ConnectionString))
                {
                    string query = $@"SELECT TBD_DaoLuYunShuCongYeRenYuanTaiZhang.XingMing ,
                                   TBD_DaoLuYunShuCongYeRenYuanTaiZhang.XingBie ,
                                   TBD_DaoLuYunShuCongYeRenYuanTaiZhang.ShenFenZhengHaoMa ,
                                   TBD_DaoLuYunShuCongYeRenYuanTaiZhang.CongYeZiGeZhengYouXiaoRiQi ,
                                   TBD_DaoLuYunShuCongYeRenYuanTaiZhang.ZhaoPian ,
                                   TBD_DaoLuYunShuCongYeRenYuanTaiZhang.ZhuangTai
                                   FROM dbo.TBD_DaoLuYunShuCongYeRenYuanTaiZhang
                                    WHERE TBD_DaoLuYunShuCongYeRenYuanTaiZhang.ShenFenZhengHaoMa = '{idCard}' AND TBD_DaoLuYunShuCongYeRenYuanTaiZhang.SYS_XiTongZhuangTai = '{sysZhengChang}'";
                    dto = Dapper.SqlMapper.Query<DaoLuYunShuCongYeRenYuanTaiZhang>(connection, query).Select(ry =>
                        new CongYeRenYuanDto
                        {
                            XingMing = ry.XingMing,
                            XingBie = ry.XingBie,
                            ShenFenZhengHaoMa = ry.ShenFenZhengHaoMa,
                            CongYeZiGeZhengYouXiaoRiQi = ry.CongYeZiGeZhengYouXiaoRiQi,
                            ZhaoPian = ry.ZhaoPian,
                            ZhuangTai = ry.ZhuangTai,
                            base64 = ""
                        }).FirstOrDefault();
                }

                HttpClient client = new HttpClient();
                var address = ConfigurationManager.AppSettings["WaiWangAddress"] + "path=" + dto.ZhaoPian;
                var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, address)).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var htmlText = response.Content.ReadAsStringAsync().Result;
                    var imgUrls = GetHtmlImageUrlList(htmlText);
                    if (imgUrls.Length > 0)
                    {
                        var address1 = ConfigurationManager.AppSettings["WaiWangHost"] +
                                       imgUrls[0].Replace("../", "").Replace("&amp;", "&");
                        dto.base64 = ImageToBase64(address1);
                    }
                }

                return new ServiceResult<object>() {Data = dto};
            });
        }

        /// <summary>
        /// 获取Img的路径
        /// </summary>
        /// <param name="htmlText">Html字符串文本</param>
        /// <returns>以数组形式返回图片路径</returns>
        public static string[] GetHtmlImageUrlList(string htmlText)
        {
            Regex regImg =
                new Regex(
                    @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>",
                    RegexOptions.IgnoreCase);
            //新建一个matches的MatchCollection对象 保存 匹配对象个数(img标签)
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            //遍历所有的img标签对象
            foreach (Match match in matches)
            {
                //获取所有Img的路径src,并保存到数组中
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            }

            return sUrlList;
        }

        #region Image To base64

        public static Image UrlToImage(string url)
        {
            WebClient mywebclient = new WebClient();
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);
                return outputImg;
            }
        }

        /// <summary>
        /// Image 转成 base64
        /// </summary>
        /// <param name="fileFullName"></param>
        public static string ImageToBase64(Image img)
        {
            try
            {
                Bitmap bmp = new Bitmap(img);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int) ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ImageToBase64(string url)
        {
            return ImageToBase64(UrlToImage(url));
        }

        #endregion


        #region 同步指定企业信息

        public ServiceResult<bool> QiYeSynchronization(QiYeDataSynDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto?.OrgCode))
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "业户代码不能为空"};
                }

                string url = ConfigurationManager.AppSettings["HangYeYunServiceGateway"];
                CWRequest request = new CWRequest();
                CWPublicRequest publicRequest = new CWPublicRequest
                {
                    reqid = Guid.NewGuid().ToString(),
                    servicecode = "006600200131",
                    protover = "1.0",
                    servicever = "1.0",
                    requesttime = DateTime.Now.ToString()
                };
                request.publicrequest = publicRequest;
                request.body = new
                {
                    Page = 1,
                    Rows = 1,
                    data = new {OrgCode = dto.OrgCode.Trim()},
                };
                ServiceHttpHelper httpHelper = new ServiceHttpHelper();
                string resp = httpHelper.Post(url, request, "a0051da8-8eaa-42f9-b3ca-b25ae2469efd");
                CWResponse response =
                    JsonConvert.DeserializeObject<CWResponse>(JsonConvert.DeserializeObject(resp).ToString());
                if (response.publicresponse.statuscode != 0)
                {
                    LogHelper.Error("查询企业数据出错" + JsonConvert.SerializeObject(response));
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "同步失败"};
                }

                if (response.body == null || response.body.totalcount <= 0)
                {
                    return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "该业户未备案"};
                }

                List<QiYeInfoResponseDto> qiYeModelList =
                    JsonConvert.DeserializeObject<List<QiYeInfoResponseDto>>(
                        JsonConvert.SerializeObject(response.body.items));

                var model = qiYeModelList.FirstOrDefault();
                if (model != null)
                {
                    bool isCreateOrgBaseInfo = false;
                    bool isCreateYeHuInfo = false;
                    OrgBaseInfo OrgBaseModel = null;
                    CheLiangYeHu cheLiangYeHuModel = null;
                    //新增或修改基础档案
                    var dangAnOrgBase = _orgBaseInfoRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == model.YeHuDaiMa).FirstOrDefault();
                    if (dangAnOrgBase == null)
                    {
                        OrgBaseModel = new OrgBaseInfo
                        {
                            Id = Guid.NewGuid(),
                            OrgCode = model.YeHuDaiMa,
                            OrgName = model.YeHuMingCheng,
                            OrgShortName = model.YeHuJianCheng,
                            JingYingFanWei = "广东" + model.XiaQuShi,
                            YeWuJingYingFanWei = model.JingYingFanWei,
                            OrgType = (int) OrganizationType.企业,
                            XiaQuSheng = "广东",
                            XiaQuShi = model.XiaQuShi,
                            XiaQuXian = model.XiaQuXian,
                            DiZhi = model.DiZhi,
                            ZhuangTai = (int) ZhuangTaiEnum.正常营业,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常,
                            SYS_ShuJuLaiYuan = "手动同步运政信息",
                        };
                        isCreateOrgBaseInfo = true;
                    }
                    else
                    {
                        dangAnOrgBase.OrgName = model.YeHuMingCheng;
                        dangAnOrgBase.OrgShortName = model.YeHuJianCheng;
                        dangAnOrgBase.JingYingFanWei = "广东" + model.XiaQuShi;
                        dangAnOrgBase.YeWuJingYingFanWei = model.JingYingFanWei;
                        dangAnOrgBase.OrgType = (int) OrganizationType.企业;
                        dangAnOrgBase.XiaQuSheng = "广东";
                        dangAnOrgBase.XiaQuShi = model.XiaQuShi;
                        dangAnOrgBase.XiaQuXian = model.XiaQuXian;
                        dangAnOrgBase.DiZhi = model.DiZhi;
                        dangAnOrgBase.ZhuangTai = (int) ZhuangTaiEnum.正常营业;
                        dangAnOrgBase.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        dangAnOrgBase.SYS_ShuJuLaiYuan = "手动同步运政信息";
                        OrgBaseModel = dangAnOrgBase;
                    }

                    //新增或修改业户档案
                    var yehuModel = _cheLiangYeHuRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == model.YeHuDaiMa).FirstOrDefault();
                    if (yehuModel == null)
                    {
                        cheLiangYeHuModel = new CheLiangYeHu
                        {
                            Id = Guid.NewGuid(),
                            BaseId = OrgBaseModel.Id.ToString(),
                            OrgCode = model.YeHuDaiMa,
                            OrgName = model.YeHuMingCheng,
                            OrgShortName = model.YeHuJianCheng,
                            JingYingXuKeZhengHao = model.JingYingXuKeZhengHao,
                            JingJiLeiXing = model.JingJiLeiXing,
                            ChuanZhenHaoMa = model.ChuanZhen,
                            LianXiRen = model.LianXiRen,
                            LianXiFangShi = model.LianXiDianHua,
                            IsConfirmInfo = 0,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常,
                            SYS_ShuJuLaiYuan = "手动同步运政信息",
                        };
                        isCreateYeHuInfo = true;
                        yehuModel = cheLiangYeHuModel;
                    }
                    else
                    {
                        yehuModel.BaseId = OrgBaseModel.Id.ToString();
                        yehuModel.OrgCode = model.YeHuDaiMa;
                        yehuModel.OrgName = model.YeHuMingCheng;
                        yehuModel.OrgShortName = model.YeHuJianCheng;
                        yehuModel.JingYingXuKeZhengHao = model.JingYingXuKeZhengHao;
                        yehuModel.JingJiLeiXing = model.JingJiLeiXing;
                        yehuModel.ChuanZhenHaoMa = model.ChuanZhen;
                        yehuModel.LianXiRen = model.LianXiRen;
                        yehuModel.LianXiFangShi = model.LianXiDianHua;
                        yehuModel.IsConfirmInfo = 0;
                        yehuModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        yehuModel.SYS_ShuJuLaiYuan = "手动同步运政信息";
                    }

                    bool isSuccess = true;
                    using (var unw = new UnitOfWork())
                    {
                        unw.BeginTransaction();

                        if (isCreateOrgBaseInfo)
                        {
                            _orgBaseInfoRepository.Add(OrgBaseModel);
                        }
                        else
                        {
                            _orgBaseInfoRepository.Update(OrgBaseModel);
                        }

                        if (isCreateYeHuInfo)
                        {
                            _yeHuRepository.Add(yehuModel);
                        }
                        else
                        {
                            _yeHuRepository.Update(yehuModel);
                        }

                        isSuccess = unw.CommitTransaction() > 0;
                    }

                    return new ServiceResult<bool> {Data = isSuccess};
                }

                return new ServiceResult<bool> {Data = true};
            }
            catch (Exception ex)
            {
                LogHelper.Error("手动同步企业信息出错" + ex.Message, ex);
                return new ServiceResult<bool> {Data = false, StatusCode = 2, ErrorMessage = "同步出错"};
            }
        }


        #endregion

        #region 企业信息导入相关接口

        public ServiceResult<bool> ImportUpdate(ImportFuWu dto)
        {
            var result = new ServiceResult<bool>() {Data = true};
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Field))
                {
                    result.StatusCode = 2;
                    result.Data = false;
                    result.ErrorMessage = "企业信息导入失败，请选择文件";
                    return result;
                }

                var b = FileAgentUtility.GetFileData(new Guid(dto.Field));
                var list = ImportMapToObjectEx(b);
                if (list == null || !list.Any())
                {
                    result.StatusCode = 2;
                    result.Data = false;
                    result.ErrorMessage = "企业信息导入失败，表格内容为空";
                    return result;
                }

                #region 数据格式化

                foreach (var entity in list)
                {
                    switch (entity.XiaQuXian)
                    {
                        case "清新区":
                            entity.XiaQuXian = "清新";
                            break;
                        case "英德市":
                            entity.XiaQuXian = "英德";
                            break;
                        case "连州市":
                            entity.XiaQuXian = "连州";
                            break;
                        case "佛冈县":
                            entity.XiaQuXian = "佛冈";
                            break;
                        case "连山壮族瑶族自治县":
                            entity.XiaQuXian = "连山";
                            break;
                        case "连南瑶族自治县":
                            entity.XiaQuXian = "连南";
                            break;
                        case "阳山县":
                            entity.XiaQuXian = "阳山";
                            break;
                        case "清城区":
                            entity.XiaQuXian = "清城";
                            break;
                    }
                }

                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var enterpriseList = _cheLiangYeHuRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0).ToList();
                var starring = new[] {"清新", "英德", "连州", "佛冈", "连山", "连南", "阳山", "清城"};
                var cheLiangQuery = _cheLiangRepository.GetQuery(u => u.SYS_XiTongZhuangTai == sysZhengChang).ToList();
                //新运政企业映射编号
                var enterpriseSynchronizationList = EnterpriseSynchronizationList();
                foreach (var enterpriseEntity in list)
                {
                    if (string.IsNullOrWhiteSpace(enterpriseEntity.YeHuMingCheng) || string.IsNullOrWhiteSpace(
                                                                                      enterpriseEntity.YeHuDaiMa)
                                                                                  || string.IsNullOrWhiteSpace(
                                                                                      enterpriseEntity.XiaQuShi) ||
                                                                                  string.IsNullOrWhiteSpace(
                                                                                      enterpriseEntity.XiaQuXian)
                    )
                    {
                        continue;
                    }

                    if (enterpriseEntity.XiaQuShi != "清远市" && enterpriseEntity.XiaQuShi != "清远")
                    {
                        continue;
                    }

                    //只接受清远下的九个区数据
                    var xiaQuXian = starring.Contains(enterpriseEntity.XiaQuXian);
                    if (!xiaQuXian)
                    {
                        continue;
                    }

                    enterpriseEntity.XiaQuShi = "清远";
                    var enterpriseEntityCode = enterpriseEntity.YeHuDaiMa;

                    var enterpriseSynchronization = enterpriseSynchronizationList.FirstOrDefault(x =>
                        x.EnterpriseId == enterpriseEntity.YeHuDaiMa && x.OrgName == enterpriseEntity.YeHuMingCheng);
                    //业户编号
                    var synchronizationstate = false;
                    if (enterpriseSynchronization != null)
                    {
                        enterpriseEntityCode = enterpriseSynchronization.OrgCode;
                        synchronizationstate = true;
                    }

                    //业户信息
                    var enterpriseDate = enterpriseList.FirstOrDefault(x =>
                        x.OrgName == enterpriseEntity.YeHuMingCheng &&
                        x.JingYingXuKeZhengHao == enterpriseEntity.JingYingXuKeZhengHao);
                    if (enterpriseDate != null)
                    {
                        //同步企业地址
                        var orgGuid = new Guid(enterpriseDate.BaseId);
                        var enterprise = _orgBaseInfoRepository.GetByKey(orgGuid);
                        if (enterprise != null)
                        {
                            enterprise.DiZhi = enterpriseEntity.DiZhi;
                            enterprise.YeWuJingYingFanWei = enterpriseEntity.JingYingFanWei;
                            enterprise.JingYingFanWei = "广东清远";
                            EnterpriseUpdateInformation(enterprise);
                        }


                        //if (enterpriseEntity.SYS_XiTongZhuangTai != "注销") continue;
                        //var enterpriseVehicles =
                        //    cheLiangQuery.FirstOrDefault(x => x.YeHuOrgCode == enterpriseEntity.YeHuDaiMa);
                        ////存在绑定的车辆暂不删除
                        //if (enterpriseVehicles != null)
                        //{
                        //    continue;
                        //}
                        ////删除企业
                        //EnterpriseUpdate(enterpriseDate);
                        ////删除企业用户
                        //UserDelete(enterpriseDate);
                    }
                    else
                    {
                        if (enterpriseEntity.SYS_XiTongZhuangTai != "营业") continue;
                        var enterpriseName =
                            enterpriseList.FirstOrDefault(x => x.OrgName == enterpriseEntity.YeHuMingCheng);
                        var enterpriseCode =
                            enterpriseList.FirstOrDefault(x => x.OrgCode == enterpriseEntityCode);
                        if (enterpriseName != null || enterpriseCode != null)
                        {
                            continue;
                        }

                        //只新增经营许可证不相同的数据
                        var businessLicense = enterpriseList.FirstOrDefault(x =>
                            x.JingYingXuKeZhengHao == enterpriseEntity.JingYingXuKeZhengHao);
                        if (businessLicense != null)
                        {
                            continue;
                        }

                        //是否为guid类型
                        var guidState = Guid.TryParse(enterpriseEntityCode, out Guid data);
                        if (guidState)
                        {
                            enterpriseEntity.YeHuDaiMa = "yh" + EnterpriseCode();
                            var enterprise = _yeHuRepository
                                .GetQuery(x => x.OrgCode == enterpriseEntity.YeHuDaiMa && x.SYS_XiTongZhuangTai == 0)
                                .FirstOrDefault();
                            if (enterprise != null)
                            {
                                result.StatusCode = 2;
                                result.Data = false;
                                result.ErrorMessage = "企业编号重复，请重新导入";
                                return result;
                            }
                        }

                        EnterpriseAdd(enterpriseEntity);
                        //企业编号映射数据不存在
                        if (!synchronizationstate && guidState)
                        {
                            EnterpriseSynchronizationAdd(enterpriseEntity, enterpriseEntityCode);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.Data = false;
                result.ErrorMessage = "企业信息导入失败" + ex.Message;
            }

            return result;
        }

        //excel文件转实体
        private List<ImportVehicleContactInfoDto> ImportMapToObjectEx(byte[] buff)
        {
            Stream s = new MemoryStream(buff);
            var wk = WorkbookFactory.Create(s);
            var sheet = wk.GetSheetAt(0);
            var row = sheet.GetRow(0);
            #region 格式校验
            var dictionary = new Dictionary<string, string> {
                {"企业名称","YeHuMingCheng"},
                {"业户代码","YeHuDaiMa"},
                {"辖区市","XiaQuShi"},
                {"辖区县","XiaQuXian"},
                {"业户经营范围","JingYingFanWei"},
                {"地址","DiZhi"},
                {"经营许可证号","JingYingXuKeZhengHao"},
                {"联系人","LianXiRen"},
                {"联系人电话","LianXiDianHua"},
                {"传真","ChuanZhen"},
                {"状态","SYS_XiTongZhuangTai"}
            };
            foreach (var r in row)
            {
                if (!dictionary.ContainsKey(r.ToString()))
                {
                    return null;
                }
            }
            #endregion
            var list = new List<ImportVehicleContactInfoDto>();
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                var importDto = new ImportVehicleContactInfoDto
                {

                    YeHuMingCheng = row.GetCell(0)?.ToString(),
                    YeHuDaiMa = row.GetCell(1)?.ToString(),
                    XiaQuShi = row.GetCell(2)?.ToString(),
                    XiaQuXian = row.GetCell(3)?.ToString(),
                    JingYingFanWei = row.GetCell(4)?.ToString(),
                    DiZhi = row.GetCell(5)?.ToString(),
                    JingYingXuKeZhengHao = row.GetCell(6)?.ToString(),
                    LianXiRen = row.GetCell(7)?.ToString(),
                    LianXiDianHua = row.GetCell(8)?.ToString(),
                    ChuanZhen = row.GetCell(9)?.ToString(),
                    SYS_XiTongZhuangTai = row.GetCell(10)?.ToString()
                };
                list.Add(importDto);
            }
            return list;
        }

        /// <summary>
        /// 企业同步数据信息表
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseSynchronizationTable> EnterpriseSynchronizationList()
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var sql = @"SELECT Id,OrgCode,OrgName,EnterpriseId  FROM [DC_GPSJCDAGL].[dbo].[T_EnterpriseSynchronizationTable] WITH (NOLOCK) where  SYS_XiTongZhuangTai=0";
                    var enterpriseTerminalList = conn.Query<EnterpriseSynchronizationTable>(sql, null).ToList();
                    return enterpriseTerminalList;
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"企业基础同步信息获取失败" + ex.Message);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 获取企业序列号
        /// </summary>
        /// <returns></returns>
        public string GetEnterpriseCode()
        {
            try
            {
                var getSNoResponse = GetInvokeRequest("00000020013", "1.0", new
                {
                    SysId = "60190FC4-5103-4C76-94E4-12A54B62C92A", //写死，勿动
                    Module = "00660021",
                    Type = 2
                });
                if (getSNoResponse.publicresponse.statuscode != 0)
                {
                    LogHelper.Error($"获取序列号出错了");
                }
                var str = "yh";
                string Sno = getSNoResponse.body.SNo.ToString().PadLeft(4, '0');
                return str + Sno;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"序列号信息获取失败" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 新增业户编号映射数据
        /// </summary>
        /// <param name="enterpriseDate"></param>
        /// <param name="enterpriseId"></param>
        public void EnterpriseSynchronizationAdd(ImportVehicleContactInfoDto enterpriseDate, string enterpriseId)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var time = new
                    {
                        Id = Guid.NewGuid(),
                        OrgName = enterpriseDate.YeHuMingCheng,
                        OrgCode = enterpriseDate.YeHuDaiMa,
                        EnterpriseId = enterpriseId
                    };

                    var sql = @"
INSERT INTO [DC_GPSJCDAGL].[dbo].[T_EnterpriseSynchronizationTable]
           ([Id]
           ,[OrgCode]
           ,[EnterpriseId]
           ,[OrgName]
           ,[SYS_ShuJuLaiYuan]
           ,[SYS_ChuangJianRenID]
           ,[SYS_ChuangJianRen]
           ,[SYS_ChuangJianShiJian]
           ,[SYS_XiTongZhuangTai]
           ,[SYS_XiTongBeiZhu])
     VALUES
           (@Id
           ,@OrgCode
           ,@EnterpriseId
           ,@OrgName
           ,'新运政同步新增'
           ,'新运政同步新增'
           ,'新运政同步新增'
           ,GETDATE()
             ,0
           ,'新运政同步新增')";
                    conn.Execute(sql, time);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"企业编号映射新增失败" + ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 删除企业用户信息
        /// </summary>
        /// <param name="enterpriseData"></param>
        public void UserDelete(CheLiangYeHu enterpriseData)
        {
            try
            {
                var qiYeQuery =
                    new Dtos.SystemOrgInfoDto
                    {
                        SysId = "C6380E44-F83F-A921-7174-5B6A8565BB4E",
                        OrganizationName = enterpriseData.OrgName,
                        OrganizationType = 2,
                        OrganizationCode = enterpriseData.OrgCode,
                        SYS_ZuiJinXiuGaiRen = "同步新运政注销",
                        SYS_ZuiJinXiuGaiRenID = string.Empty
                    };
                var qiYeList = new List<Dtos.SystemOrgInfoDto> { qiYeQuery };
                var information = GetInvokeRequest("00000030035", "1.0", new
                {
                    SystemOrganizationInfo = qiYeList.ToList()
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"删除企业用户失败" + ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 企业伪删除
        /// </summary>
        /// <param name="enterpriseData"></param>
        public void EnterpriseUpdate(CheLiangYeHu enterpriseData)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {

                    var sql = $@"UPDATE [DC_GPSJCDAGL].[dbo].[T_CheLiangYeHu]
SET SYS_XiTongZhuangTai =1,
SYS_XiTongBeiZhu='新运政同步注销',
SYS_ZuiJinXiuGaiShiJian=getdate()
WHERE  OrgName='{enterpriseData.OrgName}'
AND  OrgCode='{enterpriseData.OrgCode}'
AND SYS_XiTongZhuangTai=0";
                    conn.ExecuteScalar<int>(sql);

                    var sqlSecond = $@"UPDATE [DC_GPSJCDAGL].[dbo].[T_OrgBaseInfo]
SET SYS_XiTongZhuangTai =1,
SYS_XiTongBeiZhu='新运政同步注销',
SYS_ZuiJinXiuGaiShiJian=getdate()
WHERE  Id='{enterpriseData.BaseId}'
AND SYS_XiTongZhuangTai=0";
                    conn.ExecuteScalar<int>(sqlSecond);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"企业伪删除失败" + ex.Message);
                    throw ex;
                }
            }
        }



        /// <summary>
        /// 新增企业
        /// </summary>
        /// <param name="enterpriseDate"></param>
        public void EnterpriseAdd(ImportVehicleContactInfoDto enterpriseDate)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var item = new AddOrUpdateQiYeDangAn
                    {
                        Id = Guid.NewGuid(),
                        OrgName = enterpriseDate.YeHuMingCheng,
                        OrgCode = enterpriseDate.YeHuDaiMa,
                        OrgShortName = enterpriseDate.YeHuMingCheng,
                        XiaQuSheng = "广东",
                        XiaQuShi = enterpriseDate.XiaQuShi,
                        XiaQuXian = enterpriseDate.XiaQuXian,
                        JingYingFanWei = "广州"+ enterpriseDate.XiaQuShi+ enterpriseDate.XiaQuXian,
                        YeWuJingYingFanWei = enterpriseDate.JingYingFanWei,
                        DiZhi = enterpriseDate.DiZhi,
                        ZhuangTai = "1",
                        JingYingXuKeZhengHao = enterpriseDate.JingYingXuKeZhengHao,
                        LianXiRen = enterpriseDate.LianXiRen,
                        LianXiDianHua = enterpriseDate.LianXiDianHua,
                        ChuanZhen = enterpriseDate.ChuanZhen,
                        JingJiLeiXing = ""
                    };
                    var addSql = @"
                                        INSERT DC_GPSJCDAGL.dbo.T_OrgBaseInfo
                                        (
                                            Id,
                                            OrgCode,
                                            OrgType,
                                            OrgShortName,
                                            OrgName,
                                            ParentOrgId,
                                            JingYingFanWei,
                                            XiaQuSheng,
                                            XiaQuShi,
                                            XiaQuXian,
                                            DiZhi,
                                            ZhuangTai,
                                            ChuangJianRenOrgCode,
                                            ZuiJinXiuGaiRenOrgCode,
                                            Remark,
                                            SYS_ShuJuLaiYuan,
                                            SYS_ChuangJianRenID,
                                            SYS_ChuangJianRen,
                                            SYS_ChuangJianShiJian,
                                            SYS_ZuiJinXiuGaiRenID,
                                            SYS_ZuiJinXiuGaiRen,
                                            SYS_ZuiJinXiuGaiShiJian,
                                            SYS_XiTongZhuangTai,
                                            SYS_XiTongBeiZhu,
                                            Street,
                                            YeWuJingYingFanWei
                                        )
                                        VALUES
                                        (   @Id, -- Id - uniqueidentifier
                                            @OrgCode, -- OrgCode - nvarchar(16)
                                            2, -- OrgType - int
                                            @OrgShortName, -- OrgShortName - nvarchar(50)
                                            @OrgName, -- OrgName - nvarchar(50)
                                            NULL, -- ParentOrgId - uniqueidentifier
                                            @JingYingFanWei, -- JingYingFanWei - text
                                            @XiaQuSheng, -- XiaQuSheng - nvarchar(30)
                                            @XiaQuShi, -- XiaQuShi - nvarchar(30)
                                            @XiaQuXian, -- XiaQuXian - nvarchar(30)
                                            @DiZhi, -- DiZhi - text
                                            1, -- ZhuangTai - int
                                            NULL, -- ChuangJianRenOrgCode - nvarchar(16)
                                            NULL, -- ZuiJinXiuGaiRenOrgCode - nvarchar(16)
                                            NULL, -- Remark - text
                                            '新运政同步', -- SYS_ShuJuLaiYuan - nvarchar(255)
                                            NULL, -- SYS_ChuangJianRenID - nvarchar(255)
                                            NULL, -- SYS_ChuangJianRen - nvarchar(255)
                                            GETDATE(), -- SYS_ChuangJianShiJian - datetime
                                            NULL, -- SYS_ZuiJinXiuGaiRenID - nvarchar(255)
                                            NULL, -- SYS_ZuiJinXiuGaiRen - nvarchar(255)
                                            GETDATE(), -- SYS_ZuiJinXiuGaiShiJian - datetime
                                            0, -- SYS_XiTongZhuangTai - int
                                            NULL, -- SYS_XiTongBeiZhu - nvarchar(255)
                                            NULL, -- Street - nvarchar(100)
                                            @YeWuJingYingFanWei  -- YeWuJingYingFanWei - nvarchar(2000)
                                            )


                                            INSERT DC_GPSJCDAGL.dbo.T_CheLiangYeHu
                                            (
                                                Id,
                                                BaseId,
                                                OrgCode,
                                                OrgType,
                                                OrgShortName,
                                                OrgName,
                                                SuoShuJianKongPingTai,
                                                JingYingXuKeZhengHao,
                                                JingYingXuKeZhengYouXiaoQi,
                                                JingYingXuKeZhengChangQiYouXiao,
                                                GongShangYingYeZhiZhaoHao,
                                                GongShangYingYeZhiZhaoYouXiaoQi,
                                                GongShangYingYeZhiZhaoChangQiYouXiao,
                                                QiYeXingZhi,
                                                LianXiRen,
                                                ChuanZhenHaoMa,
                                                LianXiFangShi,
                                                ShiFouGeTiHu,
                                                ShenHeZhuangTai,
                                                SYS_ShuJuLaiYuan,
                                                SYS_ChuangJianRenID,
                                                SYS_ChuangJianRen,
                                                SYS_ChuangJianShiJian,
                                                SYS_ZuiJinXiuGaiRenID,
                                                SYS_ZuiJinXiuGaiRen,
                                                SYS_ZuiJinXiuGaiShiJian,
                                                SYS_XiTongZhuangTai,
                                                SYS_XiTongBeiZhu,
                                                JingJiLeiXing,
                                                SuoShuQiYe,
                                                QiYeBiaoZhiId,
                                                IsConfirmInfo,
                                                SheHuiXinYongDaiMa,
                                                GeTiHuShenFenZhengHaoMa
                                            )
                                            VALUES
                                            (   NEWID(), -- Id - uniqueidentifier
                                                @Id, -- BaseId - nvarchar(255)
                                                @OrgCode, -- OrgCode - nvarchar(16)
                                                2, -- OrgType - int
                                                @OrgShortName, -- OrgShortName - nvarchar(50)
                                                @OrgName, -- OrgName - nvarchar(50)
                                                NULL, -- SuoShuJianKongPingTai - nvarchar(50)
                                                @JingYingXuKeZhengHao, -- JingYingXuKeZhengHao - nvarchar(50)
                                                NULL, -- JingYingXuKeZhengYouXiaoQi - date
                                                NULL, -- JingYingXuKeZhengChangQiYouXiao - bit
                                                NULL, -- GongShangYingYeZhiZhaoHao - nvarchar(50)
                                                NULL, -- GongShangYingYeZhiZhaoYouXiaoQi - date
                                                NULL, -- GongShangYingYeZhiZhaoChangQiYouXiao - bit
                                                NULL, -- QiYeXingZhi - nvarchar(50)
                                                @LianXiRen, -- LianXiRen - nvarchar(50)
                                                @ChuanZhen, -- ChuanZhenHaoMa - nvarchar(16)
                                                @LianXiDianHua, -- LianXiFangShi - nvarchar(20)
                                                NULL, -- ShiFouGeTiHu - int
                                                1, -- ShenHeZhuangTai - int
                                                 '新运政同步', -- SYS_ShuJuLaiYuan - nvarchar(255)
                                                NULL, -- SYS_ChuangJianRenID - nvarchar(255)
                                                NULL, -- SYS_ChuangJianRen - nvarchar(255)
                                                GETDATE(), -- SYS_ChuangJianShiJian - datetime
                                                NULL, -- SYS_ZuiJinXiuGaiRenID - nvarchar(255)
                                                NULL, -- SYS_ZuiJinXiuGaiRen - nvarchar(255)
                                                NULL, -- SYS_ZuiJinXiuGaiShiJian - datetime
                                                0, -- SYS_XiTongZhuangTai - int
                                                NULL, -- SYS_XiTongBeiZhu - nvarchar(255)
                                                NULL, -- JingJiLeiXing - nvarchar(30)
                                                NULL, -- SuoShuQiYe - nvarchar(30)
                                                NULL, -- QiYeBiaoZhiId - uniqueidentifier
                                                0,    -- IsConfirmInfo - int
                                                NULL, -- SheHuiXinYongDaiMa - nvarchar(50)
                                                NULL  -- GeTiHuShenFenZhengHaoMa - nvarchar(50)
                                                )";
                    conn.Execute(addSql, item);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"新增企业失败" + ex.Message + "企业" + enterpriseDate.YeHuMingCheng, ex);
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 企业修改信息
        /// </summary>
        /// <param name="enterpriseData"></param>
        public void EnterpriseUpdateInformation(OrgBaseInfo enterpriseData)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var sqlSecond = $@"UPDATE [DC_GPSJCDAGL].[dbo].[T_OrgBaseInfo]
SET DiZhi ='{enterpriseData.DiZhi}',
YeWuJingYingFanWei='{enterpriseData.YeWuJingYingFanWei}',
JingYingFanWei='{enterpriseData.JingYingFanWei}'
WHERE  Id='{enterpriseData.Id}'";
                    conn.ExecuteScalar<int>(sqlSecond);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"企业同步基础信息失败" + ex.Message);
                    throw ex;
                }
            }
        }
        #endregion

        /// <summary>
        /// 同步网约车企业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> ImportAppointmentEnterprise(ImportFuWu dto)
        {
            var result = new ServiceResult<bool>() { Data = true };
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Field))
                {
                    result.StatusCode = 2;
                    result.Data = false;
                    result.ErrorMessage = "企业信息导入失败，请选择文件";
                    return result;
                }

                var b = FileAgentUtility.GetFileData(new Guid(dto.Field));
                var list = ImportMapToObjectAppointment(b);
                if (list == null || !list.Any())
                {
                    result.StatusCode = 2;
                    result.Data = false;
                    result.ErrorMessage = "企业信息导入失败，表格内容为空";
                    return result;
                }

                #region 数据格式化

                foreach (var entity in list)
                {
                    switch (entity.XiaQuXian)
                    {
                        case "清新区":
                            entity.XiaQuXian = "清新";
                            break;
                        case "英德市":
                            entity.XiaQuXian = "英德";
                            break;
                        case "连州市":
                            entity.XiaQuXian = "连州";
                            break;
                        case "佛冈县":
                            entity.XiaQuXian = "佛冈";
                            break;
                        case "连山壮族瑶族自治县":
                            entity.XiaQuXian = "连山";
                            break;
                        case "连南瑶族自治县":
                            entity.XiaQuXian = "连南";
                            break;
                        case "阳山县":
                            entity.XiaQuXian = "阳山";
                            break;
                        case "清城区":
                            entity.XiaQuXian = "清城";
                            break;
                    }
                }

                var sysZhengChang = (int) XiTongZhuangTaiEnum.正常;
                var starring = new[] {"清新", "英德", "连州", "佛冈", "连山", "连南", "阳山", "清城"};
                foreach (var enterpriseEntity in list)
                {
                    if (string.IsNullOrWhiteSpace(enterpriseEntity.YeHuMingCheng))
                    {
                        continue;
                    }
                    lock (CWHelper.GetStringLock(enterpriseEntity.YeHuMingCheng, "AddEnterprise"))
                    {
                        if (enterpriseEntity.XiaQuShi != "清远市" && enterpriseEntity.XiaQuShi != "清远")
                        {
                            continue;
                        }
                        //只接受清远下的九个区数据
                        var xiaQuXian = starring.Contains(enterpriseEntity.XiaQuXian);
                        if (!xiaQuXian)
                        {
                            continue;
                        }
                        //业户信息
                        var enterpriseDate = _orgBaseInfoRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang&& x.OrgName == enterpriseEntity.YeHuMingCheng
                                           &&x.XiaQuXian== enterpriseEntity.XiaQuXian).FirstOrDefault();
                        if (enterpriseDate != null) continue;
                        {
                            #region 获取序列号和赋值组织代码等
                            var orgCode = "yh" + EnterpriseCode();
                            var enterprise = _yeHuRepository
                                .GetQuery(x => x.OrgCode == orgCode && x.SYS_XiTongZhuangTai == 0)
                                .FirstOrDefault();
                            if (enterprise != null)
                            {
                                result.StatusCode = 2;
                                result.Data = false;
                                result.ErrorMessage = "企业编号重复，请重新导入";
                                return result;
                            }
                            #endregion
                            AppointmentEnterpriseAdd(enterpriseEntity, orgCode);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.Data = false;
                result.ErrorMessage = "网约车企业信息导入失败" + ex.Message;
            }

            return result;
        }

        private List<ImportVehicleAppointmentDto> ImportMapToObjectAppointment(byte[] buff)
        {
            Stream s = new MemoryStream(buff);
            var wk = WorkbookFactory.Create(s);
            var sheet = wk.GetSheetAt(0);
            var row = sheet.GetRow(0);
            #region 格式校验
            var dictionary = new Dictionary<string, string> {
                {"企业名称","YeHuMingCheng"},
                {"辖区省","XiaQuSheng"},
                {"辖区市","XiaQuShi"},
                {"辖区县","XiaQuXian"},
                {"经营范围","JingYingFanWei"},
                {"企业性质","QiYeXingZhi"}
            };
            foreach (var r in row)
            {
                if (!dictionary.ContainsKey(r.ToString()))
                {
                    return null;
                }
            }
            #endregion
            var list = new List<ImportVehicleAppointmentDto>();
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                var importDto = new ImportVehicleAppointmentDto
                {

                    YeHuMingCheng = row.GetCell(0)?.ToString(),
                    XiaQuSheng = row.GetCell(1)?.ToString(),
                    XiaQuShi = row.GetCell(2)?.ToString(),
                    XiaQuXian = row.GetCell(3)?.ToString(),
                    JingYingFanWei = row.GetCell(4)?.ToString(),
                    QiYeXingZhi = row.GetCell(5)?.ToString()
                };
                list.Add(importDto);
            }
            return list;
        }
       /// <summary>
       /// 新增网约车企业
       /// </summary>
       /// <param name="enterpriseDate"></param>
       /// <param name="orgCode"></param>
        public void AppointmentEnterpriseAdd(ImportVehicleAppointmentDto enterpriseDate,string orgCode)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var item = new AddOrUpdateQiYeDangAn
                    {
                        Id = Guid.NewGuid(),
                        OrgName = enterpriseDate.YeHuMingCheng,
                        OrgCode = orgCode,
                        OrgShortName = enterpriseDate.YeHuMingCheng,
                        XiaQuSheng = "广东",
                        XiaQuShi = enterpriseDate.XiaQuShi,
                        XiaQuXian = enterpriseDate.XiaQuXian,
                        JingYingFanWei = enterpriseDate.JingYingFanWei,
                        ZhuangTai = "1",
                        QiYeXingZhi = enterpriseDate.QiYeXingZhi
                    };
                    var addSql = @"
                                        INSERT DC_GPSJCDAGL.dbo.T_OrgBaseInfo
                                        (
                                            Id,
                                            OrgCode,
                                            OrgType,
                                            OrgShortName,
                                            OrgName,
                                            JingYingFanWei,
                                            XiaQuSheng,
                                            XiaQuShi,
                                            XiaQuXian,
                                            ZhuangTai,
                                            SYS_ShuJuLaiYuan,
                                            SYS_ChuangJianShiJian,
                                            SYS_ZuiJinXiuGaiShiJian,
                                            SYS_XiTongZhuangTai
                                        )
                                        VALUES
                                        (   @Id, -- Id - uniqueidentifier
                                            @OrgCode, -- OrgCode - nvarchar(16)
                                            2, -- OrgType - int
                                            @OrgShortName, -- OrgShortName - nvarchar(50)
                                            @OrgName, -- OrgName - nvarchar(50)
                                            @JingYingFanWei, -- JingYingFanWei - text
                                            @XiaQuSheng, -- XiaQuSheng - nvarchar(30)
                                            @XiaQuShi, -- XiaQuShi - nvarchar(30)
                                            @XiaQuXian, -- XiaQuXian - nvarchar(30)
                                            1, -- ZhuangTai - int
                                            '数据导入', -- SYS_ShuJuLaiYuan - nvarchar(255)
                                            GETDATE(), -- SYS_ChuangJianShiJian - datetime
                                            GETDATE(), -- SYS_ZuiJinXiuGaiShiJian - datetime
                                            0 -- SYS_XiTongZhuangTai - int
                                            )


                                            INSERT DC_GPSJCDAGL.dbo.T_CheLiangYeHu
                                            (
                                                Id,
                                                BaseId,
                                                OrgCode,
                                                OrgType,
                                                OrgShortName,
                                                OrgName,
                                                ShenHeZhuangTai,
                                                   QiYeXingZhi,   
                                                SYS_ShuJuLaiYuan,
                                                SYS_ChuangJianShiJian,
                                                SYS_XiTongZhuangTai,
                                                IsConfirmInfo,
                                                SheHuiXinYongDaiMa,
                                                GeTiHuShenFenZhengHaoMa
                                            )
                                            VALUES
                                            (   NEWID(), -- Id - uniqueidentifier
                                                @Id, -- BaseId - nvarchar(255)
                                                @OrgCode, -- OrgCode - nvarchar(16)
                                                2, -- OrgType - int
                                                @OrgShortName, -- OrgShortName - nvarchar(50)
                                                @OrgName, -- OrgName - nvarchar(50)
                                                1, -- ShenHeZhuangTai - int
                                                 @QiYeXingZhi,  
                                                 '数据导入', -- SYS_ShuJuLaiYuan - nvarchar(255)
                                                GETDATE(), -- SYS_ChuangJianShiJian - datetime
                                                0, -- SYS_XiTongZhuangTai - int
                                                0,    -- IsConfirmInfo - int
                                                NULL, -- SheHuiXinYongDaiMa - nvarchar(50)
                                                NULL  -- GeTiHuShenFenZhengHaoMa - nvarchar(50)
                                                )";
                    conn.Execute(addSql, item);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"新增网约车企业失败" + ex.Message + "企业" + enterpriseDate.YeHuMingCheng, ex);
                    throw ex;
                }
            }

        }


       /// <summary>
       /// 获取企业序列号
       /// </summary>
       /// <returns></returns>
       public string EnterpriseCode()
       {
           string account;
           try
           {
               using (IDbConnection conn =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
               {
                   var state = false;
                   var sql =
                       $@"
SELECT *  FROM DC_GPSJCDAGL.dbo.T_EnterpriseCode ";
                   var code = conn.Query<EnterpriseCodeModel>(sql).ToList().FirstOrDefault();
                   account = code == null ? "10100" : code.OrgCode;
                   account = Convert.ToString(Convert.ToInt16(account) + 1).PadLeft(5, '0');
                   while (!state)
                   {
                       sql = $@" SELECT COUNT(*)  FROM DC_GPSJCDAGL.dbo.T_CheLiangYeHu
                    WHERE SYS_XiTongZhuangTai=0    AND OrgCode='yh{account}' ";
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
                   sql = $@" UPDATE DC_GPSJCDAGL.dbo.T_EnterpriseCode
	  SET OrgCode='{account}'";
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
    }
}
