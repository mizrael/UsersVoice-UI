# UsersVoice

UsersVoice is the project submitted by the Coders101 team at the Dell Limerick Hackathon 2016. We didn't won the competition but thought would be nice to share what we accomplished in two days of coding.

The team was composed as follows:
- Stephen Byrne - front-end UI and back-end coding
- Davide Guida - API, database and front-end coding
- Jacob Mendoza - front-end UI and back-end coding
- Alex Estevam - PM
- Tom Gormley - testing

The system works as a "suggestion box" for user ideas, with the possibility for the community to comment and vote the ideas they like most. Each user has a fixed amount of points available from the beginning that can spend voting (1, 2 or 3 votes). Each idea can be voted once but a user can take back the vote if she changes her mind.
In case an idea is cancelled by the admins, the voters will get their points back. In case the idea is approved, the voters will get the points back multiplied by 1.5 . Finally, if an idea is implemented (eg. the requested software is written ), the voters will receive half of the points spent in addition.

The system was deployed entirely on Azure using a standard web app for the front-end and a virtual machine for the APIs.

## Architecture
The system is divided in two main parts: the public front-end and the API for the data services.

### Front-end
The front-end is written in C# using ASP.NET MVC, WebAPI and AngularJS. All the data displayed is loaded and updated via API calls.

### API
The API is written in C# using ASP.NET WebAPI and MongoDb as persistence mechanism. The system is architectured with the CQRS pattern, using two separate databases for commands and queries and an event system to keep the data in sync ( we used the excellent Mediatr library ).

## Database preparation
- download MongoDB
- extract the files on your hard drive ( we used this path: "C:\Program Files\MongoDB\Server\3.2\" )
- create a folder for the database files ( we used this path: "C:\databases\mongodb\usersvoice" )
- run the batch file UsersVoice.Services\tools\server_start.bat ( this will fire up the MongoDb instance )
- run the batch file UsersVoice.Services\tools\init_db.bat ( this will create the db users and feed the db with some fake data )

## Running the project

### API
- make sure the MongoDb instance is running
- open the solution UsersVoice.Services\UsersVoice.Services.sln
- run the project UsersVoice.Services.API

### Front-end
- open the solution UsersVoice.Public\UsersVoice.Public.sln
- open the web.config file, locate the "DataServiceUrl" key in the appSettings node and update it with the endpoint for the UsersVoice.Services.API
- run the project

##### Note: 
Executing the Front-end in DEBUG mode will load data from a fake repository system, thus requiring no API running. Executing in RELEASE mode instead will load the data from the API. 
