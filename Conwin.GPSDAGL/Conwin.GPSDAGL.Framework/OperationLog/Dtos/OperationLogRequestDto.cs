using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    /// <summary>
    /// 添加操作日志对象
    /// </summary>
    public class OperationLogRequestDto
    {
        /// <summary>
        /// 系统名称（最多20个字符长度）
        /// </summary>
        public OperLogSystemName SystemName { get; set; }
        /// <summary>
        /// 模块名称（最多20个字符长度）
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 执行方法名称（最多30个字符长度）
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 业务操作类型（其他，新增，删除，更新，查询）
        /// </summary>
        public OperLogBizOperType BizOperType { get; set; }
        /// <summary>
        /// 简短描述（对当前操作行为的简短概括，最长200个字符）
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// 旧的业务数据（例如：更新前的旧数据，删除前的旧数据）
        /// </summary>
        public string OldBizContent { get; set; }
        /// <summary>
        /// 新的业务数据（例如：更新后的新数据，新增的数据）
        /// </summary>
        public string NewBizContent { get; set; }
        /// <summary>
        /// 业务操作执行耗时时长（单位：秒）
        /// </summary>
        public decimal? ExecuteDuration { get; set; }
        /// <summary>
        /// 操作人名称（不可空）
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作人唯一标识（可使用操作人ID之类的唯一键）
        /// </summary>
        public string OperatorID { get; set; }
        /// <summary>
        /// 操作者组织代码
        /// </summary>
        public string OperatorOrgCode { get; set; }
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
        /// 拓展信息(当前修改操作用于存储变更详情)
        /// </summary>
        public string ExtendInfo { get; set; }


        //初始化
        public OperationLogRequestDto() { }

        /// <summary>
        /// 业务操作日志
        /// </summary>
        public OperationLogRequestDto(string moduleName,
            string actionName,
            OperLogBizOperType bizOperType,
            string shortDescription,
            string oldBizContent,
            string newBizContent,
            string operatorName,
            string operatorID,
            string operatorOrgCode,
            string operatorOrgName,
            string extendInfo
            )
        {
            this.SystemName = OperLogSystemName.道路运输车辆风控服务平台;
            this.ModuleName = moduleName;
            this.ActionName = actionName;
            this.BizOperType = bizOperType;
            this.ShortDescription = shortDescription;
            this.OldBizContent = oldBizContent;
            this.NewBizContent = newBizContent;
            this.OperatorName = operatorName;
            this.OperatorID = operatorID;
            this.OperatorOrgCode = operatorOrgCode;
            this.OperatorOrgName = operatorOrgName;
            this.SysID = System.Configuration.ConfigurationManager.AppSettings["WEBAPISYSID"];
            this.AppCode = System.Configuration.ConfigurationManager.AppSettings["APPCODE"];
            this.ExtendInfo = extendInfo;
        }
    }

}
