using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
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
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class DiSanFangXinXiService : ApiServiceBase, IDiSanFangXinXiService
    {
        #region 构造函数
        private readonly string sysId = string.Empty;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;

        public DiSanFangXinXiService(
            IOrgBaseInfoRepository orgBaseInfoRepository,
        IBussinessLogger bussinessLogger) : base(bussinessLogger)
        {
            _orgBaseInfoRepository = orgBaseInfoRepository;
            sysId = base.SysId;
        }
        #endregion

        #region 新增
        public ServiceResult<bool> Create(string SysId, DiSanFangExDto model)
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
                    OrgType = model.JiGouLeiXing,
                    OrgShortName = model.JiGouJianCheng,
                    OrgName = model.JiGouMingCheng,
                    ParentOrgId = Guid.Parse(userInfo.ExtOrganizationId),
                    JingYingFanWei = model.JingYingQuYu,
                    XiaQuSheng = model.XiaQuSheng,
                    XiaQuShi = model.XiaQuShi,
                    XiaQuXian = model.XiaQuXian,
                    DiZhi = model.GongSiDiZhi,
                    Remark = model.BeiZhu,
                    ZhuangTai = (int)ZhuangTaiEnum.正常营业,
                    ChuangJianRenOrgCode = userInfo.OrganizationCode,
                    SYS_ChuangJianRenID = userInfo.Id,
                    SYS_ChuangJianRen = userInfo.UserName,
                    SYS_ChuangJianShiJian = DateTime.Now,
                    SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                };
                var isExitModel = _orgBaseInfoRepository.Count(x => x.OrgName == orgBaseInfo.OrgName && x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常);
                if (isExitModel > 0)
                {
                    LogHelper.Error(string.Format("已经存在名称为{0}的第三方机构，请使用其它名称", model.JiGouMingCheng));
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("已经存在名称为{0}的第三方机构，请使用其它名称", orgBaseInfo.OrgName) };
                }
                if (orgBaseInfo.OrgType == null)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("第三方机构类型不能为空") };
                }
                if (string.IsNullOrWhiteSpace(orgBaseInfo.XiaQuShi))
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("辖区市信息不能为空") };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var prefix = "";
                    string RoleCode = "";
                    if (orgBaseInfo.OrgType == (int)OrganizationType.市政府)
                    {
                        prefix = "zf";
                        RoleCode = string.Format("{0:000},{1:000}", (int)ZuZhiJueSe.市级政府, (int)ZuZhiJueSe.组织管理员);
                    }
                    else if (orgBaseInfo.OrgType == (int)OrganizationType.县政府)
                    {
                        prefix = "xjzf";
                        RoleCode = string.Format("{0:000},{1:000}", (int)ZuZhiJueSe.县级政府, (int)ZuZhiJueSe.组织管理员);
                    }
                    var getSNoResponse = GetInvokeRequest("00000020013", "1.0", new
                    {
                        SysId = "60190FC4-5103-4C76-94E4-12A54B62C92A", //2021-01-21 by_hyh因为多系统使用该注册接口，慎改
                        Module = "00330021",
                        Type = 4
                    });
                    if (getSNoResponse.publicresponse.statuscode != 0)
                    {
                        LogHelper.Error("调用服务（00000020013-1.0）出错" + getSNoResponse.publicresponse.message);
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "获取序列号出错了" };
                    }
                    string SNo = getSNoResponse.body.SNo.ToString().PadLeft(4, '0');
                    string OrgType = string.Format("{0}", orgBaseInfo.OrgType);
                    string UserName = prefix + "admin" + SNo;
                    string OrgCode = prefix + SNo;

                    //TODO:同步生成管理员账号密码机构账号为dsfadmin+四位流水号，密码Gdunis1234； 
                    var getInitOrgAndSysUserResponse = GetInvokeRequest("00000030033", "1.0", new
                    {
                        SysId = SysId,
                        SysOrgId = userInfo.OrganId,
                        OrgName = orgBaseInfo.OrgName,
                        OrgType = OrgType,
                        OrgCode = OrgCode,
                        UserName = UserName,//TODO:生成序列号
                        RoleCode = RoleCode,
                        OrgProvince = model.XiaQuSheng,
                        OrgCity = model.XiaQuShi,
                        OrgDistrict = model.XiaQuXian,
                        ManageArea = orgBaseInfo.JingYingFanWei,
                        SYS_XiTongBeiZhu = OrgCode
                    });
                    if (getInitOrgAndSysUserResponse.publicresponse.statuscode != 0)
                    {
                        LogHelper.Error("调用服务（00000030033-1.0）出错" + getInitOrgAndSysUserResponse.publicresponse.message);
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "新增系统组织及管理员等出错了" };
                    }
                    orgBaseInfo.OrgCode = OrgCode;
                    if (orgBaseInfo.ChuangJianRenOrgCode == null)
                    {
                        return new ServiceResult<bool>() { StatusCode = 2, ErrorMessage = "获取创建单位组织代码失败!" };
                    }
                    _orgBaseInfoRepository.Add(orgBaseInfo);

                    var addResult = uow.CommitTransaction() > 0;
                    if (addResult)
                    {
                        //AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                        //{
                        //    ReqId = reqid,
                        //    YeWuDuiXiangLeiXing = "企业GPS基础档案",
                        //    YeWuDuiXiangZiLei = "第三方机构档案",
                        //    YeWuDuiXiangID = orgBaseInfo.Id,
                        //    YeWuDuiXiangBiaoZhi = model.JiGouMingCheng,
                        //    YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(model),
                        //    YeWuLeiXing = "新增企业GPS基础档案",
                        //    YeWuZiLei = "新增第三方机构档案",
                        //    YeWuChangJingLeiXing = "基础档案业务",
                        //    MoKuaiMingCheng = "企业GPS基础档案管理",
                        //    XiTongMingCheng = "企业GPS基础档案系统",
                        //    YingYongMingCheng = "第三方机构档案系统",
                        //    YeWuGaiYaoXinXi = string.Format("新增档案：{0}", model.JiGouMingCheng),
                        //}, userInfo);
                        return new ServiceResult<bool>() { Data = true };
                    }
                    else
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "新增第三方机构出错了" };
                    }
                }
            });
        }
        #endregion

        #region 查看
        public ServiceResult<DiSanFangExDto> Get(Guid id)
        {
            var result = new ServiceResult<DiSanFangExDto>();

            UserInfoDtoNew userInfo = GetUserInfo();
            int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

            // 组织基本信息
            OrgBaseInfo orgBaseInfo = _orgBaseInfoRepository.GetQuery(x => x.Id == id && x.SYS_XiTongZhuangTai == sysZhengChang).FirstOrDefault();
            DiSanFangExDto dto = new DiSanFangExDto
            {
                Id = orgBaseInfo?.Id.ToString(),
                JiGouMingCheng = orgBaseInfo?.OrgName,
                JiGouJianCheng = orgBaseInfo?.OrgShortName,
                JiGouLeiXing = orgBaseInfo?.OrgType,
                JingYingQuYu = orgBaseInfo?.JingYingFanWei,
                YouXiaoZhuangTai = orgBaseInfo?.ZhuangTai,
                GongSiDiZhi = orgBaseInfo?.DiZhi,
                BeiZhu = orgBaseInfo?.Remark,
                XiaQuSheng=orgBaseInfo.XiaQuSheng,
                XiaQuShi=orgBaseInfo.XiaQuShi,
                XiaQuXian=orgBaseInfo.XiaQuXian
            };

            //var lianXiRenQuery = _lianXiRenXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && m.LeiBie == 2 && m.BenDanWeiOrgCode  == mainList.BenDanWeiOrgCode).FirstOrDefault();
            //if (lianXiRenQuery != null)
            //{
            //    model.FuZheRen = lianXiRenQuery.LianXiRen;
            //    model.FuZheRenDianHua = lianXiRenQuery.ShouJiHaoMa;
            //}
            //else
            //{
            dto.FuZheRen = "暂未添加负责人";
            dto.FuZheRenDianHua = "暂未添加负责人";
            //}
            result.Data = dto;
            return result;
        }
        #endregion

        #region 列表
        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                DiSanFangSearchDto search = JsonConvert.DeserializeObject<DiSanFangSearchDto>(queryData.data.ToString());
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                List<int> types = new List<int>() { (int)OrganizationType.保险机构, (int)OrganizationType.第三方监测中心, (int)OrganizationType.市政府, (int)OrganizationType.县政府 };

                Expression<Func<OrgBaseInfo, bool>> exp = p => p.SYS_XiTongZhuangTai == sysZhengChang && types.Contains((int)p.OrgType);
                // 机构名称
                if (!string.IsNullOrWhiteSpace(search.JiGouMingCheng))
                {
                    exp = exp.And(p => p.OrgName.Contains(search.JiGouMingCheng.Trim()));
                }

                //机构类型
                if (search.JiGouLeiXing != null)
                {
                    exp = exp.And(p => p.OrgType == search.JiGouLeiXing);
                }

                //有效状态 正常营业   合约到期
                if (!string.IsNullOrWhiteSpace(search.YouXiaoZhuangTai))
                {
                    int Status = Convert.ToInt32(search.YouXiaoZhuangTai);
                    exp = exp.And(m => m.ZhuangTai == Status);
                }

                // 组织基本信息
                var orgBaseInfoQuery = _orgBaseInfoRepository.GetQuery(exp).ToList();

                //var lianXiRenQuery = _lianXiRenXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == sysZhengChang);

                var query = from p in orgBaseInfoQuery
                            select new
                            {
                                p.Id,
                                p.OrgCode,
                                p.OrgName,
                                p.OrgType,
                                JingYingQuYu = p.JingYingFanWei,
                                YouXiaoZhuangTai = p.ZhuangTai,
                                //t.LianXiRen,
                                //t.ShouJiHaoMa,
                                // t.GuDingDianHua,
                                p.SYS_ChuangJianShiJian
                            };
                //if (!string.IsNullOrWhiteSpace(search.LianXiRen))
                //{
                //    query = query.Where(m => m.LianXiRen.Contains(search.LianXiRen.Trim()));
                //}
                //if (!string.IsNullOrWhiteSpace(search.ShouJiHaoMa))
                //{
                //    query = query.Where(m => m.ShouJiHaoMa.Contains(search.ShouJiHaoMa.Trim()) || m.GuDingDianHua.Contains(search.ShouJiHaoMa.Trim()));
                //}

                QueryResult queryResult = new QueryResult();
                queryResult.totalcount = query.Distinct().Count();
                //List<PingTaiDaiLiShangDto> resultList = new List<PingTaiDaiLiShangDto>();
                queryResult.items = query.Distinct().OrderByDescending(x => x.SYS_ChuangJianShiJian).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                return new ServiceResult<QueryResult>() { Data = queryResult };
            });
        }
        #endregion

        #region 修改
        public ServiceResult<bool> Update(string sysId, DiSanFangExDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (string.IsNullOrWhiteSpace(dto.Id))
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("参数错误,Id为空") };
                }

                // 组织基本信息
                OrgBaseInfo orgBaseInfo = _orgBaseInfoRepository.GetQuery(x => x.Id == new Guid(dto.Id) && x.SYS_XiTongZhuangTai == sysZhengChang).FirstOrDefault();

                if (orgBaseInfo == null)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("找不到组织基本信息") };
                }


                if(string.IsNullOrWhiteSpace(dto.XiaQuShi))
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("辖区市信息不能为空") };
                }

                //组织基本信息 更新的信息
                orgBaseInfo.OrgName = dto.JiGouMingCheng.Trim();
                orgBaseInfo.OrgType = dto.JiGouLeiXing;
                orgBaseInfo.OrgShortName = dto.JiGouJianCheng;
                orgBaseInfo.XiaQuSheng = dto.XiaQuSheng;
                orgBaseInfo.XiaQuShi = dto.XiaQuShi;
                orgBaseInfo.XiaQuXian = dto.XiaQuXian;
                orgBaseInfo.JingYingFanWei = dto.JingYingQuYu;
                orgBaseInfo.DiZhi = dto.GongSiDiZhi;
                orgBaseInfo.Remark = dto.BeiZhu;
                orgBaseInfo.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                orgBaseInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                orgBaseInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                orgBaseInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;


                var isExitModel = _orgBaseInfoRepository.Count(x => x.OrgName == orgBaseInfo.OrgName && x.Id != orgBaseInfo.Id && x.SYS_XiTongZhuangTai == sysZhengChang);
                if (isExitModel > 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = string.Format("已经存在名称为{0}的第三方机构，请使用其它名称", orgBaseInfo.OrgName) };
                }

                var systemOrganizationInfoDto = new { OrganizationCode = orgBaseInfo.OrgCode, OrganizationName = orgBaseInfo.OrgName, ManageArea = orgBaseInfo.JingYingFanWei, SysId = sysId, OrganizationType = orgBaseInfo.OrgType };

                var updateSystemOrgResult = GetInvokeRequest("00000030037", "1.0", systemOrganizationInfoDto);
                if (updateSystemOrgResult.publicresponse.statuscode != 0)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = updateSystemOrgResult.publicresponse.message };
                }
                if (!(bool)updateSystemOrgResult.body.success)
                {
                    return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = updateSystemOrgResult.body.msg };
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _orgBaseInfoRepository.Update(orgBaseInfo);
                    var updateResult = uow.CommitTransaction() > 0;
                    if (updateResult)
                    {
                        //AddBussiness(new Conwin.Framework.BusinessLogger.Dtos.BusinessLogDTO()
                        //{
                        //    ReqId = reqid,
                        //    YeWuDuiXiangLeiXing = "企业GPS基础档案",
                        //    YeWuDuiXiangZiLei = "第三方机构档案",
                        //    YeWuDuiXiangID = orgBaseInfo.Id,
                        //    YeWuDuiXiangBiaoZhi = orgBaseInfo.OrgName,
                        //    YeWuDuiXiangKuoZhanXinXi = JsonConvert.SerializeObject(orgBaseInfo),
                        //    YeWuLeiXing = "更新企业GPS基础档案",
                        //    YeWuZiLei = "更新第三方机构档案",
                        //    YeWuChangJingLeiXing = "基础档案业务",
                        //    MoKuaiMingCheng = "企业GPS基础档案管理",
                        //    XiTongMingCheng = "企业GPS基础档案系统",
                        //    YingYongMingCheng = "第三方机构档案系统",
                        //    YeWuGaiYaoXinXi = string.Format("更新档案：{0}", orgBaseInfo.OrgName),
                        //}, userInfo);
                        return new ServiceResult<bool>() { Data = true };
                    }
                    else
                    {
                        return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "修改第三方机构信息出错了" };
                    }
                }
            });
        }
        #endregion

        public override void Dispose()
        {

            _orgBaseInfoRepository.Dispose();
        }
    }
}
