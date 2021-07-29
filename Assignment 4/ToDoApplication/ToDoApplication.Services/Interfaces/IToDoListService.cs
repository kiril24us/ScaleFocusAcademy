﻿using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Services.Interfaces
{
    public interface IToDoListService
    {
        bool CreateToDoList(string title, int creator);

        bool DeleteToDolist(int toDoListId, int creatorId);

        bool CheckIfToDoListIsCreatedByUser(int toDoListId, int userId);

        bool CheckIfToDoListExistInTheDatabase(int toDoListId);

        bool ShareToDoList(int toDoListId, int userIdToBeShared);

        bool EditToDoList(int toDoListId, int userId, string title);

        bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId);

        ToDoList GetToDoListFromDatabaseById(int toDoListId);

        ToDoList GetToDoListByTitle(string title);

        List<ToDoList> GetAllListsCreatedByUser(int userId);

        List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId);

        int GetToDoListIdWhichContainsTask(int taskId);
    }
}
