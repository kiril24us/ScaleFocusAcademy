using System;
using System.Collections.Generic;
using System.Text;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{
    public interface ITaskService
    {
        bool CreateTaskInToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId);

        public bool CreateTaskInSharedToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId);

        bool DeleteTask(int toDoListId, int taskId, int userId);

        Task FindTaskById(ToDoList toDoList,int taskId);

        bool EditTask(int toDoListId, int taskId, int userId, string title, string description, bool isComplete);

        bool CheckIfToDoListExistInAllUsersToDoLists(int toDoListId);

        bool CheckIfToDoListExistInAllUsersSharedToDoLists(int toDoListId);

        List<Task> GetAllTasksFromToDoListCreatedByUser(int toDoListId, int userId);

        List<Task> GetAllTasksFromSharedToDoListWithUser(int toDoListId, int userId);

        bool AssignTask(int taskId, int userId, ToDoList toDoList);

        bool AssignTaskInSharedToDoList(int taskId, int userId, ToDoList toDoList);

        List<ToDoList> GetAllListsThatContainsTask(int taskId);

        List<Task> FindAllTasksById(int taskId);

        void CompleteTasks(List<Task> tasksToComplete);

        bool CheckIfTaskExistsInAllToDoLists(int taskId);

        bool CheckIfTaskExistsInAllSharedToDoLists(int taskId);
    }
}
