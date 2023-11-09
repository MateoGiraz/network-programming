using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Config;

namespace ServerConnection.AMQP
{
    internal class TopicsQueueProvider
    {
        private readonly IModel channel;
        public TopicsQueueProvider()
        {
            ISettingsManager settingsManager = new SettingsManager();

            var rabbitMQIpAddress = settingsManager.Get(ServerConfig.RabbitMQIpConfigKey);
            channel = new ConnectionFactory() { HostName = rabbitMQIpAddress }.CreateConnection().CreateModel();
            channel.ExchangeDeclare(exchange: "mail-exchange", ExchangeType.Topic, true);
        }

        public Task<bool> SendMessage(string message)
        {
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "mail-exchange",
                    routingKey: "mail.send",
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
