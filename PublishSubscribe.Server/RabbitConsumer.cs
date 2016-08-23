using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PublishSubscribe.Server
{
    public class RabbitConsumer : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "PublishSubscribeExchangeDemo";
        private const string QueueName = "PublishSubscribeDemoQueue";

        private readonly IConnection connection;
        private readonly IModel channel;

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
            channel.ExchangeDeclare(ExchangeName, "fanout", true);
            channel.QueueDeclare(QueueName, true, false, false, null);
            channel.QueueBind(QueueName, ExchangeName, "");
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

            channel.BasicConsume(QueueName, false, consumer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}