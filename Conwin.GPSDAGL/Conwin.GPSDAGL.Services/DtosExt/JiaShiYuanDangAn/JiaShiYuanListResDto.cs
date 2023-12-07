using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conwin.GPSDAGL.Entities.Enums;

namespace Conwin.GPSDAGL.Services.DtosExt.JiaShiYuanDangAn
{
    /// <summary>
    /// 驾驶员档案 驾驶员列表 响应数据
    /// </summary>
    public class JiaShiYuanListResDto
    {
        /// <summary>
        /// 驾驶员ID
        /// </summary>
        public Guid Id { get; set; }
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
        /// 工作状态<see cref="JiaShiYuanWorkStatus"/>：
        /// 0=待确认（无操作）；1=聘用（在职）；2=解聘（离职）；
        /// </summary>
        public int? WorkingStatus { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string WorkingStatusText
        {
            get
            {
                string statusText = "";
                if (WorkingStatus.HasValue)
                {
                    switch (WorkingStatus.Value)
                    {
                        case (int)JiaShiYuanWorkStatus.Hire:
                            statusText = "聘用";
                            break;
                        case (int)JiaShiYuanWorkStatus.Dismissal:
                            statusText = "解聘";
                            break;
                        default:
                            statusText = "待确认";
                            break;
                    }
                }
                else
                {
                    statusText = "待确认";
                }
                return statusText;
            }
        }
        /// <summary>
        /// 辖区省
        /// </summary>
        public string XiaQuSheng { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string ChePaiYanSe { get; set; }
        /// <summary>
        /// 聘用（入职）时间
        /// </summary>
        public DateTime? EntryDate { get; set; }
        /// <summary>
        /// 解聘（离职）时间
        /// </summary>
        public DateTime? DismissalDate { get; set; }
        /// <summary>
        /// 聘用/解聘时间
        /// </summary>
        public string WorkingDate
        {
            get
            {
                DateTime? workingTime = null;
                if (WorkingStatus.HasValue)
                {
                    switch (WorkingStatus.Value)
                    {
                        case (int)JiaShiYuanWorkStatus.Hire:
                            workingTime = EntryDate;
                            break;
                        case (int)JiaShiYuanWorkStatus.Dismissal:
                            workingTime = DismissalDate;
                            break;
                    }
                }
                return workingTime.HasValue ? workingTime.Value.ToString("yyyy-MM-dd") : "";
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ChuangJianShiJian { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string JingYingFanWei { get; set; }
    }
}
