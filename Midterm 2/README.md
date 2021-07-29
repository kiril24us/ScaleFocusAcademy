Midterm Project

SFA dotNet - Midterm exam 1
Project Management application (1 week)
1. Assignment Goals
The Project Management application manages the operations of an IT company.

2. Assignment Description
2.1 The Project Management application is a platform for project management and time tracking. The system orchestrates the activities of multiple teams, each working on different projects. It works with registered users. Each user is represented by a username, password, first name, last name and whether they have administrator access.

2.2 A team is represented by a team name and team members (list of users). Each team can be assigned to a single project.

A project consists of tasks and can be assigned to an owner, a list of tasks that need to be completed within the project scope. A project’s owner is the user that created the project. Although the project can have only one owner, it can have multiple teams assigned to it. Think what information you need to save about each task or user. 

A task belongs to a project (the project this task belongs to) and is assigned to a user. Keep in mind that every task should have a status (pending or completed) and a list of work logs (records, describing hours spent by users, working on the task).

A work log is represented by a user that worked on the task. You need to save the time that was used for accomplishing the task, date (the date when these hours were spent working on the task) and task (the task that these hours were spent on).

2.1 Required Task
Authentication

The User should be able to log into the Project Management application with his username and password. When the user logs in for a first time and this is the first application execution there should be just one user the administrator.

Username: admin Password: adminpass

For a User without administrative privileges the access to the Users Management Endpoints needs to be restricted.

Login should be implemented as an endpoint that accepts username and password and returns user id.

All endpoints should be accessible only by registered users, by providing the userId as an http header.

If a request does not contain the header the endpoint should return 401 Status code.

Users Management

As a User with administrative privileges I need to be able to access the Users Management Endpoints where I can list all users, create, edit and delete a user. The admin user should be the one able to create a new user and persist the information needed in the database. The same user should be able to delete a user and to edit the user properties. Consider the information that needs to be gathered during the edit process.

Teams Management

The User with administrative privileges needs to be able to list all teams, create, edit and delete a team. Please consider the information that will be needed and persist it in the database. The admin user should be able to delete and edit a team. Consider the information that you need in order to be able to perform the actions. When a new user is created the admin should assign her to a team. The admin should be able to assign any user to any team. One user can be part of multiple teams.

Projects Management

Any User should be able to access the Project Management Endpoints and all Projects that are created by her or are assigned to teams where she is a member. That user should be able to edit and delete projects that are created by her. Any other projects should be locked for that particular user and she should be able just to view them. Any User should be able to create a Project. Please consider the information that you need to store in the database. A project shall be restricted for deletion except in the case it was created by the user. Please consider the information you should be able to provide in order to be able to delete that project. To edit a project the same rules should apply but the information that will be edited should be slightly different.

Task Management

Any User should be able to assign Teams to Projects that she own. As a User I need to be able to access the Tasks Management Endpoints where the user should be able to list all Tasks from a single Project that is either created by her or is assigned to a Team that she is a member of. That user should be able to create, edit and delete tasks in the project. As above consider the the information that is needed for any of the operations above. The user should be also able to get any task details by providing the ID. The user should be able to change the task statuses. The asignee of a task should be able to assign it to a different user.

Work Log Management

A User should be able to access the Task Details View where all Work Logs for this Task are available. The user should be able to create work log, edit it and delete it. Consider the access privileges based on the above rules. Any task should be available for status change following the rules from above. A User should be able to create a Work Log in a Task that is assigned to her, and persist the information. A User should be able to delete a Work Log. The work log should be editable. Consider the information that you will have to gather and persist.

3. Assignment Grading
In all the assignments, writing quality code that builds without warnings or errors, and then testing the resulting application and iterating until it functions properly is the goal. Here are the most common reasons assignments receive low points:

Project does not build.
One or more items in the Required functionalities section was not satisfied.
A fundamental concept was not understood.
Project does not build without warnings.
Code Quality - Your solution is difficult (or impossible) for someone reading the code to understand due to:
Code is visually sloppy and hard to read (e.g. indentation is not consistent, etc.).
No meaningful variable, method and class names
Not following C# code style guides
Over/under used methods, classes, variables, data structures or code comments.
Assignment is not submitted as per Assignment Submission section below.
4. Assignment Submission
You already have access to your personal ScaleFocus Academy repositories in GitLab. Every Assignment is submitted in a separate folder in that repo, on your master branch. Every folder is named by the assignment name and number -ex: Final Exam.

Here is a sample structure of how your master branch of your repo should look like towards the end of your SFA training:

├── Assignment 1
├── Assignment 2
├── Assignment 3
├── Assignment 4
├── Midterm 1
├── └── Workforce-management
├──     └── README.md
├──     └── src
├──         └── WorkforceManagement.sln
├──         └── ConsoleApp
├──            └── ConsoleApp.csproj
├──            └── Program.cs
├──            └── ...
├──         └── ClassLibrary
├──         └── LibraryClass.cs
├── Assignment 5
├── Midterm 2
├── Assignment 6
├──         └── ...
├──         └── ...
...
 
Assignments that have not been submitted to the master branch or have incorrect folder structure will not be graded.

How to use Git to submit your assignments for review?

⚠️ Make sure you have read the GitLab Reading Materials first, available here.

Let's imagine that the first required task from your assignment is to create a Login View in your project:

Make sure you have the latest version of your code;

Open a bash terminal (CMD for Windows) in your Assignment folder or navigate there with cd Assignment-1/

Create a new branch for the feature you're developing git checkout -b login View, where login Views the name of your new branch.

Now you need to add all the file you have changed. You can use git add .when you want to add all files in the current folder. Or use git add file.txt you can define specific files you want to push to yor remote repo.

Your next step is to commit the changes you have made. You need to use git commit -m "add README" where the message must be meaningful and is describe the exact reason for change;

The last step you need to perform is to push your changes to the remote repo by git push -u origin loginView. Pay close attention that master is your main branch and you are not committing to it directly. Pushes are done ONLY against feature branches(branches other than master)!

Create a Merge Request and assign your Tutor to it -Open GitLab and navigate to Merge Requests> Create new Merge Request and select your feature branch login Views source and master as target/destination.

Your Tutor/Mentor will now review your code. If you have merge request comments, you will need to resolve them before proceeding:

Up vote or write something under the comment, acknowledging that you agree with the comment. If there is something you don't understand, now is your time to discuss it by writing under this comment.
If everything is clear with the comment, go back to your source code. Make sure you're on your branch, by calling git checkout loginView
Do work here that resolves comments
Commit as usual(check above).
The merge request will be updated with the new code, so your Tutor/Mentor will see your new changes. If there are additional Merge Request comments repeat step
When done with all changes you will be allowed to merge your branch with the master branch. Do not forget to mark the branch to be deleted after the merge. Keep in mind that all versions of your code are kept in git and you don't need the branches in your repo.