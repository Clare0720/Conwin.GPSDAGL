using AutoMapper;
using Conwin.EntityFramework;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Framework.OperationLog;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class VideoZhongDuanService : ApiServiceBase, IVideoZhongDuanService
    {
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly ICheLiangYeHuRepository _yeHuRepository;
        private readonly IFuWuShangRepository _fuWuShangRepository;
        private readonly IZhongDuanFileMapperRepository _zhongDuanFileMapperRepository;
        private readonly IFuWuShangCheLiangRepository _fuWuShangCheLiangRepository;
        private readonly IFuWuShangCheLiangGPSZhongDuanXinXiRepository _fuWuShangCheLiangGPSZhongDuanXinXiRepository;
        private readonly IFuWuShangCheLiangVideoZhongDuanXinXiRepository _fuWuShangCheLiangVideoZhongDuanXinXiRepository;
        private readonly IFuWuShangZhongDuanFileMapperRepository _fuWuShangZhongDuanFileMapperRepository;
        private readonly ICheLiangBaoXianXinXiRepository _cheLiangBaoXianXinXiRepository;
        private readonly ICheLiangYeHuLianXiXinXiRepository _cheLiangYeHuLianXiXinXiRepository;
        private readonly IFuWuShangCheLiangBaoXianXinXiRepository _fuWuShangCheLiangBaoXianXinXiRepository;
        private readonly IFuWuShangCheLiangYeHuLianXiXinXiRepository _fuWuShangCheLiangYeHuLianXiXinXiRepository;
        private readonly IFuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
        private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;
        private readonly ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
        public VideoZhongDuanService(ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository,
            ICheLiangRepository cheLiangXinXiRepository,
            ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
            ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
            ICheLiangYeHuRepository yeHuRepository,
            IFuWuShangRepository fuWuShangRepository,
        IZhongDuanFileMapperRepository zhongDuanFileMapperRepository,
        IFuWuShangCheLiangVideoZhongDuanXinXiRepository fuWuShangCheLiangVideoZhongDuanXinXiRepository,
        IFuWuShangCheLiangGPSZhongDuanXinXiRepository fuWuShangCheLiangGPSZhongDuanXinXiRepository,
        IFuWuShangCheLiangRepository fuWuShangCheLiangRepository,
        IFuWuShangZhongDuanFileMapperRepository fuWuShangZhongDuanFileMapperRepository,
        ICheLiangBaoXianXinXiRepository cheLiangBaoXianXinXiRepository,
        ICheLiangYeHuLianXiXinXiRepository cheLiangYeHuLianXiXinXiRepository,
        IFuWuShangCheLiangBaoXianXinXiRepository fuWuShangCheLiangBaoXianXinXiRepository,
        IFuWuShangCheLiangYeHuLianXiXinXiRepository fuWuShangCheLiangYeHuLianXiXinXiRepository,
        IBussinessLogger _bussinessLogger, IFuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository, ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository,
        ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository) : base(_bussinessLogger)
        {
            _cheLiangXinXiRepository = cheLiangXinXiRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
            _yeHuRepository = yeHuRepository;
            _fuWuShangRepository = fuWuShangRepository;
            _zhongDuanFileMapperRepository = zhongDuanFileMapperRepository;
            _fuWuShangCheLiangRepository = fuWuShangCheLiangRepository;
            _fuWuShangCheLiangGPSZhongDuanXinXiRepository = fuWuShangCheLiangGPSZhongDuanXinXiRepository;
            _fuWuShangCheLiangVideoZhongDuanXinXiRepository = fuWuShangCheLiangVideoZhongDuanXinXiRepository;
            _fuWuShangZhongDuanFileMapperRepository = fuWuShangZhongDuanFileMapperRepository;
            _cheLiangBaoXianXinXiRepository = cheLiangBaoXianXinXiRepository;
            _cheLiangYeHuLianXiXinXiRepository = cheLiangYeHuLianXiXinXiRepository;
            _fuWuShangCheLiangBaoXianXinXiRepository = fuWuShangCheLiangBaoXianXinXiRepository;
            _fuWuShangCheLiangYeHuLianXiXinXiRepository = fuWuShangCheLiangYeHuLianXiXinXiRepository;
            _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository = fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository = cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
            _cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;
        }

        public ServiceResult<VideoZhongDuanXinXiExDto> Get(Guid id, UserInfoDto userInfo)
        {
            try
            {
                var result = new ServiceResult<VideoZhongDuanXinXiExDto>();
                var list = from a in _cheLiangXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           join b in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           on a.Id.ToString() equals b.CheLiangId
                           join c in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           on a.Id.ToString() equals c.CheLiangId
                           join d in _yeHuRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           on a.YeHuOrgCode equals d.OrgCode
                           join f in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           on a.Id.ToString() equals f.CheLiangId
                           into temp
                           from g in temp.DefaultIfEmpty()
                           join h in _fuWuShangRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           on a.FuWuShangOrgCode equals h.OrgCode
                           into temp1
                           from l in temp1.DefaultIfEmpty()
                           join w in _cheLiangDingWeiXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                           .OrderByDescending(o => o.SYS_ChuangJianShiJian)
                           on new { plate = a.ChePaiHao, color = a.ChePaiYanSe } equals new { plate = w.RegistrationNo, color = w.RegistrationNoColor } into wds
                           where a.Id == id
                           select new VideoZhongDuanXinXiExDto()
                           {
                               ChePaiHao = a.ChePaiHao,
                               ChePaiYanSe = a.ChePaiYanSe,
                               CheLiangZhongLei = a.CheLiangZhongLei,
                               OrgName = d.OrgName,
                               SheBeiXingHao = b.SheBeiXingHao,
                               FuWuShangMingCheng = l.OrgName,
                               SheBeiJiShenLeiXing = b.SheBeiJiShenLeiXing,
                               SheBeiGouCheng = b.SheBeiGouCheng,
                               ChangJiaBianHao = b.ChangJiaBianHao,
                               ShengChanChangJia = b.ShengChanChangJia,
                               AnZhuangShiJian = b.AnZhuangShiJian,
                               ShiPinTouGeShu = c.ShiPinTouGeShu,
                               SIMKaHao = "",
                               ZhongDuanMDT = b.ZhongDuanMDT,
                               ShiPingChangShangLeiXing = c.ShiPingChangShangLeiXing,
                               ShiPinTouAnZhuangXuanZe = c.ShiPinTouAnZhuangXuanZe,
                               ZhongDuanId = b.Id,
                               CheLiangId = a.Id,
                               NeiRong = g.NeiRong,
                               ShuJuJieRu = g.ShuJuJieRu,
                               SheBeiWanZheng = g.SheBeiWanZheng,
                               BeiAnZhuangTai = g.BeiAnZhuangTai,
                               LatestGpsTime = wds.FirstOrDefault().LatestGpsTime
                           };

                var model = list.FirstOrDefault();
                if (model != null)
                {
                    var fileList = from a in _zhongDuanFileMapperRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0)
                                   where a.ZhongDuanId == model.ZhongDuanId
                                   select a;
                    model.FileList = fileList.ToList();
                }
                result.Data = model;
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取智能视频终端报警信息失败，" + ex.Message, ex);
                return new ServiceResult<VideoZhongDuanXinXiExDto> { StatusCode = 2, ErrorMessage = "获取信息失败" };
            }
        }


        public ServiceResult<bool> Confirm(CheLiangVideoZhongDuanConfirmDto dto, UserInfoDto userInfo)
        {

            Mapper.CreateMap<CheLiangVideoZhongDuanConfirmDto, CheLiangVideoZhongDuanConfirm>();
            var entity = Mapper.Map<CheLiangVideoZhongDuanConfirm>(dto);

            if (string.IsNullOrWhiteSpace(dto.CheLiangId))
            {
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
            }
            var cheliangId = new Guid(dto.CheLiangId);
            var carModel = _cheLiangXinXiRepository.GetQuery(x => x.Id == cheliangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
            if (carModel == null)
            {
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "找不到对应的车辆信息", Data = false };
            }

            var existEntity = _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.CheLiangId == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();

            //修改数据对比
            Mapper.CreateMap<CheLiangVideoZhongDuanConfirmDto, CheLiangVideoZhongDuanConfirmInfoDto>();
            Mapper.CreateMap<CheLiangVideoZhongDuanConfirm, CheLiangVideoZhongDuanConfirmInfoDto>();

            CheLiangVideoZhongDuanConfirmInfoDto oldModel = Mapper.Map<CheLiangVideoZhongDuanConfirmInfoDto>(existEntity);
            CheLiangVideoZhongDuanConfirmInfoDto newModel = Mapper.Map<CheLiangVideoZhongDuanConfirmInfoDto>(dto);
            if (oldModel == null)
            {
                oldModel = new CheLiangVideoZhongDuanConfirmInfoDto() { SheBeiWanZheng = false, ShuJuJieRu = false };
            }
            List<LogUpdateValueDto> updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, newModel, true);

            bool addResult = false;
            bool isUpdateBeiAnStatus = true;
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                if (existEntity != null)
                {
                    //修改为每次保存都同步
                    //if (existEntity?.BeiAnZhuangTai != entity.BeiAnZhuangTai)
                    //{
                    //    isUpdateBeiAnStatus = true;
                    //}
                    existEntity.BeiAnZhuangTai = entity.BeiAnZhuangTai;
                    existEntity.NeiRong = entity.NeiRong;
                    existEntity.ShuJuJieRu = entity.ShuJuJieRu;
                    existEntity.SheBeiWanZheng = entity.SheBeiWanZheng;
                    existEntity.ZhongDuanId = entity.ZhongDuanId;
                    existEntity.CheLiangId = entity.CheLiangId;
                    existEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    if (userInfo != null)
                    {
                        existEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        existEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                    }
                    _cheLiangVideoZhongDuanConfirmRepository.Update(existEntity);
                }
                else
                {
                    //isUpdateBeiAnStatus = true;
                    entity.Id = Guid.NewGuid();
                    entity.SYS_XiTongZhuangTai = 0;
                    entity.SYS_ChuangJianShiJian = DateTime.Now;

                    entity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    if (userInfo != null)
                    {
                        entity.SYS_ChuangJianRenID = userInfo.Id;
                        entity.SYS_ChuangJianRen = userInfo.UserName;
                        entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                    }

                    _cheLiangVideoZhongDuanConfirmRepository.Add(entity);
                }
                addResult = uow.CommitTransaction() > 0;
            }
            if (addResult)
            {


                //发生备案状态变更时同步
                if (isUpdateBeiAnStatus)
                {
                    if (!string.IsNullOrWhiteSpace(dto.CheLiangId))
                    {
                        var updateModel = new UpdateBeiAnZhuangTaiToFuWuShangDto
                        {
                            cheliangID = dto.CheLiangId,
                            beiAnZhuangTai = dto.BeiAnZhuangTai
                        };
                        UpdateBeiAnXinXiToFuWuShang(updateModel);
                    }
                }

                if (userInfo == null)
                {
                    userInfo = new UserInfoDto
                    {
                        UserName = "分析程序",
                        OrganizationCode = "0000",
                        OrganizationName = "系统组织"
                    };
                }

                //记录日志
                OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                {
                    SystemName = OprateLogHelper.GetSysTemName(),
                    ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                    ActionName = nameof(Confirm),
                    BizOperType = OperLogBizOperType.UPDATE,
                    ShortDescription = "车辆档案核查安装：" + carModel?.ChePaiHao + "[" + carModel.ChePaiYanSe + "]-" + typeof(ZhongDuanBeiAnZhuangTai).GetEnumName(dto.BeiAnZhuangTai),
                    OperatorName = userInfo?.UserName,
                    OldBizContent = JsonConvert.SerializeObject(dto),
                    OperatorID = userInfo?.Id,
                    OperatorOrgCode = userInfo?.OrganizationCode,
                    OperatorOrgName = userInfo?.OrganizationName,
                    SysID = SysId,
                    AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                });


                return new ServiceResult<bool>() { Data = true };
            }
            else
            {
                return new ServiceResult<bool>() { Data = false, StatusCode = 2, ErrorMessage = "核查出错" };
            }

        }


        private void UpdateBeiAnXinXiToFuWuShang(UpdateBeiAnZhuangTaiToFuWuShangDto dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    userInfo = new UserInfoDtoNew()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "分析程序"
                    };
                }
                //从车辆档案查找车辆信息
                Guid carId = new Guid(dto.cheliangID);
                var cheliangModel = _cheLiangXinXiRepository.GetQuery(x => x.Id == carId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(cheliangModel?.FuWuShangOrgCode))
                {
                    //服务商车辆信息
                    var fuWuShangCarModel = _fuWuShangCheLiangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.ChePaiHao == cheliangModel.ChePaiHao && x.ChePaiYanSe == cheliangModel.ChePaiYanSe && x.FuWuShangOrgCode == cheliangModel.FuWuShangOrgCode).FirstOrDefault();
                    if (fuWuShangCarModel != null)
                    {

                        using (var uow = new UnitOfWork())
                        {
                            uow.BeginTransaction();
                            //删除服务商档案中存在的同车牌车牌颜色的车
                            var fwsCarInfoModel = _fuWuShangCheLiangRepository.GetQuery(x => x.Id == fuWuShangCarModel.Id && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                            if (fwsCarInfoModel != null)
                            {
                                SetDeleteSYSInfo(fwsCarInfoModel, userInfo);
                                fwsCarInfoModel.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangRepository.Update(fwsCarInfoModel);
                            }

                            //新增基础档案车辆到服务商车辆档案
                            Mapper.CreateMap<CheLiang, FuWuShangCheLiang>();
                            FuWuShangCheLiang addCarModel = Mapper.Map<FuWuShangCheLiang>(cheliangModel);
                            SetCreateSYSInfo(addCarModel, userInfo);
                            addCarModel.BeiAnZhuangTai = dto.beiAnZhuangTai;
                            addCarModel.Id = Guid.NewGuid();
                            addCarModel.SYS_ChuangJianShiJian = fwsCarInfoModel?.SYS_ChuangJianShiJian;
                            addCarModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            addCarModel.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            _fuWuShangCheLiangRepository.Add(addCarModel);
                            //同步GPS终端信息到服务商GPS终端信息
                            string fuWuShangCheLiangId = fuWuShangCarModel.Id.ToString();

                            var fwsGpsZhongDuanModel = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == fuWuShangCheLiangId && x.SYS_XiTongZhuangTai == 0).ToList();
                            fwsGpsZhongDuanModel.ForEach(x =>
                            {
                                SetDeleteSYSInfo(x, userInfo);
                                x.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangGPSZhongDuanXinXiRepository.Update(x);
                            });


                            string dangAnCarId = cheliangModel.Id.ToString();
                            var gpsZhongDuanXinXiModel = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dangAnCarId).FirstOrDefault();
                            if (gpsZhongDuanXinXiModel == null)
                            {
                                gpsZhongDuanXinXiModel = new CheLiangGPSZhongDuanXinXi();
                            }
                            Mapper.CreateMap<CheLiangGPSZhongDuanXinXi, FuWuShangCheLiangGPSZhongDuanXinXi>();
                            FuWuShangCheLiangGPSZhongDuanXinXi addGpsZhongDuanXinXiModel = Mapper.Map<FuWuShangCheLiangGPSZhongDuanXinXi>(gpsZhongDuanXinXiModel);
                            SetCreateSYSInfo(addGpsZhongDuanXinXiModel, userInfo);
                            addGpsZhongDuanXinXiModel.Id = Guid.NewGuid();
                            addGpsZhongDuanXinXiModel.FuWuShangCheLiangId = addCarModel.Id.ToString();
                            _fuWuShangCheLiangGPSZhongDuanXinXiRepository.Add(addGpsZhongDuanXinXiModel);

                            //同步智能视频终端信息到服务商智能视频信息

                            var fwsVideoZhongDuanModel = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == fuWuShangCheLiangId && x.SYS_XiTongZhuangTai == 0).ToList();
                            fwsVideoZhongDuanModel.ForEach(x =>
                            {
                                SetDeleteSYSInfo(x, userInfo);
                                x.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangVideoZhongDuanXinXiRepository.Update(x);
                                //删除终端随带附件
                                var fwsFileList = _fuWuShangZhongDuanFileMapperRepository.GetQuery(z => z.FuWuShangZhongDuanId == x.Id).ToList();
                                fwsFileList.ForEach(y =>
                                {
                                    SetDeleteSYSInfo(y, userInfo);
                                    y.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                    _fuWuShangZhongDuanFileMapperRepository.Update(y);
                                });

                            });
                            var videoZhongDuanXinXiModel = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dangAnCarId).FirstOrDefault();
                            if (videoZhongDuanXinXiModel == null)
                            {
                                videoZhongDuanXinXiModel = new CheLiangVideoZhongDuanXinXi();
                            }
                            Mapper.CreateMap<CheLiangVideoZhongDuanXinXi, FuWuShangCheLiangVideoZhongDuanXinXi>();
                            FuWuShangCheLiangVideoZhongDuanXinXi addVideoZhongDuanXinXiModel = Mapper.Map<FuWuShangCheLiangVideoZhongDuanXinXi>(videoZhongDuanXinXiModel);
                            SetCreateSYSInfo(addGpsZhongDuanXinXiModel, userInfo);
                            addVideoZhongDuanXinXiModel.Id = Guid.NewGuid();
                            addVideoZhongDuanXinXiModel.FuWuShangCheLiangId = addCarModel.Id.ToString();
                            _fuWuShangCheLiangVideoZhongDuanXinXiRepository.Add(addVideoZhongDuanXinXiModel);


                            //同步业户联系信息与保险信息
                            var fwsYeHuLianXiList = _fuWuShangCheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == fuWuShangCarModel.Id).ToList();
                            fwsYeHuLianXiList.ForEach(x =>
                            {
                                SetDeleteSYSInfo(x, userInfo);
                                x.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangYeHuLianXiXinXiRepository.Update(x);
                            });
                            var fwsBaoXianList = _fuWuShangCheLiangBaoXianXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == fuWuShangCarModel.Id).ToList();
                            fwsBaoXianList.ForEach(x =>
                            {
                                SetDeleteSYSInfo(x, userInfo);
                                x.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangBaoXianXinXiRepository.Update(x);
                            });
                            var yhLianXiModel = _cheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == cheliangModel.Id).FirstOrDefault();
                            if (yhLianXiModel != null)
                            {
                                Mapper.CreateMap<CheLiangYeHuLianXiXinXi, FuWuShangCheLiangYeHuLianXiXinXi>();
                                FuWuShangCheLiangYeHuLianXiXinXi addFuWuShangYeHuLianXiModel = Mapper.Map<FuWuShangCheLiangYeHuLianXiXinXi>(yhLianXiModel);
                                SetCreateSYSInfo(addFuWuShangYeHuLianXiModel, userInfo);
                                addFuWuShangYeHuLianXiModel.Id = Guid.NewGuid();
                                addFuWuShangYeHuLianXiModel.CheLiangId = addCarModel.Id;
                                _fuWuShangCheLiangYeHuLianXiXinXiRepository.Add(addFuWuShangYeHuLianXiModel);
                            }
                            var cheliangBaoXianModel = _cheLiangBaoXianXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == cheliangModel.Id).FirstOrDefault();
                            if (cheliangBaoXianModel != null)
                            {
                                Mapper.CreateMap<CheLiangBaoXianXinXi, FuWuShangCheLiangBaoXianXinXi>();
                                FuWuShangCheLiangBaoXianXinXi addFuWuShangBaoXianModel = Mapper.Map<FuWuShangCheLiangBaoXianXinXi>(cheliangBaoXianModel);
                                SetCreateSYSInfo(addFuWuShangBaoXianModel, userInfo);
                                addFuWuShangBaoXianModel.Id = Guid.NewGuid();
                                addFuWuShangBaoXianModel.CheLiangId = addCarModel.Id;
                                _fuWuShangCheLiangBaoXianXinXiRepository.Add(addFuWuShangBaoXianModel);
                            }


                            //同步终端附件信息           
                            if (videoZhongDuanXinXiModel != null)
                            {
                                //同步档案的终端附件到服务商车辆档案
                                var dangAnFileList = _zhongDuanFileMapperRepository.GetQuery(x => x.ZhongDuanId == videoZhongDuanXinXiModel.Id && x.SYS_XiTongZhuangTai == 0).ToList();
                                Mapper.CreateMap<ZhongDuanFileMapper, FuWuShangZhongDuanFileMapper>();
                                dangAnFileList.ForEach(x =>
                                {
                                    FuWuShangZhongDuanFileMapper addZhongDuanFileMapperModel = Mapper.Map<FuWuShangZhongDuanFileMapper>(x);
                                    SetCreateSYSInfo(addZhongDuanFileMapperModel, userInfo);
                                    addZhongDuanFileMapperModel.Id = Guid.NewGuid();
                                    addZhongDuanFileMapperModel.FuWuShangZhongDuanId = addVideoZhongDuanXinXiModel.Id;
                                    addZhongDuanFileMapperModel.SYS_XiTongBeiZhu = "车辆档案审核同步附件";
                                    _fuWuShangZhongDuanFileMapperRepository.Add(addZhongDuanFileMapperModel);
                                });
                            }

                            //同步车辆终端数据通讯配置信息
                            var fuWuShangPeiZhiList = _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangID == fuWuShangCarModel.Id).ToList();
                            fuWuShangPeiZhiList.ForEach(x =>
                            {
                                SetDeleteSYSInfo(x, userInfo);
                                x.SYS_XiTongBeiZhu = "车辆档案审核，旧数据删除";
                                _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Update(x);
                            });
                            var peiZhiModel = _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangID == cheliangModel.Id).FirstOrDefault();

                            if(peiZhiModel!=null)
                            {
                                Mapper.CreateMap<CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi, FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>();
                                var addFuWuShangPeiZhiModel = Mapper.Map<FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>(peiZhiModel);
                                SetCreateSYSInfo(addFuWuShangPeiZhiModel, userInfo);
                                addFuWuShangPeiZhiModel.Id = Guid.NewGuid();
                                addFuWuShangPeiZhiModel.CheLiangID = addCarModel.Id;
                                addFuWuShangPeiZhiModel.ZhongDuanID = gpsZhongDuanXinXiModel.Id;
                                _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Add(addFuWuShangPeiZhiModel);
                            }



                            uow.CommitTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆档案备案提交同步到服务商车辆档案出错" + ex.Message + "请求参数" + JsonConvert.SerializeObject(dto), ex);
            }
        }

        class UpdateBeiAnZhuangTaiToFuWuShangDto
        {
            public string cheliangID { get; set; }
            public int? beiAnZhuangTai { get; set; }
        }
        public override void Dispose()
        {

        }
    }
}
