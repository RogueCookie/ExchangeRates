# Tracking the exchange rate of the Czech crown
# ����������� �������
�����  
![Architecture](./UpdatedArchitecture.png)  
�������� � drawio [Architecture](./UpdatedArchitecture.drawio) 
������� ������������ ���� ������������� ����������� ����������� �������� ���� ������� �����.
���������� ���� �������� �� ������ https://www.cnb.cz/en/financial_markets/foreign_excha..
������������ ������ �������� �� ������ https://www.cnb.cz/en/financial_markets/foreign_excha..
���������� ����������� �������, ��������� �� �����������:
1) ��������� ���������� ���������� ��� ���������� �� ������� �������� ������ �� 2017 � 2018 ��� (������� Rate � ������� CNB).
2) ������ �� ����������, ������� ��������� ������� ���� � ��. ������ ������� ������� � ������������.
3) ����������� web API, � ������� �������� ����� �������� ����� �� ����� ����� � �������� �����/���. � ������ ���������� ������� �����������, ������������ �������� � ������� ��� ������ �� ��������� ����� ��������, �������� ������ �������������� �� ������� ������ (�.�. ���� � ������ 4 ������ - ���� ������� ��� ������ ������ ����������� ����������). ������, �� ������� �������� �����, �������� � ������������ ����������. ���������� � ������ ������������ ��� ������ � ���-�� 1 �������� �������, �.�. ��� Amount = 1.
��������, ���� � ������������ ������� ������ EUR � USD, ��� ������� 2018 ����� � txt ������� ����� ���������:
Year: 2018, month: February
Week periods:
1...2: USD - max: , min: , median: ; EUR - max: , min: , media: ;
5...9: USD - max: , min: , median: ; EUR - max: , min: , media: ;
12...16: USD - max: , min: , median: ; EUR - max: , min: , media: ;
19...23: USD - max: , min: , median: ; EUR - max: , min: , media: ;
26...28: USD - max: , min: , median: ; EUR - max: , min: , media: ;
������������� ����� ������ � ���� �������� � txt � JSON (��� ��������� � ���������� �������, ������ ������ ��� JSON ��������� ��������������).
������������ ����������: .net core, entity framework. �� ��������� �����������, ������� ����� ������������ ��� ���������� ������� - �� ��� �����.
Heroku https://ozexchangerate.herokuapp.com/ 


### �������� �������:
#### BTB�onnector
Service for downloading data from BTB Bank. Implements a console application for downloading / receiving data from remote (BTB Bank).
[Readme](./Services/BTBConnector/Readme.md)
#### Loader
Service collects data from different suppliers and converts them to the required type for storage in a database. Implements a console application with methods for getting, transforming, saving data to a database. 
[Readme](./Services/Loader/Readme.md)
#### ReportApi 
Service for providing data to the user by currency. Implements API and logic for issuing reports in two formats(txt,json) for a specific period.
[Readme](./Services/ReportApi/Readme.md)
#### Scheduler 
Service for creating scheduled tasks. Implements API and logic for adding / editing scheduled tasks 
[Readme](./Services/Scheduler/Readme.md)
