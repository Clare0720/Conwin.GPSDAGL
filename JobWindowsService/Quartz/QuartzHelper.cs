using Quartz;
using Conwin.GPSDAGL.JobWindowsService.Quartz.QFactory;
using Conwin.GPSDAGL.JobWindowsService.Config;
using System;
using Conwin.Framework.Log4net;

namespace Conwin.GPSDAGL.JobWindowsService.Quartz
{
    public class QuartzHelper
    {
        #region Quartz 的基本功能

        private static IScheduler _scheduler;

        public static IScheduler GetScheduler()
        {
            return _scheduler;
        }

        /// <summary>
        /// 开启Quartz
        /// </summary>
        public static IScheduler StartQuartz()
        {
            var scheduler = QuartzScheduler.GetScheduler();
            scheduler.Start();
            return scheduler;
        }

        /// <summary>
        /// 保持激活状态
        /// </summary>
        public static IScheduler KeepAliveQuartz()
        {
            //var scheduler = QuartzScheduler.GetScheduler();
            if (!_scheduler.IsStarted) //如果检测到未开启，则要重新开启
            {
                _scheduler.Start();
                return _scheduler;
            }
            else
            {
                return _scheduler;
            }
        }

        #endregion

        #region Quartz 扩展

        /// <summary>
        /// 初始化Quartz
        /// </summary>
        public static void InitQuartz()
        {
            try
            {
                //开启调度程序
                _scheduler = StartQuartz();

                //读取配置生成job
                var quartConfig = CollectionConfig.CustomConfig.QuartzConfig;
                for (int i = 0; i < quartConfig.Count; i++)
                {
                    var qc = quartConfig[i];
                    var type = Type.GetType(qc.QuartzJobClass);
                    IJobDetail job = JobBuilder.Create(type)
                               .WithIdentity($"Trigger{i}", "group1").Build();
                    switch (qc.QuartzType)
                    {
                        case "cron":
                            var trigger = QuartzTrigger.GetCronTrigger($"Trigger{i}", qc.QuartzCron);
                            _scheduler.ScheduleJob(job, trigger);
                            break;
                        case "startnow":
                            var trigger1 = QuartzTrigger.GetSlmpleTrigger($"Trigger{i}");
                            _scheduler.ScheduleJob(job, trigger1);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("初始化Quartz异常", ex);
                throw ex;
            }        
        }
        #endregion
    }
}
