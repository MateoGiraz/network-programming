package main

import (
	"fmt"
	"log"
	"os"
	"time"

	"github.com/ORT-PDR/M6C_241195_256345_231355/mail-service/event"
	amqp "github.com/rabbitmq/amqp091-go"
)

type Config struct {
	Rabbit *amqp.Connection
}

func main() {
	rabbitURL := getRabbitURL()
	rabbitConn, err := connect(rabbitURL)
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

func getRabbitURL() string {
	rabbitURL := os.Getenv("RABBITMQ_URL")
	if rabbitURL == "" {
		rabbitURL = "amqp://guest:guest@rabbitmq"
	}

	return rabbitURL
}

func connect(rabbitURL string) (*amqp.Connection, error) {
	var connection *amqp.Connection

	for {
		c, err := amqp.Dial(rabbitURL)
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
