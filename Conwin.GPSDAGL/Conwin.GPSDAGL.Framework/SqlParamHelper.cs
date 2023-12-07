using System.Collections.Generic;
using System.Data.SqlClient;

namespace Conwin.GPSDAGL.Framework
{
	public class SqlParamHelper
	{
		private int paramIndex = 0;

		/// <summary>
		/// 生成where子句 数组
		/// </summary>
		/// <param name="where">where子句 引用传递</param>
		/// <param name="parameters">参数 引用传递</param>
		/// <param name="tableName">表名（别名）可传入null或空字符串</param>
		/// <param name="columnName">字段名</param>
		/// <param name="parameterValue">参数值</param>
		public void AppendParameterIn<T>(ref string where, ref List<SqlParameter> parameters, string tableName, string columnName, IEnumerable<T> parameterValue)
		{
			string tableName_ = string.IsNullOrWhiteSpace(tableName) ? string.Empty : $"{tableName}.";
			string s = "";
			foreach (T v in parameterValue)
			{
				string parameterName = GetParameterName();
				if (string.IsNullOrWhiteSpace(s))
				{
					s += $"{parameterName}";
				}
				else
				{
					s += $",{parameterName}";
				}
				parameters.Add(new SqlParameter(parameterName,v));
			}
			if (string.IsNullOrWhiteSpace(where))
			{
				where += $"where {tableName_}{columnName} in ({s})";
			}
			else
			{
				where += $" AND {tableName_}{columnName} in ({s})";
			}

		}


		/// <summary>
		/// 生成where子句 相等
		/// </summary>
		/// <param name="where">where子句 引用传递</param>
		/// <param name="parameters">参数 引用传递</param>
		/// <param name="tableName">表名（别名）可传入null或空字符串</param>
		/// <param name="columnName">字段名</param>
		/// <param name="condition">条件</param>
		/// <param name="parameterValue">参数值</param>
		public void AppendParameter(ref string where, ref List<SqlParameter> parameters, string tableName, string columnName, string condition, object parameterValue)
		{
			string parameterName = GetParameterName();
			string tableName_ = string.IsNullOrWhiteSpace(tableName) ? string.Empty : $"{tableName}.";
			if (string.IsNullOrWhiteSpace(where))
			{
				where += $"where {tableName_}{columnName} {condition} {parameterName}";
			}
			else
			{
				where += $" AND {tableName_}{columnName} {condition} {parameterName}";
			}
			parameters.Add(new SqlParameter($"{parameterName}", parameterValue));
		}

		/// <summary>
		/// 生成where子句 相等
		/// </summary>
		/// <param name="where">where子句 引用传递</param>
		/// <param name="parameters">参数 引用传递</param>
		/// <param name="tableName">表名（别名）</param>
		/// <param name="columnName">字段名</param>
		/// <param name="parameterValue">参数值</param>
		public void AppendParameter(ref string where, ref List<SqlParameter> parameters, string tableName, string columnName, object parameterValue)
		{
			string parameterName = GetParameterName();
			if (string.IsNullOrWhiteSpace(where))
			{
				where += $"where {tableName}.{columnName} = {parameterName}";
			}
			else
			{
				where += $" AND {tableName}.{columnName} = {parameterName}";
			}
			parameters.Add(new SqlParameter($"{parameterName}", parameterValue));
		}

		/// <summary>
		/// 生成where子句 字符串模糊查询
		/// </summary>
		/// <param name="where">where子句 引用传递</param>
		/// <param name="parameters">参数 引用传递</param>
		/// <param name="tableName">表名（别名）</param>
		/// <param name="columnName">字段名</param>
		/// <param name="parameterValue">参数值</param>
		public void AppendParameter(ref string where, ref List<SqlParameter> parameters, string tableName, string columnName, string parameterValue)
		{
			string parameterName = GetParameterName();
			parameterValue = $"%{parameterValue}%";

			if (string.IsNullOrWhiteSpace(where))
			{
				where += $"where {tableName}.{columnName} like {parameterName}";
			}
			else
			{
				where += $" AND {tableName}.{columnName} like {parameterName}";
			}
			parameters.Add(new SqlParameter($"{parameterName}", parameterValue));
		}

		private string GetParameterName()
		{
			string parameterName = $"@param_{paramIndex}";
			paramIndex += 1;
			return parameterName;
		}
    }
}
