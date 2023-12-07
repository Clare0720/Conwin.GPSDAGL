using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class DiSanFangSearchDto
    {
        public string JiGouMingCheng { get; set; }
        public int? JiGouLeiXing { get; set; }
        public string YouXiaoZhuangTai { get; set; }
        public string LianXiRen { get; set; }
        public string ShouJiHaoMa { get; set; }
    }

    public class DiSanFangExDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string JiGouMingCheng { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string JiGouJianCheng { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        public int? JiGouLeiXing { get; set; }

        /// <summary>
        /// 营业执照号
        /// </summary>
        public string YingYeZhiZhaoHao { get; set; }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string TongYiSheHuiXinYongDaiMa { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public string QiYeLeiXing { get; set; }

        /// <summary>
        /// 经营区域
        /// </summary>
        public string JingYingQuYu { get; set; }

        /// <summary>
        /// 有效状态
        /// </summary>
        public int? YouXiaoZhuangTai { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string YouBian { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string GongSiDiZhi { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string FuZheRen { get; set; }
        /// <summary>
        /// 负责人 电话
        /// </summary>
        public string FuZheRenDianHua { get; set; }

        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }

    }
}
