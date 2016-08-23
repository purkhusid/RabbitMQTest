using System;
using System.Text;
using RabbitMQ.Client;

namespace OneWayMessaging.Client
{
    public class RabbitService : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "";
        private const string QueueName = "OneWayMessagingDemo";

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
            channel.QueueDeclare(QueueName, true, false, false, null);
        }

        public void Send(string message)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes(message);

            channel.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
