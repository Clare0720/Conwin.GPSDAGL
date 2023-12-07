using Conwin.EntityFramework.Extensions;
using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.AnZhuangZhongDuan;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services;
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

namespace Conwin.GPSDAGL.Services
{
    public class AnZhuangZhongDuanService : ApiServiceBase, IAnZhuangZhongDuanService
    {
        private readonly ICheLiangRepository _cheLiangRepository;
        private readonly ICheLiangGPSZhongDuanXinXiRepository _cheLiangGPSZhongDuanXinXiRepository;
        private readonly ICheLiangVideoZhongDuanXinXiRepository _cheLiangVideoZhongDuanXinXiRepository;

        public AnZhuangZhongDuanService(ICheLiangRepository cheLiangRepository,
                ICheLiangGPSZhongDuanXinXiRepository cheLiangGPSZhongDuanXinXiRepository,
                ICheLiangVideoZhongDuanXinXiRepository cheLiangVideoZhongDuanXinXiRepository,
                IBussinessLogger _bussinessLogger
            ) : base(_bussinessLogger)
        {
            _cheLiangRepository = cheLiangRepository;
            _cheLiangGPSZhongDuanXinXiRepository = cheLiangGPSZhongDuanXinXiRepository;
            _cheLiangVideoZhongDuanXinXiRepository = cheLiangVideoZhongDuanXinXiRepository;
        }

        public override void Dispose() { }

        /// <summary>
        /// 车辆终端安装列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            var result = new ServiceResult<QueryResult>();
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录" };
                }

                GetCheLiangJieRuXinXiDto resultData = GetAnZhuangZhuangZhongDuanXinXi(queryData, userInfo);

                var queryResult = new QueryResult()
                {
                    totalcount = resultData.Count,
                    items = resultData.list
                };

