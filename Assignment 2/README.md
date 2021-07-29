ToDo Application add connection to database (1 week)

1. Assignment Goals
Using the ToDo application you are expected to implement a database connection in order to store all of your data in MSSQL.

2. Assignment Description
Using the stories from Assignment 1 implement an application that stores all of the required data in an MSSQL database.

The database columns should have proper types for the objects they store.
The database tables should have proper Primary and Foreign Keys configured on the respective columns.
Data in the database should be in normalized format following the relational database normalization techniques.
Use ADO.NET without an ORM (EntityFramework, NHibernate, Dapper ORM etc.) for the implementation of your database code.
Use configuration files to configure your connection strings
The Extra Credit (CLI Tool) tasks from Assignment 1 are not included in the scope of this one.
2.1 Required Tasks
Task 1

Create a Database and database tables that will store the data of your ToDo application.

Follow the SOLID principles while you are implementing your application and use layered architecture

Task 2

When your application is started for the first time it should be able to create an empty database and tables so that developers working with the code don't have to create the database themselves. Generating unique and sequential Ids should be done in the database.

2.2. Extra Credit
No extra credit is preserved.