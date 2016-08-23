using System;
using System.Text;
using RabbitMQ.Client;

namespace Routing.Client
{
    public class RabbitService : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "TopicsDemo"; //topic type exchange

        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitService()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = Username,
                Password = Password
            };

            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, "topic", true);
        }

        public void Send(string message, string topic)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes(message);

            //We are using a direct exchange now.
            //Queues specify what topics they are interested in
            channel.BasicPublish(ExchangeName, topic, properties, messageBuffer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
