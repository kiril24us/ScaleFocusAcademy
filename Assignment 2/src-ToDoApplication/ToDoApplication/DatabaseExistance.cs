using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ToDoApplication
{
    public class DatabaseExistance
    {
        public static bool CheckIfDatabaseExists(string connectionString)

        {
            string cmdText = "select count(*) from master.dbo.sysdatabases where name=@ToDoApp";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCmd = new SqlCommand(cmdText, sqlConnection))
                {
                    // Use parameters to protect against Sql Injection
                    sqlCmd.Parameters.Add("@ToDoApp", System.Data.SqlDbType.NVarChar).Value = "ToDoApp";

                    // Open the connection as late as possible
                    sqlConnection.Open();
                    // count(*) will always return an2 int, so it's safe to use Convert.ToInt32
                    return Convert.ToInt32(sqlCmd.ExecuteScalar()) == 1;
                }
            }
        }
    }
}
