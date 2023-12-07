using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案列表搜索
    /// </summary>
    public class JiaShiYuanSearchDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string XingMing { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string QiYeMingCheng { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LianXiDianHua { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZhengJianHaoMa { get; set; }
        /// <summary>
        /// 状态：1=待确认；2=在职；3=离职；
        /// </summary>
        public int? ZhuangTai { get; set; }
    }
}
