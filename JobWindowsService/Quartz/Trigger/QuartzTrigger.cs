using Conwin.Framework.Log4net;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.JobWindowsService.Quartz.QFactory
{
    public class QuartzTrigger
    {
        /// <summary>
        /// 获取CronTrigger
        /// </summary>
        /// <returns></returns>
        public static ITrigger GetCronTrigger(string triggerName, string cron)
        {

            //新建
            ITrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity(triggerName, "group1")
                .StartAt(DateTime.Now.AddSeconds(0))
                .WithCronSchedule(cron)
                .Build();

            return trigger;
        }

        /// <summary>
        /// 获取立即执行的Trigger
        /// </summary>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        public static ITrigger GetSlmpleTrigger(string triggerName)
        {
            try
            {
                //新建
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerName, "group1")
                    .StartNow()
                    .Build();

                return trigger;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"初始化Trigger失败！\n Trigger:'{triggerName}' \n 错误信息: {ex.Message}", ex);
                return null;
            }

        }
    }
}
