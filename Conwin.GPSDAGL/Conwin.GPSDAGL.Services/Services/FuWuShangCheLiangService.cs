using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.Framework.Redis;
using Conwin.Framework.ServiceAgent.Utilities;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.DtosExt.FuWuShangCheLiang;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services
{
    public class FuWuShangCheLiangService : ApiServiceBase, IFuWuShangCheLiangService
    {
        private static readonly string sysId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString();

        private readonly ICheLiangRepository _cheLiangRepository;
        private readonly IFuWuShangCheLiangRepository _fwsCheLiangRepository;
        private readonly IFuWuShangCheLiangGPSZhongDuanXinXiRepository _fuWuShangCheLiangGPSZhongDuanXinXiRepository;
        private readonly IFuWuShangCheLiangVideoZhongDuanXinXiRepository _fuWuShangCheLiangVideoZhongDuanXinXiRepository;
        private readonly IFuWuShangZhongDuanFileMapperRepository _fwsZhongDuanFileMapperRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        private readonly IZhongDuanFileMapperRepository _zhongDuanFileMapperRepository;
        private readonly IVideoZhongDuanService _videoZhongDuanService;
        private readonly IFuWuShangCheLiangBaoXianXinXiRepository _fuWuShangCheLiangBaoXianXinXiRepository;
        private readonly IFuWuShangCheLiangYeHuLianXiXinXiRepository _fuWuShangCheLiangYeHuLianXiXinXiRepository;
        private readonly ICheLiangBaoXianXinXiRepository _cheLiangBaoXianXinXiRepository;
        private readonly ICheLiangYeHuLianXiXinXiRepository _cheLiangYeHuLianXiXinXiRepository;
        private readonly IFuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
        private readonly ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
        private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;

        public FuWuShangCheLiangService(ICheLiangRepository cheLiangRepository,
                IFuWuShangCheLiangRepository fwsCheLiangRepository,
                IOrgBaseInfoRepository orgBaseInfoRepository,
                ICheLiangYeHuRepository cheLiangYeHuRepository,
                IFuWuShangCheLiangGPSZhongDuanXinXiRepository fuWuShangCheLiangGPSZhongDuanXinXiRepository,
                IFuWuShangCheLiangVideoZhongDuanXinXiRepository fuWuShangCheLiangVideoZhongDuanXinXiRepository,
                IFuWuShangZhongDuanFileMapperRepository fwsZhongDuanFileMapperRepository,
                ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository,
                ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
                ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
                IZhongDuanFileMapperRepository zhongDuanFileMapperRepository,
                IVideoZhongDuanService videoZhongDuanService,
                IFuWuShangCheLiangBaoXianXinXiRepository fuWuShangCheLiangBaoXianXinXiRepository,
                IFuWuShangCheLiangYeHuLianXiXinXiRepository fuWuShangCheLiangYeHuLianXiXinXiRepository,
                ICheLiangBaoXianXinXiRepository cheLiangBaoXianXinXiRepository,
                ICheLiangYeHuLianXiXinXiRepository cheLiangYeHuLianXiXinXiRepository,
                IBussinessLogger _bussinessLogger
, IFuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository, ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository, ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository) : base(_bussinessLogger)
        {
            _cheLiangRepository = cheLiangRepository;
            _fwsCheLiangRepository = fwsCheLiangRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _fuWuShangCheLiangGPSZhongDuanXinXiRepository = fuWuShangCheLiangGPSZhongDuanXinXiRepository;
            _fuWuShangCheLiangVideoZhongDuanXinXiRepository = fuWuShangCheLiangVideoZhongDuanXinXiRepository;
            _fwsZhongDuanFileMapperRepository = fwsZhongDuanFileMapperRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
            _zhongDuanFileMapperRepository = zhongDuanFileMapperRepository;
            _videoZhongDuanService = videoZhongDuanService;
            _fuWuShangCheLiangBaoXianXinXiRepository = fuWuShangCheLiangBaoXianXinXiRepository;
            _fuWuShangCheLiangYeHuLianXiXinXiRepository = fuWuShangCheLiangYeHuLianXiXinXiRepository;
            _cheLiangBaoXianXinXiRepository = cheLiangBaoXianXinXiRepository;
            _cheLiangYeHuLianXiXinXiRepository = cheLiangYeHuLianXiXinXiRepository;
            _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository = fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository = cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
            _cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;
        }

        public override void Dispose() { }

        /// <summary>
        /// 自动更新车辆备案状态
        /// </summary>
        /// <param name="cheliangId">车辆Id</param>
        /// <param name="chePaiHao">车牌号</param>
        /// <param name="chePaiYanSe">车牌颜色</param>
        /// <param name="videoZhongDuanId">智能视频终端Id</param>
        /// <returns></returns>
        private ServiceResult<bool> AutoUpdateCheLiangBeiAn(Guid cheliangId, string chePaiHao, string chePaiYanSe, Guid videoZhongDuanId)
        {
            string methodName = $"自动更新车辆备案状态（{nameof(FuWuShangCheLiangService)}.{nameof(AutoUpdateCheLiangBeiAn)}）";
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                if (cheliangId == Guid.Empty || string.IsNullOrWhiteSpace(chePaiHao) || string.IsNullOrWhiteSpace(chePaiYanSe) || videoZhongDuanId == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = "车辆参数不能为空";
                    return result;
                }

                //DMS、ADAS报警事件代码
                List<string> baojingEventTypeCodes = new List<string>();
                //ADAS 报警事件代码
                baojingEventTypeCodes.AddRange(new List<string>() { "ZHBJSJ017", "ZHBJSJ018", "ZHBJSJ019", "ZHBJSJ020", "ZHBJSJ021", "ZHBJSJ022", "ZHBJSJ023" });
                //DMS 报警事件代码
                baojingEventTypeCodes.AddRange(new List<string>() { "ZHBJSJ000", "ZHBJSJ001", "ZHBJSJ002", "ZHBJSJ003", "ZHBJSJ004", "ZHBJSJ005", "ZHBJSJ006", "ZHBJSJ007", "ZHBJSJ008", "ZHBJSJ009", "ZHBJSJ010", "ZHBJSJ011", "ZHBJSJ012", "ZHBJSJ013", "ZHBJSJ014", "ZHBJSJ015", "ZHBJSJ016", "ZHBJSJ024" });

                var queryBody = new CheLiangBaoJingQueryDto()
                {
                    ChePaiHao = chePaiHao,
                    ChePaiYanSe = chePaiYanSe,
                    EventTypeCode = string.Join(",", baojingEventTypeCodes)
                };
                CWRequest queryRequest = CWHelper.GenerateRequest(sysId, "006600300024", "1.0", queryBody);
                string responseString = ServiceAgentUtility.Send(queryRequest);
                if (!string.IsNullOrWhiteSpace(responseString))
                {
                    var response = ServiceAgentUtility.DeserializeResponse(JsonConvert.DeserializeObject(responseString).ToString());
                    if (response != null && response.publicresponse != null
                        && response.publicresponse.statuscode == 0
                        && response.body != null)
                    {
                        //车辆已产生DMS或ADAS任意一种报警事件则认为审核通过
                        List<CheLiangBaoJingResDto> eventList = JsonConvert.DeserializeObject<List<CheLiangBaoJingResDto>>(Convert.ToString(response.body));
                        bool isExistsBaoJing = false;

                        if (eventList != null && eventList.Count > 0)
                        {
                            var cheliangEvents = eventList.Select(x => x.EventTypeCode).Distinct().ToList();
                            isExistsBaoJing = cheliangEvents != null && cheliangEvents.Exists(x => baojingEventTypeCodes.Contains(x));
                        }

                        int beianZhuangTai = isExistsBaoJing ? (int)ZhongDuanBeiAnZhuangTai.通过备案 : (int)ZhongDuanBeiAnZhuangTai.不通过备案;
                        string neianNeiRong = isExistsBaoJing ? "备案车辆上传智能视频终端数据正常，车辆备案通过（系统自动判定）" : "备案车辆未上传智能视频终端数据，车辆备案不通过（系统自动判定）";

                        var passResult = _videoZhongDuanService.Confirm(new Dtos.CheLiangVideoZhongDuanConfirmDto
                        {
                            CheLiangId = cheliangId.ToString(),
                            ZhongDuanId = videoZhongDuanId.ToString(),
                            NeiRong = neianNeiRong,
                            BeiAnZhuangTai = beianZhuangTai
                        }, null);

                        return passResult;
                    }
                    else
                    {
                        LogHelper.Debug($"[{methodName}]获取车辆{chePaiHao}({chePaiYanSe})报警事件失败：{response?.publicresponse?.message}");

                        result.StatusCode = 2;
                        result.ErrorMessage = "获取车辆报警事件失败";
                        return result;
                    }
                }
                else
                {
                    LogHelper.Error($"[{methodName}]获取车辆({chePaiHao}({chePaiYanSe}))报警事件失败(返回为空)");

                    result.StatusCode = 2;
                    result.ErrorMessage = "获取车辆报警事件失败(返回为空)";
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[{methodName}]异常。{JsonConvert.SerializeObject(new { cheliangId, chePaiHao, chePaiYanSe, videoZhongDuanId })}", ex);

                result.StatusCode = 2;
                result.ErrorMessage = "自动更新车辆备案状态失败";
                return result;
            }
        }


        /// <summary>
        /// 备案信息提交审核
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> ZhongDuanXinXiShengHe(FuWuShangSubmitReviewDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "提交备案信息不能为空", StatusCode = 2 };
                }

                var UserInfo = GetUserInfo();
                string dangAnCheliangIdStr = string.Empty;
                if (UserInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败", Data = false };
                }
                if (UserInfo.OrganizationType != 6 && UserInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "服务商用户才能提交审核", StatusCode = 2 };
                }
                if (!dto.CheLiangId.HasValue)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
                }
                //从服务商车辆表中查询车辆信息
                var cheliangIdStr = dto.CheLiangId.ToString();
                var carModel = _fwsCheLiangRepository.GetQuery(x => x.Id == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (carModel == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆信息失败", Data = false };
                }

                //车辆备案自动审核逻辑：
                //如果车辆的辖区在配置的范围内，则提交备案审核后，系统会去查询该车辆是否存在指定报警事件，
                //若存在DMS或ADAS报警事件（不限时间），则自动审核通过，否则审核不通过。
                //需要自动审核的车辆10分钟内只能提交一次备案审核。
                //新需求：永徽 2021-04-30

                //是否自动审核
                bool isAutoShenHe = false;
                string autoShenHeXiaQuShi = ConfigurationManager.AppSettings["FuWuShangCheLiangBeiAnAutoShenHeXiaQuShi"].ToString();
                if (!string.IsNullOrWhiteSpace(autoShenHeXiaQuShi))
                {
                    var xiaquShi = autoShenHeXiaQuShi.Trim().Split(',');
                    isAutoShenHe = xiaquShi.Contains(carModel.XiaQuShi);
                }

                //若自动审核，则10分钟内只能提交1次（避免短时间内重复提交）
                if (isAutoShenHe)
                {
                    var key = $"{ConfigurationManager.AppSettings["APPCODE"].ToString()}-CLBASH:{carModel.ChePaiHao}{carModel.ChePaiYanSe}";
                    if (RedisManager.HasKey(key))
                    {
                        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "10分钟内已经提交过审核，请稍后再试", Data = false };
                    }
                    else
                    {
                        RedisManager.Set(key, new { carModel.ChePaiHao, carModel.ChePaiYanSe }, TimeSpan.FromMinutes(10));
                    }
                }


                //查询服务商车辆对应的终端信息
                var gpsZhongDuanModel = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == cheliangIdStr && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (gpsZhongDuanModel == null)
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "请保存终端信息", StatusCode = 2 };
                }

                var videoZhongDuanModel = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == cheliangIdStr && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (videoZhongDuanModel == null)
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "请保存终端信息", StatusCode = 2 };
                }
                var peizhiModel = _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(pz => pz.CheLiangID == dto.CheLiangId && pz.SYS_XiTongZhuangTai == 0).FirstOrDefault();



                //检查车辆业户联系信息
                var fuWuShangCheLiangYeHuLianXiInfo = _fuWuShangCheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == carModel.Id).FirstOrDefault();
                //清远平台未上线业户联系信息模块，暂不验证
                //if (fuWuShangCheLiangYeHuLianXiInfo == null && carModel.XiaQuShi != "清远")
                //{
                //    return new ServiceResult<bool> { Data = false, ErrorMessage = "请保存车辆业户联系信息", StatusCode = 2 };
                //}
                //检车当前服务商车辆备案状态
                if (carModel.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                {
                    return new ServiceResult<bool> { ErrorMessage = "“待审核”或“通过备案”的记录不能操作提交备案" };
                }
                if (carModel.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "“待审核”或“通过备案”的记录不能操作提交备案" };
                }
                //确认当前车辆档案中该车辆的状态
                var carDangAnModel = _cheLiangRepository.GetQuery(x => x.ChePaiHao == carModel.ChePaiHao && x.ChePaiYanSe == carModel.ChePaiYanSe && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (carDangAnModel != null)
                {
                    dangAnCheliangIdStr = carDangAnModel.Id.ToString();
                    var zhongduanConfirmModel = _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.CheLiangId == dangAnCheliangIdStr && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                    if (zhongduanConfirmModel != null)
                    {
                        if (zhongduanConfirmModel.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆已在其他服务商通过备案，不能再次提交" };
                        }
                        if (zhongduanConfirmModel.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该车辆已由其他运营商提交审核,不能再次提交" };
                        }
                        if (zhongduanConfirmModel.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.不通过备案 && carDangAnModel.FuWuShangOrgCode != carModel.FuWuShangOrgCode)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该车辆已由其他运营商提交审核,不能再次提交" };
                        }
                    }
                }
                //验证GPS终端信息
                var gpsInforepeatData = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && (x.SIMKaHao == gpsZhongDuanModel.SIMKaHao || x.ZhongDuanMDT == gpsZhongDuanModel.ZhongDuanMDT)).ToList();
                if (!string.IsNullOrWhiteSpace(gpsZhongDuanModel.ZhongDuanMDT))
                {
                    if (gpsInforepeatData.Where(x => x.ZhongDuanMDT == gpsZhongDuanModel.ZhongDuanMDT && new Guid(x.CheLiangId) != carDangAnModel?.Id).Count() > 0)
                    {
                        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该终端号已存在，请重新核实修正！" };
                    }
                }
                if (!string.IsNullOrWhiteSpace(gpsZhongDuanModel.SIMKaHao))
                {
                    if (gpsInforepeatData.Where(x => x.SIMKaHao == gpsZhongDuanModel.SIMKaHao && new Guid(x.CheLiangId) != carDangAnModel?.Id).Count() > 0)
                    {
                        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该SIM卡号已存在，请重新核实修正！" };
                    }
                }
                //验证智能视频终端信息
                //找出重复的记录
                var videorepeatData = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.ZhongDuanMDT == videoZhongDuanModel.ZhongDuanMDT).ToList();
                if (!string.IsNullOrWhiteSpace(videoZhongDuanModel.ZhongDuanMDT))
                {
                    if (videorepeatData.Where(x => x.ZhongDuanMDT == videoZhongDuanModel.ZhongDuanMDT && new Guid(x.CheLiangId) != carDangAnModel?.Id).Count() > 0)
                    {
                        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端MDT已存在，请重新核实修正！" };
                    }
                }


                Guid cheliangId, videoZhongDuanId;
                string chePaiHao, chePaiYanSe;
                bool isSuccess = false;
                Guid? oldVideoZhongDuanId = null;
                using (var u = new UnitOfWork())
                {
                    u.BeginTransaction();
                    //同步车辆信息与终端信息到车辆档案中
                    if (carDangAnModel != null)
                    {

                        dangAnCheliangIdStr = carDangAnModel.Id.ToString();
                        //删除档案中存在的同车牌车牌颜色的车
                        var dangAnCarList = _cheLiangRepository.GetQuery(x => x.Id == carDangAnModel.Id && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                        SetDeleteSYSInfo(dangAnCarList, UserInfo);
                        dangAnCarList.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                        dangAnCarList.ZuiJinXiuGaiRenOrgCode = UserInfo.OrganizationCode;
                        _cheLiangRepository.Update(dangAnCarList);

                        //该车辆当前在档案中的智能视频终端信息
                        var videoZhongDuanXinXiModel = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.CheLiangId == dangAnCheliangIdStr && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                        if (videoZhongDuanModel != null)
                        {
                            oldVideoZhongDuanId = videoZhongDuanModel.Id;
                        }
                        //删除档案中车辆对应的GPS终端信息
                        var dangAnGpsInfoList = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.CheLiangId == dangAnCheliangIdStr && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnGpsInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangGPSZhongDuanXinXiRepository.Update(x);
                        });
                        //删除档案中车辆对应的智能视频终端信息
                        var dangAnVideoInfoList = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.CheLiangId == dangAnCheliangIdStr && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnVideoInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangVideoZhongDuanXinXiRepository.Update(x);
                        });

                        //删除档案中车辆对应的智能视频终端核查信息
                        var dangAnCarConfirmInfoList = _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.CheLiangId == dangAnCheliangIdStr && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnCarConfirmInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangVideoZhongDuanConfirmRepository.Update(x);
                        });
                        //删除档案中对应的车辆保险信息
                        var cheliangBaoXianInfoList = _cheLiangBaoXianXinXiRepository.GetQuery(x => x.CheLiangId == carDangAnModel.Id && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnCarConfirmInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangVideoZhongDuanConfirmRepository.Update(x);
                        });
                        //删除档案中对应的车辆业户联系信息
                        var cheLiangYeHuLianXiInfoList = _cheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.CheLiangId == carDangAnModel.Id && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnCarConfirmInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangVideoZhongDuanConfirmRepository.Update(x);
                        });

                        //删除档案中对应车辆的终端数据通讯配置
                        var cheLiangPeiZhiList = _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(pz => pz.CheLiangID == carDangAnModel.Id && pz.SYS_XiTongZhuangTai == 0).ToList();
                        cheLiangPeiZhiList.ForEach(pz =>
                        {
                            SetDeleteSYSInfo(pz, UserInfo);
                            pz.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Update(pz);
                        });


                    }
                    //新增服务商车辆到车辆档案
                    Mapper.CreateMap<FuWuShangCheLiang, CheLiang>();
                    CheLiang addCarModel = Mapper.Map<CheLiang>(carModel);
                    addCarModel.Id = Guid.NewGuid();
                    addCarModel.YunZhengZhuangTai = "营运";
                    SetCreateSYSInfo(addCarModel, UserInfo);
                    _cheLiangRepository.Add(addCarModel);
                    //新增GPS终端配置信息
                    Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanXinXi, CheLiangGPSZhongDuanXinXi>();
                    CheLiangGPSZhongDuanXinXi addGpsZhongDuanXinXiModel = Mapper.Map<CheLiangGPSZhongDuanXinXi>(gpsZhongDuanModel);
                    addGpsZhongDuanXinXiModel.Id = Guid.NewGuid();
                    addGpsZhongDuanXinXiModel.CheLiangId = addCarModel.Id.ToString();
                    SetCreateSYSInfo(addGpsZhongDuanXinXiModel, UserInfo);
                    _cheLiangGPSZhongDuanXinXiRepository.Add(addGpsZhongDuanXinXiModel);
                    //新增智能视频终端配置信息
                    Mapper.CreateMap<FuWuShangCheLiangVideoZhongDuanXinXi, CheLiangVideoZhongDuanXinXi>();
                    CheLiangVideoZhongDuanXinXi addVideoZhongDuanXinXiModel = Mapper.Map<CheLiangVideoZhongDuanXinXi>(videoZhongDuanModel);
                    addVideoZhongDuanXinXiModel.Id = Guid.NewGuid();
                    addVideoZhongDuanXinXiModel.CheLiangId = addCarModel.Id.ToString();
                    SetCreateSYSInfo(addVideoZhongDuanXinXiModel, UserInfo);
                    _cheLiangVideoZhongDuanXinXiRepository.Add(addVideoZhongDuanXinXiModel);
                    //新增智能视频终端核查信息
                    var confirmModel = new CheLiangVideoZhongDuanConfirm
                    {
                        Id = Guid.NewGuid(),
                        CheLiangId = addCarModel.Id.ToString(),
                        ZhongDuanId = addVideoZhongDuanXinXiModel.Id.ToString(),
                        BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.未审核,
                        TiJiaoBeiAnShiJian = DateTime.Now
                    };
                    SetCreateSYSInfo(confirmModel, UserInfo);
                    _cheLiangVideoZhongDuanConfirmRepository.Add(confirmModel);





                    //新增车辆保险信息
                    var fuWuShangBaoXianInfo = _fuWuShangCheLiangBaoXianXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == carModel.Id).FirstOrDefault();
                    if (fuWuShangBaoXianInfo != null)
                    {
                        Mapper.CreateMap<FuWuShangCheLiangBaoXianXinXi, CheLiangBaoXianXinXi>();
                        CheLiangBaoXianXinXi addCheLiangBaoXianXinXiModel = Mapper.Map<CheLiangBaoXianXinXi>(fuWuShangBaoXianInfo);
                        addCheLiangBaoXianXinXiModel.Id = Guid.NewGuid();
                        addCheLiangBaoXianXinXiModel.CheLiangId = addCarModel.Id;
                        SetCreateSYSInfo(addCheLiangBaoXianXinXiModel, UserInfo);
                        _cheLiangBaoXianXinXiRepository.Add(addCheLiangBaoXianXinXiModel);
                    }
                    //新增车辆业户联系信息
                    if (fuWuShangCheLiangYeHuLianXiInfo != null)
                    {
                        Mapper.CreateMap<FuWuShangCheLiangYeHuLianXiXinXi, CheLiangYeHuLianXiXinXi>();
                        CheLiangYeHuLianXiXinXi addCheLiangYeHuLianXiModel = Mapper.Map<CheLiangYeHuLianXiXinXi>(fuWuShangCheLiangYeHuLianXiInfo);
                        addCheLiangYeHuLianXiModel.Id = Guid.NewGuid();
                        addCheLiangYeHuLianXiModel.CheLiangId = addCarModel.Id;
                        SetCreateSYSInfo(addCheLiangYeHuLianXiModel, UserInfo);
                        _cheLiangYeHuLianXiXinXiRepository.Add(addCheLiangYeHuLianXiModel);
                    }

                    //同步附件图片
                    var fuWuShangFileList = _fwsZhongDuanFileMapperRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.FuWuShangZhongDuanId == videoZhongDuanModel.Id).ToList();
                    Mapper.CreateMap<FuWuShangZhongDuanFileMapper, ZhongDuanFileMapper>();
                    fuWuShangFileList.ForEach(x =>
                    {
                        ZhongDuanFileMapper addZhongDuanFileMapperModel = Mapper.Map<ZhongDuanFileMapper>(x);
                        SetCreateSYSInfo(addZhongDuanFileMapperModel, UserInfo);
                        addZhongDuanFileMapperModel.Id = Guid.NewGuid();
                        addZhongDuanFileMapperModel.ZhongDuanId = addVideoZhongDuanXinXiModel.Id;
                        addZhongDuanFileMapperModel.SYS_XiTongBeiZhu = "终端审核提交同步附件";
                        _zhongDuanFileMapperRepository.Add(addZhongDuanFileMapperModel);
                    });

                    //删除旧的图片信息
                    if (oldVideoZhongDuanId != null)
                    {
                        var dangAnzhongDuanFileList = _zhongDuanFileMapperRepository.GetQuery(x => x.ZhongDuanId == oldVideoZhongDuanId && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnzhongDuanFileList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, UserInfo);
                            x.SYS_XiTongBeiZhu = "服务商提交车辆审核，旧数据删除";
                            _zhongDuanFileMapperRepository.Update(x);
                        });
                    }

                    if (peizhiModel != null)
                    {
                        //新增终端配置信息
                        Mapper.CreateMap<FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi, CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>();
                        var addPeiZhiModel = Mapper.Map<CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>(peizhiModel);
                        SetCreateSYSInfo(addPeiZhiModel, UserInfo);
                        addPeiZhiModel.CheLiangID = addCarModel.Id;
                        addPeiZhiModel.ZhongDuanID = addGpsZhongDuanXinXiModel?.Id;
                        _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Add(addPeiZhiModel);
                    }

                    //修改审核状态
                    SetUpdateSYSInfo(carModel, carModel, UserInfo);
                    carModel.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.未审核;
                    carModel.ZuiJinXiuGaiRenOrgCode = UserInfo.OrganizationCode;
                    _fwsCheLiangRepository.Update(carModel);

                    isSuccess = u.CommitTransaction() > 0;

                    cheliangId = addCarModel.Id;
                    chePaiHao = addCarModel.ChePaiHao;
                    chePaiYanSe = addCarModel.ChePaiYanSe;
                    videoZhongDuanId = addVideoZhongDuanXinXiModel.Id;
                }
                if (isSuccess)
                {
                    //提交备案审核后，系统自动进行审核通过
                    if (isAutoShenHe)
                    {
                        Task.Run(() =>
                        {
                            AutoUpdateCheLiangBeiAn(cheliangId, chePaiHao, chePaiYanSe, videoZhongDuanId);
                        });
                    }

                    return new ServiceResult<bool> { Data = true };
                }
                else
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "提交审核失败", StatusCode = 2 };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("提交终端信息审核出错" + ex.Message, ex);
                return new ServiceResult<bool> { Data = false, StatusCode = 2, ErrorMessage = "提交出错" };
            }
        }


        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            var result = new ServiceResult<QueryResult>();
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                FuWuShangCheLiangQueryDto dto = JsonConvert.DeserializeObject<FuWuShangCheLiangQueryDto>(queryData.data.ToString());

                Expression<Func<FuWuShangCheLiang, bool>> exp = t => t.SYS_XiTongZhuangTai == 0;
                Expression<Func<CheLiangYeHu, bool>> yhExp = t => t.SYS_XiTongZhuangTai == 0 && t.OrgCode != null && t.OrgCode != "";

                if (userInfo.OrganizationType != (int)OrganizationType.平台运营商)
                {
                    exp = exp.And(p => p.FuWuShangOrgCode == userInfo.OrganizationCode);
                }

                if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    exp = exp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.Trim().ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    exp = exp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe.Trim().ToUpper());
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuSheng))
                {
                    exp = exp.And(p => p.XiaQuSheng == dto.XiaQuSheng);
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuShi))
                {
                    exp = exp.And(p => p.XiaQuShi == dto.XiaQuShi);
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuXian))
                {
                    exp = exp.And(p => p.XiaQuXian == dto.XiaQuXian);
                }
                if (dto.ChuangJianShiJianStart.HasValue)
                {
                    exp = exp.And(p => p.SYS_ChuangJianShiJian >= dto.ChuangJianShiJianStart);
                }
                if (dto.ChuangJianShiJianEnd.HasValue)
                {
                    exp = exp.And(p => p.SYS_ChuangJianShiJian < dto.ChuangJianShiJianEnd);
                }
                if (!string.IsNullOrWhiteSpace(dto.YeHuMingCheng))
                {
                    yhExp = yhExp.And(p => p.OrgName.Contains(dto.YeHuMingCheng.Trim()));
                }
                if (dto.CheLiangZhongLei.HasValue)
                {
                    exp = exp.And(p => p.CheLiangZhongLei == dto.CheLiangZhongLei.Value);
                }
                if (dto.BeiAnZhuangTai.HasValue)
                {
                    exp = exp.And(p => p.BeiAnZhuangTai == dto.BeiAnZhuangTai.Value);
                }

                var queryResult = new QueryResult();

                var list = from u in _fwsCheLiangRepository.GetQuery(exp)
                           join yh in _cheLiangYeHuRepository.GetQuery(yhExp) on u.YeHuOrgCode equals yh.OrgCode
                           join tongxunpeizhi_temp in _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on u.Id equals tongxunpeizhi_temp.CheLiangID into tongxunpeizhi_temp2
                           from tongxunpeizhi in tongxunpeizhi_temp2.DefaultIfEmpty()
                           where !dto.ShuJuTongXunBanBenHao.HasValue || (tongxunpeizhi.BanBenHao == (int?)dto.ShuJuTongXunBanBenHao)
                           group u by new
                           {
                               u.Id,
                               u.ChePaiHao,
                               u.ChePaiYanSe,
                               yh.OrgName,
                               u.XiaQuSheng,
                               u.XiaQuShi,
                               u.XiaQuXian,
                               u.CheLiangZhongLei,
                               u.BeiAnZhuangTai,
                               u.SYS_ChuangJianShiJian,
                               tongxunpeizhi.BanBenHao
                           } into m




                           select new FuWuShangCheLiangQueryResultDto
                           {
                               Id = m.Key.Id.ToString(),
                               ChePaiHao = m.Key.ChePaiHao,
                               ChePaiYanSe = m.Key.ChePaiYanSe,
                               YeHuMingCheng = m.Key.OrgName,
                               XiaQuSheng = m.Key.XiaQuSheng,
                               XiaQuShi = m.Key.XiaQuShi,
                               XiaQuXian = m.Key.XiaQuXian,
                               CheLiangZhongLei = m.Key.CheLiangZhongLei,
                               BeiAnZhuangTai = m.Key.BeiAnZhuangTai,
                               ChuangJianShiJian = m.Key.SYS_ChuangJianShiJian,
                               ShuJuTongXunBanBenHao = m.Key.BanBenHao
                           };

                queryResult.totalcount = list.Count();
                if (queryResult.totalcount > 0)
                {
                    var take = queryData.rows < 1 ? 10 : queryData.rows;
                    var skip = ((queryData.page < 1 ? 1 : queryData.page) - 1) * take;
                    queryResult.items = list.OrderByDescending(s => s.ChuangJianShiJian).Skip(skip).Take(take);
                }
                result.Data = queryResult;
                return result;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
                result.StatusCode = 2;
                result.ErrorMessage = "获取服务商车辆列表失败";
                return result;
            }

        }


        public ServiceResult<Guid?> QueryToExcel(QueryData queryData)
        {
            var result = new ServiceResult<Guid?>();
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<Guid?> { StatusCode = 2, ErrorMessage = "当前用户信息不能为空" };
                }

                FuWuShangCheLiangQueryDto dto = JsonConvert.DeserializeObject<FuWuShangCheLiangQueryDto>(queryData.data.ToString());

                Expression<Func<FuWuShangCheLiang, bool>> exp = t => t.SYS_XiTongZhuangTai == 0;
                Expression<Func<CheLiangYeHu, bool>> yhExp = t => t.SYS_XiTongZhuangTai == 0 && t.OrgCode != null && t.OrgCode != "";


                if (userInfo.OrganizationType != (int)OrganizationType.平台运营商)
                {
                    exp = exp.And(p => p.FuWuShangOrgCode == userInfo.OrganizationCode);
                }

                if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    exp = exp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.Trim().ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    exp = exp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe.Trim().ToUpper());
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuSheng))
                {
                    exp = exp.And(p => p.XiaQuSheng == dto.XiaQuSheng);
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuShi))
                {
                    exp = exp.And(p => p.XiaQuShi == dto.XiaQuShi);
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuXian))
                {
                    exp = exp.And(p => p.XiaQuXian == dto.XiaQuXian);
                }
                if (dto.ChuangJianShiJianStart.HasValue)
                {
                    exp = exp.And(p => p.SYS_ChuangJianShiJian >= dto.ChuangJianShiJianStart);
                }
                if (dto.ChuangJianShiJianEnd.HasValue)
                {
                    exp = exp.And(p => p.SYS_ChuangJianShiJian < dto.ChuangJianShiJianEnd);
                }
                if (!string.IsNullOrWhiteSpace(dto.YeHuMingCheng))
                {
                    yhExp = yhExp.And(p => p.OrgName.Contains(dto.YeHuMingCheng.Trim()));
                }
                if (dto.CheLiangZhongLei.HasValue)
                {
                    exp = exp.And(p => p.CheLiangZhongLei == dto.CheLiangZhongLei.Value);
                }
                if (dto.BeiAnZhuangTai.HasValue)
                {
                    exp = exp.And(p => p.BeiAnZhuangTai == dto.BeiAnZhuangTai.Value);
                }


                var queryResult = new QueryResult();

                var list = from u in _fwsCheLiangRepository.GetQuery(exp)
                           join yh in _cheLiangYeHuRepository.GetQuery(yhExp) on u.YeHuOrgCode equals yh.OrgCode
                           join tongxunpeizhi_temp in _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on u.Id equals tongxunpeizhi_temp.CheLiangID into tongxunpeizhi_temp2
                           from tongxunpeizhi in tongxunpeizhi_temp2.DefaultIfEmpty()
                           where !dto.ShuJuTongXunBanBenHao.HasValue || (tongxunpeizhi.BanBenHao == (int?)dto.ShuJuTongXunBanBenHao)
                           group u by new
                           {
                               u.Id,
                               u.ChePaiHao,
                               u.ChePaiYanSe,
                               yh.OrgName,
                               u.XiaQuSheng,
                               u.XiaQuShi,
                               u.XiaQuXian,
                               u.CheLiangZhongLei,
                               u.BeiAnZhuangTai,
                               u.SYS_ChuangJianShiJian,
                               tongxunpeizhi.BanBenHao,
                           } into m
                           select new FuWuShangCheLiangQueryResultDto
                           {
                               Id = m.Key.Id.ToString(),
                               ChePaiHao = m.Key.ChePaiHao,
                               ChePaiYanSe = m.Key.ChePaiYanSe,
                               YeHuMingCheng = m.Key.OrgName,
                               XiaQuSheng = m.Key.XiaQuSheng,
                               XiaQuShi = m.Key.XiaQuShi,
                               XiaQuXian = m.Key.XiaQuXian,
                               CheLiangZhongLei = m.Key.CheLiangZhongLei,
                               BeiAnZhuangTai = m.Key.BeiAnZhuangTai,
                               ChuangJianShiJian = m.Key.SYS_ChuangJianShiJian,
                               ShuJuTongXunBanBenHao = m.Key.BanBenHao,
                           };

                result.Data = CreateExcelAndUpload(list.ToList());
                return result;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
                result.StatusCode = 2;
                result.ErrorMessage = "导出服务商车辆列表失败";
                return result;
            }
        }

        /// <summary>
        /// 车辆列表数据导出
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static Guid? CreateExcelAndUpload(List<FuWuShangCheLiangQueryResultDto> list)
        {
            if (list == null || list.Count < 1)
            {
                return null;
            }
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            //标题
            HSSFRow row = (HSSFRow)sheet.CreateRow(0);
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
            contentStyle.Alignment = HorizontalAlignment.Left;
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
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));

            string title = "服务商车辆列表";
            row.CreateCell(0).SetCellValue(title);
            //附加标题样式
            row.Cells[0].CellStyle = titleStyle;

            row = (HSSFRow)sheet.CreateRow(1);

            string[] cellTitleArry = { "企业名称", "车牌号码", "车牌颜色", "所属区域", "车辆种类", "版本协议", "创建时间", "备案状态" };
            for (int i = 0; i < cellTitleArry.Length; i++)
            {
                row.CreateCell(i).SetCellValue(cellTitleArry[i]);
                //附加表头样式
                row.Cells[i].CellStyle = cellStyle;
            }
            //内容
            for (int j = 0; j < list.Count; j++)
            {
                var item = list[j];
                row = (HSSFRow)sheet.CreateRow(j + 2);
                int index = 0;
                row.CreateCell(index++).SetCellValue(item.YeHuMingCheng);
                row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                row.CreateCell(index++).SetCellValue(item.XiaQuSheng + item.XiaQuShi + item.XiaQuXian);
                row.CreateCell(index++).SetCellValue(Enum.GetName(typeof(CheLiangZhongLei), item.CheLiangZhongLei));
                int banBenHao = 0;
                if (item.ShuJuTongXunBanBenHao.HasValue)
                {
                    banBenHao = item.ShuJuTongXunBanBenHao.Value;
                }
                row.CreateCell(index++).SetCellValue(((ZhongDuanShuJuTongXunBanBenHao)banBenHao).GetDescription());
                row.CreateCell(index++).SetCellValue(item.ChuangJianShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(index++).SetCellValue(Enum.GetName(typeof(ZhongDuanBeiAnZhuangTai), item.BeiAnZhuangTai));

                for (int i = 0; i < index; i++)
                {
                    if (i == 0)
                    {
                        //附加内容样式
                        row.Cells[i].CellStyle = contentStyle_Center;
                    }
                    else
                    {
                        //附加内容样式
                        row.Cells[i].CellStyle = contentStyle;
                    }
                    if (j == 0)
                    {
                        sheet.AutoSizeColumn(i, true);
                    }
                }
            }


            //上传
            string extension = "xls";
            string fileName = "服务商车辆列表";
            FileDTO fileDto = new FileDTO()
            {
                SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                AppName = "服务商车辆列表",
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


        /// <summary>
        /// 添加服务商车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<Guid> Create(FuWuShangCheLiangUpdateDto dto)
        {
            return ExecuteCommandStruct<Guid>(() =>
            {
                var result = new ServiceResult<Guid>();

                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"获取当前登录用户失败";
                    return result;
                }

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                if (string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"车牌号不能为空";
                    return result;
                }
                if (string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"车牌颜色不能为空";
                    return result;
                }
                FuWuShangCheLiang fwsCheLiang = _fwsCheLiangRepository.GetQuery(s => s.SYS_XiTongZhuangTai == sysZhengChang && s.ChePaiHao == dto.ChePaiHao.Trim() && s.ChePaiYanSe == dto.ChePaiYanSe.Trim() && s.FuWuShangOrgCode == dto.FuWuShangOrgCode).FirstOrDefault();

                if (fwsCheLiang != null && fwsCheLiang.Id != Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"该车辆已存在";
                    return result;
                }

                Mapper.CreateMap<FuWuShangCheLiangUpdateDto, FuWuShangCheLiang>();
                var model = Mapper.Map<FuWuShangCheLiang>(dto);
                model.ChePaiHao = dto.ChePaiHao.Trim();
                model.ChePaiYanSe = dto.ChePaiYanSe.Trim();
                model.XiaQuSheng = string.IsNullOrWhiteSpace(model.XiaQuSheng) ? "广东" : model.XiaQuSheng;
                model.Id = Guid.NewGuid();
                model.SYS_XiTongZhuangTai = sysZhengChang;
                model.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交;
                model.ChuangJianRenOrgCode = userInfo.OrganizationCode;
                model.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                SetCreateSYSInfo(model, userInfo);

                bool flag = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _fwsCheLiangRepository.Add(model);
                    flag = uow.CommitTransaction() > 0;
                }
                if (flag)
                {
                    result.StatusCode = 0;
                    result.Data = model.Id;
                    return result;
                }
                else
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"保存失败";
                    return result;
                }
            });
        }

        /// <summary>
        /// 修改服务商车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> Update(FuWuShangCheLiangUpdateDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();

                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"获取当前登录用户失败";
                    return result;
                }

                if (!dto.Id.HasValue || dto.Id.Value == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"服务商车辆ID不能为空";
                    return result;
                }

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                FuWuShangCheLiang fwsCheLiang = _fwsCheLiangRepository.GetQuery(s => s.SYS_XiTongZhuangTai == sysZhengChang && s.Id == dto.Id.Value).FirstOrDefault();

                if (fwsCheLiang == null || fwsCheLiang.Id == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"该车辆不存在";
                    return result;
                }

                ////var cheliangCount = _fwsCheLiangRepository.Count(s => s.SYS_XiTongZhuangTai == sysZhengChang && s.ChePaiHao == dto.ChePaiHao && s.ChePaiYanSe == dto.ChePaiYanSe && s.FuWuShangOrgCode == dto.FuWuShangOrgCode && s.Id != fwsCheLiang.Id);
                ////if (cheliangCount > 0)
                ////{
                ////    result.StatusCode = 2;
                ////    result.ErrorMessage = $"已存在相同车牌号的车辆";
                ////    return result;
                ////}

                ////fwsCheLiang.ChePaiHao = dto.ChePaiHao;
                ////fwsCheLiang.ChePaiYanSe = dto.ChePaiYanSe;
                fwsCheLiang.CheLiangZhongLei = dto.CheLiangZhongLei;
                fwsCheLiang.XiaQuSheng = string.IsNullOrWhiteSpace(dto.XiaQuSheng) ? "广东" : dto.XiaQuSheng;
                fwsCheLiang.XiaQuShi = dto.XiaQuShi;
                fwsCheLiang.XiaQuXian = dto.XiaQuXian;
                fwsCheLiang.CheJiaHao = dto.CheJiaHao;
                fwsCheLiang.Remark = dto.Remark;
                fwsCheLiang.YeHuOrgType = dto.YeHuOrgType;
                fwsCheLiang.YeHuOrgCode = dto.YeHuOrgCode;
                fwsCheLiang.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                SetUpdateSYSInfo(fwsCheLiang, fwsCheLiang, userInfo);

                bool flag = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _fwsCheLiangRepository.Update(fwsCheLiang);
                    flag = uow.CommitTransaction() > 0;
                }
                if (flag)
                {
                    result.StatusCode = 0;
                    result.Data = true;
                    return result;
                }
                else
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"保存失败";
                    return result;
                }
            });
        }


        public ServiceResult<bool> Delete(Guid fwsCheLiangId)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();
                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                var userInfo = GetUserInfo();
                bool isSuccess = false;

                FuWuShangCheLiang model = _fwsCheLiangRepository.GetQuery(s => s.SYS_XiTongZhuangTai == sysZhengChang && s.Id == fwsCheLiangId).FirstOrDefault();

                if (userInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"获取登录信息失败，请重新登录";
                    return result;
                }
                if (userInfo.OrganizationType != (int)OrganizationType.本地服务商)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"只有服务商才能删除车辆";
                    return result;
                }
                if (userInfo.OrganizationCode != model.FuWuShangOrgCode.Trim())
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"只能删除自己添加的车辆";
                    return result;
                }
                if (model == null || model.Id == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"该车辆不存在";
                    return result;
                }
                if (model.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.待提交 && model.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.通过备案)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"只有待提交或备案通过车辆可以进行删除操作";
                    return result;
                }

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    //删除对应的服务商GPS终端信息
                    string dangAnCheliangIdStr = model.Id.ToString();
                    var gpsZhongDuanXinXi = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == dangAnCheliangIdStr).ToList();
                    gpsZhongDuanXinXi.ForEach(x =>
                    {
                        SetDeleteSYSInfo(x, userInfo);
                        x.SYS_XiTongBeiZhu = "手动删除服务商车辆";
                        _fuWuShangCheLiangGPSZhongDuanXinXiRepository.Update(x);

                    });
                    //删除对应的服务商智能视频终端信息
                    var videoZhongDuanXinXi = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.FuWuShangCheLiangId == dangAnCheliangIdStr).ToList();
                    videoZhongDuanXinXi.ForEach(x =>
                    {

                        SetDeleteSYSInfo(x, userInfo);
                        x.SYS_XiTongBeiZhu = "手动删除服务商车辆";
                        _fuWuShangCheLiangVideoZhongDuanXinXiRepository.Update(x);

                        //删除关联的附件信息
                        var videoFileList = _fwsZhongDuanFileMapperRepository.GetQuery(y => y.FuWuShangZhongDuanId == x.Id).ToList();
                        videoFileList.ForEach(y =>
                        {
                            SetDeleteSYSInfo(x, userInfo);
                            x.SYS_XiTongBeiZhu = "手动删除服务商车辆";
                            _fwsZhongDuanFileMapperRepository.Update(y);

                        });

                    });
                    //删除车辆信息
                    SetDeleteSYSInfo(model, userInfo);
                    _fwsCheLiangRepository.Update(model);
                    isSuccess = uow.CommitTransaction() > 0;
                }

                if (isSuccess)
                {
                    result.StatusCode = 0;
                    result.Data = true;
                    return result;
                }
                else
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"删除失败";
                    return result;
                }
            });
        }





        /// <summary>
        /// 获取服务商车辆详情
        /// </summary>
        /// <param name="fwsCheLiangId"></param>
        /// <returns></returns>
        public ServiceResult<FuWuShangCheLiangUpdateDto> Detail(Guid fwsCheLiangId)
        {
            return ExecuteCommandStruct<FuWuShangCheLiangUpdateDto>(() =>
            {
                var result = new ServiceResult<FuWuShangCheLiangUpdateDto>();

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                FuWuShangCheLiang model = _fwsCheLiangRepository.GetQuery(s => s.SYS_XiTongZhuangTai == sysZhengChang && s.Id == fwsCheLiangId).FirstOrDefault();
                if (model == null || model.Id == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"该车辆不存在";
                    return result;
                }
                var latestGpsTime = _cheLiangDingWeiXinXiRepository.GetQuery(q => q.RegistrationNo == model.ChePaiHao && q.RegistrationNoColor == model.ChePaiYanSe && q.SYS_XiTongZhuangTai == 0)
                .OrderByDescending(o => o.SYS_ChuangJianShiJian).FirstOrDefault()?.LatestGpsTime;

                Mapper.CreateMap<FuWuShangCheLiang, FuWuShangCheLiangUpdateDto>();
                var dto = Mapper.Map<FuWuShangCheLiangUpdateDto>(model);
                dto.LatestGpsTime = latestGpsTime;
                //业户
                if (dto.YeHuOrgType.HasValue && !string.IsNullOrWhiteSpace(dto.YeHuOrgCode))
                {
                    var yehu = _orgBaseInfoRepository.GetQuery(o => o.SYS_XiTongZhuangTai == sysZhengChang && o.OrgType == dto.YeHuOrgType && o.OrgCode == dto.YeHuOrgCode).FirstOrDefault();
                    if (yehu != null)
                    {
                        dto.YeHuOrgName = yehu.OrgName;
                    }
                }

                result.StatusCode = 0;
                result.Data = dto;
                return result;
            });
        }

        /// <summary>
        /// 更新服务商车辆终端(新增/修改)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> UpdateZhongDuan(FuWuShangCheLiangZhongDuanUpdateDto dto)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();

                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"获取当前登录用户失败";
                    return result;
                }

                if (dto.FuWuShangCheLiangId == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"服务商车辆ID不能为空";
                    return result;
                }

                bool isSuccess = false;
                bool isCreateGpsInfo = false;
                bool isCreateVideoInfo = false;
                bool isCreatePeiZhiInfo = false;

                int sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                //保存GPS终端信息
                var gpsModel = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId == dto.FuWuShangCheLiangId.ToString()).FirstOrDefault();

                if (dto?.FuWuShangGpsInfo != null)
                {
                    if (gpsModel == null)
                    {
                        gpsModel = new FuWuShangCheLiangGPSZhongDuanXinXi
                        {
                            Id = Guid.NewGuid(),
                            FuWuShangCheLiangId = dto.FuWuShangCheLiangId.ToString(),
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                        };
                        isCreateGpsInfo = true;
                    }
                    //找出重复的记录
                    //var repeatData = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId != dto.FuWuShangCheLiangId.ToString() && (x.SIMKaHao == dto.FuWuShangGpsInfo.SIMKaHao || x.ZhongDuanMDT == dto.FuWuShangGpsInfo.ZhongDuanMDT)).ToList();

                    //if (!string.IsNullOrWhiteSpace(dto.FuWuShangGpsInfo.ZhongDuanMDT))
                    //{
                    //    if (repeatData.Where(x => x.ZhongDuanMDT == dto.FuWuShangGpsInfo.ZhongDuanMDT).Count() > 0)
                    //    {
                    //        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该终端号已存在，请重新核实修正！" };
                    //    }
                    //}
                    //if (!string.IsNullOrWhiteSpace(dto.FuWuShangGpsInfo.SIMKaHao))
                    //{
                    //    if (repeatData.Where(x => x.SIMKaHao == dto.FuWuShangGpsInfo.SIMKaHao).Count() > 0)
                    //    {
                    //        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该SIM卡号已存在，请重新核实修正！" };
                    //    }
                    //}
                    gpsModel.ZhongDuanLeiXing = dto.FuWuShangGpsInfo.ZhongDuanLeiXing;
                    gpsModel.SheBeiXingHao = dto.FuWuShangGpsInfo.SheBeiXingHao;
                    gpsModel.ShengChanChangJia = dto.FuWuShangGpsInfo.ShengChanChangJia;
                    gpsModel.ChangJiaBianHao = dto.FuWuShangGpsInfo.ChangJiaBianHao;
                    gpsModel.ZhongDuanBianMa = dto.FuWuShangGpsInfo.ZhongDuanBianMa;
                    gpsModel.SIMKaHao = dto.FuWuShangGpsInfo.SIMKaHao;
                    gpsModel.ZhongDuanMDT = dto.FuWuShangGpsInfo.ZhongDuanMDT;
                    gpsModel.M1 = dto.FuWuShangGpsInfo.M1;
                    gpsModel.IA1 = dto.FuWuShangGpsInfo.IA1;
                    gpsModel.IC1 = dto.FuWuShangGpsInfo.IC1;
                    gpsModel.ShiFouAnZhuangShiPinZhongDuan = dto.FuWuShangGpsInfo.ShiFouAnZhuangShiPinZhongDuan ?? 0;
                    gpsModel.ShiPinTouAnZhuangXuanZe = dto.FuWuShangGpsInfo.ShiPinTouAnZhuangXuanZe;
                    gpsModel.ShiPingChangShangLeiXing = dto.FuWuShangGpsInfo.ShiPingChangShangLeiXing ?? 0;
                    gpsModel.ShiPinTouGeShu = dto.FuWuShangGpsInfo.ShiPinTouGeShu ?? 0;
                    gpsModel.Remark = dto.FuWuShangGpsInfo.Remark;
                }
                gpsModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                gpsModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                gpsModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;

                //智能视频终端
                var videoModel = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId == dto.FuWuShangCheLiangId.ToString()).FirstOrDefault();

                //图片附件列表
                List<FuWuShangZhongDuanFileMapper> addFileList = new List<FuWuShangZhongDuanFileMapper>();
                List<FuWuShangZhongDuanFileMapper> updateFileList = new List<FuWuShangZhongDuanFileMapper>();

                //保存智能视频终端信息
                if (dto.FuWuShangVideoInfo != null)
                {
                    if (videoModel == null)
                    {
                        videoModel = new FuWuShangCheLiangVideoZhongDuanXinXi
                        {
                            Id = Guid.NewGuid(),
                            FuWuShangCheLiangId = dto.FuWuShangCheLiangId.ToString(),
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常,
                        };
                        isCreateVideoInfo = true;
                    }
                    //找出重复的记录
                    //var repeatData = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId != dto.FuWuShangCheLiangId.ToString() && x.ZhongDuanMDT == dto.FuWuShangVideoInfo.VideoDeviceMDT).ToList();
                    //if (!string.IsNullOrWhiteSpace(dto.FuWuShangVideoInfo.VideoDeviceMDT))
                    //{
                    //    if (repeatData.Where(x => x.ZhongDuanMDT == dto.FuWuShangVideoInfo.VideoDeviceMDT).Count() > 0)
                    //    {
                    //        return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端MDT已存在，请重新核实修正！" };
                    //    }
                    //}
                    videoModel.ZhongDuanMDT = dto.FuWuShangVideoInfo.VideoDeviceMDT;
                    videoModel.SheBeiXingHao = dto.FuWuShangVideoInfo.VideoSheBeiXingHao;
                    videoModel.SheBeiJiShenLeiXing = dto.FuWuShangVideoInfo.VideoSheBeiJiShenLeiXing ?? 0;
                    videoModel.SheBeiGouCheng = dto.FuWuShangVideoInfo.VideoSheBeiGouCheng ?? "";
                    videoModel.ChangJiaBianHao = dto.FuWuShangVideoInfo.VideoChangJiaBianHao;
                    videoModel.ShengChanChangJia = dto.FuWuShangVideoInfo.VideoShengChanChangJia;
                    videoModel.AnZhuangShiJian = dto.FuWuShangVideoInfo.VideoAnZhuangShiJian;
                }
                videoModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                videoModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                videoModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;

                //智能视频附件信息
                var imgList = _fwsZhongDuanFileMapperRepository.GetQuery(x => x.FuWuShangZhongDuanId == videoModel.Id && x.SYS_XiTongZhuangTai == sysZhengChang).ToList();
                var imgtype3 = imgList.Where(x => x.FileType == (int)VideoZhongDuanImgType.显示屏 && x.SYS_XiTongZhuangTai == sysZhengChang).ToList();

                if (!string.IsNullOrWhiteSpace(dto.FuWuShangVideoInfo.XianShiPingZhaoPianId))
                {
                    if (imgtype3.Where(x => x.FileId == dto.FuWuShangVideoInfo.XianShiPingZhaoPianId).Count() <= 0)
                    {
                        addFileList.Add(new FuWuShangZhongDuanFileMapper
                        {
                            Id = Guid.NewGuid(),
                            FuWuShangZhongDuanId = videoModel.Id,
                            FileId = dto.FuWuShangVideoInfo.XianShiPingZhaoPianId,
                            FileType = (int)VideoZhongDuanImgType.显示屏,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_ChuangJianRen = userInfo.UserName,
                            SYS_ChuangJianRenID = userInfo.Id,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                        });
                        foreach (var item in imgtype3)
                        {
                            item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            updateFileList.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in imgtype3)
                    {
                        item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        updateFileList.Add(item);
                    }
                }

                var imgtype1 = imgList.Where(x => x.FileType == (int)VideoZhongDuanImgType.声光报警系统 && x.SYS_XiTongZhuangTai == sysZhengChang).ToList();
                if (!string.IsNullOrWhiteSpace(dto.FuWuShangVideoInfo.ShengGuangBaoJingXiTongZhaoPianId))
                {
                    if (imgtype1.Where(x => x.FileId == dto.FuWuShangVideoInfo.ShengGuangBaoJingXiTongZhaoPianId).Count() <= 0)
                    {
                        addFileList.Add(new FuWuShangZhongDuanFileMapper
                        {
                            Id = Guid.NewGuid(),
                            FuWuShangZhongDuanId = videoModel.Id,
                            FileId = dto.FuWuShangVideoInfo.ShengGuangBaoJingXiTongZhaoPianId,
                            FileType = (int)VideoZhongDuanImgType.声光报警系统,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_ChuangJianRen = userInfo.UserName,
                            SYS_ChuangJianRenID = userInfo.Id,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                        });
                        foreach (var item in imgtype1)
                        {
                            item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            updateFileList.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in imgtype1)
                    {
                        item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        updateFileList.Add(item);
                    }
                }

                var imgtype2 = imgList.Where(x => x.FileType == (int)VideoZhongDuanImgType.主机存储器 && x.SYS_XiTongZhuangTai == sysZhengChang).ToList();
                if (!string.IsNullOrWhiteSpace(dto.FuWuShangVideoInfo.ZhuJiCunChuQiZhaoPianId))
                {
                    if (imgtype2.Where(x => x.FileId == dto.FuWuShangVideoInfo.ZhuJiCunChuQiZhaoPianId).Count() <= 0)
                    {
                        addFileList.Add(new FuWuShangZhongDuanFileMapper
                        {
                            Id = Guid.NewGuid(),
                            FuWuShangZhongDuanId = videoModel.Id,
                            FileId = dto.FuWuShangVideoInfo.ZhuJiCunChuQiZhaoPianId,
                            FileType = (int)VideoZhongDuanImgType.主机存储器,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_ChuangJianRen = userInfo.UserName,
                            SYS_ChuangJianRenID = userInfo.Id,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                        });
                        foreach (var item in imgtype2)
                        {
                            item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            updateFileList.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in imgtype2)
                    {
                        item.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        updateFileList.Add(item);
                    }
                }


                #region 保存终端配置

                var gCheLiangId = dto.FuWuShangCheLiangId;

                //获取终端配置信息
                var peiZhiModel = _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangID == gCheLiangId).FirstOrDefault();

                if (dto.PeiZhiInfo != null)
                {
                    if (peiZhiModel == null)
                    {
                        peiZhiModel = new FuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi
                        {
                            Id = Guid.NewGuid(),
                            CheLiangID = gCheLiangId,
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常,
                            ZhuaBaoLaiYuan = 0,
                            XieYiLeiXing = 0,
                        };
                        isCreatePeiZhiInfo = true;
                    }

                    peiZhiModel = dto.PeiZhiInfo.MapToEntity(peiZhiModel);

                    peiZhiModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                    peiZhiModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                    peiZhiModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;


                }




                #endregion



                using (var u = new UnitOfWork())
                {
                    u.BeginTransaction();
                    if (isCreateGpsInfo)
                    {
                        _fuWuShangCheLiangGPSZhongDuanXinXiRepository.Add(gpsModel);
                    }
                    else
                    {
                        _fuWuShangCheLiangGPSZhongDuanXinXiRepository.Update(gpsModel);
                    }
                    if (isCreateVideoInfo)
                    {
                        _fuWuShangCheLiangVideoZhongDuanXinXiRepository.Add(videoModel);
                    }
                    else
                    {
                        _fuWuShangCheLiangVideoZhongDuanXinXiRepository.Update(videoModel);
                    }
                    foreach (var updateItem in updateFileList)
                    {
                        _fwsZhongDuanFileMapperRepository.Update(updateItem);
                    }
                    _fwsZhongDuanFileMapperRepository.BatchInsert(addFileList.ToArray());

                    if (peiZhiModel != null)
                        if (isCreatePeiZhiInfo)
                        {
                            _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Add(peiZhiModel);
                        }
                        else
                        {
                            _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Update(peiZhiModel);
                        }

                    isSuccess = u.CommitTransaction() > 0;
                }

                if (isSuccess)
                {
                    result.StatusCode = 0;
                    result.Data = true;
                    return result;
                }
                else
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"保存失败";
                    return result;
                }
            });


        }

        /// <summary>
        /// 获取服务商车辆终端详情
        /// </summary>
        /// <param name="fwsCheLiangId"></param>
        /// <returns></returns>
        public ServiceResult<FuWuShangCheLiangZhongDuanUpdateDto> ZhongDuanDetail(Guid fwsCheLiangId)
        {
            return ExecuteCommandStruct<FuWuShangCheLiangZhongDuanUpdateDto>(() =>
            {
                var result = new ServiceResult<FuWuShangCheLiangZhongDuanUpdateDto>();

                if (fwsCheLiangId == Guid.Empty)
                {
                    result.StatusCode = 2;
                    result.ErrorMessage = $"服务商车辆ID不能为空";
                    return result;
                }

                FuWuShangCheLiangZhongDuanUpdateDto dto = new FuWuShangCheLiangZhongDuanUpdateDto()
                {
                    FuWuShangCheLiangId = fwsCheLiangId
                };

                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;

                //GPS终端信息
                var gpsModel = _fuWuShangCheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId == dto.FuWuShangCheLiangId.ToString()).FirstOrDefault();
                if (gpsModel != null)
                {
                    dto.FuWuShangGpsInfo = new FuWuShangCLGpsInfoDto()
                    {
                        ZhongDuanLeiXing = gpsModel.ZhongDuanLeiXing,
                        ShengChanChangJia = gpsModel.ShengChanChangJia,
                        ChangJiaBianHao = gpsModel.ChangJiaBianHao,
                        SheBeiXingHao = gpsModel.SheBeiXingHao,
                        ZhongDuanBianMa = gpsModel.ZhongDuanBianMa,
                        SIMKaHao = gpsModel.SIMKaHao,
                        ZhongDuanMDT = gpsModel.ZhongDuanMDT,
                        IA1 = gpsModel.IA1,
                        IC1 = gpsModel.IC1,
                        M1 = gpsModel.M1,
                        ShiFouAnZhuangShiPinZhongDuan = gpsModel.ShiFouAnZhuangShiPinZhongDuan,
                        ShiPinTouAnZhuangXuanZe = gpsModel.ShiPinTouAnZhuangXuanZe,
                        ShiPingChangShangLeiXing = gpsModel.ShiPingChangShangLeiXing,
                        ShiPinTouGeShu = gpsModel.ShiPinTouGeShu,
                        Remark = gpsModel.Remark
                    };
                }

                //智能视频终端
                var videoModel = _fuWuShangCheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == sysZhengChang && x.FuWuShangCheLiangId == dto.FuWuShangCheLiangId.ToString()).FirstOrDefault();
                if (videoModel != null)
                {
                    dto.FuWuShangVideoInfo = new FuWuShangCLVideoInfoDto()
                    {
                        VideoDeviceMDT = videoModel.ZhongDuanMDT,
                        VideoSheBeiXingHao = videoModel.SheBeiXingHao,
                        VideoSheBeiJiShenLeiXing = videoModel.SheBeiJiShenLeiXing,
                        VideoSheBeiGouCheng = videoModel.SheBeiGouCheng,
                        VideoShengChanChangJia = videoModel.ShengChanChangJia,
                        VideoChangJiaBianHao = videoModel.ChangJiaBianHao,
                        VideoAnZhuangShiJian = videoModel.AnZhuangShiJian,
                    };
                    //智能视频附件图片列表
                    var videoFileList = _fwsZhongDuanFileMapperRepository.GetQuery(x => x.FuWuShangZhongDuanId == videoModel.Id && x.SYS_XiTongZhuangTai == sysZhengChang).Select(x => new FuWuShangVideoZhongDuanImgDto { FileId = x.FileId, FileType = x.FileType }).ToList();
                    dto.FuWuShangVideoInfo.FileList = videoFileList;
                }

                //获取终端配置信息
                var peiZhi = _fuWuShangCheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && (x.CheLiangID.HasValue && x.CheLiangID == fwsCheLiangId)).Select(pz => new FuWuShangZhongDuanShuJuTongXunPeiZhiXinXiDto
                {
                    Id = pz.Id,
                    BanBenHao = pz.BanBenHao,
                    CheLiangID = pz.CheLiangID,
                    XieYiLeiXing = pz.XieYiLeiXing,
                    ZhongDuanID = pz.ZhongDuanID,
                    ZhuaBaoLaiYuan = pz.ZhuaBaoLaiYuan,
                }).FirstOrDefault();
                dto.PeiZhiInfo = peiZhi;


                result.StatusCode = 0;
                result.Data = dto;
                return result;
            });
        }



        #region 更新服务商车辆保险信息

        public ServiceResult<bool> AddOrUpdateFuWuShangCheLiangBaoXianXinXi(UpdateFuWuShangCheLiangBaoXianXinXiDto dto)
        {
            try
            {
                bool isSuccess = false;
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }
                FuWuShangCheLiangBaoXianXinXi baoXianModel = null;
                if (dto.CheLiangId != null && dto.CheLiangId.HasValue)
                {
                    baoXianModel = _fuWuShangCheLiangBaoXianXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();
                }
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (baoXianModel == null)
                    {
                        Mapper.CreateMap<UpdateFuWuShangCheLiangBaoXianXinXiDto, FuWuShangCheLiangBaoXianXinXi>();
                        baoXianModel = Mapper.Map<FuWuShangCheLiangBaoXianXinXi>(dto);
                        SetCreateSYSInfo(baoXianModel, userInfo);
                        baoXianModel.Id = Guid.NewGuid();
                        _fuWuShangCheLiangBaoXianXinXiRepository.Add(baoXianModel);
                    }
                    else
                    {
                        baoXianModel.JiaoQiangXianOrgName = dto.JiaoQiangXianOrgName;
                        baoXianModel.JiaoQiangXianEndTime = dto.JiaoQiangXianEndTime;
                        baoXianModel.ShangYeXianOrgName = dto.ShangYeXianOrgName;
                        baoXianModel.ShangYeXianEndTime = dto.ShangYeXianEndTime;
                        SetUpdateSYSInfo(baoXianModel, baoXianModel, userInfo);
                        _fuWuShangCheLiangBaoXianXinXiRepository.Update(baoXianModel);
                    }
                    isSuccess = uow.CommitTransaction() > 0;
                }
                if (isSuccess)
                {
                    return new ServiceResult<bool> { Data = true };
                }
                else
                {

                    return new ServiceResult<bool> { Data = false, ErrorMessage = "保存失败", StatusCode = 2 };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新服务商车辆保险信息出错" + ex.Message + "请求报文" + JsonConvert.SerializeObject(dto), ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "保存出错", Data = false };
            }
        }
        #endregion

        #region 获取服务商车辆保险信息
        public ServiceResult<FuWuShangCheLiangBaoXianXinXiResponseDto> GetFuWuShangCheLiangBaoXianXinXi(Guid? cheLiangId)
        {
            try
            {
                if (cheLiangId == null || !cheLiangId.HasValue)
                {
                    return new ServiceResult<FuWuShangCheLiangBaoXianXinXiResponseDto> { ErrorMessage = "车辆ID不能为空", StatusCode = 2 };
                }
                var baoXianModel = _fuWuShangCheLiangBaoXianXinXiRepository.GetQuery(x => x.CheLiangId == cheLiangId && x.SYS_XiTongZhuangTai == 0).Select(y => new FuWuShangCheLiangBaoXianXinXiResponseDto
                {
                    JiaoQiangXianOrgName = y.JiaoQiangXianOrgName,
                    JiaoQiangXianEndTime = y.JiaoQiangXianEndTime,
                    ShangYeXianOrgName = y.ShangYeXianOrgName,
                    ShangYeXianEndTime = y.ShangYeXianEndTime
                }).FirstOrDefault();
                return new ServiceResult<FuWuShangCheLiangBaoXianXinXiResponseDto> { Data = baoXianModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取服务商车辆保险信息出错" + ex.Message, ex);
                return new ServiceResult<FuWuShangCheLiangBaoXianXinXiResponseDto> { StatusCode = 2, ErrorMessage = "获取保险信息出错" };
            }


        }

        #endregion


        #region 业户联系信息

        #region 更新业户联系信息

        public ServiceResult<bool> UpdateFuWuShangYeHuLianXiXinXi(UpdateFuWuShangYeHuBaoXianXinXiRequestDto dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                bool isSuccess = false;
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { ErrorMessage = "获取登录信息失败", StatusCode = 2 };
                }
                if (dto?.CheLiangId == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
                }
                var model = _fuWuShangCheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (model == null)
                    {
                        Mapper.CreateMap<UpdateFuWuShangYeHuBaoXianXinXiRequestDto, FuWuShangCheLiangYeHuLianXiXinXi>();
                        FuWuShangCheLiangYeHuLianXiXinXi addModel = Mapper.Map<FuWuShangCheLiangYeHuLianXiXinXi>(dto);
                        SetCreateSYSInfo(addModel, userInfo);
                        addModel.Id = Guid.NewGuid();
                        _fuWuShangCheLiangYeHuLianXiXinXiRepository.Add(addModel);
                    }
                    else
                    {
                        model.YeHuPrincipalName = dto.YeHuPrincipalName;
                        model.YeHuPrincipalPhone = dto.YeHuPrincipalPhone;
                        model.DriverName = dto.DriverName;
                        model.DriverPhone = dto.DriverPhone;
                        model.CongYeZiGeZhengHao = dto.CongYeZiGeZhengHao;
                        model.JiZhongAnZhuangDianMingCheng = dto.JiZhongAnZhuangDianMingCheng;
                        model.SheBeiAnZhuangRenYuanXingMing = dto.SheBeiAnZhuangRenYuanXingMing;
                        model.SheBeiAnZhuangDanWei = dto.SheBeiAnZhuangDanWei;
                        model.SheBeiAnZhuangRenYuanDianHua = dto.SheBeiAnZhuangRenYuanDianHua;
                        SetUpdateSYSInfo(model, model, userInfo);
                        _fuWuShangCheLiangYeHuLianXiXinXiRepository.Update(model);
                    }
                    isSuccess = uow.CommitTransaction() > 0;
                }
                if (isSuccess)
                {
                    return new ServiceResult<bool> { Data = true };
                }
                else
                {
                    return new ServiceResult<bool> { ErrorMessage = "更新失败", StatusCode = 2 };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("更新车辆业户联系信息出错" + ex.Message + "\n请求参数:" + JsonConvert.SerializeObject(dto), ex);
                return new ServiceResult<bool> { ErrorMessage = "更新出错", StatusCode = 2 };
            }


        }
        #endregion

        #region 获取业户联系信息

        public ServiceResult<UpdateFuWuShangYeHuBaoXianXinXiResponseDto> GetFuWuShangYeHuLianXiXinXi(Guid CheLiangId)
        {
            try
            {
                var model = _fuWuShangCheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == CheLiangId).Select(x => new UpdateFuWuShangYeHuBaoXianXinXiResponseDto
                {
                    YeHuPrincipalName = x.YeHuPrincipalName,
                    YeHuPrincipalPhone = x.YeHuPrincipalPhone,
                    DriverName = x.DriverName,
                    DriverPhone = x.DriverPhone,
                    CongYeZiGeZhengHao = x.CongYeZiGeZhengHao,
                    JiZhongAnZhuangDianMingCheng = x.JiZhongAnZhuangDianMingCheng,
                    SheBeiAnZhuangRenYuanXingMing = x.SheBeiAnZhuangRenYuanXingMing,
                    SheBeiAnZhuangDanWei = x.SheBeiAnZhuangDanWei,
                    SheBeiAnZhuangRenYuanDianHua = x.SheBeiAnZhuangRenYuanDianHua
                }).FirstOrDefault();

                return new ServiceResult<UpdateFuWuShangYeHuBaoXianXinXiResponseDto> { Data = model };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取服务商车辆业户联系信息出错" + ex.Message, ex);
                return new ServiceResult<UpdateFuWuShangYeHuBaoXianXinXiResponseDto> { ErrorMessage = "查询出错", StatusCode = 2 };
            }

        }
        #endregion

        #endregion

    }
}
