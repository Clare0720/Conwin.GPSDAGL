using Conwin.Framework.BusinessLogger;
using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Services.DtosExt.GPSCheLiangJianKong;
using Conwin.GPSDAGL.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    /// <summary>
    /// 企业GPS车辆监控依赖服务
    /// </summary>
    public partial class GPSCheLiangJianKongService : ApiServiceBase, IGPSCheLiangJianKongService
    {

        #region 仓储、构造函数、Dispose

        /// <summary>
        /// 车辆信息仓储
        /// </summary>
        private readonly ICheLiangRepository _cheLiangXinXiRepository;


        public GPSCheLiangJianKongService(ICheLiangRepository cheLiangXinXiRepository,
        IBussinessLogger _bussinessLogger) : base(_bussinessLogger)
        {
            _cheLiangXinXiRepository = cheLiangXinXiRepository;
        }

        public override void Dispose()
        {
            _cheLiangXinXiRepository.Dispose();
        }

        #endregion

        public ServiceResult<QueryResult> QueryVehicleByUser(CheLiangQiYeShiPingQueryDto queryData)
        {
            return ExecuteCommandClass<QueryResult>(() =>
            {
                var result = new ServiceResult<QueryResult>();
                if (queryData.GovermentFlag)
                {
                    result = QueryTypeIsGoverment(queryData);
                }
                else
                {
                    if (queryData.OrganizationType == 9 || queryData.OrganizationType == 10)
                    {
                        result = QueryTypeIsThirdOrgan(queryData);
                    }
                    else if (queryData.OrganizationType == 11 || queryData.OrganizationType == 12)
                    {
                        result = QueryTypeIsQuXian(queryData);
                    }
                    else
                    {
                        result = QueryTypeIsEnterprise(queryData);
                    }
                }
                return result;
            });
        }

        public ServiceResult<QueryResult> QueryTypeIsGoverment(CheLiangQiYeShiPingQueryDto queryData)
        {
            var result = new ServiceResult<QueryResult>();
            string condition = "";
            if (!String.IsNullOrEmpty(queryData.Province))
            {
                condition += string.Format(" AND XiaQuSheng = '{0}'", queryData.Province);
            }
            if (!String.IsNullOrEmpty(queryData.City))
            {
                condition += string.Format(" AND XiaQuShi = '{0}'", queryData.City);
            }

            if (!String.IsNullOrEmpty(queryData.QiYeMingCheng))
            {
                condition += string.Format(" AND T_CheLiangYeHuXinXi.QiYeMingCheng like '%{0}%'", queryData.QiYeMingCheng);
            }

            var sql = string.Format(@"
SELECT CAST
	( T_CheLiangXinXi.ID AS nvarchar ( 50 ) ) AS ID,
	T_CheLiangXinXi.ChePaiHao,
	T_CheLiangXinXi.ChePaiYanSe,
	T_CheLiangXinXi.CheLiangZhongLei,
	XiaQuSheng,
	XiaQuShi,
	T_CheLiangYeHuXinXi.QiYeMingCheng,
	T_CLZDPEXI.ShiPingTouGeShu,
	T_CLZDPEXI.CameraSelected,
	T_CLZDPEXI.VideoServiceKind 
FROM
	T_CheLiangXinXi
	LEFT JOIN T_CheLiangYeHuXinXi ON T_CheLiangYeHuXinXi.CheLiangID = T_CheLiangXinXi.ID
	LEFT JOIN ( SELECT CheLiangId, ShiPingTouGeShu, CameraSelected, VideoServiceKind FROM T_CheLiangZhongDuanPeiZhiXinXi ) AS T_CLZDPEXI ON T_CLZDPEXI.CheLiangId = T_CheLiangXinXi.ID
WHERE 1=1 ") + condition;
            var query = _cheLiangXinXiRepository.ExecSqlQuery<CheLiangQiYeShiPingDto>(sql).ToList();
            List<CheLiangQiYeShiPingDto> datalist = new List<CheLiangQiYeShiPingDto>();
            List<IGrouping<string, CheLiangQiYeShiPingDto>> grouplist = new List<IGrouping<string, CheLiangQiYeShiPingDto>>();
            foreach (IGrouping<string, CheLiangQiYeShiPingDto> group in query.GroupBy(x => x.XiaQuShi))
            {
                List<CheLiangQiYeShiPingDto> xiaQuShiList = group.ToList<CheLiangQiYeShiPingDto>();
                foreach (IGrouping<string, CheLiangQiYeShiPingDto> qiyegroup in xiaQuShiList.GroupBy(x => x.QiYeMingCheng))
                {
                    grouplist.Add(qiyegroup);
                    List<CheLiangQiYeShiPingDto> qiYeList = qiyegroup.ToList<CheLiangQiYeShiPingDto>();
                    datalist.AddRange(qiYeList);
                }
            }
            result.Data = new QueryResult();
            result.Data.totalcount = grouplist.Count;
            result.Data.items = grouplist;
            return result;
        }
        public ServiceResult<QueryResult> QueryTypeIsEnterprise(CheLiangQiYeShiPingQueryDto queryData)
        {
            var result = new ServiceResult<QueryResult>();
            string condition = "";
            if (String.IsNullOrEmpty(queryData.OrgCode))
            {
                result.StatusCode = 2;
                result.ErrorMessage = "参数OrgCode不允许为空";
                return result;
            }
            condition += string.Format(" AND ParentOrgCode = '{0}'", queryData.OrgCode);

            if (queryData.OrganizationType == 2 || queryData.OrganizationType == 7)
            {
                condition = string.Format(" AND OrgCode = '{0}'", queryData.OrgCode);
            }

            if (queryData.OrganizationType != 7)//车队
            {
                condition += " AND T_CheLiangYeHuXinXi.QiYeMingCheng is not null AND T_CheLiangYeHuXinXi.QiYeMingCheng <> '' ";
            }

            if (!String.IsNullOrEmpty(queryData.QiYeMingCheng))
            {
                condition += string.Format(" AND T_CheLiangYeHuXinXi.QiYeMingCheng like '%{0}%'", queryData.QiYeMingCheng);
            }

            if (IsGuanLiYuanRoleCode(queryData.RoleCode))
            {
                condition += " AND T_RYFPJ.SysUserId is null ";
            }
            else
            {
                var sysuserid = queryData.SysUserId;
                if (!string.IsNullOrWhiteSpace(sysuserid))
                {
                    condition += $" AND T_RYFPJ.SysUserId ='{sysuserid}' ";
                }
                else
                {
                    //兼容操作
                    condition += " AND T_RYFPJ.SysUserId is null ";
                }
            }


            var sql = string.Format(@"
SELECT
	T_RYFPJ.ChePaiHao,
	T_RYFPJ.ChePaiYanSe,
	T_RYFPJ.CheLiangZhongLei,
	T_CheLiangYeHuXinXi.QiYeMingCheng,
	T_RYFPJ.ParentOrgName,
	CLZDPZXI.ShiPingTouGeShu,
	CLZDPZXI.CameraSelected,
	CLZDPZXI.VideoServiceKind 
FROM
	T_CheLiangYeHuXinXi
	LEFT JOIN T_RYFPJKCLSSOrgJiLu AS T_RYFPJ ON T_CheLiangYeHuXinXi.CheLiangID = T_RYFPJ.CheLiangId
	LEFT JOIN T_CheLiangZhongDuanPeiZhiXinXi AS CLZDPZXI ON T_CheLiangYeHuXinXi.CheLiangID = CLZDPZXI.CheLiangId 
WHERE
	T_CheLiangYeHuXinXi.SYS_XiTongZhuangTai = 0 
	AND T_RYFPJ.SYS_XiTongZhuangTai = 0 
	AND CLZDPZXI.SYS_XiTongZhuangTai = 0 
	AND CLZDPZXI.SYS_ChuangJianShiJian = ( SELECT MAX ( CLZDPZXIb.SYS_ChuangJianShiJian ) FROM T_CheLiangZhongDuanPeiZhiXinXi AS CLZDPZXIb WHERE CLZDPZXI.CheLiangID = CLZDPZXIb.CheLiangID ) ");
            var groupsql = string.Format($@" 
GROUP BY
	CLZDPZXI.CheLiangID,
	CLZDPZXI.SYS_ChuangJianShiJian ,
	T_RYFPJ.ChePaiHao,
	T_RYFPJ.ChePaiYanSe,
	T_RYFPJ.CheLiangZhongLei,
	T_CheLiangYeHuXinXi.QiYeMingCheng,
	T_RYFPJ.ParentOrgName,
	CLZDPZXI.ShiPingTouGeShu,
	CLZDPZXI.CameraSelected,
	CLZDPZXI.VideoServiceKind 
ORDER BY
	CLZDPZXI.SYS_ChuangJianShiJian DESC");

            var query = _cheLiangXinXiRepository.ExecSqlQuery<CheLiangQiYeShiPingDto>(sql + condition + groupsql).ToList();

            List<IGrouping<string, CheLiangQiYeShiPingDto>> grouplist = new List<IGrouping<string, CheLiangQiYeShiPingDto>>();
            foreach (IGrouping<string, CheLiangQiYeShiPingDto> qiyegroup in query.GroupBy(x => x.QiYeMingCheng))
            {
                grouplist.Add(qiyegroup);
                //List<CheLiangQiYeShiPingDto> qiYeList = qiyegroup.ToList<CheLiangQiYeShiPingDto>();
            }
            result.Data = new QueryResult();
            result.Data.totalcount = grouplist.Count;
            result.Data.items = grouplist;
            return result;
        }

        public ServiceResult<QueryResult> QueryTypeIsThirdOrgan(CheLiangQiYeShiPingQueryDto queryData)
        {
            var result = new ServiceResult<QueryResult>();
            string condition = "";
            if (!String.IsNullOrEmpty(queryData.OrgCode))
            {
                condition += String.Format(" And OrgCode = '{0}' ", queryData.OrgCode);
            }
            else
            {
                result.StatusCode = 2;
                result.ErrorMessage = "OrgCode为不能空!";
                return result;
            }

            if (!String.IsNullOrEmpty(queryData.QiYeMingCheng))
            {
                condition += string.Format(" AND T_CheLiangYeHuXinXi.QiYeMingCheng like '%{0}%'", queryData.QiYeMingCheng);
            }

            var sql = string.Format(@"
SELECT DISTINCT
	T_CheLiangXinXi.ChePaiHao,
	T_CheLiangXinXi.ChePaiYanSe,
	T_CheLiangXinXi.CheLiangZhongLei,
	T_CheLiangYeHuXinXi.QiYeMingCheng,
	T_CheLiangYeHuXinXi.GeRenCheZhuMingCheng,
	ShiPingTouGeShu,
	CameraSelected,
	VideoServiceKind,
	T_CheLiangDiSanFangJiGouXinXi.OrgCode 
FROM
	T_CheLiangDiSanFangJiGouXinXi
	LEFT JOIN T_CheLiangXinXi ON T_CheLiangXinXi.ID = T_CheLiangDiSanFangJiGouXinXi.CheLiangID
	LEFT JOIN T_CheLiangZhongDuanPeiZhiXinXi ON T_CheLiangDiSanFangJiGouXinXi.CheLiangID = T_CheLiangZhongDuanPeiZhiXinXi.CheLiangID
	LEFT JOIN T_CheLiangYeHuXinXi ON T_CheLiangDiSanFangJiGouXinXi.CheLiangID = T_CheLiangYeHuXinXi.CheLiangID 
WHERE
	T_CheLiangXinXi.SYS_XiTongZhuangTai = 0 
	AND T_CheLiangZhongDuanPeiZhiXinXi.SYS_XiTongZhuangTai = 0 
	AND T_CheLiangDiSanFangJiGouXinXi.SYS_XiTongZhuangTai = 0 
	AND T_CheLiangYeHuXinXi.SYS_XiTongZhuangTai = 0 {0}", condition);
            var query = _cheLiangXinXiRepository.ExecSqlQuery<CheLiangQiYeShiPingDto>(sql).ToList();
            foreach (var item in query)
            {
                if (!String.IsNullOrEmpty(item.GeRenCheZhuMingCheng) && String.IsNullOrEmpty(item.QiYeMingCheng))
                {
                    item.QiYeMingCheng = item.GeRenCheZhuMingCheng;
                }
            }
            List<IGrouping<string, CheLiangQiYeShiPingDto>> grouplist = new List<IGrouping<string, CheLiangQiYeShiPingDto>>();
            foreach (IGrouping<string, CheLiangQiYeShiPingDto> qiyegroup in query.GroupBy(x => x.QiYeMingCheng))
            {
                grouplist.Add(qiyegroup);
                List<CheLiangQiYeShiPingDto> qiYeList = qiyegroup.ToList<CheLiangQiYeShiPingDto>();
            }
            result.Data = new QueryResult();
            result.Data.totalcount = grouplist.Count;
            result.Data.items = grouplist;
            return result;
        }

        public ServiceResult<QueryResult> QueryTypeIsQuXian(CheLiangQiYeShiPingQueryDto queryData)
        {
            var result = new ServiceResult<QueryResult>();
            string condition = "";

            if (string.IsNullOrEmpty(queryData.Province))
            {
                //condition += string.Format($" AND 1=2 ");
                result.StatusCode = 2;
                result.ErrorMessage = "Province为不能空!";
                return result;
            }
            condition += string.Format(" AND T_CheLiangXinXi.XiaQuSheng = '{0}' ", queryData.Province);
            if (string.IsNullOrEmpty(queryData.City))
            {
                //condition += string.Format($" AND 1=2 ");
                result.StatusCode = 2;
                result.ErrorMessage = "City为不能空!";
                return result;
            }
            else
            {
                condition += string.Format($" AND T_CheLiangXinXi.XiaQuShi = '{queryData.City}' ");
                if (queryData.OrganizationType == 12)
                {
                    if (string.IsNullOrEmpty(queryData.OrgDistrict))
                    {
                        //condition += string.Format($" AND 1=2 ");
                        result.StatusCode = 2;
                        result.ErrorMessage = "OrgDistrict为不能空!";
                        return result;
                    }
                    condition += string.Format($" AND T_CheLiangXinXi.XiaQuXian = '{queryData.OrgDistrict}' ");
                }
            }

            if (!String.IsNullOrEmpty(queryData.QiYeMingCheng))
            {
                condition += string.Format(" AND T_CheLiangYeHuXinXi.QiYeMingCheng like '%{0}%'", queryData.QiYeMingCheng);
            }

            var sql = string.Format(@"
SELECT
	T_CheLiangXinXi.ChePaiHao,
	T_CheLiangXinXi.ChePaiYanSe,
	T_CheLiangXinXi.CheLiangZhongLei,
	T_CheLiangYeHuXinXi.QiYeMingCheng,
	T_CheLiangYeHuXinXi.GeRenCheZhuMingCheng,
	ShiPingTouGeShu,
	CameraSelected,
	VideoServiceKind,
	'0000' AS OrgCode 
FROM
	T_CheLiangXinXi
	LEFT JOIN T_CheLiangYeHuXinXi ON T_CheLiangYeHuXinXi.CheLiangID = T_CheLiangXinXi.ID
	LEFT JOIN ( SELECT CheLiangId, ShiPingTouGeShu, CameraSelected, VideoServiceKind FROM T_CheLiangZhongDuanPeiZhiXinXi ) AS T_CLZDPEXI ON T_CLZDPEXI.CheLiangId = T_CheLiangXinXi.ID
WHERE 1=1 ") + condition;
            var query = _cheLiangXinXiRepository.ExecSqlQuery<CheLiangQiYeShiPingDto>(sql).ToList();
            foreach (var item in query)
            {
                if (!String.IsNullOrEmpty(item.GeRenCheZhuMingCheng) && String.IsNullOrEmpty(item.QiYeMingCheng))
                {
                    item.QiYeMingCheng = item.GeRenCheZhuMingCheng;
                }
            }
            List<IGrouping<string, CheLiangQiYeShiPingDto>> grouplist = new List<IGrouping<string, CheLiangQiYeShiPingDto>>();
            foreach (IGrouping<string, CheLiangQiYeShiPingDto> qiyegroup in query.GroupBy(x => x.QiYeMingCheng))
            {
                grouplist.Add(qiyegroup);
                List<CheLiangQiYeShiPingDto> qiYeList = qiyegroup.ToList<CheLiangQiYeShiPingDto>();
            }
            result.Data = new QueryResult();
            result.Data.totalcount = grouplist.Count;
            result.Data.items = grouplist;
            return result;
        }
    }
}
