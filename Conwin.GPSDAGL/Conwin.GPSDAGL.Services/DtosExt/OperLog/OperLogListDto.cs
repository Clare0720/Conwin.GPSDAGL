using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Framework.OperationLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.OperLog
{
    public class LogDetailsInfoDto
    {
        /// <summary>
        /// 创建者组织名称
        /// </summary>
        public string OperatorOrgName { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 日志记录时间（结束区间，格式：yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public DateTime? RecordTime { get; set; }
        /// <summary>
        /// 模块名称（最多20个字符长度）
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 业务操作类型（其他，新增，删除，更新）
        /// </summary>
        public OperLogBizOperType? BizOperType { get; set; }
        /// <summary>
        /// 简短描述（对当前操作行为的简短概括，最长200个字符）
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// 详情信息
        /// </summary>
        public List<LogUpdateValueDto> DetailsList { get; set; }
    }

}
