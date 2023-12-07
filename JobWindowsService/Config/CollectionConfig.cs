using Conwin.GPSDAGL.JobWindowsService.Config.ConfigModel;
using Conwin.GPSDAGL.JobWindowsService.Quartz;
using Conwin.Framework.Log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Conwin.GPSDAGL.JobWindowsService.Config
{
    public class CollectionConfig
    {
        private static object _lock = new object();
        static CollectionConfig()
        {
            InitWatcher();
        }

        public static bool ExecuteBoth
        {
            get
            {
                var result = System.Configuration.ConfigurationManager.AppSettings["ExecuteBoth"];
                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }
                else
                {
                    try
                    {
                        return Convert.ToBoolean(result);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        private static void InitWatcher()
        {
            try
            {
                var NativeConfigWatcher = new FileSystemWatcher($"{GetExePath()}/Config/Common/", "NativeConfig.json");
                NativeConfigWatcher.EnableRaisingEvents = true; //开启监听功能
                NativeConfigWatcher.Changed += OnChange;
                //读取NativeConfig
                string NativeConfigStr = File.ReadAllText($"{GetExePath()}/Config/Common/NativeConfig.json");
                var NativeConfig = JsonConvert.DeserializeObject<NativeConfig>(NativeConfigStr);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message,ex);
            }
            
        }

        public static NativeConfig NativeConfig { get; set; }
        public static CustomConfig CustomConfig { get; set; }

        public static void LoadConfig()
        {
            IList<QuartzConfig> OldQuartzConfig = CustomConfig?.QuartzConfig;
            try
            {
                Thread.Sleep(1000);
                lock (_lock)
                {
                    //读取NativeConfig
                    string NativeConfigStr = File.ReadAllText($"{GetExePath()}/Config/Common/NativeConfig.json");
                    NativeConfig = JsonConvert.DeserializeObject<NativeConfig>(NativeConfigStr);
                    CustomConfig = NativeConfig.CustomConfig;

                    //修改配置的重置Quartz
                    //如果是第一次的话则跳过
                    if (OldQuartzConfig != null)
                    {
                        var oqcJson = JsonConvert.SerializeObject(OldQuartzConfig);
                        var qcJson = JsonConvert.SerializeObject(CustomConfig.QuartzConfig);
                        if (oqcJson != qcJson)//如果不等则重启
                        {
                            var scheduler = QuartzHelper.GetScheduler();
                            scheduler.Shutdown();
                            QuartzHelper.InitQuartz();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message,ex);
            }
            
        }

        public static void OnChange(object sender, FileSystemEventArgs e)
        {
            LoadConfig();
        }

        /// <summary>
        /// 获取运行时路径
        /// </summary>
        /// <returns></returns>
        public static string GetExePath()
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return pathToContentRoot;
        }
    }
}
