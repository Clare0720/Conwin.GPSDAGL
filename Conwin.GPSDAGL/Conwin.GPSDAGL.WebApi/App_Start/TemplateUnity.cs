using Conwin.GPSDAGL.Entities.Repositories;
using Conwin.GPSDAGL.Repository;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.Services.Services;
using Conwin.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conwin.GPSDAGL.WebApiEx.App_Start
{
    public class TemplateUnity : ConwinRegistry
    {
        public TemplateUnity()
        {
            Scan(scan => scan.Exclude((t) => t == typeof(EntityRepository<>)));

            Register(typeof(IEntityRepository<>), typeof(EntityRepository<>), new HierarchicalLifetimeManager());

            Register(typeof(IQiYeDangAnService), typeof(QiYeDangAnService), new HierarchicalLifetimeManager());
        }
    }
}