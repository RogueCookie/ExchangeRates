# Tracking the exchange rate of the Czech crown
# Scheduler Api 
Service for creating scheduled tasks. Implements API and logic for adding / editing scheduled tasks 

### Logic of adding new scheduled jobs 
We need to dynamically add jobs to the scheduler. And we have two options:

#### Through the Rest request 


#### Through a message in â exchange Scheduler
We use the next model whuch we read from json file
```
{
    "Version": "1.0",
    "JobName": "",
    "IsEnabled": true,
    "CronSchedule": "* * * * *",
    "Command": "Download",
    "RoutingKey": "service_name" 
 }
```

[Go Back](../../../Readme.md)