                result.StatusCode = 0;
                result.Data = queryResult;
                return result;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());

                result.StatusCode = 2;
                result.ErrorMessage = "获取车辆终端安装列表失败";
                return result;
            }

        }

        public ServiceResult<ExportResponseInfoDto> ExportZhongDuanAnZhuangInfo(QueryData queryData)
        {
            try
            {
                UserInfoDtoNew userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                var list = GetAnZhuangZhuangZhongDuanXinXi(queryData, userInfo)?.list;
                string tableTitle = "车辆终端安装表" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        return new ServiceResult<ExportResponseInfoDto> { Data = new ExportResponseInfoDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出车辆终端安装表" + e.Message, e);
                        return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
                    }
                }
                else
                {
                    return new ServiceResult<ExportResponseInfoDto> { StatusCode = 2, ErrorMessage = "没有需要导出的数据" };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出车辆终端安装表" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }

        }


        private GetCheLiangJieRuXinXiDto GetAnZhuangZhuangZhongDuanXinXi(QueryData queryData, DtosExt.UserInfoDtoNew userInfo)
        {
            try
            {
                AnZhuangZhongDuanQueryReqDto dto = JsonConvert.DeserializeObject<AnZhuangZhongDuanQueryReqDto>(queryData.data.ToString());
                var sysZhengChang = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<CheLiang, bool>> clExp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<CheLiangGPSZhongDuanXinXi, bool>> gpsExp = t => t.SYS_XiTongZhuangTai == sysZhengChang;
                Expression<Func<CheLiangVideoZhongDuanXinXi, bool>> videoExp = t => t.SYS_XiTongZhuangTai == sysZhengChang;

                //企业用户可以看到自己管辖范围下的车辆
                if (userInfo.OrganizationType == (int)OrganizationType.企业
                    || userInfo.OrganizationType == (int)OrganizationType.个体户)
                {
                    clExp = clExp.And(u => u.YeHuOrgCode == userInfo.OrganizationCode && u.YeHuOrgType == userInfo.OrganizationType);
                }
                //政府可以看到自己辖区范围内的车
                if (userInfo.OrganizationType == (int)OrganizationType.市政府
                    || userInfo.OrganizationType == (int)OrganizationType.县政府)
                {
                    switch (userInfo.OrganizationType)
                    {
                        case (int)OrganizationType.市政府:
                            dto.XiaQuShi = string.Empty;
                            clExp = clExp.And(u => u.XiaQuShi == userInfo.OrganCity);
                            break;
                        case (int)OrganizationType.县政府:
                            dto.XiaQuShi = string.Empty;
                            dto.XiaQuXian = string.Empty;
                            clExp = clExp.And(u => u.XiaQuXian == userInfo.OrganDistrict);
                            break;
                    }
                }
                if (userInfo.OrganizationType == (int)OrganizationType.本地服务商)
                {
                    clExp = clExp.And(u => u.FuWuShangOrgCode == userInfo.OrganizationCode);
                }

                if (!string.IsNullOrWhiteSpace(dto.ChePaiHao))
                {
                    clExp = clExp.And(p => p.ChePaiHao.Contains(dto.ChePaiHao.Trim().ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(dto.ChePaiYanSe))
                {
                    clExp = clExp.And(p => p.ChePaiYanSe == dto.ChePaiYanSe.Trim());
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuShi))
                {
                    clExp = clExp.And(p => p.XiaQuShi == dto.XiaQuShi.Trim());
                }
                if (!string.IsNullOrWhiteSpace(dto.XiaQuXian))
                {
                    clExp = clExp.And(p => p.XiaQuXian == dto.XiaQuXian.Trim());
                }

                var list = from cl in _cheLiangRepository.GetQuery(clExp)
                           join gps in _cheLiangGPSZhongDuanXinXiRepository.GetQuery(gpsExp) on cl.Id.ToString() equals gps.CheLiangId into t1
                           from gpsTemp in t1.DefaultIfEmpty()
                           join video in _cheLiangVideoZhongDuanXinXiRepository.GetQuery(videoExp) on cl.Id.ToString() equals video.CheLiangId into t2
                           from videoTemp in t2.DefaultIfEmpty()
                           where (gpsTemp.Id != null || videoTemp.Id != null)
                           select new AnZhuangZhongDuanListResDto
                           {
                               Id = cl.Id,
                               ChePaiHao = cl.ChePaiHao,
                               ChePaiYanSe = cl.ChePaiYanSe,
                               ChuangJianShiJian = cl.SYS_ChuangJianShiJian,
                               GPSZhongDuanMDT = gpsTemp.ZhongDuanMDT,
                               GPSSIMKaHao = gpsTemp.SIMKaHao,
                               VideoZhongDuanMDT = videoTemp.ZhongDuanMDT,
                               VideoAnZhuangShiJian = videoTemp.AnZhuangShiJian
                           };

                if (!string.IsNullOrWhiteSpace(dto.GPSZhongDuanMDT))
                {
                    list = list.Where(p => p.GPSZhongDuanMDT.StartsWith(dto.GPSZhongDuanMDT.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(dto.GPSSIMKaHao))
                {
                    list = list.Where(p => p.GPSSIMKaHao.StartsWith(dto.GPSSIMKaHao.Trim()));
                }
                if (!string.IsNullOrWhiteSpace(dto.VideoZhongDuanMDT))
                {
                    list = list.Where(p => p.VideoZhongDuanMDT.StartsWith(dto.VideoZhongDuanMDT.Trim()));
                }


                GetCheLiangJieRuXinXiDto resultData = new GetCheLiangJieRuXinXiDto();
                resultData.Count = list.Count();
                if (resultData.Count > 0)
                {
                    var take = queryData.rows < 1 ? 10 : queryData.rows;
                    var skip = ((queryData.page < 1 ? 1 : queryData.page) - 1) * take;
                    resultData.list = list.OrderByDescending(s => s.ChuangJianShiJian).Skip(skip).Take(take).ToList();

                }

                return resultData;

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取终端安装信息出错" + ex.Message, ex);
                return new GetCheLiangJieRuXinXiDto { };
            }
        }


        #region 导出车辆安装终端信息
        private static Guid? CreatePingTaiZhiLingQingQiuAndYingDaExcelAndUpload(List<AnZhuangZhongDuanListResDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "车辆安装终端信息";
            string[] cellTitleArry = { "车牌号码", "车牌颜色", "GPS终端号", "SIM卡号", "智能视频终端号", "智能视频终端安装时间" };

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
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));

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

                    row.CreateCell(index++).SetCellValue(item.ChePaiHao);
                    row.CreateCell(index++).SetCellValue(item.ChePaiYanSe);
                    row.CreateCell(index++).SetCellValue(item.GPSZhongDuanMDT);
                    row.CreateCell(index++).SetCellValue(item.GPSSIMKaHao);
                    row.CreateCell(index++).SetCellValue(item.VideoZhongDuanMDT);
                    string anZhuangShiJian = "";
                    if (item.VideoAnZhuangShiJian.HasValue)
                    {
                        anZhuangShiJian = Convert.ToDateTime(item.VideoAnZhuangShiJian).ToString("yyyy-MM-dd");
                    }
                    row.CreateCell(index++).SetCellValue(anZhuangShiJian);



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



    }
}
