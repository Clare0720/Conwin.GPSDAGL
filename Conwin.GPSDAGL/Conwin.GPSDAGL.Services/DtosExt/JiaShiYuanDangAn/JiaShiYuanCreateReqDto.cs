using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案 创建驾驶员 请求数据
    /// </summary>
    public class JiaShiYuanCreateReqDto
    {
        /// <summary>
        /// 驾驶员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 从业资格证号
        /// </summary>
        public string Certification { get; set; }
        /// <summary>
        /// 性别：0=男；1=女；
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string GuoJi { get; set; }
        /// <summary>
        /// 户口地址
        /// </summary>
        public string HuKouDiZhi { get; set; }
        /// <summary>
        /// 生日（出生日期）
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 从业资格证有效期
        /// </summary>
        public DateTime? CertificationEndTime { get; set; }
        /// <summary>
        /// 发证机构
        /// </summary>
        public string FaZhengJiGou { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string LianXiDiZhi { get; set; }
        /// <summary>
        /// 身份证正面，文件ID
        /// </summary>
        public string ShenFenZhengZhengMianId { get; set; }
        /// <summary>
        /// 身份证反面，文件ID
        /// </summary>
        public string ShenFenZhengFanMianId { get; set; }
        /// <summary>
        /// 驾驶员正面，文件ID
        /// </summary>
        public string JiaShiYuanZhengMianId { get; set; }
        /// <summary>
        /// 驾照初次申领日期
        /// </summary>
        public DateTime? JiaZhaoChuCiShenLing { get; set; }
        /// <summary>
        /// 准驾车型
        /// </summary>
        public string ZhunJiaCheXing { get; set; }
        /// <summary>
        /// 驾照号码
        /// </summary>
        public string JiaZhaoHaoMa { get; set; }
        /// <summary>
        /// 驾照档案编号
        /// </summary>
        public string JiaZhaoBianHao { get; set; }
        /// <summary>
        /// 年检日期
        /// </summary>
        public DateTime? NianJianRiQi { get; set; }
        /// <summary>
        /// 驾照有效截至日期
        /// </summary>
        public DateTime? JiaZhaoYouXiaoQi { get; set; }
        /// <summary>
        /// 驾驶证正面，文件ID
        /// </summary>
        public string JiaShiZhengZhengMianId { get; set; }
        /// <summary>
        /// 驾驶证反面，文件ID
        /// </summary>
        public string JiaShiZhengFanMianId { get; set; }
    }
}
