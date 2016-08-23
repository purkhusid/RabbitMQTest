using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OneWayMessaging.Server
{
    public class RabbitConsumer
    {
        private string hostName = "localhost";
        private string username = "guest";
        private string password = "guest";
        private string exchangeName = "";
        private string queueName = "OneWayMessagingDemo";
        private bool isDuable = true;

        private string virtualHost = "";
        private int port = 0;

        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly IModel channel;

        public bool Enabled { get; set; }

        public RabbitConsumer()
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
            channel.BasicQos(0, 1, false);
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.Default.GetString(body);
                Console.WriteLine($"Received: {message}");
                channel.BasicAck(ea.DeliveryTag, false);
            };

            while (Enabled)
            {
                channel.BasicConsume(queueName, false, consumer);
            }
        }

        public void CreateQueue()
        {
            channel.QueueDeclare(queueName, true, false, false, null);
        }
    }
}