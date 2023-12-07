using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Framework.OperationLog;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services;
using Nest;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using EntityFramework.Extensions;
using NPOI.SS.Formula.Functions;
using Dapper;
using System.Data.Common;
using System.Globalization;
using Conwin.Framework.Log4net.Extendsions;

namespace Conwin.GPSDAGL.Services
{

    /*
     TODO
     1、车辆档案 兼容新字段（部标）
     2、车辆档案增加运政端车辆运营状态
     3、车辆新增（安装终端之后GPS终端）-调用协同接口清理数据缓存

         */
    public class CheLiangService : ApiServiceBase, ICheLiangService
    {
        private readonly ICheLiangRepository _cheLiangXinXiRepository;
        private readonly ICheLiangZuZhiXinXiRepository _cheLiangZuZhiXinXiRepository;
        private readonly ICheLiangExRepository _cheLianExRepository;
        private readonly ICheLiangDingWeiXinXiRepository _cheLiangDingWeiXinXiRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly IYongHuCheLiangXinXiRepository _yongHuCheLiangXinXiRepository;
        private readonly ICheLiangExRepository _cheLiangExRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        private readonly IFuWuShangRepository _fuWuShangRepository;
        private readonly IJiaShiYuanRepository _jiaShiYuanRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;
        private readonly IZhongDuanFileMapperRepository _zhongDuanFileMapperRepository;
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        private readonly IFuWuShangCheLiangRepository _fuWuShangCheLiangRepository;
        private readonly IFuWuShangCheLiangGPSZhongDuanXinXiRepository _fuWuShangCheLiangGPSZhongDuanXinXiRepository;

        private readonly IFuWuShangCheLiangVideoZhongDuanXinXiRepository
            _fuWuShangCheLiangVideoZhongDuanXinXiRepository;

        private readonly ICheLiangBaoXianXinXiRepository _cheLiangBaoXianXinXiRepository;
        private readonly ICheLiangYeHuLianXiXinXiRepository _cheLiangYeHuLianXiXinXiRepository;

        private readonly ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository
            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;

        private static List<EnterpriseInformation> EnterpriseInformation;
        private static object workingObj = new object();
        private static bool IsAuditWorking = false;
        private readonly IGPSAuditRecordRepository _iGPSAuditRecordRepository;
        public string[] EventType = { "ZHBJSJ020","ZHBJSJ026","ZHBJSJ027","ZHBJSJ030","ZHBJSJ000","ZHBJSJ001",
            "ZHBJSJ002","ZHBJSJ003","ZHBJSJ004","ZHBJSJ005","ZHBJSJ006","ZHBJSJ007","ZHBJSJ008","ZHBJSJ009","ZHBJSJ010","ZHBJSJ011","ZHBJSJ012",
            "ZHBJSJ013","ZHBJSJ014","ZHBJSJ015","ZHBJSJ016","ZHBJSJ017","ZHBJSJ018","ZHBJSJ019"};

        //车辆合作关系绑定表
        private readonly IVehiclePartnershipBindingRepository _vehiclePartnershipBinding;
        //合作关系绑定表
        private readonly IPartnershipBindingTableRepository _partnershipBindingTable;
        public CheLiangService(ICheLiangRepository cheLiangXinXiRepository,
            ICheLiangZuZhiXinXiRepository cheLiangZuZhiXinXiRepository,
            ICheLiangExRepository cheLianExRepository, ICheLiangDingWeiXinXiRepository cheLiangDingWeiXinXiRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
            IYongHuCheLiangXinXiRepository yongHuCheLiangXinXiRepository,
            ICheLiangExRepository cheLiangExRepository,
            ICheLiangYeHuRepository cheLiangYeHuRepository,
            IBussinessLogger _bussinessLogger,
            IJiaShiYuanRepository jiaShiYuanRepository,
            IZhongDuanFileMapperRepository zhongDuanFileMapperRepository,
            IFuWuShangRepository fuWuShangRepository,
            ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
            ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository,
            IFuWuShangCheLiangRepository fuWuShangCheLiangRepository,
            IFuWuShangCheLiangGPSZhongDuanXinXiRepository fuWuShangCheLiangGPSZhongDuanXinXiRepository,
            IFuWuShangCheLiangVideoZhongDuanXinXiRepository fuWuShangCheLiangVideoZhongDuanXinXiRepository,
            ICheLiangBaoXianXinXiRepository cheLiangBaoXianXinXiRepository,
            ICheLiangYeHuLianXiXinXiRepository cheLiangYeHuLianXiXinXiRepository,
            ICheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository
                cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository,
            IGPSAuditRecordRepository gPSAuditRecordRepository,
            IVehiclePartnershipBindingRepository vehiclePartnershipBindingRepository,
            IPartnershipBindingTableRepository partnershipBindingTableRepository
        ) : base(_bussinessLogger)
        {
            _cheLiangXinXiRepository = cheLiangXinXiRepository;
            _cheLiangZuZhiXinXiRepository = cheLiangZuZhiXinXiRepository;
            _cheLianExRepository = cheLianExRepository;
            _cheLiangDingWeiXinXiRepository = cheLiangDingWeiXinXiRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _yongHuCheLiangXinXiRepository = yongHuCheLiangXinXiRepository;
            _cheLiangExRepository = cheLiangExRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _fuWuShangRepository = fuWuShangRepository;
            _zhongDuanFileMapperRepository = zhongDuanFileMapperRepository;
            _jiaShiYuanRepository = jiaShiYuanRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
            _fuWuShangCheLiangRepository = fuWuShangCheLiangRepository;
            _fuWuShangCheLiangGPSZhongDuanXinXiRepository = fuWuShangCheLiangGPSZhongDuanXinXiRepository;
            _fuWuShangCheLiangVideoZhongDuanXinXiRepository = fuWuShangCheLiangVideoZhongDuanXinXiRepository;
            _cheLiangBaoXianXinXiRepository = cheLiangBaoXianXinXiRepository;
            _cheLiangYeHuLianXiXinXiRepository = cheLiangYeHuLianXiXinXiRepository;
            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository =
                cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository;
            var configDataPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                "Config/Common/EnterpriseInformation.json");
            EnterpriseInformation = CommonHelper.ReadConfigData<List<EnterpriseInformation>>(configDataPath);
            _iGPSAuditRecordRepository = gPSAuditRecordRepository;

            _vehiclePartnershipBinding = vehiclePartnershipBindingRepository;
            _partnershipBindingTable = partnershipBindingTableRepository;
        }

        public override void Dispose()
        {
            _cheLiangXinXiRepository.Dispose();
            _cheLiangZuZhiXinXiRepository.Dispose();
            _cheLianExRepository.Dispose();
            _cheLiangDingWeiXinXiRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
            _yongHuCheLiangXinXiRepository.Dispose();
            _iGPSAuditRecordRepository.Dispose();
            _partnershipBindingTable.Dispose();
        }

        #region 列表

        #region 查询车辆档案列表

