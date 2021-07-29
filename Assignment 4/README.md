Before you build the program for the first time change settings for connection string. The string is located in ToDoApplication.Web, appsetings.json.

Assignment 4
SFA DotNet - 4 Assignment
ToDo Application add AspNet Core (1 week)
1. Assignment Goals
Using the ToDo application you are expected to implement an AspNet Core Web API application to replace the existing console application.

2. Assignment Description
Using the stories from Assignment 1 implement a Web API application that stores all of the required data in an MSSQL database using Entity Framework.

The AspNet Core API application template should be used
Login functionality should be implemented by Http Header or other HTTP parameter
Use Open Api generated UI for making requests to the application
All requests should be validated and have meaningful restrictions
Use readable and easy to understand routes
Use global exception handler that returns predictable response in the Http response body
All requirements for the SQL database from previous assignments should be met
The Extra Credit (CLI Tool) tasks from Assignment 1 are not included in the scope of this one.
2.1 Required Tasks
Task 1

Add AspNet Web API application to your project.

Task 2

When your application is started for the first time it should be able to create an empty database and tables so that developers working with the code don't have to create the database themselves.

Task 3

Add validation to all of your requests. And in case of invalid request return proper http status code

Task 4

Add global exception handling that will return the following response: Http status code: 500 Body:

{​​​​​​
   "ErrorMessage": "There was an exception while trying to process your requests",
   "ExceptionType": [The type of the exception]
}​​​​
 
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