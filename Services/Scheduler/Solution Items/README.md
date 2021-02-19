# Tracking the exchange rate of the Czech crown
# Scheduler Api 
Service for creating scheduled tasks. Implements API and logic for adding / editing scheduled tasks 

### Логика добавления новых задач выполняемых по расписанию
#### Через Rest запрос
#### Через сообщение в exchange Scheduler

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

[Go Back](../../Readme.md)