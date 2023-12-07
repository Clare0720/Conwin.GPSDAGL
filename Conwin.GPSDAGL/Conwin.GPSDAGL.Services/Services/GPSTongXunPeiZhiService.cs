using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class GPSTongXunPeiZhiService : ApiServiceBase, IGPSTongXunPeiZhiService
    {
        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository _cheLiangGPSTongXunPeiZhiRepository;
        private readonly ICheLiangRepository _cheLiangRepository;

        public GPSTongXunPeiZhiService(
            IBussinessLogger _bussinessLogger,
            ICheLiangRepository cheLiangXinXiRepository,
            ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository cheLiangGPSTongXunPeiZhiRepository,
            ICheLiangRepository cheLiangRepository
            ) : base(_bussinessLogger)
        {
            _cheLiangXinXiRepository = cheLiangXinXiRepository;
            _cheLiangGPSTongXunPeiZhiRepository = cheLiangGPSTongXunPeiZhiRepository;
            _cheLiangRepository = cheLiangRepository;
        }

        #region 获取车辆GPS终端数据通讯配置信息

        /// <summary>
        ///  获取车辆GPS终端数据通讯配置信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<GPSTongXunPeiZhiDto> GetGPSPeiZhiXinXi(GPSTongXunPeiZhiReqDto dto)
        {
            return ExecuteCommandStruct<GPSTongXunPeiZhiDto>(() =>
            {
                if (dto == null || dto.CheLiangID == null || dto.ZhongDuanID == null)
                {
                    return new ServiceResult<GPSTongXunPeiZhiDto>() { StatusCode = 2, ErrorMessage = "请求参数不能为空" };
                }
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var gpstx = _cheLiangGPSTongXunPeiZhiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == sysZhengChang && p.CheLiangID == dto.CheLiangID && p.ZhongDuanID == dto.ZhongDuanID).FirstOrDefault();
                if (gpstx == null || gpstx.Id == null)
                {
                    return new ServiceResult<GPSTongXunPeiZhiDto>() { StatusCode = 2, ErrorMessage = "未找到该GPS终端通讯配置数据" };
                }
                return new ServiceResult<GPSTongXunPeiZhiDto>()
                {
                    Data = new GPSTongXunPeiZhiDto()
                    {
                        CheLiangID = gpstx.CheLiangID,
                        ZhongDuanID = gpstx.ZhongDuanID,
                        XieYiLeiXing = gpstx.XieYiLeiXing,
                        ZhuaBaoLaiYuan = gpstx.ZhuaBaoLaiYuan,
                        BanBenHao = gpstx.BanBenHao
                    }
                };
            });
        }

        #endregion

        #region 添加或更新车辆GPS终端数据通讯配置信息

        /// <summary>
        /// 添加或更新车辆GPS终端数据通讯配置信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> SetGPSPeiZhiXinXi(GPSTongXunPeiZhiDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool>() { StatusCode = 2, ErrorMessage = "获取用户信息失败" };
                }
                if (dto == null || dto.CheLiangID == null || dto.ZhongDuanID == null)
                {
                    return new ServiceResult<bool>() { StatusCode = 2, ErrorMessage = "请求参数不能为空" };
                }
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var gpstx = _cheLiangGPSTongXunPeiZhiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == sysZhengChang && p.CheLiangID == dto.CheLiangID && p.ZhongDuanID == dto.ZhongDuanID).FirstOrDefault();
                bool isNew = false;//是否新增
                if (gpstx == null || gpstx.Id == null)
                {
                    //新增
                    gpstx = new CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi()
                    {
                        Id = Guid.NewGuid(),
                        CheLiangID = dto.CheLiangID,
                        ZhongDuanID = dto.ZhongDuanID,
                        XieYiLeiXing = dto.XieYiLeiXing,
                        ZhuaBaoLaiYuan = dto.ZhuaBaoLaiYuan,
                        BanBenHao = dto.BanBenHao
                    };
                    SetCreateSYSInfo(gpstx, userInfo);
                    isNew = true;
                }
                else
                {
                    //修改
                    gpstx.XieYiLeiXing = dto.XieYiLeiXing;
                    gpstx.ZhuaBaoLaiYuan = dto.ZhuaBaoLaiYuan;
                    gpstx.BanBenHao = dto.BanBenHao;
                    SetUpdateSYSInfo(gpstx, gpstx, userInfo);
                    isNew = false;
                }
                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (isNew)
                    {
                        _cheLiangGPSTongXunPeiZhiRepository.Add(gpstx);
                    }
                    else
                    {
                        _cheLiangGPSTongXunPeiZhiRepository.Update(gpstx);
                    }
                    res = uow.CommitTransaction();
                }
                if (res > 0)
                {
                    return new ServiceResult<bool>() { StatusCode = 0, Data = true };
                }
                else
                {
                    return new ServiceResult<bool>() { StatusCode = 2, ErrorMessage = "GPS终端通讯配置数据保存失败" };
                }
            });
        }



        /// <summary>
        /// 根据车牌号车牌颜色查询车辆终端通讯配置信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<GPSTongXunPeiZhiDto> GetGPSPeiZhiXinXiByChePaiHao(GPSTongXunPeiZhiReqDto dto)
        {
            return ExecuteCommandStruct<GPSTongXunPeiZhiDto>(() =>
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.ChePaiHao) || string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    return new ServiceResult<GPSTongXunPeiZhiDto>() { StatusCode = 2, ErrorMessage = "请求参数不能为空" };
                }
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == sysZhengChang;
                if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    carExp = carExp.And(x => x.ChePaiHao == dto.ChePaiHao.Trim());
                }
                if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    carExp = carExp.And(x => x.ChePaiYanSe == dto.ChePaiYanSe.Trim());
                }

                var gpstx = (from tx in _cheLiangGPSTongXunPeiZhiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang)
                             join car in _cheLiangRepository.GetQuery(carExp)
                             on tx.CheLiangID equals car.Id
                             select new
                             {
                                 Id = tx.Id,
                                 CheLiangID = tx.CheLiangID,
                                 ZhongDuanID = tx.ZhongDuanID,
                                 XieYiLeiXing = tx.XieYiLeiXing,
                                 ZhuaBaoLaiYuan = tx.ZhuaBaoLaiYuan,
                                 BanBenHao = tx.BanBenHao,
                             }).FirstOrDefault();

                if (gpstx == null || gpstx.Id == null)
                {
                    return new ServiceResult<GPSTongXunPeiZhiDto>() { StatusCode = 2, ErrorMessage = "未找到该GPS终端通讯配置数据" };
                }
                return new ServiceResult<GPSTongXunPeiZhiDto>()
                {
                    Data = new GPSTongXunPeiZhiDto()
                    {
                        CheLiangID = gpstx.CheLiangID,
                        ZhongDuanID = gpstx.ZhongDuanID,
                        XieYiLeiXing = gpstx.XieYiLeiXing,
                        ZhuaBaoLaiYuan = gpstx.ZhuaBaoLaiYuan,
                        BanBenHao = gpstx.BanBenHao
                    }
                };
            });
        }


        #endregion



        public override void Dispose()
        {
            _cheLiangXinXiRepository.Dispose();
        }
    }
}
