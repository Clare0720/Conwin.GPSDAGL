using Conwin.EntityFramework.Extensions;
using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class SheBeiZhongDuanXinXiService : ApiServiceBase, ISheBeiZhongDuanXinXiService
    {
        private readonly ISheBeiZhongDuanXinXiRepository _sheBeiZhongDuanXinXiRepository;
        public SheBeiZhongDuanXinXiService(

            ISheBeiZhongDuanXinXiRepository sheBeiZhongDuanXinXiRepository,
             IBussinessLogger _bussinessLogger
            ) : base(_bussinessLogger)
        {
            _sheBeiZhongDuanXinXiRepository = sheBeiZhongDuanXinXiRepository;
        }


        public ServiceResult<QueryResult> QuerySheBeiZhongDuanList(QueryData dto)
        {
            try
            {
                SheBeiZhongDuanXinXiQueryDto search = JsonConvert.DeserializeObject<SheBeiZhongDuanXinXiQueryDto>(JsonConvert.SerializeObject(dto.data));

                Expression<Func<SheBeiZhongDuanXinXi, bool>> sbExp = x => x.SYS_XiTongZhuangTai == 0;


                if(!string.IsNullOrWhiteSpace(search.ShengChanChangJia))
                {
                    sbExp = sbExp.And(x => x.ShengChanChangJia.Contains(search.ShengChanChangJia.Trim()));
                }
                if(!string.IsNullOrWhiteSpace(search.SheBeiXingHao))
                {
                    sbExp = sbExp.And(x => x.SheBeiXingHao.Contains(search.SheBeiXingHao.Trim()));
                }

                var list = _sheBeiZhongDuanXinXiRepository.GetQuery(sbExp).Select(x => new SheBeiZhongDuanXinXiResponseDto
                {
                    SheBeiXingHao = x.SheBeiXingHao,
                    ShengChanChangJia=x.ShengChanChangJia
                });

                QueryResult result = new QueryResult();
                result.totalcount = list.Count();
                if(result.totalcount>0)
                {
                    result.items = list.OrderBy(x => x.ShengChanChangJia).Skip((dto.page - 1) * dto.rows).Take(dto.rows).ToList();
                }

                return new ServiceResult<QueryResult> { Data = result };
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取设备终端列表出错" + ex.Message, ex);
                return new ServiceResult<QueryResult> { ErrorMessage = "查询出错", StatusCode = 2 };
            }
        }


        public class SheBeiZhongDuanXinXiResponseDto
        {
            /// <summary>
            /// 生产厂家
            /// </summary>
            public string ShengChanChangJia { get; set; }
            /// <summary>
            /// 设备型号
            /// </summary>
            public string SheBeiXingHao { get; set; }
        }

        public class SheBeiZhongDuanXinXiQueryDto
        {
            /// <summary>
            /// 生产厂家
            /// </summary>
            public string ShengChanChangJia { get; set; }
            /// <summary>
            /// 设备型号
            /// </summary>
            public string SheBeiXingHao { get; set; }
        }

        public override void Dispose()
        {

        }
    }
}
