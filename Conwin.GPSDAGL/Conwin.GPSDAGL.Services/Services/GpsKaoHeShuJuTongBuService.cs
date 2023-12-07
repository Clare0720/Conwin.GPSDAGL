using AutoMapper;
using Conwin.EntityFramework;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Dtos;
using Conwin.GPSDAGL.Services.DtosExt.GpsKaoHeShuJuTongBu;
using Conwin.GPSDAGL.Services.DtosExt.QingYuanYZShuJUTongBu;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class GpsKaoHeShuJuTongBuService : ApiServiceBase, IGpsKaoHeShuJuTongBuService
    {
        private static string[] TableNames = "VehicleGpsDataQualifiedDetail,VehicleGpsDataDriftDetail,VehicleTrackDetail".Split(',');

        SqlHelper sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["GpsAssessmentDB"].ConnectionString);

        public GpsKaoHeShuJuTongBuService(IBussinessLogger bussinessLogger) : base(bussinessLogger)
        {
        }


        public ServiceResult<TongBuOutput> GetGpsKaoHeShuJu(GetGpsKaoHeShuJuInput input)
        {

            if (input.Type < 1 || input.Type > 3)
                return new ServiceResult<TongBuOutput>
                {
                    StatusCode = 2,
                    ErrorMessage = "提供的类型不合法"
                };

            try
            {


                var parameters = new SqlParameter[] {

                new SqlParameter("@StartTime", input.StartTime.ToString("yyyy-MM-dd 00:00:00.000")),
                new SqlParameter("@EndTime", input.StartTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000")),
                new SqlParameter("@StartNum", input.StartNum),
                new SqlParameter("@StartRow", (input.PageIndex - 1) * input.PageSize)
            };

                var data = sqlHelper.DataSet($@"
                select *  from [dbo].[T_{TableNames[input.Type - 1]}]  
                where id in (
                select top  {input.PageSize}  Id from (
                select  row_number() over(order by rownumber asc) as  pagerownumber,rownumber,Id  from ( 
                select  row_number() over(order by id asc) as rownumber,  Id from [dbo].[T_{TableNames[input.Type - 1]}] 
                nolock where XiaQuSheng='广东' and XiaQuShi='清远' and  SYS_ChuangJianShiJian>=@StartTime and SYS_ChuangJianShiJian < @EndTime 
                ) temp_row 
                where rownumber>= @StartNum  
                )temp_page where pagerownumber>@StartRow
                order by id asc
                )

                ",  parameters);

                parameters = new SqlParameter[] {

                new SqlParameter("@StartTime", input.StartTime.ToString("yyyy-MM-dd 00:00:00.000")),
                new SqlParameter("@EndTime", input.StartTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000")),
                new SqlParameter("@StartNum", input.StartNum),
                new SqlParameter("@StartRow", (input.PageIndex - 1) * input.PageSize)
            };

                var total = sqlHelper.ExecuteScalar($@"select  count(0)  from ( 
                    select  row_number() over(order by id asc) as rownumber from [dbo].[T_{TableNames[input.Type - 1]}]
                    nolock where XiaQuSheng='广东' and XiaQuShi='清远' and  SYS_ChuangJianShiJian>=@StartTime and SYS_ChuangJianShiJian < @EndTime 
                    ) temp_row
                    where rownumber >= @StartNum "
                , parameters);


                var result = new TongBuOutput
                {
                    Items = new List<object>(),
                    Total = 0
                };

                //switch (TableNames[input.Type-1])
                //{
                //    case "VehicleGpsDataQualifiedDetail":
                //        result.Items.Add(new GpsAssessmentDB_VehicleGpsDataDriftDetail());

                //        break;
                //    case "VehicleGpsDataDriftDetail":
                //        result.Items.Add(new GpsAssessmentDB_VehicleGpsDataQualifiedDetail());

                //        break;
                //    case "VehicleTrackDetail":
                //        result.Items.Add(new GpsAssessmentDB_VehicleTrackDetail());

                //        break;
                //    default:break;
                //}

                result.Items = data.Tables[0];
                result.Total = (int)total;




                return new ServiceResult<TongBuOutput>
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
                return new ServiceResult<TongBuOutput>
                {
                    StatusCode = 2
                };
            }

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

        public partial class GpsAssessmentDB_VehicleGpsDataDriftDetail
        {
            public System.Guid Id { get; set; }
            public string RegistrationNo { get; set; }
            public string RegistrationNoColor { get; set; }
            public string CheLiangZhongLei { get; set; }
            public string XiaQuSheng { get; set; }
            public string XiaQuShi { get; set; }
            public string XiaQuXian { get; set; }
            public string XiaQuZhen { get; set; }
            public string OperatorCode { get; set; }
            public string OperatorName { get; set; }
            public string YeHuDaiMa { get; set; }
            public string YeHuMingCheng { get; set; }
            public Nullable<int> DriftCount { get; set; }
            public Nullable<System.DateTime> RecordDate { get; set; }
            public string SYS_ChuangJianRenID { get; set; }
            public string SYS_ChuangJianRen { get; set; }
            public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }
            public string SYS_ZuiJinXiuGaiRenID { get; set; }
            public string SYS_ZuiJinXiuGaiRen { get; set; }
            public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }
            public string SYS_ShuJuLaiYuan { get; set; }
            public Nullable<int> SYS_XiTongZhuangTai { get; set; }
            public string SYS_XiTongBeiZhu { get; set; }
        }

        public partial class GpsAssessmentDB_VehicleGpsDataQualifiedDetail
        {
            public System.Guid Id { get; set; }
            public string RegistrationNo { get; set; }
            public string RegistrationNoColor { get; set; }
            public string CheLiangZhongLei { get; set; }
            public string XiaQuSheng { get; set; }
            public string XiaQuShi { get; set; }
            public string XiaQuXian { get; set; }
            public string XiaQuZhen { get; set; }
            public string OperatorCode { get; set; }
            public string OperatorName { get; set; }
            public string YeHuDaiMa { get; set; }
            public string YeHuMingCheng { get; set; }
            public Nullable<System.DateTime> RecordDate { get; set; }
            public Nullable<int> GpsTotalCount { get; set; }
            public Nullable<int> GpsErrorCount { get; set; }
            public Nullable<int> VehicleNoErrorCount { get; set; }
            public Nullable<int> VehicleNoColorErrorCount { get; set; }
            public Nullable<int> GpsTimeErrorCount { get; set; }
            public Nullable<int> LonLatErrorCount { get; set; }
            public Nullable<int> SpeedErrorCount { get; set; }
            public Nullable<int> DirectErrorCount { get; set; }
            public Nullable<int> AltitudeErrorCount { get; set; }
            public Nullable<int> StatusErrorCount { get; set; }
            public Nullable<int> AlarmErrorCount { get; set; }
            public Nullable<double> QualifiedRate { get; set; }
            public string SYS_ChuangJianRenID { get; set; }
            public string SYS_ChuangJianRen { get; set; }
            public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }
            public string SYS_ZuiJinXiuGaiRenID { get; set; }
            public string SYS_ZuiJinXiuGaiRen { get; set; }
            public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }
            public string SYS_ShuJuLaiYuan { get; set; }
            public Nullable<int> SYS_XiTongZhuangTai { get; set; }
            public string SYS_XiTongBeiZhu { get; set; }
        }

        public partial class GpsAssessmentDB_VehicleTrackDetail
        {
            public System.Guid Id { get; set; }
            public string RegistrationNo { get; set; }
            public string RegistrationNoColor { get; set; }
            public string CheLiangZhongLei { get; set; }
            public string XiaQuSheng { get; set; }
            public string XiaQuShi { get; set; }
            public string XiaQuXian { get; set; }
            public string XiaQuZhen { get; set; }
            public string OperatorCode { get; set; }
            public string OperatorName { get; set; }
            public string YeHuDaiMa { get; set; }
            public string YeHuMingCheng { get; set; }
            public Nullable<System.DateTime> StartTime { get; set; }
            public Nullable<System.DateTime> EndTime { get; set; }
            public Nullable<double> ValidMileage { get; set; }
            public Nullable<double> TotalMileage { get; set; }
            public Nullable<System.DateTime> RecordDate { get; set; }
            public string SYS_ChuangJianRenID { get; set; }
            public string SYS_ChuangJianRen { get; set; }
            public Nullable<System.DateTime> SYS_ChuangJianShiJian { get; set; }
            public string SYS_ZuiJinXiuGaiRenID { get; set; }
            public string SYS_ZuiJinXiuGaiRen { get; set; }
            public Nullable<System.DateTime> SYS_ZuiJinXiuGaiShiJian { get; set; }
            public string SYS_ShuJuLaiYuan { get; set; }
            public Nullable<int> SYS_XiTongZhuangTai { get; set; }
            public string SYS_XiTongBeiZhu { get; set; }
        }

    }
}
