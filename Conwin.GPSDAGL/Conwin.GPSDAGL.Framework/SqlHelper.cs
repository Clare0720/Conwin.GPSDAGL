using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Conwin.GPSDAGL.Framework
{
	public class SqlHelper
    {

        private string sqlConnStr = "";

        public SqlHelper(string sqlConn)
        {
            sqlConnStr = sqlConn;
        }

        #region 获取第一行第一列数据
        public object ExecuteScalar(string sql, params SqlParameter[] P)//用于获得某种类型的查询结果，例如某个数据有几条
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, sqlConn))
                {
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P.Select(x => ((ICloneable)x).Clone()).ToArray()); //传入参数化查询参数
                    }
                    sqlConn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion


        #region 执行无返回值操作
        /// <summary>
        /// 执行无返回值操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params SqlParameter[] P)//插入，删除，更改数据之类的操作，返回受影响的记录条数
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, sqlConn))
                {
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P);
                    }
                    sqlConn.Open();
                    return cmd.ExecuteNonQuery(); //返回值为受影响的行数
                }
            }
        }
        #endregion

        #region 取得数据表
        public DataSet DataSet(string sql, params SqlParameter[] P)
        {
            DataSet sqlDataSet = new DataSet();
            using (SqlDataAdapter sqlDA = new SqlDataAdapter(sql, sqlConnStr))
            {
                if (P != null)
                {
                    sqlDA.SelectCommand.Parameters.AddRange(P);  //传入参数化查询参数
                }
                sqlDA.Fill(sqlDataSet);
            }
            return sqlDataSet;
        }

        public DataTable DataTable(string sql, params SqlParameter[] P)
        {
            DataSet sqlDataSet = new DataSet();
            using (SqlDataAdapter sqlDA = new SqlDataAdapter(sql, sqlConnStr))
            {
                if (P != null)
                {
                    sqlDA.SelectCommand.Parameters.AddRange(P);  //传入参数化查询参数
                }
                sqlDA.Fill(sqlDataSet);
            }
            return sqlDataSet.Tables[0];
        }

        public DataTable DataTable(string sql, string sqlConnStr, SqlParameter[] P)
        {
            DataSet sqlDataSet = new DataSet();
            using (SqlDataAdapter sqlDA = new SqlDataAdapter(sql, sqlConnStr))
            {
                if (P != null)
                {
                    sqlDA.SelectCommand.Parameters.AddRange(P);  //传入参数化查询参数
                }
                sqlDA.Fill(sqlDataSet);
            }
            return sqlDataSet.Tables[0];
        }

        #endregion

        #region 取得数据表
        public DataSet DataSet(string sql, string sqlConnStr2, params SqlParameter[] P)
        {
            DataSet sqlDataSet = new DataSet();
            using (SqlDataAdapter sqlDA = new SqlDataAdapter(sql, sqlConnStr2))
            {
                if (P != null)
                {
                    sqlDA.SelectCommand.Parameters.AddRange(P);  //传入参数化查询参数
                }
                sqlDA.Fill(sqlDataSet);
            }
            return sqlDataSet;
        }
        #endregion

        #region 取得数据表
        //public static DataSet dataset(string sql, params AppendParameter[] P)
        //{
        //    DataSet sqlDataSet = new DataSet();
        //    using (SqlDataAdapter sqlDA = new SqlDataAdapter(sql, sqlConnStr))
        //    {
        //        if (P != null)
        //        {
        //            sqlDA.SelectCommand.Parameters.AddRange(P);  //传入参数化查询参数
        //        }
        //        sqlDA.Fill(sqlDataSet);
        //    }
        //    return sqlDataSet;
        //}
        #endregion

        #region  逐行读取数据
        public SqlDataReader Reader(string sql, params SqlParameter[] P)
        {
            SqlConnection sqlConn = new SqlConnection(sqlConnStr);
            try
            {

                using (SqlCommand cmd = new SqlCommand(sql, sqlConn))
                {
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P);//传入可选参数params AppendParameter[] P
                    }
                    sqlConn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)    //读取出现异常时的收到释放资源
            {
                sqlConn.Dispose();
                throw ex;
            }
        }
        #endregion

        

        #region 批量执行语句，执行事务


        /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public int ExecuteSqlTran(List<string> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 0)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        #endregion

        #region 存储过程

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。 忽略其他列或行。
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public object ExecuteStoredProcName(string storedProcName, params SqlParameter[] P)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P); //传入参数化查询参数
                    }
                    sqlConn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// 执行存储过程，并返回输出参数
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public int ExecuteStoredProcNameWithOutput(string storedProcName, ref SqlParameter[] P)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P); //传入参数化查询参数
                    }
                    sqlConn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcNameDataSet(string storedProcName, string ConnStr, params SqlParameter[] P)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P); //传入参数化查询参数
                    }
                    sqlConn.Open();

                    DataSet sqlDataSet = new DataSet();
                    SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                    sqlDA.Fill(sqlDataSet);

                    return sqlDataSet;
                }
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcNameDataSet(string storedProcName, params SqlParameter[] P)
        {
            using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcName, sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (P != null)
                    {
                        cmd.Parameters.AddRange(P); //传入参数化查询参数
                    }
                    sqlConn.Open();

                    DataSet sqlDataSet = new DataSet();
                    SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                    sqlDA.Fill(sqlDataSet);

                    return sqlDataSet;
                }
            }
        }

        #endregion


    }
}
