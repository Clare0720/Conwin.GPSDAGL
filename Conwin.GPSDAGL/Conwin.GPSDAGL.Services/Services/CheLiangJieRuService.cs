using Conwin.EntityFramework.Extensions;
using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangJieRu;
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
using System.Text;
using System.Threading.Tasks;
using UnityConfiguration;

namespace Conwin.GPSDAGL.Services.Services
{
    public class CheLiangJieRuService : ApiServiceBase, ICheLiangJieRuService
    {
        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly ICheLiangBaoXianXinXiRepository _cheLiangBaoXianXinXiRepository;
        private readonly ICheLiangYeHuLianXiXinXiRepository _cheLiangYeHuLianXiXinXiRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        private readonly IFuWuShangRepository _fuWuShangRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;
        private readonly ICheLiangExRepository _cheLiangExRepository;
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        public CheLiangJieRuService(
            ICheLiangRepository cheLiangRepository,
            ICheLiangBaoXianXinXiRepository cheLiangBaoXianXinXiRepository,
            ICheLiangYeHuLianXiXinXiRepository cheLiangYeHuLianXiXinXiRepository,
            ICheLiangYeHuRepository cheLiangYeHuRepository,
            IFuWuShangRepository fuWuShangRepository,
            ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
            ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
            ICheLiangExRepository cheLiangExRepository,
            ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            IBussinessLogger _bussinessLogger
            ) : base(_bussinessLogger)
        {
            _cheLiangXinXiRepository = cheLiangRepository;
            _cheLiangBaoXianXinXiRepository = cheLiangBaoXianXinXiRepository;
            _cheLiangYeHuLianXiXinXiRepository = cheLiangYeHuLianXiXinXiRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _fuWuShangRepository = fuWuShangRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
            _cheLiangExRepository = cheLiangExRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }

