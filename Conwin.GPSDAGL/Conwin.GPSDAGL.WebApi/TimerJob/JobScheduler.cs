using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conwin.GPSDAGL.WebApi.TimerJob
{
    public class JobScheduler
    {
        public static void Start()
        {
            //调度器工厂
            ISchedulerFactory factory = new StdSchedulerFactory();
            //调度器
            IScheduler scheduler = factory.GetScheduler();
            scheduler.GetJobGroupNames();
            //创建任务
            IJobDetail job = JobBuilder.Create<JiaShiYuanTimerJob>().Build();
            //创建触发器
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("JiaShiYuanTimeTrigger", "JiaShiYuanTimeGroup").WithSimpleSchedule(t => t.WithIntervalInHours(24).RepeatForever()).Build();
            //添加任务及触发器至调度器中
            scheduler.ScheduleJob(job, trigger);
            //启动
            scheduler.Start();
        }
    }
}