using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkQueues.Server
{
    public class RabbitConsumer
    {
        private string hostName = "localhost";
        private string username = "guest";
        private string password = "guest";
        private string exchangeName = "";
        private string queueName = "WorkQueuesDemo";
        private bool isDurable = true;

        private string virtualHost = "";
        private int port = 0;

        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly IModel channel;


        public RabbitConsumer()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                //UserName = username,
                //Password = password
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
                Console.WriteLine("Starting work...");
                Thread.Sleep(1000);
                Console.WriteLine("Finished work...");

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }

        public void CreateQueue()
        {
            channel.QueueDeclare(queueName, isDurable, false, false, null);
        }
    }
}