using Conwin.Framework.Log4net;
using Quartz;
using System;
using System.Threading.Tasks;
using Conwin.GPSDAGL.JobWindowsService.Common;
using System.Collections.Generic;
using System.Linq;
using Conwin.GPSDAGL.Entities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Conwin.GPSDAGL.JobWindowsService.Config;

namespace Conwin.GPSDAGL.JobWindowsService.Quartz.Job
{
    /// <summary>
    /// 企业档案同步组织信息
    /// 更新车辆监控汇总情况和人员分配监控车辆所属组织记录
    /// </summary>
    [DisallowConcurrentExecution]
    public class JianKongJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Start1();
                if (CollectionConfig.ExecuteBoth) {
                    Start2();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"错误(JianKongJob)信息:{ex.Message},\n堆栈:{ex.StackTrace},\n内部错误:{ex.InnerException?.Message},\n错误来源:{ex.Source}", ex);
            }
        }

        #region 企业档案同步组织信息(T_OrgJieGouHuiZongXinXi)
        public static void Start1()
        {
            try
            {
                LogHelper.Warn("企业档案同步组织信息(T_OrgJieGouHuiZongXinXi) 作业执行开始" + DateTime.Now);
                string SysId = "'60190FC4-5103-4C76-94E4-12A54B62C92A'";
                long random = 10000;
                var now = DateTime.Now;

                var OrgTable = new List<OrgTable>();

                string sqlConnStr = ConfigurationManager.ConnectionStrings["YHQXDSDb"].ConnectionString;

                var TSysOrgSql = @"SELECT SysOrg.ParentSystemOrganization_Id, SysOrg.Id, SysOrg.AdminLevel, SysOrg.City, SysOrg.District,
                                    SysOrg.ManageArea, SysOrg.SysID, SysOrg.Province, SysOrg.Town, SysOrg.SYS_ZuiJinXiuGaiShiJian,
                                    SysOrg.SYS_ChuangJianShiJian, SysOrg.Village,
                                    Org.OrganizationName, OrganizationCode, OrganizationType
                                FROM DC_YHQXDS.dbo.T_Organization Org
                                INNER JOIN DC_YHQXDS.dbo.T_SystemOrganization SysOrg ON Org.Id = SysOrg.Organization_Id
                                WHERE Org.SYS_XiTongZhuangTai=0 AND SysOrg.SYS_XiTongZhuangTai=0 AND SysOrg.SysID={0}
                                ORDER BY SysOrg.SYS_ChuangJianShiJian DESC,SysOrg.SYS_ZuiJinXiuGaiShiJian DESC";
                TSysOrgSql = string.Format(TSysOrgSql, SysId);
                var TSysOrg = DapperSqlHelper.Query<TSysOrg>(sqlConnStr, TSysOrgSql);

                var FZJGSql = @"SELECT BenDanWeiOrgCode FROM DC_GPSDAGL.dbo.T_FenZhiJiGouHeGPSYunYingShangXinXi
                            WHERE SYS_XiTongZhuangTai=0 AND ShiFouKaiTongTongJiQuanXian=1";
                var FZJG = DapperSqlHelper.Query<FenZhiJiGouHeGPSYunYingShangXinXi>(FZJGSql);

                var tableA = from a in FZJG
                             from b in TSysOrg
                             where a.BenDanWeiOrgCode == b.OrganizationCode && b.OrganizationType == 5
                             select new TSysOrg
                             {
                                 ParentSystemOrganization_Id = b.ParentSystemOrganization_Id,
                                 Id = b.Id,
                                 OrganizationCode = b.OrganizationCode,
                                 OrganizationName = b.OrganizationName,
                                 OrganizationType = b.OrganizationType,
                                 ManageArea = b.ManageArea
                             };
                var FenGongSiTongJiGPSAll = from a in tableA
                                            from b in TSysOrg
                                            where a.ParentSystemOrganization_Id == b.ParentSystemOrganization_Id && b.OrganizationType == 6
                                            select new TFenGongSi
                                            {
                                                Id = a.Id,
                                                ParentSystemOrganization_Id = a.ParentSystemOrganization_Id,
                                                OrganizationCode = a.OrganizationCode,
                                                OrganizationName = a.OrganizationName,
                                                OrganizationType = a.OrganizationType,
                                                ManageArea = a.ManageArea,
                                                GPSId = b.Id,
                                                GPSOrganizationCode = b.OrganizationCode,
                                                GPSOrganizationName = b.OrganizationName,
                                                GPSOrganizationType = b.OrganizationType,
                                                GPSManageArea = b.ManageArea
                                            };
                var TFenGongSiTongJiGPSAll = FenGongSiTongJiGPSAll.GroupBy(s => new {
                    s.Id,
                    s.ParentSystemOrganization_Id,
                    s.OrganizationCode,
                    s.OrganizationName,
                    s.OrganizationType,
                    s.ManageArea,
                    s.GPSId,
                    s.GPSOrganizationCode,
                    s.GPSOrganizationName,
                    s.GPSOrganizationType,
                    s.GPSManageArea
                }).Select(a => a.First()).ToList();

                var TFenGongSiChongDie = new List<TFenGongSi>();
                foreach (var FenGongSi in TFenGongSiTongJiGPSAll)
                {

                    var FenGongSiChongDieManageArea = from a in FenGongSi.ManageArea.Split('|').ToList()
                                                      from b in FenGongSi.GPSManageArea.Split('|').ToList()
                                                      where a == b
                                                      orderby a
                                                      select a;
                    var TFenGongSiChongDieManageArea = FenGongSiChongDieManageArea.ToList();
                    var CountChongFuQuYu = TFenGongSiChongDieManageArea.Count();
                    if (CountChongFuQuYu > 0)
                    {
                        var ParentSystemOrganization_Id = FenGongSi.ParentSystemOrganization_Id;
                        var Id = FenGongSi.Id;
                        var GpsId = FenGongSi.GPSId;
                        var ManageAreaChongDie = string.Join("|", TFenGongSiChongDieManageArea);

                        TSysOrg = TSysOrg.Where(s => !(s.ParentSystemOrganization_Id == ParentSystemOrganization_Id && s.Id == Id)).ToList();
                        TSysOrg = TSysOrg.Where(s => !(s.ParentSystemOrganization_Id == ParentSystemOrganization_Id && s.Id == GpsId)).ToList();



                        TFenGongSiChongDie.Add(new TFenGongSi
                        {
                            ParentSystemOrganization_Id = FenGongSi.ParentSystemOrganization_Id,
                            Id = FenGongSi.Id,
                            OrganizationCode = FenGongSi.OrganizationCode,
                            OrganizationName = FenGongSi.OrganizationName,
                            OrganizationType = FenGongSi.OrganizationType,
                            ManageArea = FenGongSi.ManageArea,
                            GPSId = FenGongSi.GPSId,
                            GPSOrganizationCode = FenGongSi.GPSOrganizationCode,
                            GPSOrganizationName = FenGongSi.GPSOrganizationName,
                            GPSOrganizationType = FenGongSi.GPSOrganizationType,
                            GPSManageArea = ManageAreaChongDie
                        });
                    }
                }

                var TSysOrg1 = from a in TFenGongSiChongDie
                               where a.OrganizationType == 5
                               group a by new { a.ParentSystemOrganization_Id, a.Id, a.ManageArea, a.OrganizationCode, a.OrganizationName, a.OrganizationType } into temp
                               select new TSysOrg
                               {
                                   ParentSystemOrganization_Id = temp.Key.ParentSystemOrganization_Id,
                                   Id = temp.Key.Id,
                                   ManageArea = temp.Key.ManageArea,
                                   OrganizationName = temp.Key.OrganizationName,
                                   OrganizationCode = temp.Key.OrganizationCode,
                                   OrganizationType = temp.Key.OrganizationType
                               };
                var TSysOrg2 = from a in TFenGongSiChongDie
                               where a.OrganizationType == 5
                               group a by new { a.Id, a.GPSId, a.GPSManageArea, a.GPSOrganizationName, a.GPSOrganizationCode, a.GPSOrganizationType } into temp
                               select new TSysOrg
                               {
                                   ParentSystemOrganization_Id = temp.Key.Id,
                                   Id = temp.Key.GPSId,
                                   ManageArea = temp.Key.GPSManageArea,
                                   OrganizationName = temp.Key.GPSOrganizationName,
                                   OrganizationCode = temp.Key.GPSOrganizationCode,
                                   OrganizationType = temp.Key.GPSOrganizationType
                               };
                TSysOrg.AddRange(TSysOrg1.ToList());
                TSysOrg.AddRange(TSysOrg2.ToList());

                var TSysOrgBF = TSysOrg;

                var TFgongSiIndex = TFenGongSiChongDie.Where(a => a.OrganizationType == 5).GroupBy(s => new
                {
                    s.OrganizationCode
                }).ToList();

                foreach (var FgongSi in TFgongSiIndex)
                {
                    var TSysSubOrg = new List<TFenGongSi>();
                    var TSysSubOrg1 = from a in TSysOrg
                                      where a.OrganizationCode == FgongSi.Key.OrganizationCode
                                      select new TFenGongSi
                                      {
                                          OrganizationName = a.OrganizationName,
                                          OrganizationCode = a.OrganizationCode,
                                          OrganizationType = a.OrganizationType,
                                          ManageArea = a.ManageArea,
                                          ParentSystemOrganization_Id = a.ParentSystemOrganization_Id,
                                          Id = a.Id
                                      };

                    var TSysSubOrg2 = GetTSysSubOrg(TSysSubOrg1.ToList(), TSysOrg.ToList(), new List<TFenGongSi>());

                    TSysSubOrg.AddRange(TSysSubOrg1.OrderBy(s => s.ParentSystemOrganization_Id).GroupBy(a => new {
                        a.OrganizationName,
                        a.OrganizationCode,
                        a.OrganizationType,
                        a.ManageArea,
                        a.ParentSystemOrganization_Id,
                        a.Id
                    }).Select(a => a.First()).ToList());
                    TSysSubOrg.AddRange(TSysSubOrg2.OrderBy(s => s.ParentSystemOrganization_Id).GroupBy(a => new {
                        a.OrganizationName,
                        a.OrganizationCode,
                        a.OrganizationType,
                        a.ManageArea,
                        a.ParentSystemOrganization_Id,
                        a.Id
                    }).Select(a => a.First()).ToList());

                    var DelTSysSubOrgId = from a in TSysSubOrg
                                          where a.OrganizationCode != FgongSi.Key.OrganizationCode
                                          select a.Id;
                    TSysOrgBF = TSysOrgBF.Where(s => !DelTSysSubOrgId.ToList().Contains(s.Id)).ToList();

                    var SysSubOrgIndex = from a in TSysSubOrg
                                         where a.OrganizationCode != FgongSi.Key.OrganizationCode
                                         select new TFenGongSi
                                         {
                                             OrganizationName = a.OrganizationName,
                                             OrganizationCode = a.OrganizationCode,
                                             OrganizationType = a.OrganizationType,
                                             ManageArea = a.ManageArea,
                                             ParentSystemOrganization_Id = a.ParentSystemOrganization_Id,
                                             Id = a.Id
                                         };
                    var TSysSubOrgIndex = SysSubOrgIndex.ToList();

                    var FGSManageArea = TSysSubOrg.Where(a => a.OrganizationCode == FgongSi.Key.OrganizationCode).Select(b => b.ManageArea).ToList();
                    var TFGSManageArea = new List<string>();
                    if (FGSManageArea.Count() > 0)
                    {
                        foreach (var a in FGSManageArea)
                        {
                            TFGSManageArea.AddRange(a.Split('|').ToList());
                        }
                    }

                    foreach (var index in TSysSubOrgIndex)
                    {
                        var SubOrgId = index.Id.ToString();
                        random = random + 1;
                        var NewSubOrgId = SubOrgId.Remove(1, 5);
                        NewSubOrgId = NewSubOrgId.Insert(1, random.ToString());

                        var TFGSManageAreaChongDie1 = from a in TFGSManageArea
                                                      from b in index.ManageArea.Split('|').ToList()
                                                      where a == b
                                                      select a;
                        var TFGSManageAreaChongDie = TFGSManageAreaChongDie1.ToList();
                        if (TFGSManageAreaChongDie.Count() > 0)
                        {
                            var FGSManageAreaChongDie = string.Join("|", TFGSManageAreaChongDie);
                            TSysSubOrgIndex.Where(s => s.Id.ToString() == SubOrgId).ToList().ForEach(s =>
                            {
                                s.Id = Guid.Parse(NewSubOrgId);
                                s.ManageArea = FGSManageAreaChongDie;
                            });
                            TSysSubOrgIndex.Where(s => s.ParentSystemOrganization_Id.ToString() == SubOrgId).ToList().ForEach(s =>
                            {
                                s.ParentSystemOrganization_Id = Guid.Parse(NewSubOrgId);
                            });
                            TSysOrgBF.Where(s => s.ParentSystemOrganization_Id.ToString() == SubOrgId).ToList().ForEach(s =>
                            {
                                s.ParentSystemOrganization_Id = Guid.Parse(NewSubOrgId);
                            });

                            TSysOrgBF.Add(new TSysOrg
                            {
                                ParentSystemOrganization_Id = index.ParentSystemOrganization_Id,
                                Id = Guid.Parse(NewSubOrgId),
                                ManageArea = index.ManageArea,
                                OrganizationName = index.OrganizationName,
                                OrganizationCode = index.OrganizationCode,
                                OrganizationType = index.OrganizationType
                            });
                        }
                    }
                }

                foreach (var BF in TSysOrgBF)
                {
                    var TSysBFSubOrg = new List<TFenGongSi>();
                    var TSysBFSubOrg1 = from a in TSysOrgBF
                                        where a.Id == BF.Id
                                        select new TFenGongSi
                                        {
                                            OrganizationName = a.OrganizationName,
                                            OrganizationCode = a.OrganizationCode,
                                            OrganizationType = a.OrganizationType,
                                            ManageArea = a.ManageArea,
                                            ParentSystemOrganization_Id = a.ParentSystemOrganization_Id,
                                            Id = a.Id
                                        };
                    var TSysBFSubOrg2 = GetTSysBFSubOrg(TSysBFSubOrg1.ToList(), TSysOrgBF, new List<TFenGongSi>());



                    TSysBFSubOrg.AddRange(TSysBFSubOrg1.OrderBy(s => s.ParentSystemOrganization_Id).GroupBy(a => new {
                        a.OrganizationName,
                        a.OrganizationCode,
                        a.OrganizationType,
                        a.ManageArea,
                        a.ParentSystemOrganization_Id,
                        a.Id
                    }).Select(a => a.First()).ToList());
                    TSysBFSubOrg.AddRange(TSysBFSubOrg2.OrderBy(s => s.ParentSystemOrganization_Id).GroupBy(a => new {
                        a.OrganizationName,
                        a.OrganizationCode,
                        a.OrganizationType,
                        a.ManageArea,
                        a.ParentSystemOrganization_Id,
                        a.Id
                    }).Select(a => a.First()).ToList());


                    var OrgTable1 = from a in TSysBFSubOrg
                                    select new OrgTable
                                    {
                                        ParentId = BF.Id,
                                        ParentOrgCode = BF.OrganizationCode,
                                        ParentOrgName = BF.OrganizationName,
                                        ParentOrgType = BF.OrganizationType,
                                        OrgId = a.Id,
                                        OrgCode = a.OrganizationCode,
                                        OrgName = a.OrganizationName,
                                        OrgType = a.OrganizationType,
                                        OrgManageArea = a.ManageArea,
                                        SYS_ChuangJianShiJian = now,
                                        SYS_ZuiJinXiuGaiShiJian = now,
                                        SYS_XiTongZhuangTai = 0,
                                        SYS_XiTongBeiZhu = "作业定时同步"
                                    };
                    OrgTable.AddRange(OrgTable1.ToList());
                }

                var TSysOrgBFCreater = from a in TSysOrgBF
                                       join b in TSysOrgBF on a.ParentSystemOrganization_Id equals b.Id into temp
                                       from t in temp.DefaultIfEmpty()
                                       select new TSysOrgBFCreater
                                       {
                                           CreatOrganizationCode = t?.OrganizationCode,
                                           CreatOrganizationName = t?.OrganizationName,
                                           CreatOrganizationType = t?.OrganizationType,
                                           CreatOrganizationId = t?.Id,
                                           ParentSystemOrganization_Id = a?.ParentSystemOrganization_Id,
                                           Id = a.Id
                                       };

                OrgTable.ForEach(s => {
                    var temp = TSysOrgBFCreater.Where(a => a.ParentSystemOrganization_Id == s.ParentId && a.Id == s.OrgId).ToList().FirstOrDefault();
                    if (temp != null)
                    {
                        s.CreatOrganizationCode = temp.CreatOrganizationCode;
                        s.CreatOrganizationName = temp.CreatOrganizationName;
                        s.CreatOrganizationType = temp.CreatOrganizationType;
                        s.CreatOrganizationId = temp.CreatOrganizationId;
                    }
                });

                var TOrgTableNew = OrgTable.GroupBy(a => new {
                    a.ParentId,
                    a.ParentOrgCode,
                    a.ParentOrgName,
                    a.ParentOrgType,
                    a.OrgId,
                    a.OrgCode,
                    a.OrgType,
                    a.OrgManageArea,
                    a.CreatOrganizationCode,
                    a.CreatOrganizationName,
                    a.CreatOrganizationType,
                    a.CreatOrganizationId
                }).Select(a => a.First()).ToList();

                using (IDbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ToString()))
                {
                    LogHelper.Warn("T_OrgJieGouHuiZongXinXi作业执行开始" + DateTime.Now);
                    dbConnection.Open();
                    IDbTransaction transaction = dbConnection.BeginTransaction();
                    try
                    {
                        var deleteSql = "truncate table DC_GPSDAGL.dbo.T_OrgJieGouHuiZongXinXi";
                        var addOrgJieGouHuiZongXinXiSql = @"
                    INSERT INTO DC_GPSDAGL.dbo.T_OrgJieGouHuiZongXinXi
                        ( ID ,
                        ParentOrgId,
                        ParentOrgCode ,
                        ParentOrgName ,
                        ParentOrgType ,
                        OrgId,
                        OrgCode ,
                        OrgName ,
                        OrgType ,
                        OrgManageArea ,
                        CreatOrganizationCode ,
                        CreatOrganizationName ,
                        CreatOrganizationType ,
                        CreatOrganizationId ,
                        SYS_ChuangJianShiJian ,
                        SYS_ZuiJinXiuGaiShiJian ,
                        SYS_XiTongZhuangTai ,
                        SYS_XiTongBeiZhu
                        )
                        VALUES
                        ( NEWID() ,
                        @ParentId,
                        @ParentOrgCode ,
                        @ParentOrgName ,
                        @ParentOrgType ,
                        @OrgId,
                        @OrgCode ,
                        @OrgName ,
                        @OrgType ,
                        @OrgManageArea ,
                        @CreatOrganizationCode ,
                        @CreatOrganizationName ,
                        @CreatOrganizationType ,
                        @CreatOrganizationId ,
                        @SYS_ChuangJianShiJian ,
                        @SYS_ZuiJinXiuGaiShiJian ,
                        @SYS_XiTongZhuangTai ,
                        @SYS_XiTongBeiZhu
                        )";

                        dbConnection.Execute(deleteSql, null, transaction);
                        dbConnection.Execute(addOrgJieGouHuiZongXinXiSql, TOrgTableNew, transaction, 1800);
                        transaction.Commit();
                        LogHelper.Warn("T_OrgJieGouHuiZongXinXi作业执行完毕" + DateTime.Now);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("T_OrgJieGouHuiZongXinXi 作业执行异常，异常信息：" + e.Message, e);
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"更新车辆监控汇总情况和人员分配监控车辆所属组织记录异常，异常信息\n {ex.Message}", ex);
                throw ex;
            }
          

        }

        public static List<TFenGongSi> GetTSysSubOrg(List<TFenGongSi> TSysSubOrg, List<TSysOrg> TSysOrg, List<TFenGongSi> result)
        {
            var TSysSubOrg2 = from a in TSysSubOrg
                              from b in TSysOrg
                              where a.Id == b.ParentSystemOrganization_Id
                              select new TFenGongSi
                              {
                                  OrganizationName = b.OrganizationName,
                                  OrganizationCode = b.OrganizationCode,
                                  OrganizationType = b.OrganizationType,
                                  ManageArea = b.ManageArea,
                                  ParentSystemOrganization_Id = b.ParentSystemOrganization_Id,
                                  Id = b.Id
                              };
            if (TSysSubOrg2.Count() > 0)
            {
                result.AddRange(TSysSubOrg2.ToList());
                GetTSysSubOrg(TSysSubOrg2.ToList(), TSysOrg, result);
            }
            else
            {
                return result;
            }
            return result;
        }

        public static List<TFenGongSi> GetTSysBFSubOrg(List<TFenGongSi> TSysBFSubOrg, List<TSysOrg> TSysOrgBF, List<TFenGongSi> result)
        {
            var TSysBFSubOrg2 = from a in TSysBFSubOrg
                                from b in TSysOrgBF
                                where a.Id == b.ParentSystemOrganization_Id
                                select new TFenGongSi
                                {
                                    OrganizationName = b.OrganizationName,
                                    OrganizationCode = b.OrganizationCode,
                                    OrganizationType = b.OrganizationType,
                                    ManageArea = b.ManageArea,
                                    ParentSystemOrganization_Id = b.ParentSystemOrganization_Id,
                                    Id = b.Id
                                };
            if (TSysBFSubOrg2.Count() > 0)
            {
                result.AddRange(TSysBFSubOrg2.ToList());
                GetTSysBFSubOrg(TSysBFSubOrg2.ToList(), TSysOrgBF, result);
            }
            else
            {
                return result;
            }
            return result;
        }

        #endregion

        #region 更新车辆监控汇总情况和人员分配监控车辆所属组织记录(T_RYFPJKCLSSOrgJiLu、T_CheLiangJianKongHuiZongQingKuang)
        public void Start2()
        {
            try
            {
                LogHelper.Warn("更新车辆监控汇总情况和人员分配监控车辆所属组织记录(T_RYFPJKCLSSOrgJiLu、T_CheLiangJianKongHuiZongQingKuang) 作业执行开始" + DateTime.Now);

                var now = DateTime.Now;

                var OrgHuiZongSql = " SELECT *  FROM dbo.T_OrgJieGouHuiZongXinXi";
                var OrgJieGouHuiZongXinXiList = DapperSqlHelper.Query<OrgJieGouHuiZongXinXi>(OrgHuiZongSql);

                var TCheLiangSql = @"SELECT DISTINCT CL.ID AS CheLiangId, CL.ChuangJianDanWeiOrgCode, CL.XiaQuShi, CL.ChePaiHao, CL.ChePaiYanSe, CL.CheLiangZhongLei,
                            --CASE WHEN DATEDIFF(second,CLZJDWXX.ZuiJinDingWeiShiJian,DATEADD(second,-7200,GETDATE()))+7200>0 AND DATEDIFF(second,CLZJDWXX.ZuiJinDingWeiShiJian,DATEADD(second,-7200,GETDATE()))+7200<=7200 THEN 1 ELSE 2 END AS ShiFouZaiXian,
                            CASE WHEN DATEDIFF(n,CLZJDWXX.ZuiJinDingWeiShiJian,DATEADD(n,-120,GETDATE()))+120>0 AND DATEDIFF(n,CLZJDWXX.ZuiJinDingWeiShiJian,DATEADD(n,-120,GETDATE()))+120<=120 THEN 1 ELSE 2 END AS ShiFouZaiXian,
                            CLZDPZXX.ShiPingTouGeShu,
                            QY.BenDanWeiOrgCode AS QiYeOrgCode,
                            CD.BenDanWeiOrgCode AS CheDuiOrgCode , Org.ParentOrgType, Org.OrgType
                        FROM dbo.T_CheLiangXinXi CL
                            LEFT JOIN dbo.T_CheLiangZuiJinDingWeiXinXi CLZJDWXX
                            ON CL.ID = CLZJDWXX.CheLiangID AND CLZJDWXX.SYS_XiTongZhuangTai=0
                            LEFT JOIN dbo.T_CheLiangYeHuXinXi CLYHXX
                            ON CL.ID = CLYHXX.CheLiangID AND CLYHXX.SYS_XiTongZhuangTai=0
                            LEFT JOIN dbo.T_QiYeXinXi QY
                            ON CLYHXX.QiYeOrgCode = QY.BenDanWeiOrgCode AND QY.SYS_XiTongZhuangTai=0
                            LEFT JOIN dbo.T_CheDuiXinXi CD
                            ON CLYHXX.CheDuiOrgCode = CD.BenDanWeiOrgCode AND CD.SYS_XiTongZhuangTai=0
                            LEFT JOIN dbo.T_OrgJieGouHuiZongXinXi Org
                            ON CL.ChuangJianDanWeiOrgCode = Org.ParentOrgCode
                                AND Org.SYS_XiTongZhuangTai=0 AND Org.ParentOrgCode = Org.OrgCode
                            LEFT JOIN dbo.T_CheLiangZhongDuanPeiZhiXinXi CLZDPZXX
                            ON CL.ID = CLZDPZXX.CheLiangID AND CLZDPZXX.SYS_XiTongZhuangTai=0 AND CLZDPZXX.ShiFouAnZhuangShiPingZhongDuan = 1
                        WHERE CL.SYS_XiTongZhuangTai=0
                            AND CL.ZhuangTai=3";
                var TCheLiangList1 = DapperSqlHelper.Query<TCheLiang>(TCheLiangSql);

                var CheLiangJianKongYuanJiLuSql = " SELECT CheLiangID,SysUserID FROM dbo.T_CheLiangJianKongYuanJiLu WHERE  SYS_XiTongZhuangTai=0";
                var CheLiangJianKongYuanJiLuList = DapperSqlHelper.Query<CheLiangJianKongYuanJiLu>(CheLiangJianKongYuanJiLuSql);

                var TCheLiangList2 = from a in TCheLiangList1
                                     from b in CheLiangJianKongYuanJiLuList
                                     where a.CheLiangId == b.CheLiangID
                                     select new TCheLiang
                                     {
                                         CheLiangId = a.CheLiangId,
                                         ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                         XiaQuShi = a.XiaQuShi,
                                         ChePaiHao = a.ChePaiHao,
                                         ChePaiYanSe = a.ChePaiYanSe,
                                         CheLiangZhongLei = a.CheLiangZhongLei,
                                         ShiFouZaiXian = a.ShiFouZaiXian,
                                         ShiPingTouGeShu = a.ShiPingTouGeShu,
                                         QiYeOrgCode = a.QiYeOrgCode,
                                         CheDuiOrgCode = a.CheDuiOrgCode,
                                         ParentOrgType = a.ParentOrgType,
                                         OrgType = a.OrgType,
                                         SysUserId = b.SysUserID
                                     };
                TCheLiangList1.AddRange(TCheLiangList2.ToList());

                var TOrgCheLiangList = new List<TOrgCheLiang>();
                //根据车辆的创建单位构建每级车辆
                var TOrgCheLiangList1 = from a in TCheLiangList1
                                        from b in OrgJieGouHuiZongXinXiList
                                        where a.ChuangJianDanWeiOrgCode == b.OrgCode && TypeHelper.ToString(b.OrgManageArea).IndexOf(TypeHelper.ToString(a.XiaQuShi)) > 0
                                        select new TOrgCheLiang
                                        {
                                            SysUserId = a.SysUserId,
                                            ParentOrgId = b.ParentOrgId,
                                            ParentOrgCode = b.ParentOrgCode,
                                            ParentOrgType = b.ParentOrgType,
                                            ParentOrgName = b.ParentOrgName,
                                            OrgId = b.OrgId,
                                            OrgCode = b.OrgCode,
                                            OrgName = b.OrgName,
                                            OrgType = b.OrgType,
                                            CheLiangId = a.CheLiangId,
                                            ChePaiHao = a.ChePaiHao,
                                            ChePaiYanSe = a.ChePaiYanSe,
                                            CheLiangZhongLei = a.CheLiangZhongLei,
                                            ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                            ShiFouZaiXian = a.ShiFouZaiXian,
                                            ShiPingTouGeShu = a.ShiPingTouGeShu
                                        };

                //查找属于企业的车辆信息
                var TOrgCheLiangList2 = from a in TCheLiangList1
                                        from b in OrgJieGouHuiZongXinXiList
                                        where a.QiYeOrgCode == b.OrgCode && !string.IsNullOrEmpty(a.QiYeOrgCode) && TypeHelper.ToString(b.OrgManageArea).IndexOf(TypeHelper.ToString(a.XiaQuShi)) > 0
                                        select new TOrgCheLiang
                                        {
                                            SysUserId = a.SysUserId,
                                            ParentOrgId = b.ParentOrgId,
                                            ParentOrgCode = b.ParentOrgCode,
                                            ParentOrgType = b.ParentOrgType,
                                            ParentOrgName = b.ParentOrgName,
                                            OrgId = b.OrgId,
                                            OrgCode = b.OrgCode,
                                            OrgName = b.OrgName,
                                            OrgType = b.OrgType,
                                            CheLiangId = a.CheLiangId,
                                            ChePaiHao = a.ChePaiHao,
                                            ChePaiYanSe = a.ChePaiYanSe,
                                            CheLiangZhongLei = a.CheLiangZhongLei,
                                            ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                            ShiFouZaiXian = a.ShiFouZaiXian,
                                            ShiPingTouGeShu = a.ShiPingTouGeShu
                                        };
                //查找属于车队的企业信息
                var TOrgCheLiangList3 = from a in TCheLiangList1
                                        from b in OrgJieGouHuiZongXinXiList
                                        where a.CheDuiOrgCode == b.OrgCode && !string.IsNullOrEmpty(a.CheDuiOrgCode) && TypeHelper.ToString(b.OrgManageArea).IndexOf(TypeHelper.ToString(a.XiaQuShi)) > 0
                                        select new TOrgCheLiang
                                        {
                                            SysUserId = a.SysUserId,
                                            ParentOrgId = b.ParentOrgId,
                                            ParentOrgCode = b.ParentOrgCode,
                                            ParentOrgType = b.ParentOrgType,
                                            ParentOrgName = b.ParentOrgName,
                                            OrgId = b.OrgId,
                                            OrgCode = b.OrgCode,
                                            OrgName = b.OrgName,
                                            OrgType = b.OrgType,
                                            CheLiangId = a.CheLiangId,
                                            ChePaiHao = a.ChePaiHao,
                                            ChePaiYanSe = a.ChePaiYanSe,
                                            CheLiangZhongLei = a.CheLiangZhongLei,
                                            ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                            ShiFouZaiXian = a.ShiFouZaiXian,
                                            ShiPingTouGeShu = a.ShiPingTouGeShu
                                        };

                TOrgCheLiangList.AddRange(TOrgCheLiangList1.GroupBy(s => new {
                    s.SysUserId,
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgType,
                    s.ParentOrgName,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.CheLiangId,
                    s.ChePaiHao,
                    s.ChePaiYanSe,
                    s.CheLiangZhongLei,
                    s.ChuangJianDanWeiOrgCode,
                    s.ShiFouZaiXian,
                    s.ShiPingTouGeShu
                }).Select(a => a.First()).ToList());
                TOrgCheLiangList.AddRange(TOrgCheLiangList2.GroupBy(s => new {
                    s.SysUserId,
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgType,
                    s.ParentOrgName,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.CheLiangId,
                    s.ChePaiHao,
                    s.ChePaiYanSe,
                    s.CheLiangZhongLei,
                    s.ChuangJianDanWeiOrgCode,
                    s.ShiFouZaiXian,
                    s.ShiPingTouGeShu
                }).Select(a => a.First()).ToList());
                TOrgCheLiangList.AddRange(TOrgCheLiangList3.GroupBy(s => new {
                    s.SysUserId,
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgType,
                    s.ParentOrgName,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.CheLiangId,
                    s.ChePaiHao,
                    s.ChePaiYanSe,
                    s.CheLiangZhongLei,
                    s.ChuangJianDanWeiOrgCode,
                    s.ShiFouZaiXian,
                    s.ShiPingTouGeShu
                }).Select(a => a.First()).ToList());


                var Org = OrgJieGouHuiZongXinXiList.Where(s => s.ParentOrgCode != s.OrgCode).ToList();
                var NewOrgCheLiangXinXi = from a in TOrgCheLiangList
                                          join b in Org on a.ParentOrgId equals b.OrgId into temp
                                          from t in temp.DefaultIfEmpty()
                                          where TypeHelper.ToString(a.SysUserId) == ""
                                          select new TNewOrgCheLiangXinXi
                                          {
                                              ParentOrgId = t?.CreatOrganizationId,
                                              ParentOrgCode = t?.CreatOrganizationCode,
                                              ParentOrgName = t?.CreatOrganizationName,
                                              ParentOrgType = t?.CreatOrganizationType,
                                              OrgId = a.ParentOrgId,
                                              OrgCode = a.ParentOrgCode,
                                              OrgName = a.ParentOrgName,
                                              OrgType = a.ParentOrgType,
                                              CheLiangId = a.CheLiangId,
                                              ChePaiHao = a.ChePaiHao,
                                              ChePaiYanSe = a.ChePaiYanSe,
                                              CheLiangZhongLei = a.CheLiangZhongLei,
                                              ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                              ShiFouZaiXian = a.ShiFouZaiXian,
                                              ShiPingTouGeShu = a.ShiPingTouGeShu,
                                              SYS_ChuangJianShiJian = now,
                                              SYS_ZuiJinXiuGaiShiJian = now,
                                              SYS_XiTongZhuangTai = 0,
                                              SYS_XiTongBeiZhu = "作业定时同步",
                                          };

                var TNewOrgCheLiangXinXi = NewOrgCheLiangXinXi.GroupBy(s => new {
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgName,
                    s.ParentOrgType,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.CheLiangId,
                    s.ChePaiHao,
                    s.ChePaiYanSe,
                    s.CheLiangZhongLei,
                    s.ChuangJianDanWeiOrgCode,
                    s.ShiFouZaiXian,
                    s.ShiPingTouGeShu
                }).Select(a => a.First()).Where(b => !(b.ParentOrgId == null && b.OrgCode != "0000")).ToList();

                var NewOrgCheLiangXinXiBySysUser = from a in TOrgCheLiangList
                                                   join b in Org on a.ParentOrgId equals b.OrgId into temp
                                                   from t in temp.DefaultIfEmpty()
                                                   where TypeHelper.ToString(a.SysUserId) != ""
                                                   select new TNewOrgCheLiangXinXi
                                                   {
                                                       SysUserId = a.SysUserId,
                                                       ParentOrgId = t?.CreatOrganizationId,
                                                       ParentOrgCode = t?.CreatOrganizationCode,
                                                       ParentOrgName = t?.CreatOrganizationName,
                                                       ParentOrgType = t?.CreatOrganizationType,
                                                       OrgId = a.ParentOrgId,
                                                       OrgCode = a.ParentOrgCode,
                                                       OrgName = a.ParentOrgName,
                                                       OrgType = a.ParentOrgType,
                                                       CheLiangId = a.CheLiangId,
                                                       ChePaiHao = a.ChePaiHao,
                                                       ChePaiYanSe = a.ChePaiYanSe,
                                                       CheLiangZhongLei = a.CheLiangZhongLei,
                                                       ChuangJianDanWeiOrgCode = a.ChuangJianDanWeiOrgCode,
                                                       ShiFouZaiXian = a.ShiFouZaiXian,
                                                       ShiPingTouGeShu = a.ShiPingTouGeShu,
                                                       SYS_ChuangJianShiJian = now,
                                                       SYS_ZuiJinXiuGaiShiJian = now,
                                                       SYS_XiTongZhuangTai = 0,
                                                       SYS_XiTongBeiZhu = "作业定时同步"
                                                   };
                var TNewOrgCheLiangXinXiBySysUser = NewOrgCheLiangXinXiBySysUser.GroupBy(s => new {
                    s.SysUserId,
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgName,
                    s.ParentOrgType,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.CheLiangId,
                    s.ChePaiHao,
                    s.ChePaiYanSe,
                    s.CheLiangZhongLei,
                    s.ChuangJianDanWeiOrgCode,
                    s.ShiFouZaiXian,
                    s.ShiPingTouGeShu
                }).Select(a => a.First()).Where(b => !(b.ParentOrgId == null && b.OrgCode != "0000")).ToList();

                //管理员总数
                var tableA = from a in TNewOrgCheLiangXinXi
                             group a by new
                             {
                                 a.ParentOrgId,
                                 a.ParentOrgCode,
                                 a.ParentOrgName,
                                 a.ParentOrgType,
                                 a.OrgId,
                                 a.OrgCode,
                                 a.OrgName,
                                 a.OrgType,
                                 a.CheLiangId
                             } into b
                             select new
                             {
                                 b.Key.ParentOrgId,
                                 b.Key.ParentOrgCode,
                                 b.Key.ParentOrgName,
                                 b.Key.ParentOrgType,
                                 b.Key.OrgId,
                                 b.Key.OrgCode,
                                 b.Key.OrgName,
                                 b.Key.OrgType,
                                 b.Key.CheLiangId
                             };

                var TOrgCheLiangTotalCount = from a in tableA
                                             group a by new
                                             {
                                                 a.ParentOrgId,
                                                 a.ParentOrgCode,
                                                 a.ParentOrgName,
                                                 a.ParentOrgType,
                                                 a.OrgId,
                                                 a.OrgCode,
                                                 a.OrgName,
                                                 a.OrgType
                                             } into b
                                             select new
                                             {
                                                 b.Key.ParentOrgId,
                                                 b.Key.ParentOrgCode,
                                                 b.Key.ParentOrgName,
                                                 b.Key.ParentOrgType,
                                                 b.Key.OrgId,
                                                 b.Key.OrgCode,
                                                 b.Key.OrgName,
                                                 b.Key.OrgType,
                                                 OrgZongCheLiangShu = b.Count()
                                             };
                //管理员在线数
                var tableA1 = from a in TNewOrgCheLiangXinXi
                              where a.ShiFouZaiXian == 1
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.CheLiangId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.CheLiangId
                              };
                var TOrgCheLiangZaiXianCount = from a in tableA1
                                               group a by new
                                               {
                                                   a.ParentOrgId,
                                                   a.ParentOrgCode,
                                                   a.OrgId,
                                                   a.OrgCode,
                                               } into b
                                               select new
                                               {
                                                   b.Key.ParentOrgId,
                                                   b.Key.ParentOrgCode,
                                                   b.Key.OrgId,
                                                   b.Key.OrgCode,
                                                   OrgZaiXianCheLiangShu = b.Count()
                                               };
                //管理员视频总数
                var tableA2 = from a in TNewOrgCheLiangXinXi
                              where a.ShiPingTouGeShu > 0
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.ParentOrgName,
                                  a.ParentOrgType,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.OrgName,
                                  a.OrgType,
                                  a.CheLiangId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.ParentOrgName,
                                  b.Key.ParentOrgType,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.OrgName,
                                  b.Key.OrgType,
                                  b.Key.CheLiangId
                              };
                var TOrgCheLiangShiPinTotalCount = from a in tableA2
                                                   group a by new
                                                   {
                                                       a.ParentOrgId,
                                                       a.ParentOrgCode,
                                                       a.ParentOrgName,
                                                       a.ParentOrgType,
                                                       a.OrgId,
                                                       a.OrgCode,
                                                       a.OrgName,
                                                       a.OrgType,
                                                   } into b
                                                   select new
                                                   {
                                                       b.Key.ParentOrgId,
                                                       b.Key.ParentOrgCode,
                                                       b.Key.ParentOrgName,
                                                       b.Key.ParentOrgType,
                                                       b.Key.OrgId,
                                                       b.Key.OrgCode,
                                                       b.Key.OrgName,
                                                       b.Key.OrgType,
                                                       OrgShiPinZongCheLiangShu = b.Count()
                                                   };
                //管理员在线视频数
                var tableA3 = from a in TNewOrgCheLiangXinXi
                              where a.ShiFouZaiXian == 1 && a.ShiPingTouGeShu > 0
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.CheLiangId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.CheLiangId
                              };
                var TOrgCheLiangShiPinZaiXianCount = from a in tableA3
                                                     group a by new
                                                     {
                                                         a.ParentOrgId,
                                                         a.ParentOrgCode,
                                                         a.OrgId,
                                                         a.OrgCode,
                                                     } into b
                                                     select new
                                                     {
                                                         b.Key.ParentOrgId,
                                                         b.Key.ParentOrgCode,
                                                         b.Key.OrgId,
                                                         b.Key.OrgCode,
                                                         OrgShiPinZaiXianCheLiangShu = b.Count()
                                                     };
                //管理员车辆汇总情况
                var OrgCheLiangHuiZongQingKuang = from a in TOrgCheLiangTotalCount
                                                  join b in TOrgCheLiangZaiXianCount on new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                      a.OrgId,
                                                      a.OrgCode
                                                  } equals new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(b.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(b.ParentOrgCode),
                                                      b.OrgId,
                                                      b.OrgCode
                                                  } into ZaiXianCountTemp
                                                  from ZaiXianCount in ZaiXianCountTemp.DefaultIfEmpty()
                                                  join c in TOrgCheLiangShiPinTotalCount on new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                      a.OrgId,
                                                      a.OrgCode
                                                  } equals new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(c.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(c.ParentOrgCode),
                                                      c.OrgId,
                                                      c.OrgCode
                                                  } into ShiPinTotalCountTemp
                                                  from ShiPinTotalCount in ShiPinTotalCountTemp.DefaultIfEmpty()
                                                  join d in TOrgCheLiangShiPinZaiXianCount on new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                      a.OrgId,
                                                      a.OrgCode
                                                  } equals new
                                                  {
                                                      ParentOrgId = TypeHelper.ToString(d.ParentOrgId),
                                                      ParentOrgCode = TypeHelper.ToString(d.ParentOrgCode),
                                                      d.OrgId,
                                                      d.OrgCode
                                                  } into ShiPinZaiXianCountTemp
                                                  from ShiPinZaiXianCount in ShiPinZaiXianCountTemp.DefaultIfEmpty()
                                                  select new CheLiangJianKongHuiZongQingKuang
                                                  {
                                                      ParentOrgId = a.ParentOrgId,
                                                      ParentOrgCode = a.ParentOrgCode,
                                                      ParentOrgName = a.ParentOrgName,
                                                      ParentOrgType = a.ParentOrgType,
                                                      OrgId = a.OrgId,
                                                      OrgCode = a.OrgCode,
                                                      OrgName = a.OrgName,
                                                      OrgType = a.OrgType,
                                                      OrgZongCheLiangShu = a.OrgZongCheLiangShu,
                                                      OrgZaiXianCheLiangShu = ZaiXianCount?.OrgZaiXianCheLiangShu,
                                                      OrgShiPinZongCheLiangShu = ShiPinTotalCount?.OrgShiPinZongCheLiangShu,
                                                      OrgShiPinZaiXianCheLiangShu = ShiPinZaiXianCount?.OrgShiPinZaiXianCheLiangShu,
                                                      ShiFouLiShiJiLu = 2,
                                                      SYS_ChuangJianShiJian = now,
                                                      SYS_ZuiJinXiuGaiShiJian = now,
                                                      SYS_XiTongZhuangTai = 0,
                                                      SYS_XiTongBeiZhu = "作业定时同步"
                                                  };
                var TOrgCheLiangHuiZongQingKuang = OrgCheLiangHuiZongQingKuang.GroupBy(s => new {
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgType,
                    s.ParentOrgName,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.OrgZongCheLiangShu,
                    s.OrgZaiXianCheLiangShu,
                    s.OrgShiPinZongCheLiangShu,
                    s.OrgShiPinZaiXianCheLiangShu,
                    s.ShiFouLiShiJiLu
                }).Select(a => a.First()).ToList();

                //一般监控人员总数
                var tableB = from a in TNewOrgCheLiangXinXiBySysUser
                             group a by new
                             {
                                 a.ParentOrgId,
                                 a.ParentOrgCode,
                                 a.ParentOrgName,
                                 a.ParentOrgType,
                                 a.OrgId,
                                 a.OrgCode,
                                 a.OrgName,
                                 a.OrgType,
                                 a.CheLiangId,
                                 a.SysUserId
                             } into b
                             select new
                             {
                                 b.Key.ParentOrgId,
                                 b.Key.ParentOrgCode,
                                 b.Key.ParentOrgName,
                                 b.Key.ParentOrgType,
                                 b.Key.OrgId,
                                 b.Key.OrgCode,
                                 b.Key.OrgName,
                                 b.Key.OrgType,
                                 b.Key.CheLiangId,
                                 b.Key.SysUserId
                             };

                var TOrgCheLiangTotalCountBySysUser = from a in tableB
                                                      group a by new
                                                      {
                                                          a.ParentOrgId,
                                                          a.ParentOrgCode,
                                                          a.ParentOrgName,
                                                          a.ParentOrgType,
                                                          a.OrgId,
                                                          a.OrgCode,
                                                          a.OrgName,
                                                          a.OrgType,
                                                          a.SysUserId
                                                      } into b
                                                      select new
                                                      {
                                                          b.Key.ParentOrgId,
                                                          b.Key.ParentOrgCode,
                                                          b.Key.ParentOrgName,
                                                          b.Key.ParentOrgType,
                                                          b.Key.OrgId,
                                                          b.Key.OrgCode,
                                                          b.Key.OrgName,
                                                          b.Key.OrgType,
                                                          b.Key.SysUserId,
                                                          OrgZongCheLiangShu = b.Count()
                                                      };
                //一般监控人员在线数
                var tableB1 = from a in TNewOrgCheLiangXinXiBySysUser
                              where a.ShiFouZaiXian == 1
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.CheLiangId,
                                  a.SysUserId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.CheLiangId,
                                  b.Key.SysUserId
                              };
                var TOrgCheLiangZaiXianCountBySysUser = from a in tableB1
                                                        group a by new
                                                        {
                                                            a.ParentOrgId,
                                                            a.ParentOrgCode,
                                                            a.OrgId,
                                                            a.OrgCode,
                                                            a.SysUserId
                                                        } into b
                                                        select new
                                                        {
                                                            b.Key.ParentOrgId,
                                                            b.Key.ParentOrgCode,
                                                            b.Key.OrgId,
                                                            b.Key.OrgCode,
                                                            b.Key.SysUserId,
                                                            OrgZaiXianCheLiangShu = b.Count()
                                                        };
                //一般监控人员视频总数
                var tableB2 = from a in TNewOrgCheLiangXinXiBySysUser
                              where a.ShiPingTouGeShu > 0
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.ParentOrgName,
                                  a.ParentOrgType,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.OrgName,
                                  a.OrgType,
                                  a.CheLiangId,
                                  a.SysUserId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.ParentOrgName,
                                  b.Key.ParentOrgType,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.OrgName,
                                  b.Key.OrgType,
                                  b.Key.CheLiangId,
                                  b.Key.SysUserId
                              };
                var TOrgCheLiangShiPinTotalCountBySysUser = from a in tableB2
                                                            group a by new
                                                            {
                                                                a.ParentOrgId,
                                                                a.ParentOrgCode,
                                                                a.ParentOrgName,
                                                                a.ParentOrgType,
                                                                a.OrgId,
                                                                a.OrgCode,
                                                                a.OrgName,
                                                                a.OrgType,
                                                                a.SysUserId
                                                            } into b
                                                            select new
                                                            {
                                                                b.Key.ParentOrgId,
                                                                b.Key.ParentOrgCode,
                                                                b.Key.ParentOrgName,
                                                                b.Key.ParentOrgType,
                                                                b.Key.OrgId,
                                                                b.Key.OrgCode,
                                                                b.Key.OrgName,
                                                                b.Key.OrgType,
                                                                b.Key.SysUserId,
                                                                OrgShiPinZongCheLiangShu = b.Count()
                                                            };
                //一般监控人员视频在线数
                var tableB3 = from a in TNewOrgCheLiangXinXiBySysUser
                              where a.ShiFouZaiXian == 1
                              group a by new
                              {
                                  a.ParentOrgId,
                                  a.ParentOrgCode,
                                  a.OrgId,
                                  a.OrgCode,
                                  a.CheLiangId,
                                  a.SysUserId
                              } into b
                              select new
                              {
                                  b.Key.ParentOrgId,
                                  b.Key.ParentOrgCode,
                                  b.Key.OrgId,
                                  b.Key.OrgCode,
                                  b.Key.CheLiangId,
                                  b.Key.SysUserId
                              };
                var TOrgCheLiangShiPinZaiXianCountBySysUser = from a in tableB3
                                                              group a by new
                                                              {
                                                                  a.ParentOrgId,
                                                                  a.ParentOrgCode,
                                                                  a.OrgId,
                                                                  a.OrgCode,
                                                                  a.SysUserId
                                                              } into b
                                                              select new
                                                              {
                                                                  b.Key.ParentOrgId,
                                                                  b.Key.ParentOrgCode,
                                                                  b.Key.OrgId,
                                                                  b.Key.OrgCode,
                                                                  b.Key.SysUserId,
                                                                  OrgShiPinZaiXianCheLiangShu = b.Count()
                                                              };
                //一般监控人员车辆汇总情况
                var OrgCheLiangHuiZongQingKuangBySysUser = from a in TOrgCheLiangTotalCountBySysUser
                                                           join b in TOrgCheLiangZaiXianCountBySysUser on new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                               a.OrgId,
                                                               a.OrgCode,
                                                               a.SysUserId
                                                           } equals new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(b.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(b.ParentOrgCode),
                                                               b.OrgId,
                                                               b.OrgCode,
                                                               b.SysUserId
                                                           } into ZaiXianCountTemp
                                                           from ZaiXianCount in ZaiXianCountTemp.DefaultIfEmpty()
                                                           join c in TOrgCheLiangShiPinTotalCountBySysUser on new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                               a.OrgId,
                                                               a.OrgCode,
                                                               a.SysUserId
                                                           } equals new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(c.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(c.ParentOrgCode),
                                                               c.OrgId,
                                                               c.OrgCode,
                                                               c.SysUserId
                                                           } into ShiPinTotalCountTemp
                                                           from ShiPinTotalCount in ShiPinTotalCountTemp.DefaultIfEmpty()
                                                           join d in TOrgCheLiangShiPinZaiXianCountBySysUser on new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(a.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(a.ParentOrgCode),
                                                               a.OrgId,
                                                               a.OrgCode,
                                                               a.SysUserId
                                                           } equals new
                                                           {
                                                               ParentOrgId = TypeHelper.ToString(d.ParentOrgId),
                                                               ParentOrgCode = TypeHelper.ToString(d.ParentOrgCode),
                                                               d.OrgId,
                                                               d.OrgCode,
                                                               d.SysUserId
                                                           } into ShiPinZaiXianCountTemp
                                                           from ShiPinZaiXianCount in ShiPinZaiXianCountTemp.DefaultIfEmpty()
                                                           select new CheLiangJianKongHuiZongQingKuang
                                                           {
                                                               UserId = a.SysUserId,
                                                               ParentOrgId = a.ParentOrgId,
                                                               ParentOrgCode = a.ParentOrgCode,
                                                               ParentOrgName = a.ParentOrgName,
                                                               ParentOrgType = a.ParentOrgType,
                                                               OrgId = a.OrgId,
                                                               OrgCode = a.OrgCode,
                                                               OrgName = a.OrgName,
                                                               OrgType = a.OrgType,
                                                               OrgZongCheLiangShu = a.OrgZongCheLiangShu,
                                                               OrgZaiXianCheLiangShu = ZaiXianCount?.OrgZaiXianCheLiangShu,
                                                               OrgShiPinZongCheLiangShu = ShiPinTotalCount?.OrgShiPinZongCheLiangShu,
                                                               OrgShiPinZaiXianCheLiangShu = ShiPinZaiXianCount?.OrgShiPinZaiXianCheLiangShu,
                                                               ShiFouLiShiJiLu = 2,
                                                               SYS_ChuangJianShiJian = now,
                                                               SYS_ZuiJinXiuGaiShiJian = now,
                                                               SYS_XiTongZhuangTai = 0,
                                                               SYS_XiTongBeiZhu = "作业定时同步"
                                                           };
                var TOrgCheLiangHuiZongQingKuangBySysUser = OrgCheLiangHuiZongQingKuangBySysUser.GroupBy(s => new {
                    s.UserId,
                    s.ParentOrgId,
                    s.ParentOrgCode,
                    s.ParentOrgType,
                    s.ParentOrgName,
                    s.OrgId,
                    s.OrgCode,
                    s.OrgName,
                    s.OrgType,
                    s.OrgZongCheLiangShu,
                    s.OrgZaiXianCheLiangShu,
                    s.OrgShiPinZongCheLiangShu,
                    s.OrgShiPinZaiXianCheLiangShu,
                    s.ShiFouLiShiJiLu
                }).Select(a => a.First()).ToList();

                TNewOrgCheLiangXinXi.AddRange(TNewOrgCheLiangXinXiBySysUser);
                TOrgCheLiangHuiZongQingKuang.AddRange(TOrgCheLiangHuiZongQingKuangBySysUser);

                Task.Run(() => {
                    using (IDbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ToString()))
                    {
                        LogHelper.Warn("T_CheLiangJianKongHuiZongQingKuang 作业执行开始" + DateTime.Now);
                        dbConnection.Open();
                        IDbTransaction transaction = dbConnection.BeginTransaction();
                        try
                        {
                            #region sql
                            var deleteSql = @" TRUNCATE table DC_GPSDAGL.dbo.T_CheLiangJianKongHuiZongQingKuang;";
                            var addHuiZongSql = @"
                    INSERT INTO DC_GPSDAGL.dbo.T_CheLiangJianKongHuiZongQingKuang
                    ( ID ,
                    OrgCode ,
                    OrgName ,
                    OrgType ,
                    ParentOrgCode ,
                    ParentOrgName ,
                    ParentOrgType ,
                    UserId ,
                    OrgZaiXianCheLiangShu ,
                    OrgZongCheLiangShu ,
                    OrgShiPinZaiXianCheLiangShu,
                    OrgShiPinZongCheLiangShu,
                    ShiFouLiShiJiLu ,
                    SYS_ChuangJianShiJian ,
                    SYS_ZuiJinXiuGaiShiJian ,
                    SYS_XiTongZhuangTai ,
                    SYS_XiTongBeiZhu ,
                    ParentOrgId ,
                    OrgId
                    )
                    VALUES(
                        NEWID() ,
                        @OrgCode ,
                        @OrgName ,
                        @OrgType ,
                        @ParentOrgCode ,
                        @ParentOrgName ,
                        @ParentOrgType ,
                        @UserId ,
                        @OrgZaiXianCheLiangShu ,
                        @OrgZongCheLiangShu ,
                        @OrgShiPinZaiXianCheLiangShu,
                        @OrgShiPinZongCheLiangShu,
                        @ShiFouLiShiJiLu ,
                        @SYS_ChuangJianShiJian ,
                        @SYS_ZuiJinXiuGaiShiJian ,
                        @SYS_XiTongZhuangTai ,
                        @SYS_XiTongBeiZhu ,
                        @ParentOrgId ,
                        @OrgId
                    )";
                            #endregion
                            dbConnection.Execute(deleteSql, null, transaction);
                            dbConnection.Execute(addHuiZongSql, TOrgCheLiangHuiZongQingKuang, transaction,1800);
                            transaction.Commit();
                            LogHelper.Warn("T_CheLiangJianKongHuiZongQingKuang 作业执行完毕" + DateTime.Now);
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error("T_CheLiangJianKongHuiZongQingKuang 作业执行异常，异常信息：" + e.Message, e);
                            transaction.Rollback();
                        }
                    }
                });

                Task.Run(() => {
                    using (IDbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ToString()))
                    {
                        LogHelper.Warn("T_RYFPJKCLSSOrgJiLu 作业执行开始" + DateTime.Now);
                        dbConnection.Open();
                        IDbTransaction transaction = dbConnection.BeginTransaction();
                        try
                        {
                            #region sql
                            var deleteSql = @" TRUNCATE table DC_GPSDAGL.dbo.T_RYFPJKCLSSOrgJiLu;";
                            var addRYFPJKCLSSOrgJiLuSql = @" INSERT INTO DC_GPSDAGL.dbo.T_RYFPJKCLSSOrgJiLu
                    ( ID ,
                    SysUserId ,
                    OrgCode ,
                    OrgName ,
                    OrgType ,
                    ParentOrgCode ,
                    ParentOrgName ,
                    ParentOrgType ,
                    CheLiangId ,
                    ChePaiHao ,
                    ChePaiYanSe ,
                    CheLiangZhongLei ,
                    ShiFouZaiXian ,
                    ShiPingTouGeShu,
                    SYS_ChuangJianShiJian ,
                    SYS_ZuiJinXiuGaiShiJian ,
                    SYS_XiTongZhuangTai ,
                    SYS_XiTongBeiZhu ,
                    ParentOrgId ,
                    OrgId
                    )
                VALUES
                (
                    NEWID() ,
                    @SysUserId ,
                    @OrgCode ,
                    @OrgName ,
                    @OrgType ,
                    @ParentOrgCode ,
                    @ParentOrgName ,
                    @ParentOrgType ,
                    @CheLiangId ,
                    @ChePaiHao ,
                    @ChePaiYanSe ,
                    @CheLiangZhongLei ,
                    @ShiFouZaiXian ,
                    @ShiPingTouGeShu,
                    @SYS_ChuangJianShiJian ,
                    @SYS_ZuiJinXiuGaiShiJian ,
                    @SYS_XiTongZhuangTai ,
                    @SYS_XiTongBeiZhu ,
                    @ParentOrgId ,
                    @OrgId
                )";
                            #endregion
                            dbConnection.Execute(deleteSql, null, transaction);
                            dbConnection.Execute(addRYFPJKCLSSOrgJiLuSql, TNewOrgCheLiangXinXi, transaction, 1800);
                            transaction.Commit();
                            LogHelper.Warn("T_RYFPJKCLSSOrgJiLu 作业执行完毕" + DateTime.Now);
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error("T_RYFPJKCLSSOrgJiLu 作业执行异常，异常信息：" + e.Message, e);
                            transaction.Rollback();
                        }
                    }

                });

            }
            catch (Exception ex)
            {
                LogHelper.Error($"更新车辆监控汇总情况和人员分配监控车辆所属组织记录异常，异常信息\n {ex.Message}",ex);
                throw ex;
            }
        }
        #endregion

        public class TCheLiang
        {
            public Guid CheLiangId { get; set; }
            public string ChuangJianDanWeiOrgCode { get; set; }
            public string XiaQuShi { get; set; }
            public string ChePaiHao { get; set; }
            public string ChePaiYanSe { get; set; }
            public int? CheLiangZhongLei { get; set; }
            public int? ShiFouZaiXian { get; set; }
            public int? ShiPingTouGeShu { get; set; }
            public string QiYeOrgCode { get; set; }
            public string CheDuiOrgCode { get; set; }
            public Guid? SysUserId { get; set; }
            public int? ParentOrgType { get; set; }
            public int? OrgType { get; set; }
        }

        public class TOrgCheLiang
        {
            public Guid? SysUserId { get; set; }
            public Guid? ParentOrgId { get; set; }
            public string ParentOrgCode { get; set; }
            public string ParentOrgName { get; set; }
            public int? ParentOrgType { get; set; }
            public Guid? OrgId { get; set; }
            public string OrgCode { get; set; }
            public string OrgName { get; set; }
            public int? OrgType { get; set; }
            public Guid CheLiangId { get; set; }
            public string ChePaiHao { get; set; }
            public string ChePaiYanSe { get; set; }
            public int? CheLiangZhongLei { get; set; }
            public string ChuangJianDanWeiOrgCode { get; set; }
            public int? ShiFouZaiXian { get; set; }
            public int? ShiPingTouGeShu { get; set; }

        }

        public class TNewOrgCheLiangXinXi
        {
            public Guid ID { get; set; }
            public Guid? SysUserId { get; set; }
            public Guid? ParentOrgId { get; set; }
            public string ParentOrgCode { get; set; }
            public string ParentOrgName { get; set; }
            public int? ParentOrgType { get; set; }
            public Guid? OrgId { get; set; }
            public string OrgCode { get; set; }
            public string OrgName { get; set; }
            public int? OrgType { get; set; }
            public Guid CheLiangId { get; set; }
            public string ChePaiHao { get; set; }
            public string ChePaiYanSe { get; set; }
            public int? CheLiangZhongLei { get; set; }
            public int? ShiFouZaiXian { get; set; }
            public int? ShiPingTouGeShu { get; set; }
            public string ChuangJianDanWeiOrgCode { get; set; }
            public DateTime? SYS_ChuangJianShiJian { get; set; }
            public DateTime? SYS_ZuiJinXiuGaiShiJian { get; set; }
            public int? SYS_XiTongZhuangTai { get; set; }
            public string SYS_XiTongBeiZhu { get; set; }
        }

        public class TSysOrg
        {
            public Guid? Id { get; set; }

            public Guid? ParentSystemOrganization_Id { get; set; }

            public Guid? SysID { get; set; }

            public string Province { get; set; }

            public string City { get; set; }

            public string District { get; set; }

            public string Town { get; set; }

            public string Village { get; set; }

            public string ManageArea { get; set; }

            public string AdminLevel { get; set; }

            public string OrganizationName { get; set; }

            public string OrganizationCode { get; set; }

            public int? OrganizationType { get; set; }

            public DateTime? SYS_ZuiJinXiuGaiShiJian { get; set; }

            public DateTime? SYS_ChuangJianShiJian { get; set; }
        }

        public class TFenGongSi
        {
            public Guid? Id { get; set; }

            public Nullable<System.Guid> ParentSystemOrganization_Id { get; set; }

            public string OrganizationName { get; set; }

            public string OrganizationCode { get; set; }

            public int? OrganizationType { get; set; }

            public string ManageArea { get; set; }

            public Nullable<System.Guid> GPSId { get; set; }

            public string GPSOrganizationCode { get; set; }

            public string GPSOrganizationName { get; set; }

            public int? GPSOrganizationType { get; set; }

            public string GPSManageArea { get; set; }
        }

        public class OrgTable
        {
            public Guid? ParentId { get; set; }

            public string ParentOrgCode { get; set; }

            public string ParentOrgName { get; set; }

            public int? ParentOrgType { get; set; }

            public Guid? OrgId { get; set; }

            public string OrgCode { get; set; }

            public string OrgName { get; set; }

            public int? OrgType { get; set; }

            public string OrgManageArea { get; set; }

            public string CreatOrganizationCode { get; set; }

            public string CreatOrganizationName { get; set; }

            public int? CreatOrganizationType { get; set; }

            public Guid? CreatOrganizationId { get; set; }
            public DateTime? SYS_ZuiJinXiuGaiShiJian { get; set; }
            public DateTime? SYS_ChuangJianShiJian { get; set; }
            public string SYS_XiTongBeiZhu { get; set; }
            public int? SYS_XiTongZhuangTai { get; set; }

        }

        public class TSysOrgBFCreater
        {
            public Guid? Id { get; set; }

            public Nullable<System.Guid> ParentSystemOrganization_Id { get; set; }

            public string CreatOrganizationCode { get; set; }

            public string CreatOrganizationName { get; set; }

            public int? CreatOrganizationType { get; set; }

            public Guid? CreatOrganizationId { get; set; }
        }

    }
}
