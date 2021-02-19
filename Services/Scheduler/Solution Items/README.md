# Tracking the exchange rate of the Czech crown
# Scheduler Api 
Service for creating scheduled tasks. Implements API and logic for adding / editing scheduled tasks 

### ������ ���������� ����� ����� ����������� �� ����������
#### ����� Rest ������
#### ����� ��������� � exchange Scheduler

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