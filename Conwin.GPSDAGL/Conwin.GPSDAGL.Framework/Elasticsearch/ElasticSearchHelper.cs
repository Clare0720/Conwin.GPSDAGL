using Conwin.Framework.CommunicationProtocol;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.Elasticsearch
{
    public class ElasticSearchHelper
    {

        public static Task<ElasticsearchResponse<string>> GetByIdAsync(string indexName, string typeName, string id)
        {
            return ESClient.LowInstance.GetAsync<string>(indexName, typeName, id);
        }
        public static T GetById<T>(string indexName, string typeName, string id) where T : class
        {
            ElasticsearchResponse<string> response = ESClient.LowInstance.Get<string>(indexName, typeName, id);
            return ResultObjMap<T>(response);
        }

        public static Task<ElasticsearchResponse<string>> AddAsync(string indexName, string typeName, string postData)
        {
            return ESClient.LowInstance.IndexAsync<string>(indexName, typeName, postData);
        }

        public static Task<ElasticsearchResponse<string>> AddAsync(string indexName, string typeName, string id, string postData)
        {
            return ESClient.LowInstance.IndexAsync<string>(indexName, typeName, id, postData);
        }

        public static ElasticsearchResponse<string> Add(string indexName, string typeName, string id, string postData)
        {
            return ESClient.LowInstance.Index<string>(indexName, typeName, id, postData);
        }

        public static ElasticsearchResponse<string> Add(string indexName, string typeName, string postData)
        {
            return ESClient.LowInstance.Index<string>(indexName, typeName, postData);
        }

        public static Task<ElasticsearchResponse<string>> SearchAsync(string indexName, string typeName, string postData)
        {
            return ESClient.LowInstance.SearchAsync<string>(indexName, typeName, postData);
        }

        public static async Task<EsQueryResult<T>> SearchResultAsync<T>(string indexName, string typeName, string postData)
        {
            ElasticsearchResponse<string> response = await ESClient.LowInstance.SearchAsync<string>(indexName, typeName, postData);
            return ResultMap<T>(response);
        }

        public static EsQueryResult<T> SearchResult<T>(string indexName, string typeName, string postData)
        {
            ElasticsearchResponse<string> response = ESClient.LowInstance.Search<string>(indexName, typeName, postData);
            return ResultMap<T>(response);
        }

        public static ESQueryResultBody SearchResult(string indexName, string typeName, string postData)
        {
            ESQueryResultBody body = null;
            ElasticsearchResponse<string> response = ESClient.LowInstance.Search<string>(indexName, typeName, postData);
            try
            {
                if(response.Success)
                {
                    body = JsonConvert.DeserializeObject<ESQueryResultBody>(response.Body);
                }
            }
            catch
            {

            }
            return body;
        }

        public static Task<ElasticsearchResponse<string>> UpdateByQueryAsync(string indexName, string typeName, string postData)
        {
            return ESClient.LowInstance.UpdateByQueryAsync<string>(indexName,typeName, postData);
        }

        public static Task<ElasticsearchResponse<string>> UpdateAsync(string indexName, string typeName, string id, string postData)
        {
            return ESClient.LowInstance.UpdateAsync<string>(indexName, typeName, id, postData, u => u.RetryOnConflict(3));
        }
        public static Task<ElasticsearchResponse<string>> IndexPutAsync(string indexName, string typeName, string id, string postData)
        {
            return ESClient.LowInstance.IndexPutAsync<string>(indexName, typeName, id, postData);
        }

        public static void InitScroll(string postData, TimeSpan scroll)
        {
            ElasticsearchResponse<string> response = ESClient.LowInstance.Search<string>(postData, s => s.Scroll(scroll));
        }

        public static void Scroll(string postData)
        {
            ESClient.LowInstance.Scroll<string>(postData);
        }
        

        //public static EsQueryResult ResultMap(ElasticsearchResponse<string> response)
        //{
        //    var body = JsonConvert.DeserializeObject<dynamic>(response.Body);
        //    var result = new EsQueryResult
        //    {
        //        Success = response.Success,
        //        Messsage = response.DebugInformation
        //    };
        //    if (result.Success)
        //    {
        //        result.Total = body.hits.total;
        //        result.Items = body.hits.hits;
        //    }
        //    return result;
        //}

        public static EsQueryResult<T> ResultMap<T>(ElasticsearchResponse<string> response)
        {
            var result = new EsQueryResult<T>
            {
                Success = response.Success,
                Messsage = response.DebugInformation
            };
            if (result.Success)
            {
                var body = JsonConvert.DeserializeObject<JObject>(response.Body);
                result.Total = body["hits"]["total"].Value<int>();
                result.Items = body["hits"]["hits"].Select(s => JsonConvert.DeserializeObject<T>(s["_source"].ToString()));
            }
            return result;
        }


        public static T ResultObjMap<T>(ElasticsearchResponse<string> response) where T : class
        {
            T resultbobj = default(T);
            if (response.Success)
            {
                var body = JsonConvert.DeserializeObject<JObject>(response.Body);
                resultbobj = JsonConvert.DeserializeObject<T>(body["_source"].ToString());
            }
            return resultbobj;
        }


        public static Task<ElasticsearchResponse<string>> CountAsync(string indexName, string typeName, string postData)
        {
            return ESClient.LowInstance.CountAsync<string>(indexName, typeName, postData);
        }



        #region 高级客户端

        public static ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            return ESClient.HighInstance.Search<T>(selector);
        }

        public static Task<ISearchResponse<T>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            return ESClient.HighInstance.SearchAsync<T>(selector);
        }

        /// <summary>
        /// 插入ES - 高级客户端实现
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="type">type</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static IIndexResponse Insert(string index, string type, object data)
        {
            return ESClient.HighInstance.Index(data, m => m.Index(index).Type(type));
        }

        public static Task<IIndexResponse> InsertAsync(string index, string type, object data)
        {
            var response = ESClient.HighInstance.IndexAsync(data, m => m.Index(index).Type(type));
            return response;
        }

        public static Task<IBulkResponse> BulkInsertAsync<T>(string index, string type, IEnumerable<object> dataList) where T : class
        {
            List<IBulkOperation> bulkOperations = new List<IBulkOperation>();
            foreach (var item in dataList)
            {
                bulkOperations.Add(new BulkIndexOperation<T>((T)item));
            }
            var bulkRequest = new BulkRequest(index, type)
            {
                Refresh = Refresh.True,
                Operations = bulkOperations
            };
            var response = ESClient.HighInstance.BulkAsync(bulkRequest);
            return response;
        }

        /// <summary>
        /// ES查询
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="func"></param>
        /// <param name="index">索引</param>
        /// <param name="type">类型</param>
        /// <param name="page">第几页</param>
        /// <param name="rows">取多少行</param>
        /// <returns></returns>
        public static Task<ISearchResponse<T>> SearchAsync<T>(IEnumerable<Func<QueryContainerDescriptor<T>, QueryContainer>> func, string index, string type, int page, int rows) where T : class
        {
            var result = ESClient.HighInstance.SearchAsync<T>(s => s.Index(index).Type(type).Query(q => q.Bool(
                    m => m.Must(func)
                    ))
                  .From((page - 1) * rows)
                  .Size(rows));

            return result;
        }

        public static Task<IUpdateResponse<object>> UpdateAsync(object data, string index, string type, string id, Refresh refreshOption = Refresh.False)
        {
            var result = ESClient.HighInstance.UpdateAsync<object, object>(id, m => m.Index(index).Type(type).Doc(data).Refresh(refreshOption));
            return result;
        }

        #endregion

        public static bool CheckPageAndRows(QueryData queryData,Action<string> FailedAction)
        {
            bool pass = true;
            string errMsg = null;
            if (queryData.page < 1)
            {
                errMsg = "查询异常 参数 page 不能小于 1 ";
                pass = false;
            }
            else if (queryData.rows < 1)
            {
                errMsg = "查询异常 参数 rows 不能小于 1 ";
                pass = false;
            }
            else if (queryData.rows > 10000)
            {
                errMsg = "查询异常 参数 rows 不能大于 10000";
                pass = false;
            }
            if(!pass)FailedAction(errMsg);
            return pass;
        }
    }

    public class ESClient
    {
        private static ElasticLowLevelClient lowInstance;
        private static ElasticClient highInstance;
        private static object lowLockObject = new Object();
        private static object highLockObject = new Object();
        private static readonly string address = ConfigurationManager.AppSettings["ElasticsearchAddress"];

        private ESClient() { }


        public static ElasticLowLevelClient LowInstance
        {
            get
            {
                if (lowInstance == null)
                {
                    lock (lowLockObject)
                    {
                        if (lowInstance == null)
                        {
                            var connectionPool = new SingleNodeConnectionPool(new Uri(address));
                            var settings = new ConnectionConfiguration(connectionPool).DisableDirectStreaming().PrettyJson();
                            lowInstance = new ElasticLowLevelClient(settings);
                        }
                    }
                }
                return lowInstance;
            }
        }

        public static ElasticClient HighInstance
        {
            get
            {
                if (highInstance == null)
                {
                    lock (highLockObject)
                    {
                        if (highInstance == null)
                        {
                            ConnectionSettings settings = new ConnectionSettings(new Uri(address)).DefaultIndex("eventinfo").DisableDirectStreaming().PrettyJson();
                            highInstance = new ElasticClient(settings);
                        }
                    }
                }
                return highInstance;
            }
        }
    }


    public class EsQueryResult<T>
    {
        public bool Success { get; set; }
        public int Total { get; set; }
        //public JArray Items { get; set; }
        public IEnumerable<T> Items { get; set; }
        public string Messsage { get; set; }
    }
}
