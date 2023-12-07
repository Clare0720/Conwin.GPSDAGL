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
using Conwin.GPSDAGL.Services.DtosExt.GuangZhouYZShuJuTongBu;

namespace Conwin.GPSDAGL.Services.Services
{
    public class GuangZhouYZShuJuTongBuService : ApiServiceBase, IGuangZhouYZShuJuTongBuService
    {
        SqlHelper sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString);
        private static List<GuangZhouYZShuJuTongBuDto> vehicleList = null;

        public GuangZhouYZShuJuTongBuService(IBussinessLogger bussinessLogger)
            : base(bussinessLogger)
        {

        }

        public ServiceResult<QueryResult> GetYunZhengVehicleInfo(QueryData dto)
        {
            try
            {
                if (dto.page < 1) dto.page = 1;
                vehicleList = new List<GuangZhouYZShuJuTongBuDto>();
                QueryResult result = new QueryResult();
                using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString))
                {
                    string querySql = $@"SELECT * FROM T_GuangZhouYunZhengCheLiang";

                    //数据分页
                    string paginationSql = $"select top {dto.rows} * from (select row_number() over(ORDER BY vehicelList.ChePaiHao) as rownumber,*  FROM (" + querySql + $") AS vehicelList) temp_row where rownumber>{(dto.page - 1) * dto.rows} ORDER BY rownumber;";
                    //查询总记录数
                    string queryCount = $@"select count(0) from ({querySql} ) countT";
                    int count = conn.ExecuteScalar<int>(queryCount);
                    vehicleList = conn.Query<GuangZhouYZShuJuTongBuDto>(querySql).ToList();
                    result.totalcount = count;
                    result.items = vehicleList;
                }

                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询省运政车辆和业户信息出错{ex.Message}", ex);
                return new ServiceResult<QueryResult> { StatusCode = 2, ErrorMessage = ex.Message };
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





    }
}
