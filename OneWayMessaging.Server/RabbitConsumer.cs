using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OneWayMessaging.Server
{
    public class RabbitConsumer : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string QueueName = "OneWayMessagingDemo";

        private readonly IModel channel;
        private readonly IConnection connection;

        public RabbitConsumer()
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

            channel.BasicConsume(QueueName, false, consumer);
        }

        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}