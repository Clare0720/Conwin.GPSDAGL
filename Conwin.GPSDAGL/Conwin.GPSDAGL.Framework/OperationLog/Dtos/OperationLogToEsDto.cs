using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework.OperationLog
{
    /// <summary>
    /// 存入ES的操作日志对象
    /// </summary>
    public class OperationLogToEsDto
    {
        /// <summary>
        /// 唯一日志主键（只读，自动生成）
        /// </summary>
        public Guid ID { get; set; }
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
        public int BizOperType { get; set; }
        /// <summary>
        /// 业务操作类型名称（其他，新增，删除，更新）
        /// </summary>
        public string BizOperTypeName
        {
            get
            {
                OperLogBizOperType bizOperTypeEnum = (OperLogBizOperType)BizOperType;
                return EnumExtension.GetDesc(bizOperTypeEnum);
            }
        }
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
        /// 日志记录时间（只读，默认获取系统当前时间，格式：yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public DateTime RecordTime { get; set; }
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
        public OperationLogToEsDto()
        {
            this.ID = Guid.NewGuid();
            this.RecordTime = DateTime.Now;
        }

        public OperationLogToEsDto(OperationLogRequestDto reqDto)
        {
            this.ID = Guid.NewGuid();
            if (reqDto != null)
            {
                if (string.IsNullOrWhiteSpace(reqDto.ModuleName))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.ModuleName)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.ActionName))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.ActionName)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.ShortDescription))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.ShortDescription)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.OperatorOrgCode))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.OperatorOrgCode)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.OperatorOrgName))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.OperatorOrgName)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.SysID))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.SysID)} cannot be NULL OR Empty!");
                }
                if (string.IsNullOrWhiteSpace(reqDto.AppCode))
                {
                    throw new ArgumentNullException($"{nameof(reqDto.AppCode)} cannot be NULL OR Empty!");
                }

                //取值
                this.SystemName = reqDto.SystemName.ToString().Trim();
                this.ModuleName = reqDto.ModuleName.Trim();
                this.ActionName = reqDto.ActionName.Trim();
                this.BizOperType = (int)reqDto.BizOperType;
                this.ShortDescription = reqDto.ShortDescription.Trim();
                this.OldBizContent = reqDto.OldBizContent ?? "";
                this.NewBizContent = reqDto.NewBizContent ?? "";
                this.ExecuteDuration = reqDto.ExecuteDuration.HasValue ? reqDto.ExecuteDuration.Value : 0;
                this.OperatorName = reqDto.OperatorName ?? "";
                this.OperatorID = reqDto.OperatorID ?? "";
                this.OperatorOrgCode = reqDto.OperatorOrgCode.Trim();
                this.OperatorOrgName = reqDto.OperatorOrgName.Trim();
                this.SysID = reqDto.SysID.Trim();
                this.AppCode = reqDto.AppCode.Trim();
                this.ExtendInfo = reqDto.ExtendInfo?.Trim();


            }
            else
            {
                throw new ArgumentNullException("The OperationLogRequestDto object cannot be empty!");
            }
            this.RecordTime = DateTime.Now;
        }
    }


}
