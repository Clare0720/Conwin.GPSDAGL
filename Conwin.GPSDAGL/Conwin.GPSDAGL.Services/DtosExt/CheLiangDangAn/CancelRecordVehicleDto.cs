using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{

    public class QueryCancelRecordVehicleDto
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
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int? CheLiangZhongLei { get; set; }
        /// <summary>
        /// 服务商名称
        /// </summary>
        public string FuWuShangName { get; set; }
        /// <summary>
        /// 辖区市
        /// </summary>
        public string XiaQuShi { get; set; }
        /// <summary>
        /// 辖区县
        /// </summary>
        public string XiaQuXian { get; set; }
        /// <summary>
        /// 业户名称
        /// </summary>
        public string OrgName { get; set; }

        public Guid? Id { get; set; }
    }


    public class CancelRecordVehicleDto
    {
        public Guid Id { get; set; }
        public string ChePaiHao { get; set; }

        public string ChePaiYanSe { get; set; }

        public int? CheLiangZhongLei { get; set; }

        public string FuWuShangOrgCode { get; set; }

        public string FuWuShangOrgName { get; set; }

        public string XiaQuShi { get; set; }

        public string XiaQuXian { get; set; }

        public string OrgName { get; set; }

        public string OrgCode { get; set; }

        public DateTime? CancelRecordDateTime { get; set; }
        /// <summary>
        /// 取消备案原因
        /// </summary>
        public string CancellationReason { get; set; }
    }


    public class GpsAccessInformation
    {
        public string ErrorMsg { get; set; }
        public  bool IsSuccess { get; set; }
        public Guid FileId { get; set; }
    }


    public class GpsAuditInformation
    {
        public Dictionary<string, KeyValueItemValid> TempList { get; set; }
        public string ErrorMsg { get; set; }

        public int IsSuccess { get; set; }
    }

    /// <summary>
    /// 车辆审核结果
    /// </summary>
    public class VehicleFindingsModel
    {
       
        public bool Status { get; set; }
        /// <summary>
        /// 结果说明
        /// </summary>
        public string Explain { get; set; }
    }

    public class ThirdPartyVehicles
    {
        public string ChePaiHao  { get; set; }
        public string ChePaiYanSe { get; set; }
    }



    public class VehicleOperatingDays
    {
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }

        public int TotalOperationDays { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }

}
