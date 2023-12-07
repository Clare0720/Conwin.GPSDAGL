using Conwin.Framework.BusinessLogger;
using Conwin.Framework.BusinessLogger.Dtos;
using Conwin.Framework.BusinessLogger.Impl;
using Conwin.Framework.ServiceAgent.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Conwin.GPSDAGL.Services
{
    public class BussinessLogHelper
    {
        private static readonly string APPCODE = ConfigurationManager.AppSettings["APPCODE"];
        private static readonly string SysId = ConfigurationManager.AppSettings["WEBAPISYSID"].ToString();
        private readonly IBussinessLogger _bussinessLogger;
        public BussinessLogHelper()
        {
            _bussinessLogger = new CustomBussinessLogger();//new BussinessLogger();
        }
        public void AddLog(BusinessLogDTO businessLogDTO,UserInfoDto userInfo)
        {

            businessLogDTO.YeWuBanLiDanWeiID = Guid.Parse(userInfo.OrganId);
            businessLogDTO.YeWuBanLiDanWei = userInfo.OrganizationName;
            businessLogDTO.YeWuBanLiRenID = new Guid(userInfo.Id);
            businessLogDTO.YeWuBanLiRen = userInfo.UserName;
            businessLogDTO.XiTongID = Guid.Parse(ConfigurationManager.AppSettings["WEBAPISYSID"]);
            //BianGengHouShuJuBanBen = "",
            //BianGengQianShuJuBanBen = "",
            //MoKuaiID = Guid.NewGuid(),
            //YeWuLiuChengID = Guid.NewGuid(),
            //BeiZhu = ""
            businessLogDTO.YeWuShouLiHao = Guid.NewGuid().ToString();
            businessLogDTO.YeWuBanLiShiJian = DateTime.Now;
            businessLogDTO.MoKuaiBianHao = APPCODE;//必填
            businessLogDTO.ShuJuZhuangTaiBiaoZhi = "正常";


            _bussinessLogger.LogAsync(businessLogDTO);
        }

    }


    public class CustomBussinessLogger : IBussinessLogger
    {
        public void BatchLogAsync(IEnumerable<Guid> businessObjectIds, BusinessLogDTO dto)
        {

        }

        public void LogAsync(BusinessLogDTO dto)
        {

        }
    }
}