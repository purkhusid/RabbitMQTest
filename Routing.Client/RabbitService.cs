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
        private const string ExchangeName = "RoutingDemo"; //Fanout exchange

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
            channel.ExchangeDeclare(ExchangeName, "direct", true);
        }

        public void Send(string message, string routingKey)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes(message);

            //We are using a direct exchange now.
            //Queues specify what routing key they are interested in
            channel.BasicPublish(ExchangeName, routingKey, properties, messageBuffer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
