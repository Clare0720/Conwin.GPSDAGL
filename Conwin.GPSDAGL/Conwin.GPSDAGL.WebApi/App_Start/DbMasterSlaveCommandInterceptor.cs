using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;

namespace Conwin.GPSDAGL.WebApi.App_Start
{
    public class DbMasterSlaveCommandInterceptor : DbCommandInterceptor
    {
        private Lazy<string> masterConnectionString = new Lazy<string>(() => ConfigurationManager.AppSettings["MasterConnectionString"]);
        private Lazy<string> slaveConnectionString = new Lazy<string>(() => ConfigurationManager.AppSettings["SlaveConnectionString"]);

        public string MasterConnectionString
        {
            get { return this.masterConnectionString.Value; }
        }

        public string SlaveConnectionString
        {
            get { return this.slaveConnectionString.Value; }
        }


        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.UpdateConnectionStringIfNeed(interceptionContext, this.SlaveConnectionString);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.UpdateConnectionStringIfNeed(interceptionContext, this.SlaveConnectionString);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            this.UpdateConnectionStringIfNeed(interceptionContext, this.MasterConnectionString);
        }


        private void UpdateConnectionStringIfNeed(DbInterceptionContext interceptionContext, string connectionString)
        {
            foreach (var context in interceptionContext.DbContexts)
            {
                this.UpdateConnectionStringIfNeed(context.Database.Connection, connectionString);
            }
        }

        /// <summary>
        /// 此处改进了对连接字符串的修改判断机制，确认只在 <paramref name="conn"/> 所使用的连接字符串不等效于 <paramref name="connectionString"/> 的情况下才需要修改。
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="connectionString"></param>
        private void UpdateConnectionStringIfNeed(DbConnection conn, string connectionString)
        {
            if (this.ConnectionStringCompare(conn, connectionString))
            {
                ConnectionState state = conn.State;
                if (state == ConnectionState.Open)
                    conn.Close();

                conn.ConnectionString = connectionString;

                if (state == ConnectionState.Open)
                    conn.Open();
            }
        }

        private bool ConnectionStringCompare(DbConnection conn, string connectionString)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(conn);

            DbConnectionStringBuilder a = factory.CreateConnectionStringBuilder();
            a.ConnectionString = conn.ConnectionString;

            DbConnectionStringBuilder b = factory.CreateConnectionStringBuilder();
            b.ConnectionString = connectionString;

            return a.EquivalentTo(b);
        }
    }
}