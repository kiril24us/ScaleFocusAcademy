SFA DotNet - Assignment 7
Implement Authentication/Authorization (1 Week)
1. Assignment Goals
Teh goal of this assignment is to make our application secure and accessible only by users who have credentials. 

Take advantage of the authentication authorization mechanisms in ASP and the IdentityServer4 library to add 

JwtBearer authentication.

2. Assignment tasks
Create a new Web API project that uses Asp.NET Identity with Entity Framework add IdentityServer4 

authentication and middlewares. The project should be configured to return a token that can later be used to make 

request to protected endpoints.

Create a Controller that registers new Users and lists existing Users

Create another Controller that returns the current DateTime

When the app is started for the first time there should be one user already in the database.

User name: "admin" Password: "adminpassword" 

The user should be able to use the token endpoint provided by IdentityServer in order to authenticate.

The token should work with the built in Authorization in asp.net instead of having custom code to extract the data 

from the header.

Using the authorization mechanisms in ASP make all actions of the Users controller to be accessible 

only by admin users and all other to be accessible by any user that is authenticated. 

Following the demo application there should be an Authorize button in the swagger UI that allows setting the token.


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