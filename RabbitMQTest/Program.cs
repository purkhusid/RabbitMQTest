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
        static void Main(string[] args)
        {
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                Password = "guest",
                UserName = "guest",
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            model.QueueDeclare("MyQueue", true, false, false, null);
            Console.WriteLine("Queue created");

            model.ExchangeDeclare("MyExchange", ExchangeType.Topic);
            Console.WriteLine("Exchange created");

            model.QueueBind("MyQueue", "MyExchange", "cars");
            Console.WriteLine("My Queue bound to MyExchange");

            Console.ReadLine();
        }
    }
}