        public ServiceResult<QueryResult> Query(QueryData queryData, string SysId)
        {

            var result = new ServiceResult<QueryResult>();
            QueryResult queryResult = new QueryResult();
            //1.车牌号码   //T_CheLiang
            // 2.车牌颜色  //T_CheLiang
            //3.车辆状态   //T_CheLiangEx
            //4.业户名称  T_OrgBaseInfo/T_CheLiangYeHu
            //5.在线状态   T_CheLiang
            //6.业务办理状态 
            //7.运营商名称  T_OrgBaseInfo/T_CheLiangYeHu
            //8.车载电话   //T_CheLiang
            //9.终端MDT   T_CheLiangGPSZhongDuanPeiZhiXinXi/ T_CheLiangZhiNengShiPinZhongDuanPeiZhiXinXi
            //10.SIM卡号   T_CheLiangGPSZhongDuanPeiZhiXinXi
            //11.IMEI卡号   T_CheLiangZhiNengShiPinZhongDuanPeiZhiXinXi
            //12.创建时间  //T_CheLiang

            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();

                CheLiangXinXiInput dto =
                    JsonConvert.DeserializeObject<CheLiangXinXiInput>(JsonConvert.SerializeObject(queryData.data));
                if (dto.BeiAnTongGuoBeginTime > dto.BeiAnTongGuoEndTime)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "备案通过开始时间不能大于备案通过结束时间" };
                }

                IEnumerable<CheLiangSearchResponseDto> list = GetCheLiangXinXiList(userInfo, queryData);
                queryResult.totalcount = list.Distinct().Count();
                if (queryResult.totalcount > 0)
                {
                    queryResult.items = list.Distinct().OrderByDescending(u => u.ChuangJianShiJian)
                        .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                }

                result.Data = queryResult;

                //OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                //{
                //    SystemName = OperLogSystemName.清远市交通运输局两客一危营运车辆主动安全防控平台,
                //    ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                //    ActionName= nameof(Query),
                //    BizOperType= OperLogBizOperType.Query,
                //    ShortDescription="车辆档案列表查询",
                //    OperatorName=userInfo.UserName,
                //    OldBizContent = JsonConvert.SerializeObject(queryData),
                //    OperatorID =userInfo.Id,
                //    OperatorOrgCode=userInfo.OrganizationCode,
                //    OperatorOrgName=userInfo.OrganizationName,
                //    SysID= SysId,
                //    AppCode= System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                //}) ;

            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "车辆档案查询异常";
                LogHelper.Error($"调用服务{nameof(CheLiangService)}.{nameof(Query)}出错,异常信息:{ex.Message}", ex);
                return result;
            }

            return result;

        }

        #endregion

        #region 导出车辆档案列表

        public ServiceResult<ExportCheliangXinXiDto> ExportCheliangXinXi(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetCheLiangXinXiList(userInfo, queryData).ToList();

                ExportCheliangXinXiRequestDto dto =
                    JsonConvert.DeserializeObject<ExportCheliangXinXiRequestDto>(
                        JsonConvert.SerializeObject(queryData.data));
                string tableTitle = "车辆导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list.Count() > 0)
                {
                    try
                    {
                        string FileId = string.Empty;
                        //string filePath = CreateExcel(tableTitle, list, dto.Cols);


                        //FileDTO fileDto = new FileDTO()
                        //{
                        //    AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                        //    SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"],
                        //    AppName = "",
                        //    BusinessId = "",
                        //    BusinessType = "",
                        //    CreatorId = userInfo.Id,
                        //    CreatorName = userInfo.UserName,
                        //    DisplayName = Path.GetFileNameWithoutExtension(filePath),
                        //    FileName = Path.GetFileName(filePath),
                        //    Data = File.ReadAllBytes(filePath),
                        //    Remark = string.Empty

                        //};
                        //FileDto ResFile = FileAgentUtility.UploadFile(fileDto);
                        //FileId = ResFile.FileId.ToString();

                        Guid? fielId = CreateCheLiangDangAnExcelAndUpload(list, tableTitle);

                        if (fielId != null)
                        {
                            FileId = fielId.ToString();
                        }
                        else
                        {
                            LogHelper.Error("生成车辆档案上传文件出错" + JsonConvert.SerializeObject(queryData));
                            return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出失败", StatusCode = 2 };
                        }


                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                            ActionName = nameof(ExportCheliangXinXi),
                            BizOperType = OperLogBizOperType.Export,
                            ShortDescription = "车辆档案导出",
                            OperatorName = userInfo.UserName,
                            OldBizContent = JsonConvert.SerializeObject(queryData),
                            OperatorID = userInfo.Id,
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                        return new ServiceResult<ExportCheliangXinXiDto>
                        { Data = new ExportCheliangXinXiDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆档案出错" + e.Message, e);
                        return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportCheliangXinXiDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆档案信息出错" + ex.Message, ex);
                return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }

        }

        #endregion

        #region 导出车辆档案

        private static Guid? CreateCheLiangDangAnExcelAndUpload(List<CheLiangSearchResponseDto> list, string fileName)
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    return null;
                }

                //string title = "车辆档案";
                string[] cellTitleArry =
                {
                    "车牌号码", "车牌颜色", "车辆种类", "第三方机构名称", "企业名称", "营运状态", "版本协议", "备案状态", "GPS审核结果", "提交备案时间", "备案通过时间",
                    "辖区省", "辖区市", "辖区县", "GPS终端号MDT", "智能视频终端设备MDT", "SIM卡号", "车架号", "智能视频设备型号", "智能视频终端生产厂家"
                };
                HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
                int sheetRowCount = 65535; //每个sheet最大数据行数

                //循环创建sheet
                //因单个sheet最多存储65535条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
                int max_sheet_count = (list.Count + (sheetRowCount - 1) - 1) / (sheetRowCount - 1);
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
                    //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                    //string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                    //row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                    ////附加标题样式
                    //row.Cells[0].CellStyle = titleStyle;

                    row = (HSSFRow)sheet.CreateRow(0);

                    for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                    {
                        row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                        //附加表头样式
                        row.Cells[cell_index].CellStyle = cellStyle;
                    }

                    //内容
                    var loop_list = list.Skip(sheet_index * (sheetRowCount - 1)).Take(sheetRowCount - 1).ToList();
                    for (int content_index = 0; content_index < loop_list.Count; content_index++)
                    {
                        var item = loop_list[content_index];
                        row = (HSSFRow)sheet.CreateRow(content_index + 1);
                        int index = 0;
                        //车牌号
                        row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                        //车牌颜色
                        row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                        //车辆种类
                        string chelaingzhongleiStr = "";
                        if (item.CheLiangZhongLei.HasValue)
                        {
                            chelaingzhongleiStr = typeof(CheLiangZhongLei).GetEnumName(item.CheLiangZhongLei);
                            ;
                        }

                        row.CreateCell(index++).SetCellValue(chelaingzhongleiStr);
                        //服务商名称
                        row.CreateCell(index++).SetCellValue(item.FuWuShangName);
                        //企业名称
                        row.CreateCell(index++).SetCellValue(item.QiYeMingCheng);
                        //营运状态
                        row.CreateCell(index++).SetCellValue(item.YunZhengZhuangTai);
                        //协议版本
                        string shuJuTongXunBanBenHao = "";
                        if (item.ShuJuTongXunBanBenHao.HasValue)
                        {
                            try
                            {
                                shuJuTongXunBanBenHao = ((ZhongDuanShuJuTongXunBanBenHao)item.ShuJuTongXunBanBenHao)
                                    .GetDescription();
                            }
                            catch (Exception)
                            {
                                shuJuTongXunBanBenHao = item.ShuJuTongXunBanBenHao.ToString();
                            }
                        }

                        row.CreateCell(index++).SetCellValue(shuJuTongXunBanBenHao);
                        //备案状态
                        string beiAnStatus = "";
                        if (item.BeiAnZhuangTai.HasValue)
                        {
                            beiAnStatus = typeof(ZhongDuanBeiAnZhuangTai).GetEnumName(item.BeiAnZhuangTai);
                            ;
                        }

                        row.CreateCell(index++).SetCellValue(beiAnStatus);
                        //GPS审核结果
                        var findingsAudit = typeof(GPSAuditStatus).GetEnumName(item.GPSAuditStatus);
                        row.CreateCell(index++).SetCellValue(findingsAudit);
                        //提交备案时间
                        string tiJiaoBeiAnShiJianStr = "";
                        if (item.TiJiaoBeiAnShiJian.HasValue)
                        {
                            tiJiaoBeiAnShiJianStr = item.TiJiaoBeiAnShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        row.CreateCell(index++).SetCellValue(tiJiaoBeiAnShiJianStr);
                        //备案通过时间
                        string beiAnTongGuoShiJian = "";
                        if (item.BeiAnShenHeShiJian.HasValue &&
                            item.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                        {
                            beiAnTongGuoShiJian = item.BeiAnShenHeShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        row.CreateCell(index++).SetCellValue(beiAnTongGuoShiJian);
                        //辖区省
                        row.CreateCell(index++).SetCellValue(item.XiaQuSheng);
                        //辖区市
                        row.CreateCell(index++).SetCellValue(item.XiaQuShi);
                        //辖区县
                        row.CreateCell(index++).SetCellValue(item.XiaQuXian);
                        //GPS终端MDT
                        row.CreateCell(index++).SetCellValue(item.GpsZhongDuanMDT);
                        //智能视频MDT
                        row.CreateCell(index++).SetCellValue(item.VideoZhongDuanMDT);
                        //SIM卡号
                        row.CreateCell(index++).SetCellValue(item.SIMKaHao);
                        //车架号
                        row.CreateCell(index++).SetCellValue(item.CheJiaHao);
                        //智能视频设备型号
                        row.CreateCell(index++).SetCellValue(item.VideoSheBeiXingHao);
                        //智能视频生产厂家
                        row.CreateCell(index++).SetCellValue(item.VideoShengChanChangJia);

                        ////人工审核状态
                        //var videoShengChanChangJia = typeof(ManualApprovalStatus).GetEnumName(item.ManualApprovalStatus);
                        //row.CreateCell(index++).SetCellValue(videoShengChanChangJia);
                        //var selectThirdParty = "是";
                        //if (string.IsNullOrEmpty(item.ThirdPartyState))
                        //{
                        //    selectThirdParty = "否";
                        //}
                        ////是否选择第三方
                        //row.CreateCell(index++).SetCellValue(selectThirdParty);

                        for (int contInx = 0; contInx < index; contInx++)
                        {
                            row.Cells[contInx].CellStyle = contentStyle;
                            //if (content_index == 0)
                            //{
                            //    sheet.AutoSizeColumn(contInx, true);
                            //}
                        }
                    }

                    //表格样式
                    DefaultStyle(sheet, cellTitleArry);
                }

                //上传
                var extension = "zip";
                fileName += ".xlsx";
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
                    fileDto.Data = Compress(ms.ToArray());
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
            catch (Exception ex)
            {
                LogHelper.Error("生成导出车辆档案文件上传出错" + ex.Message, ex);
                return null;
            }
        }

        public static byte[] Compress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream Compress = new GZipStream(ms, CompressionMode.Compress);
                Compress.Write(bytes, 0, bytes.Length);
                Compress.Close();
                return ms.ToArray();

            }
        }

        /// <summary>
        /// 统一用同一种表格样式
        /// </summary>
        /// <param name="sheet">当前sheet</param>
        /// <param name="columnName">每一列的列表标题</param>
        /// <param name="dataList">表格内容数据</param>        
        private static void DefaultStyle(HSSFSheet sheet, string[] columnName)
        {
            //按照每列最长的那个格子设置列宽
            AutoColumnWidth(sheet, columnName);
        }

        //按照每列最长的那个格子设置列宽
        private static void AutoColumnWidth(HSSFSheet sheet, string[] columnName)
        {
            for (int i = 0; i < columnName.Length; i++)
            {
                int colWidth = sheet.GetColumnWidth(i) / 256;
                for (int row = 0; row < sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow.GetCell(i) != null)
                    {
                        int length = Encoding.Default.GetBytes(currentRow.GetCell(i).ToString()).Length + 12;
                        if (colWidth < length)
                        {
                            colWidth = length;
                        }
                    }
                }

                sheet.SetColumnWidth(i, colWidth * 256);
            }
        }

        #endregion

        #region 查询车辆档案列表数据集

        private IEnumerable<CheLiangSearchResponseDto> GetCheLiangXinXiList(UserInfoDtoNew userInfo,
            QueryData queryData)
        {
            CheLiangXinXiInput dto =
                JsonConvert.DeserializeObject<CheLiangXinXiInput>(JsonConvert.SerializeObject(queryData.data));
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangEx, bool>> cheLiangXiangQingexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<JiaShiYuan, bool>> jiaShiYuanexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangVideoZhongDuanConfirm, bool>> zdConfirmexp = t => t.SYS_XiTongZhuangTai == 0;
            if (dto.StartTime.HasValue)
            {
                cheliangexp = cheliangexp.And(p => p.SYS_ChuangJianShiJian >= dto.StartTime);
            }

            if (dto.EndTime.HasValue)
            {
                cheliangexp = cheliangexp.And(p => p.SYS_ChuangJianShiJian <= dto.EndTime);
            }

            //车牌号
            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao)) //T_CheLiang
            {
                dto.ChePaiHao = Regex.Replace(dto.ChePaiHao, @"\s", "");
                cheliangexp = cheliangexp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.ToUpper()));
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe)) //T_CheLiang
            {
                cheliangexp = cheliangexp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe);
            }

            //车辆状态
            if (!string.IsNullOrWhiteSpace(dto.YunZhengZhuangTai)) //T_CheLiang
            {
                if (dto.YunZhengZhuangTai == "非营运")
                {
                    cheliangexp = cheliangexp.And(x => x.YunZhengZhuangTai != "营运" || x.YunZhengZhuangTai == null);
                }
                else
                {
                    cheliangexp = cheliangexp.And(p => p.YunZhengZhuangTai == dto.YunZhengZhuangTai.Trim());
                }
            }

            //车辆种类
            if (!string.IsNullOrEmpty(dto.CheLiangZhongLei))
            {
                var cheLiangZhongLeiList = dto.CheLiangZhongLei.Split(',').ToList();
                cheliangexp = cheliangexp.And(x => cheLiangZhongLeiList.Contains(x.CheLiangZhongLei.ToString()));
            }

            //企业用户可以看到自己管辖范围下的车辆
            if (userInfo.OrganizationType == (int)OrganizationType.企业 ||
                userInfo.OrganizationType == (int)OrganizationType.个体户)
            {
                cheliangexp = cheliangexp.And(u => u.YeHuOrgCode == userInfo.OrganizationCode);
            }

            //政府可以看到自己辖区范围内的车
            if (userInfo.OrganizationType == (int)OrganizationType.市政府 ||
                userInfo.OrganizationType == (int)OrganizationType.县政府)
            {
                switch (userInfo.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        dto.XiaQuShi = userInfo.OrganCity;
                        break;
                    case (int)OrganizationType.县政府:
                        dto.XiaQuShi = userInfo.OrganCity;

                        //2022-04-21 李永徽调整县政府可以看到全市的车辆
                        //dto.XiaQuXian = userInfo.OrganDistrict;
                        break;
                }
            }

            //街道管理组
            if (userInfo.OrganizationType == (int)OrganizationType.街道企业管理组)
            {
                dto.XiaQuShi = userInfo.OrganCity;
                dto.XiaQuXian = userInfo.OrganDistrict;
                orgBaseexp = orgBaseexp.And(x => x.Street == userInfo.OrganTown);
            }

            if (!string.IsNullOrWhiteSpace(dto.XiaQuShi))
            {
                cheliangexp = cheliangexp.And(u => u.XiaQuShi == dto.XiaQuShi);
            }

            if (!string.IsNullOrWhiteSpace(dto.XiaQuXian))
            {
                cheliangexp = cheliangexp.And(u => u.XiaQuXian == dto.XiaQuXian);
            }

            var enterpriseCodeList = new List<string>();
            if (userInfo.OrganizationType == (int) OrganizationType.本地服务商)
            { 
                var partnershipBindingList = _partnershipBindingTable.GetQuery(x => x.SYS_XiTongZhuangTai == 0
                                                                                    && (x.ZhuangTai ==
                                                                                        (int) CooperationStatus.审批通过
                                                                                        || x.ZhuangTai ==
                                                                                        (int) CooperationStatus
                                                                                            .第三方发起取消合作
                                                                                        || x.ZhuangTai ==
                                                                                        (int) CooperationStatus.企业发起取消合作
                                                                                    ) &&
                                                                                    x.ServiceProviderCode == userInfo.OrganizationCode

                ).ToList();
                if (partnershipBindingList.Any())
                {
                    enterpriseCodeList = partnershipBindingList.Select(t => t.EnterpriseCode).ToList();
                }

                cheliangexp = cheliangexp.And(u =>
                    (u.FuWuShangOrgCode == userInfo.OrganizationCode || u.FuWuShangOrgCode == "") &&
                    enterpriseCodeList.Contains(u.YeHuOrgCode));
            }
            if (dto.BeiAnTongGuoEndTime.HasValue)
            {
                dto.BeiAnTongGuoEndTime = dto.BeiAnTongGuoEndTime.Value.AddDays(1).AddSeconds(-1);
            }

            if (dto.ShenHeShiJianEndTime.HasValue)
            {
                dto.ShenHeShiJianEndTime = dto.ShenHeShiJianEndTime.Value.AddDays(1).AddSeconds(-1);
            }

            //GPS审核结果
            if (dto.GPSAuditStatus.HasValue)
            {
                cheliangexp = cheliangexp.And(x => x.GPSAuditStatus == dto.GPSAuditStatus);
            }
            var list = from car in _cheLiangXinXiRepository.GetQuery(cheliangexp)
                       join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                           on car.YeHuOrgCode equals org.OrgCode
                       join b in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0) on
                           car.Id.ToString() equals b.CheLiangId into temp1
                       from te1 in temp1.DefaultIfEmpty()
                       join vzdc in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(zdConfirmexp) on car.Id.ToString() equals
                           vzdc.CheLiangId into temp
                       from vzdc in temp.DefaultIfEmpty()
                       join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on car.FuWuShangOrgCode
                           equals fws.OrgCode into table1
                       from fwslist in table1.DefaultIfEmpty()
                       join gps in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on
                           car.Id.ToString() equals gps.CheLiangId into table2
                       from t2 in table2.DefaultIfEmpty()
                       join tongxunpeizhi_temp in
                           _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on
                           car.Id equals tongxunpeizhi_temp.CheLiangID into tongxunpeizhi_temp2
                       from tongxunpeizhi in tongxunpeizhi_temp2.DefaultIfEmpty()

                       where string.IsNullOrEmpty(dto.SouYouRen) || org.OrgName.Contains(dto.SouYouRen)
                       where string.IsNullOrEmpty(dto.CheJiaHao) || car.CheJiaHao.Contains(dto.CheJiaHao)
                       where !dto.BeiAnZhuangTai.HasValue ||
                             (dto.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.待提交 &&
                              (vzdc.BeiAnZhuangTai == dto.BeiAnZhuangTai || vzdc.BeiAnZhuangTai == null)) ||
                             vzdc.BeiAnZhuangTai == dto.BeiAnZhuangTai
                       where string.IsNullOrEmpty(dto.FuWuShangName) || fwslist.OrgName.Contains(dto.FuWuShangName)
                       where !dto.ShenHeShiJianBeginTime.HasValue || vzdc.SYS_ZuiJinXiuGaiShiJian >= dto.ShenHeShiJianBeginTime
                       where !dto.ShenHeShiJianEndTime.HasValue || vzdc.SYS_ZuiJinXiuGaiShiJian <= dto.ShenHeShiJianEndTime
                       where !dto.BeiAnTongGuoBeginTime.HasValue ||
                             (vzdc.SYS_ZuiJinXiuGaiShiJian >= dto.BeiAnTongGuoBeginTime &&
                              vzdc.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                       where !dto.BeiAnTongGuoEndTime.HasValue ||
                             (vzdc.SYS_ZuiJinXiuGaiShiJian <= dto.BeiAnTongGuoEndTime &&
                              vzdc.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                       where !dto.ShuJuTongXunBanBenHao.HasValue ||
                             (tongxunpeizhi.BanBenHao == (int?)dto.ShuJuTongXunBanBenHao)
                      
                       select new CheLiangSearchResponseDto
                       {
                           Id = car.Id,
                           ChePaiHao = car.ChePaiHao,
                           ChePaiYanSe = car.ChePaiYanSe,
                           CheLiangZhongLei = car.CheLiangZhongLei,
                           QiYeMingCheng = org.OrgName,
                           ChuangJianShiJian = car.SYS_ChuangJianShiJian,
                           CheLiangLeiXing = car.CheLiangLeiXing,
                           XiaQuSheng = car.XiaQuSheng,
                           XiaQuShi = car.XiaQuShi,
                           XiaQuXian = car.XiaQuXian,
                           YunZhengZhuangTai = car.YunZhengZhuangTai,
                           NianShenZhuangTai = car.NianShenZhuangTai,
                           CheJiaHao = car.CheJiaHao,
                           BeiAnZhuangTai = vzdc.BeiAnZhuangTai,
                           FuWuShangOrgCode = car.FuWuShangOrgCode,
                           TiJiaoBeiAnShiJian = vzdc.TiJiaoBeiAnShiJian,
                           FuWuShangName = fwslist.OrgName,
                           BeiAnShenHeShiJian = vzdc.SYS_ZuiJinXiuGaiShiJian,
                           SIMKaHao = t2.SIMKaHao,
                           GpsZhongDuanMDT = t2.ZhongDuanMDT,
                           VideoZhongDuanMDT = te1.ZhongDuanMDT,
                           VideoSheBeiXingHao = te1.SheBeiXingHao,
                           VideoShengChanChangJia = te1.ShengChanChangJia,
                           ShuJuTongXunBanBenHao = tongxunpeizhi.BanBenHao,
                           ManualApprovalStatus = car.ManualApprovalStatus,
                           ThirdPartyState = car.FuWuShangOrgCode,
                           EnterpriseCode = car.YeHuOrgCode,
                           GPSAuditStatus = car.GPSAuditStatus,
                           BusinessHandlingResults = car.BusinessHandlingResults,
                           IsHavVideoAlarmAttachment = car.IsHavVideoAlarmAttachment
                       };

            return list;
        }

        #endregion





        #region  获取车辆取消备案信息列表

        public ServiceResult<QueryResult> GetCancelRecordVehicleList(QueryData dto)
        {
            try
            {

                QueryResult result = new QueryResult();

                var list = GetCancelRecordVehicleDataList(dto);
                result.totalcount = list.Count();
                if (result.totalcount > 0)
                {
                    DateTime dt = Convert.ToDateTime("2023-01-01");
                    var fuWuShangList = _fuWuShangRepository.GetQuery(x=>x.SYS_ZuiJinXiuGaiShiJian> dt).Select(x => new
                    {
                        OrgCode = x.OrgCode,
                        OrgName = x.OrgName,
                        ZuiJinXiuGaiShiJian = x.SYS_ZuiJinXiuGaiShiJian
                    }).ToList();

                   
                    var resultList = list.OrderByDescending(x => x.CancelRecordDateTime).Skip((dto.page - 1) * dto.rows)
                        .Take(dto.rows).ToList();

                    resultList.Each(x => {
                        x.FuWuShangOrgName = fuWuShangList.Where(q => q.OrgCode == x.FuWuShangOrgCode).OrderByDescending(y => y.ZuiJinXiuGaiShiJian).FirstOrDefault()?.OrgName;
                    });
                    result.items = resultList;
                }

                return new ServiceResult<QueryResult> { Data = result, StatusCode = 0 };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询取消备案车辆列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询错误" };
            }


        }

        #endregion


        #region 导出取消备案车辆档案

        public ServiceResult<ExportCheliangXinXiDto> ExportCancelRecordVehicle(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetCancelRecordVehicleDataList(queryData).ToList();

                DateTime dt = Convert.ToDateTime("2023-01-01");
                var fuWuShangList = _fuWuShangRepository.GetQuery(x => x.SYS_ZuiJinXiuGaiShiJian > dt).Select(x => new
                {
                    OrgCode = x.OrgCode,
                    OrgName = x.OrgName,
                    ZuiJinXiuGaiShiJian = x.SYS_ZuiJinXiuGaiShiJian
                }).ToList();

                list.Each(x => {
                    x.FuWuShangOrgName = fuWuShangList.Where(q => q.OrgCode == x.FuWuShangOrgCode).OrderByDescending(y => y.ZuiJinXiuGaiShiJian).FirstOrDefault()?.OrgName;
                });

                QueryCancelRecordVehicleDto dto =
                    JsonConvert.DeserializeObject<QueryCancelRecordVehicleDto>(
                        JsonConvert.SerializeObject(queryData.data));
                string tableTitle = "取消备案车辆导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list.Count() > 0)
                {
                    try
                    {
                        string FileId = string.Empty;

                        Guid? fielId = CreateCancelRecordVehicleExcelAndUpload(list, tableTitle);

                        if (fielId != null)
                        {
                            FileId = fielId.ToString();
                        }
                        else
                        {
                            LogHelper.Error("生成取消备案车辆档案上传文件出错" + JsonConvert.SerializeObject(queryData));
                            return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出失败", StatusCode = 2 };
                        }

                        return new ServiceResult<ExportCheliangXinXiDto>
                        { Data = new ExportCheliangXinXiDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出取消备案车辆档案出错" + e.Message, e);
                        return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportCheliangXinXiDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出取消备案车辆档案信息出错" + ex.Message, ex);
                return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }

        }


        #endregion

        #region 生成取消备案Excel文件并上传文件组件

        private static Guid? CreateCancelRecordVehicleExcelAndUpload(List<CancelRecordVehicleDto> list, string fileName)
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    return null;
                }

                //string title = "车辆档案";
                string[] cellTitleArry = { "车牌号码", "车牌颜色", "车辆种类", "服务商名称", "辖区市", "辖区县", "企业名称", "取消备案时间", "取消备案原因" };
                HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
                int sheetRowCount = 65535; //每个sheet最大数据行数

                //循环创建sheet
                //因单个sheet最多存储65535条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
                int max_sheet_count = (list.Count + (sheetRowCount - 1) - 1) / (sheetRowCount - 1);
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
                    //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                    //string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                    //row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                    ////附加标题样式
                    //row.Cells[0].CellStyle = titleStyle;

                    row = (HSSFRow)sheet.CreateRow(0);

                    for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                    {
                        row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                        //附加表头样式
                        row.Cells[cell_index].CellStyle = cellStyle;
                    }

                    //内容
                    var loop_list = list.Skip(sheet_index * (sheetRowCount - 1)).Take(sheetRowCount - 1).ToList();
                    for (int content_index = 0; content_index < loop_list.Count; content_index++)
                    {
                        var item = loop_list[content_index];
                        row = (HSSFRow)sheet.CreateRow(content_index + 1);
                        int index = 0;
                        //车牌号
                        row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                        //车牌颜色
                        row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                        //车辆种类
                        string chelaingzhongleiStr = "";
                        if (item.CheLiangZhongLei.HasValue)
                        {
                            chelaingzhongleiStr = typeof(CheLiangZhongLei).GetEnumName(item.CheLiangZhongLei);
                            ;
                        }

                        row.CreateCell(index++).SetCellValue(chelaingzhongleiStr);
                        //服务商名称
                        row.CreateCell(index++).SetCellValue(item.FuWuShangOrgName);
                        //辖区市
                        row.CreateCell(index++).SetCellValue(item.XiaQuShi);
                        //辖区县
                        row.CreateCell(index++).SetCellValue(item.XiaQuXian);
                        //企业名称
                        row.CreateCell(index++).SetCellValue(item.OrgName);

                        //提交备案时间
                        string CancelRecordDateTimeStr = "";
                        if (item.CancelRecordDateTime.HasValue)
                        {
                            CancelRecordDateTimeStr = item.CancelRecordDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        row.CreateCell(index++).SetCellValue(CancelRecordDateTimeStr);
                        //企业名称
                        row.CreateCell(index++).SetCellValue(item.CancellationReason);

                        for (int contInx = 0; contInx < index; contInx++)
                        {
                            row.Cells[contInx].CellStyle = contentStyle;
                            //if (content_index == 0)
                            //{
                            //    sheet.AutoSizeColumn(contInx, true);
                            //}
                        }
                    }

                    //表格样式
                    DefaultStyle(sheet, cellTitleArry);
                }

                //上传
                var extension = "zip";
                fileName += ".xlsx";
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
                    fileDto.Data = Compress(ms.ToArray());
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
            catch (Exception ex)
            {
                LogHelper.Error("生成导出取消备案车辆档案文件上传出错" + ex.Message, ex);
                return null;
            }
        }

        #endregion


        #region 查询取消备案车辆档案列表数据集

        public IEnumerable<CancelRecordVehicleDto> GetCancelRecordVehicleDataList(QueryData dto)
        {
            try
            {
                QueryCancelRecordVehicleDto searchDto =
                    JsonConvert.DeserializeObject<QueryCancelRecordVehicleDto>(JsonConvert.SerializeObject(dto.data));

                if (searchDto == null)
                {
                    return new List<CancelRecordVehicleDto>();
                }

                Expression<Func<CheLiang, bool>> carExp = x => x.SYS_ChuangJianShiJian.HasValue;
                Expression<Func<OrgBaseInfo, bool>> orgExp = x => x.SYS_XiTongZhuangTai == 0;
                Expression<Func<CheLiangVideoZhongDuanConfirm, bool>>
                    confirmExp = x => x.SYS_ChuangJianShiJian.HasValue;
                Expression<Func<FuWuShang, bool>> fwsExp = x => x.SYS_XiTongZhuangTai == 0;
                //车牌号
                if (!string.IsNullOrWhiteSpace(searchDto.ChePaiHao))
                {
                    carExp = carExp.And(x => x.ChePaiHao.Contains(searchDto.ChePaiHao.Trim()));
                }

                //车牌颜色
                if (!string.IsNullOrWhiteSpace(searchDto.ChePaiYanSe))
                {
                    carExp = carExp.And(x => x.ChePaiYanSe == searchDto.ChePaiYanSe.Trim());
                }

                //取消备案筛选开始时间
                if (searchDto.BeginDate.HasValue)
                {
                    confirmExp = confirmExp.And(x => x.SYS_ZuiJinXiuGaiShiJian >= searchDto.BeginDate);
                }

                //取消备案筛选结束时间
                if (searchDto.EndDate.HasValue)
                {
                    DateTime endDateTime = searchDto.EndDate.Value.Date.AddDays(1);
                    confirmExp = confirmExp.And(x => x.SYS_ZuiJinXiuGaiShiJian < endDateTime);
                }

                //车辆种类
                if (searchDto.CheLiangZhongLei.HasValue)
                {
                    carExp = carExp.And(x => x.CheLiangZhongLei == searchDto.CheLiangZhongLei);
                }

                //辖区市
                if (!string.IsNullOrWhiteSpace(searchDto.XiaQuShi))
                {
                    carExp = carExp.And(x => x.XiaQuShi == searchDto.XiaQuShi);
                }

                //辖区县
                if (!string.IsNullOrWhiteSpace(searchDto.XiaQuXian))
                {
                    carExp = carExp.And(x => x.XiaQuXian == searchDto.XiaQuXian);
                }



                //只找取消备案的车
                confirmExp = confirmExp.And(x => x.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.取消备案);

                

                var list = from a in _cheLiangXinXiRepository.GetQuery(carExp)
                           join b in _orgBaseInfoRepository.GetQuery(orgExp) on a.YeHuOrgCode equals b.OrgCode
                           //join c in _fuWuShangRepository.GetQuery(fwsExp) on a.FuWuShangOrgCode equals c.OrgCode
                           join d in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(confirmExp) on a.Id.ToString() equals d
                               .CheLiangId
                           select new CancelRecordVehicleDto
                           {
                               Id = a.Id,
                               ChePaiHao = a.ChePaiHao,
                               ChePaiYanSe = a.ChePaiYanSe,
                               CheLiangZhongLei = a.CheLiangZhongLei,
                               FuWuShangOrgCode = a.FuWuShangOrgCode,
                               FuWuShangOrgName = "",
                               XiaQuShi = a.XiaQuShi,
                               XiaQuXian = a.XiaQuXian,
                               OrgCode = b.OrgCode,
                               OrgName = b.OrgName,
                               CancelRecordDateTime = d.SYS_ZuiJinXiuGaiShiJian,
                               CancellationReason = d.NeiRong
                           };

                
                //服务商名称
                if (!string.IsNullOrWhiteSpace(searchDto.FuWuShangName))
                {
                    list = list.Where(x => x.FuWuShangOrgName == searchDto.FuWuShangName.Trim());
                }

                //企业名称
                if (!string.IsNullOrWhiteSpace(searchDto.OrgName))
                {
                    list = list.Where(x => x.OrgName == searchDto.OrgName.Trim());
                }

               

                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询取消备案车辆数据出错{ex.Message}", ex);
                return new List<CancelRecordVehicleDto>();
            }
        }


        #endregion


        #endregion

        #region 基本信息

        #region 新增车辆信息

        public ServiceResult<object> Create(CheLiangAddDto dto)
        {
            return ExecuteCommandStruct<object>(() =>
            {
                var result = new ServiceResult<object>();
                UserInfoDtoNew userInfo = GetUserInfo();
                CheLiang e = _cheLiangXinXiRepository.GetQuery(s =>
                        s.SYS_XiTongZhuangTai == 0 && s.ChePaiHao == dto.ChePaiHao && s.ChePaiYanSe == dto.ChePaiYanSe)
                    .FirstOrDefault();
                if (e != null)
                {
                    string errorMessage = string.Format("{0}车辆档案记录已存在，不允许重复添加", e.ChePaiHao);
                    return new ServiceResult<object>() { ErrorMessage = errorMessage, StatusCode = 2 };
                }

                CheLiang cInfo = _cheLiangXinXiRepository
                    .GetQuery(s => s.SYS_XiTongZhuangTai == 0 && s.CheJiaHao == dto.CheJiaHao).FirstOrDefault();
                //if (cInfo != null)
                //{
                //    string errorMessage = "车架号已存在，不允许重复添加";
                //    return new ServiceResult<object>() { ErrorMessage = errorMessage, StatusCode = 2 };
                //}
                if (string.IsNullOrEmpty(dto.Id))
                {
                    dto.Id = Guid.NewGuid().ToString();
                }

                dto.ChuangJianRenOrgCode = userInfo.OrganizationCode;
                dto.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                dto.YeWuBanLiZhuangTai = 1;
                var ZuiJingDingWei = new CheLiangDingWeiXinXi()
                {
                    Id = Guid.NewGuid(),
                    RegistrationNo = dto.ChePaiHao,
                    RegistrationNoColor = dto.ChePaiYanSe,
                };
                SetCreateSYSInfo(ZuiJingDingWei, userInfo);
                //企业添加车辆限制只能在自己辖区内
                //if (userInfo.OrganizationType ==(int)OrganizationType.企业&& !string.IsNullOrWhiteSpace(userInfo.OrganProvince) && !string.IsNullOrWhiteSpace(userInfo.OrganCity))
                //{
                //    dto.XiaQuSheng = userInfo.OrganProvince;
                //    dto.XiaQuShi = userInfo.OrganCity;
                //    dto.XiaQuXian = userInfo.OrganDistrict;
                //}
                Mapper.CreateMap<CheLiangAddDto, CheLiang>();
                var cheLiang = Mapper.Map<CheLiang>(dto);
                SetCreateSYSInfo(cheLiang, userInfo);

                var cheLiangZuZhiXinXiList = new List<CheLiangZuZhiXinXi>();
                cheLiangZuZhiXinXiList.Add(new CheLiangZuZhiXinXi
                {
                    Id = Guid.NewGuid(),
                    CheLiangId = dto.Id,
                    OrgCode = dto.YeHuOrgCode,
                    OrgType = dto.YeHuOrgType,
                    SYS_ShuJuLaiYuan = "车辆档案新增"
                });
                if (userInfo.OrganizationCode != dto.YeHuOrgCode)
                {
                    cheLiangZuZhiXinXiList.Add(new CheLiangZuZhiXinXi
                    {
                        Id = Guid.NewGuid(),
                        CheLiangId = dto.Id,
                        OrgCode = userInfo.OrganizationCode,
                        OrgType = userInfo.OrganizationType,
                        SYS_ShuJuLaiYuan = "车辆档案新增"
                    });
                }

                if (!string.IsNullOrEmpty(dto.CheDuiOrgCode) && userInfo.OrganizationCode != dto.CheDuiOrgCode)
                {
                    cheLiangZuZhiXinXiList.Add(new CheLiangZuZhiXinXi
                    {
                        Id = Guid.NewGuid(),
                        CheLiangId = dto.Id,
                        OrgCode = dto.CheDuiOrgCode,
                        OrgType = dto.CheDuiOrgType,
                        SYS_ShuJuLaiYuan = "车辆档案新增"
                    });
                }

                cheLiangZuZhiXinXiList.ForEach(s => SetCreateSYSInfo(s, userInfo));

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    cheLiang.FuWuShangOrgCode = string.Empty;
                    cheLiang.CreateCompanyCode = dto.FuWuShangOrgCode;
                    cheLiang.YunZhengZhuangTai = "营运";
                    _cheLiangXinXiRepository.Add(cheLiang);
                    _cheLiangZuZhiXinXiRepository.BatchInsert(cheLiangZuZhiXinXiList.ToArray());
                    _cheLiangDingWeiXinXiRepository.Add(ZuiJingDingWei);

                    if (userInfo?.OrganizationType == (int)OrganizationType.本地服务商)
                    {
                        var vehicleVideoZhongDuanConfirm = new CheLiangVideoZhongDuanConfirm
                        {
                            Id = Guid.NewGuid(),
                            CheLiangId = cheLiang.Id.ToString(),
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常,
                        };
                        _cheLiangVideoZhongDuanConfirmRepository.Add(vehicleVideoZhongDuanConfirm);
                    }

                    var flag = uow.CommitTransaction() > 0;
                    if (flag)
                    {
                        result.Data = dto.Id;

                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                            ActionName = nameof(Create),
                            BizOperType = OperLogBizOperType.ADD,
                            ShortDescription = "车辆档案新增：" + dto?.ChePaiHao + "[" + dto.ChePaiYanSe + "]",
                            OperatorName = userInfo.UserName,
                            OldBizContent = JsonConvert.SerializeObject(dto),
                            OperatorID = userInfo.Id,
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        });
                    }
                    else
                    {
                        return new ServiceResult<object>() { ErrorMessage = "保存失败", StatusCode = 2 };
                    }
                }

                return result;
            });
        }

        #endregion

        #region 更新车辆信息

        public ServiceResult<bool> Update(CheLiangAddDto dto)
        {
            try
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<bool> {StatusCode = 2, ErrorMessage = "获取登录信息失败,请重新登录"};
                }

                var cheLiang = Mapper.Map<CheLiang>(dto);
                var preEntity = _cheLiangXinXiRepository.GetByKey(cheLiang.Id);
                var vehicleConfirm = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.CheLiangId == dto.Id && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();

                //车辆修改无限制，但是修改后  车辆备案状态和GPS审核状态均变为待提交状态
                //if (preEntity.CheLiangLeiXing == (int) CheLiangZhongLei.客运班车 || preEntity.CheLiangLeiXing ==
                //                                                             (int) CheLiangZhongLei.旅游包车
                //                                                             || preEntity.CheLiangLeiXing ==
                //                                                             (int) CheLiangZhongLei.危险货运
                //                                                             || preEntity.CheLiangLeiXing ==
                //                                                             (int) CheLiangZhongLei.重型货车)
                //{
                //    if (vehicleConfirm != null &&
                //        vehicleConfirm?.BeiAnZhuangTai == (int) ZhongDuanBeiAnZhuangTai.通过备案 &&
                //        preEntity.GPSAuditStatus == (int) GPSAuditStatus.通过备案)
                //    {
                //        return new ServiceResult<bool> {StatusCode = 2, ErrorMessage = "通过备案以及GPS审核通过的车不能修改车辆信息"};
                //    }
                //}
                //else
                //{
                //    if (preEntity.GPSAuditStatus == (int) GPSAuditStatus.通过备案)
                //    {
                //        return new ServiceResult<bool> {StatusCode = 2, ErrorMessage = "GPS审核通过的车辆不能修改车辆信息"};
                //    }
                //}
                var vehiclePartnership = _vehiclePartnershipBinding.GetQuery(x => x.SYS_XiTongZhuangTai == 0 &&
                                                                                  x.LicensePlateNumber ==
                                                                                  preEntity.ChePaiHao &&
                                                                                  x.LicensePlateColor ==
                                                                                  preEntity.ChePaiYanSe).FirstOrDefault();
                var result = new ServiceResult<bool>();
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var cheLiangYeHu = _cheLiangYeHuRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == cheLiang.YeHuOrgCode)
                        .FirstOrDefault();
                    string oldData = JsonConvert.SerializeObject(preEntity);
                    //比较修改差异
                    Mapper.CreateMap<CheLiang, CheLiangAddDto>();
                    CheLiangAddDto oldModel = Mapper.Map<CheLiangAddDto>(preEntity);
                    List<LogUpdateValueDto> updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, dto, true);

                    preEntity.CheLiangLeiXing = cheLiang.CheLiangLeiXing;
                    preEntity.CheLiangZhongLei = cheLiang.CheLiangZhongLei;
                    preEntity.CheZaiDianHua = cheLiang.CheZaiDianHua;
                    preEntity.XiaQuSheng = cheLiang.XiaQuSheng;
                    preEntity.XiaQuShi = cheLiang.XiaQuShi;
                    preEntity.XiaQuXian = cheLiang.XiaQuXian;
                    preEntity.YunZhengZhuangTai = cheLiang.YunZhengZhuangTai;
                    preEntity.CheJiaHao = cheLiang.CheJiaHao;
                    preEntity.SuoShuPingTai = cheLiang.SuoShuPingTai;
                    preEntity.YunYingZhengHao = cheLiang.YunYingZhengHao;
                    preEntity.NianShenZhuangTai = cheLiang.NianShenZhuangTai;
                    preEntity.ZuiJinXiuGaiRenOrgCode = userInfoNew.OrganizationCode;
                    preEntity.SYS_ZuiJinXiuGaiRen = userInfoNew.UserName;
                    preEntity.SYS_ZuiJinXiuGaiRenID = userInfoNew.Id;
                    preEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                    preEntity.Remark = cheLiang.Remark;
                    if (vehiclePartnership != null && cheLiang.YeHuOrgCode != vehiclePartnership.EnterpriseCode)
                    {
                        vehiclePartnership.ZhuangTai = (int)VehicleCooperationStatus.取消合作;
                        vehiclePartnership.EnterpriseCode = cheLiang.YeHuOrgCode;
                        vehiclePartnership.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehiclePartnership.SYS_ZuiJinXiuGaiRen = userInfoNew.UserName;
                        vehiclePartnership.SYS_ZuiJinXiuGaiRenID = userInfoNew.Id;
                        _vehiclePartnershipBinding.Update(vehiclePartnership);
                        cheLiang.FuWuShangOrgCode = string.Empty;
                    }
                    //更换业户抽查未填报数据移到新业户
                    if (cheLiangYeHu != null&& preEntity.YeHuOrgCode != cheLiang.YeHuOrgCode)
                    {
                        UpdateSpoCheckYeHu(cheLiang, cheLiangYeHu.OrgName);
                    }
                    preEntity.YeHuOrgCode = cheLiang.YeHuOrgCode;
                    preEntity.FuWuShangOrgCode = cheLiang.FuWuShangOrgCode;
                    preEntity.GPSAuditStatus = (int) GPSAuditStatus.待审核;
                    _cheLiangXinXiRepository.Update(preEntity);

                    if (vehicleConfirm != null)
                    {
                        vehicleConfirm.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRenID = userInfoNew.Id;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRen = userInfoNew.UserName;
                        vehicleConfirm.BeiAnZhuangTai = (int) ZhongDuanBeiAnZhuangTai.待提交;
                        _cheLiangVideoZhongDuanConfirmRepository.Update(vehicleConfirm);
                    }
                    result.Data = uow.CommitTransaction() > 0;
                    result.StatusCode = result.Data ? 0 : 2;
                    result.ErrorMessage = result.Data ? "" : "修改数据失败";

                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                        ActionName = nameof(Update),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = "车辆档案基础信息修改：" + dto?.ChePaiHao + "[" + dto.ChePaiYanSe + "]",
                        OperatorName = userInfoNew.UserName,
                        OldBizContent = oldData,
                        NewBizContent = JsonConvert.SerializeObject(preEntity),
                        OperatorID = userInfoNew.Id,
                        OperatorOrgCode = userInfoNew.OrganizationCode,
                        OperatorOrgName = userInfoNew.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                    });

                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新车辆信息出错" + ex.Message, ex);
                return new ServiceResult<bool> {ErrorMessage = "更新出错", StatusCode = 2};
            }
        }

        #endregion

        public ServiceResult<bool> ModifyApproval(CheLiangAddDto dto)
        {
            try
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败,请重新登录" };
                }

                if (string.IsNullOrWhiteSpace(dto.Id))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
                }

                var cheLiangId = new Guid(dto.Id);
                var carModel = _cheLiangXinXiRepository.GetQuery(x => x.Id == cheLiangId && x.SYS_XiTongZhuangTai == 0)
                    .FirstOrDefault();
                if (carModel == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "找不到对应的车辆信息", Data = false };
                }

                var result = new ServiceResult<bool>();
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var cheLiang = Mapper.Map<CheLiang>(dto);
                    var preEntity = _cheLiangXinXiRepository.GetByKey(cheLiang.Id);
                    var oldData = JsonConvert.SerializeObject(preEntity);
                    //比较修改差异
                    Mapper.CreateMap<CheLiang, CheLiangAddDto>();
                    var oldModel = Mapper.Map<CheLiangAddDto>(preEntity);
                    var updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, dto, true);
                    //修改车辆审核状态
                    preEntity.ManualApprovalStatus = cheLiang.ManualApprovalStatus;
                    _cheLiangXinXiRepository.Update(preEntity);

                    result.Data = uow.CommitTransaction() > 0;
                    result.StatusCode = result.Data ? 0 : 2;
                    result.ErrorMessage = result.Data ? "" : "车辆审核失败";

                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                        ActionName = nameof(Update),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = "车辆档案审核状态修改：" + dto?.ChePaiHao + "[" + dto.ChePaiYanSe + "]",
                        OperatorName = userInfoNew.UserName,
                        OldBizContent = oldData,
                        NewBizContent = JsonConvert.SerializeObject(preEntity),
                        OperatorID = userInfoNew.Id,
                        OperatorOrgCode = userInfoNew.OrganizationCode,
                        OperatorOrgName = userInfoNew.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                    });

                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆审核出错" + ex.Message, ex);
                return new ServiceResult<bool> { ErrorMessage = "审核出错", StatusCode = 2 };
            }
        }

        #region 删除车辆信息

        public ServiceResult<bool> Delete(List<Guid> ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {

                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败,请重新登录" };
                }
                var result = new ServiceResult<bool>();
                if (ids.Count < 1)
                {
                    return new ServiceResult<bool>() { Data = false };
                }

                var list = _cheLiangXinXiRepository.GetQuery(u => ids.Contains(u.Id) && u.SYS_XiTongZhuangTai == 0)
                    .ToList();

                if (list.Count < 1)
                {
                    return new ServiceResult<bool>() { Data = false };
                }
                //List<int> beiAnZhuangTai = new List<int>
                //    {(int) ZhongDuanBeiAnZhuangTai.通过备案, (int) ZhongDuanBeiAnZhuangTai.未审核};
                var cheLiangIdList = list.Select(x => x.Id.ToString()).ToList();

                var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == 0).ToList();

                //var heChaList = _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x =>
                //    x.SYS_XiTongZhuangTai == 0 && cheLiangIdList.Contains(x.CheLiangId)).ToList();
                var ChePaiHaoListStr = "";
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    foreach (var d in list)
                    {
                        //作废基本信息
                        d.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _cheLiangXinXiRepository.Update(d);
                        ChePaiHaoListStr += d?.ChePaiHao + "[" + d.ChePaiYanSe + "]、";

                        var cheLiangId = d.Id.ToString();
                        var vehicleBinding = vehicleBindingList.Find(x =>
                            x.LicensePlateNumber == d.ChePaiHao && x.LicensePlateColor == d.ChePaiYanSe);
                        if (vehicleBinding != null)
                        {
                            vehicleBinding.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            vehicleBinding.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            vehicleBinding.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            vehicleBinding.ZhuangTai = (int) VehicleCooperationStatus.取消合作;
                            _vehiclePartnershipBinding.Update(vehicleBinding);
                        }


                        //作废终端信息
                        //删除档案中车辆对应的GPS终端信息
                        var dangAnGpsInfoList = _cheLiangGPSZhongDuanXinXiRepository
                            .GetQuery(x => x.CheLiangId == cheLiangId && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnGpsInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, userInfo);
                            x.SYS_XiTongBeiZhu = "车辆数据删除";
                            _cheLiangGPSZhongDuanXinXiRepository.Update(x);
                        });
                        //删除档案中车辆对应的智能视频终端信息
                        var dangAnVideoInfoList = _cheLiangVideoZhongDuanXinXiRepository
                            .GetQuery(x => x.CheLiangId == cheLiangId && x.SYS_XiTongZhuangTai == 0).ToList();
                        dangAnVideoInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, userInfo);
                            x.SYS_XiTongBeiZhu = "车辆数据删除";
                            _cheLiangVideoZhongDuanXinXiRepository.Update(x);
                        });
                        //删除档案中车辆对应的最近定位信息
                        var dwInfoList = _cheLiangDingWeiXinXiRepository.GetQuery(x =>
                            x.RegistrationNo == d.ChePaiHao && x.RegistrationNoColor == d.ChePaiYanSe &&
                            x.SYS_XiTongZhuangTai == 0).ToList();
                        dwInfoList.ForEach(x =>
                        {
                            SetDeleteSYSInfo(x, userInfo);
                            x.SYS_XiTongBeiZhu = "车辆数据删除";
                            _cheLiangDingWeiXinXiRepository.Update(x);
                        });
                        //服务商车辆取消备案
                        var fwsCheliangList = _fuWuShangCheLiangRepository.GetQuery(x =>
                                x.ChePaiHao == d.ChePaiHao && x.ChePaiYanSe == d.ChePaiYanSe &&
                                x.SYS_XiTongZhuangTai == 0)
                            .ToList();
                        fwsCheliangList.ForEach(x =>
                        {
                            SetUpdateSYSInfo(x, x, userInfo);
                            x.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.取消备案;
                            x.SYS_XiTongBeiZhu = "车辆档案删除取消备案";
                            _fuWuShangCheLiangRepository.Update(x);
                        });


                    }

                    result.Data = uow.CommitTransaction() > 0;
                    result.StatusCode = result.Data ? 0 : 2;
                    result.ErrorMessage = result.Data ? "" : "删除失败";
                }

                OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                {
                    SystemName = OprateLogHelper.GetSysTemName(),
                    ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                    ActionName = nameof(Delete),
                    BizOperType = OperLogBizOperType.DELETE,
                    ShortDescription = "车辆档案删除：" + ChePaiHaoListStr.TrimEnd('、'),
                    OperatorName = userInfo.UserName,
                    OldBizContent = JsonConvert.SerializeObject(ids),
                    OperatorID = userInfo.Id,
                    OperatorOrgCode = userInfo.OrganizationCode,
                    OperatorOrgName = userInfo.OrganizationName,
                    SysID = SysId,
                    AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                });

                return result;
            });
        }

        #endregion

        #region 修改车辆状态

        public ServiceResult<bool> UpdateState(CheLiang model, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                var result = new ServiceResult<bool>();
                using (var u = new UnitOfWork())
                {
                    u.BeginTransaction();
                    var list = _cheLiangXinXiRepository.GetQuery(s => s.Id == model.Id && s.SYS_XiTongZhuangTai == 0)
                        .ToList();
                    foreach (var d in list)
                    {
                        d.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        d.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        d.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        _cheLiangXinXiRepository.Update(d);
                    }

                    result.Data = u.CommitTransaction() > 0;
                    result.StatusCode = result.Data ? 0 : 2;
                    result.ErrorMessage = result.Data ? "" : "修改状态失败";
                }

                return result;
            });
        }

        #endregion

        #region 获取车辆基本信息

        public ServiceResult<object> GetVehicleBasicInfo(Guid CheLiangId)
        {

            var jsyList = _jiaShiYuanRepository.GetQuery(x =>
                x.SYS_XiTongZhuangTai == 0 && x.WorkingStatus == (int)JiaShiYuanWorkStatus.Hire &&
                x.CheLiangId == CheLiangId.ToString());
            var siJiXingMing = "";
            foreach (var item in jsyList)
            {
                siJiXingMing += item.Name + "，";
            }

            siJiXingMing = siJiXingMing.TrimEnd(',');
            var result =
                from t in _cheLiangXinXiRepository.GetQuery(p => p.Id == CheLiangId && p.SYS_XiTongZhuangTai == 0)
                join c in _cheLiangYeHuRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                    on t.YeHuOrgCode equals c.OrgCode
                join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0)
                    on t.FuWuShangOrgCode equals fws.OrgCode into table1
                from t1 in table1.DefaultIfEmpty()
                select new
                {
                    t.Id,
                    t.ChePaiHao,
                    t.ChePaiYanSe,
                    t.CheLiangLeiXing,
                    t.CheLiangZhongLei,
                    t.CheZaiDianHua,
                    t.XiaQuSheng,
                    t.XiaQuShi,
                    t.XiaQuXian,
                    t.YunZhengZhuangTai,
                    t.NianShenZhuangTai,
                    t.SuoShuPingTai,
                    t.YunYingZhengHao,
                    t.CheJiaHao,
                    CheZhuLeiXing = t.YeHuOrgType,
                    QiYeMingCheng = t.YeHuOrgType == (int)OrganizationType.企业 ? c.OrgName : "",
                    c.JingYingXuKeZhengHao,
                    LianXiFangShi = "",
                    t.Remark,
                    t.YeHuOrgCode,
                    t.CheDuiOrgCode,
                    SiJiXingMing = siJiXingMing,
                    FuWuShangMingCheng = t1.OrgName,
                    FuWuShangOrgCode = t1.OrgCode,
                };

            return new ServiceResult<object>() { Data = result.FirstOrDefault() };
        }

        #endregion

        #region 获取车辆基本信息-新

        public ServiceResult<QueryResult> GetVehicleBasicInfoNew(QueryData queryData)
        {
            CheLiang dto = JsonConvert.DeserializeObject<CheLiang>(JsonConvert.SerializeObject(queryData.data));
            var queryresult = new ServiceResult<QueryResult>();
            var data = new QueryResult();
            try
            {
                Expression<Func<CheLiang, bool>> cheliangExp = t => t.SYS_XiTongZhuangTai == 0;
                //Id
                if (dto.Id != new Guid())
                {
                    cheliangExp = cheliangExp.And(p => p.Id == dto.Id);
                }
                //车牌号
                else if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    if (dto.ChePaiHao.Length < 3)
                    {
                        queryresult.StatusCode = 2;
                        queryresult.ErrorMessage = "车牌号码不能小于三位";
                        return queryresult;
                    }

                    dto.ChePaiHao = Regex.Replace(dto.ChePaiHao, @"\s", "");
                    cheliangExp = cheliangExp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.ToUpper()));
                    //车牌颜色
                    if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                    {
                        cheliangExp = cheliangExp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe);
                    }
                }
                else
                {
                    queryresult.StatusCode = 2;
                    queryresult.ErrorMessage = "车牌号码不能小于三位";
                    return queryresult;
                }

                DateTime compareTime = DateTime.Now.AddMinutes(CommonHelper.CheLiangZaiXianExpireTime * -1);
                var result = from t in _cheLiangXinXiRepository.GetQuery(cheliangExp)

                             join clex in _cheLiangExRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 on t.Id.ToString() equals clex.CheLiangId into temp0
                             from t0 in temp0.DefaultIfEmpty()

                             join y in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 on t.YeHuOrgCode equals y.OrgCode into temp1
                             from t1 in temp1.DefaultIfEmpty()

                             join f in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 on t.FuWuShangOrgCode equals f.OrgCode into temp2
                             from t2 in temp2.DefaultIfEmpty()

                             join cheDui in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 on t.CheDuiOrgCode equals cheDui.OrgCode into temp3
                             from t3 in temp3.DefaultIfEmpty()

                             join zdxx in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 on t.Id.ToString() equals zdxx.CheLiangId into temp4
                             from t4 in temp4.DefaultIfEmpty()

                                 // join dwxx in _cheLiangDingWeiXinXiRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                                 //on new { RegistrationNo = t.ChePaiHao, RegistrationNoColor = t.ChePaiYanSe } equals new { dwxx.RegistrationNo, dwxx.RegistrationNoColor } into temp5
                                 // from dw in temp5.DefaultIfEmpty()

                             select new
                             {
                                 t.Id,
                                 t.ChePaiHao,
                                 t.ChePaiYanSe,
                                 CheLiangLeiXing = t.CheLiangLeiXing.HasValue
                                     ? ((CheLiangLeiXing)t.CheLiangLeiXing).ToString()
                                     : "",
                                 CheLiangZhongLei = t.CheLiangZhongLei.HasValue
                                     ? ((CheLiangZhongLei)t.CheLiangZhongLei).ToString()
                                     : "",
                                 t.CheZaiDianHua,
                                 t.XiaQuSheng,
                                 t.XiaQuShi,
                                 t.XiaQuXian,
                                 YeHuDaiMa = t1.OrgCode,
                                 YeHuMingCheng = t1.OrgName,
                                 OperatorCode = t2.OrgCode,
                                 OperatorName = t2.OrgName,
                                 CheDuiBianHao = t3.OrgCode,
                                 CheDuiMingCheng = t3.OrgName,

                                 t.YunYingZhengHao,
                                 t.NianShenZhuangTai,
                                 t0.JingYingFanWei,
                                 t0.CheLiangBiaoZhiId,
                                 CheShenYanSe = t0.CheShenYanSe.HasValue ? ((CheShenYanSe)t0.CheShenYanSe).ToString() : "",
                                 t0.CheLiangNengLi,
                                 t0.XingShiZhengHao,
                                 t0.XingShiZhengDiZhi,
                                 t0.XingShiZhengYouXiaoQi,
                                 t0.XingShiZhengNianShenRiQi,
                                 t0.CheLiangBaoXiaoZhongLei,
                                 t0.CheLiangBaoXiaoDaoJieZhiRiQi,
                                 t0.XingShiZhengTiXingTianShu,
                                 t0.XingShiZhengDengJiRiQi,
                                 t0.XingShiZHengSaoMiaoJianId,
                                 RanLiao = t0.RanLiao.HasValue ? ((RanLiao)t0.RanLiao).ToString() : "",
                                 t0.PaiQiLiang,
                                 t0.ZongZhiLiang,
                                 t0.ZhengBeiZhiLiang,
                                 t0.HeZaiZhiLiang,
                                 t0.FaDongJiHao,
                                 t.CheJiaHao,
                                 t0.CheGao,
                                 t0.CheChang,
                                 t0.CheKuan,
                                 ZhuangTai = t0.ZhuangTai.HasValue ? ((CheLiangZhuangTai)t0.ZhuangTai).ToString() : "",
                                 t0.XingHao,
                                 t0.JiShuDengJi,
                                 t0.AnZhuangDengJi,
                                 t0.ErWeiRiQi,
                                 t0.XiaCiErWeiRiQi,
                                 t0.ShenYanYouXiaoQi,
                                 t0.DaoLuYunShuZhengHao,
                                 t0.DaoLuYunShuZhengYouXiaoQi,
                                 t0.DaoLuYunShuZhengNianShenRiQi,
                                 t0.DaoLuYunShuZhengTiXingTianShu,
                                 t0.ZuoXing,
                                 t0.ZuoWei,
                                 t0.DunWei,
                                 t0.ChuChangRiQi,
                                 t0.CheZhouShu,
                                 t0.JieBoCheLiang,
                                 t0.HuoWuMingCheng,
                                 t0.ShiFaDi,
                                 t0.QiFaDi,
                                 t0.ShiFaZhan,
                                 t0.QiDianZhan,
                                 t0.HuoWuDunWei,
                                 t0.ChuangJianRenOrgCode,
                                 t0.ZuiJinXiuGaiRenOrgCode,

                                 ZhongDuanLeiXing = t4.ZhongDuanLeiXing.HasValue
                                     ? ((ZhongDuanLeiXing)t4.ZhongDuanLeiXing).ToString()
                                     : "",
                                 t4.ShengChanChangJia,
                                 t4.ChangJiaBianHao,
                                 t4.SheBeiXingHao,
                                 t4.ZhongDuanBianMa,
                                 t4.SIMKaHao,
                                 t4.ZhongDuanMDT,
                                 t4.M1,
                                 t4.IA1,
                                 t4.IC1,
                                 ShiFouAnZhuangShiPinZhongDuan = t4 == null ? 0 : t4.ShiFouAnZhuangShiPinZhongDuan,
                                 VideoServiceKind = t4 == null ? 0 : t4.ShiPingChangShangLeiXing,
                                 CameraSelected = t4.ShiPinTouAnZhuangXuanZe,
                                 ShiPinTouGeShu = t4 == null ? 0 : t4.ShiPinTouGeShu,
                                 ShiFouZaiXian =
                                     (t.ZaiXianZhuangTai ?? 0) ==
                                     (int)ZaiXianZhuangTai.在线, //dw.LatestGpsTime >= compareTime ? true : false,
                                 t.SYS_ChuangJianShiJian
                             };

                data.totalcount = result.Count();
                data.items = result.Distinct().OrderByDescending(u => u.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                return new ServiceResult<QueryResult>() { Data = data };
            }
            catch (Exception e)
            {
                queryresult.StatusCode = 2;
                queryresult.ErrorMessage = e.Message;
                LogHelper.Error($"查询车辆基础信息异常," + e);
                return queryresult;
            }

        }

        #endregion

        #region 获取车辆基本信息列表

        public ServiceResult<QueryResult> QueryVehicleBasicInfoList(QueryData queryData)
        {
            CheLiangSearchDto dto =
                JsonConvert.DeserializeObject<CheLiangSearchDto>(JsonConvert.SerializeObject(queryData.data));
            var queryresult = new ServiceResult<QueryResult>();
            var data = new QueryResult();
            try
            {
                var userInfo = GetUserInfo();
                Expression<Func<CheLiang, bool>> cheliangExp = t => t.SYS_XiTongZhuangTai == 0;
                Expression<Func<OrgBaseInfo, bool>> yehuExp = t => t.SYS_XiTongZhuangTai == 0 && t.OrgType == 2;
                Expression<Func<OrgBaseInfo, bool>> fuWuShangExp = t => t.SYS_XiTongZhuangTai == 0 && t.OrgType == 5;
                //车牌号码
                if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    if (dto.ChePaiHao.Length < 3)
                    {
                        throw new Exception("车牌号码不能小于三位");
                    }

                    dto.ChePaiHao = Regex.Replace(dto.ChePaiHao, @"\s", "");
                    cheliangExp = cheliangExp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.ToUpper()));

                }

                //车牌颜色
                if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    cheliangExp = cheliangExp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe);
                }

                if (!string.IsNullOrWhiteSpace(dto.YeHuMingCheng))
                {
                    yehuExp = yehuExp.And(p => p.OrgName.Contains(dto.YeHuMingCheng));
                }

                switch (userInfo.OrganizationType)
                {
                    case (int)OrganizationType.市政府:
                        if (!string.IsNullOrWhiteSpace(userInfo.OrganCity))
                        {
                            cheliangExp = cheliangExp.And(p => p.XiaQuShi == userInfo.OrganCity);
                        }
                        else
                        {
                            throw new Exception("辖区市为空！");
                        }

                        break;
                    case (int)OrganizationType.县政府:
                        if (!string.IsNullOrWhiteSpace(userInfo.OrganDistrict))
                        {
                            cheliangExp = cheliangExp.And(p => p.XiaQuXian == userInfo.OrganDistrict);
                        }
                        else
                        {
                            throw new Exception("辖区县为空！");
                        }

                        break;
                    case (int)OrganizationType.平台运营商:
                        break;
                    case (int)OrganizationType.企业:
                    case (int)OrganizationType.个体户:
                        if (!string.IsNullOrWhiteSpace(userInfo.OrganizationCode))
                        {
                            if (IsGuanLiYuanRoleCode(userInfo.RoleCode))
                            {
                                cheliangExp = cheliangExp.And(p => p.YeHuOrgCode == userInfo.OrganizationCode);
                            }
                            else
                            {
                                return GetVehicleBaseInfoByYongHu(cheliangExp, userInfo.Id.ToString(), queryData.page,
                                    queryData.rows);
                            }
                        }
                        else
                        {
                            throw new Exception("组织编号为空！");
                        }

                        break;
                    case (int)OrganizationType.本地服务商:
                        if (!string.IsNullOrWhiteSpace(userInfo.OrganizationCode))
                        {
                            if (IsGuanLiYuanRoleCode(userInfo.RoleCode))
                            {
                                cheliangExp = cheliangExp.And(p => p.FuWuShangOrgCode == userInfo.OrganizationCode);
                            }
                            else
                            {
                                return GetVehicleBaseInfoByYongHu(cheliangExp, userInfo.Id.ToString(), queryData.page,
                                    queryData.rows);
                            }
                        }
                        else
                        {
                            throw new Exception("组织编号为空！");
                        }

                        break;
                    default:
                        throw new Exception("组织类型不存在！");
                }

                var result =
                    from cheLiang in _cheLiangXinXiRepository.GetQuery(cheliangExp)
                    join yeHu in _orgBaseInfoRepository.GetQuery(yehuExp)
                        on cheLiang.YeHuOrgCode equals yeHu.OrgCode
                    join f in _orgBaseInfoRepository.GetQuery(fuWuShangExp)
                        on cheLiang.FuWuShangOrgCode equals f.OrgCode
                        into temp
                    from tmp in temp.DefaultIfEmpty()
                    select new
                    {
                        cheLiang.Id,
                        cheLiang.ChePaiHao,
                        cheLiang.ChePaiYanSe,
                        CheLiangLeiXing = cheLiang.CheLiangLeiXing.HasValue
                            ? ((CheLiangLeiXing)cheLiang.CheLiangLeiXing).ToString()
                            : "",
                        CheLiangZhongLei = cheLiang.CheLiangZhongLei.HasValue
                            ? ((CheLiangZhongLei)cheLiang.CheLiangZhongLei).ToString()
                            : "",
                        cheLiang.CheZaiDianHua,
                        cheLiang.XiaQuSheng,
                        cheLiang.XiaQuShi,
                        cheLiang.XiaQuXian,
                        YeHuDaiMa = yeHu.OrgCode,
                        YeHuMingCheng = yeHu.OrgName,
                        OperatorCode = tmp.OrgCode,
                        OperatorName = tmp.OrgName,
                        cheLiang.SYS_ChuangJianShiJian
                    };
                data.totalcount = result.Count();
                data.items = result.Distinct().OrderByDescending(u => u.SYS_ChuangJianShiJian)
                    .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                return new ServiceResult<QueryResult>() { Data = data };
            }
            catch (Exception e)
            {
                queryresult.StatusCode = 2;
                queryresult.ErrorMessage = e.Message;
                LogHelper.Error($"查询车辆基础信息异常," + e);
                return queryresult;
            }

        }

        #endregion

        #region 根据用户ID获取关联的车辆基本信息

        public ServiceResult<QueryResult> GetVehicleBaseInfoByYongHu(Expression<Func<CheLiang, bool>> cheliangExp,
            string id, int page, int rows)
        {
            var queryResult = new QueryResult();
            var result =
                from t in _cheLiangXinXiRepository.GetQuery(cheliangExp)
                join b in _yongHuCheLiangXinXiRepository.GetQuery(p => p.SYS_XiTongZhuangTai == 0 && p.SysUserId == id)
                    on t.Id.ToString() equals b.CheLiangId

                join y in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                    on t.YeHuOrgCode equals y.OrgCode into temp1
                from t1 in temp1.DefaultIfEmpty()
                join f in _orgBaseInfoRepository.GetQuery(s => s.SYS_XiTongZhuangTai == 0)
                    on t.FuWuShangOrgCode equals f.OrgCode
                    into temp2
                from t2 in temp2.DefaultIfEmpty()
                select new
                {
                    t.Id,
                    t.ChePaiHao,
                    t.ChePaiYanSe,
                    CheLiangLeiXing =
                        t.CheLiangLeiXing.HasValue ? ((CheLiangLeiXing)t.CheLiangLeiXing).ToString() : "",
                    CheLiangZhongLei = t.CheLiangZhongLei.HasValue
                        ? ((CheLiangZhongLei)t.CheLiangZhongLei).ToString()
                        : "",
                    t.CheZaiDianHua,
                    t.XiaQuSheng,
                    t.XiaQuShi,
                    t.XiaQuXian,
                    YeHuDaiMa = t1.OrgCode,
                    YeHuMingCheng = t1.OrgName,
                    OperatorCode = t2.OrgCode,
                    OperatorName = t2.OrgName,
                    t.SYS_ChuangJianShiJian,
                    BeiZhu = ""
                };
            queryResult.totalcount = result.Count();
            queryResult.items = result.Distinct().OrderByDescending(u => u.SYS_ChuangJianShiJian)
                .Skip((page - 1) * rows).Take(rows).ToList();
            return new ServiceResult<QueryResult>() { Data = queryResult };
        }

        #endregion

        #region 查询没有被当前用户订阅的车辆

        /// <summary>
        /// 查询没有被当前用户订阅的车辆
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryNotSubscribeVehicleBasicInfoList(QueryData queryData)
        {
            NotSubscribeCheLiangSearchDto dto =
                JsonConvert.DeserializeObject<NotSubscribeCheLiangSearchDto>(
                    JsonConvert.SerializeObject(queryData.data));
            dto.XiaQuXian = dto.XiaQuXian.Where(x => !string.IsNullOrEmpty(x)).ToList();
            dto.CheLiangZhongLei = dto.CheLiangZhongLei.Where(x => !string.IsNullOrEmpty(x)).ToList();
            UserInfoDtoNew userInfo = GetUserInfo();

            if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.ExtUserId))
            {
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "获取用户信息失败" };
            }

            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao) && dto.ChePaiHao.Length < 3)
            {
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "车牌号不能小于3位" };
            }

            #region 构建where语句

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserId", userInfo.ExtUserId));
            string cheLiangWhere = string.Empty, yeHuWhere = string.Empty, fuWuShangWhere = string.Empty;
            SqlParamHelper sqlParamHelper = new SqlParamHelper();
            sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "SYS_XiTongZhuangTai", 0);
            sqlParamHelper.AppendParameter(ref yeHuWhere, ref param, "yeHu", "SYS_XiTongZhuangTai", 0);
            sqlParamHelper.AppendParameter(ref yeHuWhere, ref param, "yeHu", "OrgType", 2);
            sqlParamHelper.AppendParameter(ref fuWuShangWhere, ref param, "fuWuShang", "SYS_XiTongZhuangTai", 0);
            sqlParamHelper.AppendParameter(ref fuWuShangWhere, ref param, "fuWuShang", "OrgType", 5);
            //车牌号
            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
            {
                sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "ChePaiHao", dto.ChePaiHao);
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
            {
                sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "ChePaiYanSe", dto.ChePaiYanSe);
            }

            //业户名称
            if (!string.IsNullOrWhiteSpace(dto.YeHuMingCheng))
            {
                sqlParamHelper.AppendParameter(ref yeHuWhere, ref param, "yeHu", "YeHuMingCheng", dto.YeHuMingCheng);
            }

            //辖区县
            if (dto.XiaQuXian.Any())
            {
                sqlParamHelper.AppendParameterIn(ref cheLiangWhere, ref param, "cl", "XiaQuXian", dto.XiaQuXian);
            }

            //车辆种类
            if (dto.CheLiangZhongLei.Any())
            {
                sqlParamHelper.AppendParameterIn(ref cheLiangWhere, ref param, "cl", "CheLiangZhongLei",
                    dto.CheLiangZhongLei.Select(x => Convert.ToInt32(x)));
            }

            switch (userInfo.OrganizationType)
            {
                case (int)OrganizationType.市政府:
                    if (string.IsNullOrWhiteSpace(userInfo.OrganCity))
                        return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "辖区市为空" };
                    else
                        sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "XiaQuShi",
                            userInfo.OrganCity);
                    break;
                case (int)OrganizationType.县政府:
                    if (string.IsNullOrWhiteSpace(userInfo.OrganDistrict))
                        return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "辖区市为空" };
                    else
                        sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "XiaQuXian",
                            userInfo.OrganDistrict);
                    break;
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                case (int)OrganizationType.个体户:
                    if (string.IsNullOrWhiteSpace(userInfo.OrganizationCode))
                        return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "组织编号为空！" };
                    if (IsGuanLiYuanRoleCode(userInfo.RoleCode))
                        sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "YeHuOrgCode",
                            userInfo.OrganizationCode);
                    else
                        return GetNotSubscribeVehicleBaseInfoByYongHu(dto, userInfo.Id.ToString(), queryData.page,
                            queryData.rows);
                    break;
                default:
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "组织类型不存在" };
            }


            #endregion

            string sql = @"
				SELECT
					cl.Id AS Id,
					cl.ChePaiHao AS ChePaiHao,
					cl.ChePaiYanSe AS ChePaiYanSe,
					cl.CheLiangLeiXing AS CheLiangLeiXing,
					cl.CheLiangZhongLei AS CheLiangZhongLei,
					cl.CheZaiDianHua AS CheZaiDianHua,
					cl.XiaQuSheng AS XiaQuSheng,
					cl.XiaQuShi AS XiaQuShi,
					cl.XiaQuXian AS XiaQuXian,
					cl.YeHuOrgCode AS YeHuOrgCode,
					yh.OrgCode AS YeHuDaiMa,
					yh.OrgName AS YeHuMingCheng,
					cl.FuWuShangOrgCode AS FuWuShangOrgCode,
					fws.OrgCode AS OperatorCode,
					fws.OrgName AS Operatorname,
					cl.SYS_ChuangJianShiJian 
				FROM
					DC_GPSJCDAGL.dbo.T_CheLiang AS cl
					JOIN ( SELECT * FROM DC_GPSJCDAGL.dbo.T_OrgBaseInfo AS yeHu " + yeHuWhere +
                         @" ) AS yh ON cl.YeHuOrgCode = yh.OrgCode
					LEFT JOIN ( SELECT * FROM DC_GPSJCDAGL.dbo.T_OrgBaseInfo AS fuWuShang " + fuWuShangWhere +
                         @" ) AS fws ON cl.FuWuShangOrgCode = fws.OrgCode 
				" + cheLiangWhere + @" 
					AND NOT EXISTS (
					SELECT
						1 
					FROM
						DC_PTYXJC.dbo.T_UserVehicleSubscribe AS s 
					WHERE
						s.UserId = @UserId 
						AND cl.ChePaiHao= s.RegistrationNo 
						AND cl.ChePaiYanSe= s.RegistrationNoColor 
						AND s.SYS_XiTongZhuangTai= 0)
            ";
            //分页
            int rowStart = (queryData.page - 1) * queryData.rows + 1;
            int rowEnd = rowStart + queryData.rows - 1;
            param.Add(new SqlParameter("@Row_Start", rowStart));
            param.Add(new SqlParameter("@Row_End", rowEnd));
            string pagedSql = @"with _query as (" + sql + @")
			select 
				pp.Id AS Id,
				pp.ChePaiHao AS ChePaiHao,
				pp.ChePaiYanSe AS ChePaiYanSe,
				pp.CheLiangLeiXing AS CheLiangLeiXing,
				pp.CheLiangZhongLei AS CheLiangZhongLei,
				pp.CheZaiDianHua AS CheZaiDianHua,
				pp.XiaQuSheng AS XiaQuSheng,
				pp.XiaQuShi AS XiaQuShi,
				pp.XiaQuXian AS XiaQuXian,
				pp.YeHuOrgCode AS YeHuOrgCode,
				pp.YeHuDaiMa AS YeHuDaiMa,
				pp.YeHuMingCheng AS YeHuMingCheng,
				pp.FuWuShangOrgCode AS FuWuShangOrgCode,
				pp.OperatorCode AS OperatorCode,
				pp.Operatorname AS Operatorname 
			from (
				select 
					*,
					ROW_NUMBER() over (ORDER BY p.SYS_ChuangJianShiJian desc) as row_num 
				from _query as p) 
			as pp 
			where 
				row_num BETWEEN @Row_Start and @Row_End";
            string totalCountSql = @"
				with totalQuery as ( " + sql + @")
				select count(*) from totalQuery";

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString;
            SqlHelper sqlHelper = new SqlHelper(connectionString);
            SqlDataReader reader = sqlHelper.Reader(pagedSql, param.ToArray());
            List<dynamic> items = new List<dynamic>();
            while (reader.Read())
            {
                items.Add(new
                {
                    Id = reader["Id"],
                    ChePaiHao = reader["ChePaiHao"],
                    ChePaiYanSe = reader["ChePaiYanSe"],
                    CheLiangLeiXing = reader["CheLiangLeiXing"],
                    CheLiangZhongLei = reader["CheLiangZhongLei"],
                    CheZaiDianHua = reader["CheZaiDianHua"],
                    XiaQuSheng = reader["XiaQuSheng"],
                    XiaQuShi = reader["XiaQuShi"],
                    XiaQuXian = reader["XiaQuXian"],
                    YeHuOrgCode = reader["YeHuOrgCode"],
                    YeHuDaiMa = reader["YeHuDaiMa"],
                    YeHuMingCheng = reader["YeHuMingCheng"],
                    FuWuShangOrgCode = reader["FuWuShangOrgCode"],
                    OperatorCode = reader["OperatorCode"],
                    Operatorname = reader["Operatorname"],
                });
            }


            int total = Convert.ToInt32(sqlHelper.ExecuteScalar(totalCountSql, param.ToArray()));
            QueryResult queryResult = new QueryResult
            {
                items = items,
                totalcount = total,

            };
            return new ServiceResult<QueryResult>() { Data = queryResult };
        }

        #endregion

        #region 根据用户ID获取关联的车辆基本信息，并排除已经订阅的车辆

        /// <summary>
        /// 根据用户ID获取关联的车辆基本信息，并排除已经订阅的车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private ServiceResult<QueryResult> GetNotSubscribeVehicleBaseInfoByYongHu(NotSubscribeCheLiangSearchDto dto,
            string userId, int pageNumber, int pageSize)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserId", userId));
            string cheLiangWhere = string.Empty;
            SqlParamHelper sqlParamHelper = new SqlParamHelper();
            sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "XiTongZhuangTai", 0);
            //车牌号
            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
            {
                sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "ChePaiHao", dto.ChePaiHao);
            }

            //车牌颜色
            if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
            {
                sqlParamHelper.AppendParameter(ref cheLiangWhere, ref param, "cl", "ChePaiYanSe", dto.ChePaiYanSe);
            }

            //辖区县
            if (dto.XiaQuXian.Any())
            {
                sqlParamHelper.AppendParameterIn(ref cheLiangWhere, ref param, "cl", "XiaQuXian", dto.XiaQuXian);
            }

            //车辆种类
            if (dto.CheLiangZhongLei.Any())
            {
                IEnumerable<int> cheLiangZhongLeiArr = dto.CheLiangZhongLei.Select(x => Convert.ToInt32(x));
                sqlParamHelper.AppendParameterIn(ref cheLiangWhere, ref param, "cl", "CheLiangZhongLei",
                    cheLiangZhongLeiArr);
            }

            string sql = @"
			SELECT
				cl.Id AS Id,
				cl.ChePaiHao AS ChePaiHao,
				cl.ChePaiYanSe AS ChePaiYanSe,
				cl.CheLiangLeiXing AS CheLiangLeiXing,
				cl.CheLiangZhongLei AS CheLiangZhongLei,
				cl.CheZaiDianHua AS CheZaiDianHua,
				cl.XiaQuSheng AS XiaQuSheng,
				cl.XiaQuShi AS XiaQuShi,
				cl.XiaQuXian AS XiaQuXian,
				yh.OrgCode AS YeHuDaiMa,
				yh.OrgName AS YeHuMingCheng,
				fws.OrgCode AS OperatorCode,
				fws.OrgName AS OperatorName,
				cl.SYS_ChuangJianShiJian
			FROM
				DC_GPSJCDAGL.dbo.T_CheLiang AS cl
				JOIN ( SELECT * FROM DC_GPSJCDAGL.dbo.T_YongHuCheLiangXinXi AS yonghuCheLiang WHERE yonghuCheLiang.SYS_XiTongZhuangTai = 0 AND  yonghuCheLiang.SysUserId = @UserId) AS yhcl ON cl.Id = yhcl.CheLiangId
				JOIN ( SELECT * FROM DC_GPSJCDAGL.dbo.T_OrgBaseInfo AS yeHu WHERE yeHu.SYS_XiTongZhuangTai = 0 ) AS yh ON cl.YeHuOrgCode = yh.OrgCode
				LEFT JOIN ( SELECT * FROM DC_GPSJCDAGL.dbo.T_OrgBaseInfo AS fuWuShang WHERE fuWuShang.SYS_XiTongZhuangTai = 0 ) AS fws ON cl.FuWuShangOrgCode = fws.OrgCode 
			" + cheLiangWhere + @" 
				AND NOT EXISTS (
				SELECT
					1 
				FROM
					DC_PTYXJC.dbo.T_UserVehicleSubscribe AS s 
				WHERE
					s.UserId = @UserId 
					AND cl.ChePaiHao= s.RegistrationNo 
					AND cl.ChePaiYanSe= s.RegistrationNoColor 
					AND s.SYS_XiTongZhuangTai= 0)
			";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString;
            SqlHelper sqlHelper = new SqlHelper(connectionString);
            int rowStart = (pageNumber - 1) * pageSize + 1;
            int rowEnd = rowStart + pageSize - 1;
            param.Add(new SqlParameter("@Row_Start", rowStart));
            param.Add(new SqlParameter("@Row_End", rowEnd));
            string pagedSql = @"with _query as (" + sql + @")
			select 
				pp.Id AS Id,
				pp.ChePaiHao AS ChePaiHao,
				pp.CheLiangLeiXing AS CheLiangLeiXing,
				pp.CheLiangZhongLei AS CheLiangZhongLei,
				pp.CheZaiDianHua AS CheZaiDianHua,
				pp.XiaQuSheng AS XiaQuSheng,
				pp.XiaQuShi AS XiaQuShi,
				pp.XiaQuXian AS XiaQuXian,
				pp.YeHuOrgCode AS YeHuOrgCode,
				pp.YeHuDaiMa AS YeHuDaiMa,
				pp.YeHuMingCheng AS YeHuMingCheng,
				pp.FuWuShangOrgCode AS FuWuShangOrgCode,
				pp.OperatorCode AS OperatorCode,
				pp.Operatorname AS Operatorname 
			from (
				select 
					*,
					ROW_NUMBER() over (ORDER BY p.SYS_ChuangJianShiJian desc) as row_num 
				from _query as p) 
			as pp 
			where 
				row_num BETWEEN @Row_Start and @Row_End";
            string totalCountSql = @"
				with totalQuery as ( " + sql + @")
				select count(1) from totalQuery";
            QueryResult queryResult = new QueryResult
            {
                items = sqlHelper.DataSet(pagedSql, param.ToArray()),
                totalcount = Convert.ToInt32(sqlHelper.ExecuteScalar(totalCountSql, param.ToArray()))
            };
            return new ServiceResult<QueryResult>() { Data = queryResult };
        }

        #endregion




        #endregion

        #region 详细信息

        #region 获取车辆详情信息

        public ServiceResult<object> GetVehicleDetailedInfo(string CheLiangId)
        {
            var rep = _cheLianExRepository.GetQuery(p => p.SYS_XiTongZhuangTai == 0 && p.CheLiangId == CheLiangId)
                .FirstOrDefault();
            return new ServiceResult<object>() { Data = rep };
        }

        #endregion

        #region 新增车辆详情信息

        public ServiceResult<object> AddVehicleDetailedInfo(CheLiangEx dto, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<object>(() =>
            {
                var flag = false;
                dto.Id = Guid.NewGuid();
                var cheLiangId = new Guid(dto.CheLiangId);
                var vehicleConfirm = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.CheLiangId == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                var vehicleEntity = _cheLiangXinXiRepository.GetByKey(cheLiangId);
                using (var u = _cheLiangXinXiRepository.UnitOfWork)
                {
                    u.BeginTransaction();
                    if (vehicleConfirm != null)
                    {
                        vehicleConfirm.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        vehicleConfirm.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交;
                        _cheLiangVideoZhongDuanConfirmRepository.Update(vehicleConfirm);
                    }
                    if (vehicleEntity != null)
                    {
                        vehicleEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleEntity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        vehicleEntity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        vehicleEntity.GPSAuditStatus = (int)GPSAuditStatus.待审核;
                        _cheLiangXinXiRepository.Update(vehicleEntity);
                    }
                    dto.ChuangJianRenOrgCode = userInfo.OrganizationCode;
                    dto.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                    SetCreateSYSInfo(dto, userInfo);
                    _cheLiangExRepository.Add(dto);
                    flag = u.CommitTransaction() > 0;

                    if (flag)
                    {
                        var carInfo = _cheLiangXinXiRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == cheLiangId).FirstOrDefault();
                        Mapper.CreateMap<CheLiangEx, CheLiangExInfoDto>();
                        CheLiangExInfoDto newModel = Mapper.Map<CheLiangExInfoDto>(dto);

                        //比较修改差异
                        List<LogUpdateValueDto> updateDetailList =
                            OprateLogHelper.GetObjCompareString(new CheLiangExInfoDto(), newModel, true);
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                            ActionName = nameof(AddOrUpdateCheLiangBaoXianXinXi),
                            BizOperType = OperLogBizOperType.UPDATE,
                            ShortDescription = "车辆档案详细信息修改：" + carInfo?.ChePaiHao + "[" + carInfo.ChePaiYanSe + "]",
                            OperatorName = userInfo.UserName,
                            OldBizContent = "",
                            NewBizContent = JsonConvert.SerializeObject(dto),
                            OperatorID = userInfo.Id,
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                            ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                        });
                    }
                }

                return new ServiceResult<object>() { Data = flag };
            });

        }

        #endregion

        #region 更新车辆详情信息

        public ServiceResult<object> UpDateVehicleDetailedInfo(CheLiangEx cheLiang, UserInfoDto userInfo)
        {
            var flag = false;
            var userInfoNew = GetUserInfo();
            if (userInfoNew == null)
            {
                return new ServiceResult<object> { StatusCode = 2, ErrorMessage = "获取登录信息失败,请重新登录" };
            }
            var preEntity = _cheLiangExRepository.GetByKey(cheLiang.Id);
            var cheliangId = new Guid(preEntity.CheLiangId);
            var vehicleConfirm = _cheLiangVideoZhongDuanConfirmRepository
                .GetQuery(x => x.CheLiangId == preEntity.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();

            var vehicleEntity = _cheLiangXinXiRepository.GetByKey(cheliangId);
            using (var u = _cheLiangExRepository.UnitOfWork)
            {
                u.BeginTransaction();

                try
                {
                    if (vehicleConfirm != null)
                    {
                        vehicleConfirm.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRenID = userInfoNew.Id;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRen = userInfoNew.UserName;
                        vehicleConfirm.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交;
                        _cheLiangVideoZhongDuanConfirmRepository.Update(vehicleConfirm);
                    }

                    if (vehicleEntity != null)
                    {
                        vehicleEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleEntity.SYS_ZuiJinXiuGaiRenID = userInfoNew.Id;
                        vehicleEntity.SYS_ZuiJinXiuGaiRen = userInfoNew.UserName;
                        vehicleEntity.GPSAuditStatus = (int)GPSAuditStatus.待审核;
                        _cheLiangXinXiRepository.Update(vehicleEntity);
                    }
                    //比较修改差异
                    Mapper.CreateMap<CheLiangEx, CheLiangExInfoDto>();
                    CheLiangExInfoDto newModel = Mapper.Map<CheLiangExInfoDto>(cheLiang);
                    newModel.CheChang = Convert.ToDecimal(Convert.ToDecimal(newModel.CheChang).ToString("F2"));
                    newModel.CheKuan = Convert.ToDecimal(Convert.ToDecimal(newModel.CheKuan).ToString("F2"));
                    newModel.CheGao = Convert.ToDecimal(Convert.ToDecimal(newModel.CheGao).ToString("F2"));

                    CheLiangExInfoDto oldModel = Mapper.Map<CheLiangExInfoDto>(preEntity);
                    oldModel.CheChang = Convert.ToDecimal(Convert.ToDecimal(newModel.CheChang).ToString("F2"));
                    oldModel.CheKuan = Convert.ToDecimal(Convert.ToDecimal(newModel.CheKuan).ToString("F2"));
                    oldModel.CheGao = Convert.ToDecimal(Convert.ToDecimal(newModel.CheGao).ToString("F2"));
                    List<LogUpdateValueDto> updateDetailList =
                        OprateLogHelper.GetObjCompareString(oldModel, newModel, true);
                    SetUpdateSYSInfo(preEntity, cheLiang, userInfo);
                    cheLiang.ZuiJinXiuGaiRenOrgCode = userInfo.OrganizationCode;
                    _cheLiangExRepository.Update(cheLiang);
                    flag = u.CommitTransaction() >= 0;
                    if (flag)
                    {
                        var carInfo = _cheLiangXinXiRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == cheliangId).FirstOrDefault();
                        OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                        {
                            SystemName = OprateLogHelper.GetSysTemName(),
                            ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                            ActionName = nameof(AddOrUpdateCheLiangBaoXianXinXi),
                            BizOperType = OperLogBizOperType.UPDATE,
                            ShortDescription = "车辆档案详细信息修改：" + carInfo?.ChePaiHao + "[" + carInfo.ChePaiYanSe + "]",
                            OperatorName = userInfo.UserName,
                            OldBizContent = JsonConvert.SerializeObject(preEntity),
                            NewBizContent = JsonConvert.SerializeObject(cheLiang),
                            OperatorID = userInfo.Id,
                            OperatorOrgCode = userInfo.OrganizationCode,
                            OperatorOrgName = userInfo.OrganizationName,
                            SysID = SysId,
                            AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                            ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                        });
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceResult<object>() { StatusCode = 2, Data = false, ErrorMessage = ex.Message };
                }

            }

            return new ServiceResult<object>() { Data = flag };
        }

        #endregion




        #endregion

        #region 终端信息

        #region 获取车辆终端信息

        public ServiceResult<ZhongDuanXinXiDto> GetZhongDuanXinXi(string cheLiangId)
        {
            try
            {
                //GPS终端信息
                var gps = _cheLiangGPSZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangId == cheLiangId)
                    .Select(y => new GpsZhongDuanXinXiDto
                    {
                        Id = y.Id,
                        ZhongDuanLeiXing = y.ZhongDuanLeiXing,
                        ShengChanChangJia = y.ShengChanChangJia,
                        ChangJiaBianHao = y.ChangJiaBianHao,
                        SheBeiXingHao = y.SheBeiXingHao,
                        ZhongDuanBianMa = y.ZhongDuanBianMa,
                        SIMKaHao = y.SIMKaHao,
                        ZhongDuanMDT = y.ZhongDuanMDT,
                        IA1 = (long)y.IA1,
                        IC1 = (long)y.IC1,
                        M1 = (long)y.M1,
                        ShiFouAnZhuangShiPinZhongDuan = y.ShiFouAnZhuangShiPinZhongDuan,
                        ShiPinTouAnZhuangXuanZe = y.ShiPinTouAnZhuangXuanZe,
                        ShiPingChangShangLeiXing = y.ShiPingChangShangLeiXing,
                        ShiPinTouGeShu = y.ShiPinTouGeShu,
                        Remark = y.Remark
                    }).FirstOrDefault();


                var video = _cheLiangVideoZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangId == cheLiangId)
                    .Select(y => new VideoZhongDuanXinXiDto
                    {
                        Id = y.Id,
                        VideoDeviceMDT = y.ZhongDuanMDT,
                        VideoSheBeiXingHao = y.SheBeiXingHao,
                        VideoSheBeiJiShenLeiXing = y.SheBeiJiShenLeiXing,
                        VideoSheBeiGouCheng = y.SheBeiGouCheng,
                        VideoShengChanChangJia = y.ShengChanChangJia,
                        VideoChangJiaBianHao = y.ChangJiaBianHao,
                        VideoAnZhuangShiJian = y.AnZhuangShiJian,
                    }).FirstOrDefault();
                if (video != null)
                {
                    var videoFileList = _zhongDuanFileMapperRepository
                        .GetQuery(x => x.ZhongDuanId == video.Id && x.SYS_XiTongZhuangTai == 0).Select(x =>
                            new VideoZhongDuanImgDto { FileId = x.FileId, FileType = x.FileType }).ToList();
                    video.FileList = videoFileList;
                }

                //获取终端配置信息
                var peiZhi = _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 &&
                    (x.CheLiangID.HasValue && x.CheLiangID.ToString() == cheLiangId)).Select(pz =>
                    new ZhongDuanShuJuTongXunPeiZhiXinXiDto
                    {
                        Id = pz.Id,
                        BanBenHao = pz.BanBenHao,
                        CheLiangID = pz.CheLiangID,
                        XieYiLeiXing = pz.XieYiLeiXing,
                        ZhongDuanID = pz.ZhongDuanID,
                        ZhuaBaoLaiYuan = pz.ZhuaBaoLaiYuan,
                    }).FirstOrDefault();



                var model = new ZhongDuanXinXiDto
                {
                    GpsInfo = gps,
                    VideoInfo = video,
                    PeiZhiInfo = peiZhi,
                };
                return new ServiceResult<ZhongDuanXinXiDto> { Data = model };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取车辆终端信息失败" + ex.Message, ex);
                return new ServiceResult<ZhongDuanXinXiDto> { StatusCode = 2, ErrorMessage = "" };
            }
        }

        #endregion

        #region 更新车辆终端信息(GPS+智能视频)

        /// <summary>
        /// 更新车辆终端配置信息(GPS+智能视频)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<bool> UpdateZhongDuanXinXi(UpdateZhongDuanXinXiDto dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }

                if (string.IsNullOrWhiteSpace(dto?.CheLiangId))
                {
                    return new ServiceResult<bool> { ErrorMessage = "车辆ID不能为空", StatusCode = 2 };
                }

                var cheliangId = new Guid(dto.CheLiangId);
                var carInfo = _cheLiangXinXiRepository.GetQuery(x => x.Id == cheliangId && x.SYS_XiTongZhuangTai == 0)
                    .FirstOrDefault();
                if (carInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆信息失败" };
                }
                var vehicleConfirm = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.CheLiangId == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                UpdateZhongDuanXinXiDto oldModelInfo = new UpdateZhongDuanXinXiDto();

                bool isSuccess = false;
                bool isCreateGpsInfo = false;
                bool isCreateVideoInfo = false;
                bool isCreatePeiZhiInfo = false;
                bool isReBeiAn = false;
                List<LogUpdateValueDto> logList = new List<LogUpdateValueDto>();
                //保存GPS终端信息

                var gpsModel = _cheLiangGPSZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();

                if (dto?.GpsInfo != null)
                {
                    if (gpsModel == null)
                    {
                        gpsModel = new CheLiangGPSZhongDuanXinXi
                        {
                            Id = Guid.NewGuid(),
                            CheLiangId = dto.CheLiangId,
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                        };
                        isCreateGpsInfo = true;
                    }

                    //比较数据差异
                    Mapper.CreateMap<CheLiangGPSZhongDuanXinXi, GpsZhongDuanXinXiDto>();
                    var oldModel = Mapper.Map<GpsZhongDuanXinXiDto>(gpsModel);
                    dto.GpsInfo.ShiFouAnZhuangShiPinZhongDuan = dto.GpsInfo.ShiFouAnZhuangShiPinZhongDuan ?? 0;
                    dto.GpsInfo.ShiPingChangShangLeiXing = dto.GpsInfo.ShiPingChangShangLeiXing ?? 0;
                    dto.GpsInfo.ShiPinTouGeShu = dto.GpsInfo.ShiPinTouGeShu ?? 0;
                    var gpsUpdateInfo = OprateLogHelper.GetObjCompareString(oldModel, dto.GpsInfo, true);
                    logList.AddRange(gpsUpdateInfo);
                    oldModelInfo.CheLiangId = dto.CheLiangId;
                    oldModelInfo.GpsInfo = oldModel;

                    //找出重复的记录
                    var repeatData = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangId != dto.CheLiangId &&
                        (x.SIMKaHao == dto.GpsInfo.SIMKaHao || x.ZhongDuanMDT == dto.GpsInfo.ZhongDuanMDT)).ToList();

                    if (!string.IsNullOrWhiteSpace(dto.GpsInfo.ZhongDuanMDT))
                    {
                        if (repeatData.Where(x => x.ZhongDuanMDT == dto.GpsInfo.ZhongDuanMDT).Count() > 0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该终端号已存在，请重新核实修正！" };
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dto.GpsInfo.SIMKaHao))
                    {
                        if (repeatData.Where(x => x.SIMKaHao == dto.GpsInfo.SIMKaHao).Count() > 0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "该SIM卡号已存在，请重新核实修正！" };
                        }
                    }

                    //修改MDT或者SIM卡号需要重新备案审核
                    if (!isCreateGpsInfo)
                    {
                        if (gpsModel.SIMKaHao != dto.GpsInfo.SIMKaHao ||
                            gpsModel.ZhongDuanMDT != dto.GpsInfo.ZhongDuanMDT)
                        {
                            isReBeiAn = true;
                        }
                    }

                    gpsModel.ZhongDuanLeiXing = dto.GpsInfo.ZhongDuanLeiXing;
                    gpsModel.SheBeiXingHao = dto.GpsInfo.SheBeiXingHao;
                    gpsModel.ShengChanChangJia = dto.GpsInfo.ShengChanChangJia;
                    gpsModel.ChangJiaBianHao = dto.GpsInfo.ChangJiaBianHao;
                    gpsModel.ZhongDuanBianMa = dto.GpsInfo.ZhongDuanBianMa;
                    gpsModel.SIMKaHao = dto.GpsInfo.SIMKaHao;
                    gpsModel.ZhongDuanMDT = dto.GpsInfo.ZhongDuanMDT;
                    gpsModel.M1 = dto.GpsInfo.M1;
                    gpsModel.IA1 = dto.GpsInfo.IA1;
                    gpsModel.IC1 = dto.GpsInfo.IC1;
                    gpsModel.ShiFouAnZhuangShiPinZhongDuan = dto.GpsInfo.ShiFouAnZhuangShiPinZhongDuan ?? 0;
                    gpsModel.ShiPinTouAnZhuangXuanZe = dto.GpsInfo.ShiPinTouAnZhuangXuanZe;
                    gpsModel.ShiPingChangShangLeiXing = dto.GpsInfo.ShiPingChangShangLeiXing ?? 0;
                    gpsModel.ShiPinTouGeShu = dto.GpsInfo.ShiPinTouGeShu ?? 0;
                    gpsModel.Remark = dto.GpsInfo.Remark;
                }

                if (gpsModel != null)
                {
                    gpsModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                    gpsModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                    gpsModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                }

                var videoModel = _cheLiangVideoZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();


                //图片附件列表
                List<ZhongDuanFileMapper> addFileList = new List<ZhongDuanFileMapper>();
                List<ZhongDuanFileMapper> UpdateFileList = new List<ZhongDuanFileMapper>();
                //保存智能视频终端信息
                if (dto?.VideoInfo != null)
                {
                    if (videoModel == null)
                    {
                        videoModel = new CheLiangVideoZhongDuanXinXi
                        {
                            Id = Guid.NewGuid(),
                            CheLiangId = dto.CheLiangId,
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常,
                        };
                        isCreateVideoInfo = true;
                    }

                    //比较数据差异
                    Mapper.CreateMap<CheLiangVideoZhongDuanXinXi, VideoZhongDuanXinXiDto>()
                        .ForMember(x => x.VideoDeviceMDT, y => y.MapFrom(z => z.ZhongDuanMDT))
                        .ForMember(x => x.VideoSheBeiXingHao, y => y.MapFrom(z => z.SheBeiXingHao))
                        .ForMember(x => x.VideoSheBeiJiShenLeiXing, y => y.MapFrom(z => z.SheBeiJiShenLeiXing))
                        .ForMember(x => x.VideoSheBeiGouCheng, y => y.MapFrom(z => z.SheBeiGouCheng))
                        .ForMember(x => x.VideoShengChanChangJia, y => y.MapFrom(z => z.ShengChanChangJia))
                        .ForMember(x => x.VideoChangJiaBianHao, y => y.MapFrom(z => z.ChangJiaBianHao))
                        .ForMember(x => x.VideoAnZhuangShiJian, y => y.MapFrom(z => z.AnZhuangShiJian));
                    var oldModel = Mapper.Map<VideoZhongDuanXinXiDto>(videoModel);
                    //找到旧附件
                    if (videoModel != null)
                    {
                        oldModel.XianShiPingZhaoPianId = _zhongDuanFileMapperRepository
                            .GetQuery(x =>
                                x.FileType == (int)VideoZhongDuanImgType.显示屏 && x.SYS_XiTongZhuangTai == 0 &&
                                x.ZhongDuanId == videoModel.Id).OrderByDescending(x => x.SYS_ChuangJianShiJian)
                            .FirstOrDefault()?.FileId;
                        oldModel.ShengGuangBaoJingXiTongZhaoPianId = _zhongDuanFileMapperRepository
                            .GetQuery(x =>
                                x.FileType == (int)VideoZhongDuanImgType.声光报警系统 && x.SYS_XiTongZhuangTai == 0 &&
                                x.ZhongDuanId == videoModel.Id).OrderByDescending(x => x.SYS_ChuangJianShiJian)
                            .FirstOrDefault()?.FileId;
                        oldModel.ZhuJiCunChuQiZhaoPianId = _zhongDuanFileMapperRepository
                            .GetQuery(x =>
                                x.FileType == (int)VideoZhongDuanImgType.主机存储器 && x.SYS_XiTongZhuangTai == 0 &&
                                x.ZhongDuanId == videoModel.Id).OrderByDescending(x => x.SYS_ChuangJianShiJian)
                            .FirstOrDefault()?.FileId;
                    }

                    var videoUpdateInfo = OprateLogHelper.GetObjCompareString(oldModel, dto.VideoInfo, true);
                    logList.AddRange(videoUpdateInfo);
                    oldModelInfo.VideoInfo = oldModel;
                    //找出重复的记录
                    var repeatData = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangId != dto.CheLiangId &&
                        x.ZhongDuanMDT == dto.VideoInfo.VideoDeviceMDT).ToList();
                    if (!string.IsNullOrWhiteSpace(dto.VideoInfo.VideoDeviceMDT))
                    {
                        if (repeatData.Where(x => x.ZhongDuanMDT == dto.VideoInfo.VideoDeviceMDT).Count() > 0)
                        {
                            return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端MDT已存在，请重新核实修正！" };
                        }
                    }

                    //修改MDT或者SIM卡号需要重新备案审核
                    if (!isCreateVideoInfo)
                    {
                        if (videoModel.ZhongDuanMDT != dto.VideoInfo.VideoDeviceMDT)
                        {
                            isReBeiAn = true;
                        }
                    }

                    videoModel.ZhongDuanMDT = dto.VideoInfo.VideoDeviceMDT;
                    videoModel.SheBeiXingHao = dto.VideoInfo.VideoSheBeiXingHao;
                    videoModel.SheBeiJiShenLeiXing = dto.VideoInfo.VideoSheBeiJiShenLeiXing ?? 0;
                    videoModel.SheBeiGouCheng = dto.VideoInfo.VideoSheBeiGouCheng ?? "";
                    videoModel.ChangJiaBianHao = dto.VideoInfo.VideoChangJiaBianHao;
                    videoModel.ShengChanChangJia = dto.VideoInfo.VideoShengChanChangJia;
                    videoModel.AnZhuangShiJian = dto.VideoInfo.VideoAnZhuangShiJian;
                    //智能视频附件信息
                    var imgList = _zhongDuanFileMapperRepository
                        .GetQuery(x => x.ZhongDuanId == videoModel.Id && x.SYS_XiTongZhuangTai == 0).ToList();

                    var imgtype3 = imgList.Where(x =>
                        x.FileType == (int)VideoZhongDuanImgType.显示屏 && x.SYS_XiTongZhuangTai == 0).ToList();
                    if (!string.IsNullOrWhiteSpace(dto.VideoInfo.XianShiPingZhaoPianId))
                    {
                        if (imgtype3.Where(x => x.FileId == dto.VideoInfo.XianShiPingZhaoPianId).Count() <= 0)
                        {
                            addFileList.Add(new ZhongDuanFileMapper
                            {
                                Id = Guid.NewGuid(),
                                ZhongDuanId = videoModel.Id,
                                FileId = dto.VideoInfo.XianShiPingZhaoPianId,
                                FileType = (int)VideoZhongDuanImgType.显示屏,
                                SYS_ChuangJianShiJian = DateTime.Now,
                                SYS_ChuangJianRen = userInfo.UserName,
                                SYS_ChuangJianRenID = userInfo.Id,
                                SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                            });
                            foreach (var item in imgtype3)
                            {
                                item.SYS_XiTongZhuangTai = 1;
                                item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                UpdateFileList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in imgtype3)
                        {
                            item.SYS_XiTongZhuangTai = 1;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            UpdateFileList.Add(item);
                        }
                    }

                    var imgtype1 = imgList.Where(x =>
                        x.FileType == (int)VideoZhongDuanImgType.声光报警系统 && x.SYS_XiTongZhuangTai == 0).ToList();
                    if (!string.IsNullOrWhiteSpace(dto.VideoInfo.ShengGuangBaoJingXiTongZhaoPianId))
                    {
                        if (imgtype1.Where(x => x.FileId == dto.VideoInfo.ShengGuangBaoJingXiTongZhaoPianId).Count() <=
                            0)
                        {
                            addFileList.Add(new ZhongDuanFileMapper
                            {
                                Id = Guid.NewGuid(),
                                ZhongDuanId = videoModel.Id,
                                FileId = dto.VideoInfo.ShengGuangBaoJingXiTongZhaoPianId,
                                FileType = (int)VideoZhongDuanImgType.声光报警系统,
                                SYS_ChuangJianShiJian = DateTime.Now,
                                SYS_ChuangJianRen = userInfo.UserName,
                                SYS_ChuangJianRenID = userInfo.Id,
                                SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                            });
                            foreach (var item in imgtype1)
                            {
                                item.SYS_XiTongZhuangTai = 1;
                                item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                UpdateFileList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in imgtype1)
                        {
                            item.SYS_XiTongZhuangTai = 1;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            UpdateFileList.Add(item);
                        }
                    }



                    var imgtype2 = imgList.Where(x =>
                        x.FileType == (int)VideoZhongDuanImgType.主机存储器 && x.SYS_XiTongZhuangTai == 0).ToList();
                    if (!string.IsNullOrWhiteSpace(dto.VideoInfo.ZhuJiCunChuQiZhaoPianId))
                    {
                        if (imgtype2.Where(x => x.FileId == dto.VideoInfo.ZhuJiCunChuQiZhaoPianId).Count() <= 0)
                        {
                            addFileList.Add(new ZhongDuanFileMapper
                            {
                                Id = Guid.NewGuid(),
                                ZhongDuanId = videoModel.Id,
                                FileId = dto.VideoInfo.ZhuJiCunChuQiZhaoPianId,
                                FileType = (int)VideoZhongDuanImgType.主机存储器,
                                SYS_ChuangJianShiJian = DateTime.Now,
                                SYS_ChuangJianRen = userInfo.UserName,
                                SYS_ChuangJianRenID = userInfo.Id,
                                SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常
                            });
                            foreach (var item in imgtype2)
                            {
                                item.SYS_XiTongZhuangTai = 1;
                                item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                                item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                                item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                                UpdateFileList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in imgtype2)
                        {
                            item.SYS_XiTongZhuangTai = 1;
                            item.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            item.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            item.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            UpdateFileList.Add(item);
                        }
                    }
                }

                if (videoModel != null)
                {
                    videoModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                    videoModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                    videoModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                }


                #region 保存终端配置

                var gCheLiangId = Guid.Parse(dto.CheLiangId);

                //获取终端配置信息
                var peiZhiModel = _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常 && x.CheLiangID == gCheLiangId)
                    .FirstOrDefault();

                if (dto.PeiZhiInfo != null)
                {
                    if (peiZhiModel == null)
                    {
                        peiZhiModel = new CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi
                        {
                            Id = Guid.NewGuid(),
                            CheLiangID = Guid.Parse(dto.CheLiangId),
                            SYS_ChuangJianRen = userInfo?.UserName,
                            SYS_ChuangJianRenID = userInfo?.Id,
                            SYS_ChuangJianShiJian = DateTime.Now,
                            SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.正常,
                            ZhuaBaoLaiYuan = 0,
                            XieYiLeiXing = 0,
                        };
                        isCreatePeiZhiInfo = true;
                    }

                    var oldPeiZhiInfo = ZhongDuanShuJuTongXunPeiZhiXinXiDto.MapFromEntity(peiZhiModel);
                    var peizhiUpdateInfo = OprateLogHelper.GetObjCompareString(oldPeiZhiInfo, dto.PeiZhiInfo, true);
                    logList.AddRange(peizhiUpdateInfo);
                    oldModelInfo.PeiZhiInfo = oldPeiZhiInfo;

                    peiZhiModel = dto.PeiZhiInfo.MapToEntity(peiZhiModel);

                    peiZhiModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                    peiZhiModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                    peiZhiModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;


                }




                #endregion

                using (var u = _cheLiangXinXiRepository.UnitOfWork)
                {
                    u.BeginTransaction();
                    if (gpsModel != null)
                        //如果GPS终端MDT为空，但智能视频终端MDT不为空，则两个保持一致  by longdetao 20210930 广州平台需求
                        if (string.IsNullOrWhiteSpace(gpsModel.ZhongDuanMDT)
                            && !string.IsNullOrWhiteSpace(videoModel?.ZhongDuanMDT))
                        {
                            gpsModel.ZhongDuanMDT = videoModel.ZhongDuanMDT;
                        }

                    if (isCreateGpsInfo)
                    {
                        _cheLiangGPSZhongDuanXinXiRepository.Add(gpsModel);
                    }
                    else
                    {
                        _cheLiangGPSZhongDuanXinXiRepository.Update(gpsModel);
                    }

                    if (videoModel != null)
                        if (isCreateVideoInfo)
                        {
                            _cheLiangVideoZhongDuanXinXiRepository.Add(videoModel);
                        }
                        else
                        {
                            _cheLiangVideoZhongDuanXinXiRepository.Update(videoModel);
                        }

                    if (peiZhiModel != null)
                        if (isCreatePeiZhiInfo)
                        {
                            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Add(peiZhiModel);
                        }
                        else
                        {
                            _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.Update(peiZhiModel);
                        }


                    foreach (var item in UpdateFileList)
                    {
                        _zhongDuanFileMapperRepository.Update(item);
                    }

                    _zhongDuanFileMapperRepository.BatchInsert(addFileList.ToArray());

                    if (isReBeiAn)
                    {

                        // 人工审核状态 变为不通过
                        carInfo.ManualApprovalStatus = 0;
                        _cheLiangXinXiRepository.Update(carInfo);

                        var heCheModel = _cheLiangVideoZhongDuanConfirmRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId)
                            .FirstOrDefault();
                        if (heCheModel != null && heCheModel?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.取消备案 &&
                            heCheModel?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.待提交)
                        {
                            heCheModel.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交;
                            heCheModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                            heCheModel.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                            heCheModel.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                            heCheModel.TiJiaoBeiAnShiJian = null;
                            _cheLiangVideoZhongDuanConfirmRepository.Update(heCheModel);
                        }
                    }
                    if (carInfo != null)
                    {
                        carInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        carInfo.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        carInfo.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        carInfo.GPSAuditStatus = (int)GPSAuditStatus.待审核;
                        _cheLiangXinXiRepository.Update(carInfo);
                    }
                    if (vehicleConfirm != null)
                    {
                        vehicleConfirm.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        vehicleConfirm.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        vehicleConfirm.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.待提交;
                        _cheLiangVideoZhongDuanConfirmRepository.Update(vehicleConfirm);
                    }
                    isSuccess = u.CommitTransaction() > 0;
                }



                //修改MDT或SIM卡号需要重新审核
                if (isSuccess)
                {
                    if (dto?.GpsInfo?.ShiFouAnZhuangShiPinZhongDuan == 1)
                    {
                        SynCheLiangDangAn(carInfo, dto.GpsInfo.ShiPinTouAnZhuangXuanZe);
                    }

                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                        ActionName = nameof(UpdateZhongDuanXinXi),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = "车辆档案终端信息修改：" + carInfo?.ChePaiHao + "[" + carInfo.ChePaiYanSe + "]",
                        OperatorName = userInfo.UserName,
                        OldBizContent = JsonConvert.SerializeObject(oldModelInfo),
                        NewBizContent = JsonConvert.SerializeObject(dto),
                        OperatorID = userInfo.Id,
                        OperatorOrgCode = userInfo.OrganizationCode,
                        OperatorOrgName = userInfo.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        ExtendInfo = JsonConvert.SerializeObject(logList)
                    });
                }

                return new ServiceResult<bool> { Data = isSuccess };

            }
            catch (Exception ex)
            {
                LogHelper.Error("修改终端信息出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "修改出错", Data = false };
            }


        }

        /// <summary>
        /// 同步音视频车辆
        /// </summary>
        /// <param name="cheLiangXinXidto"></param>
        /// <param name="CameraSelected"></param>
        public void SynCheLiangDangAn(CheLiang cheLiangXinXidto, string CameraSelected)
        {
            try
            {
                List<object> adlist = new List<object>();
                foreach (var item in CameraSelected.Split(','))
                    adlist.Add(new
                    {
                        LuoJiSheBeiHao = item.Split('|')[0],
                        IsAudio = item.Split('|').Length > 2 ? item.Split('|')[2] : "0"
                    });
                string peiZhiWenJian = HttpContext.Current.Server.MapPath("/Config/Common/ZiGuangVideoSyn.json");
                string url = ConfigurationManager.AppSettings["ApiGateWay"];
                StreamReader sr = File.OpenText(peiZhiWenJian);
                string jsonstr = sr.ReadToEnd();
                ZiGuangVideoSynDto dto = JsonConvert.DeserializeObject<ZiGuangVideoSynDto>(jsonstr);
                dto.ChePaiHao = cheLiangXinXidto.ChePaiHao;
                dto.ChePaiYanSe = cheLiangXinXidto.ChePaiYanSe;
                CWRequest request = new CWRequest();
                CWPublicRequest publicRequest = new CWPublicRequest
                {
                    reqid = Guid.NewGuid().ToString(),
                    servicecode = "006600400040",
                    protover = "1.0",
                    servicever = "1.0",
                    requesttime = DateTime.Now.ToString()
                };
                request.publicrequest = publicRequest;
                request.body = new { CheLiangXinXi = dto, CameraSelected = adlist };
                ServiceHttpHelper httpHelper = new ServiceHttpHelper();
                string resp = httpHelper.Post(url + "/api/ServiceGateway/DataService", request);
                CWResponse response =
                    JsonConvert.DeserializeObject<CWResponse>(JsonConvert.DeserializeObject(resp).ToString());
                if (response.publicresponse != null && response.publicresponse.statuscode == 2 &&
                    response.publicresponse.message.IndexOf("该视频车辆档案已存在") < 0)
                {
                    LogHelper.Info("请求006600400040响应：" + JsonConvert.SerializeObject(resp));
                    throw new Exception("同步车辆档案失败，" + response.publicresponse.message);
                }

                LogHelper.Info(String.Format("添加车辆{0}到车辆终端信息表成功", cheLiangXinXidto.ChePaiHao));
            }
            catch (Exception e)
            {
                LogHelper.Error("添加车辆到车辆终端信息表失败！错误信息：" + e.ToString(), e);
            }

        }

        public class ZiGuangVideoSynDto
        {
            public string ChePaiHao { get; set; }
            public string ChePaiYanSe { get; set; }
            public string ZhongDuanXieYiLeiXing { get; set; }
            public string LiuMeiTiFuWuLeiXing { get; set; }
            public string ShuJuDaiLiAPIDiZhi { get; set; }
            public string ZhongDuanJieRuIP { get; set; }
            public string ZhongDuanJieRuDuanKou { get; set; }
            public string TuiLiuIP { get; set; }
            public string TuiLiuDuanKou { get; set; }
            public string LaLiuIP { get; set; }
            public string LaLiuDuanKou { get; set; }
            public bool ShiFouPingTaiCheLiang { get; set; }
            public string ShiPinZhiLiang { get; set; }
            public string LiuMeiTiBoFangDiZhi { get; set; }
        }

        #endregion

        #endregion

        #region 保险信息

        #region 更新车辆保险信息

        public ServiceResult<bool> AddOrUpdateCheLiangBaoXianXinXi(UpdateCheLiangBaoXianXinXiDto dto)
        {
            try
            {
                bool isSuccess = false;
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }

                if (dto?.CheLiangId == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
                }

                var carInfo = _cheLiangXinXiRepository
                    .GetQuery(x => x.Id == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (carInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆信息失败" };
                }

                CheLiangBaoXianXinXi baoXianModel = null;
                string oldData = "";
                if (dto.CheLiangId != null && dto.CheLiangId.HasValue)
                {
                    baoXianModel = _cheLiangBaoXianXinXiRepository
                        .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();
                }

                //新增操作日志
                Mapper.CreateMap<CheLiangBaoXianXinXi, UpdateCheLiangBaoXianXinXiDto>();
                UpdateCheLiangBaoXianXinXiDto oldModel = Mapper.Map<UpdateCheLiangBaoXianXinXiDto>(baoXianModel);
                List<LogUpdateValueDto> updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, dto, true);
                ;


                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (baoXianModel == null)
                    {

                        Mapper.CreateMap<UpdateCheLiangBaoXianXinXiDto, CheLiangBaoXianXinXi>();
                        baoXianModel = Mapper.Map<CheLiangBaoXianXinXi>(dto);
                        SetCreateSYSInfo(baoXianModel, userInfo);
                        baoXianModel.Id = Guid.NewGuid();
                        _cheLiangBaoXianXinXiRepository.Add(baoXianModel);
                    }
                    else
                    {
                        oldData = JsonConvert.SerializeObject(baoXianModel);
                        baoXianModel.JiaoQiangXianOrgName = dto.JiaoQiangXianOrgName;
                        baoXianModel.JiaoQiangXianEndTime = dto.JiaoQiangXianEndTime;
                        baoXianModel.ShangYeXianOrgName = dto.ShangYeXianOrgName;
                        baoXianModel.ShangYeXianEndTime = dto.ShangYeXianEndTime;
                        SetUpdateSYSInfo(baoXianModel, baoXianModel, userInfo);
                        _cheLiangBaoXianXinXiRepository.Update(baoXianModel);
                    }

                    isSuccess = uow.CommitTransaction() > 0;
                }

                OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                {
                    SystemName = OprateLogHelper.GetSysTemName(),
                    ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                    ActionName = nameof(AddOrUpdateCheLiangBaoXianXinXi),
                    BizOperType = OperLogBizOperType.UPDATE,
                    ShortDescription = "车辆档案保险信息修改：" + carInfo?.ChePaiHao + "[" + carInfo.ChePaiYanSe + "]",
                    OperatorName = userInfo.UserName,
                    OldBizContent = oldData,
                    NewBizContent = JsonConvert.SerializeObject(baoXianModel),
                    OperatorID = userInfo.Id,
                    OperatorOrgCode = userInfo.OrganizationCode,
                    OperatorOrgName = userInfo.OrganizationName,
                    SysID = SysId,
                    AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                });

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
                LogHelper.Error("更新车辆保险信息出错" + ex.Message + "请求报文" + JsonConvert.SerializeObject(dto), ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "保存出错", Data = false };
            }
        }

        #endregion


        #region 获取车辆保险信息

        public ServiceResult<CheLiangBaoXianXinXiResponseDto> GetCheLiangBaoXianXinXi(Guid? cheLiangId)
        {
            try
            {
                if (cheLiangId == null || !cheLiangId.HasValue)
                {
                    return new ServiceResult<CheLiangBaoXianXinXiResponseDto>
                    { ErrorMessage = "车辆ID不能为空", StatusCode = 2 };
                }

                var baoXianModel = _cheLiangBaoXianXinXiRepository
                    .GetQuery(x => x.CheLiangId == cheLiangId && x.SYS_XiTongZhuangTai == 0).Select(y =>
                        new CheLiangBaoXianXinXiResponseDto
                        {
                            JiaoQiangXianOrgName = y.JiaoQiangXianOrgName,
                            JiaoQiangXianEndTime = y.JiaoQiangXianEndTime,
                            ShangYeXianOrgName = y.ShangYeXianOrgName,
                            ShangYeXianEndTime = y.ShangYeXianEndTime
                        }).FirstOrDefault();
                return new ServiceResult<CheLiangBaoXianXinXiResponseDto> { Data = baoXianModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取车辆保险信息出错" + ex.Message, ex);
                return new ServiceResult<CheLiangBaoXianXinXiResponseDto> { StatusCode = 2, ErrorMessage = "获取保险信息出错" };
            }


        }

        #endregion

        #endregion

        #region 业户联系信息

        #region 更新业户联系信息

        public ServiceResult<bool> UpdateYeHuLianXiXinXi(UpdateYeHuBaoXianXinXiRequestDto dto)
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

                var carInfo = _cheLiangXinXiRepository
                    .GetQuery(x => x.Id == dto.CheLiangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (carInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆信息失败" };
                }

                var model = _cheLiangYeHuLianXiXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.CheLiangId).FirstOrDefault();
                string oldData = "";

                //新增操作日志
                Mapper.CreateMap<CheLiangYeHuLianXiXinXi, UpdateYeHuBaoXianXinXiRequestDto>();
                UpdateYeHuBaoXianXinXiRequestDto oldModel = Mapper.Map<UpdateYeHuBaoXianXinXiRequestDto>(model);
                List<LogUpdateValueDto> updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, dto, true);
                ;

                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (model == null)
                    {
                        Mapper.CreateMap<UpdateYeHuBaoXianXinXiRequestDto, CheLiangYeHuLianXiXinXi>();
                        CheLiangYeHuLianXiXinXi addModel = Mapper.Map<CheLiangYeHuLianXiXinXi>(dto);
                        SetCreateSYSInfo(addModel, userInfo);
                        addModel.Id = Guid.NewGuid();
                        _cheLiangYeHuLianXiXinXiRepository.Add(addModel);
                    }
                    else
                    {
                        oldData = JsonConvert.SerializeObject(model);

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
                        _cheLiangYeHuLianXiXinXiRepository.Update(model);
                    }

                    isSuccess = uow.CommitTransaction() > 0;
                }

                OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                {
                    SystemName = OprateLogHelper.GetSysTemName(),
                    ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                    ActionName = nameof(UpdateYeHuLianXiXinXi),
                    BizOperType = OperLogBizOperType.UPDATE,
                    ShortDescription = "车辆档案业户联系信息修改：" + carInfo?.ChePaiHao + "[" + carInfo.ChePaiYanSe + "]",
                    OperatorName = userInfo.UserName,
                    OldBizContent = oldData,
                    NewBizContent = JsonConvert.SerializeObject(model),
                    OperatorID = userInfo.Id,
                    OperatorOrgCode = userInfo.OrganizationCode,
                    OperatorOrgName = userInfo.OrganizationName,
                    SysID = SysId,
                    AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                });
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

        public ServiceResult<UpdateYeHuBaoXianXinXiResponseDto> GetYeHuLianXiXinXi(Guid CheLiangId)
        {
            try
            {
                var model = _cheLiangYeHuLianXiXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == CheLiangId).Select(x =>
                        new UpdateYeHuBaoXianXinXiResponseDto
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

                return new ServiceResult<UpdateYeHuBaoXianXinXiResponseDto> { Data = model };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取车辆业户联系信息出错" + ex.Message, ex);
                return new ServiceResult<UpdateYeHuBaoXianXinXiResponseDto> { ErrorMessage = "查询出错", StatusCode = 2 };
            }

        }

        #endregion

        #endregion


        #region  其他

        #region 修改车辆终端备案状态

        public ServiceResult<bool> UpdateZhongDuanBeiAnZhuangTai(UpdateZhongDuanBeiAnZhuangTaiDto dto)
        {
            try
            {
                #region 数据校验

                var userInfo = GetUserInfo();
                if (userInfo?.OrganizationType != (int)OrganizationType.市政府 &&
                    dto.beiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.取消备案)
                {
                    return new ServiceResult<bool>
                    { Data = false, StatusCode = 2, ErrorMessage = "市级主管部门才能对车辆进行操作" };
                }

                if (dto == null)
                {
                    return new ServiceResult<bool> { Data = false, StatusCode = 2, ErrorMessage = "请求参数不能为空" };
                }

                if (!dto.cheliangId.HasValue)
                {
                    return new ServiceResult<bool> { Data = false, StatusCode = 2, ErrorMessage = "车辆ID不能为空" };
                }

                if (!dto.beiAnZhuangTai.HasValue)
                {
                    return new ServiceResult<bool> { Data = false, StatusCode = 2, ErrorMessage = "备案状态不能为空" };
                }

                var carModel = _cheLiangXinXiRepository
                    .GetQuery(x => x.Id == dto.cheliangId && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (carModel == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "找不到对应的车辆信息", Data = false };
                }

                var id = dto.cheliangId.ToString();
                var confirmModel = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == id).FirstOrDefault();
                if (confirmModel == null)
                {
                    return new ServiceResult<bool>
                    { StatusCode = 2, Data = false, ErrorMessage = "只能对通过备案状态与不通过备案状态的车辆进行该操作" };
                }

                if (dto.beiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.取消备案 &&
                    confirmModel.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.通过备案 &&
                    confirmModel.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.不通过备案)
                {
                    return new ServiceResult<bool>
                    { StatusCode = 2, Data = false, ErrorMessage = "只能对通过备案与不通过备案状态的车辆进行该操作" };
                }
                #endregion

                //新增操作日志
                Mapper.CreateMap<CheLiangVideoZhongDuanConfirm, UpdateZhongDuanBeiAnZhuangTaiDto>();
                UpdateZhongDuanBeiAnZhuangTaiDto oldModel = Mapper.Map<UpdateZhongDuanBeiAnZhuangTaiDto>(confirmModel);
                List<LogUpdateValueDto> updateDetailList = OprateLogHelper.GetObjCompareString(oldModel, dto, true);
                ;
                var vehicleBindingList = _vehiclePartnershipBinding.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == 0 && x.LicensePlateNumber == carModel.ChePaiHao && x.LicensePlateColor == carModel.ChePaiYanSe
                    && (x.ZhuangTai == (int)VehicleCooperationStatus.审批通过 ||
                        x.ZhuangTai == (int)VehicleCooperationStatus.企业发起取消合作 ||
                        x.ZhuangTai == (int)VehicleCooperationStatus.第三方发起取消合作)).FirstOrDefault();
                #region 修改备案状态

                //修改档案中的备案状态
                confirmModel.BeiAnZhuangTai = dto.beiAnZhuangTai;
                confirmModel.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                confirmModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                confirmModel.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                confirmModel.NeiRong = dto.CancelRecordRemark;

                #endregion


                //2021-09-17
                //清远平台废弃服务商车辆档案
                //清远平台调整为车辆取消备案时将档案车辆删除 ——by_黄勇华&李永徽


                #region 数据持久化

                bool isSuccess = false;
                using (var u = _cheLiangVideoZhongDuanConfirmRepository.UnitOfWork)
                {
                    u.BeginTransaction();
                    //取消备案 车辆自动跟第三方机构解绑
                    if (vehicleBindingList != null)
                    {
                        vehicleBindingList.ZhuangTai = (int)VehicleCooperationStatus.取消合作;
                        vehicleBindingList.SYS_ZuiJinXiuGaiShiJian=DateTime.Now;
                        vehicleBindingList.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                        vehicleBindingList.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                        _vehiclePartnershipBinding.Update(vehicleBindingList);
                    }
                    //删除车辆
                    carModel.SYS_XiTongZhuangTai = 1;
                    carModel.SYS_XiTongBeiZhu = "车辆取消备案删除车辆";
                    carModel.BusinessHandlingResults = (int)VehicleQualificationStatus.不予办理;
                    _cheLiangXinXiRepository.Update(carModel);


                    _cheLiangVideoZhongDuanConfirmRepository.Update(confirmModel);

                    //修改车辆对应的终端信息
                    var gpsZhongduanModel = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.cheliangId.ToString()).ToList();
                    gpsZhongduanModel.ForEach(x =>
                    {
                        x.ZhongDuanMDT = "";
                        x.SIMKaHao = "";
                        x.SYS_XiTongBeiZhu = "车辆取消备案删除终端DMT与SIM卡信息";
                        x.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        //删除终端
                        x.SYS_XiTongZhuangTai = 1;
                        _cheLiangGPSZhongDuanXinXiRepository.Update(x);

                    });
                    var videoZhongduanModel = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == dto.cheliangId.ToString()).ToList();
                    videoZhongduanModel.ForEach(x =>
                    {
                        x.ZhongDuanMDT = "";
                        x.SYS_XiTongBeiZhu = "车辆取消备案删除终端DMT信息";
                        x.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        //删除终端
                        x.SYS_XiTongZhuangTai = 1;
                        _cheLiangVideoZhongDuanXinXiRepository.Update(x);

                    });
                    //同步备案状态到服务商车辆列表中
                    if (!string.IsNullOrWhiteSpace(carModel.FuWuShangOrgCode))
                    {
                        var fuWuShangCheLiang = _fuWuShangCheLiangRepository.GetQuery(x =>
                                x.SYS_XiTongZhuangTai == 0 && x.ChePaiHao == carModel.ChePaiHao &&
                                x.ChePaiYanSe == carModel.ChePaiYanSe &&
                                x.FuWuShangOrgCode == carModel.FuWuShangOrgCode)
                            .FirstOrDefault();
                        if (fuWuShangCheLiang != null)
                        {
                            _fuWuShangCheLiangRepository.Update(x => x.Id == fuWuShangCheLiang.Id, y =>
                                new FuWuShangCheLiang()
                                {
                                    BeiAnZhuangTai = dto.beiAnZhuangTai,
                                    ZuiJinXiuGaiRenOrgCode = userInfo == null ? "" : userInfo.OrganizationCode,
                                    SYS_ZuiJinXiuGaiRen = userInfo == null ? "" : userInfo.UserName,
                                    SYS_ZuiJinXiuGaiShiJian = DateTime.Now,
                                    SYS_ZuiJinXiuGaiRenID = userInfo == null ? "" : userInfo.Id
                                });
                        }
                    }

                    isSuccess = u.CommitTransaction() > 0;

                }

                #endregion

                if (isSuccess)
                {
                    LogHelper.Info(userInfo?.UserName + "修改车辆" + carModel.ChePaiHao + "[" + carModel.ChePaiYanSe +
                                   "]备案状态=》" + dto.beiAnZhuangTai);
                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                        ActionName = nameof(UpdateZhongDuanBeiAnZhuangTai),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = "车辆档案取消备案：" + carModel?.ChePaiHao + "[" + carModel.ChePaiYanSe + "]",
                        OperatorName = userInfo.UserName,
                        NewBizContent = JsonConvert.SerializeObject(confirmModel),
                        OperatorID = userInfo.Id,
                        OperatorOrgCode = userInfo.OrganizationCode,
                        OperatorOrgName = userInfo.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                        ExtendInfo = JsonConvert.SerializeObject(updateDetailList)
                    });
                    return new ServiceResult<bool> { Data = true };
                }
                else
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "修改失败", StatusCode = 2 };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("修改车辆备案状态出错" + ex.Message, ex);
                return new ServiceResult<bool> { Data = false, ErrorMessage = "修改出错" };
            }
        }

        #endregion

        #region 服务商提交车辆备案审核

        public ServiceResult<bool> UpdateVehicleRecordState(Guid? cheLiangID)
        {
            try
            {
                if (cheLiangID == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "请选择需要提交申请的车辆" };
                }

                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "获取登录信息失败,请重新登录", StatusCode = 2 };
                }

                var vehicleInfo = _cheLiangXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == cheLiangID).FirstOrDefault();
                if (vehicleInfo == null) return new ServiceResult<bool> { Data = false, ErrorMessage = "获取车辆信息失败" };
                bool isCreate = false;
                var vehcileId = vehicleInfo.Id.ToString();
                var vehicleConfirmInfo = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == vehcileId).FirstOrDefault();
                var vehicleVideoZhongDuanInfo = _cheLiangVideoZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == vehcileId).FirstOrDefault();
                var vehicleGpsZhongDuanInfo = _cheLiangGPSZhongDuanXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == vehcileId).FirstOrDefault();
                if (vehicleConfirmInfo != null &&
                    vehicleConfirmInfo?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.取消备案 &&
                    vehicleConfirmInfo?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.不通过备案 &&
                    vehicleConfirmInfo?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.待提交)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao+"未审核、通过备案的车辆不能提交备案" };

                }

                if (vehicleVideoZhongDuanInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao+"智能视频终端信息未保存，请保存智能视频终端信息" };
                }

                if (vehicleGpsZhongDuanInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao + "Gps终端信息未保存，请保存Gps终端信息" };

                }

                if (string.IsNullOrWhiteSpace(vehicleGpsZhongDuanInfo.ZhongDuanMDT))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao + "Gps终端MDT不能为空" };
                }

                if (string.IsNullOrWhiteSpace(vehicleGpsZhongDuanInfo.SIMKaHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao + "Gps终端SIM卡号不能为空" };
                }

                if (string.IsNullOrWhiteSpace(vehicleVideoZhongDuanInfo.ZhongDuanMDT))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = vehicleInfo.ChePaiHao + "智能视频终端MDT不能为空" };
                }

                if (vehicleConfirmInfo == null)
                {
                    vehicleConfirmInfo = new CheLiangVideoZhongDuanConfirm
                    {
                        Id = Guid.NewGuid(),
                        CheLiangId = vehcileId,
                        ZhongDuanId = vehicleVideoZhongDuanInfo.Id.ToString(),
                        SYS_ChuangJianRenID = userInfo?.Id,
                        SYS_ChuangJianRen = userInfo?.UserName,
                        SYS_ChuangJianShiJian = DateTime.Now,
                        SYS_XiTongZhuangTai = 0,
                    };
                    isCreate = true;
                }

                vehicleConfirmInfo.BeiAnZhuangTai = (int)ZhongDuanBeiAnZhuangTai.未审核;
                vehicleConfirmInfo.SYS_ZuiJinXiuGaiRen = userInfo?.UserName;
                vehicleConfirmInfo.SYS_ZuiJinXiuGaiRenID = userInfo?.Id;
                vehicleConfirmInfo.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                vehicleConfirmInfo.TiJiaoBeiAnShiJian = DateTime.Now;

                bool isSuccess = false;
                using (var u = _cheLiangVideoZhongDuanConfirmRepository.UnitOfWork)
                {

                    u.BeginTransaction();
                    if (isCreate)
                    {
                        _cheLiangVideoZhongDuanConfirmRepository.Add(vehicleConfirmInfo);
                    }
                    else
                    {
                        _cheLiangVideoZhongDuanConfirmRepository.Update(vehicleConfirmInfo);
                    }

                    isSuccess = u.CommitTransaction() > 0;
                }

                if (isSuccess)
                {
                    OperLogHelper.WriteOperLog(new OperationLogRequestDto()
                    {
                        SystemName = OprateLogHelper.GetSysTemName(),
                        ModuleName = OperLogModuleName.车辆档案.GetDesc(),
                        ActionName = nameof(Create),
                        BizOperType = OperLogBizOperType.UPDATE,
                        ShortDescription = "车辆档案提交审核：" + vehicleInfo?.ChePaiHao + "[" + vehicleInfo.ChePaiYanSe + "]",
                        NewBizContent = JsonConvert.SerializeObject(vehicleConfirmInfo),
                        OperatorName = userInfo.UserName,
                        OperatorID = userInfo.Id,
                        OperatorOrgCode = userInfo.OrganizationCode,
                        OperatorOrgName = userInfo.OrganizationName,
                        SysID = SysId,
                        AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"],
                    });
                }

                return new ServiceResult<bool> { Data = isSuccess };


            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆档案提交备案出错" + ex.Message + "CheLiangId" + cheLiangID, ex);
                return new ServiceResult<bool> { Data = false, StatusCode = 2 };
            }



        }

        #endregion

        #endregion

        #region  导出相关

        private string CreateExcel(string TimeFormat, List<CheLiangSearchResponseDto> Rows,
            Dictionary<string, string> Cols)
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

        private void WriteExcel(string FilePath, List<CheLiangSearchResponseDto> Rows, Dictionary<string, string> Cols)
        {
            PagingUtil<CheLiangSearchResponseDto> pu = new PagingUtil<CheLiangSearchResponseDto>(Rows, 60000);
            string[] colsName = Cols.Keys.ToArray();
            NopiExcel nopi = new NopiExcel(0);
            while (pu.IsEffectivePage)
            {
                List<CheLiangSearchResponseDto> pageData = pu.GetCurrentPage();
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
                            nopi.WriteCell(startRow, j,
                                StringFormat(pageData[i].GetType().GetProperty(key).GetValue(pageData[i])?.ToString()));

                            // 车辆种类
                            if ("CheLiangZhongLei".Equals(key))
                            {
                                var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);

                                nopi.WriteCell(startRow, j, ChangeCheLiangZhongLei((int?)temp));
                            }

                            if ("BeiAnZhuangTai".Equals(key))
                            {
                                var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);

                                nopi.WriteCell(startRow, j, GetBeiAnZhuangTai((int?)temp));
                            }

                            if ("BeiAnShenHeShiJian".Equals(key))
                            {
                                var timeStr = "";
                                var beiAnZhuangTai = pageData[i].GetType().GetProperty("BeiAnZhuangTai")
                                    .GetValue(pageData[i]);
                                var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);
                                if (Convert.ToInt32(beiAnZhuangTai) == 1)
                                {
                                    try
                                    {
                                        timeStr = Convert.ToDateTime(temp).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }

                                nopi.WriteCell(startRow, j, timeStr);
                            }

                            if ("ShuJuTongXunBanBenHao".Equals(key))
                            {
                                var temp = pageData[i].GetType().GetProperty(key).GetValue(pageData[i]);
                                int? value = (int?)temp;
                                if (value.HasValue)
                                    nopi.WriteCell(startRow, j,
                                        ((ZhongDuanShuJuTongXunBanBenHao)value).GetDescription());
                                else nopi.WriteCell(startRow, j, "");
                            }
                        }
                    }
                }

                nopi.SetVerticalCenter(0, 0, nopi.getLastRowNum(), nopi.getLastCellNum(0)); //垂直居中
                nopi.SetFontFamliy(0, 0, nopi.getLastRowNum(), nopi.getLastCellNum(0), 12 * 20, "宋体", "", 0);
                nopi.SetColumnWidth(0, nopi.getLastCellNum(0), 18 * 300); //列宽
                pu.GotoNextPage();
            }

            nopi.Save(FilePath);
        }

        private string StringFormat(object obj)
        {
            return String.Format("{0}", obj);
        }

        private string StringFormatTime(DateTime? obj)
        {
            var a = obj?.ToString("yyyy-MM-dd");
            return String.Format("{0}", obj?.ToString("yyyy-MM-dd")?.Substring(0, 10));
        }

        private string StringFormatTime1(DateTime? obj)
        {
            var a = obj?.ToString("yyyy-MM-dd HH:mm:ss");
            return String.Format("{0}", obj?.ToString("yyyy-MM-dd HH:mm:ss"));
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
                case 10:
                    return "校车";
                default:
                    return "其他车辆";

            }
        }

        private string GetBeiAnZhuangTai(int? beiAnZhuangTai)
        {
            string beiAnZhuangTaiStr = "";
            if (beiAnZhuangTai.HasValue)
            {
                beiAnZhuangTaiStr = typeof(ZhongDuanBeiAnZhuangTai).GetEnumName(beiAnZhuangTai);
            }

            return beiAnZhuangTaiStr;
        }




        #endregion

        #region 获取车辆备案人工审核参考信息

        public ServiceResult<VehicleAnnualReviewResultDto> GetVehicelAnnualReview(Guid cheLiangId)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<VehicleAnnualReviewResultDto>
                    { StatusCode = 2, ErrorMessage = "获取用户登录信息失败，请重新登陆" };
                }

                //车辆基本信息
                var vehicleModel = _cheLiangXinXiRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == cheLiangId).FirstOrDefault();
                if (vehicleModel == null)
                {
                    return new ServiceResult<VehicleAnnualReviewResultDto>
                    { StatusCode = 2, ErrorMessage = "查询车辆信息失败，请重新查询" };
                }

                //车辆终端核查信息
                string cheLiangIdStr = cheLiangId.ToString();
                var vehicleCheckInfo = _cheLiangVideoZhongDuanConfirmRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == cheLiangIdStr).FirstOrDefault();
                if (vehicleCheckInfo == null)
                {
                    return new ServiceResult<VehicleAnnualReviewResultDto> { StatusCode = 2, ErrorMessage = "车辆未提交备案" };
                }

                VehicleAnnualReviewResultDto resultModel = new VehicleAnnualReviewResultDto()
                {
                    ChePaiHao = vehicleModel.ChePaiHao,
                    ChePaiYanSe = vehicleModel.ChePaiYanSe,
                    TiJiaoBeiAnShiJian = vehicleCheckInfo.TiJiaoBeiAnShiJian ?? vehicleCheckInfo.SYS_ChuangJianShiJian,
                };
                //读取定位信息
                GetVehicelGpsInfo(resultModel);
                //读取报警信息
                var beginDateTime = resultModel.TiJiaoBeiAnShiJian;
                var endDateTime = DateTime.Now;
                if (beginDateTime == null)
                {
                    resultModel.IsHavVideoAlarmAttachment = false;
                }
                else
                {
                    resultModel.IsHavVideoAlarmAttachment = GetVehicleAlarmInfo(beginDateTime.Value, endDateTime,
                        vehicleModel.ChePaiHao, vehicleModel.ChePaiYanSe, 1, 50);
                }
                return new ServiceResult<VehicleAnnualReviewResultDto> { Data = resultModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取车辆年审指标信息出错{ex.Message}", ex);
                return new ServiceResult<VehicleAnnualReviewResultDto> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        private void AddVehicleGpsInfoForList(List<CheLiangSearchResponseDto> list)
        {
            try
            {
                //分批进行查询
                int QueryRows = 100;
                int indexSize = (int)Math.Ceiling((decimal)(list.Count() / QueryRows)) + 1;

                List<ResultVehicleGpsInfoDto> gpsInfoList = new List<ResultVehicleGpsInfoDto>();

                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = 3;
                Parallel.For(0, indexSize, parallelOptions, y =>
                {

                    var queryList = list.Skip((y * QueryRows)).Take(QueryRows).Select(x => x.ChePaiHao + x.ChePaiYanSe)
                        .ToList();

                    //加载数据
                    var queryParam = new
                    {
                        VehicleKeys = queryList
                    };
                    var queryRequest = GetInvokeRequest("006600100001", "1.0", queryParam);
                    if (queryRequest != null)
                    {
                        if (queryRequest.publicresponse.statuscode == 0)
                        {
                            gpsInfoList.AddRange(
                                JsonConvert.DeserializeObject<List<ResultVehicleGpsInfoDto>>(
                                    JsonConvert.SerializeObject(queryRequest.body.items)));
                        }
                        else
                        {
                            LogHelper.Error(
                                $"车辆档案查询车辆最近定位信息006600100001返回响应错误{JsonConvert.SerializeObject(queryRequest)}");
                        }
                    }
                    else
                    {
                        LogHelper.Error("车辆档案查询车辆最近定位信息006600100001返回结果为空");
                    }
                });


                list.ForEach(x =>
                {
                    var gpsMpdel = gpsInfoList
                        .Where(y => y.RegistrationNo == x.ChePaiHao && y.RegistrationNoColor == x.ChePaiYanSe)
                        .FirstOrDefault();
                    if (gpsMpdel != null)
                    {
                        x.GpsTime = gpsMpdel.GpsTime;
                        x.IsHavGpsInfo = x.GpsTime.HasValue;
                    }
                });

            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆档案查询补充车辆定位信息出错" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取车辆最新定位信息
        /// </summary>
        /// <param name="dto"></param>
        private void GetVehicelGpsInfo(VehicleAnnualReviewResultDto dto)
        {
            try
            {
                //加载数据
                var queryParam = new
                {
                    VehicleKeys = $"['{dto.ChePaiHao + dto.ChePaiYanSe}']"
                };
                var queryRequest = GetInvokeRequest("006600100001", "1.0", queryParam);
                if (queryRequest != null)
                {
                    if (queryRequest.publicresponse.statuscode == 0)
                    {
                        List<ResultVehicleGpsInfoDto> gpsInfoModel =
                            JsonConvert.DeserializeObject<List<ResultVehicleGpsInfoDto>>(
                                JsonConvert.SerializeObject(queryRequest.body.items));
                        if (gpsInfoModel != null)
                        {
                            var gpsInfo = gpsInfoModel.FirstOrDefault();
                            dto.IsHavVehicleTrack = gpsInfo != null;
                            dto.VehicelLastPositioningTime = gpsInfo?.GpsTime;
                        }
                    }
                    else
                    {
                        LogHelper.Error($"车辆档案查询车辆最近定位信息006600100001返回响应错误{JsonConvert.SerializeObject(queryRequest)}");
                    }
                }
                else
                {
                    LogHelper.Error("车辆档案查询车辆最近定位信息006600100001返回结果为空");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取车辆定位信息出错{ex.Message}", ex);
            }
        }


        private bool GetVehicleAlarmInfo(DateTime beginDateTime, DateTime endDateTime, string chePaiHao,
            string chePaiYanSe, int page, int rows)
        {
            try
            {
                foreach (var entity in EventType)
                {
                    var end = Convert.ToDateTime(endDateTime.ToShortDateString());
                    var timeStamp = end.Subtract(beginDateTime);
                    var day = timeStamp.Days;
                    var endTime = DateTime.Now;

                    var baseTime = DateTime.Now.AddMonths(-3);
                    var timeStampBase = end.Subtract(baseTime).Days;

                    if (day >= timeStampBase)
                    {
                        beginDateTime = endDateTime.AddMonths(-3);
                        endTime = endTime.AddMonths(-3).AddDays(+10);
                        day = timeStampBase;
                    }
                    var count = day / 10;
                    var again = day % 10;
                    var lastTime = DateTime.Now;
                    for (var i = 0; i < count; i++)
                    {
                        lastTime = endTime.AddDays(+(i * 10));
                        var vehicleEnclosure = VehicleEnclosure(beginDateTime.AddDays(+(i * 10)), lastTime, chePaiHao, chePaiYanSe,
                            page,
                            rows, entity);
                        if (vehicleEnclosure)
                        {
                            return true;
                        }
                    }
                    if (again <= 0) continue;
                    {
                        var vehicleEnclosure = VehicleEnclosure(lastTime, DateTime.Now, chePaiHao, chePaiYanSe,
                            page,
                            rows, entity);
                        if (vehicleEnclosure)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool VehicleEnclosure(DateTime beginDateTime, DateTime endDateTime, string chePaiHao,
            string chePaiYanSe, int page, int rows, string eventTypeCode)
        {
            var queryAlarmParam = new
            {
                Page = page,
                Rows = rows,
                data = new
                {
                    RegistrationNo = chePaiHao,
                    RegistrationNoColor = chePaiYanSe,
                    DateStart = beginDateTime.ToString(),
                    DateEnd = endDateTime.ToString(),
                    AppCode = "0033",
                    EventTypeCode = eventTypeCode
                }
            };
            try
            {
                var queryRequest = GetInvokeRequest("006600300002", "1.0", queryAlarmParam);
                if (queryRequest != null)
                {
                    if (queryRequest.publicresponse.statuscode == 0)
                    {
                        if (queryRequest.body.totalcount > 0)
                        {
                            List<EventInfoDto> alarmsList =
                                JsonConvert.DeserializeObject<List<EventInfoDto>>(
                                    JsonConvert.SerializeObject(queryRequest.body.items));
                            for (var i = 0; i < alarmsList.Count(); i++)
                            {
                                if (alarmsList[i].Content.ContainsKey("VideoUrls"))
                                {
                                    if (!string.IsNullOrWhiteSpace(alarmsList[i].Content["VideoUrls"].ToString()))
                                    {
                                        return true;
                                    }
                                }
                                if (alarmsList[i].Content.ContainsKey("ImageUrls"))
                                {
                                    if (!string.IsNullOrWhiteSpace(alarmsList[i].Content["ImageUrls"].ToString()))
                                    {
                                        return true;
                                    }
                                }
                            }
                            //没有满足条件的数据，继续查询
                            if (queryRequest.body.totalcount > page * rows)
                            {
                                return VehicleEnclosure(beginDateTime, endDateTime, chePaiHao, chePaiYanSe, page + 1,
                                    rows, eventTypeCode);
                            }
                        }
                        return false;
                    }
                    LogHelper.Error(
                        $"查询车辆智能视频附件信息接口返回异常,请求报文{JsonConvert.SerializeObject(queryAlarmParam)}，响应报文{JsonConvert.SerializeObject(queryRequest)}");
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error(
                    $"获取车辆是否存在智能视频报警附件信息出错{ex.Message},请求报文{JsonConvert.SerializeObject(queryAlarmParam)}", ex);
                return false;
            }
        }

        class ResultVehicleGpsInfoDto
        {
            public string RegistrationNo { get; set; }

            public string RegistrationNoColor { get; set; }

            public DateTime? GpsTime { get; set; }
        }

        #endregion



        #region 打印GPS接入证明相关接口

        /// <summary>
        /// 打印GPS接入证明
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<GpsAccessInformation> GpsImportAccessCertificate(string id)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<GpsAccessInformation> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }

                var list = GetGpsImportAccessCertificate(id);
                return list.IsSuccess
                    ? new ServiceResult<GpsAccessInformation>
                    { Data = new GpsAccessInformation { FileId = list.FileId, IsSuccess = true } }
                    : new ServiceResult<GpsAccessInformation> { ErrorMessage = list.ErrorMsg, StatusCode = 2 };
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出取消备案车辆档案信息出错" + ex.Message, ex);
                return new ServiceResult<GpsAccessInformation> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        public GpsAccessInformation GetGpsImportAccessCertificate(string id)
        {
            try
            {
                var returnInformation = new GpsAccessInformation();
                var vehicleId = Guid.Parse(id);
                //车辆信息 
                var vehicle =
                    _cheLiangXinXiRepository.GetQuery(p => p.Id == vehicleId && p.SYS_XiTongZhuangTai == 0)
                        .FirstOrDefault();
                //车架号
                if (vehicle == null)
                {
                    returnInformation.IsSuccess = false;
                    returnInformation.ErrorMsg = "车辆信息错误！";
                    return returnInformation;
                }

                var frameNo = vehicle.CheJiaHao;
                //获取车辆定位信息
                var resultModel = new VehicleAnnualReviewResultDto()
                {
                    ChePaiHao = vehicle.ChePaiHao,
                    ChePaiYanSe = vehicle.ChePaiYanSe
                };
                var serviceProvider = _fuWuShangRepository
                    .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == vehicle.FuWuShangOrgCode)
                    .FirstOrDefault();
                //服务商名称
                var serviceName = string.Empty;
                if (serviceProvider != null)
                {
                    serviceName = serviceProvider.OrgName;
                }

                //企业名称
                var enterpriseName = string.Empty;
                var enterprise = _cheLiangYeHuRepository
                    .GetQuery(s => s.SYS_XiTongZhuangTai == 0 && s.OrgCode == vehicle.YeHuOrgCode).FirstOrDefault();
                if (enterprise != null)
                {
                    enterpriseName = enterprise.OrgName;
                }

                //gps终端信息
                var gpsConfigure =
                    _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常
                        && x.CheLiangId == id).FirstOrDefault();
                //企业联系人电话
                var enterpriseInformation = EnterpriseInformation.FindAll(x => x.QiYeMingCheng == enterpriseName)
                    .FirstOrDefault();
                //读取定位信息
                GetVehicelGpsInfo(resultModel);
                if (resultModel.VehicelLastPositioningTime == null ||
                    resultModel.VehicelLastPositioningTime.Value.Date != DateTime.Now.Date)
                {
                    returnInformation.IsSuccess = false;
                    returnInformation.ErrorMsg = "该车辆没有及时上报定位信息，不允许打印，请落实处理后另行打印！";
                    return returnInformation;
                }

                var num = GetNumber("道路运输车辆接入证明");
                num = num.PadLeft(6, '0');
                var applyTime = DateTime.Now.ToString("D");
                var fileName = $"{vehicleId}_{DateTime.Now:yyyyMMddHHmmssfff}.pdf";
                var QRCodeFileName = "道路运输车辆数据接入证明二维码";
                var localFileAddress = ConfigurationManager.AppSettings["VISSPApiFileUrl"];
                var QRCodeSrc = BarCodeHelper.WriteQRCode(
                    $"{localFileAddress}/{UrlCollection.XingZhengWenShuFile}/{fileName}", 12, QRCodeFileName);
                if (string.IsNullOrEmpty(QRCodeSrc))
                {
                    returnInformation.IsSuccess = false;
                    returnInformation.ErrorMsg = "生成二维码失败！";
                    return returnInformation;
                }

                var path = ("~" + UrlCollection.ImagesFile).MapPath();
                var officialSealSrc = Path.Combine(path, "OfficialSeal.gif");
                var dataUrl = ConfigurationManager.AppSettings["PrintGpsInstallCertUrl"];
                var htmlContent = @"<html>
<head>
<style> 
    .row {font-size: 20px;line-height: 30px;} label {font-size: 20px;} 
table td{
   border: 1px solid #3C3C3C; 
   font-size: 20px;line-height: 30px;
}
table{
   border-collapse:collapse;
   border: 1px solid #3C3C3C; 
}
</style>
</head>
<body>
<div id='printContent' style='width:100%;margin-left:10px;margin-right:10px;font-family:STFangsong'>
    <div style='height:10px;'>&nbsp;</div>
    <div style='font-weight:bold;font-size:30px;text-align:center;margin-top:10px;'>清远市道路运输车辆卫星定位数据接入备案表</div>
    <div style='font-weight:bold;font-size:28px;text-align:center;margin-top:10px;'>（" + DateTime.Now.Year + @"年）</div>
    <div style='height:50px;'>&nbsp;</div>
<div style='width:920px;margin:0 auto;'>
    <div class='row' style='height:50px' >
        <div style='text-align:left;float:left;'>编号：" + num + @"</div>
        <div style='text-align:right;float:right;'>备案表生成时间：" + applyTime + @"</div>
    </div>

<table style='width:100%;' >
    <tbody>
        <tr>
            <td >运输企业或车主名称</td>
             <td colspan='3'>" + enterpriseName + @"</td>
        </tr>
        <tr>
            <td>联系电话</td>
            <td colspan='3' >" + enterpriseInformation?.Telephone + @"</td>
        </tr>
        <tr>
            <td style='width:180px;'>车牌号码</td><td >" + vehicle.ChePaiHao + @"</td>
            <td style='width:160px;'>车牌颜色</td><td >" + vehicle.ChePaiYanSe + @"</td>
        </tr>
        <tr>
            <td>设备编码</td> <td></td>
            <td>MDT</td> <td>" + gpsConfigure?.ZhongDuanMDT + @"</td>
        </tr>
        <tr>
            <td>SIM卡号</td> <td>" + gpsConfigure?.SIMKaHao + @"</td>
            <td>设备型号</td> <td>" + gpsConfigure?.SheBeiXingHao + @"</td>
        </tr>
        <tr>
            <td>车架号</td> <td>" + frameNo + @"</td>
            <td>厂家编号</td> <td>" + gpsConfigure?.ChangJiaBianHao + @"</td>
        </tr>
        <tr>
            <td >第三方编号</td> <td></td>
            <td >第三方名称</td> <td>" + serviceName + @"</td>
        </tr>
    </tbody>
</table>

    <div class='row' style='font-size:24px;text-align:left;margin-top:30px;margin-left:50px;'>该车辆卫星定位装置数据已接入市交通运输局监管平台。</div>
    <div class='row' style='font-size:24px;text-align:left;margin-top:10px;margin-left:50px;'>数据查询网址：" + dataUrl +
                                  @"</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div style='width:280px; height:260px;float:right;margin-top:30px;'>
             <div class='row' style='margin-top:60px;margin-left:5px'>清远市交通运输局(盖章)</div>
             <div class='row' style='margin-top:0px;margin-left:80px'>" +
                                  applyTime
                                  + @"</div>"
                                  + @" <div style='width:220px; height:150px;z-index:2; margin-top:-154px; margin-left:0px' ></div>
                <div style='float:right; margin-top:10px;margin-right:100px'>
                <img style='width:110px; height:110px' src='" + QRCodeSrc + @"'></img>
            </div>
        </div>
 </div>
 </div></body></html>";

                dynamic result = CommonHelper.CreatePdfAndUploadPdfDoc(htmlContent, fileName);
                if (result.success)
                {
                    returnInformation.IsSuccess = true;
                    returnInformation.ErrorMsg = "接入成功！";
                    returnInformation.FileId = Guid.Parse(result.fileId);
                    return returnInformation;
                }

                returnInformation.IsSuccess = false;
                returnInformation.ErrorMsg = "接入失败！";
                return returnInformation;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"打印GPS接入证明失败{ex.Message}", ex);
                return null;
            }
        }

        public string GetNumber(string name)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
            {
                try
                {
                    var sql = $@"
  SELECT IDVal FROM  DC_GPSJCDAGL.[dbo].[IDTbl]
  WHERE IDName='{name}'";
                    var gpsInformation = conn.Query<GpsInformation>(sql, null).FirstOrDefault();
                    if (gpsInformation != null)
                    {
                        var implement = $@"  UPDATE   DC_GPSJCDAGL.[dbo].[IDTbl]
  SET IDVal=IDVal+1
   WHERE IDName='{name}' ";
                        conn.ExecuteScalar<int>(implement, null);
                        return gpsInformation.IDVal.ToString();
                    }
                    else
                    {
                        return "0";
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return "";
        }

        #endregion

        #region  GPS自动审核功能

        /// <summary>
        /// GPS自动审核功能
        /// </summary>
        /// <returns></returns>
        public ServiceResult<bool> AuditAuto()
        {
            try
            {
                if (IsAuditWorking)
                    return new ServiceResult<bool> { ErrorMessage = "task is working", StatusCode = 2 };
                lock (workingObj)
                {
                    IsAuditWorking = true;
                }

                var vehicleList =
                    _cheLiangXinXiRepository.GetQuery(p =>
                            p.SYS_XiTongZhuangTai == 0 && p.GPSAuditStatus != (int)GPSAuditStatus.通过备案
                                                       && p.XiaQuShi == "清远")
                        .OrderByDescending(x => x.SYS_ZuiJinXiuGaiShiJian)
                        .ToList();
                foreach (var vehicle in vehicleList)
                {
                    var vehicleId = vehicle.Id.ToString();
                    //车辆 备案状态为 待提交的 不进行GPS审核
                    var cheLiangVideoZhongDuanConfirmList =
                        _cheLiangVideoZhongDuanConfirmRepository
                            .GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == vehicleId).FirstOrDefault();
                    if (cheLiangVideoZhongDuanConfirmList == null || cheLiangVideoZhongDuanConfirmList.BeiAnZhuangTai ==
                        (int)ZhongDuanBeiAnZhuangTai.待提交)
                        continue;
                    {
                        var keyValueItemValidDic = new Dictionary<string, KeyValueItemValid>();
                        var enterpriseName = string.Empty;
                        var enterprise = _cheLiangYeHuRepository
                            .GetQuery(s => s.SYS_XiTongZhuangTai == 0 && s.OrgCode == vehicle.YeHuOrgCode)
                            .FirstOrDefault();
                        if (enterprise != null)
                        {
                            enterpriseName = enterprise.OrgName;
                        }

                        //gps终端信息
                        var gpsConfigure =
                            _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x =>
                                x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常
                                && x.CheLiangId == vehicleId).FirstOrDefault();
                        var vehicleGpsInfo = new VehicleGpsInfoDto
                        {
                            RegistrationNo = vehicle.ChePaiHao,
                            RegistrationNoColor = vehicle.ChePaiYanSe,
                            XiaQuShi = vehicle.XiaQuShi,
                            XiaQuXian = vehicle.XiaQuXian,
                            CheJiaHao = vehicle.CheJiaHao,
                            YeHuMingCheng = enterpriseName,
                            MDT = gpsConfigure?.ZhongDuanMDT,
                            M1 = gpsConfigure?.M1,
                            IA1 = gpsConfigure?.IA1,
                            IC1 = gpsConfigure?.IC1,
                            DeviceMode = gpsConfigure?.SheBeiXingHao,
                            SimNo = gpsConfigure?.SIMKaHao,
                            TerminalType = gpsConfigure?.ZhongDuanLeiXing
                        };
                        //车辆种类
                        if (vehicle.CheLiangZhongLei.HasValue)
                        {
                            vehicleGpsInfo.CheLiangZhongLei =
                                typeof(CheLiangZhongLei).GetEnumName(vehicle.CheLiangZhongLei);
                        }

                        //设备型号
                        if (gpsConfigure?.ZhongDuanLeiXing != null)
                        {
                            vehicleGpsInfo.TerminalTypeName =
                                typeof(ZhongDuanLeiXing).GetEnumName(gpsConfigure.ZhongDuanLeiXing);
                        }

                        ValidVehicleInfoBaseInfo(vehicleGpsInfo, keyValueItemValidDic);
                        ValidVehicleDynamicInfo(vehicleGpsInfo, keyValueItemValidDic);
                        var calResult = CalculateResult(keyValueItemValidDic);
                        var gpsAuditRecord = new GPSAuditRecord
                        {
                            Id = Guid.NewGuid(),
                            FiledComment = calResult.Item2,
                            FiledDate = DateTime.Now,
                            ResultComment = JsonConvert.SerializeObject(keyValueItemValidDic),
                            GPSAuditStatus = calResult.Item1 ? (int)GPSAuditStatus.通过备案 : (int)GPSAuditStatus.未通过备案,
                            VehicleId = vehicleId
                        };
                        var result = GpsAuditRecord(gpsAuditRecord, vehicle);
                        if (!result)
                        {
                            LogHelper.Error("GPS自动审核功能出错" + vehicleGpsInfo.RegistrationNo);
                        }
                    }
                }

                return new ServiceResult<bool> { StatusCode = 0 };
            }
            catch (Exception ex)
            {
                LogHelper.Error("GPS自动审核功能出错" + ex.Message, ex);
                return new ServiceResult<bool> { ErrorMessage = "GPS自动审核功能出错", StatusCode = 2 };
            }
            finally
            {
                lock (workingObj)
                {
                    IsAuditWorking = false;
                }
            }
        }

        /// <summary>
        /// 新增gps审核结果
        /// </summary>
        /// <param name="vehicleGpsInfo"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public bool GpsAuditRecord(GPSAuditRecord vehicleGpsInfo, CheLiang vehicle)
        {
            bool isSuccess;
            lock (CWHelper.GetStringLock(vehicleGpsInfo.VehicleId, "GpsAuditRecord"))
            {
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    var preEntity = _iGPSAuditRecordRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int) XiTongZhuangTaiEnum.正常
                        && x.VehicleId == vehicleGpsInfo.VehicleId).FirstOrDefault();
                    if (preEntity == null)
                    {
                        vehicleGpsInfo.SYS_ChuangJianRen = "系统自动生成";
                        vehicleGpsInfo.SYS_ChuangJianRenID = string.Empty;
                        vehicleGpsInfo.SYS_ChuangJianShiJian = DateTime.Now;
                        vehicleGpsInfo.SYS_XiTongZhuangTai = (int) XiTongZhuangTaiEnum.正常;
                        //新增gps审核信息基本信息
                        _iGPSAuditRecordRepository.Add(vehicleGpsInfo);
                    }
                    else
                    {
                        preEntity.FiledComment = vehicleGpsInfo.FiledComment;
                        preEntity.FiledDate = vehicleGpsInfo.FiledDate;
                        preEntity.ResultComment = vehicleGpsInfo.ResultComment;
                        preEntity.GPSAuditStatus = vehicleGpsInfo.GPSAuditStatus;
                        preEntity.SYS_ZuiJinXiuGaiRen = "系统自动生成";
                        preEntity.SYS_ZuiJinXiuGaiRenID = string.Empty;
                        preEntity.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        //新增gps审核信息基本信息
                        _iGPSAuditRecordRepository.Update(preEntity);
                    }

                    if (vehicleGpsInfo.GPSAuditStatus == (int) GPSAuditStatus.通过备案)
                    {
                        vehicle.GPSAuditStatus = (int) GPSAuditStatus.通过备案;
                    }
                    else
                    {
                        vehicle.GPSAuditStatus = (int) GPSAuditStatus.未通过备案;
                    }

                    _cheLiangXinXiRepository.Update(vehicle);
                    isSuccess = uow.CommitTransaction() > 0;
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// gps审核结果详情
        /// </summary>
        /// <param name="tempList"></param>
        /// <returns></returns>
        private Tuple<bool, string> CalculateResult(Dictionary<string, KeyValueItemValid> tempList)
        {
            var contrast = new StringBuilder();
            var auditResult = true;
            var result = string.Empty;
            //不符合要求说明
            var nonConformance = new StringBuilder();
            foreach (var item in tempList.Values)
            {
                if (item.Key.Equals("FiledComment") || item.Key.Equals("FiledComment") ||
                    item.Key.Equals("FiledResult"))
                    continue;
                if (item.Result)
                {
                    result = "符合要求";
                }
                else if (!string.IsNullOrEmpty(item.FiledStander))
                {
                    result = "不符合要求";
                    auditResult = false;
                    nonConformance.Append($"{item.FiledStander} 结果：{result} 说明：{item.Description}；");
                }

                if (!string.IsNullOrEmpty(item.FiledStander))
                {
                    contrast.Append($"备案指标：{item.FiledStander} 结果：{result} 说明：{item.Description}；");
                }
            }

            var key = "FiledComment";
            if (!string.IsNullOrEmpty(nonConformance.ToString()) && tempList.ContainsKey(key))
            {
                tempList[key].Result = true;
                tempList[key].Value = nonConformance.ToString();
            }

            key = "FiledResult";
            if (!tempList.ContainsKey(key))
            {
                tempList.Add(key, new KeyValueItemValid());
            }

            if (!auditResult)
            {
                tempList[key].Result = false;
                tempList[key].Value = "不符合要求，未通过备案";
            }
            else
            {
                tempList[key].Result = true;
                tempList[key].Value = "符合要求，通过备案";
            }

            var tp = Tuple.Create<bool, string>(auditResult, contrast.ToString());
            return tp;
        }

        /// <summary>
        /// 车辆基础信息验证
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="tempList"></param>
        private void ValidVehicleInfoBaseInfo(VehicleGpsInfoDto vehicle, Dictionary<string, KeyValueItemValid> tempList)
        {
            #region 初始化验证属性(验证输入项的数据是否为空)

            //车牌号
            foreach (System.Reflection.PropertyInfo itemPrperty in vehicle.GetType().GetProperties())
            {
                var keyValue = new KeyValueItemValid();
                var listAttributes = itemPrperty.GetCustomAttributesData();

                if (listAttributes.Count > 0)
                {
                    var temValue = itemPrperty.GetValue(vehicle, null) == null
                        ? ""
                        : itemPrperty.GetValue(vehicle, null).ToString();
                    keyValue.Key = itemPrperty.Name;
                    keyValue.Value = temValue;
                    if (string.IsNullOrEmpty(temValue))
                    {
                        keyValue.Result = false;
                        keyValue.Description = "该项不能为空";
                    }
                    else
                    {
                        keyValue.Result = true;
                        keyValue.Description = "";
                    }

                    if (!tempList.ContainsKey(itemPrperty.Name))
                    {

                        tempList.Add(itemPrperty.Name, keyValue);
                    }
                }
                else
                {
                    keyValue.Key = itemPrperty.Name;
                    keyValue.Result = true;
                    keyValue.Description = "";
                    keyValue.Value = itemPrperty.GetValue(vehicle, null) == null
                        ? ""
                        : itemPrperty.GetValue(vehicle, null).ToString();
                    tempList.Add(itemPrperty.Name, keyValue);
                    continue;
                }

                if (keyValue.Key == "CheJiaHao" || keyValue.Key == "M1" || keyValue.Key == "IA1" ||
                    keyValue.Key == "IC1")
                {
                    keyValue.Result = true;
                    keyValue.Description = "";
                }

                string[] vehicleColor = { "蓝色", "黄色", "白色", "黑色", "黄绿双拼色", "渐变绿色" };
                var exists = ((IList)vehicleColor).Contains(vehicle.RegistrationNoColor);
                if (keyValue.Key == "RegistrationNoColor" && !exists)
                {
                    keyValue.Result = false;
                    keyValue.Description = "车牌颜色不再范围内";
                }

                if (keyValue.Key == "XiaQuShi" && keyValue.Value != "清远")
                {
                    keyValue.Result = false;
                    keyValue.Description = "辖区市无效";
                }
            }

            if (!tempList.ContainsKey("IsGpsTimeInRecentMonth"))
            {
                tempList.Add("IsGpsTimeInRecentMonth",
                    new KeyValueItemValid() { Key = "IsGpsTimeInRecentMonth", Description = "" });
            }

            //初始指标
            InitialStander(tempList);

            #endregion
        }

        private void InitialStander(Dictionary<string, KeyValueItemValid> tempList)
        {
            if (tempList == null) return;
            foreach (var item in tempList)
            {
                switch (item.Key)
                {
                    case "RegistrationNo":
                        item.Value.FiledStander = "车牌号";
                        break;
                    case "RegistrationNoColor":
                        item.Value.FiledStander = "车牌颜色";
                        break;
                    case "CheLiangZhongLei":
                        item.Value.FiledStander = "车辆种类";
                        break;
                    case "MDT":
                        item.Value.FiledStander = "终端MDT";
                        break;
                    case "CheJiaHao":
                        item.Value.FiledStander = "车架号";
                        break;
                    case "M1":
                        item.Value.FiledStander = "M1";
                        break;
                    case "IA1":
                        item.Value.FiledStander = "IA1";
                        break;
                    case "IC1":
                        item.Value.FiledStander = "IC1";
                        break;
                    case "DeviceCode":
                        item.Value.FiledStander = "终端型号";
                        break;
                    case "TerminalTypeName":
                        item.Value.FiledStander = "设备型号";
                        break;
                    case "SimNo":
                        item.Value.FiledStander = "SIM卡号";
                        break;
                    case "TerminalType":
                        break;
                    case "YeHuMingCheng":
                        item.Value.FiledStander = "业户名称";
                        break;
                    case "XiaQuShi":
                        item.Value.FiledStander = "辖区市";
                        break;
                    case "XiaQuXian":
                        item.Value.FiledStander = "辖区县";
                        break;
                    case "GpsTime":
                        item.Value.FiledStander = "定位时间";
                        break;
                    case "Longtitude":
                        item.Value.FiledStander = "经度";
                        break;
                    case "Latitude":
                        item.Value.FiledStander = "纬度";
                        break;
                    case "Speed":
                        item.Value.FiledStander = "行驶速度";
                        break;
                    case "UploadFrequency":
                        item.Value.FiledStander = "上传频率";
                        break;
                    case "IsGpsTimeInRecentMonth":
                        item.Value.FiledStander = "定位时间是否在一个月以内";
                        break;
                }
            }
        }

        /// <summary>
        /// 验证车辆动态数据
        /// </summary>
        private void ValidVehicleDynamicInfo(VehicleGpsInfoDto vehicleinfo,
            Dictionary<string, KeyValueItemValid> tempList)
        {
            var vehicleDynamicInfoList = RequestVehicleDynamicInfo(vehicleinfo);
            if (vehicleDynamicInfoList != null)
            {
                var item = vehicleDynamicInfoList;
                //已安装
                if (!string.IsNullOrEmpty(item.Time))
                {
                    var now = DateTime.Now;
                    if (now.Subtract(DateTime.Parse(item.Time)).TotalMinutes > 120)
                    {
                        var key = "GpsTime";
                        if (tempList.ContainsKey(key))
                        {
                            tempList[key].Value = item.Time;
                            tempList[key].Result = false;
                            tempList[key].Description = "定位时间不在正负2小时内";
                        }

                    }
                    else
                    {
                        var key = "GpsTime";
                        if (tempList.ContainsKey(key))
                        {
                            tempList[key].Value = item.Time;
                            tempList[key].Result = true;
                            tempList[key].Description = string.Empty;
                        }
                    }
                }
                else
                {
                    var key = "GpsTime";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Time;
                        tempList[key].Result = false;
                        tempList[key].Description = "Gps时间无效";
                    }
                }

                if (item.Latitude <= 0 || item.Latitude > 90)
                {
                    var key = "Latitude";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Latitude.ToString(CultureInfo.InvariantCulture);
                        tempList[key].Result = false;
                        tempList[key].Description = "纬度无效";
                    }
                }
                else
                {
                    var key = "Latitude";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Latitude.ToString(CultureInfo.InvariantCulture);
                        tempList[key].Result = true;
                        tempList[key].Description = string.Empty;
                    }
                }

                if (item.Longitude <= 0 || item.Longitude > 180)
                {
                    var key = "Longtitude";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Longitude.ToString(CultureInfo.InvariantCulture);
                        tempList[key].Result = false;
                        tempList[key].Description = "经度无效";
                    }
                }
                else
                {
                    var key = "Longtitude";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Longitude.ToString(CultureInfo.InvariantCulture);
                        tempList[key].Result = true;
                        tempList[key].Description = string.Empty;
                    }
                }

                if (item.Speed < 0 || item.Speed > 160)
                {
                    var key = "Speed";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Speed.ToString();
                        tempList[key].Result = false;
                        tempList[key].Description = "速度无效";
                    }
                }
                else
                {
                    var key = "Speed";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.Speed.ToString();
                        tempList[key].Result = true;
                        tempList[key].Description = string.Empty;
                    }
                }

                if (!string.IsNullOrWhiteSpace(item.UploadFrequency) && item.UploadFrequency.Equals(">10秒"))
                {
                    var key = "UploadFrequency";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.UploadFrequency;
                        tempList[key].Result = true;
                        tempList[key].Description = string.Empty;
                    }
                }
                else
                {
                    var key = "UploadFrequency";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.UploadFrequency;
                        tempList[key].Result = false;
                        tempList[key].Description = "不能低于10秒/条";
                    }
                }


                //一个月内是否有定位上传
                var compareDate = CompareDate(item.Time, 1);
                if (!compareDate)
                {
                    var key = "IsGpsTimeInRecentMonth";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = "是";
                        tempList[key].Result = true;
                        tempList[key].Description = string.Empty;
                    }
                }
                else
                {
                    var key = "IsGpsTimeInRecentMonth";
                    if (tempList.ContainsKey(key))
                    {
                        tempList[key].Value = item.IsGpsTimeInRecentMonth;
                        tempList[key].Result = false;
                        tempList[key].Description = "最近一个月内未上传数据";
                    }
                }
            }
            else
            {
                foreach (var subitem in tempList)
                {
                    if (!subitem.Key.Equals("GpsTime") && !subitem.Key.Equals("Latitude") &&
                        !subitem.Key.Equals("Longtitude") && !subitem.Key.Equals("Speed") &&
                        !subitem.Key.Equals("UploadFrequency") &&
                        !subitem.Key.Equals("IsGpsTimeInRecentMonth")) continue;
                    tempList[subitem.Key].Value = string.Empty;
                    tempList[subitem.Key].Result = false;
                    tempList[subitem.Key].Description = "未上传定位信息";
                }
            }
        }

        /// <summary>
        /// 是否超过一个月
        /// </summary>
        /// <param name="JobDateFrom"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool CompareDate(string JobDateFrom, int n)
        {
            if (string.IsNullOrWhiteSpace(JobDateFrom))
            {
                return true;
            }

            var dTJobDateFrom = Convert.ToDateTime(JobDateFrom);
            var dTJobDateTo = DateTime.Now;
            dTJobDateFrom = dTJobDateFrom.AddMonths(n);
            return dTJobDateFrom < dTJobDateTo;
        }

        /// <summary>
        /// 获取车辆动态数据
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private VehicleDynamicInfoDto RequestVehicleDynamicInfo(VehicleGpsInfoDto vehicle)
        {
            var vehicleDynamicInfo = new VehicleDynamicInfoDto();
            try
            {
                var queryParam = new
                {
                    VehicleKeys = $"['{vehicle.RegistrationNo + vehicle.RegistrationNoColor}']"
                };
                var queryRequest = GetInvokeRequest("006600100001", "1.0", queryParam);
                if (queryRequest != null)
                {
                    if (queryRequest.publicresponse.statuscode == 0)
                    {
                        List<GpsInfoDto> gpsInfoModel =
                            JsonConvert.DeserializeObject<List<GpsInfoDto>>(
                                JsonConvert.SerializeObject(queryRequest.body.items));

                        if (!gpsInfoModel.Any()) return vehicleDynamicInfo;

                        var gpsInfo = gpsInfoModel.FirstOrDefault();
                        if (gpsInfo != null)
                        {
                            vehicleDynamicInfo.Time = gpsInfo.GpsTime.ToString();
                            if (gpsInfo.Longitude != null)
                                vehicleDynamicInfo.Longitude = (float)gpsInfo.Longitude;

                            if (gpsInfo.Latitude != null) vehicleDynamicInfo.Latitude = (float)gpsInfo.Latitude;

                            vehicleDynamicInfo.Speed = gpsInfo.Speed;
                            vehicleDynamicInfo.UploadFrequency = gpsInfo.Frequency;
                            return vehicleDynamicInfo;
                        }
                    }

                    LogHelper.Error($"车辆档案查询车辆最近定位信息006600100001返回响应错误{JsonConvert.SerializeObject(queryRequest)}");
                    return null;
                }

                LogHelper.Error("车辆档案查询车辆最近定位信息006600100001返回结果为空");
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取车辆定位信息出错{ex.Message}", ex);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 查看gps审核结果
        /// </summary>
        /// <param name="cheLiangId"></param>
        /// <returns></returns>
        public ServiceResult<GpsAuditInformation> GetAuditResult(Guid cheLiangId)
        {
            try
            {
                var id = cheLiangId.ToString();
                var vehicleGps = _iGPSAuditRecordRepository.GetQuery(x =>
                    x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常
                    && x.VehicleId == id && x.GPSAuditStatus == (int)GPSAuditStatus.通过备案).FirstOrDefault();
                var keyValueItemValidDic = new Dictionary<string, KeyValueItemValid>();
                if (vehicleGps != null)
                {
                    if (string.IsNullOrEmpty(vehicleGps.ResultComment))
                        return new ServiceResult<GpsAuditInformation>
                        { Data = new GpsAuditInformation { IsSuccess = 2, ErrorMsg = "查询错误" } };

                    keyValueItemValidDic =
                        JsonConvert.DeserializeObject<Dictionary<string, KeyValueItemValid>>(vehicleGps
                            .ResultComment);
                    return new ServiceResult<GpsAuditInformation>
                    { Data = new GpsAuditInformation { IsSuccess = 0, TempList = keyValueItemValidDic } };
                }

                #region 车辆GPS审核结果

                var vehicle =
                    _cheLiangXinXiRepository.GetByKey(cheLiangId);
                if (vehicle == null)
                {
                    return new ServiceResult<GpsAuditInformation>
                    { Data = new GpsAuditInformation { IsSuccess = 2, ErrorMsg = "车辆信息为空" } };
                }

                var enterpriseName = string.Empty;
                var enterprise = _cheLiangYeHuRepository
                    .GetQuery(s => s.SYS_XiTongZhuangTai == 0 && s.OrgCode == vehicle.YeHuOrgCode).FirstOrDefault();
                if (enterprise != null)
                {
                    enterpriseName = enterprise.OrgName;
                }

                var vehicleId = cheLiangId.ToString();
                //gps终端信息
                var gpsConfigure =
                    _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x =>
                        x.SYS_XiTongZhuangTai == (int)XiTongZhuangTaiEnum.正常
                        && x.CheLiangId == vehicleId).FirstOrDefault();
                var vehicleGpsInfo = new VehicleGpsInfoDto
                {
                    RegistrationNo = vehicle.ChePaiHao,
                    RegistrationNoColor = vehicle.ChePaiYanSe,
                    XiaQuShi = vehicle.XiaQuShi,
                    XiaQuXian = vehicle.XiaQuXian,
                    CheJiaHao = vehicle.CheJiaHao,
                    YeHuMingCheng = enterpriseName,
                    MDT = gpsConfigure?.ZhongDuanMDT,
                    M1 = gpsConfigure?.M1,
                    IA1 = gpsConfigure?.IA1,
                    IC1 = gpsConfigure?.IC1,
                    DeviceMode = gpsConfigure?.SheBeiXingHao,
                    SimNo = gpsConfigure?.SIMKaHao,
                    TerminalType = gpsConfigure?.ZhongDuanLeiXing
                };
                //车辆种类
                if (vehicle.CheLiangZhongLei.HasValue)
                {
                    vehicleGpsInfo.CheLiangZhongLei =
                        typeof(CheLiangZhongLei).GetEnumName(vehicle.CheLiangZhongLei);
                }

                //设备型号
                if (gpsConfigure?.ZhongDuanLeiXing != null)
                {
                    vehicleGpsInfo.TerminalTypeName =
                        typeof(ZhongDuanLeiXing).GetEnumName(gpsConfigure.ZhongDuanLeiXing);
                }

                ValidVehicleInfoBaseInfo(vehicleGpsInfo, keyValueItemValidDic);
                ValidVehicleDynamicInfo(vehicleGpsInfo, keyValueItemValidDic);
                var calResult = CalculateResult(keyValueItemValidDic);
                var gpsAuditRecord = new GPSAuditRecord
                {
                    Id = Guid.NewGuid(),
                    FiledComment = calResult.Item2,
                    FiledDate = DateTime.Now,
                    ResultComment = JsonConvert.SerializeObject(keyValueItemValidDic),
                    GPSAuditStatus = calResult.Item1 ? (int)GPSAuditStatus.通过备案 : (int)GPSAuditStatus.未通过备案,
                    VehicleId = vehicleId
                };
                var implement = GpsAuditRecord(gpsAuditRecord, vehicle);

                #endregion

                if (implement)
                {
                    if (string.IsNullOrEmpty(gpsAuditRecord.ResultComment))
                        return new ServiceResult<GpsAuditInformation>
                        { Data = new GpsAuditInformation { IsSuccess = 2, ErrorMsg = "车辆信息为空" } };

                    keyValueItemValidDic =
                        JsonConvert.DeserializeObject<Dictionary<string, KeyValueItemValid>>(gpsAuditRecord
                            .ResultComment);
                    return new ServiceResult<GpsAuditInformation>
                    { Data = new GpsAuditInformation { IsSuccess = 0, TempList = keyValueItemValidDic } };
                }

                LogHelper.Error("查看GPS审核结果出错 " + cheLiangId);
                return new ServiceResult<GpsAuditInformation>
                { Data = new GpsAuditInformation { IsSuccess = 2, ErrorMsg = "查看失败" } };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查看GPS审核结果出错" + ex.Message, ex);
                return new ServiceResult<GpsAuditInformation>
                { Data = new GpsAuditInformation { IsSuccess = 2, ErrorMsg = "查询错误" } };
            }
        }

        #region 车辆业务资质查询
        /// <summary>
        /// 车辆业务资质查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryVehicleQualification(QueryData queryData)
        {

            var result = new ServiceResult<QueryResult>();
            var queryResult = new QueryResult();
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    userInfo.OrganizationType = (int) OrganizationType.市政府;
                }
                CheLiangXinXiInput dto =
                    JsonConvert.DeserializeObject<CheLiangXinXiInput>(JsonConvert.SerializeObject(queryData.data));
                if (dto.BeiAnTongGuoBeginTime > dto.BeiAnTongGuoEndTime)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "备案通过开始时间不能大于备案通过结束时间" };
                }

                var list = GetCheLiangXinXiList
                    (userInfo, queryData);
                queryResult.totalcount = list.Distinct().Count();
                if (queryResult.totalcount > 0)
                {
                    var basicDataList = list.Distinct().OrderByDescending(u => u.ChuangJianShiJian)
                        .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                    queryResult.items = VehicleQualificationList(basicDataList.ToList());
                }
                result.Data = queryResult;
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "车辆业务资质查询异常";
                LogHelper.Error($"调用QueryVehicleQualification{nameof(CheLiangService)}.{nameof(Query)}出错,异常信息:{ex.Message}", ex);
                return result;
            }

            return result;
        }
        /// <summary>
        /// 车辆业务资质结果
        /// </summary>
        /// <param name="basicDataList"></param>
        /// <returns></returns>
        public List<CheLiangSearchResponseDto> VehicleQualificationList(List<CheLiangSearchResponseDto> basicDataList)
        {
            try
            {
                if (!basicDataList.Any())
                {
                    return basicDataList;
                }
                //不是两客一危的车 只需要GPS审核通过就能办理业务资质  
                foreach (var entity in basicDataList)
                {
                    if (entity.BusinessHandlingResults == (int)VehicleQualificationStatus.给予办理)
                    {
                        entity.State = (int)VehicleQualificationStatus.给予办理;
                        continue;
                    }
                    #region 业务处理
                    //GPS审核结果
                    var gpsAuditStatus = entity.GPSAuditStatus == 1;
                    entity.State = 1;
                    entity.Explain = string.Empty;
                    if (!gpsAuditStatus)
                    {
                        entity.State = 0;
                        entity.Explain += "GPS审核结果未通过";
                    }
                    if (entity.CheLiangZhongLei==(int)CheLiangZhongLei.客运班车|| entity.CheLiangZhongLei== (int)CheLiangZhongLei.客运班车
                                                                           || entity.CheLiangZhongLei == (int)CheLiangZhongLei.危险货运
                                                                           || entity.CheLiangZhongLei == (int)CheLiangZhongLei.重型货车)
                    {
                        //人工审核状态
                        var manualApproval = VehicleFindings(entity);
                        //备案状态
                        var beiAnZhuangTai = entity.BeiAnZhuangTai == 1;
                        //抽查结果
                        var vehicleFindingsSpotCheck = VehicleFindingsSpotCheck(entity.ChePaiHao, entity.ChePaiYanSe);
                        if (!manualApproval.Status)
                        {
                            entity.State = 0;
                            entity.Explain = manualApproval.Explain + ",";
                        }
                        if (!beiAnZhuangTai)
                        {
                            entity.State = 0;
                            entity.Explain += "备案状态未通过,";
                        }
                        if (!vehicleFindingsSpotCheck.Status)
                        {
                            entity.State = 0;
                            entity.Explain += "车辆抽查未填报";
                        }
                        entity.Explain = entity.Explain.TrimEnd(',');
                    }
                    if (entity.State == (int)VehicleQualificationStatus.给予办理)
                    {
                        UpdateVehicleHandle(entity.Id);
                    }
                    #endregion
                }
                return basicDataList;
            }

            catch (Exception ex)
            {
                LogHelper.Error($"车辆业务资质查询出错,异常信息:{ex.Message}", ex);
            }

            return basicDataList;
        }

        /// <summary>
        /// 人工审核结果
        /// </summary>
        /// <param name="vehicleInformation"></param>
        /// <returns></returns>
        public VehicleFindingsModel VehicleFindings(CheLiangSearchResponseDto vehicleInformation)
        {

            var vehicleFindings = new VehicleFindingsModel
            {
                Status = true,
                Explain = string.Empty
            };
            try
            {
                var resultModel = new VehicleAnnualReviewResultDto()
                {
                    ChePaiHao = vehicleInformation.ChePaiHao,
                    ChePaiYanSe = vehicleInformation.ChePaiYanSe,
                    IsHavVideoAlarmAttachment = vehicleInformation.IsHavVideoAlarmAttachment == 1,
                    IsHavVehicleTrack = false
                };
                //读取定位信息
                GetVehicelGpsInfo(resultModel);
                if (!resultModel.IsHavVideoAlarmAttachment)
                {
                    vehicleFindings.Status = false;
                    vehicleFindings.Explain += "智能视频报警附件不存在,";
                }
                if (!resultModel.IsHavVehicleTrack)
                {
                    vehicleFindings.Status = false;
                    vehicleFindings.Explain += "车辆轨迹信息不存在,";
                }
                if (resultModel.VehicelLastPositioningTime == null)
                {
                    vehicleFindings.Status = false;
                    vehicleFindings.Explain += "车辆最新定位时间不存在,";
                }
                vehicleFindings.Explain = vehicleFindings.Explain.TrimEnd(',');
            }
            catch (Exception e)
            {
                LogHelper.Error($"人工审核结果查询出错,异常信息:{e.Message}", e);
                throw;
            }

            return vehicleFindings;
        }

        /// <summary>
        /// 车辆抽查情况
        /// </summary>
        /// <param name="chePaiHao"></param>
        /// <param name="chePaiYanSe"></param>
        /// <returns></returns>
        public VehicleFindingsModel VehicleFindingsSpotCheck(string chePaiHao, string chePaiYanSe)
        {
            var vehicleFindings = new VehicleFindingsModel
            {
                Status = true
            };
            try
            {
                using (IDbConnection conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DC_DLYSZHGLPT"].ConnectionString))
                {
                    var sql =
                        $@"SELECT 
           count(*)
    FROM dbo.T_RiskSpotCheck
    WHERE SYS_XiTongZhuangTai = 0
          AND EventType = '监管抽查'
          AND FillingStatus = 0
          AND StartTime >= '{DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd 00:00:00")}'
          AND StartTime < '{DateTime.Now.ToString("yyyy-MM-dd 00:00:00")}'
		  AND RegistrationNo='{chePaiHao}'
		  AND RegistrationNoColor='{chePaiYanSe}'";
                    var count = conn.ExecuteScalar<int>(sql);
                    if (count > 0)
                    {
                        vehicleFindings.Status = false;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error($"车辆抽查情况查询出错,异常信息:{e.Message}", e);
                vehicleFindings.Status = false;
                return vehicleFindings;
            }
            return vehicleFindings;
        }
        #endregion

        /// <summary>
        /// 车辆业务资质导出
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<ExportCheliangXinXiDto> ExportVehicleQualification(QueryData queryData)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                var list = GetCheLiangXinXiList(userInfo, queryData).ToList();
                var vehicleQualificationList = VehicleQualificationList(list.ToList());
                var tableTitle = "车辆业务资质导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list.Any())
                {
                    try
                    {
                        var FileId = string.Empty;
                        var fielId = VehicleQualificationExcelAndUpload(vehicleQualificationList, tableTitle);

                        if (fielId != null)
                        {
                            FileId = fielId.ToString();
                        }
                        else
                        {
                            LogHelper.Error("生成车辆业务资质上传文件出错" + JsonConvert.SerializeObject(queryData));
                            return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出失败", StatusCode = 2 };
                        }

                        return new ServiceResult<ExportCheliangXinXiDto>
                        { Data = new ExportCheliangXinXiDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆业务资质出错" + e.Message, e);
                        return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }

                return new ServiceResult<ExportCheliangXinXiDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆业务资质出错" + ex.Message, ex);
                return new ServiceResult<ExportCheliangXinXiDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        private static Guid? VehicleQualificationExcelAndUpload(List<CheLiangSearchResponseDto> list, string fileName)
        {
            try
            {
                if (list == null || list.Count == 0)
                {
                    return null;
                }
                //string title = "车辆档案";
                string[] cellTitleArry =
                {
                    "车牌号码", "车牌颜色", "车辆种类", "所属区域", "企业名称", "营运状态", "版本协议", "业务办理建议", "数据表现情况", "提交备案时间", "备案通过时间"
                };
                HSSFWorkbook workbook = new HSSFWorkbook(); //HSSFWorkbook
                int sheetRowCount = 65535; //每个sheet最大数据行数

                //循环创建sheet
                //因单个sheet最多存储65535条记录，故分sheet存储数据，-2的原因是标题和列头占据了两行
                int max_sheet_count = (list.Count + (sheetRowCount - 1) - 1) / (sheetRowCount - 1);
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
                    //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                    //string titleNum = max_sheet_count == 1 ? "" : ($"({(sheet_index + 1)})");
                    //row.CreateCell(0).SetCellValue($"{title}{titleNum}");
                    ////附加标题样式
                    //row.Cells[0].CellStyle = titleStyle;

                    row = (HSSFRow)sheet.CreateRow(0);

                    for (int cell_index = 0; cell_index < cellTitleArry.Length; cell_index++)
                    {
                        row.CreateCell(cell_index).SetCellValue(cellTitleArry[cell_index]);
                        //附加表头样式
                        row.Cells[cell_index].CellStyle = cellStyle;
                    }

                    //内容
                    var loop_list = list.Skip(sheet_index * (sheetRowCount - 1)).Take(sheetRowCount - 1).ToList();
                    for (int content_index = 0; content_index < loop_list.Count; content_index++)
                    {
                        var item = loop_list[content_index];
                        row = (HSSFRow)sheet.CreateRow(content_index + 1);
                        int index = 0;
                        //车牌号
                        row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                        //车牌颜色
                        row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                        //车辆种类
                        string chelaingzhongleiStr = "";
                        if (item.CheLiangZhongLei.HasValue)
                        {
                            chelaingzhongleiStr = typeof(CheLiangZhongLei).GetEnumName(item.CheLiangZhongLei);
                            ;
                        }

                        row.CreateCell(index++).SetCellValue(chelaingzhongleiStr);

                        row.CreateCell(index++).SetCellValue(item.XiaQuSheng + " " + item.XiaQuShi + " " + item.XiaQuXian);

                        //企业名称
                        row.CreateCell(index++).SetCellValue(item.QiYeMingCheng);
                        //营运状态
                        row.CreateCell(index++).SetCellValue(item.YunZhengZhuangTai);
                        //协议版本
                        string shuJuTongXunBanBenHao = "";
                        if (item.ShuJuTongXunBanBenHao.HasValue)
                        {
                            try
                            {
                                shuJuTongXunBanBenHao = ((ZhongDuanShuJuTongXunBanBenHao)item.ShuJuTongXunBanBenHao)
                                    .GetDescription();
                            }
                            catch (Exception)
                            {
                                shuJuTongXunBanBenHao = item.ShuJuTongXunBanBenHao.ToString();
                            }
                        }

                        row.CreateCell(index++).SetCellValue(shuJuTongXunBanBenHao);
                        //业务办理状态
                        string beiAnStatus = "";
                        if (item.State.HasValue)
                        {
                            beiAnStatus = typeof(VehicleQualificationStatus).GetEnumName(item.State);
                        }
                        row.CreateCell(index++).SetCellValue(beiAnStatus);
                        row.CreateCell(index++).SetCellValue(item.Explain);

                        //提交备案时间
                        string tiJiaoBeiAnShiJianStr = "";
                        if (item.TiJiaoBeiAnShiJian.HasValue)
                        {
                            tiJiaoBeiAnShiJianStr = item.TiJiaoBeiAnShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        row.CreateCell(index++).SetCellValue(tiJiaoBeiAnShiJianStr);
                        //备案通过时间
                        string beiAnTongGuoShiJian = "";
                        if (item.BeiAnShenHeShiJian.HasValue &&
                            item.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                        {
                            beiAnTongGuoShiJian = item.BeiAnShenHeShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        row.CreateCell(index++).SetCellValue(beiAnTongGuoShiJian);
                        for (int contInx = 0; contInx < index; contInx++)
                        {
                            row.Cells[contInx].CellStyle = contentStyle;
                        }
                    }

                    //表格样式
                    DefaultStyle(sheet, cellTitleArry);
                }
                //上传
                var extension = "zip";
                fileName += ".xlsx";
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
                    fileDto.Data = Compress(ms.ToArray());
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
            catch (Exception ex)
            {
                LogHelper.Error("生成导出车辆业务资质上传出错" + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 车辆业务资质给予办理
        /// </summary>
        /// <param name="vehicleId"></param>
        public void UpdateVehicleHandle(Guid? vehicleId)
        {
            using (var uow = new UnitOfWork())
            {
                try
                {
                    uow.BeginTransaction();
                    var vehicle = _cheLiangXinXiRepository.GetByKey(vehicleId);
                    vehicle.BusinessHandlingResults = (int)VehicleQualificationStatus.给予办理;
                    _cheLiangXinXiRepository.Update(vehicle);
                    uow.CommitTransaction();
                    LogHelper.Info($"车辆业务资质审核通过" + vehicleId);
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"车辆业务资质修改,异常信息:{ex.Message}" + "，车辆" + vehicleId.ToString(), ex);
                    uow.RollBackTransaction();
                }
            }
        }
        /// <summary>
        /// 车辆业务资质查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> GetOpenVehicleQualification(QueryData queryData)
        {
           
            var result = new ServiceResult<QueryResult>();
            var queryResult = new QueryResult();
            try
            {
            
                var list = GetVehicleInformationList(queryData);
                queryResult.totalcount = list.Distinct().Count();
                if (queryResult.totalcount > 0)
                {
                    var basicDataList = list.Distinct().OrderByDescending(u => u.ChuangJianShiJian)
                        .Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                    queryResult.items = VehicleQualificationList(basicDataList.ToList());
                }
                result.Data = queryResult;
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.ErrorMessage = "车辆业务资质查询异常";
                LogHelper.Error($"调用QueryVehicleQualification{nameof(CheLiangService)}.{nameof(Query)}出错,异常信息:{ex.Message}", ex);
                return result;
            }
            return result;
        }
        private IEnumerable<CheLiangSearchResponseDto> GetVehicleInformationList(QueryData queryData)
        {
            CheLiangXinXiInput dto =
                JsonConvert.DeserializeObject<CheLiangXinXiInput>(JsonConvert.SerializeObject(queryData.data));
            Expression<Func<CheLiang, bool>> cheliangexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<OrgBaseInfo, bool>> orgBaseexp = t => t.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangVideoZhongDuanConfirm, bool>> zdConfirmexp = t => t.SYS_XiTongZhuangTai == 0;
            //车牌号
            if (!string.IsNullOrWhiteSpace(dto.ChePaiHao)) //T_CheLiang
            {
                dto.ChePaiHao = Regex.Replace(dto.ChePaiHao, @"\s", "");
                cheliangexp = cheliangexp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.ToUpper()));
            }

            var list = from car in _cheLiangXinXiRepository.GetQuery(cheliangexp)
                       join org in _orgBaseInfoRepository.GetQuery(orgBaseexp)
                           on car.YeHuOrgCode equals org.OrgCode
                       join b in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0) on
                           car.Id.ToString() equals b.CheLiangId into temp1
                       from te1 in temp1.DefaultIfEmpty()
                       join vzdc in _cheLiangVideoZhongDuanConfirmRepository.GetQuery(zdConfirmexp) on car.Id.ToString() equals
                           vzdc.CheLiangId into temp
                       from vzdc in temp.DefaultIfEmpty()
                       join fws in _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on car.FuWuShangOrgCode
                           equals fws.OrgCode into table1
                       from fwslist in table1.DefaultIfEmpty()
                       join gps in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on
                           car.Id.ToString() equals gps.CheLiangId into table2
                       from t2 in table2.DefaultIfEmpty()
                       join tongxunpeizhi_temp in
                           _cheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0) on
                           car.Id equals tongxunpeizhi_temp.CheLiangID into tongxunpeizhi_temp2
                       from tongxunpeizhi in tongxunpeizhi_temp2.DefaultIfEmpty()

                       where string.IsNullOrEmpty(dto.SouYouRen) || org.OrgName.Contains(dto.SouYouRen)
                       where string.IsNullOrEmpty(dto.CheJiaHao) || car.CheJiaHao.Contains(dto.CheJiaHao)
                       where !dto.BeiAnZhuangTai.HasValue ||
                             (dto.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.待提交 &&
                              (vzdc.BeiAnZhuangTai == dto.BeiAnZhuangTai || vzdc.BeiAnZhuangTai == null)) ||
                             vzdc.BeiAnZhuangTai == dto.BeiAnZhuangTai
                       where string.IsNullOrEmpty(dto.FuWuShangName) || fwslist.OrgName.Contains(dto.FuWuShangName)
                       where !dto.ShenHeShiJianBeginTime.HasValue || vzdc.SYS_ZuiJinXiuGaiShiJian >= dto.ShenHeShiJianBeginTime
                       where !dto.ShenHeShiJianEndTime.HasValue || vzdc.SYS_ZuiJinXiuGaiShiJian <= dto.ShenHeShiJianEndTime
                       where !dto.BeiAnTongGuoBeginTime.HasValue ||
                             (vzdc.SYS_ZuiJinXiuGaiShiJian >= dto.BeiAnTongGuoBeginTime &&
                              vzdc.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                       where !dto.BeiAnTongGuoEndTime.HasValue ||
                             (vzdc.SYS_ZuiJinXiuGaiShiJian <= dto.BeiAnTongGuoEndTime &&
                              vzdc.BeiAnZhuangTai == (int)ZhongDuanBeiAnZhuangTai.通过备案)
                       where !dto.ShuJuTongXunBanBenHao.HasValue ||
                             (tongxunpeizhi.BanBenHao == (int?)dto.ShuJuTongXunBanBenHao)

                       select new CheLiangSearchResponseDto
                       {
                           Id = car.Id,
                           ChePaiHao = car.ChePaiHao,
                           ChePaiYanSe = car.ChePaiYanSe,
                           CheLiangZhongLei = car.CheLiangZhongLei,
                           QiYeMingCheng = org.OrgName,
                           ChuangJianShiJian = car.SYS_ChuangJianShiJian,
                           CheLiangLeiXing = car.CheLiangLeiXing,
                           XiaQuSheng = car.XiaQuSheng,
                           XiaQuShi = car.XiaQuShi,
                           XiaQuXian = car.XiaQuXian,
                           YunZhengZhuangTai = car.YunZhengZhuangTai,
                           NianShenZhuangTai = car.NianShenZhuangTai,
                           CheJiaHao = car.CheJiaHao,
                           BeiAnZhuangTai = vzdc.BeiAnZhuangTai,
                           FuWuShangOrgCode = car.FuWuShangOrgCode,
                           TiJiaoBeiAnShiJian = vzdc.TiJiaoBeiAnShiJian,
                           FuWuShangName = fwslist.OrgName,
                           BeiAnShenHeShiJian = vzdc.SYS_ZuiJinXiuGaiShiJian,
                           SIMKaHao = t2.SIMKaHao,
                           GpsZhongDuanMDT = t2.ZhongDuanMDT,
                           VideoZhongDuanMDT = te1.ZhongDuanMDT,
                           VideoSheBeiXingHao = te1.SheBeiXingHao,
                           VideoShengChanChangJia = te1.ShengChanChangJia,
                           ShuJuTongXunBanBenHao = tongxunpeizhi.BanBenHao,
                           ManualApprovalStatus = car.ManualApprovalStatus,
                           ThirdPartyState = car.FuWuShangOrgCode,
                           EnterpriseCode = car.YeHuOrgCode,
                           GPSAuditStatus = car.GPSAuditStatus,
                           BusinessHandlingResults = car.BusinessHandlingResults,
                           IsHavVideoAlarmAttachment = car.IsHavVideoAlarmAttachment
                       };
            return list;
        }
        /// <summary>
        /// 批量提交备案
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ServiceResult<bool> BatchFiling(List<Guid> ids)
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return new ServiceResult<bool> {Data = false, ErrorMessage = "获取登录信息失败,请重新登录", StatusCode = 2};
            }

            try
            {
                foreach (var id in ids)
                {
                    var dataResults = UpdateVehicleRecordState(id);
                    if (!dataResults.Data)
                    {
                        return dataResults;
                    }
                }
                return new ServiceResult<bool> {Data = true};
            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆档案提交备案出错" + ex.Message + "CheLiangId" + JsonConvert.SerializeObject(ids), ex);
                return new ServiceResult<bool> {Data = false, StatusCode = 2};
            }
        }

        /// <summary>
        /// 抽查车辆替换业户更新业户
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="enterpriseName"></param>
        public void UpdateSpoCheckYeHu(CheLiang vehicle, string enterpriseName)
        {
            using (IDbConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DC_DLYSZHGLPT"].ConnectionString))
            {
                var sql = $@"
UPDATE DC_DLYSZHGLPT.dbo.T_RiskSpotCheck
SET YeHuDaiMa='{vehicle.YeHuOrgCode}',
YeHuMingCheng='{enterpriseName}'
WHERE SYS_XiTongZhuangTai=0
AND FillingStatus=0
AND RegistrationNo='{vehicle.ChePaiHao}'
AND RegistrationNoColor='{vehicle.ChePaiYanSe}'
AND YeHuDaiMa <> '{vehicle.YeHuOrgCode}'
";
                conn.ExecuteScalar<int>(sql, null);
            }
        }

        /// <summary>
        /// 车辆凌晨营运天数统计
        /// </summary>
        /// <returns></returns>
        public ServiceResult<List<VehicleOperatingDays>> VehicleOperatingDays(QueryData queryData)
        {
            try
            {

                VehicleOperatingDays dto =
                    JsonConvert.DeserializeObject<VehicleOperatingDays>(JsonConvert.SerializeObject(queryData.data));
                using (IDbConnection conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DC_DLYSZHGLPT"].ConnectionString))
                {
                    var sql = $@"
SELECT COUNT(TotalOperationDays) TotalOperationDays,
       RegistrationNo,
       RegistrationNoColor
FROM
(
    SELECT RegistrationNo,
           RegistrationNoColor,
           (CASE
                WHEN COUNT(*) >= 1 THEN
                    1
                ELSE
                    0
            END
           ) TotalOperationDays
    FROM GpsReportDB.[dbo].[VehicleEarlyMorningOperationDetail]  WITH (NOLOCK)
    WHERE GenerationTime >='{dto.StartTime}'
	AND GenerationTime < '{dto.EndTime}'
    GROUP BY CONVERT(VARCHAR(100), GenerationTime, 23),
             RegistrationNo,
             CheLiangZhongLei,
             RegistrationNoColor
)vem
GROUP BY RegistrationNo,
         RegistrationNoColor
";
                    var vehicleSpotCheck = conn.Query<VehicleOperatingDays>(sql).ToList();
                    return new ServiceResult<List<VehicleOperatingDays>> { Data = vehicleSpotCheck };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("车辆凌晨营运天数统计" + ex.Message, ex);
                return new ServiceResult<List<VehicleOperatingDays>> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }
    }
}
