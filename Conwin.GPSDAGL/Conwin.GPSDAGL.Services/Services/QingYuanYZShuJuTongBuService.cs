using AutoMapper;
using Conwin.EntityFramework;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities.QingYuanSync.CheLiang;
using Conwin.GPSDAGL.Entities.QingYuanSync.YeHu;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace Conwin.GPSDAGL.Services.Services
{
    public class QingYuanYZShuJuTongBuService : ApiServiceBase, IQingYuanYZShuJuTongBuService
    {
        SqlHelper sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString);

        public QingYuanYZShuJuTongBuService(IBussinessLogger bussinessLogger)
            : base(bussinessLogger)
        {

        }

        public override void Dispose()
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        public ServiceResult<object> GetNewYeHu(GetNewYeHuInput input)
        {
            try
            {

                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@StartTime", input.StartTime));
                parameters.Add(new SqlParameter("@EndTime", input.EndTime));


                var data = sqlHelper.DataSet($@"select * from V_YeHu where YeHuDaiMa in (
                    select top {input.PageSize} YeHuDaiMa  from(select  row_number() over(order by YeHuDianNaoBianHao asc) as rownumber, YeHuDaiMa from V_YeHu
                    where SYS_ZuiJinXiuGaiShiJian >= @StartTime and SYS_ZuiJinXiuGaiShiJian <= @EndTime
                    ) temp_row where rownumber > {(input.PageIndex - 1) * input.PageSize} order by YeHuDianNaoBianHao asc)", parameters.ToArray());

                //var yehuList =_yeHuRepository.GetQuery(yh => yh.SYS_ZuiJinXiuGaiShiJian >= input.QiShiZuiXinGengXinShiJian && yh.SYS_ZuiJinXiuGaiShiJian <= input.JieZhiZuiXinGengXinShiJian).OrderBy(yh=>yh.SYS_ZuiJinXiuGaiShiJian).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToList();

                return new ServiceResult<object>
                {
                    Data =
                    //new List<object>
                    //    {
                    //        new {
                    //            YeHuDianNaoBianHao= 164,
                    //            YeHuDaiMa= "E4000001",
                    //            YeHuJianCheng= "佛山市高明区汽车运输有限公司",
                    //            YeHuMingCheng= "佛山市高明区汽车运输有限公司",
                    //            JingYingXuKeZhengZi= "佛",
                    //            JingYingXuKeZhengHao= "440600000005",
                    //            JiGouDaiMa= "75109837-9",
                    //            ShenFenZhengHaoMa= "",
                    //            GongSiLeiXing= "本公司",
                    //            DiZhi= "广东省佛山市高明区高明大道东123号",
                    //            TongXinDiZhi= "",
                    //            YouBian= "528500",
                    //            JingYingFanWei= "市际班车客运，省际班车客运,市际包车客运;县际包车客运，县内包车客运;省际包车客运",
                    //            JingJiLeiXing= "股份制",
                    //            JiCengJiaoGuanBuMen= "荷城交管所",
                    //            KongGuGongSiDianNaoBianHao= -1,
                    //            FaDingDaiBiaoRen= "杜仕明",
                    //            LianXiBuMen= "",
                    //            LianXiRen= "谭健生",
                    //            LianXiDianHua= "0757-88219910",
                    //            YiDongDianHua= "13928538850",
                    //            DianZiXinXiang= "",
                    //            TouSuDianHua= "",
                    //            ChuanZhen= "",
                    //            GongSiFuZeRen= "杜仕明",
                    //            ZiZhiDengJi= "客运三级",
                    //            HuoYunZiZhiDengJi= "",
                    //            HuoYunQiYeLeiXing= "",
                    //            ZhuCeZiJin= "",
                    //            ZhengBenFaZhengRiQi= "2015-11-24T00:00:00",
                    //            ZhengBenYouXiaoQi= "2019-12-31T00:00:00",
                    //            FuBenFaZhengRiQi= "2015-11-24T00:00:00",
                    //            FuBenYouXiaoQi= "2019-12-31T00:00:00",
                    //            FaZhengJiGuan= "佛山市交通运输局",
                    //            KaiYeRiQi= "",
                    //            KaiYePiWenHao= "",
                    //            KaiYePiWenDanWei= "",
                    //            KaiYePiWenRiQi= "",
                    //            ZhuCeZiJinYuanBi= "",
                    //            WeiXianHuoWuPinMingCheng= "",
                    //            WeiXianHuoWuPinBianHaoJi= "",
                    //            QiTaJingYingFanWei= "出租客运，公共客运",
                    //            ShiFouSuoDing= "",
                    //            SuoDingYuanYin= "",
                    //            DangAnHao= "",
                    //            XiaQuShi= "佛山",
                    //            XiaQuXian= "高明",
                    //            XiaQuZhen= "荷城",
                    //            ZhuangTai= "营业",
                    //            JieSuoYuanYin= "",
                    //            WaiZiTouZiE= "",
                    //            NeiDiTouZiE= "",
                    //            FenGongSiHeSuanLeiXing= "",
                    //            KeYunZhiLiangXinYuKaoHeNianDu_keyun= "2014年",
                    //            KeYunZhiLiangXinYuKaoHeDengJi__keyun= "AAA级",
                    //            HuoYunZhiLiangXinYuKaoHeNianDu_huoyun= "",
                    //            HuoYunZhiLiangXinYuKaoHeDengJi_huoyun= "",
                    //            ZhuXiaoRiQi= "",
                    //            sys_xinzengren= "",
                    //            SYS_XinZengShiJian= "",
                    //            sys_zuijinxiugairen= "麦倩琪@佛山市交通运输局",
                    //            SYS_ZuiJinXiuGaiShiJian= "2015-11-24T09:11:17.82",
                    //            shujuzhuangtai= "正常",
                    //            OrgBaseInfoId= "ffdaa90b-29a0-45a6-9ad6-ab84ebf91f79",
                    //            CheLiangYeHuId= "9dec11fe-2812-4eb0-ac64-4d66a06d290c"
                    //          }
                    //    }


                    data.Tables[0],
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
                return new ServiceResult<object>
                {
                    StatusCode = 2,
                    ErrorMessage = ex.Message
                };
            }
        }

        public ServiceResult<object> GetQingYuanYZCheLiang(GetQingYuanYZCheLiangInput input)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", input.QiShiID));

                var data = sqlHelper.DataSet($"select top {input.PageSize} * from V_QingYuanYZCheLiang where ID >=@ID  order by id asc", parameters.ToArray());

                //var cheLiangList = _qingYuanYZCheLiangRepository.GetQuery(cl=>cl.ID>=input.QiShiID).OrderBy(cl=>cl.ID).Take(input.PageSize).ToList();

                return new ServiceResult<object>
                {
                    Data =
                    //new List<object> {
                    //        new {
                    //            RegistrationId= "e7729cc1-c87d-4be4-bca6-6d8112aaafdf",
                    //            FuWuShangOrgBaseId= "923b0c7f-4c54-4fb9-a845-4760ea1f8f05",
                    //            ShiPinTouGeShu= 0,
                    //            ShiPinTouAnZhuangXuanZe= "",
                    //            ShiPingChangShangLeiXing= "",
                    //            ShiFouAnZhuangShiPinZhongDuan= "",
                    //            ID= 310000,
                    //            CheLiangDianNaoBianHao= 0,
                    //            RegistrationNo= "粤ZQH7RX",
                    //            RegistrationNoColor= "渐变绿色",
                    //            CheLiangZhongLei= "客运班车",
                    //            IsYZ= false,
                    //            MDT= "",
                    //            M1= "",
                    //            IA1= "",
                    //            IC1= "",
                    //            Key= 0,
                    //            DeviceCode= "",
                    //            FactoryCode= "",
                    //            SimNo= "",
                    //            TerminalType= 0,
                    //            YeHuDaiMa= "",
                    //            YeHuMingCheng= "佛山市顺德区乐从供销集团液化石油气供应有限公司",
                    //            OperatorCode= "",
                    //            OperatorName= "",
                    //            DistrictID= 0,
                    //            XiaQuSheng= "广东",
                    //            XiaQuShi= "佛山",
                    //            XiaQuXian= "",
                    //            XiaQuZhen= "",
                    //            FirstUploadTime= "2019-05-27T17:46:42.21",
                    //            LatestLongtitude= 0.0,
                    //            LatestLatitude= 0.0,
                    //            LatestGpsTime= "2019-06-27T17:46:42.217",
                    //            LatestRecvTime= "2019-06-27T17:46:42.223",
                    //            HasRecvGps= true,
                    //            ZhuangTai= "正常",
                    //            FirstTmTime= "",
                    //            LatestTmModTime= "",
                    //            CheJiaHao= "",
                    //            DeviceModel= "",
                    //            SourceAddress= "",
                    //            IMEINo= "",
                    //            IMEIFactoryName= ""
                    //          }
                    //    }
                    data.Tables[0]
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
                return new ServiceResult<object>
                {
                    StatusCode = 2,
                    ErrorMessage = ex.Message
                };
            }
        }



        #region 新调整的接口，与旧接口暂时共存

        /// <summary>
        /// 获取业户数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ServiceResult<object> GetYeHuList(GetNewYeHuInput input)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@StartTime", input.StartTime));
                parameters.Add(new SqlParameter("@EndTime", input.EndTime));
                parameters.Add(new SqlParameter("@PageSize", input.PageSize));
                parameters.Add(new SqlParameter("@PageIndex", input.PageIndex));

                var data = sqlHelper.DataSet($@"select top (@PageSize) *  from(select  row_number() over(order by YeHuDianNaoBianHao asc) as rownumber, * from V_YeHu
                    where SYS_ZuiJinXiuGaiShiJian >= @StartTime and SYS_ZuiJinXiuGaiShiJian <= @EndTime
                    ) temp_row where rownumber > (@PageIndex - 1) * @PageSize order by YeHuDianNaoBianHao asc", parameters.ToArray());

                return new ServiceResult<object>
                {
                    Data = data.Tables[0],
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"数据同步接口-业户数据同步接口 【异常】", ex);
                return new ServiceResult<object>
                {
                    StatusCode = 2,
                    ErrorMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// 获取车辆数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ServiceResult<object> GetCheLiangList(GetNewCheLiangInput input)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@StartTime", input.StartTime));
                parameters.Add(new SqlParameter("@EndTime", input.EndTime));
                parameters.Add(new SqlParameter("@PageSize", input.PageSize));
                parameters.Add(new SqlParameter("@PageIndex", input.PageIndex));


                input.PageIndex -= 1;
                var cheliangList = sqlHelper.DataSet($@"exec P_TongBuCheLiangChaXun @StartTime,@EndTime, @PageIndex, @PageSize", parameters.ToArray());

                return new ServiceResult<object>
                {
                    Data = cheliangList.Tables[0]
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"数据同步接口-车辆数据同步接口 【异常】", ex);
                return new ServiceResult<object>
                {
                    StatusCode = 2,
                    ErrorMessage = ex.Message
                };
            }
        }


        #endregion



        #region 获取范围内的企业信息

        public ServiceResult<QueryResult> GetShengGpsYeHuList(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                QiYeInfoQueryDto queryParam = JsonConvert.DeserializeObject<QiYeInfoQueryDto>(JsonConvert.SerializeObject(dto.data));
                string whereSql = "";
                if(!string.IsNullOrWhiteSpace(queryParam?.OrgCode))
                {
                    whereSql += $" AND YeHuDaiMa='{queryParam.OrgCode.Trim()}'";
                }

                List<QiYeInfoResponseDto> qiYeList = new List<QiYeInfoResponseDto>();

                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    //查询两客一危一重业户
                    string sql = $@"SELECT 
                                    YeHuDianNaoBianHao,
                                    YeHuDaiMa,
                                    YeHuJianCheng,
                                    YeHuMingCheng,
                                    JingYingXuKeZhengZi,
                                    JingYingXuKeZhengHao,
                                    JiGouDaiMa,
                                    GongSiLeiXing,
                                    DiZhi,
                                    TongXinDiZhi,
                                    YouBian,
                                    JingYingFanWei,
                                    JingJiLeiXing,
                                    FaDingDaiBiaoRen,
                                    LianXiRen,
                                    LianXiDianHua,
                                    YiDongDianHua,
                                    ChuanZhen,
                                    GongSiFuZeRen,
                                    HuoYunQiYeLeiXing,
                                    XiaQuShi,
                                    XiaQuXian,
                                    XiaQuZhen,
                                    ZhuangTai,
                                    sys_xinzengren,
                                    SYS_XinZengShiJian,
                                    sys_zuijinxiugairen,
                                    SYS_ZuiJinXiuGaiShiJian,
                                    shujuzhuangtai
                                    FROM 
                                    GpsDB.dbo.YeHu WITH(NOLOCK)
                                    WHERE XiaQuShi = '清远' 
                                    AND ZhuangTai <> '注销' 
                                    AND ShuJuZhuangTai='正常' 
                                    AND YeHuDaiMa <> ''
                                    AND (
                                    JingYingFanWei LIKE '%班车客运%' 
                                    OR JingYingFanWei LIKE '%包车客运%'
                                    OR JingYingFanWei LIKE '%旅游客运%'
                                    OR JingYingFanWei LIKE '%危险货物运输%'
                                    OR JingYingFanWei LIKE '%普通货运%' 
                                    OR JingYingFanWei LIKE '%货物专用运输%'
                                    OR JingYingFanWei LIKE '%大型物件运输%'                                    
                                    ) {whereSql}";

                    //数据分页
                    string paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY YeHuXinXi.YeHuDaiMa) as rownumber,*  FROM (" + sql + $") AS YeHuXinXi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    string queryCount = $@"select count(0) from ({sql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount);
                    qiYeList = conn.Query<QiYeInfoResponseDto>(paginationSql).ToList();
                    result.totalcount = count;
                    result.items = qiYeList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省PGS企业异常" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        /// <summary>
        /// 清远企业注册功能查询企业用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceResult<QueryResult> QueryShengGpsYeHuList(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                QiYeInfoQueryDto queryParam = JsonConvert.DeserializeObject<QiYeInfoQueryDto>(JsonConvert.SerializeObject(dto.data));
                string whereSql = "";
                if (!string.IsNullOrWhiteSpace(queryParam?.OrgCode))
                {
                    whereSql += $" AND YeHuDaiMa='{queryParam.OrgCode.Trim()}'";
                }
                if (!string.IsNullOrWhiteSpace(queryParam?.OrgName))
                {
                    whereSql += $" AND YeHuMingCheng  LIKE '%{queryParam.OrgName.Trim()}%'";
                }
                if (!string.IsNullOrWhiteSpace(queryParam?.XiaQuXian))
                {
                    whereSql += $" AND XiaQuXian  = '{queryParam.XiaQuXian.Trim()}'";
                }

                List<QiYeInfoResponseDto> qiYeList = new List<QiYeInfoResponseDto>();
                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    //查询两客一危一重业户
                    string sql = $@"SELECT 
                                    YeHuDaiMa,
                                    YeHuJianCheng,
                                    YeHuMingCheng,
                                    JingYingXuKeZhengZi,
                                    JingYingXuKeZhengHao,
                                    JiGouDaiMa,
                                    GongSiLeiXing,
                                    DiZhi,
                                    TongXinDiZhi,
                                    YouBian,
                                    JingYingFanWei,
                                    JingJiLeiXing,
                                    FaDingDaiBiaoRen,
                                    LianXiRen,
                                    LianXiDianHua,
                                    YiDongDianHua,
                                    ChuanZhen,
                                    GongSiFuZeRen,
                                    HuoYunQiYeLeiXing,
                                    XiaQuShi,
                                    XiaQuXian
                                    FROM 
                                    GpsDB.dbo.YeHu WITH(NOLOCK)
                                    WHERE XiaQuShi = '清远' 
                                    AND ZhuangTai = '营业' 
                                    AND ShuJuZhuangTai='正常' 
                                    AND YeHuDaiMa <> ''{whereSql}";

                    //数据分页
                    string paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY YeHuXinXi.YeHuDaiMa) as rownumber,*  FROM (" + sql + $") AS YeHuXinXi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    string queryCount = $@"select count(0) from ({sql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount);
                    qiYeList = conn.Query<QiYeInfoResponseDto>(paginationSql).ToList();
                    result.totalcount = count;
                    result.items = qiYeList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省PGS企业异常" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        #endregion


        #region 查询指定车辆的信息(作废)

        public ServiceResult<QueryResult> GetShengGpsVehicleInfo(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                VehicleQueryDto searchInfo = JsonConvert.DeserializeObject<VehicleQueryDto>(JsonConvert.SerializeObject(dto.data));
                if (searchInfo == null)
                {
                    return new ServiceResult<QueryResult> { };
                };
                List<string> vehicleSearchList = new List<string>();
                if (searchInfo.VehicleList.Count() <= 0)
                {
                    return new ServiceResult<QueryResult> { };
                }
                var vehicleNoList = searchInfo.VehicleList.Where(x => !string.IsNullOrEmpty(x.RegistrationNo) && !string.IsNullOrEmpty(x.RegistrationNoColor)).Select(x => x.RegistrationNo.Trim() + x.RegistrationNoColor.Trim()).ToList();

                List<VeheiclQueryResponseDto> responseList = new List<VeheiclQueryResponseDto>();

                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    var param = new
                    {
                        vehicleNoList = vehicleNoList
                    };
                    //数据查询
                    string sql = @"SELECT RegistrationNo,RegistrationNoColor,ZhuangTai FROM GpsDB.dbo.VehicleInfo WITH(NOLOCK)
                    WHERE ZhuangTai = '营运'
                    AND CheLiangZhongLei IN('客运班车', '旅游包车', '危险货运', '重型货车')
                    AND RegistrationNo+RegistrationNoColor IN @vehicleNoList";
                    string queryCount = $@"select count(0) from ({sql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount, param);
                    //数据分页
                    string paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY cheliangxinxi.RegistrationNo DESC) as rownumber,*  FROM (" + sql + $") AS cheliangxinxi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    responseList = conn.Query<VeheiclQueryResponseDto>(paginationSql, param).ToList();
                    result.totalcount = count;
                    result.items = responseList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省GPS车辆信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }
        #endregion

        #region 查询指定营运车辆的信息(同步营运状态使用)
        public ServiceResult<QueryResult> GetShengGpsVehicleInfoNew(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                VehicleQueryDto searchInfo = JsonConvert.DeserializeObject<VehicleQueryDto>(JsonConvert.SerializeObject(dto.data));
                if (searchInfo == null)
                {
                    return new ServiceResult<QueryResult> { };
                };
                List<string> vehicleSearchList = new List<string>();
                if (searchInfo.VehicleList.Count() <= 0)
                {
                    return new ServiceResult<QueryResult> { };
                }
                var vehicleNoList = searchInfo.VehicleList.Where(x => !string.IsNullOrEmpty(x.RegistrationNo)).ToList();

                List<VeheiclQueryResponseDto> responseList = new List<VeheiclQueryResponseDto>();

                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    var param = new
                    {
                        vehicleNoList = vehicleNoList
                    };
                    //数据查询
                    string loadData = $@"CREATE TABLE #queryVehicleList
                                            (
                                            ChePaiHao NVARCHAR(50) NULL,
                                            ChePaiYanSe NVARCHAR(50) NULL
                                            ) ";

                    vehicleNoList.ForEach(x =>
                    {
                        loadData += $" INSERT #queryVehicleList VALUES('{x.RegistrationNo}','{x.RegistrationNoColor}') ";

                    });



                    string sql =$@" SELECT RegistrationNo,RegistrationNoColor,ZhuangTai FROM GpsDB.dbo.VehicleInfo a WITH(NOLOCK)
                                    join #queryVehicleList b on a.RegistrationNo=b.ChePaiHao and a.RegistrationNoColor=b.ChePaiYanSe
                                    WHERE a.ZhuangTai = '营运'
                                    AND a.CheLiangZhongLei IN('客运班车', '旅游包车', '危险货运', '重型货车')";
                    string queryCount = $@"{loadData}  select count(0) from ({sql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount, param);
                    //数据分页
                    string paginationSql = $"{loadData} select top {dto.rows} * from (select row_number() over(ORDER BY cheliangxinxi.RegistrationNo DESC) as rownumber,*  FROM (" + sql + $") AS cheliangxinxi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    responseList = conn.Query<VeheiclQueryResponseDto>(paginationSql, param).ToList();
                    result.totalcount = count;
                    result.items = responseList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省GPS车辆信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
            
        }
        #endregion



        #region 查询指定车辆的信息(同步车辆基本信息使用)

        public ServiceResult<QueryResult> GetShengGpsVehicleBaseInfo(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                VehicleQueryDto searchInfo = JsonConvert.DeserializeObject<VehicleQueryDto>(JsonConvert.SerializeObject(dto.data));
                if (searchInfo == null)
                {
                    return new ServiceResult<QueryResult> { };
                };
                List<string> vehicleSearchList = new List<string>();
                if (searchInfo.VehicleList.Count() <= 0)
                {
                    return new ServiceResult<QueryResult> { };
                }
                var vehicleNoList = searchInfo.VehicleList.Where(x => !string.IsNullOrEmpty(x.RegistrationNo)).ToList();

                List<VeheiclBaseInfoQueryResponseDto> responseList = new List<VeheiclBaseInfoQueryResponseDto>();

                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    var param = new
                    {
                        vehicleNoList = vehicleNoList
                    };
                    //数据查询
                    string loadData = $@"CREATE TABLE #queryVehicleList
                                            (
                                            ChePaiHao NVARCHAR(50) NULL,
                                            ChePaiYanSe NVARCHAR(50) NULL
                                            ) ";

                    vehicleNoList.ForEach(x =>
                    {
                        loadData += $" INSERT #queryVehicleList VALUES('{x.RegistrationNo}','{x.RegistrationNoColor}') ";

                    });

                    string sql = $@" SELECT *
                                        from
                                        (
                                          select  
                                          ROW_NUMBER() over(partition by RegistrationNo,RegistrationNoColor order by LatestRecvTime desc,ZhuangTai desc) as KeyId,--分区查询，同一辆车只取最新的数据
                                          RegistrationNo,
                                          RegistrationNoColor,
                                          YeHuDaiMa,
                                          YeHuMingCheng,
                                          ZhuangTai,
                                          LatestRecvTime,
                                          XiaQuXian
                                          FROM  GpsDB.dbo.VehicleInfo A with(nolock)
                                          join #queryVehicleList b on a.RegistrationNo=b.ChePaiHao and a.RegistrationNoColor=b.ChePaiYanSe
                                          WHERE  A.CheLiangZhongLei IN('客运班车', '旅游包车', '危险货运', '重型货车')
                                          AND A.XiaQuShi='清远'
                                          AND A.ZhuangTai<>'注销'
                                        ) d
                                         where KeyId=1";
                    string queryCount = $@"{loadData}  select count(0) from ({sql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount, param);
                    //数据分页
                    string paginationSql = $"{loadData} select top {dto.rows} * from (select row_number() over(ORDER BY cheliangxinxi.RegistrationNo DESC) as rownumber,*  FROM (" + sql + $") AS cheliangxinxi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    responseList = conn.Query<VeheiclBaseInfoQueryResponseDto>(paginationSql, param).ToList();
                    result.totalcount = count;
                    result.items = responseList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省GPS车辆基本信息出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }

        }


        #endregion
        public ServiceResult<QueryResult> GetVehicleInformation(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                
                var qiYeList = new List<VehicleInformation>();
                var result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["QinYuanGpsDb"].ConnectionString))
                {
                    //查询地市两客一危一重车辆
                    string sql = $@"SELECT 
                                   *
                                    FROM 
                                    GpsDB.dbo.VehicleInfo WITH(NOLOCK)";
                    //数据分页
                    string paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY YeHuXinXi.ID) as rownumber,*  FROM (" + sql + $") AS YeHuXinXi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    var queryCount = $@"select count(0) from ({sql} ) countT";
                    LogHelper.Info(paginationSql);
                    LogHelper.Info(queryCount);
                    var count = conn.ExecuteScalar<int>(queryCount);
                    qiYeList = conn.Query<VehicleInformation>(paginationSql).ToList();
                    result.totalcount = count;
                    LogHelper.Info(result.totalcount.ToString());
                    result.items = qiYeList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省车辆异常" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

        public ServiceResult<QueryResult> GetVehicleConfiguration(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> {StatusCode = 2, ErrorMessage = "请求参数错误"};
                }
                VehicleConfiguration queryParam = JsonConvert.DeserializeObject<VehicleConfiguration>(JsonConvert.SerializeObject(dto.data));
                string whereSql = "";
                if (!string.IsNullOrWhiteSpace(queryParam?.ChePaiHao))
                {
                    whereSql += $" AND ChePaiHao='{queryParam.ChePaiHao.Trim()}'";
                }
                if (!string.IsNullOrWhiteSpace(queryParam?.ChePaiYanSe))
                {
                    whereSql += $" AND ChePaiYanSe ='{queryParam.ChePaiYanSe.Trim()}'";
                }

                var result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                  
                    var sql = $@"SELECT  ChePaiHao,ChePaiYanSe,JingYingFanWei,
			   SYS_ZuiJinXiuGaiShiJian FROM    GpsDB.[dbo].[VehicleInfoEx]
