using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConnection.AMQP
{
    internal class TopicsQueueProvider
    {
        private readonly IModel channel;
        public TopicsQueueProvider()
        {
            channel = new ConnectionFactory() { HostName = "localhost" }.CreateConnection().CreateModel();
            channel.ExchangeDeclare(exchange: "mails_topic", ExchangeType.Topic, true);
        }

        public Task<bool> SendMessage(string message, string topicRoutingKey)
        {
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "mails_topic",
                    routingKey: topicRoutingKey,
                    basicProperties: null,
                    body: body);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }
        }
    }
}
