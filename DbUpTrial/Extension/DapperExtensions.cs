using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlBuilder;

namespace DbUpTrial.Extension
{
    public static class DapperExtensions
    {
        public static async Task<(IEnumerable<BaseQueryResult>, int?)> QueryAsync<BaseQueryResult>(this IDbConnection conn, string query, object param = null, int? pageSize = null, int? pageNumber = null, IDbTransaction tran = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(query));
            }

            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Value must be greater than or equal to 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value must be greater than or equal to 1.");
            }

            var builder = new SqlBuilder();
            int? offset = pageSize.HasValue && pageNumber.HasValue ? (pageNumber - 1) * pageSize : null;

            var limitQuery = "";
            if (offset != null)
                limitQuery = GetLimitQuery(conn, offset.Value, pageSize.Value);

            //query = string.Concat(query, pageSize.HasValue && pageNumber.HasValue ? $" Limit {pageSize} Offset (({)" : "");

            // Build the final query.
            string finalQuery = $"{query} {limitQuery}";

            var t = builder.AddTemplate(finalQuery);

            // Calculate the total number of matching rows.
            string countQuery = GetCountQuery(t);
            int totalCount = conn.ExecuteScalar<int>(countQuery, param, tran);

            return (await conn.QueryAsync<BaseQueryResult>(t.RawSql, param, tran), totalCount);
        }

        private static string GetLimitQuery(IDbConnection conn,int offset, int pageSize)
        {
            switch (conn.GetType().Name)
            {
                case "MySqlConnection":
                    return $"LIMIT {offset}, {pageSize}";
                case "NpgsqlConnection":
                    return $"OFFSET {offset} LIMIT {pageSize}";
                case "SqlConnection":
                    return $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                default:
                    throw new NotSupportedException($"Database type {conn.GetType().Name} is not supported");
            }
        }

        public static string GetCountQuery(this Template template)
        {
            var q0 = template.RawSql;
            int orderByIndex = q0.IndexOf("ORDER BY");
            if (orderByIndex >= 0)
                q0 = q0.Substring(0, orderByIndex);
            int groupByIndex = q0.IndexOf("GROUP BY");
            if (groupByIndex >= 0)
                q0 = q0.Substring(0, groupByIndex);
            var q1 = q0.Replace("\n", "");
            var q2 = q1.RemoveBetween("SELECT", "FROM");

            return q2;
        }

        private static string RemoveBetween(this string sourceString, string startTag, string endTag)
        {
            Regex regex = new Regex(string.Format("{0}(.*?){1}", Regex.Escape(startTag.ToLowerInvariant()), Regex.Escape(endTag.ToLowerInvariant())), RegexOptions.RightToLeft);
            var q = regex.Replace(sourceString.ToLowerInvariant(), startTag + " count(*) " + endTag);
            return q;
        }
    }
}
