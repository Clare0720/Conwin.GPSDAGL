using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.JobWindowsService.Common
{
    public class DapperSqlHelper
    {
        private static string sqlConnStr = ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString;//数据库连接配置文件
        


        #region  + static List<T> Query<T>(string sql, object param = null)
        public static List<T> Query<T>(string sql, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnStr))
            {
                connection.Open();
                return connection.Query<T>(sql, param).ToList();
            }
        }

        public static List<T> Query<T>(string ConnStr,string sql, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                return connection.Query<T>(sql, param).ToList();
            }
        }
        #endregion

        #region  + static T QuerySingle<T>(string sql, object param = null)
        public static T QuerySingle<T>(string sql, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnStr))
            {
                connection.Open();
                return connection.Query<T>(sql, param).FirstOrDefault();
            }
        }
        public static T QuerySingle<T>(string ConnStr, string sql, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                connection.Open();
                return connection.Query<T>(sql, param).FirstOrDefault();
            }
        }
        #endregion


        #region ExecuteScalar
        public static T ExecuteScalar<T>(string sql, object param = null)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnStr))
            {
                connection.Open();
                return connection.ExecuteScalar<T>(sql, param);
            }
        }
       
        #endregion

        #region --Execute--

        #region + static int Execute(string sql, object param = null, IDbTransaction transaction = null)
        public static int Execute(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return transaction.Connection.Execute(sql, param, transaction);
            else
            {
                using (SqlConnection connection = new SqlConnection(sqlConnStr))
                {
                    connection.Open();
                    return connection.Execute(sql, param, transaction);
                }
            }
        }

        public static int Execute(string ConnStr, string sql, object param = null, IDbTransaction transaction = null)
        {
            if (transaction != null)
                return transaction.Connection.Execute(sql, param, transaction);
            else
            {
                using (SqlConnection connection = new SqlConnection(ConnStr))
                {
                    connection.Open();
                    return connection.Execute(sql, param, transaction);
                }
            }
        }
        #endregion

        #region + static void Execute(Func<IDbTransaction, bool> excuteFunc)
        public static void Execute(Func<IDbTransaction, bool> excuteFunc)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnStr))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    bool result = false;
                    if (excuteFunc != null)
                    {
                        result = excuteFunc(transaction);
                    }
                    if (result)
                        transaction.Commit();
                    else transaction.Rollback();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        

        #region --ExecuteInsert--

        #region + static void ExecuteInsert<T>(T entity, IDbTransaction transaction = null)
        public static int ExecuteInsert<T>(T entity, IDbTransaction transaction = null)
        {
            string sql = GenerateEntityInsertSql(entity);
            int affectedrows = 0;
            if (transaction != null)
                affectedrows = transaction.Connection.Execute(sql, entity, transaction);
            else
            {
                using (var connection = new SqlConnection(sqlConnStr))
                {
                    connection.Open();
                    affectedrows = connection.Execute(sql, entity, transaction);
                }
            }
            return affectedrows;
        }
        #endregion

        #region + static void ExecuteInsert<T>(List<T> entityList, IDbTransaction transaction = null)
        public static void ExecuteInsert<T>(List<T> entityList, IDbTransaction transaction = null)
        {

            var properties = typeof(T).GetProperties();
            var tableName = ((typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true))[0] as TableNameAttribute).Value;

            IDbConnection connection = null;
            bool transactionCommitEnable = false;
            try
            {
                if (transaction != null)
                    connection = transaction.Connection;
                else
                {
                    connection = new SqlConnection(sqlConnStr);
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    transactionCommitEnable = true;
                }
                int rowIndex = 1;
                foreach (var entity in entityList)
                {
                    string sql = GenerateEntityInsertSql(entity);
                    var affectedrows = connection.Execute(sql.ToString(), entity, transaction);
                    if (affectedrows <= 0)
                    {
                        throw new Exception(string.Format("因第{0}条数据写入失败,任务执行已中断", rowIndex));
                    }
                    rowIndex++;
                }
                if (transactionCommitEnable)
                {
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                if (transactionCommitEnable)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
        }
        #endregion

        #region - static string GenerateEntityInsertSql<T>(T entity)
        private static string GenerateEntityInsertSql<T>(T entity)
        {
            var properties = typeof(T).GetProperties();
            var tableName = ((typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true))[0] as TableNameAttribute).Value;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldsSql = new StringBuilder();
            StringBuilder valuesSql = new StringBuilder();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(FieldNameAttribute), true);
                if (attribute.Count() <= 0 || IsIdentityField(property))//过滤非数据库字段的数据与标识列字段
                {
                    continue;
                }
                var fieldName = (attribute[0] as FieldNameAttribute).Value;
                fieldsSql.Append(string.Format("{0},", fieldName));
                valuesSql.Append(string.Format("@{0},", property.Name));
            }
            if (fieldsSql.Length <= 0)
            {
                throw new Exception("实体没有定义数据库字段映射关系");
            }
            sql.Append(string.Format(@"INSERT INTO {0}({1}) VALUES({2}); ", tableName, fieldsSql.ToString().TrimEnd(','), valuesSql.ToString().TrimEnd(',')));
            return sql.ToString();
        }
        #endregion

        #endregion

        #region --ExecuteUpdate--

        #region + static int ExecuteUpdate<T>(T entity,List<string> updatePropertyNames=null, IDbTransaction transaction = null)
        public static int ExecuteUpdate<T>(T entity, List<string> updatePropertyNames = null, IDbTransaction transaction = null)
        {
            string sql = GenerateEntityUpdateSql(entity, updatePropertyNames);
            int affectedrows = 0;
            if (transaction != null)
                affectedrows = transaction.Connection.Execute(sql, entity, transaction);
            else
            {
                using (var connection = new SqlConnection(sqlConnStr))
                {
                    connection.Open();
                    affectedrows = connection.Execute(sql, entity, transaction);
                }
            }
            return affectedrows;
        }
        #endregion


        #region - static string GenerateEntityUpdateSql<T>(T entity,List<string> updatePropertyNames=null)
        private static string GenerateEntityUpdateSql<T>(T entity, List<string> updatePropertyNames = null)
        {
            var properties = typeof(T).GetProperties();
            var tableNameAttribute = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true);
            if (tableNameAttribute.Count() <= 0)
            {
                throw new Exception("实体未定义表名映射关系");
            }
            var tableName = (tableNameAttribute[0] as TableNameAttribute).Value;
            var tableKeyAttribute = typeof(T).GetCustomAttributes(typeof(TableKeyAttribute), true);
            List<string> tableKeys = null;
            Dictionary<string, string> tableKeysDic = new Dictionary<string, string>();//数据库主键字段名称与实体属性名称映射字典
            if (tableKeyAttribute.Count() > 0)
            {
                tableKeys = tableKeyAttribute.Cast<TableKeyAttribute>().Select(m => m.Value).ToList();
            }
            if (tableKeys == null || tableKeys.Count <= 0)
            {
                throw new Exception("实体未定义主键映射关系");
            }
            
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldsSql = new StringBuilder();
            StringBuilder whereSql = new StringBuilder();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(FieldNameAttribute), true);
                if (attribute.Count() <= 0 || IsIdentityField(property))//过滤非数据库字段的数据与标识列字段
                {
                    continue;
                }
                var fieldName = (attribute[0] as FieldNameAttribute).Value;
                if (tableKeys.Contains(fieldName))
                {
                    if (whereSql.Length <= 0)
                        whereSql.Append(string.Format(" {0}=@{1} ", fieldName, property.Name));
                    else whereSql.Append(string.Format(" AND {0}=@{1} ", fieldName, property.Name));
                    continue;
                }
                if (updatePropertyNames != null && updatePropertyNames.Count > 0 && !updatePropertyNames.Contains(property.Name))
                {
                    continue;
                }

                fieldsSql.Append(string.Format("{0}=@{1},", fieldName, property.Name));
            }
            if (fieldsSql.Length <= 0)
            {
                throw new Exception("实体没有定义待更新的数据库字段映射关系");
            }

            if (whereSql.Length <= 0)
            {
                throw new Exception("where语句不可为空");
            }
            sql.Append(string.Format(@"UPDATE {0} SET {1} WHERE {2}; ", tableName, fieldsSql.ToString().TrimEnd(','), whereSql.ToString()));
            return sql.ToString();
        }
        #endregion
        #endregion

        /// <summary>
        /// 是否是标识列
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsIdentityField(PropertyInfo property)
        {
            //获取数据库表标识列（如自增列等不可insert和update的列）
            var attribute = property.GetCustomAttributes(typeof(IdentityFieldAttribute), true);

            return attribute.Count() > 0;

        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string Value { get; private set; }
        public TableNameAttribute(string tableName)
        {
            Value = tableName;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldNameAttribute : Attribute
    {
        public string Value { get; private set; }
        public FieldNameAttribute(string fieldName)
        {
            Value = fieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityFieldAttribute : Attribute
    {
        public string Value { get; private set; }
        public IdentityFieldAttribute(string fieldName)
        {
            Value = fieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableKeyAttribute : Attribute
    {
        public string Value { get; private set; }
        public TableKeyAttribute(string fieldName)
        {
            Value = fieldName;
        }
    }

    
}
