using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Data
{
    public class ToDoListDatabase
    {
        private static string _sqlConnectionString;

        public ToDoListDatabase(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        /// <summary>
        /// Create a ToDo List in Database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool CreateToDoList(ToDoList toDoList)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "INSERT INTO [dbo].[ToDoLists] ([Title],[IdOfUserLastChange],[DateOfLastChange],[DateOfCreation],[CreatorId]) VALUES " +
                                                                           "(@Title, @IdOfUserLastChange, @DateOfLastChange, @DateOfCreation, @CreatorId) SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@Title", SqlDbType.NVarChar, toDoList.Title);
                        SQLCommonClass.AddParameter(command, "@IdOfUserLastChange", SqlDbType.Int, toDoList.IdOfUserLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfLastChange", SqlDbType.DateTime2, toDoList.DateOfLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfCreation", SqlDbType.DateTime2, toDoList.DateOfCreation);
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, toDoList.CreatorId);
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
        /// Delete Shared ToDoList
        /// </summary> 
        /// <param name="toDoListId"></param>
        /// <param name="userId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool DeleteSharedToDoList(int toDoListId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Delete from [UsersToDoLists] Where ToDoListId = @ToDoListId AND UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.NVarChar, toDoListId);
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
        /// Delete ToDoList
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="userId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool DeleteToDoList(int toDoListId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Delete from [ToDoLists] Where ToDoListId = @ToDoListId AND CreatorId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.NVarChar, toDoListId);
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

        public bool ShareToDoList(int toDoListId, int userIdToBeShared)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Insert Into [UsersToDoLists] Values (@UserId, @ToDoListId) SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userIdToBeShared);
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
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
        /// Edit To Do List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="dateOfChange"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool EditToDoList(int toDoListId, DateTime dateOfChange, int userId, string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Update [ToDoLists] Set Title = @Title, IdOfUserLastChange = @IdOfUserLastChange, DateOfLastChange = @DateOfLastChange Where ToDoListId = @ToDoListId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
                        SQLCommonClass.AddParameter(command, "@Title", SqlDbType.NVarChar, title);
                        SQLCommonClass.AddParameter(command, "@IdOfUserLastChange", SqlDbType.Int, userId);
                        SQLCommonClass.AddParameter(command, "@DateOfLastChange", SqlDbType.DateTime2, dateOfChange);
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

        public bool CheckIfToDoListExistsByTitle(string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT 1 FROM [dbo].[ToDoLists] where Title = @Title"))
                    {
                        SQLCommonClass.AddParameter(command, "@Title", SqlDbType.NVarChar, title);

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

        public bool CheckIfToDoListExistInTheUser(int toDoListId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT 1 FROM [dbo].[ToDoLists] where ToDoListId = @ToDoListId AND CreatorId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
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

        public bool CheckIfToDoListExistInTheDatabase(int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [ToDoLists] Where ToDoListId = @ToDoListId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);

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

        public bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [UsersToDoLists] where ToDoListId = @ToDoListId AND UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
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

        public List<int> GetUsersIdsWithSharedList(int toDoListId)
        {
            List<int> usersIds = new List<int>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select [UserId] from UsersToDoLists Where ToDoListId = @ToDoListId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(0);
                                    usersIds.Add(id);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return usersIds;
        }

        public User GetUserById(int creatorId)
        {
            return UserDatabase.GetUserById(creatorId);
        }

        public bool CheckIfToDoListIsSharedByOtherUsers(int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 from [UsersToDoLists] Where ToDoListId = @ToDoListId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);

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

        public bool CheckIfToDoListIsAlreadySharedWithThatUser(int toDoListId, int userIdToBeShared)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [UsersToDoLists] Where ToDoListId = @ToDoListId AND UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userIdToBeShared);

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

        public static ToDoList GetToDoListFromHisCreator(int toDoListId, int creatorId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT [Title], [IdOfUserLastChange], [CreatorId] FROM ToDoLists Where ToDoListId = @ToDoListId AND CreatorId = @CreatorId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.NVarChar, toDoListId);
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, creatorId);
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                ToDoList toDoList = new ToDoList();
                                toDoList.Title = reader.GetString(0);
                                toDoList.IdOfUserLastChange = reader.GetInt32(1);
                                toDoList.CreatorId = reader.GetInt32(2);
                                return toDoList;
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

        public int GetToDoListIdWhichContainsTask(int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "SELECT ToDoListId FROM [Tasks] Where TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                int toDoListId = reader.GetInt32(0);
                                return toDoListId;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId)
        {
            List<int> sharedToDoListsIds = new List<int>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select ToDoListId from UsersToDoLists Where UserId = @UserId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    sharedToDoListsIds.Add(reader.GetInt32(0));
                                }
                            }
                        }
                        return sharedToDoListsIds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<ToDoList> GetAllListsCreatedByUser(int userId)
        {
            List<ToDoList> allListsCreatedByUser = new List<ToDoList>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select * From ToDoLists where CreatorId = @CreatorId"))
                    {
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ToDoList toDoList = new ToDoList();
                                    toDoList.Id = reader.GetInt32(0);
                                    toDoList.Title = reader.GetString(1);
                                    toDoList.IdOfUserLastChange = reader.GetInt32(2);
                                    toDoList.DateOfLastChange = reader.GetDateTime(3);
                                    toDoList.DateOfCreation = reader.GetDateTime(4);
                                    allListsCreatedByUser.Add(toDoList);
                                }
                            }
                        }
                    }
                }
                return allListsCreatedByUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ToDoList> GetAllListsSharedByUser(List<int> allToDoListsIds)
        {
            List<ToDoList> allListsSharedByUser = new List<ToDoList>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    foreach (int id in allToDoListsIds)
                    {
                        using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select * From [ToDoLists] Where ToDoListId = @ToDoListId"))
                        {
                            SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        ToDoList toDoList = new ToDoList();
                                        toDoList.Id = reader.GetInt32(0);
                                        toDoList.Title = reader.GetString(1);
                                        toDoList.IdOfUserLastChange = reader.GetInt32(2);
                                        toDoList.DateOfLastChange = reader.GetDateTime(3);
                                        toDoList.DateOfCreation = reader.GetDateTime(4);
                                        allListsSharedByUser.Add(toDoList);
                                    }
                                }
                            }
                        }
                    }
                }
                return allListsSharedByUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}


