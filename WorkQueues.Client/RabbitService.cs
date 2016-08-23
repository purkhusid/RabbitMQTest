using System;
using System.Text;
using RabbitMQ.Client;

namespace WorkQueues.Client
{
    public class RabbitService : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = ""; //Default Queue
        private const string QueueName = "WorkQueuesDemo";

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
            channel.BasicQos(0, 1, false);
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
