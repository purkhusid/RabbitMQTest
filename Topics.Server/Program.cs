using System;

namespace Routing.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ queue processor");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please insert topics the queue should receive: ");

            var topics = Console.ReadLine();

            var queueProcessor = new RabbitConsumer(topics);
            queueProcessor.Start();
            Console.ReadLine();
        }
    }
}
