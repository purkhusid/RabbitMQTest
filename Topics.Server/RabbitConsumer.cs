using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Routing.Server
{
    public class RabbitConsumer : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "TopicsDemo"; //topic type exchange
        private readonly string queueName;

        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitConsumer(string topics)
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

            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, ExchangeName, topics);
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
                Console.WriteLine("Starting work...");
                Thread.Sleep(1000);
                Console.WriteLine("Finished work...");

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}