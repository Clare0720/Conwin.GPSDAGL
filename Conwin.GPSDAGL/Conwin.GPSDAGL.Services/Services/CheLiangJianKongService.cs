using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.VehicleOnNetStatistics;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace Conwin.GPSDAGL.Services
{
    public partial class CheLiangJianKongService : ApiServiceBase, ICheLiangJianKongService
    {
        /// <summary>
        /// 分钟
        /// </summary>
        private static double CheLiangDingWeiZaiXian = CommonHelper.CheLiangZaiXianExpireTime * -1;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly ICheLiangZuZhiXinXiRepository _cheLiangZuZhiXinXiRepository;
        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly IYongHuCheLiangXinXiRepository _yongHuCheLiangXinXiRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        //private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;

        public CheLiangJianKongService(IOrgBaseInfoRepository orgBaseInfoRepository,
          ICheLiangZuZhiXinXiRepository cheLiangZuZhiXinXiRepository,
          ICheLiangRepository cheLiangXinXiRepository,
          IYongHuCheLiangXinXiRepository yongHuCheLiangXinXiRepository,
          ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository,
          ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
          ICheLiangYeHuRepository cheLiangYeHuRepository,
          IBussinessLogger _bussinessLogger) : base(_bussinessLogger)
        {
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _cheLiangZuZhiXinXiRepository = cheLiangZuZhiXinXiRepository;
            _cheLiangXinXiRepository = cheLiangXinXiRepository;
            _yongHuCheLiangXinXiRepository = yongHuCheLiangXinXiRepository;
            //_cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
        }

        public override void Dispose()
        {
            _orgBaseInfoRepository.Dispose();
        }


        public ServiceResult<object> GetVehicleInfoYeHu(UserInfoDto userInfo)
        {
            return null;
            //var result = (from a in _zuZhiGuanLianXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.OrgCode == userInfo.OrganizationCode && u.RelationOrgType == (int)OrganizationType.企业)
            //              join b in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
            //              on a.RelationOrgCode equals b.OrgCode
            //              select new
            //              {
            //                  //OrgId = b.Id,
            //                  OrgCode = b.OrgCode,
            //                  OrgName = b.OrgName
            //              }).ToList();
            //var userInfoNew = GetUserInfo();
            //if (userInfoNew.OrganizationType == (int)OrganizationType.企业)
            //{
            //    result.Add(new
            //    {
            //        OrgCode = userInfoNew.OrganizationCode,
            //        OrgName = userInfoNew.OrganizationName
            //    });
            //}

            //return  new ServiceResult<object>() { Data = result };
        }

        public ServiceResult<object> GetVehicleInfoCheDui(UserInfoDto userInfo)
        {
            return null;
            //var result = (from a in _zuZhiGuanLianXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == 0 && u.OrgCode == userInfo.OrganizationCode && u.RelationOrgType == (int)OrganizationType.车队)
            //              join b in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
            //              on a.RelationOrgCode equals b.OrgCode
            //              select new
            //              {
            //                  //OrgId = b.Id,
            //                  OrgCode = b.OrgCode,
            //                  OrgName = b.OrgName
            //              }).ToList();

            //var userInfoNew = GetUserInfo();
            //if (userInfoNew.OrganizationType == (int)OrganizationType.车队)
            //{
            //    result.Add(new
            //    {
            //        OrgCode = userInfoNew.OrganizationCode,
            //        OrgName = userInfoNew.OrganizationName
            //    });
            //}


            //return new ServiceResult<object>() { Data = result };
        }


        public ServiceResult<object> GetVehicleInfoByYeHu(QueryData qd, UserInfoDto userInfo)
        {
            CheLiangFenPeiJianKongSearchDto dto = JsonConvert.DeserializeObject<CheLiangFenPeiJianKongSearchDto>(qd.data.ToString());
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == 0;

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
            {
                cheliangexp = cheliangexp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
            {
                cheliangexp = cheliangexp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao));
            }

            if (!string.IsNullOrWhiteSpace(dto.YeHuOrgCode))
            {
                cheliangexp = cheliangexp.And(p => p.YeHuOrgCode == dto.YeHuOrgCode);
            }

            if (!string.IsNullOrWhiteSpace(dto.CheDuiOrgCode))
            {
                cheliangexp = cheliangexp.And(p => p.CheDuiOrgCode == dto.CheDuiOrgCode);
            }
            //定义返回的数据变量
            QueryResult queryResult = new QueryResult();

            //判断是要否要已分配监控的车
            if (dto.IsFPJK.Value)
            {
                //已分配监控的车辆
                var result = from a in _cheLiangZuZhiXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == 0 && a.OrgCode == userInfo.OrganizationCode)
                             join b in _cheLiangXinXiRepository.GetQuery(cheliangexp)
                             on Guid.Parse(a.CheLiangId) equals b.Id
                             join c in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                             on b.YeHuOrgCode equals c.OrgCode
                             select new
                             {
                                 b.Id,
                                 b.ChePaiHao,
                                 b.ChePaiYanSe,
                                 b.CheLiangZhongLei,
                                 b.SYS_ZuiJinXiuGaiShiJian,
                                 c.OrgName
                             };

                queryResult.totalcount = result.Count();
                queryResult.items = result.OrderByDescending(s => s.SYS_ZuiJinXiuGaiShiJian).Skip((qd.page - 1) * qd.rows).Take(qd.rows);
            }
            else
            {
                //未分配监控的车辆
                var cheliangIdList = (from f in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == 0 && p.SysUserId == dto.SysUserID.ToString())
                                      select f.CheLiangId).ToList();

                var result = from a in _cheLiangZuZhiXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == 0 && a.OrgCode == userInfo.OrganizationCode)
                             join b in _cheLiangXinXiRepository.GetQuery(cheliangexp)
                             on a.CheLiangId equals b.Id.ToString()
                             join c in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                             on b.YeHuOrgCode equals c.OrgCode
                             where !cheliangIdList.Contains(b.Id.ToString())
                             select new
                             {
                                 b.Id,
                                 b.ChePaiHao,
                                 b.ChePaiYanSe,
                                 b.CheLiangZhongLei,
                                 b.SYS_ZuiJinXiuGaiShiJian,
                                 c.OrgName
                             };


                queryResult.totalcount = result.Count();
                queryResult.items = result.OrderByDescending(s => s.SYS_ZuiJinXiuGaiShiJian).Skip((qd.page - 1) * qd.rows).Take(qd.rows);
            }
            return new ServiceResult<object>() { Data = queryResult };
        }

        public ServiceResult<object> AddVehicleMonitoring(List<CheLiangXinXiInput> dto, UserInfoDto userInfo)
        {
            var flag = false;
            var valueList = dto.Select(s => new YongHuCheLiangXinXi
            {
                Id = Guid.NewGuid(),
                CheLiangId = s.CheLiangID,
                SysUserId = s.SysUserID.ToString()
            }).ToList();

            valueList.ForEach(n => SetCreateSYSInfo(n, userInfo));

            if (valueList.Count > 0)
            {
                using (var u = new UnitOfWork())
                {
                    u.BeginTransaction();
                    _yongHuCheLiangXinXiRepository.BatchInsert(valueList.ToArray());
                    try
                    {
                        flag = u.CommitTransaction() >= 0;
                    }
                    catch (Exception ex)
                    {
                        return new ServiceResult<object>() { Data = flag, ErrorMessage = ex.Message, StatusCode = 2 };
                    }
                }
            }


            return new ServiceResult<object>() { Data = flag };
        }

        public ServiceResult<object> DelVehicleMonitoring(List<CheLiangXinXiInput> dto, UserInfoDto userInfo)
        {
            try
            {
                var cIds = dto.Select(s => s.CheLiangID).ToList();
                var sysUserId = dto.First().SysUserID.ToString();
                var list = _yongHuCheLiangXinXiRepository.GetQuery(s => cIds.Contains(s.CheLiangId.ToString()) && s.SysUserId == sysUserId && s.SYS_XiTongZhuangTai == 0).ToList();

                using (var u = new UnitOfWork())
                {
                    u.BeginTransaction();

                    foreach (var d in list)
                    {
                        d.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _yongHuCheLiangXinXiRepository.Update(d);
                    }
                    var flag = u.CommitTransaction() >= 0;
                    return new ServiceResult<object>() { Data = flag };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<object>() { Data = false, ErrorMessage = ex.Message, StatusCode = 2 };
            }

        }

        /// <summary>
        /// 获取车辆监控树
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<JianKongShuResult> QueryJianKongShu(CheLiangJianKongShuQueryDto queryData)
        {
            return ExecuteCommandClass<JianKongShuResult>(() =>
            {
                var result = new ServiceResult<JianKongShuResult>();

                var userInfo = GetUserInfo();
                var querySelf = string.IsNullOrWhiteSpace(queryData.OrgCode);
                if (querySelf)
                {
                    return QueryZuZhiCheLiangShu(userInfo);
                }

                //查询下级
                OrgBaseInfo orgInfo;
                if (queryData.OrgCode == AdminOrgCode)//平台运营商
                {
                    orgInfo = new OrgBaseInfo { OrgCode = AdminOrgCode, OrgType = 0 };
                }
                else
                {
                    orgInfo = _orgBaseInfoRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgCode == queryData.OrgCode).FirstOrDefault();
                }
                if (orgInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "该组织不存在";
                    return result;
                }

                switch (userInfo.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        result = QueryZhengFuXiaJiAndCheLiangZaiXianShu(userInfo, orgInfo);
                        break;
                    case (int)OrganizationType.县政府:
                        result = QueryXianZhengFuXiaJiAndCheLiangZaiXianShu(userInfo, orgInfo);
                        break;
                    case (int)OrganizationType.平台运营商:
                        result = QueryPingTaiYunYingShangXiaJiAndCheLiangZaiXianShu(userInfo, orgInfo);
                        break;
                    case (int)OrganizationType.企业:
                    case (int)OrganizationType.个体户:
                        result = QueryYeHuXiaJiAndCheLiangZaiXianShu(userInfo, orgInfo);
                        break;
                    default:
                        result.StatusCode = 2;
                        result.ErrorMessage = "该组织不存在";
                        break;
                }

                return result;
            }, e =>
            {
                LogHelper.Error("QueryJianKongShu异常", e);
            });

        }

        /// <summary>
        /// 获取监控树车辆-根据组织代码获取权限范围内车辆
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryJianKongShuCheLiang(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                var result = new ServiceResult<QueryResult>();

                var userInfo = GetUserInfo();

                CheLiangJianKongShuQueryDto inputData = JsonConvert.DeserializeObject<CheLiangJianKongShuQueryDto>(Convert.ToString(queryData.data));
                var inputOrgCode = inputData.OrgCode;
                if (string.IsNullOrEmpty(inputOrgCode))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "组织编号不能为空";
                    return result;
                }

                //查询下级
                OrgBaseInfo orgInfo;
                if (inputOrgCode == AdminOrgCode)//平台运营商
                {
                    orgInfo = new OrgBaseInfo { OrgCode = AdminOrgCode, OrgType = 0 };
                }
                else
                {
                    orgInfo = _orgBaseInfoRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgCode == inputOrgCode).FirstOrDefault();
                }
                if (orgInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "该组织不存在";
                    return result;
                }

                switch (orgInfo.OrgType)
                {
                    case (int)OrganizationType.市政府:
                        result = QueryZhengFuJianKongShuCheLiang(userInfo, orgInfo, queryData.page, queryData.rows);
                        break;
                    case (int)OrganizationType.县政府:
                        result = QueryXianZhengFuJianKongShuCheLiang(userInfo, orgInfo, queryData.page, queryData.rows);
                        break;
                    case (int)OrganizationType.平台运营商:
                        result = QueryPingTaiYunYingShangJianKongShuCheLiang(userInfo, orgInfo, queryData.page, queryData.rows);
                        break;
                    case (int)OrganizationType.企业:
                    case (int)OrganizationType.个体户:
                        result = QueryYeHuJianKongShuCheLiang(userInfo, orgInfo, queryData.page, queryData.rows);
                        break;
                    default:
                        result.StatusCode = 2;
                        result.ErrorMessage = "该组织不存在";
                        break;
                }

                return result;
            }, e =>
            {
                LogHelper.Error("QueryJianKongShu异常", e);
            });

        }

        public ServiceResult<JiGouJieGuoDto> QueryDangQianZuZhiCheLiangZaiXianShu(CheLiangJianKongShuQueryDto queryData)
        {
            try
            {
                var result = new ServiceResult<JiGouJieGuoDto>() { StatusCode = 0, Data = new JiGouJieGuoDto() };
                DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

                var orgInfo = _orgBaseInfoRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgCode == queryData.OrgCode && p.OrgType == (int)queryData.OrgType).FirstOrDefault();
                if (orgInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "该组织不存在";
                    return result;
                }

                Expression<Func<CheLiangZuZhiXinXi, bool>> exp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgCode == queryData.OrgCode;

                Expression<Func<CheLiang, bool>> cheLiangExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;

                switch (queryData.OrgType)
                {
                    case OrganizationType.平台运营商:
                        break;
                    case OrganizationType.企业:
                    case OrganizationType.个体户:
                        cheLiangExp = cheLiangExp.And(c => c.YeHuOrgCode == orgInfo.OrgCode);
                        break;
                    case OrganizationType.市政府:
                        cheLiangExp = cheLiangExp.And(c => c.XiaQuShi == orgInfo.XiaQuShi);
                        break;
                    case OrganizationType.县政府:
                        cheLiangExp = cheLiangExp.And(c => c.XiaQuXian == orgInfo.XiaQuXian);
                        break;
                    default:
                        result.StatusCode = 2;
                        result.ErrorMessage = "组织类型不存在";
                        return result;
                }
                //var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                //            join b in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                //            on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = b.RegistrationNo, ChePaiYanSe = b.RegistrationNoColor }
                //            select new DangQianZuZhiDto
                //            {
                //                OrgCode = queryData.OrgCode,
                //                OrgName = queryData.OrgName,
                //                OrgType = queryData.OrgType,
                //                CheLiangId = a.Id.ToString(),
                //                ShiFouZaiXian = b.LatestGpsTime >= compareTime ? (int)ShiFouZaiXian.在线 : (int)ShiFouZaiXian.离线
                //            };

                var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                            select new DangQianZuZhiDto
                            {
                                OrgCode = queryData.OrgCode,
                                OrgName = queryData.OrgName,
                                OrgType = queryData.OrgType,
                                CheLiangId = a.Id.ToString(),
                                ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0
                            };

                if (query.Count() > 0)
                {
                    var dto = new JiGouJieGuoDto()
                    {
                        OrgCode = orgInfo.OrgCode,
                        OrgName = orgInfo.OrgName,
                        OrgType = orgInfo.OrgType.Value,
                        ParentOrgCode = string.Empty,
                        OrgZaiXianCheLiangShu = query.Count(l => l.ShiFouZaiXian.Value == (int)ZaiXianZhuangTai.在线),
                        OrgZongCheLiangShu = query.Count()
                    };
                    result.Data = dto;
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("调用 QueryDangQianZuZhiCheLiangZaiXianShu 出错," + JsonConvert.SerializeObject(queryData), ex);
                return new ServiceResult<JiGouJieGuoDto>() { StatusCode = 2, ErrorMessage = ex.Message };
            }
        }

        public ServiceResult<List<JiGouJieGuoDto>> QueryXiaJiZuZhi(CheLiangJianKongShuQueryDto queryData)
        {
            return ExecuteCommandClass<List<JiGouJieGuoDto>>(() =>
            {
                var result = new ServiceResult<List<JiGouJieGuoDto>>();
                result.Data = new List<JiGouJieGuoDto>();
                DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
                List<string> CheLiangList = new List<string>();

                var roleCode = queryData.RoleCode;
                var sysUserId = queryData.SysUserId;

                if (string.IsNullOrEmpty(queryData.OrgCode))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "OrgCode不能为空";
                    return result;
                }

                var orgInfo = _orgBaseInfoRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgCode == queryData.OrgCode).FirstOrDefault();
                if (orgInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "该组织不存在";
                    return result;
                }


                //switch (orgInfo.OrgType)
                //{
                //    case (int)OrganizationType.市政府:
                //        result = QueryZhengFuXiaJiAndCheLiangZaiXianShu(orgInfo);
                //        break;
                //    case (int)OrganizationType.县政府:
                //        result = QueryZhengFuXiaJiAndCheLiangZaiXianShu(orgInfo);
                //        break;
                //    case (int)OrganizationType.平台运营商:
                //        result = QueryZhengFuXiaJiAndCheLiangZaiXianShu(orgInfo);
                //        break;
                //    default:
                //        result.StatusCode = 2;
                //        result.ErrorMessage = "该组织不存在";
                //        break;
                //}

                return result;
            });
        }


        public ServiceResult<QueryResult> QueryXiaJiCheLiang(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                var result = new ServiceResult<QueryResult>() { StatusCode = 0, Data = new QueryResult() };
                CheLiangJianKongShuQueryDto body = JsonConvert.DeserializeObject<CheLiangJianKongShuQueryDto>(queryData.data.ToString());
                List<string> CheLiangList = new List<string>();
                DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

                if (IsGuanLiYuanRoleCode(body.RoleCode))
                {
                    CheLiangList = (from a in _cheLiangZuZhiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.OrgCode == body.OrgCode)
                                    select a.CheLiangId).ToList();
                }
                else
                {
                    CheLiangList = (from a in _yongHuCheLiangXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.SysUserId == body.SysUserId)
                                    select a.CheLiangId).ToList();
                }

                var list = from a in CheLiangList
                           join b in _cheLiangXinXiRepository.GetQuery(e => e.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                           on a equals b.Id.ToString()

                           //join e in _cheLiangDingWeiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                           //on new { b.ChePaiHao, b.ChePaiYanSe } equals new { ChePaiHao = e.RegistrationNo, ChePaiYanSe = e.RegistrationNoColor } into temp
                           //from t in temp.DefaultIfEmpty()

                           select new
                           {
                               ChePaiHao = b.ChePaiHao,
                               CheLiangZhongLei = b.CheLiangZhongLei,
                               ChePaiYanSe = b.ChePaiYanSe,
                               ShiFouZaiXian = b.ZaiXianZhuangTai.HasValue ? b.ZaiXianZhuangTai.Value == (int)ZaiXianZhuangTai.在线 : false//t.LatestGpsTime >= compareTime ? true : false,
                           };
                if (queryData.page == 0 && queryData.rows == 0)
                {
                    var rlist = list.Distinct().ToList();
                    result.Data.totalcount = rlist.Count;
                    result.Data.items = rlist;
                }
                else
                {
                    var qlist = list.Distinct();
                    result.Data.totalcount = qlist.Count();
                    var rlist = qlist.OrderByDescending(s => s.ChePaiHao).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                    result.Data.items = rlist;
                }

                return result;
            });
        }

        public ServiceResult<QueryResult> QueryCheLiangXinXi(QueryData queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                var result = new ServiceResult<QueryResult>();
                CheLiangCanShuDto body = JsonConvert.DeserializeObject<CheLiangCanShuDto>(queryData.data.ToString());
                List<string> CheLiangList = new List<string>();
                Expression<Func<CheLiang, bool>> exp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;
                var query = _cheLiangXinXiRepository.GetQuery().Where(exp);
                DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

                if (IsGuanLiYuanRoleCode(body.RoleCode))
                {
                    CheLiangList = (from a in _cheLiangZuZhiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.OrgCode == body.OrgCode)
                                    select a.CheLiangId).ToList();
                }
                else
                {
                    CheLiangList = (from a in _yongHuCheLiangXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.SysUserId == body.SysUserId)
                                    select a.CheLiangId).ToList();
                }


                if (!string.IsNullOrWhiteSpace(body.ChePaiHao))
                {
                    query = query.Where(u => u.ChePaiHao.Contains(body.ChePaiHao));
                }
                if (!string.IsNullOrWhiteSpace(body.ChePaiYanSe))
                {
                    query = query.Where(u => u.ChePaiYanSe == body.ChePaiYanSe);
                }

                var list = new List<CheLiangJieGuoDto>();
                //var queryResult = from a in query
                //                  join e in _cheLiangDingWeiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                //                  on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = e.RegistrationNo, ChePaiYanSe = e.RegistrationNoColor } into temp
                //                  from t in temp.DefaultIfEmpty()

                //                      //join m in _zhongDuanAnZhuangXinXiRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                //                      //on a.Id.ToString() equals m.CheLiangId into temp1
                //                      //from t1 in temp1.DefaultIfEmpty()

                //                      //join g in _cheLiangGPSZhongDuanPeiZhiXinXiRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                //                      //on t1.Id.ToString() equals g.ZhongDuanAnZhuangId into temp2
                //                      //from t2 in temp2.DefaultIfEmpty()

                //                  select new
                //                  {
                //                      CheLiangZhongLei = a.CheLiangZhongLei,
                //                      ChePaiHao = a.ChePaiHao,
                //                      ChePaiYanSe = a.ChePaiYanSe,
                //                      ShiFouZaiXian = t.LatestGpsTime >= compareTime ? (int)ShiFouZaiXian.在线 : (int)ShiFouZaiXian.离线
                //                      //IsHasShiPinTou = t2.ShiFouAnZhuangShiPinZhongDuan,
                //                      //ShiPingTouGeShu = t2.ShiPinTouGeShu
                //                  };

                var queryResult = from a in query
                                  select new
                                  {
                                      CheLiangZhongLei = a.CheLiangZhongLei,
                                      ChePaiHao = a.ChePaiHao,
                                      ChePaiYanSe = a.ChePaiYanSe,
                                      ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0//t.LatestGpsTime >= compareTime ? (int)ShiFouZaiXian.在线 : (int)ShiFouZaiXian.离线
                                      //IsHasShiPinTou = t2.ShiFouAnZhuangShiPinZhongDuan,
                                      //ShiPingTouGeShu = t2.ShiPinTouGeShu
                                  };

                if (body.ShiFouAnZhuangShiPingZhongDuan)
                {
                    //queryResult = queryResult.Where(u => u.ShiPingTouGeShu > 0);
                }

                queryResult.OrderByDescending(o => o.ShiFouZaiXian).ThenBy(o => o.ChePaiHao).Distinct();
                var count = queryResult.Count();
                queryResult.OrderByDescending(o => o.ShiFouZaiXian).ThenBy(o => o.ChePaiHao).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList().Each(u => list.Add(new CheLiangJieGuoDto()
                {
                    CheLiangZhongLei = Enum.GetName(typeof(Entities.Enums.CheLiangZhongLei), u.CheLiangZhongLei.GetValueOrDefault()),
                    ChePaiHao = u.ChePaiHao,
                    ChePaiYanSe = u.ChePaiYanSe,
                    ShiFouZaiXian = u.ShiFouZaiXian == (int)ShiFouZaiXian.在线,
                    //ShiPingTouGeShu = u.ShiPingTouGeShu
                }));
                result.Data = new QueryResult();
                result.Data.totalcount = count;
                result.Data.items = list;
                return result;
            });
        }

        public ServiceResult<List<JiGouResponseDto>> QueryQiYeHuoCheDui(QiYeCheLiangXinXiCanShuDto queryData)
        {
            return ExecuteCommandClass<List<JiGouResponseDto>>(() =>
            {
                List<string> CheLiangList = new List<string>();

                Expression<Func<OrgBaseInfo, bool>> exp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;
                var query = _orgBaseInfoRepository.GetQuery(exp);

                if (IsGuanLiYuanRoleCode(queryData.RoleCode))
                {
                    CheLiangList = (from a in _cheLiangZuZhiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.OrgCode == queryData.OrgCode)
                                    select a.CheLiangId).ToList();
                }
                else
                {
                    CheLiangList = (from a in _yongHuCheLiangXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.SysUserId == queryData.SysUserId)
                                    select a.CheLiangId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(queryData.OrgName))
                {
                    query = query.Where(u => u.OrgName.Contains(queryData.OrgName));
                }
                if (queryData.OrgType.HasValue)
                {
                    query = query.Where(u => u.OrgType == queryData.OrgType.Value);
                }

                var list = from a in query
                           join c in _cheLiangZuZhiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                           on a.OrgCode equals c.OrgCode into temp1
                           from t1 in temp1.DefaultIfEmpty()

                           join d in _cheLiangXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                           on t1.CheLiangId equals d.Id.ToString() into temp2
                           from t2 in temp2.DefaultIfEmpty()

                               //join e in _cheLiangDingWeiXinXiRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                               //on new { t2.ChePaiHao, t2.ChePaiYanSe } equals new { ChePaiHao = e.RegistrationNo, ChePaiYanSe = e.RegistrationNoColor } into temp3
                               //from t3 in temp3.DefaultIfEmpty()

                           where CheLiangList.Contains(t1.CheLiangId)
                           select new
                           {
                               OrgId = a.Id.ToString(),
                               OrgCode = a.OrgCode,
                               OrgName = a.OrgName,
                               OrgType = a.OrgType,
                               ShiFouZaiXian = t2.ZaiXianZhuangTai ?? 0, //t3.LatestGpsTime >= compareTime ? (int)ShiFouZaiXian.在线 : (int)ShiFouZaiXian.离线,
                               ParentOrgCode = "",
                               ParentOrgId = ""
                           };

                var tm = from o in list
                         group o by
                         new
                         {
                             o.OrgId,
                             o.OrgType,
                             o.OrgCode,
                             o.OrgName,
                             o.ParentOrgId,
                             o.ParentOrgCode
                         } into m
                         select new
                         {
                             OrgId = m.Key.OrgId,
                             OrgType = m.Key.OrgType,
                             OrgCode = m.Key.OrgCode,
                             OrgName = m.Key.OrgName,
                             ParentOrgId = m.Key.ParentOrgId,
                             ParentOrgCode = m.Key.ParentOrgCode,
                             OrgZaiXianCheLiangShu = list.Count(s => s.OrgId == m.Key.OrgId && s.ShiFouZaiXian == (int)ShiFouZaiXian.在线),
                             OrgZongCheLiangShu = list.Count(s => s.OrgId == m.Key.OrgId)
                         };

                if (tm != null && tm.Count() > 0)
                {
                    return new ServiceResult<List<JiGouResponseDto>>()
                    {
                        Data = tm.Select(m => new JiGouResponseDto
                        {
                            OrgId = m.OrgId,
                            OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), m.OrgType.GetValueOrDefault()),
                            OrgCode = m.OrgCode,
                            OrgName = m.OrgName,
                            ParentOrgId = m.ParentOrgId,
                            ParentOrgCode = m.ParentOrgCode,
                            OrgZaiXianCheLiangShu = m.OrgZaiXianCheLiangShu,
                            OrgZongCheLiangShu = m.OrgZongCheLiangShu
                        }).OrderBy(u => u.OrgName).ToList()
                    };
                }
                else
                {
                    return new ServiceResult<List<JiGouResponseDto>>() { Data = new List<JiGouResponseDto>() };
                }
            });
        }

        public ServiceResult<List<JiGouResponseDto>> QueryQiYeHuoCheDuiV2(QiYeCheLiangXinXiCanShuDto queryData)
        {
            return ExecuteCommandClass<List<JiGouResponseDto>>(() =>
            {
                var result = new ServiceResult<List<JiGouResponseDto>>();

                if (queryData == null || !queryData.OrgType.HasValue || string.IsNullOrWhiteSpace(queryData.OrgName))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "请求参数不能为空";
                    return result;
                }

                var userInfo = GetUserInfo();
                if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.Id) || !userInfo.OrganizationType.HasValue)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "获取登录用户信息失败";
                    return result;
                }


                List<JiGouResponseDto> orgList = new List<JiGouResponseDto>();

                if (IsGuanLiYuanRoleCode(queryData.RoleCode))
                {
                    //获取当前登录组织的车辆
                    Expression<Func<CheLiang, bool>> cheLiangExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;

                    switch (userInfo.OrganizationType.Value)
                    {
                        case (int)OrganizationType.平台运营商:
                            break;
                        case (int)OrganizationType.企业:
                        case (int)OrganizationType.个体户:
                            cheLiangExp = cheLiangExp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
                            break;
                        case (int)OrganizationType.车队:
                            cheLiangExp = cheLiangExp.And(c => c.CheDuiOrgCode == userInfo.OrganizationCode);
                            break;
                        case (int)OrganizationType.市政府:
                            cheLiangExp = cheLiangExp.And(c => c.XiaQuShi == userInfo.OrganCity);
                            break;
                        case (int)OrganizationType.县政府:
                            cheLiangExp = cheLiangExp.And(c => c.XiaQuXian == userInfo.OrganDistrict);
                            break;
                        default:
                            result.StatusCode = 2;
                            result.ErrorMessage = "当前登录组织类型不存在";
                            return result;
                    }

                    //按查询的组织类型区分
                    Expression<Func<OrgBaseInfo, bool>> orgExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgType == queryData.OrgType && p.OrgName.Contains(queryData.OrgName.Trim());

                    switch (queryData.OrgType.Value)
                    {
                        case (int)OrganizationType.企业:
                        case (int)OrganizationType.个体户:
                            var yhOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                                            join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                            on c.YeHuOrgCode equals o.OrgCode
                                            select new
                                            {
                                                OrgId = o.Id.ToString(),
                                                OrgCode = o.OrgCode,
                                                OrgName = o.OrgName,
                                                OrgType = o.OrgType,
                                                ParentOrgId = o.ParentOrgId,
                                                CheLiangId = c.Id,
                                                ShiFouZaiXian = c.ZaiXianZhuangTai
                                            }).ToList();
                            if (yhOrgCLs != null && yhOrgCLs.Count > 0)
                            {
                                orgList = yhOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        case (int)OrganizationType.车队:
                            var cdOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                                            join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                            on c.CheDuiOrgCode equals o.OrgCode
                                            select new
                                            {
                                                OrgId = o.Id.ToString(),
                                                OrgCode = o.OrgCode,
                                                OrgName = o.OrgName,
                                                OrgType = o.OrgType,
                                                ParentOrgId = o.ParentOrgId,
                                                CheLiangId = c.Id,
                                                ShiFouZaiXian = c.ZaiXianZhuangTai
                                            }).ToList();
                            if (cdOrgCLs != null && cdOrgCLs.Count > 0)
                            {
                                orgList = cdOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        case (int)OrganizationType.本地服务商:
                            var fwsOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                                             join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                             on c.FuWuShangOrgCode equals o.OrgCode
                                             select new
                                             {
                                                 OrgId = o.Id.ToString(),
                                                 OrgCode = o.OrgCode,
                                                 OrgName = o.OrgName,
                                                 OrgType = o.OrgType,
                                                 ParentOrgId = o.ParentOrgId,
                                                 CheLiangId = c.Id,
                                                 ShiFouZaiXian = c.ZaiXianZhuangTai
                                             }).ToList();
                            if (fwsOrgCLs != null && fwsOrgCLs.Count > 0)
                            {
                                orgList = fwsOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        default:
                            result.StatusCode = 2;
                            result.ErrorMessage = "当前登录组织类型不存在";
                            return result;
                    }
                }
                else
                {
                    //监控的车辆
                    Expression<Func<YongHuCheLiangXinXi, bool>> yhclExp = u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && u.SysUserId == queryData.SysUserId;

                    //按查询的组织类型区分
                    Expression<Func<OrgBaseInfo, bool>> orgExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.OrgType == queryData.OrgType && p.OrgName.Contains(queryData.OrgName.Trim());

                    switch (queryData.OrgType.Value)
                    {
                        case (int)OrganizationType.企业:
                        case (int)OrganizationType.个体户:
                            var yhOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(c => c.SYS_XiTongZhuangTai == 0)
                                            join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                            on c.YeHuOrgCode equals o.OrgCode
                                            join y in _yongHuCheLiangXinXiRepository.GetQuery(yhclExp)
                                            on c.Id.ToString() equals y.CheLiangId
                                            select new
                                            {
                                                OrgId = o.Id.ToString(),
                                                OrgCode = o.OrgCode,
                                                OrgName = o.OrgName,
                                                OrgType = o.OrgType,
                                                ParentOrgId = o.ParentOrgId,
                                                CheLiangId = c.Id,
                                                ShiFouZaiXian = c.ZaiXianZhuangTai
                                            }).ToList();
                            if (yhOrgCLs != null && yhOrgCLs.Count > 0)
                            {
                                orgList = yhOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        case (int)OrganizationType.车队:
                            var cdOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(c => c.SYS_XiTongZhuangTai == 0)
                                            join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                            on c.CheDuiOrgCode equals o.OrgCode
                                            join y in _yongHuCheLiangXinXiRepository.GetQuery(yhclExp)
                                            on c.Id.ToString() equals y.CheLiangId
                                            select new
                                            {
                                                OrgId = o.Id.ToString(),
                                                OrgCode = o.OrgCode,
                                                OrgName = o.OrgName,
                                                OrgType = o.OrgType,
                                                ParentOrgId = o.ParentOrgId,
                                                CheLiangId = c.Id,
                                                ShiFouZaiXian = c.ZaiXianZhuangTai
                                            }).ToList();
                            if (cdOrgCLs != null && cdOrgCLs.Count > 0)
                            {
                                orgList = cdOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        case (int)OrganizationType.本地服务商:
                            var fwsOrgCLs = (from c in _cheLiangXinXiRepository.GetQuery(c => c.SYS_XiTongZhuangTai == 0)
                                             join o in _orgBaseInfoRepository.GetQuery(orgExp)
                                             on c.FuWuShangOrgCode equals o.OrgCode
                                             join y in _yongHuCheLiangXinXiRepository.GetQuery(yhclExp)
                                             on c.Id.ToString() equals y.CheLiangId
                                             select new
                                             {
                                                 OrgId = o.Id.ToString(),
                                                 OrgCode = o.OrgCode,
                                                 OrgName = o.OrgName,
                                                 OrgType = o.OrgType,
                                                 ParentOrgId = o.ParentOrgId,
                                                 CheLiangId = c.Id,
                                                 ShiFouZaiXian = c.ZaiXianZhuangTai
                                             }).ToList();
                            if (fwsOrgCLs != null && fwsOrgCLs.Count > 0)
                            {
                                orgList = fwsOrgCLs.GroupBy(a => new
                                {
                                    a.OrgId,
                                    a.OrgType,
                                    a.OrgCode,
                                    a.OrgName,
                                    a.ParentOrgId
                                })
                                .Select(a => new JiGouResponseDto
                                {
                                    OrgId = a.Key.OrgId,
                                    OrgType = Enum.GetName(typeof(Entities.Enums.OrganizationType), a.Key.OrgType.GetValueOrDefault()),
                                    OrgCode = a.Key.OrgCode,
                                    OrgName = a.Key.OrgName,
                                    ParentOrgId = a.Key.ParentOrgId.HasValue ? a.Key.ParentOrgId.Value.ToString() : "",
                                    OrgZaiXianCheLiangShu = a.Where(c => c.ShiFouZaiXian == (int)ShiFouZaiXian.在线).Count(c => c.CheLiangId != Guid.Empty),
                                    OrgZongCheLiangShu = a.Count(c => c.CheLiangId != Guid.Empty)
                                }).ToList();
                            }
                            break;
                        default:
                            result.StatusCode = 2;
                            result.ErrorMessage = "当前登录组织类型不存在";
                            return result;
                    }
                }

                result.StatusCode = 0;
                result.Data = orgList;
                return result;
            });
        }

        /// <summary>
        /// 获取车辆相关的组织和用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<CheLiangOrgsOutputDto> QueryCheLiangOrgAndUser(CheLiangOrgsInputDto dto)
        {
            return ExecuteCommandClass<CheLiangOrgsOutputDto>(() =>
            {
                if (dto == null)
                {
                    return new ServiceResult<CheLiangOrgsOutputDto>() { StatusCode = 2, ErrorMessage = "请求参数不能为空" };
                }
                if (string.IsNullOrWhiteSpace(dto.ChePaiHao) || string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    return new ServiceResult<CheLiangOrgsOutputDto>() { StatusCode = 2, ErrorMessage = "车牌号或车牌颜色不能为空" };
                }
                var cheliang = _cheLiangXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.ChePaiHao == dto.ChePaiHao && x.ChePaiYanSe == dto.ChePaiYanSe).FirstOrDefault();
                if (cheliang == null)
                {
                    return new ServiceResult<CheLiangOrgsOutputDto>() { StatusCode = 2, ErrorMessage = "未找到该车辆数据" };
                }

                List<string> orgCodes = new List<string>();

                //业户
                if (!string.IsNullOrWhiteSpace(cheliang.YeHuOrgCode))
                {
                    orgCodes.Add(cheliang.YeHuOrgCode);
                }
                //车队
                if (!string.IsNullOrWhiteSpace(cheliang.CheDuiOrgCode))
                {
                    orgCodes.Add(cheliang.CheDuiOrgCode);
                }
                //市政府
                if (!string.IsNullOrWhiteSpace(cheliang.XiaQuShi))
                {
                    var shiZhengFu = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.OrgType == (int)OrganizationType.市政府 && x.XiaQuShi == cheliang.XiaQuShi).FirstOrDefault();
                    if (shiZhengFu != null)
                    {
                        orgCodes.Add(shiZhengFu.OrgCode);
                    }
                }
                //县政府
                if (!string.IsNullOrWhiteSpace(cheliang.XiaQuXian))
                {
                    var xianZhengFu = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.OrgType == (int)OrganizationType.县政府 && x.XiaQuXian == cheliang.XiaQuXian).FirstOrDefault();
                    if (xianZhengFu != null)
                    {
                        orgCodes.Add(xianZhengFu.OrgCode);
                    }
                }

                return new ServiceResult<CheLiangOrgsOutputDto>()
                {
                    StatusCode = 0,
                    Data = new CheLiangOrgsOutputDto()
                    {
                        OrgCode = orgCodes,
                        SysUserId = new List<string>()
                    }
                };
            });
        }

        #region 查询车辆视频监控树内容（下级组织或车辆列表）

        /// <summary>
        /// 查询车辆视频监控树内容（下级组织或车辆列表）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<JianKongShuResult> QueryVideoJianKongShu(VideoJianKongShuQueryDto dto)
        {
            return ExecuteCommandStruct<JianKongShuResult>(() =>
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                var result = new ServiceResult<JianKongShuResult>();

                if (dto == null || string.IsNullOrWhiteSpace(dto.OrgCode))
                {
                    //查询当前登录组织
                    result = V_CurrentOrgAndCheLiangShu(userInfoNew);
                }
                else
                {
                    //查询组织信息
                    OrgBaseInfo orgInfo;
                    if (dto.OrgCode == AdminOrgCode)
                    {
                        //平台运营商没有组织数据保存，故直接实例化平台运营商的信息
                        orgInfo = new OrgBaseInfo { OrgCode = AdminOrgCode, OrgType = (int)OrganizationType.平台运营商 };
                    }
                    else
                    {
                        //除平台运营商外，皆可在数据库查到组织记录
                        var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                        orgInfo = _orgBaseInfoRepository.GetQuery(o => o.SYS_XiTongZhuangTai == sysZhengChang && o.OrgCode == dto.OrgCode.Trim()).FirstOrDefault();
                    }
                    if (orgInfo == null || string.IsNullOrWhiteSpace(orgInfo.OrgCode))
                    {
                        return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = $"未找到该组织信息（{ dto.OrgCode}）" };
                    }

                    //按组织类型查询下级组织或车辆
                    switch (orgInfo.OrgType.Value)
                    {
                        case (int)OrganizationType.平台运营商:
                            result = V_PingTaiYunYingShangOrgAndCheLiangShu(orgInfo);
                            break;
                        case (int)OrganizationType.市政府:
                            result = V_ZhengFuOrgAndCheLiangShu(orgInfo);
                            break;
                        case (int)OrganizationType.县政府:
                            result = V_ZhengFuOrgAndCheLiangShu(orgInfo);
                            break;
                        case (int)OrganizationType.个体户:
                        case (int)OrganizationType.企业:
                            result = V_YeHuCheLiang(orgInfo, userInfoNew);
                            break;
                        default:
                            result = new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = $"该组织类型不存在（{ orgInfo.OrgType}）" };
                            break;
                    }
                }
                return result;
            });
        }

        /// <summary>
        /// （终端视频）查询当前登录组织及车辆数
        /// </summary>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_CurrentOrgAndCheLiangShu(UserInfoDtoNew userInfoNew)
        {
            var dto = new VideoOrgItem()
            {
                OrgCode = userInfoNew.OrganizationCode,
                OrgName = userInfoNew.OrganizationName,
                OrgType = userInfoNew.OrganizationType.Value,
                ParentOrgCode = string.Empty
            };
            return new ServiceResult<JianKongShuResult>()
            {
                StatusCode = 0,
                Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = new List<VideoOrgItem> { dto }
                }
            };
        }

        /// <summary>
        /// （终端视频）查询平台运营商下级组织及车辆数
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_PingTaiYunYingShangOrgAndCheLiangShu(OrgBaseInfo org)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            var yeHuOrgQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == sysZhengChang)
                                   join x in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == sysZhengChang) on u.YeHuOrgCode equals x.OrgCode
                                   join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang) on u.Id.ToString() equals a.CheLiangId
                                   select new
                                   {
                                       x.OrgCode,
                                       x.OrgName,
                                       x.OrgType
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = org.OrgCode
                    }).Distinct().ToList()
                }
            };
        }

        /// <summary>
        /// （终端视频）查询政府（市/县）下级组织及车辆数
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_ZhengFuOrgAndCheLiangShu(OrgBaseInfo org)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang;
            if (org.OrgType == (int)OrganizationType.市政府)
            {
                clExp = clExp.And(c => c.XiaQuShi == org.XiaQuShi);
            }
            else if (org.OrgType == (int)OrganizationType.县政府)
            {
                clExp = clExp.And(c => c.XiaQuXian == org.XiaQuXian);
            }
            else
            {
                return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "无效的政府组织类型" };
            }

            var yeHuOrgQueryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                                   join x in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == sysZhengChang)
                                   on u.YeHuOrgCode equals x.OrgCode
                                   join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                   on u.Id.ToString() equals a.CheLiangId
                                   select new
                                   {
                                       x.OrgCode,
                                       x.OrgName,
                                       x.OrgType,
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = org.OrgCode
                    }).Distinct().ToList()
                }
            };
        }

        /// <summary>
        /// （终端视频）查询业户（个体户/企业）下级车辆
        /// </summary>
        /// <param name="org"></param>
        /// <param name="userInfoNew"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_YeHuCheLiang(OrgBaseInfo org, UserInfoDtoNew userInfoNew)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

            var cheLiangList = (from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == sysZhengChang && p.YeHuOrgCode == org.OrgCode)
                                join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.Id.ToString() equals a.CheLiangId
                                //join d in _cheLiangDingWeiXinXiRepository.GetQuery(d => d.SYS_XiTongZhuangTai == sysZhengChang)
                                //on new { RegistrationNo = u.ChePaiHao, RegistrationNoColor = u.ChePaiYanSe } equals new { d.RegistrationNo, d.RegistrationNoColor } into temp1
                                //from dw in temp1.DefaultIfEmpty()
                                select new
                                {
                                    u.ChePaiHao,
                                    u.ChePaiYanSe,
                                    u.CheLiangZhongLei,
                                    ShiFouZaiXian = u.ZaiXianZhuangTai.HasValue ? u.ZaiXianZhuangTai.Value == (int)ZaiXianZhuangTai.在线 : false,
                                    //dw.LatestGpsTime,
                                    a.ShiPinTouGeShu,
                                    a.ShiPinTouAnZhuangXuanZe,
                                    a.ShiPingChangShangLeiXing
                                }).ToList();
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 1,
                    Items = cheLiangList.Select(x => new VideoCheLiangItem()
                    {
                        ChePaiHao = x.ChePaiHao,
                        ChePaiYanSe = x.ChePaiYanSe,
                        CheLiangZhongLei = x.CheLiangZhongLei,
                        ShiFouZaiXian = x.ShiFouZaiXian,//CheLiangShiFouZaiXian(x.LatestGpsTime),
                        ShiPinTouGeShu = x.ShiPinTouGeShu,
                        CameraSelected = x.ShiPinTouAnZhuangXuanZe,
                        VideoServiceKind = x.ShiPingChangShangLeiXing
                    }).Distinct().ToList()
                }
            };
        }

        private bool? CheLiangShiFouZaiXian(DateTime? latestGpsTime)
        {
            if (latestGpsTime.HasValue)
            {
                //是否在线判断规则：
                //车辆最后一次定位时间在 2 小时以内
                DateTime compareTime = DateTime.Now;
                return Math.Abs((compareTime - latestGpsTime.Value).TotalHours) <= 2;
            }
            else
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 获取车辆监控企业及车辆数列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<List<QueryTreeYeHuListDto>> QueryJianKongYeHuAndCheLiangShu(QueryTreeYeHuReqDto queryData)
        {
            return ExecuteCommandClass<List<QueryTreeYeHuListDto>>(() =>
            {
                var result = new ServiceResult<List<QueryTreeYeHuListDto>>();

                var userInfo = GetUserInfo();
                if (string.IsNullOrWhiteSpace(queryData.OrgName))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "业户名称不能为空";
                    return result;
                }

                //查询过滤
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                //查询业户组织
                List<int?> orgTypes = new List<int?>() { (int)OrganizationType.企业, (int)OrganizationType.个体户 };
                Expression<Func<CheLiang, bool>> cheliangExp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<OrgBaseInfo, bool>> yehuExp = t => t.SYS_XiTongZhuangTai == sysZhengChang && orgTypes.Contains(t.OrgType) && t.OrgName.Contains(queryData.OrgName.Trim());

                //权限判断
                switch (userInfo.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        cheliangExp = cheliangExp.And(t => t.XiaQuShi == userInfo.OrganCity);
                        break;
                    case (int)OrganizationType.县政府:
                        cheliangExp = cheliangExp.And(t => t.XiaQuXian == userInfo.OrganDistrict);
                        break;
                    case (int)OrganizationType.平台运营商:
                        break;
                    case (int)OrganizationType.企业:
                    case (int)OrganizationType.个体户:
                        yehuExp = yehuExp.And(t => t.OrgCode == userInfo.OrganizationCode);
                        break;
                    default:
                        result.StatusCode = 2;
                        result.ErrorMessage = "当前组织不存在";
                        break;
                }

                //是否管理员
                var isGuanLiYuan = IsGuanLiYuanRoleCode(userInfo.RoleCode);
                IQueryable<JianKongTreeYeHuListQueryDto> yehucheliangQuery = null;
                if (isGuanLiYuan)
                {
                    yehucheliangQuery = from c in _cheLiangXinXiRepository.GetQuery(cheliangExp)
                                        join o in _orgBaseInfoRepository.GetQuery(yehuExp)
                                        on c.YeHuOrgCode equals o.OrgCode
                                        select new JianKongTreeYeHuListQueryDto
                                        {
                                            OrgType = o.OrgType,
                                            OrgCode = o.OrgCode,
                                            OrgName = o.OrgName,
                                            ChePaiHao = c.ChePaiHao,
                                            ChePaiYanSe = c.ChePaiYanSe,
                                            ZaiXianZhuangTai = c.ZaiXianZhuangTai
                                        };
                }
                else
                {
                    yehucheliangQuery = from c in _cheLiangXinXiRepository.GetQuery(cheliangExp)
                                        join o in _orgBaseInfoRepository.GetQuery(yehuExp)
                                        on c.YeHuOrgCode equals o.OrgCode
                                        join y in _yongHuCheLiangXinXiRepository.GetQuery(t => t.SYS_XiTongZhuangTai == sysZhengChang && t.SysUserId == userInfo.Id)
                                        on c.Id.ToString() equals y.CheLiangId
                                        select new JianKongTreeYeHuListQueryDto
                                        {
                                            OrgType = o.OrgType,
                                            OrgCode = o.OrgCode,
                                            OrgName = o.OrgName,
                                            ChePaiHao = c.ChePaiHao,
                                            ChePaiYanSe = c.ChePaiYanSe,
                                            ZaiXianZhuangTai = c.ZaiXianZhuangTai
                                        };
                }

                result.StatusCode = 0;
                result.Data = (from c in yehucheliangQuery
                               group c by new { c.OrgType, c.OrgCode, c.OrgName } into d
                               select new QueryTreeYeHuListDto
                               {
                                   OrgType = d.Key.OrgType,
                                   OrgCode = d.Key.OrgCode,
                                   OrgName = d.Key.OrgName,
                                   OrgZongCheLiangShu = d.Count(),
                                   OrgZaiXianCheLiangShu = d.Count(s => s.ZaiXianZhuangTai == (int)ZaiXianZhuangTai.在线),
                                   ParentOrgCode = userInfo.OrganizationCode
                               }).ToList();
                return result;
            }, e =>
            {
                LogHelper.Error("QueryJianKongYeHuAndCheLiangShu异常", e);
            });
        }

        public class DangQianZuZhiDto
        {
            public string OrgID { get; set; }
            public string OrgCode { get; set; }
            public string OrgName { get; set; }

            public OrganizationType OrgType { get; set; }

            public string CheLiangId { get; set; }

            public int? ShiFouZaiXian { get; set; }

        }
    }


    /// <summary>
    /// 私有方法
    /// </summary>
    public partial class CheLiangJianKongService
    {
        #region 车辆监控树节点

        /// <summary>
        /// 政府下级组织及车辆在线数统计
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> QueryZhengFuXiaJiAndCheLiangZaiXianShu(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo)
        {
            var result = new ServiceResult<JianKongShuResult>() { Data = new JianKongShuResult { Items = new List<string>() } };
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

            if (IsGuanLiYuanRoleCode(userInfoDto.RoleCode))
            {
                if (userInfoDto.OrganizationCode == orgInfo.OrgCode)
                {
                    #region 查询政府下级企业列表

                    var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuShi == userInfoDto.OrganCity)
                                            join x in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            on u.YeHuOrgCode equals x.OrgCode
                                            //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                            //into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                            select new
                                            {
                                                u.ChePaiHao,
                                                u.ChePaiYanSe,
                                                x.OrgCode,
                                                x.OrgName,
                                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                                                //ZuiJinDingWeiShiJian = tempItem.LatestGpsTime ?? DateTime.MinValue
                                            };

                    var yeHuOrgQueryList = from a in cheLiangQueryList
                                           group a by new { a.OrgCode, a.OrgName } into c
                                           select new OrgItem
                                           {
                                               OrgCode = c.Key.OrgCode,
                                               OrgName = c.Key.OrgName,
                                               ParentOrgCode = userInfoDto.OrganizationCode,
                                               OrgZongCheLiangShu = c.Count(),
                                               //OrgZaiXianCheLiangShu = c.Count(s => s.ZuiJinDingWeiShiJian >= compareTime)
                                               OrgZaiXianCheLiangShu = c.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                           };
                    result.Data = new JianKongShuResult
                    {
                        NodeType = 0,
                        Items = yeHuOrgQueryList.ToList()
                    };

                    #endregion 查询政府下级企业列表
                }
                else
                {
                    if (orgInfo.OrgType == (int)OrganizationType.企业 || orgInfo.OrgType == (int)OrganizationType.个体户)
                    {
                        #region 查询企业或个体户车辆列表

                        //var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuShi == userInfoDto.OrganCity && p.YeHuOrgCode == orgInfo.OrgCode)
                        //                        join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                        //                        on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                        //                         into temp
                        //                        from tempItem in temp.DefaultIfEmpty()
                        //                        select new CheLiangItem
                        //                        {
                        //                            ChePaiHao = u.ChePaiHao,
                        //                            ChePaiYanSe = u.ChePaiYanSe,
                        //                            CheLiangZhongLei = u.CheLiangZhongLei.Value,
                        //                            ShiFouZaiXian = (tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime
                        //                        };


                        var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuShi == userInfoDto.OrganCity && p.YeHuOrgCode == orgInfo.OrgCode)
                                                    //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                                    //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                                    // into temp
                                                    //from tempItem in temp.DefaultIfEmpty()
                                                select new CheLiangItem
                                                {
                                                    ChePaiHao = u.ChePaiHao,
                                                    ChePaiYanSe = u.ChePaiYanSe,
                                                    CheLiangZhongLei = u.CheLiangZhongLei.Value,
                                                    ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线
                                                };

                        result.Data = new JianKongShuResult
                        {
                            NodeType = 1,
                            Items = cheLiangQueryList.ToList()
                        };

                        #endregion 查询企业或个体户车辆列表
                    }
                }

            }
            else
            {
                if (userInfoDto.OrganizationCode == orgInfo.OrgCode)
                {
                    var cheLiangQueryList = from a in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuShi == userInfoDto.OrganCity)
                                            join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                                            on a.Id equals Guid.Parse(b.CheLiangId)
                                            join c in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            on a.YeHuOrgCode equals c.OrgCode
                                            //join d in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = d.RegistrationNo, ChePaiYanSe = d.RegistrationNoColor }
                                            // into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                            select new
                                            {
                                                a.ChePaiHao,
                                                a.ChePaiYanSe,
                                                c.OrgCode,
                                                c.OrgName,
                                                ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0
                                                //ZuiJinDingWeiShiJian = tempItem.LatestGpsTime ?? DateTime.MinValue
                                            };

                    var yeHuOrgQueryList = from a in cheLiangQueryList
                                           group a by new { a.OrgCode, a.OrgName } into c
                                           select new OrgItem
                                           {
                                               OrgCode = c.Key.OrgCode,
                                               OrgName = c.Key.OrgName,
                                               ParentOrgCode = userInfoDto.OrganizationCode,
                                               OrgZongCheLiangShu = c.Count(),
                                               //OrgZaiXianCheLiangShu = c.Count(s => s.ZuiJinDingWeiShiJian >= compareTime)
                                               OrgZaiXianCheLiangShu = c.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                           };
                    result.Data = new JianKongShuResult
                    {
                        NodeType = 0,
                        Items = yeHuOrgQueryList.ToList()
                    };
                }
                else
                {
                    if (orgInfo.OrgType == (int)OrganizationType.企业 || orgInfo.OrgType == (int)OrganizationType.个体户)
                    {
                        //查询企业或个体户车辆列表
                        result = QueryUserCheLiangList(userInfoDto, orgInfo);


                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 县政府下级组织及车辆在线数统计
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> QueryXianZhengFuXiaJiAndCheLiangZaiXianShu(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo)
        {
            var result = new ServiceResult<JianKongShuResult>() { Data = new JianKongShuResult { Items = new List<string>() } };
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

            if (IsGuanLiYuanRoleCode(userInfoDto.RoleCode))
            {
                if (userInfoDto.OrganizationCode == orgInfo.OrgCode)
                {
                    #region 查询政府下级企业列表

                    var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuXian == userInfoDto.OrganDistrict)
                                            join x in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            on u.YeHuOrgCode equals x.OrgCode
                                            //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                            // into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                            select new
                                            {
                                                u.ChePaiHao,
                                                u.ChePaiYanSe,
                                                x.OrgCode,
                                                x.OrgName,
                                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                                                //ZuiJinDingWeiShiJian = tempItem.LatestGpsTime ?? DateTime.MinValue
                                            };

                    var yeHuOrgQueryList = from a in cheLiangQueryList
                                           group a by new { a.OrgCode, a.OrgName } into c
                                           select new OrgItem
                                           {
                                               OrgCode = c.Key.OrgCode,
                                               OrgName = c.Key.OrgName,
                                               ParentOrgCode = userInfoDto.OrganizationCode,
                                               OrgZongCheLiangShu = c.Count(),
                                               //OrgZaiXianCheLiangShu = c.Count(s => s.ZuiJinDingWeiShiJian >= compareTime)
                                               OrgZaiXianCheLiangShu = c.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                           };
                    result.Data = new JianKongShuResult
                    {
                        NodeType = 0,
                        Items = yeHuOrgQueryList.ToList()
                    };

                    #endregion 查询政府下级企业列表
                }
                else
                {
                    if (orgInfo.OrgType == (int)OrganizationType.企业 || orgInfo.OrgType == (int)OrganizationType.个体户)
                    {
                        #region 查询企业或个体户车辆列表

                        var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuXian == userInfoDto.OrganDistrict && p.YeHuOrgCode == orgInfo.OrgCode)
                                                    //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                                    //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                                    // into temp
                                                    //from tempItem in temp.DefaultIfEmpty()
                                                select new CheLiangItem
                                                {
                                                    ChePaiHao = u.ChePaiHao,
                                                    ChePaiYanSe = u.ChePaiYanSe,
                                                    CheLiangZhongLei = u.CheLiangZhongLei.Value,
                                                    ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime
                                                };

                        result.Data = new JianKongShuResult
                        {
                            NodeType = 1,
                            Items = cheLiangQueryList.ToList()
                        };

                        #endregion 查询企业或个体户车辆列表
                    }
                }

            }
            else
            {
                if (userInfoDto.OrganizationCode == orgInfo.OrgCode)
                {
                    var cheLiangQueryList = from a in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuXian == userInfoDto.OrganDistrict)
                                            join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                                            on a.Id equals Guid.Parse(b.CheLiangId)
                                            join c in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            on a.YeHuOrgCode equals c.OrgCode
                                            //join d in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = d.RegistrationNo, ChePaiYanSe = d.RegistrationNoColor }
                                            //  into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                            select new
                                            {
                                                a.ChePaiHao,
                                                a.ChePaiYanSe,
                                                c.OrgCode,
                                                c.OrgName,
                                                ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0
                                                //ZuiJinDingWeiShiJian = tempItem.LatestGpsTime ?? DateTime.MinValue
                                            };

                    var yeHuOrgQueryList = from a in cheLiangQueryList
                                           group a by new { a.OrgCode, a.OrgName } into c
                                           select new OrgItem
                                           {
                                               OrgCode = c.Key.OrgCode,
                                               OrgName = c.Key.OrgName,
                                               ParentOrgCode = userInfoDto.OrganizationCode,
                                               OrgZongCheLiangShu = c.Count(),
                                               //OrgZaiXianCheLiangShu = c.Count(s => s.ZuiJinDingWeiShiJian >= compareTime)
                                               OrgZaiXianCheLiangShu = c.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                           };
                    result.Data = new JianKongShuResult
                    {
                        NodeType = 0,
                        Items = yeHuOrgQueryList.ToList()
                    };
                }
                else
                {
                    if (orgInfo.OrgType == (int)OrganizationType.企业 || orgInfo.OrgType == (int)OrganizationType.个体户)
                    {
                        //查询企业或个体户车辆列表
                        result = QueryUserCheLiangList(userInfoDto, orgInfo);


                    }
                }
            }

            return result;
        }

        private ServiceResult<JianKongShuResult> QueryPingTaiYunYingShangXiaJiAndCheLiangZaiXianShu(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo)
        {
            var result = new ServiceResult<JianKongShuResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);


            if (userInfoDto.OrganizationCode == orgInfo.OrgCode)
            {
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        join x in _orgBaseInfoRepository.GetQuery(u => u.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode equals x.OrgCode
                                        //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                        //into temp
                                        //from tempItem in temp.DefaultIfEmpty()
                                        select new
                                        {
                                            u.ChePaiHao,
                                            u.ChePaiYanSe,
                                            x.OrgCode,
                                            x.OrgName,
                                            ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                                            //ZuiJinDingWeiShiJian = tempItem.LatestGpsTime ?? DateTime.MinValue
                                        };

                var yeHuOrgQueryList = from a in cheLiangQueryList
                                       group a by new { a.OrgCode, a.OrgName } into c
                                       select new OrgItem
                                       {
                                           OrgCode = c.Key.OrgCode,
                                           OrgName = c.Key.OrgName,
                                           ParentOrgCode = userInfoDto.OrganizationCode,
                                           OrgZongCheLiangShu = c.Count(),
                                           //OrgZaiXianCheLiangShu = c.Count(s => s.ZuiJinDingWeiShiJian >= compareTime)
                                           OrgZaiXianCheLiangShu = c.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                       };
                result.Data = new JianKongShuResult
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.ToList()
                };
            }
            else
            {
                if (orgInfo.OrgType == (int)OrganizationType.企业 || orgInfo.OrgType == (int)OrganizationType.个体户)
                {
                    #region 查询企业或个体户车辆列表

                    var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgCode == orgInfo.OrgCode)
                                                //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                                //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                                //  into temp
                                                //from tempItem in temp.DefaultIfEmpty()

                                            select new CheLiangItem
                                            {
                                                ChePaiHao = u.ChePaiHao,
                                                ChePaiYanSe = u.ChePaiYanSe,
                                                CheLiangZhongLei = u.CheLiangZhongLei.Value,
                                                //ShiFouZaiXian = (tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime
                                                ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线
                                            };

                    result.Data = new JianKongShuResult
                    {
                        NodeType = 1,
                        Items = cheLiangQueryList.ToList()
                    };

                    #endregion 查询企业或个体户车辆列表
                }
            }


            return result;
        }

        private ServiceResult<JianKongShuResult> QueryYeHuXiaJiAndCheLiangZaiXianShu(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo)
        {
            var result = new ServiceResult<JianKongShuResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
            if (IsGuanLiYuanRoleCode(userInfoDto.RoleCode))
            {
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgCode == orgInfo.OrgCode)
                                            //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                            //  into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                        select new CheLiangItem
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei.Value,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线
                                            //ShiFouZaiXian = (tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime
                                        };

                result.Data = new JianKongShuResult
                {
                    NodeType = 1,
                    Items = cheLiangQueryList.ToList()
                };
            }
            else
            {
                result = QueryUserCheLiangList(userInfoDto, orgInfo);
            }


            return result;
        }


        private ServiceResult<JianKongShuResult> QueryZuZhiCheLiangShu(UserInfoDtoNew userInfoDto)
        {
            var result = new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult
                {
                    Items = new List<OrgItem>()
                }
            };
            Expression<Func<CheLiang, bool>> cheLiangExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;
            switch (userInfoDto.OrganizationType.Value)
            {
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                case (int)OrganizationType.个体户:
                    cheLiangExp = cheLiangExp.And(c => c.YeHuOrgCode == userInfoDto.OrganizationCode);
                    break;
                case (int)OrganizationType.市政府:
                    cheLiangExp = cheLiangExp.And(c => c.XiaQuShi == userInfoDto.OrganCity);
                    break;
                case (int)OrganizationType.县政府:
                    cheLiangExp = cheLiangExp.And(c => c.XiaQuXian == userInfoDto.OrganDistrict);
                    break;
                default:
                    result.StatusCode = 2;
                    result.ErrorMessage = "组织类型不存在";
                    return result;
            }
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
            if (IsGuanLiYuanRoleCode(userInfoDto.RoleCode))
            {
                //var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                //            join b in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                //            on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = b.RegistrationNo, ChePaiYanSe = b.RegistrationNoColor } into c
                //            from d in c.DefaultIfEmpty()
                //            select new
                //            {
                //                ShiFouZaiXian = (d.LatestGpsTime ?? DateTime.MinValue) >= compareTime
                //            };


                var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                            select new
                            {
                                ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0
                            };

                if (query.Count() > 0)
                {
                    var dto = new OrgItem()
                    {
                        OrgCode = userInfoDto.OrganizationCode,
                        OrgName = userInfoDto.OrganizationName,
                        OrgType = userInfoDto.OrganizationType.Value,
                        ParentOrgCode = string.Empty,
                        OrgZaiXianCheLiangShu = query.Count(l => l.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线),
                        OrgZongCheLiangShu = query.Count()
                    };
                    result.Data = new JianKongShuResult
                    {
                        Items = new List<OrgItem> { dto }
                    };
                }
            }
            else
            {
                //var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                //            join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                //            on a.Id equals Guid.Parse(b.CheLiangId)
                //            join c in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                //            on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = c.RegistrationNo, ChePaiYanSe = c.RegistrationNoColor } into temp1
                //            from e in temp1.DefaultIfEmpty()
                //            select new
                //            {
                //                ShiFouZaiXian = (e.LatestGpsTime ?? DateTime.MinValue) >= compareTime
                //            };

                var query = from a in _cheLiangXinXiRepository.GetQuery(cheLiangExp)
                            join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                            on a.Id.ToString() equals b.CheLiangId
                            //join c in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                            //on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = c.RegistrationNo, ChePaiYanSe = c.RegistrationNoColor } into temp1
                            //from e in temp1.DefaultIfEmpty()
                            select new
                            {
                                ShiFouZaiXian = a.ZaiXianZhuangTai ?? 0
                            };
                if (query.Count() > 0)
                {
                    var dto = new OrgItem()
                    {
                        OrgCode = userInfoDto.OrganizationCode,
                        OrgName = userInfoDto.OrganizationName,
                        OrgType = userInfoDto.OrganizationType.Value,
                        ParentOrgCode = string.Empty,
                        OrgZaiXianCheLiangShu = query.Count(l => l.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线),
                        OrgZongCheLiangShu = query.Count()
                    };
                    result.Data = new JianKongShuResult
                    {
                        Items = new List<OrgItem> { dto }
                    };
                }
            }

            return result;

        }
        /// <summary>
        /// 根据用户获取对应业户下的车辆
        /// </summary>
        /// <param name="userInfoDto"></param>
        /// <param name="orgInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> QueryUserCheLiangList(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo)
        {
            var result = new ServiceResult<JianKongShuResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
            var cheLiangQueryList = from a in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgCode == orgInfo.OrgCode)
                                    join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                                    on a.Id equals Guid.Parse(b.CheLiangId)
                                    //join c in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                    //on new { a.ChePaiHao, a.ChePaiYanSe } equals new { ChePaiHao = c.RegistrationNo, ChePaiYanSe = c.RegistrationNoColor }
                                    //into temp
                                    //from tempItem in temp.DefaultIfEmpty()
                                    select new CheLiangItem
                                    {
                                        ChePaiHao = a.ChePaiHao,
                                        ChePaiYanSe = a.ChePaiYanSe,
                                        CheLiangZhongLei = a.CheLiangZhongLei,
                                        ShiFouZaiXian = (a.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime
                                    };

            result.Data = new JianKongShuResult
            {
                NodeType = 1,
                Items = cheLiangQueryList.ToList()
            };

            return result;
        }

        #endregion 车辆监控树节点

        #region 根据组织获取车辆列表

        private ServiceResult<QueryResult> QueryZhengFuJianKongShuCheLiang(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

            var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuShi == userInfoDto.OrganCity)
                                        //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                        // into temp
                                        //from tempItem in temp.DefaultIfEmpty()
                                    select new
                                    {
                                        ChePaiHao = u.ChePaiHao,
                                        ChePaiYanSe = u.ChePaiYanSe,
                                        CheLiangZhongLei = u.CheLiangZhongLei,
                                        ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime,
                                        SYS_ZuiJinXiuGaiShiJian = u.SYS_ZuiJinXiuGaiShiJian
                                    };
            var queryResult = new QueryResult();
            queryResult.totalcount = cheLiangQueryList.Count();
            queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
            {
                ChePaiHao = s.ChePaiHao,
                ChePaiYanSe = s.ChePaiYanSe,
                CheLiangZhongLei = s.CheLiangZhongLei,
                ShiFouZaiXian = s.ShiFouZaiXian
            }).ToList();

            result.Data = queryResult;

            return result;
        }

        private ServiceResult<QueryResult> QueryXianZhengFuJianKongShuCheLiang(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

            var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuXian == userInfoDto.OrganDistrict)
                                        //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                        // into temp
                                        //from tempItem in temp.DefaultIfEmpty()
                                    select new
                                    {
                                        ChePaiHao = u.ChePaiHao,
                                        ChePaiYanSe = u.ChePaiYanSe,
                                        CheLiangZhongLei = u.CheLiangZhongLei,
                                        ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime,
                                        u.SYS_ZuiJinXiuGaiShiJian
                                    };
            var queryResult = new QueryResult();
            queryResult.totalcount = cheLiangQueryList.Count();
            queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
            {
                ChePaiHao = s.ChePaiHao,
                ChePaiYanSe = s.ChePaiYanSe,
                CheLiangZhongLei = s.CheLiangZhongLei,
                ShiFouZaiXian = s.ShiFouZaiXian
            }).ToList();

            result.Data = queryResult;

            return result;
        }

        private ServiceResult<QueryResult> QueryPingTaiYunYingShangJianKongShuCheLiang(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);

            var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                        // into temp
                                        //from tempItem in temp.DefaultIfEmpty()
                                    select new
                                    {
                                        ChePaiHao = u.ChePaiHao,
                                        ChePaiYanSe = u.ChePaiYanSe,
                                        CheLiangZhongLei = u.CheLiangZhongLei,
                                        ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime,
                                        SYS_ZuiJinXiuGaiShiJian = u.SYS_ZuiJinXiuGaiShiJian
                                    };
            var queryResult = new QueryResult();
            queryResult.totalcount = cheLiangQueryList.Count();
            queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
            {
                ChePaiHao = s.ChePaiHao,
                ChePaiYanSe = s.ChePaiYanSe,
                CheLiangZhongLei = s.CheLiangZhongLei,
                ShiFouZaiXian = s.ShiFouZaiXian
            }).ToList();

            result.Data = queryResult;

            return result;
        }

        private ServiceResult<QueryResult> QueryYeHuJianKongShuCheLiang(UserInfoDtoNew userInfoDto, OrgBaseInfo orgInfo, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            DateTime compareTime = DateTime.Now.AddMinutes(CheLiangDingWeiZaiXian);
            var queryResult = new QueryResult();

            if (IsGuanLiYuanRoleCode(userInfoDto.RoleCode))
            {
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgCode == orgInfo.OrgCode)
                                            //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                            //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                            // into temp
                                            //from tempItem in temp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            else
            {
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.YeHuOrgCode == orgInfo.OrgCode)
                                        join v in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.SysUserId == userInfoDto.Id)
                                        on u.Id equals Guid.Parse(v.CheLiangId)
                                        //join w in _cheLiangDingWeiXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        //on new { u.ChePaiHao, u.ChePaiYanSe } equals new { ChePaiHao = w.RegistrationNo, ChePaiYanSe = w.RegistrationNoColor }
                                        // into temp
                                        //from tempItem in temp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,//(tempItem.LatestGpsTime ?? DateTime.MinValue) > compareTime,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }



            result.Data = queryResult;

            return result;
        }

        #endregion 根据组织获取车辆列表

        private int[] CheLiangZhongLeis { get; set; }

        /// <summary>
        /// 查询车辆视频监控树内容（省市区、企业、车辆种类或车辆列表）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<JianKongShuResult> QueryVideoJianKongShuV2(VideoJianKongShuQueryDto dto)
        {
            return ExecuteCommandStruct<JianKongShuResult>(() =>
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                var result = new ServiceResult<JianKongShuResult>();
                CheLiangZhongLeis = dto.CheLiangZhongLeis;

                switch (userInfoNew.OrganizationType)
                {
                    //临时处理，服务商查询
                    case (int)OrganizationType.本地服务商:
                    {
                        switch (dto.NodeType)
                        {
                            case 1:
                                result = V_FuWuShangYeHuAndCheLiangShu(userInfoNew);
                                break;
                            case 2:
                                result = V_FuWuShangCheLiangZhongLeiAndCheLiangShu(dto, userInfoNew);
                                break;
                            case 3:
                                result = V_FuWuShangCheLiangV2(dto, userInfoNew);
                                break;
                            default:
                                result = V_FuWuShangAndCheLiangShu(userInfoNew);
                                break;
                        }

                        return result;
                    }
                    case (int)OrganizationType.企业:
                        switch (dto.NodeType)
                        {
                            case 2:
                                result = V_EnterpriseCheLiangZhongLeiAndCheLiangShu(dto, userInfoNew);
                                break;
                            case 3:
                                result = V_EnterpriseCheLiangV2(dto, userInfoNew);
                                break;
                            default:
                                result = V_EnterpriseAndCheLiangShu(userInfoNew);
                                break;
                        }
                        return result;
                }


                if (dto == null || string.IsNullOrWhiteSpace(dto.OrgCode))
                {
                    //查询省级节点
                    result = V_FirstNodeCheLiangShu(1, new string[] { "清远" });
                }
                else if (dto.OrgCode == "广东")
                {
                    //查询市级节点
                    result = V_FirstNodeCheLiangShu(2);
                }
                else if (dto.OrgCode == "清远" && dto.NodeType == 0)
                {
                    //查询县级节点
                    result = V_XiaQuXianAndCheLiangShu();
                }
                else if (dto.NodeType == 1)
                {
                    //查询企业节点
                    result = V_YeHuAndCheLiangShu(dto.OrgCode, userInfoNew);
                }
                else if (dto.NodeType == 2)
                {
                    //查询车辆种类节点
                    result = V_CheLiangZhongLeiAndCheLiangShu(dto, userInfoNew);
                }
                else
                {
                    //查询车辆节点
                    result = V_YeHuCheLiangV2(dto, userInfoNew);
                }

                return result;
            });
        }

        /// <summary>
        /// 查询监控树车辆v2
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryJianKongShuCheLiangV2(QueryData queryData)
        {
            return ExecuteCommandStruct<QueryResult>(() =>
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                var result = new ServiceResult<QueryResult>();

                result = QueryYeHuJianKongShuCheLiangV2(queryData, userInfoNew, queryData.page, queryData.rows);

                return result;
            });
        }


        private ServiceResult<QueryResult> QueryYeHuJianKongShuCheLiangV2(QueryData queryData, UserInfoDtoNew userInfoDto, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            var queryResult = new QueryResult();
            string orgCode = queryData.data.OrgCode;
            int nodeType = queryData.data.NodeType;
            string xiaQuXian = queryData.data.XiaQuXian;
            JArray shiList = queryData.data.XiaQuShiList;
            string[] xiaQuShiList = null;
            if (shiList != null)
            {
                xiaQuShiList = shiList.ToObject<string[]>();
            }

            Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常;

            if (orgCode == "广东")
            {
                Expression<Func<CheLiang, bool>> clExp = p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuSheng == "广东" && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString());
                if (xiaQuShiList != null && xiaQuShiList.Length > 0)
                {
                    clExp = clExp.And(c => xiaQuShiList.Contains(c.XiaQuShi));
                }
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                                        join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                                        from tt in tmp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            else if (nodeType != 1 && orgCode == "清远")
            {
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()))
                                        join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                                        from tt in tmp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            else if (queryData.data.NodeType == 1)
            {

                if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.FuWuShangOrgCode == userInfoDto.OrganizationCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));

                }
                else
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.XiaQuXian == orgCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));
                }

                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(carExp)
                                        join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                                        from tt in tmp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };



                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            else if (queryData.data.NodeType == 2)
            {

                if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.FuWuShangOrgCode == userInfoDto.OrganizationCode && p.YeHuOrgCode == orgCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));

                }
                else
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.XiaQuXian == (!string.IsNullOrEmpty(xiaQuXian) ? xiaQuXian : p.XiaQuXian) && p.YeHuOrgCode == orgCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));
                }

                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(carExp)
                                        join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                                        from tt in tmp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            else if (queryData.data.NodeType == 3)
            {
                string ParentOrgCode = queryData.data.ParentOrgCode;
                int? cheLiangZhongLei = Convert.ToInt32(orgCode);
                if (userInfoDto.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.FuWuShangOrgCode == userInfoDto.OrganizationCode && p.CheLiangZhongLei == cheLiangZhongLei && p.YeHuOrgCode == ParentOrgCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));

                }
                else
                {
                    carExp = carExp.And(p => p.XiaQuSheng == "广东" && p.XiaQuShi == "清远" && p.XiaQuXian == xiaQuXian && p.CheLiangZhongLei == cheLiangZhongLei && p.YeHuOrgCode == ParentOrgCode && p.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(p.CheLiangZhongLei.ToString()));
                }
                var cheLiangQueryList = from u in _cheLiangXinXiRepository.GetQuery(carExp)
                                        join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常)
                                        on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                                        from tt in tmp.DefaultIfEmpty()
                                        select new
                                        {
                                            ChePaiHao = u.ChePaiHao,
                                            ChePaiYanSe = u.ChePaiYanSe,
                                            CheLiangZhongLei = u.CheLiangZhongLei,
                                            ShiFouZaiXian = (u.ZaiXianZhuangTai ?? 0) == (int)ZaiXianZhuangTai.在线,
                                            u.SYS_ZuiJinXiuGaiShiJian
                                        };

                queryResult.totalcount = cheLiangQueryList.Count();
                queryResult.items = cheLiangQueryList.OrderByDescending(o => o.SYS_ZuiJinXiuGaiShiJian).Skip((page - 1) * rows).Take(rows).Select(s => new CheLiangItem
                {
                    ChePaiHao = s.ChePaiHao,
                    ChePaiYanSe = s.ChePaiYanSe,
                    CheLiangZhongLei = s.CheLiangZhongLei,
                    ShiFouZaiXian = s.ShiFouZaiXian
                }).ToList();
            }
            result.Data = queryResult;

            return result;
        }

        /// <summary>
        /// （终端视频）查询父节点
        /// </summary>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_FirstNodeCheLiangShu(int queryType, string[] xiaQuShiList = null)
        {
            var dto = new VideoOrgItem();

            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.YunZhengZhuangTai == "营运";
            if (xiaQuShiList != null && xiaQuShiList.Length > 0)
            {
                clExp = clExp.And(c => xiaQuShiList.Contains(c.XiaQuShi));
            }
            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }


            var result = new ServiceResult<JianKongShuResult>()
            {
                StatusCode = 0,
                Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = new List<VideoOrgItem> { dto }
                }
            };

            if (queryType == 1)
            {
                clExp = clExp.And(c => c.XiaQuSheng == "广东");
            }
            if (queryType == 2)
            {
                clExp = clExp.And(c => c.XiaQuShi == "清远");
            }



            var queryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            select new
                            {
                                u.XiaQuSheng,
                                u.XiaQuShi,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            if (queryType == 1)
            {
                var yeHuOrgQueryList = from u in queryList
                                       group u by
                                       new
                                       {
                                           u.XiaQuSheng
                                       } into k
                                       select new
                                       {
                                           OrgCode = k.Key.XiaQuSheng,
                                           OrgName = k.Key.XiaQuSheng,
                                           OrgType = 1,
                                           OrgZongCheLiangShu = k.Count(),
                                           OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                       };

                result.Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.ToList()
                };
            }
            else
            {
                var yeHuOrgQueryList = from u in queryList
                                       group u by
                                       new
                                       {
                                           u.XiaQuShi
                                       } into k
                                       select new
                                       {
                                           OrgCode = k.Key.XiaQuShi,
                                           OrgName = k.Key.XiaQuShi,
                                           OrgType = (int)OrganizationType.市政府,
                                           OrgZongCheLiangShu = k.Count(),
                                           OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                       };

                result.Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.ToList()
                };
            }



            return result;
        }

        /// <summary>
        /// （终端视频）查询县级节点
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_XiaQuXianAndCheLiangShu()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
            }

            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运";

            if (userInfo.OrganizationType == (int)OrganizationType.县政府)
            {
                clExp = clExp.And(c => c.XiaQuXian == userInfo.OrganDistrict);
            }

            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            select new
                            {
                                u.XiaQuXian,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.XiaQuXian
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.XiaQuXian,
                                       OrgName = k.Key.XiaQuXian,
                                       OrgType = (int)OrganizationType.县政府,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };



            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 1,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = userInfo.OrganCity,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu
                    }).Distinct().OrderBy(x => x.OrgName).ToList()
                }
            };
        }

        /// <summary>
        /// （终端视频）查询企业节点
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_YeHuAndCheLiangShu(string XiaQuXian, UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang;
            //clExp = clExp.And(c => c.XiaQuXian == XiaQuXian);


            Expression<Func<CheLiang, bool>> cl2Exp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运";


            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                cl2Exp = cl2Exp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            //企业只能查询自己的数据
            if (userInfo.OrganizationType == (int)OrganizationType.企业)
            {
                clExp = clExp.And(c => c.OrgCode == userInfo.OrganizationCode);
                cl2Exp = cl2Exp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
                cl2Exp = cl2Exp.And(c => c.XiaQuXian == XiaQuXian);
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            join b in _orgBaseInfoRepository.GetQuery(clExp)
                            on u.YeHuOrgCode.ToString() equals b.OrgCode
                            select new
                            {
                                b.OrgName,
                                b.OrgCode,
                                b.OrgType,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 2,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = XiaQuXian,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = XiaQuXian
                    }).Distinct().ToList()
                }
            };
        }


        /// <summary>
        /// 查询服务商一级监控树
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_FuWuShangAndCheLiangShu(UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiang, bool>> cl2Exp = c =>
                c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" &&
                c.YunZhengZhuangTai == "营运";


            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                cl2Exp = cl2Exp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            if (userInfo.OrganizationType == (int)OrganizationType.本地服务商)
            {
                clExp = clExp.And(c => c.OrgCode == userInfo.OrganizationCode);
                cl2Exp = cl2Exp.And(c => c.FuWuShangOrgCode == userInfo.OrganizationCode);
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            join b in _orgBaseInfoRepository.GetQuery(clExp)
                            on u.FuWuShangOrgCode.ToString() equals b.OrgCode
                            select new
                            {
                                b.OrgName,
                                b.OrgCode,
                                b.OrgType,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 1,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = "",
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = userInfo.OrganDistrict
                    }).Distinct().ToList()
                }
            };
        }
        /// <summary>
        /// 查询服务商二级监控树(企业列表)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_FuWuShangYeHuAndCheLiangShu(UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiang, bool>> cl2Exp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运";
            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                cl2Exp = cl2Exp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }
            //企业只能查询自己的数据
            if (userInfo.OrganizationType == (int)OrganizationType.本地服务商)
            {
                clExp = clExp.And(c => c.OrgCode == userInfo.OrganizationCode);
                cl2Exp = cl2Exp.And(c => c.FuWuShangOrgCode == userInfo.OrganizationCode);
            }
            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString()
                            join b in _orgBaseInfoRepository.GetQuery(clExp)
                            on u.FuWuShangOrgCode.ToString() equals b.OrgCode
                            select new
                            {
                                a.OrgName,
                                a.OrgCode,
                                a.OrgType,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };
            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 2,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = userInfo.OrganizationName,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = userInfo.OrganDistrict
                    }).Distinct().ToList()
                }
            };
        }
        /// <summary>
        /// 查询服务商三级监控树(车辆种类列表)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_FuWuShangCheLiangZhongLeiAndCheLiangShu(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfo)
        {
            List<VideoOrgItem> v = new List<VideoOrgItem>();

            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运";

            clExp = clExp.And(c => c.YeHuOrgCode == dto.OrgCode && c.FuWuShangOrgCode == userInfo.OrganizationCode);

            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            select new
                            {
                                u.CheLiangZhongLei,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            foreach (int myCode in Enum.GetValues(typeof(CheLiangZhongLeiEnum)))
            {
                string strName = Enum.GetName(typeof(CheLiangZhongLeiEnum), myCode);//获取名称
                string strVaule = myCode.ToString();//获取值
                if ("1,2,3,4,5,6,7,8,10,11,9".Contains(strVaule))
                {
                    v.Add(new VideoOrgItem()
                    {
                        OrgCode = strVaule,
                        OrgName = strName,
                        OrgType = userInfo.OrganizationType,
                        ParentOrgCode = dto.OrgCode
                    });
                }
            }


            var yeHuOrgQueryList = from j in v
                                   join u in queryList
                                   on j.OrgCode equals u.CheLiangZhongLei.Value.ToString()
                                   group u by
                                   new
                                   {
                                       j.OrgCode,
                                       j.OrgName
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = userInfo.OrganizationType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };

            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 3,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = dto.OrgCode,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = dto.XiaQuXian
                    }).ToList()
                }
            };
        }
        /// <summary>
        /// 查询服务商四级监控树(车辆列表)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfoNew"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_FuWuShangCheLiangV2(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfoNew)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = p =>
                p.SYS_XiTongZhuangTai == sysZhengChang && p.YeHuOrgCode == dto.YeHuOrgCode &&
                p.CheLiangZhongLei == dto.CheLiangZhongLei
                && p.YunZhengZhuangTai == "营运";

            clExp = clExp.And(c => c.FuWuShangOrgCode == userInfoNew.OrganizationCode);


            var cheLiangList = (from u in _cheLiangXinXiRepository.GetQuery(clExp)
                                join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.Id.ToString() equals a.CheLiangId into tmp
                                from tt in tmp.DefaultIfEmpty()
                                join b in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.YeHuOrgCode.ToString() equals b.OrgCode.ToString() into tmp2
                                from tt2 in tmp2.DefaultIfEmpty()
                                select new
                                {
                                    u.ChePaiHao,
                                    u.ChePaiYanSe,
                                    u.CheLiangZhongLei,
                                    ShiFouZaiXian = u.ZaiXianZhuangTai.HasValue ? u.ZaiXianZhuangTai.Value == (int)ZaiXianZhuangTai.在线 : false,
                                    //dw.LatestGpsTime,
                                    ShiPinTouGeShu = tt == null ? 0 : tt.ShiPinTouGeShu,
                                    tt.ShiPinTouAnZhuangXuanZe,
                                    ShiPingChangShangLeiXing = tt == null ? 0 : tt.ShiPingChangShangLeiXing
                                }).ToList();


            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 4,
                    Items = cheLiangList.Select(x => new VideoCheLiangItem()
                    {
                        ChePaiHao = x.ChePaiHao,
                        ChePaiYanSe = x.ChePaiYanSe,
                        CheLiangZhongLei = x.CheLiangZhongLei,
                        ShiFouZaiXian = x.ShiFouZaiXian,//CheLiangShiFouZaiXian(x.LatestGpsTime),
                        ShiPinTouGeShu = x.ShiPinTouGeShu,
                        CameraSelected = x.ShiPinTouAnZhuangXuanZe,
                        VideoServiceKind = x.ShiPingChangShangLeiXing
                    }).Distinct().ToList()
                }
            };
        }



        /// <summary>
        /// （终端视频）查询车辆种类
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_CheLiangZhongLeiAndCheLiangShu(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfo)
        {
            List<VideoOrgItem> v = new List<VideoOrgItem>();

            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c =>
                c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" &&
                c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运";

            clExp = clExp.And(c => c.YeHuOrgCode == dto.OrgCode && c.XiaQuXian == dto.XiaQuXian);

            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode.ToString() into tmp
                            from tt in tmp.DefaultIfEmpty()
                            select new
                            {
                                u.CheLiangZhongLei,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            foreach (int myCode in Enum.GetValues(typeof(CheLiangZhongLeiEnum)))
            {
                string strName = Enum.GetName(typeof(CheLiangZhongLeiEnum), myCode);//获取名称
                string strVaule = myCode.ToString();//获取值
                if ("1,2,3,4,5,6,7,8,10,11,9".Contains(strVaule))
                {
                    v.Add(new VideoOrgItem()
                    {
                        OrgCode = strVaule,
                        OrgName = strName,
                        OrgType = userInfo.OrganizationType,
                        ParentOrgCode = dto.OrgCode
                    });
                }
            }


            var yeHuOrgQueryList = from j in v
                                   join u in queryList
                                   on j.OrgCode equals u.CheLiangZhongLei.Value.ToString()
                                   group u by
                                   new
                                   {
                                       j.OrgCode,
                                       j.OrgName
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = userInfo.OrganizationType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };

            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 3,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = dto.OrgCode,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = dto.XiaQuXian
                    }).ToList()
                }
            };
        }

        /// <summary>
        /// （终端视频）查询业户（个体户/企业）下级车辆
        /// </summary>
        /// <param name="org"></param>
        /// <param name="userInfoNew"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_YeHuCheLiangV2(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfoNew)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = p =>
                p.SYS_XiTongZhuangTai == sysZhengChang && p.XiaQuXian == dto.XiaQuXian &&
                p.YeHuOrgCode == dto.YeHuOrgCode && p.CheLiangZhongLei == dto.CheLiangZhongLei &&
                p.YunZhengZhuangTai == "营运";
            //clExp = clExp.And(c => c.YeHuOrgCode == dto.OrgCode && c.XiaQuXian == dto.XiaQuXian);
            clExp = clExp.And(c => c.XiaQuXian == dto.XiaQuXian);

            //if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            //{
            //    clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            //}
            LogHelper.Info($"车辆信息查询：{JsonConvert.SerializeObject(_cheLiangXinXiRepository.GetQuery(clExp).ToList())}");

            var cheLiangList = (from u in _cheLiangXinXiRepository.GetQuery(clExp)
                                join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.Id.ToString() equals a.CheLiangId into tmp
                                from tt in tmp.DefaultIfEmpty()
                                join b in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.YeHuOrgCode.ToString() equals b.OrgCode.ToString() into tmp2
                                from tt2 in tmp2.DefaultIfEmpty()
                                select new
                                {
                                    u.ChePaiHao,
                                    u.ChePaiYanSe,
                                    u.CheLiangZhongLei,
                                    ShiFouZaiXian = u.ZaiXianZhuangTai.HasValue ? u.ZaiXianZhuangTai.Value == (int)ZaiXianZhuangTai.在线 : false,
                                    //dw.LatestGpsTime,
                                    ShiPinTouGeShu = tt == null ? 0 : tt.ShiPinTouGeShu,
                                    tt.ShiPinTouAnZhuangXuanZe,
                                    ShiPingChangShangLeiXing = tt == null ? 0 : tt.ShiPingChangShangLeiXing
                                }).ToList();


            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 4,
                    Items = cheLiangList.Select(x => new VideoCheLiangItem()
                    {
                        ChePaiHao = x.ChePaiHao,
                        ChePaiYanSe = x.ChePaiYanSe,
                        CheLiangZhongLei = x.CheLiangZhongLei,
                        ShiFouZaiXian = x.ShiFouZaiXian,//CheLiangShiFouZaiXian(x.LatestGpsTime),
                        ShiPinTouGeShu = x.ShiPinTouGeShu,
                        CameraSelected = x.ShiPinTouAnZhuangXuanZe,
                        VideoServiceKind = x.ShiPingChangShangLeiXing
                    }).Distinct().ToList()
                }
            };
        }


        /// <summary>
        /// 查询企业车辆视频监控树内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<JianKongShuResult> QueryQiYeVideoJianKongShu(VideoJianKongShuQueryDto dto)
        {
            return ExecuteCommandStruct<JianKongShuResult>(() =>
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                var result = new ServiceResult<JianKongShuResult>();

                if (!string.IsNullOrEmpty(dto.YeHuMingCheng))
                {
                    if (string.IsNullOrEmpty(dto.YeHuMingCheng))
                    {
                        return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = "业户名称不能为空" };
                    }
                    //查询企业节点
                    result = V_YeHuAndSearchShu(dto.YeHuMingCheng, userInfoNew);
                }
                else
                {
                    //查询组织信息
                    OrgBaseInfo orgInfo;
                    if (dto.OrgCode == AdminOrgCode)
                    {
                        //平台运营商没有组织数据保存，故直接实例化平台运营商的信息
                        orgInfo = new OrgBaseInfo { OrgCode = AdminOrgCode, OrgType = (int)OrganizationType.平台运营商 };
                    }
                    else
                    {
                        //除平台运营商外，皆可在数据库查到组织记录
                        var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                        orgInfo = _orgBaseInfoRepository.GetQuery(o => o.SYS_XiTongZhuangTai == sysZhengChang && o.OrgCode == dto.OrgCode.Trim()).FirstOrDefault();
                    }
                    if (orgInfo == null || string.IsNullOrWhiteSpace(orgInfo.OrgCode))
                    {
                        return new ServiceResult<JianKongShuResult> { StatusCode = 2, ErrorMessage = $"未找到该组织信息（{ dto.OrgCode}）" };
                    }
                    //查询车辆节点
                    result = V_YeHuCheLiang(orgInfo, userInfoNew);
                }
                return result;
            });
        }

        /// <summary>
        /// （终端视频）模糊查询企业节点
        /// </summary>
        /// <param name="OrgBaseInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_YeHuAndSearchShu(string YeHuMingCheng, UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.OrgName.Contains(YeHuMingCheng);

            Expression<Func<CheLiang, bool>> cl2Exp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" && c.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(c.CheLiangZhongLei.ToString());
            //cl2Exp = cl2Exp.And(c => c.XiaQuXian == XiaQuXian);

            //企业只能查询自己的数据
            if (userInfo.OrganizationType == (int)OrganizationType.企业)
            {
                clExp = clExp.And(c => c.OrgCode == userInfo.OrganizationCode);
                cl2Exp = cl2Exp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _orgBaseInfoRepository.GetQuery(clExp)
                            on u.YeHuOrgCode.ToString() equals a.OrgCode
                            select new
                            {
                                a.OrgName,
                                a.OrgCode,
                                a.OrgType,
                                a.XiaQuXian,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };


            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType,
                                       u.XiaQuXian
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       XiaQuXian = k.Key.XiaQuXian,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };

            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 0,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        XiaQuXian = x.XiaQuXian,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu
                    }).Distinct().ToList()
                }
            };
        }



        public ServiceResult<VehicleOnNetInfoDto> GetVehicelNumber()
        {
            try
            {

                Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == 0;
                Expression<Func<OrgBaseInfo, bool>> orgExp = x => x.SYS_XiTongZhuangTai == 0;

                var list = from car in _cheLiangXinXiRepository.GetQuery(carExp)
                           join org in _orgBaseInfoRepository.GetQuery(orgExp) on car.YeHuOrgCode equals org.OrgCode
                           select new VehicleAreaBaseInfo
                           {
                               ChePaiHao = car.ChePaiHao,
                               ChePaiYanSe = car.ChePaiYanSe,
                               XiaQuShi = car.XiaQuShi,
                               XiaQuXian = car.XiaQuXian,
                               ZaiXianZhuangTai = car.ZaiXianZhuangTai,
                               YunZhengZhuangTai = car.YunZhengZhuangTai
                           };
                VehicleOnNetInfoDto resultDto = new VehicleOnNetInfoDto();
                //各个辖区营运车辆数
                resultDto.YunZhengCheLiangStatistics = new List<VehicleStatisticsDto>(
                     list.Where(x => x.YunZhengZhuangTai == "营运").GroupBy(x => x.XiaQuXian).Select(x => new VehicleStatisticsDto
                     {
                         XiaQuXian = x.Key,
                         VehicelCount = x.Count(),
                         OnNetVehicleCount = x.Where(y => y.ZaiXianZhuangTai == 1).Count()
                     }).ToList()
                    );
                //各个辖区在线车辆数
                //resultDto.ZaiXianCheLiangStatistics = new List<VehicleStatisticsDto>(
                //    list.Where(x => x.YunZhengZhuangTai == "营运" && x.ZaiXianZhuangTai == 1).GroupBy(x => x.XiaQuXian).Select(x => new VehicleStatisticsDto
                //    {
                //        XiaQuXian = x.Key,
                //        VehicelCount = x.Count(),
                //    }).ToList()
                //   );
                return new ServiceResult<VehicleOnNetInfoDto> { Data = resultDto };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询辖区车辆在线数量出错" + ex.Message, ex);
                return new ServiceResult<VehicleOnNetInfoDto>();
            }
        }

        /// <summary>
        /// 查询企业监控树(企业列表)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_EnterpriseCheLiangShu(UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> cl2Exp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远" 
                                                           && c.YunZhengZhuangTai == "营运" && "1,2,3,4".Contains(c.CheLiangZhongLei.ToString());
            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                cl2Exp = cl2Exp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            //企业只能查询自己的数据
            if (userInfo.OrganizationType == (int)OrganizationType.企业)
            {
                cl2Exp = cl2Exp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode equals a.OrgCode
                            select new
                            {
                                a.OrgName,
                                a.OrgCode,
                                a.OrgType,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 2,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = userInfo.OrganizationName,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = userInfo.OrganDistrict
                    }).Distinct().ToList()
                }
            };
        }
        /// <summary>
        /// 查询企业二级监控树(车辆种类列表)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_EnterpriseCheLiangZhongLeiAndCheLiangShu(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfo)
        {
            var v = new List<VideoOrgItem>();

            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang && c.XiaQuSheng == "广东" && c.XiaQuShi == "清远"
                                                          && c.YunZhengZhuangTai == "营运";
            clExp = clExp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                clExp = clExp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }
            var queryList = from u in _cheLiangXinXiRepository.GetQuery(clExp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode equals a.OrgCode into tmp
                            from tt in tmp.DefaultIfEmpty()
                            select new
                            {
                                u.CheLiangZhongLei,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            foreach (int myCode in Enum.GetValues(typeof(CheLiangZhongLeiEnum)))
            {
                var strName = Enum.GetName(typeof(CheLiangZhongLeiEnum), myCode);//获取名称
                var strVaule = myCode.ToString();//获取值
                if ("1,2,3,4,5,6,7,8,10,11,9".Contains(strVaule))
                {
                    v.Add(new VideoOrgItem()
                    {
                        OrgCode = strVaule,
                        OrgName = strName,
                        OrgType = userInfo.OrganizationType,
                        ParentOrgCode = dto.OrgCode
                    });
                }
            }


            var yeHuOrgQueryList = from j in v
                                   join u in queryList
                                   on j.OrgCode equals u.CheLiangZhongLei.Value.ToString()
                                   group u by
                                   new
                                   {
                                       j.OrgCode,
                                       j.OrgName
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = userInfo.OrganizationType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };

            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 3,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = dto.OrgCode,
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = dto.XiaQuXian
                    }).ToList()
                }
            };
        }

        /// <summary>
        /// 查询企业三级监控树(车辆列表)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfoNew"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_EnterpriseCheLiangV2(VideoJianKongShuQueryDto dto, UserInfoDtoNew userInfoNew)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<CheLiang, bool>> clExp = p => p.SYS_XiTongZhuangTai == sysZhengChang &&
                                                          p.YeHuOrgCode == userInfoNew.OrganizationCode && p.CheLiangZhongLei == dto.CheLiangZhongLei && 
                                                          p.YunZhengZhuangTai == "营运";
            var cheLiangList = (from u in _cheLiangXinXiRepository.GetQuery(clExp)
                                join a in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.Id.ToString() equals a.CheLiangId into tmp
                                from tt in tmp.DefaultIfEmpty()
                                join b in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                                on u.YeHuOrgCode equals b.OrgCode into tmp2
                                from tt2 in tmp2.DefaultIfEmpty()
                                select new
                                {
                                    u.ChePaiHao,
                                    u.ChePaiYanSe,
                                    u.CheLiangZhongLei,
                                    ShiFouZaiXian = u.ZaiXianZhuangTai.HasValue && u.ZaiXianZhuangTai.Value == (int)ZaiXianZhuangTai.在线,
                                    ShiPinTouGeShu = tt == null ? 0 : tt.ShiPinTouGeShu,
                                    tt.ShiPinTouAnZhuangXuanZe,
                                    ShiPingChangShangLeiXing = tt == null ? 0 : tt.ShiPingChangShangLeiXing
                                }).ToList();


            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 4,
                    Items = cheLiangList.Select(x => new VideoCheLiangItem()
                    {
                        ChePaiHao = x.ChePaiHao,
                        ChePaiYanSe = x.ChePaiYanSe,
                        CheLiangZhongLei = x.CheLiangZhongLei,
                        ShiFouZaiXian = x.ShiFouZaiXian,//CheLiangShiFouZaiXian(x.LatestGpsTime),
                        ShiPinTouGeShu = x.ShiPinTouGeShu,
                        CameraSelected = x.ShiPinTouAnZhuangXuanZe,
                        VideoServiceKind = x.ShiPingChangShangLeiXing
                    }).Distinct().ToList()
                }
            };
        }

        /// <summary>
        /// 查询企业一级监控树
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private ServiceResult<JianKongShuResult> V_EnterpriseAndCheLiangShu(UserInfoDtoNew userInfo)
        {
            var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
            Expression<Func<OrgBaseInfo, bool>> clExp = c => c.SYS_XiTongZhuangTai == sysZhengChang;
            Expression<Func<CheLiang, bool>> cl2Exp = c => c.SYS_XiTongZhuangTai == sysZhengChang
                                                           && c.XiaQuSheng == "广东" 
                                                           && c.XiaQuShi == "清远" 
                                                           && c.YunZhengZhuangTai == "营运";
            if (CheLiangZhongLeis != null && CheLiangZhongLeis.Length > 0)
            {
                cl2Exp = cl2Exp.And(c => CheLiangZhongLeis.Contains(c.CheLiangZhongLei.Value));
            }

            if (userInfo.OrganizationType == (int)OrganizationType.企业)
            {
                cl2Exp = cl2Exp.And(c => c.YeHuOrgCode == userInfo.OrganizationCode);
            }

            var queryList = from u in _cheLiangXinXiRepository.GetQuery(cl2Exp)
                            join a in _cheLiangYeHuRepository.GetQuery(a => a.SYS_XiTongZhuangTai == sysZhengChang)
                            on u.YeHuOrgCode equals a.OrgCode into tmp
                            from tt in tmp.DefaultIfEmpty()
                            join b in _orgBaseInfoRepository.GetQuery(clExp)
                            on u.YeHuOrgCode equals b.OrgCode
                            select new
                            {
                                b.OrgName,
                                b.OrgCode,
                                b.OrgType,
                                ShiFouZaiXian = u.ZaiXianZhuangTai ?? 0
                            };

            var yeHuOrgQueryList = from u in queryList
                                   group u by
                                   new
                                   {
                                       u.OrgName,
                                       u.OrgCode,
                                       u.OrgType
                                   } into k
                                   select new
                                   {
                                       OrgCode = k.Key.OrgCode,
                                       OrgName = k.Key.OrgName,
                                       OrgType = k.Key.OrgType,
                                       OrgZongCheLiangShu = k.Count(),
                                       OrgZaiXianCheLiangShu = k.Count(s => s.ShiFouZaiXian == (int)ZaiXianZhuangTai.在线)
                                   };
            return new ServiceResult<JianKongShuResult>()
            {
                Data = new JianKongShuResult()
                {
                    NodeType = 2,
                    Items = yeHuOrgQueryList.Select(x => new VideoOrgItem()
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        OrgType = x.OrgType,
                        ParentOrgCode = "",
                        OrgZongCheLiangShu = x.OrgZongCheLiangShu,
                        OrgZaiXianCheLiangShu = x.OrgZaiXianCheLiangShu,
                        XiaQuXian = userInfo.OrganDistrict
                    }).Distinct().ToList()
                }
            };
        }
    }
}
