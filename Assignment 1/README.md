SFA DotNet - 1 Assignement
ToDo Application (1 week)
The ToDo application introduces organization in day-to-day tasks. It is a great performance booster for organizations and individuals, working on multiple projects.

1. Assignment Goals
Development of ToDo application for management of ToDo lists;
Development of machine usable CLI tool for access to core system functionality.
2. Assignment Description
2.1 Required Tasks
Authentication

Story 1.

As a User I need to be able to log into the ToDo application with my username and password. If this is the first execution of the application and there are no users in the database I need to be able to log-in with the following:

User name: "admin"
Password: "adminpassword"
Story 2.

As a user without administrative privileges my access to the Users Management View needs to be restricted.

Users Management

Story: 3.

As a user with administrative privileges I need to be able to access the Users Management View where I can list all users, create, edit and delete a user.

Story: 4.

As a User with administrative privileges I need to be able to create a user and persist the following information:

Id
Username
Password
First Name
Last Name
Role (Admin/RegularUser)
Date of creation
Id of the creator
Date of last change
Id of the user that did the last change
Story: 5.

As a User with administrative privileges I need to be able to delete a user by Id

Story: 6.

As a User with administrative privileges I need to be able to select a user by Id and edit the following information:

Username
Password
First Name
Last Name

  * Date of creation and Id of the creator remain unchanged
  * Date of last change and Id of the user that did the last change are updated automatically
ToDo Lists Management

Story: 7.

As a User I need to be able to access the ToDo List Management View where I can access all ToDo lists that are created by me or are shared with me, list all ToDo lists, create, edit and delete a ToDo list.

Story: 8.

As a User I need to be able to create a ToDo list and persist the following information:

Id
Title
Date of creation
Id of the creator
Date of last change
Id of the user that did the last change
Story: 9.

As a User I need to be able to delete a ToDo list by Id. If the ToDo list is not created by me (is shared with me by another user) then the delete action removes the share but does not actually delete the ToDo list.

Story: 10.

As a User I need to be able to edit a ToDo list by Id by providing the following information:

Id
Title

*Date of creation and Id of the creator remain unchanged
*Date of last change and Id of the user that did the last change are updated automatically
Story: 11.

As a User I need to be able to share a ToDo list with other users.

Task Management

Story: 12.

As a User I need to be able to access the Tasks Management View where I can access all Tasks from a single ToDo list that is either created by me or is shared with me. I should be able to list all Tasks in the ToDo list, create, edit and delete a Task in the ToDo list.

Story: 13.

As a User I need to be able to create a Task in a ToDo list and persist the following information:

Id
Id of the List  (the Id of the ToDo list that the Task belongs to)
Title
Description
IsComplete
Date of creation
Id of the creator
Date of last change
Id of the user that did the last change
Story: 14.

As a User I need to be able to delete a Task by Id.

Story: 15.

As a User I need to be able to edit a Task by Id by providing the following information:

Id
Title
Description
IsComplete

* Date of creation and Id of the creator remain unchanged
* Date of last change and Id of the user that did the last change are updated automatically
Story: 16.

As a User I need to be able to assign a Task to one or more Users in the system that have access to the ToDo list that the Task belongs to.

Story: 17. 

As a User I need to be able to complete a Task by Id through a fast access menu in the Task Management View.





Task Management

Story: 18.

As a User I need to be able to access through the CLI tool all data related to Tasks from a given ToDo list that is either created by me or is shared with me. I should be able to list all Tasks in the ToDo list, create, edit and delete a Task in the ToDo list.

Story: 19.

As a User I need to be able to create a Task in a ToDo list through the CLI tool and persist the following information:

Id
Id of the List  (the Id of the ToDo list that the Task belongs to)
Title
Description
IsComplete
Date of creation
Id of the creator
Date of last changeId of the user that did the last change
Story: 20.

As a User I need to be able to delete a Task by Id through the CLI tool.

Story: 21.

As a User I need to be able to edit a Task by Id through the CLI tool by providing the following information:

Title
Description
IsComplete
* Date of creation and Id of the creator remain unchanged
* Date of last change and Id of the user that did the last change are updated automatically
Story: 22.

As a User I need to be able to assign a Task through the CLI tool to one or more Users in the system that have access to the ToDo list that the Task belongs to. Story: 23. As a User I need to be able to complete a Task by Id through the CLI tool using a fast access command argument.

2.2. Extra Credit
User Management CLI tool

Story: 24.

As a User I need to be able to access all the features of the ToDo application through the CLI tool by providing my username and password as arguments to the CLI tool.

Story: 25.

As a User with administrative privileges I need to be able to access all User related data in the ToDo application through the CLI tool.

Story: 26.

As a User with administrative privileges I need to be able to create a user through the CLI tool and persist the following information:

Id
Username
Password
First Name
Last Name
Dateof creation
Id of the creator
Date of last change
Id of the user that did the last change
Story: 27.

As a User with administrative privileges I need to be able to delete a user by Id through the CLI tool.

Story: 28.

As a User with administrative privileges I need to be able to edit a user by Id through the CLI tool by providing the following information:

Username
Password
First Name
Last Name
Date of creation and Id of the creator remain unchanged
Date of last change and Id of the user that did the last change are updated automatically
ToDo Lists Management CLI tool

Story: 29.

As a User I need to be able to access through the CLI tool all data related to ToDo lists that are created by me or are shared with me -list all ToDo lists, create, edit and delete a ToDo list.

Story: 30.

As a User I need to be able to create a ToDo list through the CLI tool and persist the following information:IdTitleDate of creationId of the creatorDate of last changeId of the user that did the last change

Story: 31.

As a User I need to be able to delete a ToDo list by Id through the CLI tool. If the ToDo list is not created by me (is shared with me by another user) then the delete action removes the share.

Story: 32.

As a User I need to be able to edit a ToDo list by Id through the CLI tool by providing the following information:

Id
Title
Date of creation and Id of the creator remain unchanged
Date of last change and Id of the user that did the last change are updated automatically
Story: 33.

As a User I need to be able to share a ToDo list with other users through the CLI tool.