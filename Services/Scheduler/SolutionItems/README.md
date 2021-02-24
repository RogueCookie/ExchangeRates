# Tracking the exchange rate of the Czech crown
# Scheduler Api 
Service for creating scheduled tasks. Implements API and logic for adding / editing scheduled tasks 

### Logic of adding new scheduled jobs 
We need to dynamically add jobs to the scheduler. And we have two options:
When we add a Connector to the system, it must inform the scheduler once at startup that it is there, it has some default settings to execute and some command that the Scheduler should send on a schedule.
For that we need:
1) determine which command the scheduler will react to when he needs to add a new command with a schedule 
2) decide on the command to which the connector will respond 

#### Through the Rest request 
it's by swagger

#### Through a message in â exchange Scheduler
Will be automatically registered service connector when we start current service

1) When the service connector starts, it sends a message (only ones) that it is available to the "Scheduler" exchanger; 

We use the next model which we read from json file and allow us to registered new job
```
{
    "Version": "1.0",
    "JobName": "",
    "IsEnabled": true,
    "CronScheduler": "* * * * *",
    "Command": "Download",
    "RoutingKey": "service_name" 
 }
```
2) as soon as the connector sends this command, the scheduler must accept it and register the job for execution, with the parameters that came; 
3) then he will send this command (data from the Jason model) to the exchange "Sheduler" with routingKey = service_name;

[Go Back](../../../Readme.md)