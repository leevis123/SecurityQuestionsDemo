SecurityQuestionsDemo Readme

Data Storage
*****************************
- An SQLite database was used to store data for users, security questions, and answers for each security question.
- A script file named Setup.sql can be found in the _sql directory in the solution. The script contains the DB schema for the tables used, as well as an INSERT statement to pre-seed the security questions table. 
- In the UserSecurityQuestion table, a constraint is in place to prevent a user from adding a duplicate security question. 

Program Design
*****************************
The program was designed to separate the entities used, a data access layer for handling application data, and a business logic layer to maintain the flow of the data moving through the application. Since it's a small project, everything could have been lumped into a single project, but to demonstrate good design and code practices, I chose to separate the application into separate projects. 

- SecurityQuestionsDemo: the console application containing the functions to steps through the each of the requirements. 
- SecurityQuestionsDemo.BL: library with classes to manage the flow of data coming from the application. 
- SecurityQuestionsDemo.DAL: library with class to perform data operations. The SQLite database (SecurityQuestionsDemo.db) used by the application can also be found in this project folder.
- SecurityQuestionsDemo.Entity: library that defines objects used in the application. 

Running the Program
*****************************
The SecurityQuestionsDemo project is configured to be the Startup Project for the solution. The program can be launched directly from Visual Studio from the Debug menu or toolbar. 

Other Notes
*****************************
- Due to time constraints, I did not put as much effort into exception handling as I would in a production application. Instead, I relied on the database constraints put in place and the validation logic in code to handle program errors.

Thanks for your time and the opportunity!