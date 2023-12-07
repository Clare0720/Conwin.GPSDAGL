using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt
{
    /// <summary>
    /// 视频监控树 请求
    /// </summary>
    public class VideoJianKongShuQueryDto
    {
        public string OrgCode { get; set; }

        public int NodeType { get; set; }

        public string YeHuOrgCode { get; set; }

        public string XiaQuXian { get; set; }

        public int? CheLiangZhongLei { get; set; }

        public string YeHuMingCheng { get; set; }

        public int[] CheLiangZhongLeis { get; set; }
    }

    /// <summary>
    /// 视频监控树 组织列表
    /// </summary>
    public class VideoOrgItem
    {
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织类型
        /// </summary>
        public int? OrgType { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 车辆总数
        /// </summary>
        public int? OrgZongCheLiangShu { get; set; }
        /// <summary>
        /// 在线车辆总数
        /// </summary>
        public int? OrgZaiXianCheLiangShu { get; set; }
        /// <summary>
        /// 上级组织编码
        /// </summary>
        public string ParentOrgCode { get; set; }

        public string XiaQuXian { get; set; }
    }

    /// <summary>
    /// 视频监控树 车辆列表
    /// </summary>
    public class VideoCheLiangItem
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int? CheLiangZhongLei { get; set; }
        /// <summary>
        /// 车辆是否在线
        /// </summary>
        public bool? ShiFouZaiXian { get; set; }
        /// <summary>
        /// 视频头个数
        /// </summary>
        public int? ShiPinTouGeShu { get; set; }
        /// <summary>
        /// 视频头选择数据
        /// </summary>
        public string CameraSelected { get; set; }
        /// <summary>
        /// 视频服务商类型（厂商类型）
        /// </summary>
        public int? VideoServiceKind { get; set; }
    }
}
