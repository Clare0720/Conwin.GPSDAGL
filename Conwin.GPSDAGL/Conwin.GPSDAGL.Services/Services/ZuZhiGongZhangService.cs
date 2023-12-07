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
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class ZuZhiGongZhangService : ApiServiceBase, IZuZhiGongZhangXinXiService
    {
        IZuZhiGongZhangXinXiRepository _zuZhiGongZhangRepository;
        IOrgBaseInfoRepository _orgBaseInfoRepository;
        public ZuZhiGongZhangService(
            IBussinessLogger _bussinessLogger,
            IZuZhiGongZhangXinXiRepository zuZhiGongZhangRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository
            ) :base(_bussinessLogger)
        {
            _zuZhiGongZhangRepository = zuZhiGongZhangRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }


        public ServiceResult<bool> Create(string reqid, ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo)
        {
            var result = new ServiceResult<bool>() { Data = false };
            //TODO 规则检验
            //TODO 业务逻辑
            var isEmpty = false;
            if (userInfo == null)
            {
                return new ServiceResult<bool>() { Data = false };
            }
            if (!string.IsNullOrWhiteSpace(dto.OrgCode))
            {
                isEmpty = _zuZhiGongZhangRepository.GetQuery(m => m.OrgCode == dto.OrgCode && m.SYS_XiTongZhuangTai == 0).Count() == 0;
                result.StatusCode = 2;
                result.ErrorMessage = result.Data ? "" : "记录已存在";
            }
            if (isEmpty)
            {
                var entity = Mapper.Map<ZuZhiGongZhangXinXi>(dto);
                entity.Id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid() : entity.Id;
                entity.SYS_XiTongZhuangTai = 0;
                entity.SYS_ChuangJianRen = userInfo.UserName;
                entity.SYS_ChuangJianRenID = userInfo.Id;
                entity.SYS_ChuangJianShiJian = DateTime.Now;
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = entity.SYS_ChuangJianShiJian;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _zuZhiGongZhangRepository.Add(entity);
                    result.Data = uow.CommitTransaction() > 0;
                }

                result.StatusCode = result.Data ? 0 : 2;
                result.ErrorMessage = result.Data ? "" : "新增失败";
            }

            return result;
        }

        public ServiceResult<bool> Update(string reqid, ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo)
        {
            var result = new ServiceResult<bool>() { Data = true };
            //TODO 规则检验
            //TODO 业务逻辑
            if (result.Data)
            {
                var entity = Mapper.Map<ZuZhiGongZhangXinXi>(dto);
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _zuZhiGongZhangRepository.Update(
                        m => m.Id == entity.Id,
                        n => new ZuZhiGongZhangXinXi()
                        {
                            //根据配置文件生成部分
                            ChuangJianDanWeiOrgCode = entity.ChuangJianDanWeiOrgCode,
                            OrgName = entity.OrgName,
                            OrgCode = entity.OrgCode,
                            GongZhangZhaoPianId = entity.GongZhangZhaoPianId,
                            //固定修改部分
                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                            SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now
                        });
                    result.Data = uow.CommitTransaction() >= 0;                  
                }
                result.StatusCode = result.Data ? 0 : 2;
                result.ErrorMessage = result.Data ? "" : "更新失败";
            }
            return result;
        }

        public ServiceResult<bool> Delete(string reqid, Guid[] ids, UserInfoDto userInfo)
        {
            var result = new ServiceResult<bool>() { Data = true };
            //TODO 规则检验
            //TODO 业务逻辑
            if (result.Data)
            {
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _zuZhiGongZhangRepository.Update(
                        m => ids.Contains(m.Id),
                        n => new ZuZhiGongZhangXinXi()
                        {
                            SYS_XiTongZhuangTai = 1,
                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                            SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now
                        });
                    result.Data = uow.CommitTransaction() >= 0;
                    var logList = _zuZhiGongZhangRepository.GetQuery(x => ids.Contains(x.Id)).ToList();         
                }
                result.StatusCode = result.Data ? 0 : 2;
                result.ErrorMessage = result.Data ? "" : "删除失败";
            }
            return result;
        }
        public ServiceResult<ZuZhiGongZhangXinXiDto> Get(Guid id)
        {
            var result = new ServiceResult<ZuZhiGongZhangXinXiDto>();
            var query = _zuZhiGongZhangRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0 && m.Id == id);
            result.Data = Mapper.Map<ZuZhiGongZhangXinXiDto>(query.FirstOrDefault());
            return result;
        }
        public ServiceResult<QueryResult> Query(ZuZhiGongZhangXinXiDto dto, UserInfoDto userInfo, int page, int rows)
        {
            var result = new ServiceResult<QueryResult>();
            result.Data = new QueryResult();
            UserInfoDtoNew userInfoNew = GetUserInfo();
            Expression<Func<ZuZhiGongZhangXinXi, bool>> gongzhangExp = x => x.SYS_XiTongZhuangTai == 0;
            //不是管理员只能查询自己及下级分公司的公章信息
            if (userInfoNew.OrganizationType != (int)OrganizationType.平台运营商)
            {
                var userOrgId = Guid.Parse(userInfo.ExtOrganizationId);
                var codeList = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.ParentOrgId == userOrgId).Select(x => x.OrgCode).ToList();
                gongzhangExp = gongzhangExp.And(x => x.OrgCode == userInfoNew.OrganizationCode || codeList.Contains(x.OrgCode));
            }

            var query = _zuZhiGongZhangRepository.GetQuery(gongzhangExp);
            if (!string.IsNullOrWhiteSpace(dto.ChuangJianDanWeiOrgCode))
            {
                query = query.Where(m => m.ChuangJianDanWeiOrgCode.Contains(dto.ChuangJianDanWeiOrgCode.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.OrgName))
            {
                query = query.Where(m => m.OrgName.Contains(dto.OrgName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.OrgCode))
            {
                query = query.Where(m => m.OrgCode.Contains(dto.OrgCode.Trim()));
            }
            if (dto.GongZhangZhaoPianId.HasValue)
            {
                query = query.Where(m => m.GongZhangZhaoPianId == dto.GongZhangZhaoPianId);
            }
            result.Data.totalcount = query.Count();
            result.Data.items = Mapper.Map<List<ZuZhiGongZhangXinXiDto>>(query.OrderByDescending(m => m.SYS_ChuangJianShiJian).Skip((page - 1) * rows).Take(rows));
            return result;
        }
        public override void Dispose()
        {
            _zuZhiGongZhangRepository.Dispose();
        }
    }
}
