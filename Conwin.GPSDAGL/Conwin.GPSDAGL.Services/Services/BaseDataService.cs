using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Services.DtosExt.BaseData;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class BaseDataService : ApiServiceBase, IBaseDataService
    {
        private static List<GetAreaInfoDto> DistrictsList = null;
        public BaseDataService(IBussinessLogger _bussinessLogger) : base(_bussinessLogger)
        {

        }

        public List<GetAreaInfoDto> GetDistrictsList(QueryDistrictsDto dto)
        {
            try
            {
                if (DistrictsList == null)
                {
                    using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["JCCSDb"].ConnectionString))
                    {
                        string paginationSql = @"SELECT a.ProvinceName,a.ProvinceID,b.CityName,b.CityID,c.DistrictName as [Key],c.DistrictID as Value FROM  
                                                    DC_JCCSDS.dbo.T_Province a
                                                    JOIN DC_JCCSDS.dbo.T_City b ON a.ProvinceID=b.ProvinceID
                                                    JOIN  DC_JCCSDS.dbo.T_District c ON b.CityID =c.CityID
                                                    WHERE 
                                                    a.SYS_XiTongZhuangTai=0
                                                    AND b.SYS_XiTongZhuangTai=0
                                                    AND c.SYS_XiTongZhuangTai=0";
                        DistrictsList = conn.Query<GetAreaInfoDto>(paginationSql).ToList();
                    }
                }

                var list = DistrictsList;
                if(!string.IsNullOrWhiteSpace(dto.ProvinceName))
                {
                    list = list.Where(x => x.ProvinceName == dto.ProvinceName.Trim()).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dto.CityName))
                {
                    list = list.Where(x => x.CityName == dto.CityName.Trim()).ToList();
                }
                return list;

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取辖区县信息出错" + ex.Message, ex);
                return new List<GetAreaInfoDto>();
            }
        }



        public override void Dispose()
        {

        }
    }
}
