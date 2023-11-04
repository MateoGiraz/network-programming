package main

import (
	"fmt"
	"github.com/ORT-PDR/M6C_241195_256345_231355/mail-service/event"
	amqp "github.com/rabbitmq/amqp091-go"
	"log"
	"time"
)

type Config struct {
	Rabbit *amqp.Connection
}

func main() {
	rabbitConn, err := connect()
	if err != nil {
		log.Panic(err)
	}

	defer rabbitConn.Close()
	log.Println("Listening for and consuming RabbitMQ messages...")

	consumer, err := event.NewConsumer(rabbitConn)
	if err != nil {
		log.Panic(err)
	}

	consumer.Listen()
}

func connect() (*amqp.Connection, error) {
	var connection *amqp.Connection

	for {
		c, err := amqp.Dial("amqp://guest:guest@rabbitmq")
		if err != nil {
			fmt.Println("RabbitMQ not yet ready, backing off for two seconds...")
			time.Sleep(2 * time.Second)
			continue
		}

		log.Println("Connected to RabbitMQ")
		connection = c
		break
	}
	return connection, nil
}
