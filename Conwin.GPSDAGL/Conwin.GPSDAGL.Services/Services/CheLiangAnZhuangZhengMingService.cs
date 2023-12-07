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
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing;
using Conwin.GPSDAGL.Services.Enums;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Conwin.GPSDAGL.Services.Services
{
    public class CheLiangAnZhuangZhengMingService : ApiServiceBase, ICheLiangAnZhuangZhengMingService
    {
        private readonly ICheLiangAnZhuangZhengMingRepository _cheLiangAnZhuangZhengMingRepository;
        private readonly ICheLiangRepository _cheLiangRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly ICheLiangYeHuRepository _cheLiangYeHuRepository;
        private readonly IFuWuShangRepository _fuWuShangRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly IZuZhiGongZhangXinXiRepository _zuZhiGongZhangXinXiRepository;
        private readonly ICheLiangVideoZhongDuanConfirmRepository _cheLiangVideoZhongDuanConfirmRepository;
        private readonly ICheLiangBaoXianXinXiRepository _cheLiangBaoXianXinXiRepository;

        private static string VideoChengNuoHanHtmlModel = "";
        private static string VideoChengNuoHanByBaoXianHtmlModel = "";
        public CheLiangAnZhuangZhengMingService(
             IBussinessLogger _bussinessLogger,
             ICheLiangAnZhuangZhengMingRepository cheLiangAnZhuangZhengMingRepository,
             ICheLiangRepository cheLiangRepository,
             ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
             ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
             ICheLiangYeHuRepository cheLiangYeHuRepository,
             IFuWuShangRepository fuWuShangRepository,
             IOrgBaseInfoRepository orgBaseInfoRepository,
             IZuZhiGongZhangXinXiRepository zuZhiGongZhangXinXiRepository,
             ICheLiangVideoZhongDuanConfirmRepository cheLiangVideoZhongDuanConfirmRepository,
             ICheLiangBaoXianXinXiRepository cheLiangBaoXianXinXiRepository
            ) : base(_bussinessLogger)
        {
            _cheLiangAnZhuangZhengMingRepository = cheLiangAnZhuangZhengMingRepository;
            _cheLiangRepository = cheLiangRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
            _cheLiangYeHuRepository = cheLiangYeHuRepository;
            _fuWuShangRepository = fuWuShangRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _zuZhiGongZhangXinXiRepository = zuZhiGongZhangXinXiRepository;
            _cheLiangVideoZhongDuanConfirmRepository = cheLiangVideoZhongDuanConfirmRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangBaoXianXinXiRepository = cheLiangBaoXianXinXiRepository;
        }

        #region 查询安装证明列表
        public ServiceResult<QueryResult> QueryList(QueryData dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                QueryListRequestDto search = JsonConvert.DeserializeObject<QueryListRequestDto>(JsonConvert.SerializeObject(dto.data));
                if (search == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询参数不能为空" };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }

                IEnumerable<QueryListResponseDto> zmList = GetZhengMingList(userInfo, search);
                QueryResult result = new QueryResult();
                result.totalcount = zmList.Count();
                if (result.totalcount > 0)
                {
                    result.items = zmList.OrderByDescending(x => x.ChuangJianShiJian).Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList();
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询安装证明列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion

        #region 生成智能视频监控报警装置安装证明
        public ServiceResult<bool> GenerateVideoDeviceInstallCert_Test(Guid CheLiangId, UserInfoDto userInfo)
        {
            try
            {
                #region 非空检验
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }
                //获取车辆基本信息与终端信息
                var carInfo = GetCarInstallationCertificateInfo(CheLiangId);
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiYanSe))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌颜色不能为空" };
                }
                if (carInfo.CheLiangXinXi?.CheliangZhongLei == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆种类不能为空" };
                }
                if (carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端安装时间不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.GPSZhongDuanXinXi?.SIMKaHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "SIM卡号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.ShengChanChangJia))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端生产厂家不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.SheBeiXingHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端设备型号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.XiaQuXian))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆辖区县不能为空" };
                }
                if (carInfo.ZhongDuanBeiAnXinXi?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.通过备案)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只有通过备案的车辆才能生成" };
                }
                //查找车辆业户信息
                var cheLiangYeHuXinXi = _cheLiangYeHuRepository.GetQuery(x => x.OrgCode == carInfo.CheLiangXinXi.CheliangYeHuOrgCode && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(cheLiangYeHuXinXi?.OrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆业户名称不能为空" };
                }

                //查找车辆所在辖区县级政府名称
                var XianZhengFuXinXi = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.XiaQuXian == carInfo.CheLiangXinXi.XiaQuXian && x.OrgType == (int)OrganizationType.县政府).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(XianZhengFuXinXi?.OrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆所在辖区的县级交通局单位失败" };
                }

                #endregion

                //处理企业名称中的数字
                var yeHuMingCheng = cheLiangYeHuXinXi.OrgName;
                if (yeHuMingCheng.Trim().Length > 3)
                {
                    char[] yh = yeHuMingCheng.Trim().ToCharArray();
                    for (int i = yh.Length - 1; i >= yh.Length - 3; i--)
                    {
                        if (char.IsNumber(yh[i]))
                        {
                            yh[i] = ' ';
                        }
                    }
                    cheLiangYeHuXinXi.OrgName = new string(yh).Trim();
                }
                //生成文件名
                string outFileName = string.Format("{0}.pdf", string.Concat(carInfo.CheLiangXinXi.ChePaiHao, "智能视频监控报警装置安装证明"));
                //读取打印模板
                string fileContent = string.Empty;
                if (string.IsNullOrWhiteSpace(VideoChengNuoHanHtmlModel))
                {
                    var htmlPath = "/Config/WebFile/ZhiNengShiPinChengNuoHan.html";
                    htmlPath = HttpContext.Current.Server.MapPath(htmlPath);
                    using (var reader = new StreamReader(htmlPath, System.Text.Encoding.Default))
                    {
                        fileContent = reader.ReadToEnd();
                        VideoChengNuoHanHtmlModel = fileContent;
                    }
                }
                else
                {
                    fileContent = VideoChengNuoHanHtmlModel;
                }

                string fuWuShangLianXiPhone = "";

                var fuWuShangModel = _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == carInfo.CheLiangXinXi.FuWuShangOrgCode).FirstOrDefault();
                if (fuWuShangModel != null)
                {
                    fuWuShangLianXiPhone = fuWuShangModel.LianXiRenPhone?.Trim();
                }


                var LocalFileAddress = ConfigurationManager.AppSettings["LocalFileAddress"];
                byte[] QRCodeSrc = BarCodeHelper.GetQRCodeByte(string.Format("{0}/File/GetFile?filename={1}", LocalFileAddress, HttpUtility.UrlEncode(outFileName)), 12);
                if (QRCodeSrc == null || QRCodeSrc.Length <= 0)
                {
                    LogHelper.Error(carInfo.CheLiangXinXi.ChePaiHao + "生成二维码失败");
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "生成二维码失败!", StatusCode = 2 };
                }


                string htmlContent = fileContent.Trim()
                     .Replace("{ChePaiHao}", carInfo.CheLiangXinXi.ChePaiHao)
                     .Replace("{XiaQuXian}", carInfo.CheLiangXinXi.XiaQuXian)
                     .Replace("{XianZhengFuXinXi}", XianZhengFuXinXi?.OrgName)
                     .Replace("{YeHuMingCheng}", cheLiangYeHuXinXi.OrgName)
                     .Replace("{SIMKaHao}", carInfo.GPSZhongDuanXinXi.SIMKaHao)
                     .Replace("{FuWuShangPhone}", fuWuShangLianXiPhone)
                     .Replace("{CheLiangZhongLei}", typeof(CheLiangZhongLei).GetEnumName(carInfo.CheLiangXinXi.CheliangZhongLei))
                     .Replace("{ZhongDuanXingHao}", carInfo.VideoZhongDuanXinXi.SheBeiXingHao)
                     .Replace("{ShengChanChangJiaMingCheng}", carInfo.VideoZhongDuanXinXi?.ShengChanChangJia)
                     .Replace("{AnZhuangShiJianYear}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Year.ToString())
                     .Replace("{AnZhuangShiJianMonth}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Month.ToString())
                     .Replace("{AnZhuangShiJianDay}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Day.ToString())
                     .Replace("{NowYear}", DateTime.Now.Year.ToString())
                     .Replace("{NowMonth}", DateTime.Now.Month.ToString())
                     .Replace("{NowDay}", DateTime.Now.Day.ToString());
                //查找服务商组织公章
                Guid? fuWuShangGongZhangFile = null;
                Guid? jiShuZhiChiFile = null;
                string operatorSealSrc = string.Empty;
                var g = GetByOrgName(carInfo.CheLiangXinXi.FuWuShangOrgCode);
                if (g != null)
                {
                    if (g.Data != null && !string.IsNullOrEmpty(g.Data.GongZhangZhaoPianId.ToString()))
                    {
                        fuWuShangGongZhangFile = g.Data.GongZhangZhaoPianId;
                    }
                }
                //查找技术支持单位公章
                string jiShuZhiChiOrgCode = ConfigurationManager.AppSettings["OperatorSeal"];
                var z = GetByOrgName(jiShuZhiChiOrgCode);
                if (z != null)
                {
                    if (z.Data != null && !string.IsNullOrEmpty(z.Data.GongZhangZhaoPianId.ToString()))
                    {
                        jiShuZhiChiFile = z.Data.GongZhangZhaoPianId;
                    }
                }

                dynamic uploadRes = CommonHelper.CreatePdfAndUploadPdfDoc(htmlContent, outFileName, userInfo, "--zoom 1.5");
                if (uploadRes.success)
                {

                    //生成无水印带二维码的文件
                    Guid? PdfFile = null;//无公章文件
                    var addQRCodeFileList = new List<QRCodeInfoDto>();
                    if (QRCodeSrc?.Length > 0)
                    {
                        var QRCodeModel = new QRCodeInfoDto()
                        {
                            FileData = QRCodeSrc,
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 230,
                            YCoordinate = 720,
                        };
                        addQRCodeFileList.Add(QRCodeModel);
                    }
                    if (addQRCodeFileList.Count() > 0)
                    {
                        PdfAddWatermarkDto fileModel = new PdfAddWatermarkDto()
                        {
                            FileName = outFileName,
                            PdfFileId = uploadRes.FileId,
                            WatermarkList = new List<WatermarkInfoDto>(),
                            QRCodelFileList = addQRCodeFileList
                        };
                        PdfFile = PdfAddWatermark(fileModel, userInfo, PDFOutputFormat.PDF);
                    }


                    //生成带水印公章的文件
                    Guid? shuiYingPdfFile = null;
                    string addWatermarkFileName = string.Concat("(盖章)" + carInfo.CheLiangXinXi.ChePaiHao, "智能视频监控报警装置安装证明");
                    //添加公章水印
                    var imgModelList = new List<WatermarkInfoDto>();
                    if (fuWuShangGongZhangFile != null)
                    {

                        var gongZhang1 = new WatermarkInfoDto
                        {
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 300,
                            YCoordinate = 360,
                            WatermarkFileID = fuWuShangGongZhangFile

                        };
                        imgModelList.Add(gongZhang1);

                    }
                    if (jiShuZhiChiFile != null)
                    {
                        var gongZhang2 = new WatermarkInfoDto
                        {
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 300,
                            YCoordinate = 500,
                            WatermarkFileID = jiShuZhiChiFile

                        };
                        imgModelList.Add(gongZhang2);
                    }
                    //添加二维码
                    var QRCodeFileList = new List<QRCodeInfoDto>();
                    byte[] addWatermarkQRCodeSrc = BarCodeHelper.GetQRCodeByte(string.Format("{0}/File/GetFile?filename={1}", LocalFileAddress, HttpUtility.UrlEncode(addWatermarkFileName)), 12);
                    if (addWatermarkQRCodeSrc?.Length > 0)
                    {

                        var QRCodeModel = new QRCodeInfoDto()
                        {
                            FileData = addWatermarkQRCodeSrc,
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 230,
                            YCoordinate = 720,
                        };
                        QRCodeFileList.Add(QRCodeModel);
                    }
                    if (imgModelList.Count() > 0)
                    {
                        PdfAddWatermarkDto fileModel = new PdfAddWatermarkDto()
                        {
                            FileName = addWatermarkFileName,
                            PdfFileId = uploadRes.FileId,
                            WatermarkList = imgModelList,
                            QRCodelFileList = QRCodeFileList
                        };
                        shuiYingPdfFile = PdfAddWatermark(fileModel, userInfo, PDFOutputFormat.Png);
                    }

                    // 生成安装证明材料记录信息
                    CheLiangAnZhuangZhengMingDto dto = new CheLiangAnZhuangZhengMingDto();
                    dto.CheLiangID = CheLiangId;
                    dto.ZhengMingLeiXin = (int)ZhengMingLeiXin.ZhongXingHuoCheShiPinAnZhuangZhengMing;
                    dto.ZhengMingFileId = PdfFile;
                    dto.ShuiYinPDFFileId = shuiYingPdfFile?.ToString();
                    var createRes = CreateInstallCertMsg(dto, fuWuShangGongZhangFile, userInfo);
                    if (!createRes.Data)
                    {
                        return createRes;
                    }
                }
                else
                {
                    LogHelper.Error("文件服务器返回异常结果:" + JsonConvert.SerializeObject(uploadRes));
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "生成安装证明失败，失败原因：" + uploadRes.msg, StatusCode = 2 };
                }
                return new ServiceResult<bool> { Data = true };

            }
            catch (Exception ex)
            {
                LogHelper.Error("生成佛山定制化安装承诺函出错" + ex.Message, ex);
                return new ServiceResult<bool> { ErrorMessage = "生成出错", StatusCode = 2 };
            }



        }
        #endregion

        #region 生成智能视频监控报警装置安装证明
        public ServiceResult<bool> GenerateVideoDeviceInstallCert(Guid CheLiangId, UserInfoDto userInfo)
        {
            try
            {
                #region 非空检验
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }
                //获取车辆基本信息与终端信息
                var carInfo = GetCarInstallationCertificateInfo(CheLiangId);
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiYanSe))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌颜色不能为空" };
                }
                if (carInfo.CheLiangXinXi?.CheliangZhongLei == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆种类不能为空" };
                }
                if (carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端安装时间不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.GPSZhongDuanXinXi?.SIMKaHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "SIM卡号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.ShengChanChangJia))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端生产厂家不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.SheBeiXingHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端设备型号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.XiaQuXian))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆辖区县不能为空" };
                }
                if (carInfo.ZhongDuanBeiAnXinXi?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.通过备案)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只有通过备案的车辆才能生成" };
                }
                //查找车辆业户信息
                var cheLiangYeHuXinXi = _cheLiangYeHuRepository.GetQuery(x => x.OrgCode == carInfo.CheLiangXinXi.CheliangYeHuOrgCode && x.SYS_XiTongZhuangTai == 0).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(cheLiangYeHuXinXi?.OrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆业户名称不能为空" };
                }

                //查找车辆所在辖区县级政府名称
                var XianZhengFuXinXi = _orgBaseInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.XiaQuXian == carInfo.CheLiangXinXi.XiaQuXian && x.OrgType == (int)OrganizationType.县政府).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(XianZhengFuXinXi?.OrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取车辆所在辖区的县级交通局单位失败" };
                }

                #endregion

                //处理企业名称中的数字
                var yeHuMingCheng = cheLiangYeHuXinXi.OrgName;
                if (yeHuMingCheng.Trim().Length > 3)
                {
                    char[] yh = yeHuMingCheng.Trim().ToCharArray();
                    for (int i = yh.Length - 1; i >= yh.Length - 3; i--)
                    {
                        if (char.IsNumber(yh[i]))
                        {
                            yh[i] = ' ';
                        }
                    }
                    cheLiangYeHuXinXi.OrgName = new string(yh).Trim();
                }
                //生成文件名
                string outFileName = string.Format("{0}.pdf", string.Concat(carInfo.CheLiangXinXi.ChePaiHao, "智能视频监控报警装置安装证明"));
                //读取打印模板
                string fileContent = string.Empty;
                if (string.IsNullOrWhiteSpace(VideoChengNuoHanHtmlModel))
                {
                    var htmlPath = "/Config/WebFile/ZhiNengShiPinChengNuoHan.html";
                    htmlPath = HttpContext.Current.Server.MapPath(htmlPath);
                    using (var reader = new StreamReader(htmlPath, System.Text.Encoding.Default))
                    {
                        fileContent = reader.ReadToEnd();
                        VideoChengNuoHanHtmlModel = fileContent;
                    }
                }
                else
                {
                    fileContent = VideoChengNuoHanHtmlModel;
                }

                string fuWuShangLianXiPhone = "";

                var fuWuShangModel = _fuWuShangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.OrgCode == carInfo.CheLiangXinXi.FuWuShangOrgCode).FirstOrDefault();
                if (fuWuShangModel != null)
                {
                    fuWuShangLianXiPhone = fuWuShangModel.LianXiRenPhone?.Trim();
                }

                string htmlContent = fileContent.Trim()
                     .Replace("{ChePaiHao}", carInfo.CheLiangXinXi.ChePaiHao)
                     .Replace("{XiaQuShi}", carInfo.CheLiangXinXi.XiaQuShi)
                     .Replace("{XiaQuXian}", carInfo.CheLiangXinXi.XiaQuXian)
                     .Replace("{XianZhengFuXinXi}", XianZhengFuXinXi?.OrgName)
                     .Replace("{YeHuMingCheng}", cheLiangYeHuXinXi.OrgName)
                     .Replace("{SIMKaHao}", carInfo.GPSZhongDuanXinXi.SIMKaHao)
                     .Replace("{FuWuShangPhone}", fuWuShangLianXiPhone)
                     .Replace("{CheLiangZhongLei}", typeof(CheLiangZhongLei).GetEnumName(carInfo.CheLiangXinXi.CheliangZhongLei))
                     .Replace("{ZhongDuanXingHao}", carInfo.VideoZhongDuanXinXi.SheBeiXingHao)
                     .Replace("{ShengChanChangJiaMingCheng}", carInfo.VideoZhongDuanXinXi?.ShengChanChangJia)
                     .Replace("{AnZhuangShiJianYear}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Year.ToString())
                     .Replace("{AnZhuangShiJianMonth}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Month.ToString())
                     .Replace("{AnZhuangShiJianDay}", carInfo.VideoZhongDuanXinXi.AnZhuangShiJian.Value.Day.ToString())
                     .Replace("{NowYear}", DateTime.Now.Year.ToString())
                     .Replace("{NowMonth}", DateTime.Now.Month.ToString())
                     .Replace("{NowDay}", DateTime.Now.Day.ToString());
                //查找服务商组织公章
                Guid? fuWuShangGongZhangFile = null;
                Guid? jiShuZhiChiFile = null;
                string operatorSealSrc = string.Empty;
                var g = GetByOrgName(carInfo.CheLiangXinXi.FuWuShangOrgCode);
                if (g != null)
                {
                    if (g.Data != null && !string.IsNullOrEmpty(g.Data.GongZhangZhaoPianId.ToString()))
                    {
                        fuWuShangGongZhangFile = g.Data.GongZhangZhaoPianId;
                    }
                }
                //查找技术支持单位公章
                string jiShuZhiChiOrgCode = ConfigurationManager.AppSettings["OperatorSeal"];
                var z = GetByOrgName(jiShuZhiChiOrgCode);
                if (z != null)
                {
                    if (z.Data != null && !string.IsNullOrEmpty(z.Data.GongZhangZhaoPianId.ToString()))
                    {
                        jiShuZhiChiFile = z.Data.GongZhangZhaoPianId;
                    }
                }

                dynamic uploadRes = CommonHelper.CreatePdfAndUploadPdfDoc(htmlContent, outFileName, userInfo, "--zoom 1.5");
                if (uploadRes.success)
                {

                    //生成带水印公章的文件
                    Guid? shuiYingPdfFile = null;

                    var imgModelList = new List<WatermarkInfoDto>();

                    if (fuWuShangGongZhangFile != null)
                    {

                        var gongZhang1 = new WatermarkInfoDto
                        {
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 300,
                            YCoordinate = 360,
                            WatermarkFileID = fuWuShangGongZhangFile

                        };
                        imgModelList.Add(gongZhang1);

                    }
                    if (jiShuZhiChiFile != null)
                    {
                        var gongZhang2 = new WatermarkInfoDto
                        {
                            Height = 140,
                            Width = 140,
                            PdfPage = 0,
                            XCoordinate = 300,
                            YCoordinate = 500,
                            WatermarkFileID = jiShuZhiChiFile

                        };
                        imgModelList.Add(gongZhang2);
                    }


                    if (imgModelList.Count() > 0)
                    {
                        PdfAddWatermarkDto fileModel = new PdfAddWatermarkDto()
                        {
                            FileName = string.Concat("(盖章)" + carInfo.CheLiangXinXi.ChePaiHao, "智能视频监控报警装置安装证明"),
                            PdfFileId = uploadRes.FileId,
                            WatermarkList = imgModelList
                        };
                        shuiYingPdfFile = PdfAddWatermark(fileModel, userInfo);
                    }



                    // 生成安装证明材料记录信息
                    CheLiangAnZhuangZhengMingDto dto = new CheLiangAnZhuangZhengMingDto();
                    dto.CheLiangID = CheLiangId;
                    dto.ZhengMingLeiXin = (int)ZhengMingLeiXin.ZhongXingHuoCheShiPinAnZhuangZhengMing;
                    dto.ZhengMingFileId = uploadRes.FileId;
                    dto.ShuiYinPDFFileId = shuiYingPdfFile?.ToString();
                    var createRes = CreateInstallCertMsg(dto, fuWuShangGongZhangFile, userInfo);
                    if (!createRes.Data)
                    {
                        return createRes;
                    }
                }
                else
                {
                    LogHelper.Error("文件服务器返回异常结果:" + JsonConvert.SerializeObject(uploadRes));
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "生成安装证明失败，失败原因：" + uploadRes.msg, StatusCode = 2 };
                }
                return new ServiceResult<bool> { Data = true };

            }
            catch (Exception ex)
            {
                LogHelper.Error("生成佛山定制化安装承诺函出错" + ex.Message, ex);
                return new ServiceResult<bool> { ErrorMessage = "生成出错", StatusCode = 2 };
            }



        }
        #endregion


        #region 生成智能视频监控报警装置安装承诺函(保险)

        public ServiceResult<bool> ZhiNengZhiPinChengNuoHanByBaoXian(Guid cheLiangId, UserInfoDto userInfo)
        {
            try
            {
                #region 非空检验
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }
                //获取车辆基本信息与终端信息
                var carInfo = GetCarInstallationCertificateInfo(cheLiangId);
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.ChePaiYanSe))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆车牌颜色不能为空" };
                }
                if (carInfo.CheLiangXinXi?.CheliangZhongLei == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆种类不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangXinXi?.CheJiaHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车架号不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.ShengChanChangJia))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端生产厂家不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.VideoZhongDuanXinXi?.SheBeiXingHao))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "智能视频终端设备型号不能为空" };
                }
                if (carInfo.ZhongDuanBeiAnXinXi?.BeiAnZhuangTai != (int)ZhongDuanBeiAnZhuangTai.通过备案)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "只有通过备案的车辆才能生成" };
                }
                if (carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆智能视频终端安装时间不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangYeHuXinXi?.OrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "车辆业户名称不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianOrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "交强险保险机构名称不能为空" };
                }
                if (string.IsNullOrWhiteSpace(carInfo.CheLiangBaoXianXinXi?.ShangYeXianOrgName))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "商业险保险机构名称不能为空" };
                }
                if (carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianEndTime == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "交强险保险到期日期不能为空" };

                }
                if (carInfo.CheLiangBaoXianXinXi?.ShangYeXianEndTime == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "商业险保险到期日期不能为空" };

                }

                #endregion

                //处理企业名称中的数字
                var yeHuMingCheng = carInfo.CheLiangYeHuXinXi.OrgName;
                if (yeHuMingCheng.Trim().Length > 3)
                {
                    char[] yh = yeHuMingCheng.Trim().ToCharArray();
                    for (int i = yh.Length - 1; i >= yh.Length - 3; i--)
                    {
                        if (char.IsNumber(yh[i]))
                        {
                            yh[i] = ' ';
                        }
                    }
                    carInfo.CheLiangYeHuXinXi.OrgName = new string(yh).Trim();
                }
                //生成文件名
                string outFileName = string.Format("{0}.pdf", string.Concat(carInfo.CheLiangXinXi.ChePaiHao, "广东保险行业重型货运车辆智能视频监控报警装置安装承诺函"));
                //读取打印模板
                string fileContent = string.Empty;
                if (string.IsNullOrWhiteSpace(VideoChengNuoHanByBaoXianHtmlModel))
                {
                    var htmlPath = "/Config/WebFile/ZhiNengShiPinChengNuoHanByBaoXian.html";
                    htmlPath = HttpContext.Current.Server.MapPath(htmlPath);
                    using (var reader = new StreamReader(htmlPath, System.Text.Encoding.Default))
                    {
                        fileContent = reader.ReadToEnd();
                        VideoChengNuoHanByBaoXianHtmlModel = fileContent;
                    }
                }
                else
                {
                    fileContent = VideoChengNuoHanByBaoXianHtmlModel;
                }
                string htmlContent = fileContent.Trim()
                     .Replace("{YeHuMingCheng}", carInfo.CheLiangYeHuXinXi.OrgName)
                     .Replace("{ChePaiHao}", carInfo.CheLiangXinXi.ChePaiHao)
                     .Replace("{CheLiangZhongLei}", typeof(CheLiangZhongLei).GetEnumName(carInfo.CheLiangXinXi.CheliangZhongLei))
                     .Replace("{CheJiaHao}", carInfo.CheLiangXinXi.CheJiaHao)
                     .Replace("{JiaoQiangXianOrgName}", carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianOrgName)
                     .Replace("{JiaoQiangXianEndTime_Year}", carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianEndTime?.Year.ToString())
                     .Replace("{JiaoQiangXianEndTime_Month}", carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianEndTime?.Month.ToString())
                     .Replace("{JiaoQiangXianEndTime_Day}", carInfo.CheLiangBaoXianXinXi?.JiaoQiangXianEndTime?.Day.ToString())
                     .Replace("{ShangYeXianOrgName}", carInfo.CheLiangBaoXianXinXi?.ShangYeXianOrgName)
                     .Replace("{ShangYeXianEndTime_Year}", carInfo.CheLiangBaoXianXinXi?.ShangYeXianEndTime?.Year.ToString())
                     .Replace("{ShangYeXianEndTime_Month}", carInfo.CheLiangBaoXianXinXi?.ShangYeXianEndTime?.Month.ToString())
                     .Replace("{ShangYeXianEndTime_Day}", carInfo.CheLiangBaoXianXinXi?.ShangYeXianEndTime?.Day.ToString())
                     .Replace("{AnZhuangShiJianYear_Year}", carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian?.Year.ToString())
                     .Replace("{AnZhuangShiJianYear_Month}", carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian?.Month.ToString())
                     .Replace("{AnZhuangShiJianYear_Day}", carInfo.VideoZhongDuanXinXi?.AnZhuangShiJian?.Day.ToString())
                     .Replace("{ShengChanChangJiaMingCheng}", carInfo.VideoZhongDuanXinXi?.ShengChanChangJia)
                     .Replace("{ZhongDuanXingHao}", carInfo.VideoZhongDuanXinXi.SheBeiXingHao)
                     .Replace("{NowYear}", DateTime.Now.Year.ToString())
                     .Replace("{NowMonth}", DateTime.Now.Month.ToString())
                     .Replace("{NowDay}", DateTime.Now.Day.ToString());
                dynamic uploadRes = CommonHelper.CreatePdfAndUploadPdfDoc(htmlContent, outFileName, userInfo, "--zoom 1.5");
                if (uploadRes.success)
                {
                    // 生成安装证明材料记录信息
                    CheLiangAnZhuangZhengMingDto dto = new CheLiangAnZhuangZhengMingDto();
                    dto.CheLiangID = cheLiangId;
                    dto.ZhengMingLeiXin = (int)ZhengMingLeiXin.BaoXianShiPinAnZhuangZhengMing;
                    dto.ZhengMingFileId = uploadRes.FileId;
                    dto.ShuiYinPDFFileId = null;
                    var createRes = CreateInstallCertMsg(dto, null, userInfo);
                    if (!createRes.Data)
                    {
                        return createRes;
                    }
                }
                else
                {
                    LogHelper.Error("文件服务器返回异常结果:" + JsonConvert.SerializeObject(uploadRes));
                    return new ServiceResult<bool> { Data = false, ErrorMessage = "生成安装承诺函失败，失败原因：" + uploadRes.msg, StatusCode = 2 };
                }
                return new ServiceResult<bool> { Data = true };

            }
            catch (Exception ex)
            {
                LogHelper.Error("生成智能视频安装承诺函(保险)出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "生成出错", Data = false };
            }


        }
        #endregion

        #region 导出安装证明

        public ServiceResult<ExportResponseDto> ExportAnZhuangZhengMing(QueryListRequestDto search)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (search == null)
                {
                    return new ServiceResult<ExportResponseDto> { StatusCode = 2, ErrorMessage = "查询参数不能为空" };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseDto> { StatusCode = 2, ErrorMessage = "获取登录信息失败" };
                }
                IEnumerable<QueryListResponseDto> zmList = GetZhengMingList(userInfo, search);
                if (zmList.Count() > 0)
                {
                    string fileName = "证明记录_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var fileDate = WriteExcelToByte(zmList.ToList());
                    FileDTO fileDto = new FileDTO()
                    {
                        AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"],
                        AppName = string.Empty,
                        BusinessId = new Guid().ToString(),
                        BusinessType = "",
                        CreatorId = userInfo.Id,
                        CreatorName = userInfo.UserName,
                        DisplayName = fileName,
                        FileName = fileName + ".xls",
                        Remark = string.Empty,
                        SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"]
                    };
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileDto.Data = fileDate.ToArray();
                    }
                    try
                    {
                        FileDto ResFile = FileAgentUtility.UploadFile(fileDto);
                        if (ResFile != null)
                        {
                            return new ServiceResult<ExportResponseDto> { Data = new ExportResponseDto { File = ResFile.FileId.ToString() }, StatusCode = 0 };
                        }
                        else
                        {
                            return new ServiceResult<ExportResponseDto> { StatusCode = 2, ErrorMessage = "上传文件失败" };
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("上传资料证明导出文件失败：" + ex.ToString());
                        return new ServiceResult<ExportResponseDto> { StatusCode = 2, ErrorMessage = "导出失败" };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseDto> { StatusCode = 2, ErrorMessage = "未找到需要导出的数据" };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("导出安装证明出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }
        }

        #endregion

        #region 查询安装证明数据集
        private IEnumerable<QueryListResponseDto> GetZhengMingList(UserInfoDtoNew userInfo, QueryListRequestDto search)
        {
            Expression<Func<CheLiang, bool>> carExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangAnZhuangZhengMing, bool>> zmExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangYeHu, bool>> yhExp = x => x.SYS_XiTongZhuangTai == 0;
            Expression<Func<CheLiangVideoZhongDuanConfirm, bool>> confirmExp = x => x.SYS_XiTongZhuangTai == 0;

            //企业名称
            if (!string.IsNullOrWhiteSpace(search.YeHuMingCheng))
            {
                yhExp = yhExp.And(x => x.OrgName.Contains(search.YeHuMingCheng.Trim()));
            }
            //证明编号
            if (!string.IsNullOrWhiteSpace(search.ZhengMingBianHao))
            {
                zmExp = zmExp.And(x => x.ZhengMingBianHao == search.ZhengMingBianHao.Trim());
            }
            //证明类型
            if (search.ZhengMingLeiXin.HasValue)
            {
                zmExp = zmExp.And(x => x.ZhengMingLeiXin == search.ZhengMingLeiXin);
            }
            //车牌号码
            if (!string.IsNullOrWhiteSpace(search.ChePaiHao))
            {
                carExp = carExp.And(x => x.ChePaiHao.Contains(search.ChePaiHao.Trim()));
            }
            //车牌颜色
            if (!string.IsNullOrWhiteSpace(search.ChePaiYanSe))
            {
                carExp = carExp.And(x => x.ChePaiYanSe == search.ChePaiYanSe.Trim());
            }
            //辖区市
            if (!string.IsNullOrWhiteSpace(search.XiaQuShi))
            {
                carExp = carExp.And(x => x.XiaQuShi == search.XiaQuShi.Trim());
            }
            //辖区县
            if (!string.IsNullOrWhiteSpace(search.XiaQuXian))
            {
                carExp = carExp.And(x => x.XiaQuXian == search.XiaQuXian.Trim());
            }
            //角色权限过滤
            switch (userInfo.OrganizationType)
            {
                case (int)OrganizationType.平台运营商:
                    break;
                case (int)OrganizationType.企业:
                    carExp = carExp.And(x => x.YeHuOrgCode == userInfo.OrganizationCode.Trim());
                    break;
                case (int)OrganizationType.本地服务商:
                    carExp = carExp.And(x => x.FuWuShangOrgCode == userInfo.OrganizationCode.Trim());
                    break;
                case (int)OrganizationType.市政府:
                    carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity.Trim());
                    break;
                case (int)OrganizationType.县政府:
                    carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity.Trim() && x.XiaQuXian == userInfo.OrganDistrict.Trim());
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(userInfo.OrganCity))
                    {
                        carExp = carExp.And(x => x.XiaQuShi == userInfo.OrganCity.Trim());
                    }
                    if (!string.IsNullOrWhiteSpace(userInfo.OrganDistrict))
                    {
                        carExp = carExp.And(x => x.XiaQuXian == userInfo.OrganDistrict.Trim());
                    }
                    break;
            }

            var zmList = from zm in _cheLiangAnZhuangZhengMingRepository.GetQuery(zmExp)
                         join car in _cheLiangRepository.GetQuery(carExp)
                         on zm.CheLiangID equals car.Id
                         join yh in _cheLiangYeHuRepository.GetQuery(yhExp)
                         on car.YeHuOrgCode equals yh.OrgCode
                         select new QueryListResponseDto
                         {
                             Id = zm.Id,
                             ZhengMingBianHao = zm.ZhengMingBianHao,
                             ChePaiHao = car.ChePaiHao,
                             ChePaiYanSe = car.ChePaiYanSe,
                             ChuangJianShiJian = zm.SYS_ChuangJianShiJian,
                             ZhengMingLeiXin = zm.ZhengMingLeiXin,
                             ZhengMingFileId = zm.ZhengMingFileId,
                             ShuiYinPDFFileId = zm.ShuiYinPDFFileId,
                             XiaQuShi = car.XiaQuShi,
                             XiaQuXian = car.XiaQuXian,
                             YeHuMingCheng = yh.OrgName
                         };
            return zmList;
        }

        #endregion

        #region 获取打印证明文件的车辆相关信息

        public CarInfoHuiZong GetCarInstallationCertificateInfo(Guid cheLiangId)
        {
            try
            {
                //车辆基本信息
                var carBaseInfo = _cheLiangRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == cheLiangId)
                    .Select(x => new CarBaseInfoDto
                    {
                        Id = x.Id,
                        ChePaiHao = x.ChePaiHao,
                        ChePaiYanSe = x.ChePaiYanSe,
                        XiaQuShi = x.XiaQuShi,
                        XiaQuXian = x.XiaQuXian,
                        FuWuShangOrgCode = x.FuWuShangOrgCode,
                        CheliangYeHuOrgCode = x.YeHuOrgCode,
                        CheliangZhongLei = x.CheLiangZhongLei,
                        CheJiaHao = x.CheJiaHao
                    }).FirstOrDefault();
                if (carBaseInfo == null)
                {
                    return new CarInfoHuiZong { };
                }
                //车辆业户信息
                var yhInfo = _cheLiangYeHuRepository.GetQuery(x => x.OrgCode == carBaseInfo.CheliangYeHuOrgCode && x.SYS_XiTongZhuangTai == 0).Select(x => new CheLiangYeHuXinXi
                {
                    Id = x.Id,
                    OrgCode = x.OrgCode,
                    OrgName = x.OrgName

                }).FirstOrDefault();
                //车辆智能视频终端信息
                string cheLiangIdStr = carBaseInfo.Id.ToString();
                var zdInfo = _cheLiangVideoZhongDuanXinXiRepository.GetQuery(x => x.CheLiangId == cheLiangIdStr && x.SYS_XiTongZhuangTai == 0).Select(
                    x => new CarVideoZhongDuanInfoDto
                    {
                        Id = x.Id,
                        AnZhuangShiJian = x.AnZhuangShiJian,
                        ShengChanChangJia = x.ShengChanChangJia,
                        SheBeiXingHao = x.SheBeiXingHao

                    }).FirstOrDefault();
                //车辆GPS终端信息
                var gpsInfo = _cheLiangGPSZhongDuanXinXiRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.CheLiangId == cheLiangIdStr).Select
                    (x => new CarGPSZhongDuanInfoDto
                    {
                        Id = x.Id,
                        SIMKaHao = x.SIMKaHao
                    }).FirstOrDefault();
                //车辆保险信息
                var bxInfo = _cheLiangBaoXianXinXiRepository.GetQuery(x => x.CheLiangId == carBaseInfo.Id && x.SYS_XiTongZhuangTai == 0).Select(x => new CheLiangBaoXianInfo
                {
                    Id = x.Id,
                    JiaoQiangXianOrgName = x.JiaoQiangXianOrgName,
                    JiaoQiangXianEndTime = x.JiaoQiangXianEndTime,
                    ShangYeXianOrgName = x.ShangYeXianOrgName,
                    ShangYeXianEndTime = x.ShangYeXianEndTime
                }).FirstOrDefault();

                //终端备案信息
                if (zdInfo == null)
                {
                    zdInfo = new CarVideoZhongDuanInfoDto();
                }
                string videoZhongDuanId = zdInfo.Id.ToString();
                var baInfo = _cheLiangVideoZhongDuanConfirmRepository.GetQuery(x => x.CheLiangId == cheLiangIdStr && x.ZhongDuanId == videoZhongDuanId && x.SYS_XiTongZhuangTai == 0).Select(
                    x => new ZhongDuanConfirmInfo
                    {
                        BeiAnZhuangTai = x.BeiAnZhuangTai
                    }
                    ).FirstOrDefault();

                return new CarInfoHuiZong { CheLiangXinXi = carBaseInfo, VideoZhongDuanXinXi = zdInfo, GPSZhongDuanXinXi = gpsInfo, ZhongDuanBeiAnXinXi = baInfo, CheLiangYeHuXinXi = yhInfo, CheLiangBaoXianXinXi = bxInfo };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取车辆信息出错" + ex.Message, ex);
                return new CarInfoHuiZong { };
            }

        }
        #endregion

        #region 获取组织公章信息
        public ServiceResult<ZuZhiGongZhangXinXiDto> GetByOrgName(string orgCode)
        {
            if (!string.IsNullOrEmpty(orgCode))
            {
                var result = new ServiceResult<ZuZhiGongZhangXinXiDto>();
                var query = _zuZhiGongZhangXinXiRepository.GetQuery(m => m.SYS_XiTongZhuangTai == 0 && m.OrgCode == orgCode);
                result.Data = Mapper.Map<ZuZhiGongZhangXinXiDto>(query.FirstOrDefault());
                return result;
            }
            return null;
        }
        #endregion

        #region 生成安装证明文件记录信息
        public ServiceResult<bool> CreateInstallCertMsg(CheLiangAnZhuangZhengMingDto dto, Guid? GongZhangFileID, UserInfoDto userInfo)
        {
            try
            {
                if (dto.CheLiangID == null || userInfo == null)
                {
                    return new ServiceResult<bool> { Data = false };
                }

                var entity = Mapper.Map<CheLiangAnZhuangZhengMing>(dto);

                //生成证明编号,调用流水号接口
                var getSNoResponse = GetInvokeRequest("00000020013", "1.0", new
                {
                    SysId = ConfigurationManager.AppSettings["WEBAPISYSID"],
                    Module = "00330021",
                    Type = 6
                });
                if (getSNoResponse.publicresponse.statuscode != 0)
                {
                    LogHelper.Error("生成安装证明记录获取流水号接口00000020013返回失败,响应报文:" + JsonConvert.SerializeObject(getSNoResponse));
                    return new ServiceResult<bool> { Data = false };
                }

                entity.ZhengMingBianHao = DateTime.Now.ToString("yyyyMMdd") + getSNoResponse.body.SNo.ToString().PadLeft(4, '0');
                entity.Id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid() : entity.Id;
                entity.SYS_XiTongZhuangTai = 0;
                entity.SYS_ChuangJianRen = userInfo.UserName;
                entity.SYS_ChuangJianRenID = userInfo.Id;
                entity.SYS_ChuangJianShiJian = DateTime.Now;
                entity.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                entity.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                entity.SYS_ZuiJinXiuGaiShiJian = entity.SYS_ChuangJianShiJian;
                entity.GongZhangId = GongZhangFileID;

                bool isSuccess = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _cheLiangAnZhuangZhengMingRepository.Add(entity);
                    isSuccess = uow.CommitTransaction() > 0;
                }
                if (isSuccess)
                {
                    return new ServiceResult<bool> { Data = true };
                }
                else
                {
                    return new ServiceResult<bool> { Data = false };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("添加安装证明记录出错" + ex.Message, ex);
                return new ServiceResult<bool> { Data = false };
            }
        }

        #endregion

        #region DPF文件添加水印后生成指定文件
        /// <summary>
        /// 给PDF文件添加图片水印
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private Guid? PdfAddWatermark(PdfAddWatermarkDto dto, UserInfoDto userInfo, PDFOutputFormat fileType)
        {
            Guid? watermarkFileID = null;
            try
            {
                if (dto?.PdfFileId == null)
                {
                    return watermarkFileID;
                }
                if (dto?.WatermarkList?.Count() <= 0 && dto.QRCodelFileList?.Count() <= 0)
                {
                    return watermarkFileID;
                }
                //找到PDF文件

                var file = FileAgentUtility.GetFileData(dto.PdfFileId);
                if (file?.Length > 0)
                {

                    //读取PDF文件
                    Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                    pdf.LoadFromBytes(file);

                    //水印文件列表
                    foreach (var item in dto.WatermarkList)
                    {

                        if (item.WatermarkFileID.HasValue)
                        {
                            //获取公章图片
                            var OfficialSealImg = FileAgentUtility.GetFileData((Guid)item.WatermarkFileID);
                            if (OfficialSealImg?.Length > 0)
                            {
                                //获取指定页数PDF对象
                                Spire.Pdf.PdfPageBase page = pdf.Pages[item.PdfPage];
                                //读取公章图片
                                MemoryStream mes = new MemoryStream(OfficialSealImg);
                                //Image backimg = Image.FromStream(mes);
                                Spire.Pdf.Graphics.PdfImage img = Spire.Pdf.Graphics.PdfImage.FromStream(mes);
                                mes.Dispose();
                                //设置画布大小和位置
                                Spire.Pdf.Annotations.PdfRubberStampAnnotation loStamp = new Spire.Pdf.Annotations.PdfRubberStampAnnotation(new RectangleF(new PointF(item.XCoordinate, item.YCoordinate), new SizeF(item.Width, item.Height)));
                                Spire.Pdf.Annotations.Appearance.PdfAppearance loApprearance = new Spire.Pdf.Annotations.Appearance.PdfAppearance(loStamp);
                                //新建PDF对象，并将图片绘制到新的PDF，合并PDF
                                Spire.Pdf.Graphics.PdfTemplate template = new Spire.Pdf.Graphics.PdfTemplate(160, 160);
                                template.Graphics.DrawImage(img, 0, 0, item.Width, item.Height);
                                loApprearance.Normal = template;
                                loStamp.Appearance = loApprearance;
                                //合并
                                page.AnnotationsWidget.Add(loStamp);


                                //获取图片并将其设置为页面的背景图
                                //page.BackgroundImage = backimg;
                                ////指定背景图的位置和大小
                                //page.BackgroundRegion = new RectangleF(0, 0, 200, 200);


                            }
                        }
                    }
                    //添加二维码
                    foreach (var item in dto.QRCodelFileList)
                    {
                        if (item.FileData.Length > 0)
                        {
                            //获取指定页数PDF对象
                            Spire.Pdf.PdfPageBase page = pdf.Pages[item.PdfPage];
                            //读取公章图片
                            MemoryStream mes = new MemoryStream(item.FileData);
                            //Image backimg = Image.FromStream(mes);
                            Spire.Pdf.Graphics.PdfImage img = Spire.Pdf.Graphics.PdfImage.FromStream(mes);
                            mes.Dispose();
                            //设置画布大小和位置
                            Spire.Pdf.Annotations.PdfRubberStampAnnotation loStamp = new Spire.Pdf.Annotations.PdfRubberStampAnnotation(new RectangleF(new PointF(item.XCoordinate, item.YCoordinate), new SizeF(item.Width, item.Height)));
                            Spire.Pdf.Annotations.Appearance.PdfAppearance loApprearance = new Spire.Pdf.Annotations.Appearance.PdfAppearance(loStamp);
                            //新建PDF对象，并将图片绘制到新的PDF，合并PDF
                            Spire.Pdf.Graphics.PdfTemplate template = new Spire.Pdf.Graphics.PdfTemplate(160, 160);
                            template.Graphics.DrawImage(img, 0, 0, item.Width, item.Height);
                            loApprearance.Normal = template;
                            loStamp.Appearance = loApprearance;
                            //合并
                            page.AnnotationsWidget.Add(loStamp);

                        }
                    }

                    //DPF加密，修改此处需要备份
                    //pdf.Security.Encrypt("", "pdfpermissionforgdzg@abc123", PdfPermissionsFlags.Print, PdfEncryptionKeySize.Key128Bit);
                    //输出修改后的图片
                    string fileName = dto.FileName;
                    string extension = "pdf";
                    if (fileType == PDFOutputFormat.Png)
                    {
                        extension = "png";
                    }


                    //上传文件
                    FileDTO fileDto = new FileDTO()
                    {
                        SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                        AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                        AppName = "",
                        CreatorId = userInfo?.Id,
                        CreatorName = userInfo?.UserName,
                        BusinessType = "",
                        BusinessId = "",
                        FileName = fileName + "." + extension,
                        FileExtension = extension,
                        DisplayName = fileName,
                        Remark = ""
                    };
                    using (MemoryStream ms = new MemoryStream())
                    {
                        if (fileType == PDFOutputFormat.Png)
                        {
                            //将PDF的第一页生成图片，X和Y系均采用300PDI像素，保证打印清晰
                            pdf.SaveAsImage(0, 300, 300).Save(ms, ImageFormat.Png);
                            fileDto.Data = ms.ToArray();
                            //pdf.SaveAsImage(0, 300, 300).Save("D://pdf.png", ImageFormat.Png);
                        }
                        else if (fileType == PDFOutputFormat.PDF)
                        {
                            pdf.SaveToStream(ms);
                            fileDto.Data = ms.ToArray();
                        }

                    }
                    FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
                    if (fileDtoResult != null)
                    {
                        watermarkFileID = fileDtoResult.FileId;
                    }
                    else
                    {
                        LogHelper.Error("上传添加水印PDF文件失败" + dto.PdfFileId);
                    }
                }

                return watermarkFileID;
            }
            catch (Exception ex)
            {
                LogHelper.Error("PDF添加水印出错" + ex.Message, ex);
                return null;
            }
        }

        public enum PDFOutputFormat
        {
            PDF = 1,
            Png = 2,
        }

        #endregion

        #region DPF文件添加水印后生成PNF文件
        /// <summary>
        /// 给PDF文件添加图片水印
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private Guid? PdfAddWatermark(PdfAddWatermarkDto dto, UserInfoDto userInfo)
        {
            Guid? watermarkFileID = null;
            try
            {
                if (dto?.PdfFileId == null)
                {
                    return watermarkFileID;
                }
                if (dto?.WatermarkList?.Count() <= 0)
                {
                    return watermarkFileID;
                }
                //找到PDF文件

                var file = FileAgentUtility.GetFileData(dto.PdfFileId);
                if (file?.Length > 0)
                {

                    //读取PDF文件
                    Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                    pdf.LoadFromBytes(file);

                    //水印文件列表
                    foreach (var item in dto.WatermarkList)
                    {

                        if (item.WatermarkFileID.HasValue)
                        {
                            //获取公章图片
                            var OfficialSealImg = FileAgentUtility.GetFileData((Guid)item.WatermarkFileID);
                            if (OfficialSealImg?.Length > 0)
                            {
                                //获取指定页数PDF对象
                                Spire.Pdf.PdfPageBase page = pdf.Pages[item.PdfPage];
                                //读取公章图片
                                MemoryStream mes = new MemoryStream(OfficialSealImg);
                                //Image backimg = Image.FromStream(mes);
                                Spire.Pdf.Graphics.PdfImage img = Spire.Pdf.Graphics.PdfImage.FromStream(mes);
                                mes.Dispose();
                                //设置画布大小和位置
                                Spire.Pdf.Annotations.PdfRubberStampAnnotation loStamp = new Spire.Pdf.Annotations.PdfRubberStampAnnotation(new RectangleF(new PointF(item.XCoordinate, item.YCoordinate), new SizeF(item.Width, item.Height)));
                                Spire.Pdf.Annotations.Appearance.PdfAppearance loApprearance = new Spire.Pdf.Annotations.Appearance.PdfAppearance(loStamp);
                                //新建PDF对象，并将图片绘制到新的PDF，合并PDF
                                Spire.Pdf.Graphics.PdfTemplate template = new Spire.Pdf.Graphics.PdfTemplate(160, 160);
                                template.Graphics.DrawImage(img, 0, 0, item.Width, item.Height);
                                loApprearance.Normal = template;
                                loStamp.Appearance = loApprearance;
                                //合并
                                page.AnnotationsWidget.Add(loStamp);


                                //获取图片并将其设置为页面的背景图
                                //page.BackgroundImage = backimg;
                                ////指定背景图的位置和大小
                                //page.BackgroundRegion = new RectangleF(0, 0, 200, 200);


                            }
                        }
                    }
                    //DPF加密，修改此处需要备份
                    //pdf.Security.Encrypt("", "pdfpermissionforgdzg@abc123", PdfPermissionsFlags.Print, PdfEncryptionKeySize.Key128Bit);
                    //输出修改后的图片
                    string fileName = dto.FileName;
                    string extension = "png";

                    //上传文件
                    FileDTO fileDto = new FileDTO()
                    {
                        SystemId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString(),
                        AppId = ConfigurationManager.AppSettings["WEBAPIAPPID"].ToString(),
                        AppName = "",
                        CreatorId = userInfo?.Id,
                        CreatorName = userInfo?.UserName,
                        BusinessType = "",
                        BusinessId = "",
                        FileName = fileName + "." + extension,
                        FileExtension = extension,
                        DisplayName = fileName,
                        Remark = ""
                    };
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //将PDF的第一页生成图片，X和Y系均采用300PDI像素，保证打印清晰
                        pdf.SaveAsImage(0, 300, 300).Save(ms, ImageFormat.Png);
                        fileDto.Data = ms.ToArray();
                        //pdf.SaveAsImage(0, 300, 300).Save("D://pdf.png", ImageFormat.Png);

                    }
                    FileDto fileDtoResult = FileAgentUtility.UploadFile(fileDto);
                    if (fileDtoResult != null)
                    {
                        watermarkFileID = fileDtoResult.FileId;
                    }
                    else
                    {
                        LogHelper.Error("上传添加水印PDF文件失败" + dto.PdfFileId);
                    }
                }

                return watermarkFileID;
            }
            catch (Exception ex)
            {
                LogHelper.Error("PDF添加水印出错" + ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region 导出Excel
        private byte[] WriteExcelToByte(List<QueryListResponseDto> Rows)
        {
            PagingUtil<QueryListResponseDto> pu = new PagingUtil<QueryListResponseDto>(Rows, 60000);
            NopiExcel nopi = new NopiExcel(0);
            while (pu.IsEffectivePage)
            {
                List<QueryListResponseDto> pageData = pu.GetCurrentPage();
                int start = ((pu.PageNo - 1) * pu.PageSize) + 1;
                int end = ((pu.PageNo - 1) * pu.PageSize) + pageData.Count();
                nopi.SetCurrentCreateNewSheet(start + "-" + end);
                nopi.InsertRow(0, 0, new string[]{
                    "证明编号",
                    "车牌号",
                    "车牌颜色",
                    "企业名称",
                    "辖区市",
                    "辖区县",
                    "证明类型",
                    "证明生成日期"
                });
                for (int i = 0; i < pageData.Count; i++)
                {
                    string zhengMingLeiXing = string.Empty;
                    switch (pageData[i].ZhengMingLeiXin)
                    {
                        case (int)ZhengMingLeiXin.BusinessProcessCert:
                            zhengMingLeiXing = "业务办理证明";
                            break;
                        case (int)ZhengMingLeiXin.GPSInstallCert:
                            zhengMingLeiXing = "卫星定位安装证明";
                            break;
                        case (int)ZhengMingLeiXin.VedioInstallCert:
                            zhengMingLeiXing = "智能视频安装承诺函";
                            break;
                        case (int)ZhengMingLeiXin.VedioInstallCertification:
                            zhengMingLeiXing = "智能视频安装证明";
                            break;
                        case (int)ZhengMingLeiXin.FoShanGPSInstallCert:
                            zhengMingLeiXing = "卫星定位安装证明(佛山)";
                            break;
                        case (int)ZhengMingLeiXin.FoShanVedioInstallCert:
                            zhengMingLeiXing = "视频报警装置安装承诺函(佛山)";
                            break;
                        case (int)ZhengMingLeiXin.FoShanVedioInstallCertification:
                            zhengMingLeiXing = "智能视频安装证明(佛山)";
                            break;
                        case (int)ZhengMingLeiXin.FoShanTransmissionCertification:
                            zhengMingLeiXing = "GPS传输证明(佛山)";
                            break;
                        case (int)ZhengMingLeiXin.ZhongXingHuoCheShiPinAnZhuangZhengMing:
                            zhengMingLeiXing = "智能视频监控报警装置安装证明";
                            break;
                        case (int)ZhengMingLeiXin.BaoXianShiPinAnZhuangZhengMing:
                            zhengMingLeiXing = "保险行业智能视频监控报警装置安装承诺函";
                            break;
                        default:
                            zhengMingLeiXing = "业务办理证明";
                            break;
                    }
                    var startRow = i + 1;
                    nopi.WriteCell(startRow, 0, StringFormat(pageData[i].ZhengMingBianHao));
                    nopi.WriteCell(startRow, 1, StringFormat(pageData[i].ChePaiHao));
                    nopi.WriteCell(startRow, 2, StringFormat(pageData[i].ChePaiYanSe));//数据库类型为navrcahr(20),使用以数字枚举，可能需要修改。
                    nopi.WriteCell(startRow, 3, StringFormat(pageData[i].YeHuMingCheng));
                    nopi.WriteCell(startRow, 4, StringFormat(pageData[i].XiaQuShi));
                    nopi.WriteCell(startRow, 5, StringFormat(pageData[i].XiaQuXian));
                    nopi.WriteCell(startRow, 6, StringFormat(zhengMingLeiXing));
                    nopi.WriteCell(startRow, 7, StringFormat(pageData[i].ChuangJianShiJian.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                //样式
                nopi.SetColumnWidth(0, nopi.getLastCellNum(1), 17 * 256);//列宽
                nopi.SetCellHeight(0, 1, 50 * 20);//高
                nopi.SetCellHeight(1, nopi.getLastRowNum() - 1, 27 * 20);//高    
                nopi.SetVerticalCenter(0, 0, nopi.getLastRowNum() - 1, nopi.getLastCellNum(1));//垂直居中
                nopi.SetHorizontalCenter(0, 0, nopi.getLastRowNum() - 1, nopi.getLastCellNum(1));//水平居中
                nopi.DrawThinBorder(1, 0, nopi.getLastRowNum() - 1, nopi.getLastCellNum(1));//边框
                pu.GotoNextPage();
            }
            return nopi.SaveToBinary();
        }

        private string StringFormat(object obj)
        {
            return String.Format("{0}", obj);
        }
        #endregion

        public override void Dispose()
        {
            _cheLiangAnZhuangZhengMingRepository.Dispose();
            _cheLiangRepository.Dispose();
            _cheLiangVideoZhongDuanXinXiRepository.Dispose();
            _cheLiangYeHuRepository.Dispose();
            _fuWuShangRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
            _zuZhiGongZhangXinXiRepository.Dispose();
        }
    }
}
