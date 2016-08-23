using System;

namespace Routing.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ Message Sender");
            Console.WriteLine();
            Console.WriteLine();

            var messageCount = 0;
            var rabbitService = new RabbitService();

            while (true)
            {
                Console.WriteLine("Please insert a topic: ");
                var topic = Console.ReadLine();

                var message = $"Message: {messageCount}";
                Console.WriteLine($"Sending - {message}");
                rabbitService.Send(message, topic);
                messageCount++;
            }
        }
    }
}
