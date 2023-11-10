# Starting mail service

If Make installed, run ```make start```

If not, run the following command

```
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management-alpine
```