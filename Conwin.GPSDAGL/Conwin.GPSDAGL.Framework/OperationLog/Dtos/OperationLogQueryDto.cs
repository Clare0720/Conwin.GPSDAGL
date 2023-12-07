using Conwin.Framework.CommunicationProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    /// <summary>
    /// 查询操作日志分页列表
    /// </summary>
    public class OperationLogQueryDto
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public List<string> ID { get; set; }
        /// <summary>
        /// 系统名称（最多20个字符长度）
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 模块名称（最多20个字符长度）
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 执行方法名称（最多30个字符长度）
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 业务操作类型（其他，新增，删除，更新）
        /// </summary>
        public OperLogBizOperType? BizOperType { get; set; }
        /// <summary>
        /// 简短描述（对当前操作行为的简短概括，最长200个字符）
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// 业务操作执行耗时时长（区间最小值）（单位：秒）
        /// </summary>
        public decimal? ExecuteDurationMin { get; set; }
        /// <summary>
        /// 业务操作执行耗时时长（区间最大值）（单位：秒）
        /// </summary>
        public decimal? ExecuteDurationMax { get; set; }
        /// <summary>
        /// 操作人名称（不可空）
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作人唯一标识（可使用操作人ID之类的唯一键）
        /// </summary>
        public List<string> OperatorID { get; set; }
        /// <summary>
        /// 操作者组织代码
        /// </summary>
        public List<string> OperatorOrgCode { get; set; }
        /// <summary>
        /// 操作者组织名称
        /// </summary>
        public string OperatorOrgName { get; set; }
        /// <summary>
        /// 系统ID（例如：60190FC4-5103-4C76-94E4-12A54B62C92A 代表风控系统）
        /// </summary>
        public string SysID { get; set; }
        /// <summary>
        /// 应用编码（例如：0066002 代表基础档案）
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 日志记录时间（开始区间，格式：yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public DateTime? RecordTimeStart { get; set; }
        /// <summary>
        /// 日志记录时间（结束区间，格式：yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public DateTime? RecordTimeEnd { get; set; }
        /// <summary>
        /// 拓展信息(当前修改操作用于存储变更详情)
        /// </summary>
        public string ExtendInfo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
    }

    /// <summary>
    /// 操作日志分页列表 请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperLogQueryData<T> : QueryData
        where T : class
    {
        public new T data { get; set; }
    }

    /// <summary>
    /// 操作日志分页列表 响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperLogQueryResult<T> : QueryResult
        where T : class
    {
        public new List<T> items { get; set; }
    }
}
