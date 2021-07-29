using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Data
{
    public class UserDatabase
    {
        private static string _sqlConnectionString;
        
        public UserDatabase(string connectionString)
        {
            _sqlConnectionString = connectionString;
            
        }

        /// <summary>
        /// Create a user in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "INSERT INTO [dbo].[Users] ([Username],[Password],[FirstName],[LastName],[Role],[IdOfUserLastChange],[DateOfLastChange],[DateOfCreation],[CreatorId]) VALUES " +
                                                                           "(@Username, @Password, @Firstname, @LastName, @Role, @IdOfUserLastChange, @DateOfLastChange, @DateOfCreation, @CreatorId) SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@Username", SqlDbType.NVarChar, user.Username ?? (object)DBNull.Value);
                        SQLCommonClass.AddParameter(command, "@Password", SqlDbType.NVarChar, user.Password ?? (object)DBNull.Value);
                        SQLCommonClass.AddParameter(command, "@Firstname", SqlDbType.NVarChar, user.FirstName ?? (object)DBNull.Value);
                        SQLCommonClass.AddParameter(command, "@LastName", SqlDbType.NVarChar, user.LastName ?? (object)DBNull.Value);
                        SQLCommonClass.AddParameter(command, "@Role", SqlDbType.Int, (int)user.Role);
                        SQLCommonClass.AddParameter(command, "@IdOfUserLastChange", SqlDbType.Int, user.IdOfUserLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfLastChange", SqlDbType.DateTime2, user.DateOfLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfCreation", SqlDbType.DateTime2, user.DateOfCreation);
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, user.CreatorId);
                        object result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "DELETE FROM [dbo].[Users] WHERE UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        object result = command.ExecuteNonQuery();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="dateOfChange"></param>
        /// <param name="IdOfUserLastChange"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool EditUser(int userId, string username, string password, string firstname, string lastname, DateTime dateOfChange, int IdOfUserLastChange)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Update Users Set [Username] = @Username, [Password] = @Password, [FirstName] = @FirstName, [LastName] = @Lastname, [DateOfLastChange] = @DateOfLastChange, [IdOfUserLastChange] = @IdOfUserLastChange where UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        SQLCommonClass.AddParameter(command, "@Username", SqlDbType.NVarChar, username);
                        SQLCommonClass.AddParameter(command, "@Password", SqlDbType.NVarChar, password);
                        SQLCommonClass.AddParameter(command, "@FirstName", SqlDbType.NVarChar, firstname);
                        SQLCommonClass.AddParameter(command, "@LastName", SqlDbType.NVarChar, lastname);
                        SQLCommonClass.AddParameter(command, "@DateOfLastChange", SqlDbType.DateTime2, dateOfChange);
                        SQLCommonClass.AddParameter(command, "@IdOfUserLastChange", SqlDbType.Int, IdOfUserLastChange);
                        object result = command.ExecuteNonQuery();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> usersFromDatabase = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select * From Users"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    User user = new User();
                                    user.Id = reader.GetInt32(0);
                                    user.Username = reader.GetString(1);
                                    user.Password = reader.GetString(2);
                                    user.FirstName = reader.GetString(3);
                                    user.LastName = reader.GetString(4);
                                    user.Role = Enum.Parse<Role>(reader.GetInt32(5).ToString());
                                    user.IdOfUserLastChange = reader.GetInt32(6);
                                    user.DateOfLastChange = reader.GetDateTime(7);                                    
                                    user.DateOfCreation = reader.GetDateTime(8);
                                    user.CreatorId = reader.GetInt32(9);
                                    usersFromDatabase.Add(user);
                                }
                            }
                        }
                    }
                }
                return usersFromDatabase;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckIfUserExistById(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT 1 FROM [dbo].[Users] where UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {                            
                            return reader.Read();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User GetUserByUsername(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT UserId, Username, Role FROM Users Where [UserName] = @username AND  [Password] = @password"))
                    {
                        SQLCommonClass.AddParameter(command, "@username", SqlDbType.NVarChar, username);
                        SQLCommonClass.AddParameter(command, "@password", SqlDbType.NVarChar, password);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                User user = new User();
                                user.Id = reader.GetInt32(0);
                                user.Username = reader.GetString(1);
                                user.Role = Enum.Parse<Role>(reader.GetInt32(2).ToString());
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public static User GetUserById(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT UserId, Username, Role FROM Users Where [UserId] = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                User user = new User();
                                user.Id = reader.GetInt32(0);
                                user.Username = reader.GetString(1);
                                user.Role = Enum.Parse<Role>(reader.GetInt32(2).ToString());
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }      
    }
}
