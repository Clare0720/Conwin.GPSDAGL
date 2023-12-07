using Conwin.GPSDAGL.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    public class PingTaiDaiLiShangSearchDto
    {
        public string JiGouMingCheng { get; set; }
        public string YingYeZhiZhaoHao { get; set; }
        public string YouXiaoZhuangTai { get; set; }
        public string LianXiRen { get; set; }
        public string ShouJiHaoMa { get; set; }
        public string HeZuoFangShi { get; set; }
    }

    public class PingTaiDaiLiShangExDto
    {
        public string Id { get; set; }
		/// <summary>
        /// 机构名称
        /// </summary>
        public  string JiGouMingCheng { get; set; }

		/// <summary>
        /// 机构简称
        /// </summary>
        public string JiGouJianCheng { get; set; }
        
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
        ///  平台编号
        /// </summary>
        public string PingTaiBianHao { get; set; }

		/// <summary>
        /// 过检批次
        /// </summary>
        public string PingTaiGuoJianPiCi { get; set; }
  
		/// <summary>
        /// 公司地址
        /// </summary>
        public string GongSiDiZhi { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }

		/// <summary>
        /// 营业执照副本Id
        /// </summary>
        public string YingYeZhiZhaoFuBenId { get; set; }

        /// <summary>
        /// 法人代表身份证复印件 Id
        /// </summary>
        public string FaRenShenFenZhengFuYingJianId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string  FuZheRen { get; set; }
        /// <summary>
        /// 负责人 电话
        /// </summary>
        public string FuZheRenDianHua { get; set; }
    }

    public class SystemOrgInfoDto
    {
        public string SysId { get; set; }
        public object OrganizationName { get; set; }
        public int OrganizationType { get; set; }
        public object OrganizationCode { get; set; }
        public object SYS_ZuiJinXiuGaiRen { get; set; }
        public object SYS_ZuiJinXiuGaiRenID { get; set; }
    }

}
