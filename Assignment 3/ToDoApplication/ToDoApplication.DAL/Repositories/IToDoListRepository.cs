using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
{
    public interface IToDoListRepository : IRepository<ToDoList>
    {
        bool DeleteSharedToDoList(UserToDoList sharedList);

        bool CheckIfToDoListExistByTitle(string title);

        bool CheckIfToDoListIsCreatedByUser(int toDoListId, int userId);

        bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId);

        bool CheckIfToDoListIsSharedByOtherUsers(int toDoListId);

        bool CheckIfToDoListIsAlreadySharedWithThatUser(int toDoListId, int userIdToBeShared);

        bool CheckIfToDoListExistInTheDatabase(int toDoListId);

        bool CheckIfThereAreCreatedTasksToToDoList(int toDoListId);

        bool ShareToDoList(UserToDoList userToDoList);

        int GetToDoListIdWhichContainsTask(int taskId);
      
        List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId);

        List<ToDoList> GetAllListsCreatedByUser(int userId);

        List<int> GetUsersIdsWithSharedList(int toDoListId);

        UserToDoList GetSharedList(int toDoListId, int userId);

        ToDoList GetToDoListById(int toDoListId);
    }
}