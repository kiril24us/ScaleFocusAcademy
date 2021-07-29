using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Data
{

    public class TaskDatabase
    {
        private static string _sqlConnectionString;
        public TaskDatabase(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        /// <summary>
        /// Create a Task in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool CreateTask(Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "INSERT INTO [dbo].[Tasks] ([Title], [Description], [IsComplete], [IdOfUserLastChange],[DateOfLastChange],[DateOfCreation],[CreatorId], [ToDoListId]) VALUES " +
                                                                           "(@Title, @Description, @IsComplete, @IdOfUserLastChange, @DateOfLastChange, @DateOfCreation, @CreatorId, @ToDoListId) SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@Title", SqlDbType.NVarChar, task.Title);
                        SQLCommonClass.AddParameter(command, "@Description", SqlDbType.NVarChar, task.Description);
                        SQLCommonClass.AddParameter(command, "@IsComplete", SqlDbType.Bit, task.IsComplete);
                        SQLCommonClass.AddParameter(command, "@IdOfUserLastChange", SqlDbType.Int, task.IdOfUserLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfLastChange", SqlDbType.DateTime2, task.DateOfLastChange);
                        SQLCommonClass.AddParameter(command, "@DateOfCreation", SqlDbType.DateTime2, task.DateOfCreation);
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, task.CreatorId);
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, task.ToDoListId);
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
        /// Delete Task
        /// </summary> 
        /// <param name="toDoListId"></param>
        /// <param name="userId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool DeleteTask(int toDoListId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Delete From [Tasks] Where ToDoListId = @ToDoListId AND TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@ToDoListId", SqlDbType.Int, toDoListId);
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);
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
        /// Delete Assign Task
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool DeleteAssignTask(int userId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Delete From [UsersTasks] Where UserId = @UserId AND TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);
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
        /// Edit Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isComplete"></param>
        /// <param name="dateOfChange"></param>
        /// <param name="userId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool EditTask(int taskId, string title, string description, bool isComplete, DateTime dateOfChange, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Update [Tasks] Set Title = @Title, Description = @Description, IsComplete = @IsComplete, IdOfUserLastChange = @IdOfUserLastChange, DateOfLastChange = @DateOfLastChange Where TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);
                        SQLCommonClass.AddParameter(command, "@Title", SqlDbType.NVarChar, title);
                        SQLCommonClass.AddParameter(command, "@Description", SqlDbType.NVarChar, description);
                        SQLCommonClass.AddParameter(command, "@IsComplete", SqlDbType.Bit, isComplete);
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

        /// <summary>
        /// Assign Task to a User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool AssignTask(int userId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "INSERT INTO [dbo].[UsersTasks] ([UserId], [TaskId]) VALUES (@UserId,@TaskId) SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.NVarChar, userId);
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.NVarChar, taskId);
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
        /// Complete Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Return true or throw an Exception</returns>
        public bool CompleteTask(int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Update [Tasks] Set IsComplete = 'true' Where TaskId = @TaskId SELECT SCOPE_IDENTITY()"))
                    {
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.NVarChar, taskId);
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
        /// Get All Tasks of To Do List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns></returns>
        public List<Task> GetAllTasks(int toDoListId)
        {
            List<Task> allTasks = new List<Task>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select * From [Tasks] where TodoListId = @TodoListId"))
                    {
                        SQLCommonClass.AddParameter(command, "@TodoListId", SqlDbType.Int, toDoListId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Task task = new Task();
                                    task.Id = reader.GetInt32(0);
                                    task.Title = reader.GetString(1);
                                    task.Description = reader.GetString(2);
                                    task.IsComplete = reader.GetBoolean(3);
                                    task.IdOfUserLastChange = reader.GetInt32(4);
                                    task.DateOfLastChange = reader.GetDateTime(5);
                                    task.DateOfCreation = reader.GetDateTime(6);
                                    task.CreatorId = reader.GetInt32(7);
                                    task.ToDoListId = reader.GetInt32(8);
                                    allTasks.Add(task);
                                }
                            }
                        }
                    }
                }
                return allTasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [Tasks] Where TaskId = @TaskId AND CreatorId = @CreatorId"))
                    {
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);
                        SQLCommonClass.AddParameter(command, "@CreatorId", SqlDbType.Int, userId);

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

        public bool CheckIfTaskExistInDatabase(int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [Tasks] Where TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);

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

        public bool CheckIfTaskIsAlreadyAssignToUser(int userId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [UsersTasks] Where UserId = @UserId AND TaskId = @TaskId"))
                    {
                        SQLCommonClass.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        SQLCommonClass.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);

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

        public bool CheckIfThereIsAlreadyTaskWithGivenTitle(string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = SQLCommonClass.CreateCommand(connection, "Select 1 From [Tasks] Where Title = @Title"))
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
    }
}
