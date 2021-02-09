# Tracking the exchange rate of the Czech crown
# Loader console app
Service collects data from different suppliers and converts them to the required type for storage in a database. Implements a console application with methods for getting, transforming, saving data to a database. 

### Services description:
#### RabbitService
Rabbit service allows to connect to a rabbit service deployed in a docker container and read (get) a message from particular named queue  
The service accepts parameters in the form of a logger and settings described below 

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