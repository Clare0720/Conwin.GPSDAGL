using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Entities.QingYuanSync.CheLiang
{
    public class CheLiangInfo
    {
       
        public Nullable<System.Guid> RegistrationId { get; set; }
        public Nullable<System.Guid> FuWuShangOrgBaseId { get; set; }
        public string ShiPinTouAnZhuangXuanZe { get; set; }
        public Nullable<int> ShiPingChangShangLeiXing { get; set; }
        public int ShiFouAnZhuangShiPinZhongDuan { get; set; }
        public long ID { get; set; }
        public Nullable<int> CheLiangDianNaoBianHao { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationNoColor { get; set; }
        public string CheLiangZhongLei { get; set; }
        public bool IsYZ { get; set; }
        public string MDT { get; set; }
        public Nullable<long> M1 { get; set; }
        public Nullable<long> IA1 { get; set; }
        public Nullable<long> IC1 { get; set; }
        public Nullable<long> Key { get; set; }
        public string DeviceCode { get; set; }
        public string FactoryCode { get; set; }
        public string SimNo { get; set; }
        public Nullable<int> TerminalType { get; set; }
        public string YeHuDaiMa { get; set; }
        public string YeHuMingCheng { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorName { get; set; }
        public long DistrictID { get; set; }
        public string XiaQuSheng { get; set; }
        public string XiaQuShi { get; set; }
        public string XiaQuXian { get; set; }
        public string XiaQuZhen { get; set; }
        public Nullable<System.DateTime> FirstUploadTime { get; set; }
        public Nullable<double> LatestLongtitude { get; set; }
        public Nullable<double> LatestLatitude { get; set; }
        public Nullable<System.DateTime> LatestGpsTime { get; set; }
        public Nullable<System.DateTime> LatestRecvTime { get; set; }
        public Nullable<bool> HasRecvGps { get; set; }
        public string ZhuangTai { get; set; }
        public Nullable<System.DateTime> FirstTmTime { get; set; }
        public Nullable<System.DateTime> LatestTmModTime { get; set; }
        public string CheJiaHao { get; set; }
        public string DeviceModel { get; set; }
        public string SourceAddress { get; set; }
        public string IMEINo { get; set; }
        public string IMEIFactoryName { get; set; }
        public int? ShiPinTouGeShu { get; set; }


        
    }
}
