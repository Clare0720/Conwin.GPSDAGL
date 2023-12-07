using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.AnQuanGuanLiRenYuan
{
    /// <summary>
    /// 列表查询响应
    /// </summary>
    public class QueryAnQuanGuanLiRenYuanDto
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 人员劳动合同
        /// </summary>
        public string LaborContractFileId { get; set; }
        /// <summary>
        /// 人员考核通过证明
        /// </summary>
        public string CertificatePassingExaminationFileId { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IDCardFrontId { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IDCardBackId { get; set; }
        /// <summary>
        /// 人员社保合同
        /// </summary>
        public string SocialSecurityContractFileId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ChuangJianShiJian { get; set; }

    }

    public class GetAnQuanGuanLiRenYuanListDto
    {
        public int Count { get; set; }

        public List<QueryAnQuanGuanLiRenYuanDto> list { get; set; }
    }

}
