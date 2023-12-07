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
using Conwin.GPSDAGL.Services.DtosExt.RenYuan;
using Conwin.GPSDAGL.Services.Interfaces;
using Nest;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class DangYuanXinXiService : ApiServiceBase, IDangYuanXinXiService
    {
        private readonly IDangYuanRepository _dangYuanRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        public DangYuanXinXiService(
            IDangYuanRepository dangYuanRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            IBussinessLogger _bussinessLogger
            ) : base(_bussinessLogger)
        {
            _dangYuanRepository = dangYuanRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;

        }
        /// <summary>
        /// 新增党员信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Create(DangYuanDto model, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();
                result.StatusCode = 0;
                if (userInfo == null)
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "获取用户信息失败";
                    return result;
                }
                int repeatIdCardCount = _dangYuanRepository.GetQuery(x => x.IDCard == model.IDCard).Count();
                if (repeatIdCardCount > 0)
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "证件号码已存在";
                    return result;
                }
                int repeatContactNumberCount = _dangYuanRepository.GetQuery(x => x.ContactNumber == model.ContactNumber).Count();
                if (repeatContactNumberCount > 0)
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "手机号码重复";
                    return result;
                }
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    var entity = Mapper.Map<DangYuan>(model);
                    entity.OrgCode = userInfo.OrganizationCode;
                    entity.Id = Guid.NewGuid();
                    //插入系统信息
                    SetCreateSYSInfo(entity, userInfo);
                    _dangYuanRepository.Add(entity);
                    var commitRes = uow.CommitTransaction() > 0;
                    result.Data = commitRes;
                    result.StatusCode = commitRes ? 0 : 2;
                    result.ErrorMessage = commitRes ? "" : "新增党员档案数据失败";
                }
                return result;
            });
        }
        /// <summary>
        /// 删除党员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Delete(Guid[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
           {
               var result = new ServiceResult<bool>() { Data = true };
               if (result.Data)
               {
                   using (var uow = new UnitOfWork())
                   {
                       uow.BeginTransaction();

                       _dangYuanRepository.Update(
                           m => ids.Contains(m.Id),
                           n => new DangYuan()
                           {
                               SYS_XiTongZhuangTai = 1,
                               SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                               SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                               SYS_ZuiJinXiuGaiShiJian = DateTime.Now
                           });
                       result.Data = uow.CommitTransaction() >= 0;
                       result.StatusCode = result.Data ? 0 : 2;
                       result.ErrorMessage = result.Data ? "" : "删除失败";
                   }
               }
               return result;
           });
        }


        /// <summary>
        /// 查看党员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<object> Get(DangYuanDto dto)
        {
            return ExecuteCommandStruct<object>(() =>
            {
                var id = new Guid(dto.Id);
                var resp = _dangYuanRepository.GetQuery(x => x.Id == id && x.SYS_XiTongZhuangTai == 0);
                return new ServiceResult<object> { Data = resp };
            });
        }

        public ServiceResult<QueryResult> Query(QueryData queryData, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<QueryResult>(() =>
             {
                DangYuanListQueryDto search = JsonConvert.DeserializeObject<DangYuanListQueryDto>(queryData.data.ToString());
                var userInfoNew = GetUserInfo();
                QueryResult result = new QueryResult();
                if (userInfoNew == null)
                {
                    return new ServiceResult<QueryResult>() { StatusCode = 2, ErrorMessage = "获取用户信息失败" };
                }
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                 Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == sysZhengChang;

                 if (userInfoNew.OrganizationType == (int)OrganizationType.市政府)
                 {
                     OrgBaseExp = OrgBaseExp.And(x => x.XiaQuShi == userInfoNew.OrganCity);
                 }
                 if (userInfoNew.OrganizationType == (int)OrganizationType.县政府)
                 {
                     OrgBaseExp = OrgBaseExp.And(x => x.XiaQuXian == userInfoNew.OrganDistrict);
                 }
                 if (userInfoNew.OrganizationType == (int)OrganizationType.企业)
                 {
                     OrgBaseExp = OrgBaseExp.And(x => x.OrgCode == userInfoNew.OrganizationCode);
                 }

                 var dangYuanList = _dangYuanRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang);
                var OrgInfo = _orgBaseInfoRepository.GetQuery(OrgBaseExp);
                var query = from  dy in dangYuanList 
                            join qiye in OrgInfo on dy.OrgCode equals qiye.OrgCode 
                            where string.IsNullOrEmpty(search.Name)|| dy.Name.Contains(search.Name)
                            where string.IsNullOrEmpty(search.QiYeMingCheng) || qiye.OrgName.Contains(search.QiYeMingCheng)
                            where string.IsNullOrEmpty(search.IDCard) || dy.IDCard.Contains(search.IDCard)
                            select new
                            {
                                dy.Id,
                                qiye.OrgName,
                                dy.Name,
                                dy.IDCard,
                                dy.Sex,
                                dy.Nation,
                                dy.NativePlace,
                                dy.Education,
                                dy.SYS_ChuangJianShiJian
                            };
                result.totalcount = query.Count();
                result.items = query.Distinct().OrderByDescending(x => x.SYS_ChuangJianShiJian).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                return new ServiceResult<QueryResult>() { Data = result };
            });
        }

        public ServiceResult<bool> Update(DangYuanDto model, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();
                result.StatusCode = 0;
                if (userInfo == null)
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "获取用户信息失败";
                    return result;
                }
                var id = new Guid(model.Id);
                var resp = _dangYuanRepository.GetQuery(x => x.Id == id && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (resp == null)
                {
                    result.Data = false;
                    result.StatusCode = 2;
                    result.ErrorMessage = "获取党员档案信息失败";
                    return result;
                }
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (!string.IsNullOrWhiteSpace(model.IDCard))
                    {
                        int repeatIdCardCount = _dangYuanRepository.GetQuery(x => x.IDCard == model.IDCard && x.Id != id).Count();
                        if (repeatIdCardCount > 0)
                        {
                            result.Data = false;
                            result.StatusCode = 2;
                            result.ErrorMessage = "证件号码已存在";
                            return result;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(model.ContactNumber))
                    {
                        int repeatContactNumberCount = _dangYuanRepository.GetQuery(x => x.ContactNumber == model.ContactNumber && x.Id != id).Count();
                        if (repeatContactNumberCount > 0)
                        {
                            result.Data = false;
                            result.StatusCode = 2;
                            result.ErrorMessage = "手机号码重复";
                            return result;
                        }
                    }

                    resp.Name = model.Name;
                    resp.IDCard = model.IDCard;
                    resp.NativePlace = model.NativePlace;
                    resp.Position = model.Position;
                    resp.Education = model.Education;
                    resp.Degree = model.Degree;
                    resp.Sex = model.Sex;
                    resp.ContactNumber = model.ContactNumber;
                    resp.EntryDate = model.EntryDate;
                    resp.DangZuZhiSuoZaiDi = model.DangZuZhiSuoZaiDi;
                    resp.LiuDongDangYuan = model.LiuDongDangYuan;
                    resp.Nation = model.Nation;
                    //resp.OrgCode = model.OrgCode;
                    resp.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                    resp.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                    resp.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    _dangYuanRepository.Update(resp);

                    result.Data = uow.CommitTransaction() > 0;
                    result.StatusCode = result.Data ? 0 : 2;
                    result.ErrorMessage = result.Data ? "" : "修改人员信息失败";
                };


                return result;
            });
        }

        public override void Dispose()
        {
            _dangYuanRepository.Dispose();
        }


    }
}