WHERE  SYS_XiTongZhuangTai='正常'
AND  XiaQuShi='清远'   {whereSql}";
                    //数据分页
                    var paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY YeHuXinXi.SYS_ZuiJinXiuGaiShiJian desc) as rownumber,*  FROM (" + sql + $") AS YeHuXinXi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    var queryCount = $@"select count(0) from ({sql} ) countT";
                    var count = conn.ExecuteScalar<int>(queryCount);
                    var list = conn.Query<VehicleConfiguration>(paginationSql).ToList();
                    result.totalcount = count;
                    result.items = list;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询车辆异常" + ex.Message, ex);
                return new ServiceResult<QueryResult> {StatusCode = 2, ErrorMessage = "查询出错"};
            }
        }

        public ServiceResult<QueryResult> GetYeHuTwoGuestsList(QueryData dto)
        {
            try
            {
                if (dto == null)
                {
                    return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "请求参数错误" };
                }
                var result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TongBuGpsDb"].ConnectionString))
                {
                    //查询省gps两客业户
                    var sql = $@"SELECT 
                                    YeHuDianNaoBianHao,
                                    YeHuDaiMa,
                                    YeHuJianCheng,
                                    YeHuMingCheng,
                                    JingYingXuKeZhengZi,
                                    JingYingXuKeZhengHao,
                                    JiGouDaiMa,
                                    GongSiLeiXing,
                                    DiZhi,
                                    TongXinDiZhi,
                                    YouBian,
                                    JingYingFanWei,
                                    JingJiLeiXing,
                                    FaDingDaiBiaoRen,
                                    LianXiRen,
                                    LianXiDianHua,
                                    YiDongDianHua,
                                    ChuanZhen,
                                    GongSiFuZeRen,
                                    HuoYunQiYeLeiXing,
                                    XiaQuShi,
                                    XiaQuXian,
                                    XiaQuZhen,
                                    ZhuangTai,
                                    sys_xinzengren,
                                    SYS_XinZengShiJian,
                                    sys_zuijinxiugairen,
                                    SYS_ZuiJinXiuGaiShiJian,
                                    shujuzhuangtai
                                    FROM 
                                    GpsDB.dbo.YeHu WITH(NOLOCK)
                                    WHERE XiaQuShi = '清远'  
                                    AND ShuJuZhuangTai='正常' 
                                    AND YeHuDaiMa <> ''
                                    AND (
                                    JingYingFanWei LIKE '%班车客运%' 
                                    OR JingYingFanWei LIKE '%包车客运%' 
                                    OR JingYingFanWei LIKE '%旅游客运%'                                   
                                    )";

                    //数据分页
                    var paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY YeHuXinXi.YeHuDaiMa) as rownumber,*  FROM (" + sql + $") AS YeHuXinXi) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    var queryCount = $@"select count(0) from ({sql} ) countT";
                    var count = conn.ExecuteScalar<int>(queryCount);
                    var qiYeList = conn.Query<QiYeInfoResponseDto>(paginationSql).ToList();
                    result.totalcount = count;
                    result.items = qiYeList;
                }
                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询省两客GPS企业异常" + ex.Message, ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = "查询出错" };
            }
        }

    }
}
