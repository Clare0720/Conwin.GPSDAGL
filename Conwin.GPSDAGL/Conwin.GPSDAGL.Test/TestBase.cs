using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using AutoMapper;
using UnityConfiguration;
using Conwin.EntityFramework;
using Conwin.GPSDAGL.Services;

namespace Conwin.GPSDAGL.Tests
{
    [TestClass]
    public class TestBase
    {
        private bool clearData;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearData">如果为true，则清除所有数据</param>
        public TestBase(bool clearData = true)
        {
            this.clearData = clearData;
        }

        [TestInitialize]
        public void Init()
        {
            if (!isInited)
            {
                InitPrivate();
                isInited = true;
            }

            //注册服务类型
            var container = Conwin.Unity.UnityConfig.GetConfiguredContainer();
            RegisterTypes(container);
        }

        private static bool isInited = false;


        private void InitPrivate()
        {

            var container = Conwin.Unity.UnityConfig.GetConfiguredContainer();


            var provider = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => provider);

            //fix conwin bug
            //IocFactory.SetContainer(container);

            Mapper.Initialize(
               config =>
              AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.StartsWith("Conwin")).ForEach(p => p
                       .GetTypes()
                       .Where(
                           type =>
                           typeof(Profile).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
                       .Select(Activator.CreateInstance)
                       .Cast<Profile>()
                       .ForEach(config.AddProfile)));

            //container.RegisterType(typeof(IEntityRepository<>), typeof(EntityRepository<>), new HierarchicalLifetimeManager());
            DbContextManager.InitStorage(new SimpleDbContextStorage());
            DbContextManager.Init("DefaultDb", new[] { "Conwin.GPSDAGL.EntityMaps" });
            //DbContextManager.Init("WorkflowConnectionString", new[] { "Conwin.Framework.Workflow.EntityMaps" },null,true);

            if (this.clearData)
            {
                ClearData();
            }

        }

        private void ClearData()
        {
            //取消以下注释并替换相关代码，以清除数据数据
            //ServiceLocator.Current.GetInstance<IPermissionService>().DeleteAll();
            //ServiceLocator.Current.GetInstance<IPermissionGroupService>().DeleteAll();
            //ServiceLocator.Current.GetInstance<IPermissionAssignmentService>().DeleteAll();
            //ServiceLocator.Current.GetInstance<IPermissionRegisterInfoService>().DeleteAll();
        }


        protected virtual void RegisterTypes(IUnityContainer container)
        {

        }
    }
}
