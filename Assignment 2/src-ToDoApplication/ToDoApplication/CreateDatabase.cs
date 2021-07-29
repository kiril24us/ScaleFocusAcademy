using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ToDoApplication
{
    public class CreateDatabase
    {
        public static void Create(string connectionString)
        {
            Console.WriteLine("Loading scripts...");
            StreamReader createDatabaseScriptStreamReader = File.OpenText("Scripts\\CreateToDoAppDatabase.sql");

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                ExecuteScript(createDatabaseScriptStreamReader, sqlConnection);
                
                sqlConnection.Close();
            }

            Console.WriteLine("Finished.");
        }

        private static void ExecuteScript(StreamReader createDatabaseScriptStreamReader, SqlConnection sqlConnection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (!createDatabaseScriptStreamReader.EndOfStream)
            {
                string line = createDatabaseScriptStreamReader.ReadLine();

                if (line == "GO")
                {
                    try
                    {
                        string command = stringBuilder.ToString();
                        string message;

                        if (command.Length > 15)
                            message = command.Substring(0, 15);
                        else
                            message = command;

                        message = message.Trim();

                        Console.WriteLine("Executing command \"" + message + "...\"");

                        SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.AppendLine(line);
                }

            }
        }
    }
}
