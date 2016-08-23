using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace OneWayMessaging.Client
{
    public class RabbitService : IDisposable
    {
        private string hostName = "localhost";
        private string username = "guest";
        private string password = "guest";
        private string exchangeName = "";
        private string queueName = "OneWayMessagingDemo";
        private bool isDuable = true;

        private string virtualHost = "";
        private int port = 0;

        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;

        public RabbitService()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = username,
                Password = password
            };

            if (!string.IsNullOrEmpty(virtualHost))
                connectionFactory.VirtualHost = virtualHost;

            if (port > 0)
                connectionFactory.Port = port;

            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void CreateQueue()
        {
            channel.QueueDeclare(queueName, true, false, false, null);
        }

        public void Send(string message)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes(message);

            channel.BasicPublish(exchangeName, queueName, properties, messageBuffer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
