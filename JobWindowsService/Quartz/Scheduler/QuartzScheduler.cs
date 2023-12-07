using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.JobWindowsService.Quartz.QFactory
{
    public static class QuartzScheduler
    {
        //private static StdSchedulerFactory _factory;
        private static StdSchedulerFactory GetSchedulerFactory()
        {
            //if (_factory == null)
            //{
                var _factory = new StdSchedulerFactory();
                NameValueCollection nvc = new NameValueCollection()
                {
                    ["quartz.scheduler.instanceName"] = $"UnisQuartz{Guid.NewGuid()}",
                    ["quartz.threadPool.threadCount"] = "20"
                };
                _factory.Initialize(nvc);
                return _factory;
            //}
            //else
            //{
            //    return _factory;
            //}
        }

        /// <summary>
        /// 获取Scheduler
        /// </summary>
        /// <returns></returns>
        public static IScheduler GetScheduler()
        {
            var factory = GetSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler();
            return scheduler;
        }
    }
}
