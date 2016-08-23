using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQTest
{
    class Program
    {
        public static string exchangeName = "MyExchange";
        private static string queueName = "MyQueue";

        static void Main(string[] args)
        {
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                Password = "guest",
                UserName = "guest",
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queueName, true, false, false, null);
            Console.WriteLine("Queue created");

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            Console.WriteLine("Exchange created");

            channel.QueueBind(queueName, exchangeName, "cars");
            Console.WriteLine("My Queue bound to MyExchange");

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes("This is a message");

            channel.BasicPublish(exchangeName, "cars", properties, messageBuffer);
            Console.WriteLine("Message sent");

            Console.ReadLine();
        }
    }
}
