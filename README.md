# Stock Order
Order to buy Bitcoin regularly on a certain day of each month

## Tech
* .NET 7
* DB: SQL LocalDB
* ORM: EFCore
* Packages: FluentValidation, Mapster, SystemTextJsonPatch
* MessageBroker: RabbitMQ
* ServiceBus: MassTransit
* UnitTest : XUnit, Moq

## Requirements
* The user can give orders for the 1-28 days of the month.
* The user can give a minimum order of 100 TL and a maximum of 20,000 TL.
* A user can have only 1 active order.
* The user can cancel the order.
* The user can view the given order.
* The user can filter and list the canceled orders.
* The user must choose which channels he wants to be informed through. user at once can make more choices.
  ○ SMS
  ○ Email
  ○ Push Notification
* The user can list the notification channels of the order.
* After the order is given successfully, the user wants to receive notification.
  Channel should be informed. At this point, an imaginary http for the notification process
  Sampling is expected upon request.
* Every notification should be logged at the database level. It should be possible to answer the questions of which order was made on which channel, when and with which information letter.


### System Design
[system design](https://miro.com/app/board/uXjVOFq5Lqg=/#tpicker-content)
![Screenshot](https://github.com/gulizay91/stock-order/blob/main/etc/system-design.png?raw=true)

### Sample Data
![sample data order](https://github.com/gulizay91/stock-order/blob/main/etc/ss-sql-1.png?raw=true)
![sample data orderNotification](https://github.com/gulizay91/stock-order/blob/main/etc/ss-sql-2.png?raw=true)
![sample data notification](https://github.com/gulizay91/stock-order/blob/main/etc/ss-sql-3.png?raw=true)

### Api Documentation (Swagger)
![sample data notification](https://github.com/gulizay91/stock-order/blob/main/etc/api-documentation.png?raw=true)

### Startup Projects
* Order.Api
* Notification.Consumer

## Docker
if you need rabbitmq on docker
```sh
docker run -d --hostname rabbit --name rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
if you need mssql on docker in macos
```sh
docker pull mcr.microsoft.com/azure-sql-edge
```
```sh
sudo docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=P@ssw0rd123!' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/azure-sql-edge
```
start docker image mssql
```sh
sudo docker start sqlserver
```

## Getting Started

Steps to build a Docker image:

1. Clone this repo
```sh
git clone https://github.com/gulizay91/stock-order.git
```

2. Run docker compose

```sh
docker-compose up -d
```

3. Send health check

```sh
curl -L -X GET 'http://localhost:5112/health'
```

You can check data separated db

db: stock-order
```
Server Name: localhost,1433
SqlServer
    UserName: sa
    Password: P@ssw0rd123!
```
db: stock-notification
```
Server Name: localhost,1434
SqlServer
    UserName: sa
    Password: P@ssw0rd123!
```

## License
[MIT](https://choosealicense.com/licenses/mit/)
