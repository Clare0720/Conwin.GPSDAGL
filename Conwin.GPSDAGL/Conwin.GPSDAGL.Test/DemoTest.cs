//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Practices.ServiceLocation;
//using Conwin.GPSDAGL.Services.Dtos;
//using Conwin.GPSDAGL.Services;
//using System.Collections.Generic;
//using Conwin.Framework.WcfCore.Extensions;
//using System.Linq;
//using Conwin.EntityFramework.Extensions;
//using Conwin.EntityFramework;
//using Conwin.GPSDAGL.ServiceAgent;

//namespace Conwin.GPSDAGL.Tests
//{
//    [TestClass]
//    public class PermissionTest : TestBase
//    {
//        public PermissionTest()
//            : base(false)
//        {

//        }

//        [TestMethod]
//        public void 权限_CURD_Test()
//        {
//            var provider = ServiceProvider.Create();

//            //新建一个权限对象
//            var newItem = new PermissionDto
//            {
//                Id = Guid.NewGuid().ToString(),
//                SysID = Guid.NewGuid().ToString(),
//                ApplicationID = new Guid(),
//                Name = "Test",
//                Code = "Test"
//            };
//            provider.PermissionSvc.Insert(newItem);
//            var dbRecord = provider.PermissionSvc.Get(newItem.Id).Result;
//            Assert.IsNotNull(dbRecord);
//            Assert.AreEqual(newItem.Id, dbRecord.Id);
//        }
//    }
//}
