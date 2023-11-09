package main

import (
	"encoding/json"
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

type AppConfig struct {
	RabbitMQ struct {
		URL string `json:"url"`
	}
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
	app, err := loadConfig("./config.json")
	if app.RabbitMQ.URL == "" || err != nil {
		rabbitURL := os.Getenv("RABBITMQ_URL")
		if rabbitURL == "" {
			rabbitURL = "amqp://guest:guest@rabbitmq"
		}
		return rabbitURL
	}
	return app.RabbitMQ.URL
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

func loadConfig(filename string) (*AppConfig, error) {
	var app AppConfig
	configFile, err := os.ReadFile(filename)
	if err != nil {
		return &AppConfig{}, err
	}

	err = json.Unmarshal(configFile, &app)
	if err != nil {
		return &AppConfig{}, err
	}
	return &app, err
}
