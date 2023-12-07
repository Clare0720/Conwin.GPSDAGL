using Conwin.Framework.Log4net;
using Conwin.GPSDAGL.Framework.OperationLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework
{
    public class OprateLogHelper
    {
        /// <summary>
        /// 获取两个对象间的值发生变化的描述
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="obj1">变化前的对象</param>
        /// <param name="obj2">变化后的对象</param>
        /// <param name="isDes">是否过滤掉没有[Description]标记的</param>
        /// <returns>字符串</returns>
        public static List<LogUpdateValueDto> GetObjCompareString<T>(T obj1, T obj2, bool isDes) where T : new()
        {
            List<LogUpdateValueDto> updateList = new List<LogUpdateValueDto>();
            try
            {
                string res = string.Empty;
                if (obj1 == null || obj2 == null)
                {
                    return new List<LogUpdateValueDto>();
                }
                var properties =
                    from property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    select property;

                string objVal1 = string.Empty;
                string objVal2 = string.Empty;

                foreach (var property in properties)
                {
                    var ingoreCompare = Attribute.GetCustomAttribute(property, typeof(IngoreCompareAttribute));
                    if (ingoreCompare != null)
                    {
                        continue;
                    }

                    objVal1 = property.GetValue(obj1, null) == null ? string.Empty : property.GetValue(obj1, null).ToString();
                    objVal2 = property.GetValue(obj2, null) == null ? string.Empty : property.GetValue(obj2, null).ToString();

                    string des = string.Empty;
                    DescriptionAttribute descriptionAttribute = ((DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)));
                    if (descriptionAttribute != null)
                    {
                        des = ((DescriptionAttribute)Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute))).Description;// 属性值
                    }
                    if (isDes && descriptionAttribute == null)
                    {
                        continue;
                    }
                    if (!objVal1.Equals(objVal2) && property.GetValue(obj1, null) != property.GetValue(obj2, null))
                    {

                        var propertyName = string.IsNullOrEmpty(des) ? property.Name : des;
                        LogUpdateValueDto updateModel = new LogUpdateValueDto
                        {
                            AttributesName = propertyName,
                            OldValue = objVal1,
                            NewValue = objVal2
                        };
                        updateList.Add(updateModel);
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("对比实体差异出错" + ex.Message, ex);
                return new List<LogUpdateValueDto>();
            }
            return updateList;
        }



        public static OperLogSystemName GetSysTemName()
        {
            try
            {


                var sysId = System.Configuration.ConfigurationManager.AppSettings["WEBAPISYSID"];
                sysId = sysId.ToUpper();
                OperLogSystemName sysTypeName = OperLogSystemName.清远市交通运输局两客一危营运车辆主动安全防控平台;
                switch (sysId)
                {
                    case "C6380E44-F83F-A921-7174-5B6A8565BB4E":
                        sysTypeName = OperLogSystemName.清远市交通运输局两客一危营运车辆主动安全防控平台;
                        break;
                    case "6AF42885-09BB-786C-E652-5D53A9D28280":
                        sysTypeName = OperLogSystemName.广州市道路运输行业安全信息服务平台;
                        break;
                    case "956534C2-ABBC-F3B9-ACD9-75DB8C99CF85":
                        sysTypeName = OperLogSystemName.湛江市道路运输动态安全监管平台;
                        break;
                }
                return sysTypeName;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取系统组织名称出错" + ex.Message, ex);
                return OperLogSystemName.清远市交通运输局两客一危营运车辆主动安全防控平台;
            }
        }

    }




    /// <summary>
    /// 加入些特性后，在实体差异比较中会忽略该属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IngoreCompareAttribute : Attribute
    {
        public IngoreCompareAttribute()
        {
            Flag = true;
        }

        public bool Flag { get; set; }
    }

    public class LogUpdateValueDto
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributesName { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }

    }


}
