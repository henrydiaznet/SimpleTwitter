The project "Simple Twitter", a small microblogging solution that will allow people to post comments and see other comments 
that other people have posted. Things that you'll need to include are a database to store the comments and usernames, 
a web-based UI, and some validation to make sure garbage isn't making it into the data storage layer.

Functional requirements:
- A username should be required to post a comment.
- A comment should not exceed 140 characters.
- Comments should be displayed in reverse chronological order (newer first).

Technical requirements:
- C# should be used for the server side.
- React should be used for the client side/UI.
- SQL Server should be used for the database.
- Unit tests should be included.

***

In order to satisfy technical requirements solution is split into 3 containerized components:

- SimpleTwitter.Api (using .net8 minimal APIs)
- ClientApp/SimpleTwitter (using React + vite)
- MS SQL database

Launch pre-requisites:
1) Make sure docker is installed (https://www.docker.com/)
2) Make sure a version of dotnet 6-8 SDK is installed on your machine https://dotnet.microsoft.com/en-us/download/dotnet/8.0 (this is only required for next step)
3) If you have not already, generate dev certificate in {user}\.aspnet\https folder:
`dotnet dev-certs https -ep %USERPROFILE%\https\simpletwitter.pfx -p qwerty` 
and:
`dotnet dev-certs https --trust`

4) Open the terminal in the solution folder and execute:

`docker compose up`
5) In your browser navigate to http://localhost:3000

Notes:

Data and logs folder for containerized SQL server are mounted to .sqlserver folder, and will persist through restarts.

API and client can be run without docker, but client requires node.js, and instance of SQL server would be required. Connection string should be set in appsettings.json file.

Tests are located in ./tests/SimpleTwitter.Tests project.
Integration tests are implemented according to recommendations provided by 
https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0



