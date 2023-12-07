using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using AutoMapper;
using System.Collections.Generic;
namespace Conwin.GPSDAGL.WebApi.App_Start
{
    public class AutoMapConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(
                configuration =>
                {
                    var assemblyList = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(p => p.FullName.StartsWith("Conwin")).ToList();
                    List<Type> typeList = new List<Type>();
                    foreach (var item in assemblyList)
                    {
                        Type[] typeArray = null;
                        try
                        {
                            typeArray = item.GetTypes();
                        }
                        catch (Exception ex)
                        {
                        }
                        if (typeArray != null && typeArray.Length > 0)
                            foreach (var t in typeArray)
                            {
                                if (typeof(AutoMapper.Profile).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null)
                                {
                                    typeList.Add(t);
                                }
                            }
                    }
                    var profiles = typeList.Select(Activator.CreateInstance)
                     .Cast<AutoMapper.Profile>().ToList();
                    profiles.ForEach(configuration.AddProfile);
                });
        }
    }
}