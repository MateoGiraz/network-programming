package event

import (
	"encoding/json"
	"log"

	amqp "github.com/rabbitmq/amqp091-go"
)

type Consumer struct {
	conn      *amqp.Connection
	queueName string
}

func NewConsumer(conn *amqp.Connection) (Consumer, error) {
	consumer := Consumer{
		conn: conn,
	}

	err := consumer.setup()
	if err != nil {
		return Consumer{}, err
	}

	return consumer, nil
}

func (consumer *Consumer) setup() error {
	channel, err := consumer.conn.Channel()
	if err != nil {
		return err
	}
	return declareExchange(channel)
}

type Payload struct {
	User    string `json:"user"`
	Product string `json:"product"`
}

func (consumer *Consumer) Listen() error {
	ch, err := consumer.conn.Channel()
	if err != nil {
		return err
	}
	defer ch.Close()

	q, err := declareQueue(ch)
	if err != nil {
		return err
	}

	ch.QueueBind(
		q.Name,
		"mail.send",
		"mail-exchange",
		false,
		nil,
	)

	messages, err := ch.Consume(q.Name, "", true, false, false, false, nil)
	if err != nil {
		return err
	}

	for d := range messages {
		var payload Payload
		_ = json.Unmarshal(d.Body, &payload)
		log.Println("Enviando correo a", payload.User)
	}

	return nil
}
