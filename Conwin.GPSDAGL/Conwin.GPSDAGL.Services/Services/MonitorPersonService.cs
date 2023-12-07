using AutoMapper;
using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.FileModule.ServiceAgent;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.FileAgent;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.MonitorPersonInfo;
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

namespace Conwin.GPSDAGL.Services.Services
{
    public class MonitorPersonService : ApiServiceBase, IMonitorPersonInfoService
    {
        private readonly IMonitorPersonInfoRepository _monitorPersonInfoRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;

        public MonitorPersonService(
            IBussinessLogger _bussinessLogger,
            IMonitorPersonInfoRepository monitorPersonInfoRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository
            ) : base(_bussinessLogger)
        {
            _monitorPersonInfoRepository = monitorPersonInfoRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }

        /// <summary>
        /// 创建企业监控人员档案 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Create(MonitorPersonInfo model)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录" };
                }
                if (string.IsNullOrWhiteSpace(model?.Name))
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "" };
                }

                if (_monitorPersonInfoRepository.GetQuery(x => x.IDCard == model.IDCard.Trim() && x.SYS_XiTongZhuangTai == 0).Count() > 1)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "证件号码已存在。" };
                }
                bool isSuccess = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    var entity = Mapper.Map<MonitorPersonInfo>(model);
                    entity.OrgCode = userInfo.OrganizationCode;
                    entity.Id = Guid.NewGuid();
                    //插入系统信息
                    SetCreateSYSInfo(entity, userInfo);
                    _monitorPersonInfoRepository.Add(entity);

                    isSuccess = uow.CommitTransaction() > 0;
                }

                return new ServiceResult<bool> { Data = isSuccess };
            }
            catch (Exception ex)
            {
                LogHelper.Error("创建企业监控人员功能出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "新增失败" };
            }
        }
        /// <summary>
        /// 删除安全员档案
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Delete(Guid[] ids)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
                }
                bool isSuccess = false;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _monitorPersonInfoRepository.Update(
                        m => ids.Contains(m.Id),
                        n => new MonitorPersonInfo()
                        {
                            SYS_XiTongZhuangTai = 1,
                            SYS_ZuiJinXiuGaiRen = userInfo.UserName,
                            SYS_ZuiJinXiuGaiRenID = userInfo.Id,
                            SYS_ZuiJinXiuGaiShiJian = DateTime.Now
                        });
                }
                return new ServiceResult<bool> { Data = isSuccess };

            }
            catch (Exception ex)
            {
                LogHelper.Error("删除企业安全监控人员出错" + ex.Message, ex);
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "删除出错" };
            }
        }
        /// <summary>
        /// 企业监控人员详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<object> Get(Guid id)
        {
            try
            {
                var personModel = _monitorPersonInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == id).FirstOrDefault();
                return new ServiceResult<object> { Data = personModel };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取企业监控人员详情出错" + ex.Message, ex);
                return new ServiceResult<object> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }
        /// <summary>
        /// 查询安全员信息列表
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            try
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<QueryResult>() { StatusCode = 2, ErrorMessage = "获取用户信息失败，请重新登录。" };
                }
                Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == 0;
                Expression<Func<MonitorPersonInfo, bool>> PeopleExp = q => q.SYS_XiTongZhuangTai == 0;
                MonitorPersonInfoDto search = JsonConvert.DeserializeObject<MonitorPersonInfoDto>(queryData.data.ToString());
                if (userInfoNew.OrganizationType == (int)OrganizationType.企业)
                {
                    OrgBaseExp = OrgBaseExp.And(x => x.OrgCode == userInfoNew.OrganizationCode);
                }

                var list = from p in _monitorPersonInfoRepository.GetQuery(PeopleExp)
                           join o in _orgBaseInfoRepository.GetQuery(OrgBaseExp) on p.OrgCode equals o.OrgCode
                           select new QueryMonitorPersonListDto
                           {
                               Id = p.Id,
                               Name = p.Name,
                               OrgName = o.OrgName,
                               IDCard = p.IDCard,
                               Tel = p.Tel,
                               IDCardFrontId = p.IDCardFrontId,
                               IDCardBackId = p.IDCardBackId,
                               LaborContractFileId = p.LaborContractFileId,
                               CertificatePassingExaminationFileId = p.CertificatePassingExaminationFileId,
                               SocialSecurityContractFileId = p.SocialSecurityContractFileId,
                               ChuangJianShiJian = p.SYS_ChuangJianShiJian
                           };


                QueryResult result = new QueryResult();
                result.totalcount = list.Count();
                if (result.totalcount > 0)
                {
                    result.items = list.OrderByDescending(x => x.ChuangJianShiJian).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList();
                }

                return new ServiceResult<QueryResult> { Data = result };

            }
            catch (Exception ex)
            {
                LogHelper.Error("查询企业监控管理人员列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }
        /// <summary>
        /// 更新企业监控员信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Update(MonitorPersonInfoDto model)
        {

            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录。" };
            };
            var id = new Guid(model.Id);
            var perModel = _monitorPersonInfoRepository.GetQuery(x => x.SYS_XiTongZhuangTai == 0 && x.Id == id).FirstOrDefault();
            if (perModel == null)
            {
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "未能找到相关人员信息记录，请重新查询" };
            }

            if (_monitorPersonInfoRepository.GetQuery(x => x.IDCard == model.IDCard.Trim() && x.SYS_XiTongZhuangTai == 0).Count() > 1)
            {
                return new ServiceResult<bool> { StatusCode = 2, ErrorMessage = "证件号码已存在。" };
            }

            perModel.Name = model.Name.Trim();
            perModel.IDCard = model.IDCard.Trim();
            perModel.Tel = model.Tel;
            perModel.IDCardBackId = model.IDCardBackId;
            perModel.IDCardFrontId = model.IDCardFrontId;
            perModel.CertificatePassingExaminationFileId = model.CertificatePassingExaminationFileId;
            perModel.LaborContractFileId = model.LaborContractFileId;
            perModel.SocialSecurityContractFileId = model.SocialSecurityContractFileId;
            perModel.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
            perModel.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
            perModel.SYS_ZuiJinXiuGaiRenID = userInfo.Id;


            bool isSuccess = false;
            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                _monitorPersonInfoRepository.Update(perModel);
                isSuccess = uow.CommitTransaction() > 0;
            }

            return new ServiceResult<bool> { Data = isSuccess };
        }
        public ServiceResult<ExportResponseInfoDto> ExportMonitorPersonInfo(QueryData queryData)
        {
            try
            {
                var userInfoNew = GetUserInfo();
                if (userInfoNew == null)
                {
                    return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "获取登录信息失败，请重新登录", StatusCode = 2 };
                }
                Expression<Func<OrgBaseInfo, bool>> OrgBaseExp = q => q.SYS_XiTongZhuangTai == 0;
                Expression<Func<MonitorPersonInfo, bool>> PeopleExp = q => q.SYS_XiTongZhuangTai == 0;
                MonitorPersonInfoDto search = JsonConvert.DeserializeObject<MonitorPersonInfoDto>(queryData.data.ToString());
                if (userInfoNew.OrganizationType == (int)OrganizationType.企业)
                {
                    OrgBaseExp = OrgBaseExp.And(x => x.OrgCode == userInfoNew.OrganizationCode);
                }

                var list = from p in _monitorPersonInfoRepository.GetQuery(PeopleExp)
                           join o in _orgBaseInfoRepository.GetQuery(OrgBaseExp) on p.OrgCode equals o.OrgCode
                           select new QueryMonitorPersonListDto
                           {
                               Id = p.Id,
                               Name = p.Name,
                               OrgName = o.OrgName,
                               IDCard = p.IDCard,
                               Tel = p.Tel,
                               IDCardFrontId = p.IDCardFrontId,
                               IDCardBackId = p.IDCardBackId,
                               LaborContractFileId = p.LaborContractFileId,
                               CertificatePassingExaminationFileId = p.CertificatePassingExaminationFileId,
                               SocialSecurityContractFileId = p.SocialSecurityContractFileId,
                               ChuangJianShiJian = p.SYS_ChuangJianShiJian
                           };
                string tableTitle = "企业监控人员档案" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (list != null && list.Count() > 0)
                {
                    try
                    {
                        string FileId = string.Empty;
                        Guid? fileUploadId = CreateAnQuanGuanLiRenYuanExcelAndUpload(list.ToList(), tableTitle);
                        if (fileUploadId != null)
                        {
                            FileId = fileUploadId.ToString();
                        }
                        return new ServiceResult<ExportResponseInfoDto> { Data = new ExportResponseInfoDto { FileId = FileId } };
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("导出企业监控人员档案出错" + e.Message, e);
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
                LogHelper.Error("导出企业监控人员档案出错" + ex.Message, ex);
                return new ServiceResult<ExportResponseInfoDto> { ErrorMessage = "导出出错", StatusCode = 2 };
            }


        }
        private static Guid? CreateAnQuanGuanLiRenYuanExcelAndUpload(List<QueryMonitorPersonListDto> list, string fileName)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            string title = "企业监控人员档案";
            string[] cellTitleArry = { "姓名", "证件号码", "企业名称", "联系电话" };

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
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));

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

                    row.CreateCell(index++).SetCellValue(item.Name);
                    row.CreateCell(index++).SetCellValue(item.IDCard);
                    row.CreateCell(index++).SetCellValue(item.OrgName);
                    row.CreateCell(index++).SetCellValue(item.Tel);
                    for (int contInx = 0; contInx < index; contInx++)
                    {
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


        public override void Dispose()
        {
            _monitorPersonInfoRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
        }
    }
}
