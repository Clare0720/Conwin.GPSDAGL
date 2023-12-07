using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Entities;
using Conwin.GPSDAGL.JobWindowsService.Common;
using Conwin.GPSDAGL.JobWindowsService.Config;
using Conwin.GPSDAGL.JobWindowsService.Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JobWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();

            //初始化配置
            //初始化读取自定义配置
            try
            {
                CollectionConfig.LoadConfig();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"读取自定义配置失败！\n {ex.Message}");
                throw ex;
            }
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.Warn("OnStart");
            QuartzHelper.InitQuartz();
        }


        protected override void OnStop()
        {
        }

      

    }
}
