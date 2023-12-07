using AutoMapper;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities.Enums;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Framework.OperationLog;
using Conwin.GPSDAGL.Services.DtosExt.OperLog;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class OperLogService : ApiServiceBase, IOperLogService
    {
        private readonly IOrgBaseInfoRepository _orgBaseInfoRepository;
        public OperLogService(
            IOrgBaseInfoRepository orgBaseInfoRepository,
        IBussinessLogger _bussinessLogger
            ) : base(_bussinessLogger)
        {
            _orgBaseInfoRepository = orgBaseInfoRepository;
        }

        public ServiceResult<QueryResult> QueryOperLogList(QueryData dto)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录" };
                }
                OperationLogQueryDto search = JsonConvert.DeserializeObject<OperationLogQueryDto>(JsonConvert.SerializeObject(dto.data));
                if (search.RecordTimeEnd.HasValue)
                {
                    search.RecordTimeEnd = search.RecordTimeEnd.Value.AddDays(1).AddSeconds(-1);
                }
                QueryResult result = new QueryResult();
                var list = OperLogHelper.QueryOperLog(new OperLogQueryData<OperationLogQueryDto> { page = dto.page, rows = dto.rows, data = search });

                if (list.StatusCode == 0)
                {
                    result.totalcount = list.Data.totalcount;
                    result.items = list.Data.items;
                }
                else
                {
                    LogHelper.Error("查询操作日志信息返回失败" + list.ErrorMessage + ",请求参数：" + JsonConvert.SerializeObject(dto));
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询失败" };
                }
                return new ServiceResult<QueryResult> { Data = result };

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取操作日志信息失败" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }

        }


        public ServiceResult<LogDetailsInfoDto> GetOperLogDetails(Guid? logID)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (userInfo == null)
                {
                    return new ServiceResult<LogDetailsInfoDto> { StatusCode = 2, ErrorMessage = "获取登录信息失败，请重新登录" };
                }

                var list = OperLogHelper.QueryOperLog(new OperLogQueryData<OperationLogQueryDto> { page = 1, rows = 1, data = new OperationLogQueryDto { ID = new List<string> { logID.ToString() } } });
                LogDetailsInfoDto model = null;
                if (list.StatusCode == 0)
                {
                    var detailsInfo = list.Data.items.FirstOrDefault();
                    if (detailsInfo != null)
                    {
                        Mapper.CreateMap<OperationLogResponseDto, LogDetailsInfoDto>();
                        model = Mapper.Map<LogDetailsInfoDto>(detailsInfo);
                        if (model.BizOperType == OperLogBizOperType.UPDATE)
                        {
                            if (!string.IsNullOrWhiteSpace(detailsInfo.ExtendInfo))
                            {
                                model.DetailsList = JsonConvert.DeserializeObject<List<LogUpdateValueDto>>(detailsInfo.ExtendInfo);
                                LogFilter(model.DetailsList);
                            }
                        }
                    }
                    return new ServiceResult<LogDetailsInfoDto> { Data = model };

                }
                else
                {
                    LogHelper.Error("查询操作日志信息返回失败" + list.ErrorMessage + ",请求参数：" + JsonConvert.SerializeObject(logID));
                    return new ServiceResult<LogDetailsInfoDto> { StatusCode = 2, ErrorMessage = "查询失败" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取日志详情出错" + ex.Message, ex);
                return new ServiceResult<LogDetailsInfoDto>();
            }
        }


        private List<LogUpdateValueDto> LogFilter(List<LogUpdateValueDto> dtoList)
        {
            try
            {
                dtoList.ForEach(x =>
                {
                    if (x.AttributesName == "业户代码")
                    {
                        var oldYeHuMingCheng = _orgBaseInfoRepository.GetQuery(t => t.SYS_XiTongZhuangTai == 0 && t.OrgCode == x.OldValue).FirstOrDefault()?.OrgName;
                        var newYeHuMingCheng = _orgBaseInfoRepository.GetQuery(t => t.SYS_XiTongZhuangTai == 0 && t.OrgCode == x.NewValue).FirstOrDefault()?.OrgName;
                        x.AttributesName = "业户名称";
                        x.OldValue = oldYeHuMingCheng;
                        x.NewValue = newYeHuMingCheng;

                    }
                    if (x.AttributesName == "GPS终端类型")
                    {
                        x.OldValue = GetZhongDuanLeiXingName(x.OldValue);
                        x.NewValue = GetZhongDuanLeiXingName(x.NewValue);
                    }
                    if (x.AttributesName == "是否安装视频终端")
                    {
                        x.OldValue = GetIntToStr(x.OldValue);
                        x.NewValue = GetIntToStr(x.NewValue);
                    }
                    if (x.AttributesName == "视频厂商类型")
                    {
                        x.OldValue = GetChangShangLeiXing(x.OldValue);
                        x.NewValue = GetChangShangLeiXing(x.NewValue);
                    }
                    if (x.AttributesName == "智能视频设备机身类型")
                    {
                        x.OldValue = GetSheBeiJiShenLeiXing(x.OldValue);
                        x.NewValue = GetSheBeiJiShenLeiXing(x.NewValue);
                    }
                    if (x.AttributesName == "车身颜色")
                    {
                        x.OldValue = GetCheShenYanSe(x.OldValue);
                        x.NewValue = GetCheShenYanSe(x.NewValue);
                    }
                    if (x.AttributesName == "燃料")
                    {
                        x.OldValue = GetRanLiaoName(x.OldValue);
                        x.NewValue = GetRanLiaoName(x.NewValue);
                    }
                    if (x.AttributesName == "车辆类型")
                    {
                        x.OldValue = GetCheLiangLeiXing(x.OldValue);
                        x.NewValue = GetCheLiangLeiXing(x.NewValue);
                    }
                    if (x.AttributesName == "车辆种类")
                    {
                        x.OldValue = GetCheLiangZhongLei(x.OldValue);
                        x.NewValue = GetCheLiangZhongLei(x.NewValue);
                    }
                    if (x.AttributesName == "年审状态")
                    {
                        x.OldValue = GetNianShengZhuangTai(x.OldValue);
                        x.NewValue = GetNianShengZhuangTai(x.NewValue);
                    }
                    if (x.AttributesName == "备案状态")
                    {
                        x.OldValue = GetBeiAnZhuangTai(x.OldValue);
                        x.NewValue = GetBeiAnZhuangTai(x.NewValue);
                    }
                    if (x.AttributesName == "设备完整")
                    {
                        x.OldValue = GetBoolStr(x.OldValue);
                        x.NewValue = GetBoolStr(x.NewValue);
                    }
                    if (x.AttributesName == "数据接入")
                    {
                        x.OldValue = GetBoolStr(x.OldValue);
                        x.NewValue = GetBoolStr(x.NewValue);
                    }
                    if (x.AttributesName == "智能视频设备构成")
                    {
                        x.OldValue = GetSheBeiGouCheng(x.OldValue);
                        x.NewValue = GetSheBeiGouCheng(x.NewValue);
                    }
                    if (x.AttributesName == "视频头安装选择")
                    {
                        x.OldValue = GetSheXiangTouAnZhuangXuanZe(x.OldValue);
                        x.NewValue = GetSheXiangTouAnZhuangXuanZe(x.NewValue);
                    }
                    if (x.AttributesName == "终端数据通讯版本号")
                    {
                        x.OldValue = GetZhongDuanShuJuTongXunBanBenHao(x.OldValue);
                        x.NewValue = GetZhongDuanShuJuTongXunBanBenHao(x.NewValue);
                    }
                    DateTime dtDate;
                    if(DateTime.TryParse(x.OldValue,out dtDate))
                    {
                        x.OldValue = dtDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (DateTime.TryParse(x.NewValue, out dtDate))
                    {
                        x.NewValue = dtDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                });

                return dtoList;
            }
            catch (Exception ex)
            {
                LogHelper.Error("日志内容处理异常" + ex.Message + ex);
                return dtoList;
            }

        }

        #region 类型转换
        private string GetZhongDuanLeiXingName(string dto)
        {
            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "1":
                        nameStr = "部标";
                        break;
                    case "2":
                        nameStr = "部标+北斗";
                        break;
                    case "3":
                        nameStr = "DB44";
                        break;
                    case "4":
                        nameStr = "其他";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;
        }

        private string GetIntToStr(string dto)
        {

            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "0":
                        nameStr = "否";
                        break;
                    case "1":
                        nameStr = "是";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetChangShangLeiXing(string dto)
        {

            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "1":
                        nameStr = "紫光视频平台";
                        break;
                    case "2":
                        nameStr = "有为视频平台";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }
        private string GetSheBeiJiShenLeiXing(string dto)
        {

            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "1":
                        nameStr = "一体机";
                        break;
                    case "2":
                        nameStr = "分体机";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetCheShenYanSe(string dto)
        {

            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "1":
                        nameStr = "黄色";
                        break;
                    case "2":
                        nameStr = "黑色";
                        break;
                    case "3":
                        nameStr = "蓝色";
                        break;
                    case "4":
                        nameStr = "白色";
                        break;
                    case "5":
                        nameStr = "其它";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetRanLiaoName(string dto)
        {
            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "1":
                        nameStr = "柴油";
                        break;
                    case "2":
                        nameStr = "油气双燃料";
                        break;
                    case "3":
                        nameStr = "节能油电混合动力";
                        break;
                    case "4":
                        nameStr = "纯电动";
                        break;
                    case "5":
                        nameStr = "插电式混合动力";
                        break;
                    case "6":
                        nameStr = "氢燃料";
                        break;
                    case "7":
                        nameStr = "燃油";
                        break;
                    case "8":
                        nameStr = "其它";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetCheLiangLeiXing(string dto)
        {
            {
                string nameStr = "";
                try
                {

                    switch (dto)
                    {
                        case "1":
                            nameStr = "重型货车";
                            break;
                        case "2":
                            nameStr = "大型货车";
                            break;
                        case "3":
                            nameStr = "中型货车";
                            break;
                        case "4":
                            nameStr = "小型货车";
                            break;
                        case "5":
                            nameStr = "特大型客车";
                            break;
                        case "6":
                            nameStr = "大型客车";
                            break;
                        case "7":
                            nameStr = "中型客车";
                            break;
                        case "8":
                            nameStr = "小型客车";
                            break;
                        case "9":
                            nameStr = "特大型卧铺";
                            break;
                        case "10":
                            nameStr = "大型卧铺";
                            break;
                        case "11":
                            nameStr = "中型卧铺";
                            break;
                        case "12":
                            nameStr = "出租的士";
                            break;
                        case "13":
                            nameStr = "公交车";
                            break;
                        case "0":
                            nameStr = "其它车辆";
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return nameStr;

            }
        }
        private string GetCheLiangZhongLei(string dto)
        {
            {
                string nameStr = "";
                try
                {

                    switch (dto)
                    {
                        case "1":
                            nameStr = "客运班车";
                            break;
                        case "2":
                            nameStr = "旅游包车";
                            break;
                        case "3":
                            nameStr = "危险货运";
                            break;
                        case "4":
                            nameStr = "重型货车";
                            break;
                        case "5":
                            nameStr = "公交客运";
                            break;
                        case "6":
                            nameStr = "出租客运";
                            break;
                        case "7":
                            nameStr = "教练员车";
                            break;
                        case "8":
                            nameStr = "普通货运";
                            break;
                        case "9":
                            nameStr = "其它车辆";
                            break;
                        case "10":
                            nameStr = "校车";
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return nameStr;

            }
        }

        private string GetNianShengZhuangTai(string dto)
        {
            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "0":
                        nameStr = "未通过";
                        break;
                    case "1":
                        nameStr = "通过";
                        break;
                    case "2":
                        nameStr = "未审核";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetBeiAnZhuangTai(string dto)
        {
            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "0":
                        nameStr = "不通过备案";
                        break;
                    case "1":
                        nameStr = "通过备案";
                        break;
                    case "2":
                        nameStr = "未审核";
                        break;
                    case "3":
                        nameStr = "取消备案";
                        break;
                    case "4":
                        nameStr = "待提交";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetBoolStr(string dto)
        {
            string nameStr = "";
            try
            {

                switch (dto)
                {
                    case "False":
                        nameStr = "否";
                        break;
                    case "True":
                        nameStr = "是";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        private string GetSheBeiGouCheng(string dto)
        {
            string nameStr = "";
            try
            {
                List<string> gouChengList = dto.Split('|').ToList();

                foreach (var item in gouChengList)
                {
                    switch (item)
                    {
                        case "1":
                            nameStr += "GPS、";
                            break;
                        case "2":
                            nameStr += "ADAS、";
                            break;
                        case "3":
                            nameStr += "DSM、";
                            break;
                        case "4":
                            nameStr += "右侧盲区视频、";
                            break;
                        case "5":
                            nameStr += "显示屏、";
                            break;
                        case "6":
                            nameStr += "声光报警、";
                            break;
                        case "7":
                            nameStr += "500G以上存储装置、";
                            break;
                    }
                }
                nameStr = nameStr.TrimEnd('、');

            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }


        private string GetSheXiangTouAnZhuangXuanZe(string dto)
        {
            string nameStr = "";
            try
            {
                //得到每个视频头的配置
                List<string> gouChengList = dto.Split(',').ToList();

                foreach (var item in gouChengList)
                {
                    string[] configDetails = item.Split('|');
                    nameStr += "摄像头" + configDetails[0] + "|" + configDetails[1] + "|" + (configDetails[2] == "0" ? "无音频" : "有音频") + "、";
                }
                nameStr = nameStr.TrimEnd('、');
            }
            catch (Exception ex)
            {

            }
            return nameStr;

        }

        public string GetZhongDuanShuJuTongXunBanBenHao(string dto)
        {
            string nameStr = "";
            try
            {
                var banBenHao = 0;
                int.TryParse(dto, out banBenHao);
                return ((ZhongDuanShuJuTongXunBanBenHao)banBenHao).GetDescription();
            }
            catch (Exception ex)
            {

            }
            return nameStr;
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
