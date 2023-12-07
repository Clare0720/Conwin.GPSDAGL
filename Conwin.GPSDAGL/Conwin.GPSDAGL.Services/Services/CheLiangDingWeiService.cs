using Conwin.EntityFramework;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.Redis;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDingWei;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class CheLiangDingWeiService : ApiServiceBase, ICheLiangDingWeiService
    {
        private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;
        private readonly ICheLiangRepository _cheLiangRepository;
        public CheLiangDingWeiService(
            IBussinessLogger bussinessLogger,
            ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository, ICheLiangRepository cheLiangRepository)
            : base(bussinessLogger)
        {
            _cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;
            _cheLiangRepository = cheLiangRepository;
        }

        public override void Dispose()
        {
            _cheLiangDingWeiXinXiRepository.Dispose();
        }


        #region 更新车辆定位信息（有则更新，无则添加）

        /// <summary>
        /// 更新车辆定位信息（有则更新，无则添加）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> UpdateGPSInfo(CheLiangDingWeiAddReqDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (dto == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "车辆定位信息数据不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.RegistrationNo))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "车牌号不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.RegistrationNoColor))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "车牌颜色不能为空",
                        Data = false
                    };
                }
                if (!dto.LatestLongtitude.HasValue)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "定位经度（LatestLongtitude）不能为空",
                        Data = false
                    };
                }
                if (!dto.LatestLatitude.HasValue)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "定位纬度（LatestLatitude）不能为空",
                        Data = false
                    };
                }

                //利用Redis判重
                var duration = CommonHelper.ToInt(System.Configuration.ConfigurationManager.AppSettings["GPSDWCacheDuration"], 600);
                var appCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"];
                var key = $"{appCode}-GPSDW:{dto.RegistrationNo}{dto.RegistrationNoColor}";
                if (!RedisManager.HasKey(key))
                {
                    //有则更新，无则添加
                    var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                    var model = _cheLiangDingWeiXinXiRepository.GetQuery(d => d.SYS_XiTongZhuangTai == sysStatus && d.RegistrationNo == dto.RegistrationNo && d.RegistrationNoColor == dto.RegistrationNoColor).FirstOrDefault();
                    if (model != null && model.RegistrationNo == dto.RegistrationNo && model.RegistrationNoColor == dto.RegistrationNoColor)
                    {
                        model.LatestGpsTime = dto.LatestGpsTime;
                        model.LatestRecvTime = dto.LatestRecvTime;
                        model.LatestLongtitude = dto.LatestLongtitude;
                        model.LatestLatitude = dto.LatestLatitude;
                        model.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        int uptRes = 0;
                        using (var uow = new UnitOfWork())
                        {
                            uow.BeginTransaction();
                            _cheLiangDingWeiXinXiRepository.Update(model);
                            uptRes = uow.CommitTransaction();
                        }
                        if (uptRes > 0)
                        {
                            //设置缓存
                            var time = new TimeSpan(0, 0, duration);
                            RedisManager.Set(key, model, time);

                            return new ServiceResult<bool>() { Data = true };
                        }
                        else
                        {
                            var currMethod = System.Reflection.MethodBase.GetCurrentMethod();
                            LogHelper.Error($"调用{currMethod.DeclaringType.FullName}.{currMethod.Name}更新车辆定位信息出错。入参：{JsonConvert.SerializeObject(dto)}");
                            return new ServiceResult<bool>()
                            {
                                StatusCode = 2,
                                ErrorMessage = "车辆定位信息更新失败"
                            };
                        }
                    }
                    else
                    {
                        var newModel = new CheLiangDingWeiXinXi()
                        {
                            Id = Guid.NewGuid(),
                            RegistrationNo = dto.RegistrationNo,
                            RegistrationNoColor = dto.RegistrationNoColor,
                            LatestGpsTime = dto.LatestGpsTime,
                            FirstUploadTime = DateTime.Now,
                            LatestRecvTime = dto.LatestRecvTime,
                            LatestLongtitude = dto.LatestLongtitude,
                            LatestLatitude = dto.LatestLatitude,
                            SYS_XiTongZhuangTai = sysStatus,
                            SYS_ChuangJianShiJian = DateTime.Now
                        };
                        int addRes = 0;
                        using (var uow = new UnitOfWork())
                        {
                            uow.BeginTransaction();
                            _cheLiangDingWeiXinXiRepository.Add(newModel);
                            addRes = uow.CommitTransaction();
                        }
                        if (addRes > 0)
                        {
                            //设置缓存
                            var time = new TimeSpan(0, 0, duration);
                            RedisManager.Set(key, model, time);

                            return new ServiceResult<bool>() { Data = true };
                        }
                        else
                        {
                            var currMethod = System.Reflection.MethodBase.GetCurrentMethod();
                            LogHelper.Error($"调用{currMethod.DeclaringType.FullName}.{currMethod.Name}添加车辆定位信息出错。入参：{JsonConvert.SerializeObject(dto)}");
                            return new ServiceResult<bool>()
                            {
                                StatusCode = 2,
                                ErrorMessage = "车辆定位信息添加失败"
                            };
                        }
                    }
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "车辆定位信息写入过于频繁",
                        Data = false
                    };
                }
            });
        }

        #endregion

        #region 车辆在线状态变更（推送到推送服务中心）

        /// <summary>
        /// 车辆在线状态变更（推送到推送服务中心）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> ZaiXianZhuangTaiChange(ZaiXianZhuangTaiChangeDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                if (dto == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "参数不能为空" };
                }
                if (string.IsNullOrWhiteSpace(dto.ChePaiHao)
                    || string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆信息不能为空" };
                }
                if (dto.ZaiXianZhuangTai != 0
                    && dto.ZaiXianZhuangTai != 1)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆在线状态有误" };
                }

                //更新数据库车辆在线状态
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                int zaixianZhuangTai = dto.ZaiXianZhuangTai == 1 ? (int)ZaiXianZhuangTai.在线 : (int)ZaiXianZhuangTai.离线;
                DateTime nowTime = DateTime.Now;
                Expression<Func<CheLiang, bool>> filterExp = cl => cl.SYS_XiTongZhuangTai == sysStatus
                          && cl.ChePaiHao.Trim() == dto.ChePaiHao.Trim() && cl.ChePaiYanSe.Trim() == dto.ChePaiYanSe.Trim();
                Expression<Func<CheLiang, CheLiang>> updateExp = upt => new CheLiang() { ZaiXianZhuangTai = zaixianZhuangTai, SYS_ZuiJinXiuGaiShiJian = nowTime };
                _cheLiangRepository.Update(filterExp, updateExp);
            

                return new ServiceResult<bool>() { StatusCode = 0, Data = true };
            });
        }

        #endregion

       


    }
}
