using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ToDoApplication.DAL.Data
{
    public class SQLCommonClass
    {
        public static void AddParameter(SqlCommand command, string parametername, SqlDbType parameterType, object parameterValue)
        {
            command.Parameters.Add(parametername, parameterType).Value = parameterValue;
        }

        public static SqlCommand CreateCommand(SqlConnection connection, string sql)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }
    }
}
