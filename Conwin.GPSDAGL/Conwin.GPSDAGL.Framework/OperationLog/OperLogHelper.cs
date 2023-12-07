using AutoMapper;
using Conwin.Framework.CommunicationProtocol;
using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Framework.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    /// <summary>
    /// 操作日志帮助类
    /// </summary>
    public static class OperLogHelper
    {
        private static readonly string operLog_es_index = "operationlog";
        private static readonly string operLog_es_type = "log";

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool WriteOperLog(OperationLogRequestDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException("日志数据(dto)不能为空");
                }
                var esOperLog = new OperationLogToEsDto(dto);
                var esLogRes = Elasticsearch.ElasticSearchHelper.Add(operLog_es_index, operLog_es_type, Newtonsoft.Json.JsonConvert.SerializeObject(esOperLog));
                if (!esLogRes.Success)
                {
                    LogHelper.Error($"记录操作日志到ES失败：{esLogRes.ServerError?.Error?.Reason}【操作日志】{Newtonsoft.Json.JsonConvert.SerializeObject(dto)}【ES响应】{Newtonsoft.Json.JsonConvert.SerializeObject(esLogRes)}");
                }
                return esLogRes.Success;
            }
            catch (ArgumentNullException anEx)
            {
                LogHelper.Error($"记录操作日志失败：{anEx.Message}【操作日志】{Newtonsoft.Json.JsonConvert.SerializeObject(dto)}", anEx);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"记录操作日志异常。【操作日志】{Newtonsoft.Json.JsonConvert.SerializeObject(dto)}", ex);
                return false;
            }
        }


        /// <summary>
        /// 查询操作日志分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public static ServiceResult<OperLogQueryResult<OperationLogResponseDto>> QueryOperLog(OperLogQueryData<OperationLogQueryDto> queryDto)
        {
            var result = new ServiceResult<OperLogQueryResult<OperationLogResponseDto>>();
            try
            {
                if (queryDto == null)
                {
                    throw new ArgumentNullException("查询(queryDto)不能为空");
                }

                if (queryDto.page <= 0) { queryDto.page = 1; }
                if (queryDto.rows <= 0) { queryDto.rows = 10; }

                var q = new ESQueryBody();
                var f = new ESFilter();

                if (queryDto.data != null)
                {
                    if (queryDto.data.ID != null && queryDto.data.ID.Count > 0)
                    {
                        f.Must_Terms("ID.keyword", queryDto.data.ID);
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.SystemName))
                    {
                        f.Must_Wildcard("SystemName.keyword",$"*{queryDto.data.SystemName.Trim()}*");
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.ModuleName))
                    {
                        f.Must_Wildcard("ModuleName.keyword", $"*{queryDto.data.ModuleName.Trim()}*");
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.ChePaiHao))
                    {
                        f.Must_Wildcard("ShortDescription.keyword", $"*{queryDto.data.ChePaiHao.Trim()}*");
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.ActionName))
                    {
                        f.Must_Term("ActionName.keyword", queryDto.data.ActionName.Trim());
                    }
                    if (queryDto.data.BizOperType.HasValue)
                    {
                        f.Must_Term("BizOperType", queryDto.data.BizOperType.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.ShortDescription))
                    {
                        f.Must_Wildcard("ShortDescription.keyword", $"*{queryDto.data.ShortDescription.Trim()}*");
                    }
                    if (queryDto.data.ExecuteDurationMin.HasValue)
                    {
                        f.Must_Range("ExecuteDuration", ESQueryRange.GTE, queryDto.data.ExecuteDurationMin.Value);
                    }
                    if (queryDto.data.ExecuteDurationMax.HasValue)
                    {
                        f.Must_Range("ExecuteDuration", ESQueryRange.LTE, queryDto.data.ExecuteDurationMax.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.OperatorName))
                    {
                        f.Must_Wildcard("OperatorName.keyword", $"*{queryDto.data.OperatorName.Trim()}*");
                    }
                    if (queryDto.data.OperatorID!=null&& queryDto.data.OperatorID.Count>0)
                    {
                        f.Must_Terms("OperatorID.keyword", queryDto.data.OperatorID);
                    }
                    if (queryDto.data.OperatorOrgCode != null && queryDto.data.OperatorOrgCode.Count > 0)
                    {
                        f.Must_Terms("OperatorOrgCode.keyword", queryDto.data.OperatorOrgCode);
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.OperatorOrgName))
                    {
                        f.Must_Wildcard("OperatorOrgName.keyword", $"*{queryDto.data.OperatorOrgName.Trim()}*");
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.SysID))
                    {
                        f.Must_Term("SysID.keyword", queryDto.data.SysID.Trim());
                    }
                    if (!string.IsNullOrWhiteSpace(queryDto.data.AppCode))
                    {
                        f.Must_Term("AppCode.keyword", queryDto.data.AppCode.Trim());
                    }
                    if (queryDto.data.RecordTimeStart.HasValue)
                    {
                        f.Must_Range("RecordTime", ESQueryRange.GTE, queryDto.data.RecordTimeStart.Value);
                    }
                    if (queryDto.data.RecordTimeEnd.HasValue)
                    {
                        f.Must_Range("RecordTime", ESQueryRange.LTE, queryDto.data.RecordTimeEnd.Value);
                    }
                }
                q.SetQueryFilter(f);
                q.Set_SourceIncludes(new string[] { "ID", "SystemName", "ModuleName", "ActionName", "BizOperType", "BizOperTypeName", "ShortDescription", "OldBizContent", "NewBizContent", "ExecuteDuration", "OperatorName", "OperatorID", "OperatorOrgCode", "OperatorOrgName", "RecordTime", "SysID", "AppCode", "ExtendInfo" });
                q.AddSort("RecordTime", ESQuerySort.DESC);

                int fromIndex = (queryDto.page - 1) * queryDto.rows;
                q.SetFromSize(fromIndex, queryDto.rows);

                var json = q.GetQuery();

                var response = ElasticSearchHelper.SearchResult<OperationLogToEsDto>(operLog_es_index, operLog_es_type, json);
                if (response.Success)
                {
                    var resultData = new OperLogQueryResult<OperationLogResponseDto>()
                    {
                        totalcount = response.Total
                    };
                    if (response.Items != null && response.Items.Count() > 0)
                    {
                        Mapper.CreateMap<OperationLogToEsDto, OperationLogResponseDto>();
                        resultData.items = Mapper.Map<List<OperationLogResponseDto>>(response.Items.ToList());
                    }
                    result.StatusCode = 0;
                    result.Data = resultData;
                    return result;
                }
                else
                {
                    LogHelper.Error($"查询操作日志(ES)失败：{response.Messsage}【查询参数】{Newtonsoft.Json.JsonConvert.SerializeObject(queryDto)}【ES响应】{Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
                    result.StatusCode = 2;
                    result.ErrorMessage = $"查询失败：{response.Messsage}";
                    return result;
                }
            }
            catch (ArgumentNullException anEx)
            {
                result.StatusCode = 2;
                result.ErrorMessage = $"查询参数校验不通过：{anEx.Message}";
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"查询操作日志(ES)异常。【查询参数】{Newtonsoft.Json.JsonConvert.SerializeObject(queryDto)}", ex);
                result.StatusCode = 2;
                result.ErrorMessage = $"查询异常：{ex.Message}";
                return result;
            }
        }

    }
}
