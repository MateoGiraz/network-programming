start:
    @echo "Running RabbitMQ container..."
    docker run -d \
      --name rabbitmq \
      -p 5672:5672 \
      -p 15672:15672 \
      rabbitmq:3.9-management-alpine
    @echo "RabbitMQ container is now running."

.PHONY: run-rabbitmq



docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management-alpine