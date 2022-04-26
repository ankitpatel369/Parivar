using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Parivar.Data.Extensions
{
    public static class DatabaseExtension
    {
        public static async Task<DataSet> GetQueryDatatableAsync(this Microsoft.EntityFrameworkCore.DbContext context, string sqlQuery, SqlParameter[] sqlParam = null, CommandType type = CommandType.StoredProcedure)
        {
            using (DbCommand cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandTimeout = 0;
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = type;
                if (sqlParam != null)
                { cmd.Parameters.AddRange(sqlParam); }
                cmd.CommandText = sqlQuery;
                using (DbDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    await Task.Run(() => dataAdapter.Fill(ds));
                    return ds;
                }
            }
        }

        public static DataSet GetQueryDatatable(this Microsoft.EntityFrameworkCore.DbContext context, string sqlQuery, SqlParameter[] sqlParam = null, CommandType type = CommandType.StoredProcedure)
        {
            using (DbCommand cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.Connection = context.Database.GetDbConnection();
                cmd.CommandType = type;
                if (sqlParam != null)
                { cmd.Parameters.AddRange(sqlParam); }
                cmd.CommandText = sqlQuery;
                using (DbDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds);
                    return ds;
                }
            }
        }

        public static List<T> ToListReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }))
            {
                List<T> toReturn = query.ToList();
                scope.Complete();
                return toReturn;
            }
        }

        public static IQueryable ReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            }))
            {
                var toReturn = query;
                scope.Complete();
                return toReturn;
            }
        }

        public static IEnumerable<dynamic> CollectionFromSql(this Microsoft.EntityFrameworkCore.DbContext dbContext, string Sql) //, DbParameter[] Parameters)
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                cmd.CommandTimeout = dbContext.Database.GetCommandTimeout() ?? 60;
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                //foreach (var param in Parameters)
                //{
                //    //DbParameter dbParameter = cmd.CreateParameter();
                //    //dbParameter.ParameterName = param.Key;
                //    //dbParameter.Value = param.Value;
                //    cmd.Parameters.Add(param);
                //}

                //var retObject = new List<dynamic>();
                using (var dataReader = cmd.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;

                    }
                }


            }
        }

        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}
