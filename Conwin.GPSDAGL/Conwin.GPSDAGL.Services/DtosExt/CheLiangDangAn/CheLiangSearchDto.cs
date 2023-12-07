using Conwin.GPSDAGL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangDangAn
{
    class CheLiangSearchDto
    {
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }

        public string YeHuMingCheng { get; set; }
    }

    class NotSubscribeCheLiangSearchDto
    {
	    public string ChePaiHao { get; set; }

	    public string ChePaiYanSe { get; set; }

	    public string YeHuMingCheng { get; set; }

        public IEnumerable<string> CheLiangZhongLei { get; set; }

        public IEnumerable<string> XiaQuXian { get; set; }
    }

    public class NotSubscribeCheLiangSearchResultDto
    {
        /// <summary>
        /// Id
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string ChePaiHao { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public string CheLiangLeiXing { get; set; }

        /// <summary>
        /// 车辆种类
        /// </summary>
        public string CheLiangZhongLei { get; set; }

        /// <summary>
        /// 车载电话
        /// </summary>
        public string CheZaiDianHua { get; set; }

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
        /// 业户代码
        /// </summary>
        public string YeHuDaiMa { get; set; }

        /// <summary>
        /// 业户名称
        /// </summary>
        public string YeHuMingCheng { get; set; }

        public string OperatorCode { get; set; }

        public string OperatorName { get; set; }
        
    }

    public class CheLiangSearchResponseDto
    {
        public Guid? Id { get; set; }
        public string ChePaiHao { get; set; }
        public string ChePaiYanSe { get; set; }
        public int? CheLiangLeiXing { get; set; }
        public int? CheLiangZhongLei { get; set; }
        public string CheJiaHao { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string QiYeMingCheng { get; set; }
        public string YunZhengZhuangTai { get; set; }
        public int? NianShenZhuangTai { get; set; }
        public int? BeiAnZhuangTai { get; set; }
        public string FuWuShangOrgCode { get; set; }
        public string FuWuShangName { get; set; }
        public DateTime? TiJiaoBeiAnShiJian { get; set; }
        public DateTime? ChuangJianShiJian { get; set; }
        public DateTime? BeiAnShenHeShiJian { get; set; }
        public string GpsZhongDuanMDT { get; set; }
        public string VideoZhongDuanMDT { get; set; }
        public string SIMKaHao { get; set; }
        public string VideoSheBeiXingHao { get; set; }
        public string VideoShengChanChangJia { get; set; }

        /// <summary>
        /// 数据通讯版本号
        /// </summary>
        public int? ShuJuTongXunBanBenHao { get; set; }

        /// <summary>
        /// 人工审核状态
        /// </summary>
        public int ManualApprovalStatus { get; set; }
        /// <summary>
        /// 是否选择第三方
        /// </summary>
        public int SelectThirdParty { get; set; }
        /// <summary>
        /// 最近Gps定位时间
        /// </summary>
        public DateTime? GpsTime { get; set; }
        /// <summary>
        /// 是否有定位信息
        /// </summary>
        public bool? IsHavGpsInfo { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public  string EnterpriseCode { get; set; }

        public string ThirdPartyState { get; set; }
        /// <summary>
        /// GPS审核结果
        /// </summary>
        public int GPSAuditStatus { get; set; }
        /// <summary>
        /// 业务资质结果
        /// </summary>
        public int? State { get; set; }
        /// <summary>
        /// 业务资质结果说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 车辆业务资质结果
        /// </summary>
        public int? BusinessHandlingResults { get; set; }

        /// <summary>
        /// 车辆智能视频附件
        /// </summary>
        public int? IsHavVideoAlarmAttachment { get; set; }
    }

}
