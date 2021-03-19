# Tracking the exchange rate of the Czech crown
# BTB connector app
Service for downloading data from BTB Bank. Implements a console application for downloading / receiving data from BTB Bank.

### Services description:
#### RabbitService
Rabbit service allows to connect to a rabbit service deployed in a docker container and send a message through a specific exchanger to a named queue  
The service accepts parameters in the form of a logger and settings described below 

1) When the service connector starts, it sends a message (only ones) that it is available to the "Scheduler" exchanger; 

We use the next model which we read from json file and allow us to registered new job
```
{
    "Version": "1.0",
    "JobName": "some name",
    "IsEnabled": true,
    "CronScheduler": "* * * * *",
    "Command": "Download",
    "RoutingKey": "service_name" 
 }
```
2) as soon as the connector sends this command, the scheduler must accept it and register the job for execution, with the parameters that came; 
3) then he will send this command (data from the Jason model) to the exchange "Sheduler" with routingKey = service_name;

### Parameter description:
From appsettings.json we have settings for connecting to RabbitMQ when we run it locally by the next settings:
RabbitSettings
- "HostName"
- "Port": 
- "Login": Login for connection(login) in rabbit on the server
- "Password": Password for login in rabbit on the server

These settings will be overwritten in global environment by docker-compose.override file with the next list of settings:
- RabbitSettings__HostName - host name
- RabbitSettings__Port - port number
- RabbitSettings__Login - Login for connection(login) in rabbit on the server which we deploy in docker
- RabbitSettings__Password - Password for login in rabbit on the server which we deploy in docker


[Go Back](../../Readme.md)