        #region 智能报警装置安装接入信息
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> GetZhiNengShiPinJieRuXinXi(QueryData dto)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                GetCheLiangJieRuXinXiDto returnData = GetJieRuDataList(dto, userInfo);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取智能视频接入信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { ErrorMessage = "查询出错", StatusCode = 2 };
            }

        }
        /// <summary>
        /// 导出列表数据
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<ExportZhiNengShiPingJieRuDto> ExportZhiNengShiPingJieRuXinXi(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                var list = GetJieRuDataList(queryData, userInfo)?.list;
                ExportZhiNengShiPingJieRuRequestDto dto = JsonConvert.DeserializeObject<ExportZhiNengShiPingJieRuRequestDto>(JsonConvert.SerializeObject(queryData.data));
                string tableTitle = "智能报警装置安装接入登记表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count() > 0)
                {
                    try
                    {
                        string FileId = string.Empty;
                        Guid? fileUploadId = CreatePingTaiZhiLingQingQiuAndYingDaExcelAndUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            FileId = fileUploadId.ToString();
                        }
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { Data = new ExportZhiNengShiPingJieRuDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出智能报警装置安装接入登记表" + e.Message, e);
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出智能报警装置安装接入登记表" + ex.Message, ex);
                return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private GetCheLiangJieRuXinXiDto GetJieRuDataList(QueryData dto, UserInfoDtoNew userInfo)
        {
            ZhiNengShiPingJieRuRequestDto search = JsonConvert.DeserializeObject<ZhiNengShiPingJieRuRequestDto>(JsonConvert.SerializeObject(dto.data));
            Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangYeHu, bool>> yhExp = x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode != null && x.OrgCode != "";
            Expression<Func<OrgBaseInfo, bool>> fwsExp = x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode != null && x.OrgCode != "" && x.OrgType == (int)OrganizationType.本地服务商;
            Expression<Func<CheLiangGPSZhongDuanXinXi, bool>> gpsExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangBaoXianXinXi, bool>> bxExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangVideoZhongDuanXinXi, bool>> videoExp = x => x.SYS_XiTongZhuangTai == 0;

            List<ZhiNengShiPingJieRuResponseDto> dataList = new List<ZhiNengShiPingJieRuResponseDto>();
            GetCheLiangJieRuXinXiDto returnData = new GetCheLiangJieRuXinXiDto();
            if (search == null)
            {
                return new GetCheLiangJieRuXinXiDto { };
            }
            //辖区市
            if (!string.IsNullOrWhiteSpace(search?.XiaQuShi))
            {
                carExp = carExp.And(x => x.XiaQuShi == search.XiaQuShi.Trim());
            }
            //辖区县
            if (!string.IsNullOrWhiteSpace(search?.XiaQuXian))
            {
                carExp = carExp.And(x => x.XiaQuXian == search.XiaQuXian.Trim());
            }
            //业户名称
            if (!string.IsNullOrWhiteSpace(search?.OrgName))
            {
                yhExp = yhExp.And(x => x.OrgName.Contains(search.OrgName.Trim()));
            }

            switch (userInfo.OrganizationType)
            {
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                    carExp = carExp.And(x => x.YeHuOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.本地服务商:
                    carExp = carExp.And(x => x.FuWuShangOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.市政府:
                    carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity);
                    break;
                case (int)OrganizationType.县政府:
                    carExp = carExp.And(x => x.XiaQuXian == userInfo.OrganDistrict);
                    break;
                default:
                    return new GetCheLiangJieRuXinXiDto { };
            }


            var list = from car in _cheLiangXinXiRepository.GetQuery(carExp)
                       join yh in _cheLiangYeHuRepository.GetQuery(yhExp) on car.YeHuOrgCode equals yh.OrgCode
                       join fws in _orgBaseInfoRepository.GetQuery(fwsExp) on car.FuWuShangOrgCode equals fws.OrgCode into table4
                       from t4 in table4.DefaultIfEmpty()
                       join video in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(videoExp) on car.Id.ToString() equals video.CheLiangId into table1
                       from t1 in table1.DefaultIfEmpty()
                       join bx in _cheLiangBaoXianXinXiRepository.GetQuery(bxExp) on car.Id equals bx.CheLiangId into table2
                       from t2 in table2.DefaultIfEmpty()
                       join yhlx in _cheLiangYeHuLianXiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0)
                       on car.Id equals yhlx.CheLiangId into table3
                       from t3 in table3.DefaultIfEmpty()
                           //安装日期起
                       where !search.AnZhuangRiQiBegin.HasValue || t1.AnZhuangShiJian >= search.AnZhuangRiQiBegin
                       //安装日期止
                       where !search.AnZhuangRiQiEnd.HasValue || t1.AnZhuangShiJian <= search.AnZhuangRiQiEnd
                       //交强险公司名称
                       where string.IsNullOrEmpty(search.JiaoQiangXianOrgName) || t2.JiaoQiangXianOrgName.Contains(search.JiaoQiangXianOrgName.Trim())
                       //商业险保险公司名称
                       where string.IsNullOrEmpty(search.ShangYeXianOrgName) || t2.ShangYeXianOrgName.Contains(search.ShangYeXianOrgName.Trim())
                       //服务商名称
                       where string.IsNullOrEmpty(search.FuWuShangName) || t4.OrgName.Contains(search.FuWuShangName.Trim())

                       select new ZhiNengShiPingJieRuResponseDto
                       {
                           CheLiangId = car.Id,
                           XiaQuShi = car.XiaQuShi,
                           XiaQuXian = car.XiaQuXian,
                           OrgName = yh.OrgName,
                           ChePaiHao = car.ChePaiHao,
                           ChePaiYanSe = car.ChePaiYanSe,
                           CheJiaHao = car.CheJiaHao,
                           YeHuPrincipalName = t3.YeHuPrincipalName,
                           YeHuPrincipalPhone = t3.YeHuPrincipalPhone,
                           DriverName = t3.DriverName,
                           DriverPhone = t3.DriverPhone,
                           CongYeZiGeZhengHao = t3.CongYeZiGeZhengHao,
                           FuWuShangOrgName = t4.OrgName,
                           SheBeiAnZhuangRenYuanName = t3.SheBeiAnZhuangRenYuanXingMing,
                           SheBeiAnZhuangRenYuanPhone = t3.SheBeiAnZhuangRenYuanDianHua,
                           JiZhongAnZhuangDianMingCheng = t3.JiZhongAnZhuangDianMingCheng,
                           JiaoQiangXianOrgName = t2.JiaoQiangXianOrgName,
                           ShangYeXianOrgName = t2.ShangYeXianOrgName,
                           AnZhuangShiJian = t1.AnZhuangShiJian,
                           VideoSheBeiXingHao = t1.SheBeiXingHao,
                           VideoShengChanChangJia = t1.ShengChanChangJia,
                           SheHuiXinYongDaiMa = yh.SheHuiXinYongDaiMa,
                           GeTiHuIDCard = yh.GeTiHuShenFenZhengHaoMa
                       };
            returnData.Count = list.Count();
            if (returnData.Count > 0)
            {
                //分页
                dataList = list.OrderByDescending(x => x.AnZhuangShiJian).Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList();
                //GPS终端信息
                var gpsInfoList = (from a in list
                                   join gps in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0)
                                   on a.CheLiangId.ToString() equals gps.CheLiangId
                                   select new
                                   {
                                       CheLiangId = a.CheLiangId,
                                       SimKaHao = gps.SIMKaHao,
                                   }).ToList();
                //车辆扩展信息
                var cheLiangExList = (from a in list
                                      join carEx in _cheLiangExRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0)
                                      on a.CheLiangId.ToString() equals carEx.CheLiangId
                                      select new
                                      {
                                          CheLiangId = a.CheLiangId,
                                          FaDongJiHao = carEx.FaDongJiHao,
                                      }).ToList();
                //补充信息
                dataList.ForEach(x =>
                {
                    x.SimKaHao = gpsInfoList.Where(y => y.CheLiangId == x.CheLiangId).FirstOrDefault()?.SimKaHao;
                    x.FaDongJiHao = cheLiangExList.Where(y => y.CheLiangId == x.CheLiangId).FirstOrDefault()?.FaDongJiHao;
                });
                returnData.list = dataList;
            }

            return returnData;
        }

        #endregion

        #region 服务商接入统计
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> GetFuWuShangJieRuXinXi(QueryData dto)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                GetFuWuShangJieRuXinXiDto returnData = GetFuWuShangJieRuDataList(dto, userInfo);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取服务商接入信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { ErrorMessage = "查询出错", StatusCode = 2 };
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<ExportZhiNengShiPingJieRuDto> ExportFuWuShangJieRuXinXi(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                var list = GetFuWuShangJieRuDataList(queryData, userInfo)?.list;
                ExportZhiNengShiPingJieRuRequestDto dto = JsonConvert.DeserializeObject<ExportZhiNengShiPingJieRuRequestDto>(JsonConvert.SerializeObject(queryData.data));
                string tableTitle = "服务商智能视频接入登记表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count() > 0)
                {
                    string FileId = string.Empty;
                    string filePath = CreateExcel(tableTitle, list.ToList(), dto.Cols);

                    FileDTO fileDto = new FileDTO()
                    {
                        AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                        AppName = string.Empty,
                        BusinessId = new Guid().ToString(),
                        BusinessType = "",
                        CreatorId = userInfo.Id,
                        CreatorName = userInfo.UserName,
                        DisplayName = Path.GetFileNameWithoutExtension(filePath),
                        FileName = Path.GetFileName(filePath),
                        Data = File.ReadAllBytes(filePath),
                        Remark = string.Empty,
                        SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
                    };
                    try
                    {
                        FileDto ResFile = FileAgentUtility.UploadFile(fileDto);
                        FileId = ResFile.FileId.ToString();
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { Data = new ExportZhiNengShiPingJieRuDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出服务商智能视频接入登记表" + e.Message, e);
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出服务商智能视频接入登记表" + ex.Message, ex);
                return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }
        private GetFuWuShangJieRuXinXiDto GetFuWuShangJieRuDataList(QueryData dto, UserInfoDtoNew userInfo)
        {
            FuWuShangJieRuRequestDto search = JsonConvert.DeserializeObject<FuWuShangJieRuRequestDto>(JsonConvert.SerializeObject(dto.data));
            Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == 0 && x.FuWuShangOrgCode != null && x.FuWuShangOrgCode != "";
            Expression<Func<OrgBaseInfo, bool>> fwsExp = x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode != null && x.OrgCode != "" && x.OrgType == (int)OrganizationType.本地服务商 && x.OrgName != "" && x.OrgName != null;
            Expression<Func<CheLiangVideoZhongDuanXinXi, bool>> videoExp = x => x.SYS_XiTongZhuangTai == 0;
            List<FuWuShangResponseDto> resultData = new List<FuWuShangResponseDto>();
            GetFuWuShangJieRuXinXiDto returnData = new GetFuWuShangJieRuXinXiDto();
            if (search == null)
            {
                return new GetFuWuShangJieRuXinXiDto { };
            }


            //辖区市
            if (!string.IsNullOrWhiteSpace(search?.XiaQuShi))
            {
                carExp = carExp.And(x => x.XiaQuShi == search.XiaQuShi.Trim());
            }
            //辖区县
            if (!string.IsNullOrWhiteSpace(search?.XiaQuXian))
            {
                carExp = carExp.And(x => x.XiaQuXian == search.XiaQuXian.Trim());
            }
            //车辆种类
            if (search.CheLiangZhongLei.HasValue)
            {
                if (search.CheLiangZhongLei == (int)CheLiangZhongLei.其它车辆)
                {
                    carExp = carExp.And(x => x.CheLiangZhongLei == search.CheLiangZhongLei || x.CheLiangZhongLei == null || x.CheLiangZhongLei == 0);
                }
                else
                {
                    carExp = carExp.And(x => x.CheLiangZhongLei == search.CheLiangZhongLei);
                }
            }
            //设备服务商名称
            if (!string.IsNullOrWhiteSpace(search?.FuWuShangName))
            {
                fwsExp = fwsExp.And(x => x.OrgName.Contains(search.FuWuShangName.Trim()));
            }
            switch (userInfo.OrganizationType)
            {
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                    carExp = carExp.And(x => x.YeHuOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.本地服务商:
                    carExp = carExp.And(x => x.FuWuShangOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.市政府:
                    carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity);
                    break;
                case (int)OrganizationType.县政府:
                    carExp = carExp.And(x => x.XiaQuXian == userInfo.OrganDistrict);
                    break;
                default:
                    return new GetFuWuShangJieRuXinXiDto { };
            }

            var list = (from car in _cheLiangXinXiRepository.GetQuery(carExp)
                        join yh in _cheLiangYeHuRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode != "" && x.OrgCode != null) on car.YeHuOrgCode equals yh.OrgCode
                        join fws in _orgBaseInfoRepository.GetQuery(fwsExp) on car.FuWuShangOrgCode equals fws.OrgCode
                        join video in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(videoExp) on car.Id.ToString() equals video.CheLiangId into table1
                        from t1 in table1.DefaultIfEmpty()
                        join ba in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on car.Id.ToString() equals ba.CheLiangId into table2
                        from t2 in table2.DefaultIfEmpty()
                            //安装日期起
                        where !search.AnZhuangRiQiBegin.HasValue || t1.AnZhuangShiJian >= search.AnZhuangRiQiBegin
                        //安装日期止
                        where !search.AnZhuangRiQiEnd.HasValue || t1.AnZhuangShiJian <= search.AnZhuangRiQiEnd
                        select new QueryFuWuShangJieRuDto
                        {
                            CheLiangId = car.Id,
                            CheliangZhongLei = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            FuWuShangCode = fws.OrgCode,
                            FuWuShangOrgName = fws.OrgName,
                            YeHuOrgCode = car.YeHuOrgCode,
                            BeiAnZhuangTai = t2.BeiAnZhuangTai
                        }).ToList();
            if (list.Count() > 0)
            {

                //不属于枚举中的车全部归类为其他车辆
                list.Where(x => x.CheliangZhongLei == 0 || x.CheliangZhongLei == null).ForEach(x =>
                {
                    x.CheliangZhongLei = (int)CheLiangZhongLei.其它车辆;
                });

                //按服务商分组
                var groupFuWuShangNameList = list.GroupBy(x => x.FuWuShangCode).Select(y => new { y.Key, list = y }).ToList();
                groupFuWuShangNameList.ForEach(x =>
                {
                    //按辖区继续分组
                    var groupXiaQuList = x.list.GroupBy(s => new { s.XiaQuXian, s.XiaQuShi }).Select(d => new { d.Key, list = d }).ToList();
                    groupXiaQuList.ForEach(f =>
                    {
                        //继续按车辆种类分组
                        var groupCheLiangZhongLei = f.list.GroupBy(e => e.CheliangZhongLei).Select(t => new { t.Key, list = t }).ToList();
                        groupCheLiangZhongLei.ForEach(u =>
                        {
                            var dList = u.list.ToList();
                            var firstdata = u.list.FirstOrDefault();
                            FuWuShangResponseDto mo = new FuWuShangResponseDto();
                            mo.XiaQuShi = firstdata.XiaQuShi;
                            mo.XiaQuXian = firstdata.XiaQuXian;
                            mo.FuWuShangOrgName = firstdata.FuWuShangOrgName;
                            mo.FuWuShangCode = firstdata.FuWuShangCode;
                            mo.CheliangZhongLei = u.Key;
                            mo.YeHuNumber = dList.Where(j => !string.IsNullOrWhiteSpace(j.YeHuOrgCode)).GroupBy(g => g.YeHuOrgCode).Count();
                            mo.CheLiangNumber = dList.Count();

                            decimal beiAnCheLiangNumber = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.不通过备案 || j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案 || j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核).Count();
                            mo.CheLiangJieRuLv = beiAnCheLiangNumber / Convert.ToDecimal(mo.CheLiangNumber);
                            mo.JieRuCheLiangShu = (int)beiAnCheLiangNumber;
                            mo.TongGuoBeiAnCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案).Count();
                            mo.WeiTongGuoBeiAnCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.不通过备案).Count();
                            mo.WeiShengHeCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核).Count();
                            mo.QuXiaoBeiAnShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.取消备案).Count();
                            resultData.Add(mo);
                        });
                    });
                });
                returnData.Count = resultData.Count();
                //分页
                returnData.list = resultData.Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList(); ;
            }

            return returnData;
        }

        #endregion

        #region 辖区接入统计


        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> GetXiaQuJieRuXinXi(QueryData dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                GetXiaQuJieRuXinXiDto returnData = GetXiaQuJieRuDataList(dto, userInfo);
                QueryResult result = new QueryResult();
                result.totalcount = returnData.Count;
                result.items = returnData.list;
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取辖区接入信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { ErrorMessage = "查询出错", StatusCode = 2 };
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<ExportZhiNengShiPingJieRuDto> ExportXiaQuJieRuXinXi(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                var list = GetXiaQuJieRuDataList(queryData, userInfo)?.list;
                ExportZhiNengShiPingJieRuRequestDto dto = JsonConvert.DeserializeObject<ExportZhiNengShiPingJieRuRequestDto>(JsonConvert.SerializeObject(queryData.data));
                string tableTitle = "辖区智能视频接入登记表" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count() > 0)
                {
                    string FileId = string.Empty;
                    string filePath = CreateExcel(tableTitle, list.ToList(), dto.Cols);

                    FileDTO fileDto = new FileDTO()
                    {
                        AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                        AppName = string.Empty,
                        BusinessId = new Guid().ToString(),
                        BusinessType = "",
                        CreatorId = userInfo.Id,
                        CreatorName = userInfo.UserName,
                        DisplayName = Path.GetFileNameWithoutExtension(filePath),
                        FileName = Path.GetFileName(filePath),
                        Data = File.ReadAllBytes(filePath),
                        Remark = string.Empty,
                        SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
                    };
                    try
                    {
                        FileDto ResFile = FileAgentUtility.UploadFile(fileDto);
                        FileId = ResFile.FileId.ToString();
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { Data = new ExportZhiNengShiPingJieRuDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出辖区智能视频接入登记表" + e.Message, e);
                        return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportZhiNengShiPingJieRuDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出辖区智能视频接入登记表" + ex.Message, ex);
                return new ServiceResult<ExportZhiNengShiPingJieRuDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private GetXiaQuJieRuXinXiDto GetXiaQuJieRuDataList(QueryData dto, UserInfoDtoNew userInfo)
        {
            XiaQuJieRuRequestDto search = JsonConvert.DeserializeObject<XiaQuJieRuRequestDto>(JsonConvert.SerializeObject(dto.data));
            Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangVideoZhongDuanXinXi, bool>> videoExp = x => x.SYS_XiTongZhuangTai == 0;
            List<XiqQuJieRuResponseDto> resultData = new List<XiqQuJieRuResponseDto>();
            GetXiaQuJieRuXinXiDto returnData = new GetXiaQuJieRuXinXiDto();

            if (search == null)
            {
                return new GetXiaQuJieRuXinXiDto { };
            }
            //辖区市
            if (!string.IsNullOrWhiteSpace(search?.XiaQuShi))
            {
                carExp = carExp.And(x => x.XiaQuShi == search.XiaQuShi.Trim());
            }
            //辖区县
            if (!string.IsNullOrWhiteSpace(search?.XiaQuXian))
            {
                carExp = carExp.And(x => x.XiaQuXian == search.XiaQuXian.Trim());
            }
            //车辆种类
            if (search.CheLiangZhongLei.HasValue)
            {
                if (search.CheLiangZhongLei == (int)CheLiangZhongLei.其它车辆)
                {

                    carExp = carExp.And(x => x.CheLiangZhongLei == search.CheLiangZhongLei || x.CheLiangZhongLei == null || x.CheLiangZhongLei == 0);
                }
                else
                {
                    carExp = carExp.And(x => x.CheLiangZhongLei == search.CheLiangZhongLei);
                }
            }
            switch (userInfo.OrganizationType)
            {
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                    carExp = carExp.And(x => x.YeHuOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.本地服务商:
                    carExp = carExp.And(x => x.FuWuShangOrgCode == userInfo.OrganizationCode);
                    break;
                case (int)OrganizationType.市政府:
                    carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity);
                    break;
                case (int)OrganizationType.县政府:
                    carExp = carExp.And(x => x.XiaQuXian == userInfo.OrganDistrict);
                    break;
                default:
                    return new GetXiaQuJieRuXinXiDto { };
            }

            var list = (from car in _cheLiangXinXiRepository.GetQuery(carExp)
                        join yh in _cheLiangYeHuRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode != "" && x.OrgCode != null) on car.YeHuOrgCode equals yh.OrgCode
                        join video in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(videoExp) on car.Id.ToString() equals video.CheLiangId into table1
                        from t1 in table1.DefaultIfEmpty()
                        join ba in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on car.Id.ToString() equals ba.CheLiangId into table2
                        from t2 in table2.DefaultIfEmpty()
                            //安装日期起
                        where !search.AnZhuangRiQiBegin.HasValue || t1.AnZhuangShiJian >= search.AnZhuangRiQiBegin
                        //安装日期止
                        where !search.AnZhuangRiQiEnd.HasValue || t1.AnZhuangShiJian <= search.AnZhuangRiQiEnd
                        select new QueryXiaQuJieRuDto
                        {
                            CheLiangId = car.Id,
                            CheliangZhongLei = car.CheLiangZhongLei,
                            XiaQuShi = car.XiaQuShi,
                            XiaQuXian = car.XiaQuXian,
                            YeHuOrgCode = car.YeHuOrgCode,
                            BeiAnZhuangTai = t2.BeiAnZhuangTai
                        }).ToList();
            if (list.Count() > 0)
            {

                //不属于枚举中的车全部归类为其他车辆
                list.Where(x => x.CheliangZhongLei == 0 || x.CheliangZhongLei == null).ForEach(x =>
                     {
                         x.CheliangZhongLei = (int)CheLiangZhongLei.其它车辆;
                     });

                //按辖区分组
                var groupXiaQuList = list.GroupBy(s => new { s.XiaQuXian, s.XiaQuShi }).Select(d => new { d.Key, list = d }).ToList();
                groupXiaQuList.ForEach(f =>
                {
                    //继续按车辆种类分组
                    var groupCheLiangZhongLei = f.list.GroupBy(e => e.CheliangZhongLei).Select(t => new { t.Key, list = t }).ToList();
                    groupCheLiangZhongLei.ForEach(u =>
                    {
                        var dList = u.list.ToList();
                        var firstdata = u.list.FirstOrDefault();
                        XiqQuJieRuResponseDto mo = new XiqQuJieRuResponseDto();
                        mo.XiaQuShi = firstdata.XiaQuShi;
                        mo.XiaQuXian = firstdata.XiaQuXian;
                        mo.CheliangZhongLei = u.Key;
                        mo.YeHuNumber = dList.Where(j => !string.IsNullOrWhiteSpace(j.YeHuOrgCode)).GroupBy(g => g.YeHuOrgCode).Count();
                        mo.CheLiangNumber = dList.Count();

                        decimal beiAnCheLiangNumber = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.不通过备案 || j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案 || j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核).Count();
                        mo.CheLiangJieRuLv = Convert.ToDecimal(beiAnCheLiangNumber / Convert.ToDecimal(mo.CheLiangNumber));
                        mo.JieRuCheLiangShu = (int)beiAnCheLiangNumber;
                        mo.TongGuoBeiAnCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案).Count();
                        mo.WeiTongGuoBeiAnCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.不通过备案).Count();
                        mo.WeiShengHeCheLiangShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.未审核).Count();
                        mo.QuXiaoBeiAnShu = dList.Where(j => j.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.取消备案).Count();
                        resultData.Add(mo);
                    });
                });
                returnData.Count = resultData.Count();
                //分页
                returnData.list = resultData.Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList(); ;
            }

            return returnData;
        }

        #endregion

        #region  导出相关

        private string CreateExcel<T>(string TimeFormat, List<T> Rows, Dictionary<string, string> Cols) where T : class
        {
            string ExcelPath = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\ExcelFileCache";
            if (!Directory.Exists(ExcelPath))
            {
                Directory.CreateDirectory(ExcelPath);
            }
            string FilePath = string.Empty;
            FilePath = string.Format("{0}\\{1}.xls", ExcelPath, TimeFormat);
            WriteExcel(FilePath, Rows, Cols);
            return FilePath;
        }

        private void WriteExcel<T>(string FilePath, List<T> Rows, Dictionary<string, string> Cols) where T : class
        {
            PagingUtil<T> pu = new PagingUtil<T>(Rows, 60000);
            string[] colsName = Cols.Keys.ToArray();
            NopiExcel nopi = new NopiExcel(0);
            while (pu.IsEffectivePage)
            {
                List<T> pageData = pu.GetCurrentPage();
                int start = ((pu.PageNo - 1) * pu.PageSize) + 1;
                int end = ((pu.PageNo - 1) * pu.PageSize) + pageData.Count();
                nopi.SetCurrentCreateNewSheet(start + "-" + end);
                nopi.Insertrow(0, 0, colsName);

                for (int i = 0; i < pageData.Count; i++)
                {
                    var startRow = i + 1;
                    nopi.WriteCell(startRow, 0, StringFormat(i + start));
                    for (int j = 0; j < colsName.Length; j++)
                    {
                        string key = string.Empty;
                        Cols.TryGetValue(colsName[j], out key);
                        if (!string.IsNullOrEmpty(key))
                        {
                            nopi.WriteCell(startRow, j, StringFormat(pageData[i].GetType().GetProperty(key).GetValue(pageData[i])?.ToString()));
                        }
                        // 车辆种类
                        if ("CheliangZhongLei".Equals(key))
                        {
                            var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);

                            nopi.WriteCell(startRow, j, ChangeCheLiangZhongLei((int?)temp));
                        }
                        // 接入率
                        if ("CheLiangJieRuLv".Equals(key))
                        {
                            var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);

                            nopi.WriteCell(startRow, j, GetDecimalString((decimal)temp));
                        }

                    }

                    nopi.SetVerticalCenter(0, 0, nopi.getLastRowNum(), nopi.getLastCellNum(0));//垂直居中
                    nopi.SetFontFamliy(0, 0, nopi.getLastRowNum(), nopi.getLastCellNum(0), 12 * 20, "宋体", "", 0);
                    nopi.SetColumnWidth(0, nopi.getLastCellNum(0), 18 * 300);//列宽
                    pu.GotoNextPage();
                }
                nopi.Save(FilePath);
            }
        }

        private string StringFormat(object obj)
        {
            return String.Format("{0}", obj);
        }


        private string ChangeCheLiangZhongLei(int? cheLiangZhongLei)
        {
            switch (cheLiangZhongLei)
            {
                case 1:
                    return "客运班车";
                case 2:
                    return "旅游包车";
                case 3:
                    return "危险货运";
                case 4:
                    return "重型货车";
                case 5:
                    return "公交客运";
                case 6:
                    return "出租客运";
                case 7:
                    return "教练员车";
                case 8:
                    return "普通货运";
                case 9:
                    return "其它车辆";
                case 10:
                    return "校车";
                default:
                    return "其它车辆";

            }
        }


        private string GetDecimalString(decimal number)
        {
            string returnData = "0%";
            try
            {
                if (number <= 0) return returnData;
                return (number * 100).ToString("F2") + "%";
            }
            catch (Exception ex)
            {
                LogHelper.Error("接入统计转换接入率出错" + ex.Message, ex);
                return returnData;
            }
        }

        #endregion


        #region 导出智能视频接入统计
        private static Guid? CreatePingTaiZhiLingQingQiuAndYingDaExcelAndUpload(List<ZhiNengShiPingJieRuResponseDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "智能报警装置安装接入登记表";
            string[] cellTitleArry = { "辖区市", "辖区县", "业户名称", "车牌号", "车牌颜色", "车架号", "发动机号",
                "个体户身份证号", "业户统一社会信用代码", "业户负责人姓名", "业户负责人联系方式", "司机姓名",
                "司机联系方式", "从业资格证号", "智能视频生产厂家", "智能视频设备型号", "SIM卡号", "设备服务商",
                "设备安装人员", "设备安装人员联系电话", "终端安装时间", "集中安装点名称", "交强险保险公司名称", "商业险保险公司名称" };

            HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
            int sheetRowCount = 65536; //每个sheet最大数据行数

            //循环创建sheet
            //因单个sheet最多存储65536条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
            int max_sheet_count = (list.Count + (sheetRowCount - 2) - 1) / (sheetRowCount - 2);

            for (int sheet_index = 0; sheet_index < max_sheet_count; sheet_index++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet($"Sheet{(sheet_index + 1)}");
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
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));

                string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                //附加标题样式
                row.Cells[0].CellStyle = titleStyle;

                row = (HSSFRow)sheet.CreateRow(1);

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
                    row = (HSSFRow)sheet.CreateRow(content_index + 2);
                    int index = 0;

                    row.CreateCell(index++).SetCellValue(item.XiaQuShi);
                    row.CreateCell(index++).SetCellValue(item.XiaQuXian);
                    row.CreateCell(index++).SetCellValue(item.OrgName);
                    row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                    row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                    row.CreateCell(index++).SetCellValue(item.CheJiaHao);
                    row.CreateCell(index++).SetCellValue(item.FaDongJiHao);
                    row.CreateCell(index++).SetCellValue(item.GeTiHuIDCard);
                    row.CreateCell(index++).SetCellValue(item.SheHuiXinYongDaiMa);
                    row.CreateCell(index++).SetCellValue(item.YeHuPrincipalName);
                    row.CreateCell(index++).SetCellValue(item.YeHuPrincipalPhone);
                    row.CreateCell(index++).SetCellValue(item.DriverName);
                    row.CreateCell(index++).SetCellValue(item.DriverPhone);
                    row.CreateCell(index++).SetCellValue(item.CongYeZiGeZhengHao);
                    row.CreateCell(index++).SetCellValue(item.VideoShengChanChangJia);
                    row.CreateCell(index++).SetCellValue(item.VideoSheBeiXingHao);
                    row.CreateCell(index++).SetCellValue(item.SimKaHao);
                    row.CreateCell(index++).SetCellValue(item.FuWuShangOrgName);
                    row.CreateCell(index++).SetCellValue(item.SheBeiAnZhuangRenYuanName);
                    row.CreateCell(index++).SetCellValue(item.SheBeiAnZhuangRenYuanPhone);
                    string anZhuangShiJian = "";
                    if (item.AnZhuangShiJian.HasValue)
                    {
                        anZhuangShiJian = Convert.ToDateTime(item.AnZhuangShiJian).ToString("yyyy-MM-dd");
                    }
                    row.CreateCell(index++).SetCellValue(anZhuangShiJian);
                    row.CreateCell(index++).SetCellValue(item.JiZhongAnZhuangDianMingCheng);
                    row.CreateCell(index++).SetCellValue(item.JiaoQiangXianOrgName);
                    row.CreateCell(index++).SetCellValue(item.ShangYeXianOrgName);



                    for (int contInx = 0; contInx < index; contInx++)
                    {
                        //if (contInx == 0)
                        //{
                        //    //附加内容样式
                        //    row.Cells[contInx].CellStyle = contentStyle_Center;
                        //}
                        //else
                        //{
                        //附加内容样式
                        row.Cells[contInx].CellStyle = contentStyle;
                        //}
                        if (content_index == 0)
                        {
                            sheet.AutoSizeColumn(contInx, true);
                        }
                    }
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

        public override void Dispose()
        {
            _cheLiangXinXiRepository.Dispose();
            _cheLiangBaoXianXinXiRepository.Dispose();
            _cheLiangYeHuLianXiXinXiRepository.Dispose();
        }
    }
}
