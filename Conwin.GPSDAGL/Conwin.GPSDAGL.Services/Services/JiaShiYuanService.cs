using Conwin.EntityFramework;
using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.ServiceAgent.Dtos;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Conwin.GPSDAGL.Services.Services
{
    public partial class JiaShiYuanService : ApiServiceBase, IJiaShiYuanService
    {
        private readonly IJiaShiYuanRepository _jiaShiYuanRepository;
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        private readonly ICheLiangRepository _cheLiangRepository;

        public JiaShiYuanService(
            IBussinessLogger bussinessLogger,
            IJiaShiYuanRepository jiaShiYuanRepository,
            IOrgBaseInfoRepository orgBaseInfoRepository,
            ICheLiangRepository cheLiangRepository)
            : base(bussinessLogger)
        {
            _jiaShiYuanRepository = jiaShiYuanRepository;
            _orgBaseInfoRepository = orgBaseInfoRepository;
            _cheLiangRepository = cheLiangRepository;
        }

        #region 驾驶员列表信息查询

        /// <summary>
        /// 驾驶员列表信息查询
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> Query(QueryData queryData)
        {
            return ExecuteCommandStruct<QueryResult>(() =>
            {
                var userInfoNew = GetUserInfo();

                JiaShiYuanSearchDto searchDto = JsonConvert.DeserializeObject<JiaShiYuanSearchDto>(queryData.data.ToString());

                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<JiaShiYuan, bool>> jiashiyuanExp = j => j.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(j.OrgCode);
                Expression<Func<OrgBaseInfo, bool>> orgbaseinfoExp = o => o.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(o.OrgCode);
                Expression<Func<CheLiang, bool>> cheliangExp = c => c.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(c.ChePaiHao);

                var jiashiyuanQuery = _jiaShiYuanRepository.GetQuery(jiashiyuanExp);
                var orgbaseinfoQuery = _orgBaseInfoRepository.GetQuery(orgbaseinfoExp);
                var cheliangQuery = _cheLiangRepository.GetQuery(cheliangExp);

                var query = from j in jiashiyuanQuery
                            join o in orgbaseinfoQuery on j.OrgCode equals o.OrgCode into temp1
                            from t1 in temp1.DefaultIfEmpty()
                            join c in cheliangQuery on j.CheLiangId equals c.Id.ToString() into temp2
                            from t2 in temp2.DefaultIfEmpty()
                            select new JiaShiYuanListResDto()
                            {
                                Id = j.Id,
                                Name = j.Name,
                                Cellphone = j.Cellphone,
                                IDCard = j.IDCard,
                                Certification = j.Certification,
                                Sex = j.Sex,
                                OrgCode = t1.OrgCode,
                                OrgName = t1.OrgName,
                                ChePaiHao = t2.ChePaiHao,
                                ChePaiYanSe = t2.ChePaiYanSe,
                                WorkingStatus = j.WorkingStatus,
                                XiaQuSheng = t1.XiaQuSheng,
                                XiaQuShi = t1.XiaQuShi,
                                XiaQuXian = t1.XiaQuXian,
                                EntryDate = j.EntryDate,
                                DismissalDate = j.DismissalDate,
                                ChuangJianShiJian = j.SYS_ChuangJianShiJian,
                                JingYingFanWei = t1.JingYingFanWei,
                            };

                //数据权限：只能看到当前辖区市/县的驾驶员数据
                switch (userInfoNew.OrganizationType.Value)
                {
                    case (int)OrganizationType.市政府:
                        query = query.Where(t => t.XiaQuShi == userInfoNew.OrganCity || t.JingYingFanWei.Contains(userInfoNew.OrganCity));
                        break;
                    case (int)OrganizationType.县政府:
                        query = query.Where(t => t.XiaQuXian == userInfoNew.OrganDistrict || t.JingYingFanWei.Contains(userInfoNew.OrganCity));
                        break;
                    case (int)OrganizationType.企业:
                        query = query.Where(t => t.OrgCode == userInfoNew.OrganizationCode);
                        break;
                }

                //姓名
                if (!string.IsNullOrWhiteSpace(searchDto.XingMing))
                {
                    query = query.Where(t => t.Name.Contains(searchDto.XingMing.Trim()));
                }

                //证件号码
                if (!string.IsNullOrWhiteSpace(searchDto.ZhengJianHaoMa))
                {
                    query = query.Where(t => t.IDCard.Contains(searchDto.ZhengJianHaoMa.Trim()));
                }

                //状态：0=待确认；1=聘用；2=解聘；
                if (searchDto.ZhuangTai.HasValue && searchDto.ZhuangTai.Value >= 0)
                {
                    query = query.Where(t => t.WorkingStatus == searchDto.ZhuangTai.Value);
                }

                if (userInfoNew.OrganizationType.Value == (int)OrganizationType.企业)
                {
                    //联系电话
                    if (!string.IsNullOrWhiteSpace(searchDto.LianXiDianHua))
                    {
                        query = query.Where(t => t.Cellphone.Contains(searchDto.LianXiDianHua.Trim()));
                    }
                }
                else
                {
                    //企业名称
                    if (!string.IsNullOrWhiteSpace(searchDto.QiYeMingCheng))
                    {
                        query = query.Where(t => t.OrgName.Contains(searchDto.QiYeMingCheng.Trim()));
                    }
                }

                QueryResult queryResult = new QueryResult()
                {
                    totalcount = query.Count(),
                    items = query.OrderByDescending(r => r.ChuangJianShiJian).Skip((queryData.page - 1) * queryData.rows).Take(queryData.rows).ToList()
                };

                return new ServiceResult<QueryResult>() { Data = queryResult };
            });
        }

        #endregion

        #region 驾驶员信息导出

        /// <summary>
        /// 驾驶员信息导出
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public ServiceResult<JiaShiYuanExportResDto> Export(JiaShiYuanSearchDto searchDto)
        {
            return ExecuteCommandStruct<JiaShiYuanExportResDto>(() =>
            {
                var userInfoNew = GetUserInfo();

                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                Expression<Func<JiaShiYuan, bool>> jiashiyuanExp = j => j.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(j.OrgCode);
                Expression<Func<OrgBaseInfo, bool>> orgbaseinfoExp = o => o.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(o.OrgCode);
                Expression<Func<CheLiang, bool>> cheliangExp = c => c.SYS_XiTongZhuangTai == sysStatus && !string.IsNullOrEmpty(c.ChePaiHao);

                var jiashiyuanQuery = _jiaShiYuanRepository.GetQuery(jiashiyuanExp);
                var orgbaseinfoQuery = _orgBaseInfoRepository.GetQuery(orgbaseinfoExp);
                var cheliangQuery = _cheLiangRepository.GetQuery(cheliangExp);

                var query = from j in jiashiyuanQuery
                            join o in orgbaseinfoQuery on j.OrgCode equals o.OrgCode into temp1
                            from t1 in temp1.DefaultIfEmpty()
                            join c in cheliangQuery on j.CheLiangId equals c.Id.ToString() into temp2
                            from t2 in temp2.DefaultIfEmpty()
                            select new JiaShiYuanListResDto()
                            {
                                Id = j.Id,
                                Name = j.Name,
                                Cellphone = j.Cellphone,
                                IDCard = j.IDCard,
                                OrgCode = t1.OrgCode,
                                OrgName = t1.OrgName,
                                ChePaiHao = t2.ChePaiHao,
                                ChePaiYanSe = t2.ChePaiYanSe,
                                WorkingStatus = j.WorkingStatus,
                                XiaQuSheng = t1.XiaQuSheng,
                                XiaQuShi = t1.XiaQuShi,
                                XiaQuXian = t1.XiaQuXian,
                                EntryDate = j.EntryDate,
                                DismissalDate = j.DismissalDate,
                                ChuangJianShiJian = j.SYS_ChuangJianShiJian,
                                JingYingFanWei = t1.JingYingFanWei,
                            };

                //数据权限：只能看到当前辖区市/县的驾驶员数据
                switch (userInfoNew.OrganizationType.Value)
                {
                    case (int)OrganizationType.市政府:
                        query = query.Where(t => t.XiaQuShi == userInfoNew.OrganCity || t.JingYingFanWei.Contains(userInfoNew.OrganCity));
                        break;
                    case (int)OrganizationType.县政府:
                        query = query.Where(t => t.XiaQuXian == userInfoNew.OrganDistrict || t.JingYingFanWei.Contains(userInfoNew.OrganCity));
                        break;
                    case (int)OrganizationType.企业:
                        query = query.Where(t => t.OrgCode == userInfoNew.OrganizationCode);
                        break;
                }

                //姓名
                if (!string.IsNullOrWhiteSpace(searchDto.XingMing))
                {
                    query = query.Where(t => t.Name.Contains(searchDto.XingMing.Trim()));
                }

                //证件号码
                if (!string.IsNullOrWhiteSpace(searchDto.ZhengJianHaoMa))
                {
                    query = query.Where(t => t.IDCard.Contains(searchDto.ZhengJianHaoMa.Trim()));
                }

                //状态：0=待确认；1=聘用；2=解聘；
                if (searchDto.ZhuangTai.HasValue && searchDto.ZhuangTai.Value >= 0)
                {
                    query = query.Where(t => t.WorkingStatus == searchDto.ZhuangTai.Value);
                }

                if (userInfoNew.OrganizationType.Value == (int)OrganizationType.企业)
                {
                    //联系电话
                    if (!string.IsNullOrWhiteSpace(searchDto.LianXiDianHua))
                    {
                        query = query.Where(t => t.Cellphone.Contains(searchDto.LianXiDianHua.Trim()));
                    }
                }
                else
                {
                    //企业名称
                    if (!string.IsNullOrWhiteSpace(searchDto.QiYeMingCheng))
                    {
                        query = query.Where(t => t.OrgName.Contains(searchDto.QiYeMingCheng.Trim()));
                    }
                }

                var list = query.OrderByDescending(r => r.ChuangJianShiJian).ToList();

                if (list == null || list.Count == 0)
                {
                    return new ServiceResult<JiaShiYuanExportResDto>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "未查询到驾驶员列表信息"
                    };
                }

                //创建工作簿
                HSSFWorkbook excelBook = new HSSFWorkbook();
                //为工作簿创建工作表并命名
                NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("驾驶员列表");
                //合并标题单元格
                NPOI.SS.UserModel.IRow row = sheet1.CreateRow(0);//标题   
                string title = "驾驶员列表";
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
                row.CreateCell(0).SetCellValue($"{title}");
                //创建表头
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(1);//先创建一行用来放表头         
                row1.CreateCell(0).SetCellValue("姓名");//第0行，第0列
                row1.CreateCell(1).SetCellValue("证件号码");//第0行，第1列
                if (userInfoNew.OrganizationType.HasValue && userInfoNew.OrganizationType.Value == (int)OrganizationType.企业)
                {
                    row1.CreateCell(2).SetCellValue("手机号码");//第0行，第2列
                }
                else
                {
                    row1.CreateCell(2).SetCellValue("企业名称");//第0行，第2列
                }
                row1.CreateCell(3).SetCellValue("绑定车辆");//第0行，第3列
                row1.CreateCell(4).SetCellValue("状态");//第0行，第4列
                row1.CreateCell(5).SetCellValue("聘用（解聘）日期");//第0行，第5列

                #region 单元格样式
                //标题样式
                ICellStyle titleStyle = excelBook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                IFont titleFont = excelBook.CreateFont();
                titleFont.FontName = "宋体";
                titleFont.FontHeightInPoints = 16;
                titleFont.Boldweight = short.MaxValue;
                titleStyle.SetFont(titleFont);

                //列表标题样式
                ICellStyle cellStyle = excelBook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                IFont cellFont = excelBook.CreateFont();
                cellFont.FontName = "宋体";
                cellFont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellFont);

                //内容样式
                ICellStyle contentStyle = excelBook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Center;

                IFont contentFont = excelBook.CreateFont();
                contentFont.FontName = "宋体";
                contentFont.FontHeightInPoints = 12;
                contentStyle.SetFont(contentFont);

                #endregion

                row.Cells[0].CellStyle = titleStyle;


                for (int cell_index = 0; cell_index < 6; cell_index++)
                {
                    //附加表头样式
                    row1.Cells[cell_index].CellStyle = cellStyle;
                }

                //创建数据行
                for (int i = 0; i < list.Count(); i++)
                {
                    //创建行
                    NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 2);//因为第一行已经被表头占用了，所以要+1
                    rowTemp.CreateCell(0).SetCellValue(list[i].Name);
                    rowTemp.CreateCell(1).SetCellValue(list[i].IDCard);
                    if (userInfoNew.OrganizationType.HasValue && userInfoNew.OrganizationType.Value == (int)OrganizationType.企业)
                    {
                        rowTemp.CreateCell(2).SetCellValue(list[i].Cellphone);
                    }
                    else
                    {
                        rowTemp.CreateCell(2).SetCellValue(list[i].OrgName);
                    }
                    var bindVehicle = list[i].ChePaiHao;
                    if (!string.IsNullOrWhiteSpace(list[i].ChePaiYanSe))
                    {
                        bindVehicle += string.Format("（{0}）", list[i].ChePaiYanSe);
                    }
                    rowTemp.CreateCell(3).SetCellValue(bindVehicle);
                    rowTemp.CreateCell(4).SetCellValue(list[i].WorkingStatusText);
                    rowTemp.CreateCell(5).SetCellValue(list[i].WorkingDate);
                    for (int contInx = 0; contInx < 6; contInx++)
                    {
                        //附加内容样式
                        rowTemp.Cells[contInx].CellStyle = contentStyle;
                    }
                }



                //命名文件名
                var fileName = "驾驶员列表-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //将Excel表格转化为byte[]
                byte[] file;
                //创建文件流
                using (MemoryStream bookStream = new MemoryStream())
                {
                    //文件写入流（向流中写入字节序列）
                    excelBook.Write(bookStream);
                    //输出之前调用Seek（偏移量，游标位置) 把0位置指定为开始位置
                    bookStream.Seek(0, SeekOrigin.Begin);
                    file = bookStream.ToArray();
                }

                //文件数组上传到文件服务器
                string fileId = CommonHelper.UploadFile(userInfoNew, fileName, file);

                return new ServiceResult<JiaShiYuanExportResDto>()
                {
                    Data = new JiaShiYuanExportResDto() { File = fileId }
                };
            });
        }

        #endregion

        #region 驾驶员详情

        /// <summary>
        /// 驾驶员详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<JiaShiYuanDetailResDto> Detail(string id)
        {
            return ExecuteCommandStruct<JiaShiYuanDetailResDto>(() =>
            {
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var query = from j in _jiaShiYuanRepository.GetQuery(j => j.SYS_XiTongZhuangTai == sysStatus)
                            join o in _orgBaseInfoRepository.GetQuery(o => o.SYS_XiTongZhuangTai == sysStatus) on j.OrgCode equals o.OrgCode into temp1
                            from t1 in temp1.DefaultIfEmpty()
                            join c in _cheLiangRepository.GetQuery(c => c.SYS_XiTongZhuangTai == sysStatus) on j.CheLiangId equals c.Id.ToString() into temp2
                            from t2 in temp2.DefaultIfEmpty()
                            where j.Id.ToString() == id
                            select new JiaShiYuanDetailResDto()
                            {
                                Id = j.Id,
                                Name = j.Name,
                                Cellphone = j.Cellphone,
                                IDCard = j.IDCard,
                                WorkingStatus = j.WorkingStatus.HasValue ? j.WorkingStatus.Value : 0,
                                OrgCode = j.OrgCode,
                                OrgName = t1.OrgName,
                                ChePaiHao = t2.ChePaiHao,
                                EntryDate = j.EntryDate,
                                DismissalDate = j.DismissalDate,
                                Certification = j.Certification,
                                Sex = j.Sex,
                                GuoJi = j.GuoJi,
                                HuKouDiZhi = j.HuKouDiZhi,
                                Birthday = j.Birthday,
                                CertificationEndTime = j.CertificationEndTime,
                                FaZhengJiGou = j.FaZhengJiGou,
                                LianXiDiZhi = j.LianXiDiZhi,
                                ShenFenZhengZhengMianId = j.ShenFenZhengZhengMian,
                                ShenFenZhengFanMianId = j.ShenFenZhengFanMian,
                                JiaShiYuanZhengMianId = j.JiaShiYuanZhengMian,
                                JiaZhaoChuCiShenLing = j.JiaZhaoChuCiShenLing,
                                ZhunJiaCheXing = j.ZhunJiaCheXing,
                                JiaZhaoHaoMa = j.JiaZhaoHaoMa,
                                JiaZhaoBianHao = j.JiaZhaoBianHao,
                                NianJianRiQi = j.NianJianRiQi,
                                JiaZhaoYouXiaoQi = j.JiaZhaoYouXiaoQi,
                                JiaShiZhengZhengMianId = j.JiaShiZhengZhengMian,
                                JiaShiZhengFanMianId = j.JiaShiZhengFanMian
                            };

                var jiashiyuan = query.FirstOrDefault();

                if (jiashiyuan != null)
                {
                    return new ServiceResult<JiaShiYuanDetailResDto>()
                    {
                        Data = jiashiyuan
                    };
                }
                else
                {
                    return new ServiceResult<JiaShiYuanDetailResDto>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "未查询到该驾驶员信息"
                    };
                }
            });
        }

        #endregion

        #region 创建驾驶员信息

        /// <summary>
        /// 创建驾驶员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Create(JiaShiYuanCreateReqDto dto, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (dto == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "未查询到该驾驶员信息",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员姓名不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.IDCard))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "身份证号码不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Cellphone))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "手机号码不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Certification))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "从业资格证号码不能为空",
                        Data = false
                    };
                }
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var existCt = _jiaShiYuanRepository.Count(j => j.SYS_XiTongZhuangTai == sysStatus && j.IDCard == dto.IDCard);
                if (existCt > 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "证件号码已存在",
                        Data = false
                    };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "用户信息不能为空",
                        Data = false
                    };
                }

                //数据组装
                JiaShiYuan model = new JiaShiYuan()
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Cellphone = dto.Cellphone,
                    IDCardType = (int)JiaShiYuanIDCardType.IDCard,//默认0=身份证
                    IDCard = dto.IDCard,
                    WorkingStatus = 0,//默认0=待确认
                    OrgCode = userInfo.OrganizationCode,
                    Certification = dto.Certification,
                    Sex = dto.Sex,
                    GuoJi = dto.GuoJi,
                    HuKouDiZhi = dto.HuKouDiZhi,
                    Birthday = dto.Birthday,
                    CertificationEndTime = dto.CertificationEndTime,
                    FaZhengJiGou = dto.FaZhengJiGou,
                    LianXiDiZhi = dto.LianXiDiZhi,
                    ShenFenZhengZhengMian = dto.ShenFenZhengZhengMianId,
                    ShenFenZhengFanMian = dto.ShenFenZhengFanMianId,
                    JiaShiYuanZhengMian = dto.JiaShiYuanZhengMianId,
                    JiaZhaoChuCiShenLing = dto.JiaZhaoChuCiShenLing,
                    ZhunJiaCheXing = dto.ZhunJiaCheXing,
                    JiaZhaoHaoMa = dto.JiaZhaoHaoMa,
                    JiaZhaoBianHao = dto.JiaZhaoBianHao,
                    NianJianRiQi = dto.NianJianRiQi,
                    JiaZhaoYouXiaoQi = dto.JiaZhaoYouXiaoQi,
                    JiaShiZhengZhengMian = dto.JiaShiZhengZhengMianId,
                    JiaShiZhengFanMian = dto.JiaShiZhengFanMianId
                };
                SetCreateSYSInfo(model, userInfo);
                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _jiaShiYuanRepository.Add(model);
                    res = uow.CommitTransaction();
                }
                if (res > 0)
                {
                    return new ServiceResult<bool>() { Data = true };
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员信息创建失败"
                    };
                }
            });
        }

        #endregion

        #region 修改驾驶员信息

        /// <summary>
        /// 修改驾驶员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Update(JiaShiYuanUpdateReqDto dto, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (dto == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "未查询到该驾驶员信息",
                        Data = false
                    };
                }
                if (dto.Id == null || string.IsNullOrWhiteSpace(dto.Id.ToString()))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员ID不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员姓名不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.IDCard))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "身份证号码不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Cellphone))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "手机号码不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Certification))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "从业资格证号码不能为空",
                        Data = false
                    };
                }
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var existCt = _jiaShiYuanRepository.Count(j => j.SYS_XiTongZhuangTai == sysStatus
                        && j.Id.ToString() != dto.Id && j.IDCard == dto.IDCard);
                if (existCt > 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "证件号码已存在",
                        Data = false
                    };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "用户信息不能为空",
                        Data = false
                    };
                }
                var model = _jiaShiYuanRepository.First(j => j.SYS_XiTongZhuangTai == sysStatus && j.Id.ToString() == dto.Id);
                if (model == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "不存在该驾驶员数据",
                        Data = false
                    };
                }

                //数据组装
                model.Name = dto.Name;
                model.Cellphone = dto.Cellphone;
                model.IDCard = dto.IDCard;
                model.Certification = dto.Certification;
                model.Sex = dto.Sex;
                model.GuoJi = dto.GuoJi;
                model.HuKouDiZhi = dto.HuKouDiZhi;
                model.Birthday = dto.Birthday;
                model.CertificationEndTime = dto.CertificationEndTime;
                model.FaZhengJiGou = dto.FaZhengJiGou;
                model.LianXiDiZhi = dto.LianXiDiZhi;
                model.ShenFenZhengZhengMian = dto.ShenFenZhengZhengMianId;
                model.ShenFenZhengFanMian = dto.ShenFenZhengFanMianId;
                model.JiaShiYuanZhengMian = dto.JiaShiYuanZhengMianId;
                model.JiaZhaoChuCiShenLing = dto.JiaZhaoChuCiShenLing;
                model.ZhunJiaCheXing = dto.ZhunJiaCheXing;
                model.JiaZhaoHaoMa = dto.JiaZhaoHaoMa;
                model.JiaZhaoBianHao = dto.JiaZhaoBianHao;
                model.NianJianRiQi = dto.NianJianRiQi;
                model.JiaZhaoYouXiaoQi = dto.JiaZhaoYouXiaoQi;
                model.JiaShiZhengZhengMian = dto.JiaShiZhengZhengMianId;
                model.JiaShiZhengFanMian = dto.JiaShiZhengFanMianId;
                SetUpdateSYSInfo(model, model, userInfo);

                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    _jiaShiYuanRepository.Update(model);
                    res = uow.CommitTransaction();
                }
                if (res > 0)
                {
                    return new ServiceResult<bool>() { Data = true };
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员信息修改失败"
                    };
                }
            });
        }

        #endregion

        #region 删除驾驶员信息

        /// <summary>
        /// 删除驾驶员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> Delete(string[] ids, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (ids == null || ids.Length == 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员ID不能为空",
                        Data = false
                    };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "用户信息不能为空",
                        Data = false
                    };
                }
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var models = _jiaShiYuanRepository.GetQuery(j => j.SYS_XiTongZhuangTai == sysStatus && ids.Contains(j.Id.ToString())).ToList();
                if (models == null || models.Count == 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "不存在该驾驶员数据",
                        Data = false
                    };
                }
                var notExists = ids.Except(models.Select(j => j.Id.ToString()).ToList()).ToList();
                if (ids.Length != models.Count || (notExists != null && notExists.Count > 0))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = $"未找到驾驶员数据（{string.Join(",", notExists)}）",
                        Data = false
                    };
                }

                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //删除数据
                    foreach (var model in models)
                    {
                        model.SYS_XiTongZhuangTai = (int)XiTongZhuangTaiEnum.作废;
                        model.SYS_ZuiJinXiuGaiShiJian = DateTime.Now;
                        model.SYS_ZuiJinXiuGaiRen = userInfo.UserName;
                        model.SYS_ZuiJinXiuGaiRenID = userInfo.Id;
                        _jiaShiYuanRepository.Update(model);
                    }
                    res = uow.CommitTransaction();
                }

                if (res > 0)
                {
                    return new ServiceResult<bool>() { Data = true };
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员信息删除失败"
                    };
                }
            });
        }

        #endregion

        #region 聘用/解聘驾驶员

        /// <summary>
        /// 聘用/解聘驾驶员
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> HireOrDismissal(JiaShiYuanHireReqDto dto, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (dto == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "请求参数不能为空",
                        Data = false
                    };
                }
                if (dto.Id == null || dto.Id.Length == 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员ID不能为空",
                        Data = false
                    };
                }
                if (dto.OperationType != 1 && dto.OperationType != 2)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "操作类型错误",
                        Data = false
                    };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "用户信息不能为空",
                        Data = false
                    };
                }
                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var models = _jiaShiYuanRepository.GetQuery(j => j.SYS_XiTongZhuangTai == sysStatus && dto.Id.Contains(j.Id.ToString())).ToList();
                if (models == null || models.Count == 0)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "不存在该驾驶员数据",
                        Data = false
                    };
                }
                var notExists = dto.Id.Except(models.Select(j => j.Id.ToString()).ToList()).ToList();
                if (dto.Id.Length != models.Count || (notExists != null && notExists.Count > 0))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = $"未找到驾驶员数据（{string.Join(",", notExists)}）",
                        Data = false
                    };
                }

                if (dto.OperationType == 1)
                {
                    //已聘用的驾驶员不可再次聘用
                    var notHires = models.Where(x => x.WorkingStatus == (int)JiaShiYuanWorkStatus.Hire).ToList();
                    if (notHires != null && notHires.Count > 0)
                    {
                        return new ServiceResult<bool>()
                        {
                            StatusCode = 2,
                            ErrorMessage = $"已聘用的驾驶员无法再次聘用（{string.Join(",", notHires.Select(x => x.Name).ToList())}）"
                        };
                    }
                }
                if (dto.OperationType == 2)
                {
                    //待确认的驾驶员不可解聘
                    var waitConforms = models.Where(x => x.WorkingStatus == null || x.WorkingStatus == 0).ToList();
                    if (waitConforms != null && waitConforms.Count > 0)
                    {
                        return new ServiceResult<bool>()
                        {
                            StatusCode = 2,
                            ErrorMessage = $"待确认的驾驶员无法进行解聘（{string.Join(",", waitConforms.Select(x => x.Name).ToList())}）"
                        };
                    }
                    //已解聘的驾驶员无法再次解聘
                    var dismissals = models.Where(x => x.WorkingStatus == (int)JiaShiYuanWorkStatus.Dismissal).ToList();
                    if (dismissals != null && dismissals.Count > 0)
                    {
                        return new ServiceResult<bool>()
                        {
                            StatusCode = 2,
                            ErrorMessage = $"已解聘的驾驶员无法再次解聘（{string.Join(",", dismissals.Select(x => x.Name).ToList())}）"
                        };
                    }
                }

                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    //分操作类型进行数据修改
                    foreach (var model in models)
                    {
                        switch (dto.OperationType)
                        {
                            case 1:  //聘用
                                model.WorkingStatus = (int)JiaShiYuanWorkStatus.Hire;
                                model.EntryDate = DateTime.Now;
                                break;
                            case 2:  //解聘
                                model.WorkingStatus = (int)JiaShiYuanWorkStatus.Dismissal;
                                model.DismissalDate = DateTime.Now;
                                break;
                        }
                        SetUpdateSYSInfo(model, model, userInfo);

                        _jiaShiYuanRepository.Update(model);
                    }
                    res = uow.CommitTransaction();
                }

                if (res > 0)
                {
                    return new ServiceResult<bool>() { Data = true };
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = string.Format("驾驶员{0}失败", dto.OperationType == 1 ? "聘用" : "解聘")
                    };
                }
            });
        }

        #endregion

        #region 驾驶员绑定车辆

        /// <summary>
        /// 驾驶员绑定车辆
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> BindVehicle(JiaShiYuanVehicleReqDto dto, UserInfoDto userInfo)
        {
            return ExecuteCommandStruct<bool>(() =>
            {
                //数据校验
                if (dto == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "请求参数不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.Id))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员ID不能为空",
                        Data = false
                    };
                }
                if (string.IsNullOrWhiteSpace(dto.CheLiangId))
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "车辆ID不能为空",
                        Data = false
                    };
                }
                if (userInfo == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "用户信息不能为空",
                        Data = false
                    };
                }

                var sysStatus = (int)XiTongZhuangTaiEnum.正常;
                var model = _jiaShiYuanRepository.GetQuery(j => j.SYS_XiTongZhuangTai == sysStatus && dto.Id == j.Id.ToString()).FirstOrDefault();
                if (model == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "不存在该驾驶员数据",
                        Data = false
                    };
                }
                if (model.WorkingStatus != (int)JiaShiYuanWorkStatus.Hire)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员非聘用状态不允许绑定车辆",
                        Data = false
                    };
                }
                var cheliang = _cheLiangRepository.GetQuery(j => j.SYS_XiTongZhuangTai == sysStatus && dto.CheLiangId == j.Id.ToString()).FirstOrDefault();
                if (cheliang == null)
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "不存在该车辆数据",
                        Data = false
                    };
                }

                int res = 0;
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    model.CheLiangId = dto.CheLiangId;
                    SetUpdateSYSInfo(model, model, userInfo);
                    _jiaShiYuanRepository.Update(model);
                    res = uow.CommitTransaction();
                }

                if (res > 0)
                {
                    return new ServiceResult<bool>() { Data = true };
                }
                else
                {
                    return new ServiceResult<bool>()
                    {
                        StatusCode = 2,
                        ErrorMessage = "驾驶员绑定车辆失败"
                    };
                }
            });
        }

        #endregion


        public override void Dispose()
        {
            _jiaShiYuanRepository.Dispose();
            _orgBaseInfoRepository.Dispose();
            _cheLiangRepository.Dispose();
        }
    }
}
