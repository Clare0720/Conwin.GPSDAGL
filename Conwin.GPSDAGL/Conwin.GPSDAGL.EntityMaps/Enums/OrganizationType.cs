using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Entities.Enums
{
    /// <summary>
    /// 组织类型
    /// </summary>
    public enum OrganizationType
    {
        /// <summary>
        /// 平台运营商
        /// </summary>
        PingTaiYunYingShang = 0,

        /// <summary>
        /// 企业
        /// </summary>
        QiYe = 2,

        /// <summary>
        /// 平台代理商
        /// </summary>
        PingTaiDaiLiShang = 4,

        /// <summary>
        /// 分支机构中的分公司
        /// </summary>
        BenDiFuWuShang = 5,

        /// <summary>
        /// 车队
        /// </summary>
        CheDui = 7,

        /// <summary>
        /// 个体户
        /// </summary>
        GeTiHu = 8,

        /// <summary>
        /// 保险机构
        /// </summary>
        BaoXianJiGou = 9,

        /// <summary>
        /// 第三方监测中心
        /// </summary>
        DiSanFangJianCeZhongXin = 10,

        /// <summary>
        /// 政府
        /// </summary>
        ZhengFu = 11
    }

    public enum OrganizationNameType
    {
        /// <summary>
        /// PingTaiYunYingShang
        /// </summary>
        平台运营商 = 0,

        /// <summary>
        /// QiYe
        /// </summary>
        企业 = 2,

        /// <summary>
        /// PingTaiDaiLiShang
        /// </summary>
        平台代理商 = 4,

        /// <summary>
        /// 分支机构中的分公司BenDiFuWuShang
        /// </summary>
        本地服务商 = 5,

        /// <summary>
        /// CheDui
        /// </summary>
        车队 = 7,

        /// <summary>
        /// GeTiHu
        /// </summary>
        个体户 = 8,

        /// <summary>
        /// BaoXianJiGou
        /// </summary>
        保险机构 = 9,

        /// <summary>
        /// DiSanFangJianCeZhongXin 
        /// </summary>
        第三方监测中心 = 10,

        /// <summary>
        /// ZhengFu
        /// </summary>
        政府 = 11
    }
}
