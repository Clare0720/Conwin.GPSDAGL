using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.JobWindowsService.Config.ConfigModel
{
    /// <summary>
    /// NativeConfig配置
    /// </summary>
    public class NativeConfig
    {
        /// <summary>
        /// 自定义配置
        /// </summary>
        public CustomConfig CustomConfig { get; set; }

    }

    /// <summary>
    /// 自定义配置
    /// </summary>
    public class CustomConfig
    {
        public IList<QuartzConfig> QuartzConfig { get; set; }
    }


    public class QuartzConfig
    {
        public string QuartzJobName { get; set; }
        public string QuartzType { get; set; }
        public string QuartzCron { get; set; }
        public string QuartzJobClass { get; set; }
    }

   
}
