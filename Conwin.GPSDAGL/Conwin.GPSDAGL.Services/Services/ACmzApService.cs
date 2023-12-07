using Conwin.Framework.CommunicationProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Services.DtosExt;
using Conwin.GPSDAGL.Services.DtosExt.ACmzAp;
using Conwin.GPSDAGL.Services.Services.Interfaces;

namespace Conwin.GPSDAGL.Services
{
    public class ACmzApService : IACmzApService
    {


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
                var queryResult = new QueryResult();

                //QueryACmzApiDto resultData = GetAnZhuangZhuangZhongDuanXinXi(queryData, userInfo);

                //var queryResult = new QueryResult()
                //{
                //    totalcount = resultData.Count,
                //    items = resultData.list
                //};

                result.StatusCode = 0;
                result.Data = queryResult;
                return result;
            }
            catch (Exception e)
            {


                result.StatusCode = 2;
                result.ErrorMessage = "用户列表获取失败";
                return result;
            }

        }



    }
}
