using Autofac;
using Autofac.Integration.WebApi;
using Conwin.Framework.CommunicationProtocol;
using Conwin.GPSDAGL.Services.Common;
using Conwin.GPSDAGL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Http;

namespace Conwin.GPSDAGL.WebApi.Helper
{
    public class AutoTaskHelper
    {
        private ICheLiangService _VehicleService;
        //定时间隔（分钟）-从配置文件中读取

        static object lockobj = new object();
        static bool taskStart = false;
        private System.Timers.Timer taskTimer;
        public AutoTaskHelper()
        {
            var interval = 10;//10分钟
            int.TryParse(ConfigurationManager.AppSettings["TimerInterval"], out interval);
            taskTimer = new Timer { Interval = 1000 * 60 * interval };
            var resolver = GlobalConfiguration.Configuration.DependencyResolver as AutofacWebApiDependencyResolver;
            this._VehicleService = resolver.Container.Resolve<ICheLiangService>();
        }
        public void StartTask()
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    if (taskStart)
                        return;
                    taskTimer.Elapsed += TaskTimer_Elapsed;
                    taskTimer.Start();
                    taskStart = true;
                }
            });
        }
        private void TaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExcuteTask();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ExcuteTask()
        {
            lock (lockobj)
            {
                var url = ConfigurationManager.AppSettings["LocalApiUrl"] + "/api/GPSDAGL/CheLiang/AuditAuto";
                var reqData = new CWRequest {body = null, publicrequest = new CWPublicRequest()};
                var resultJson = new ServiceHttpHelper().Post(url, reqData);
                var dResult = JsonConvert.DeserializeObject<CWResponse>(resultJson);
            }
        }
    }
}