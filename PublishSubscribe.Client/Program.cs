using System;

namespace PublishSubscribe.Client
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
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                    break;

                if (key.Key == ConsoleKey.Enter)
                {
                    var message = $"Message: {messageCount}";
                    Console.WriteLine($"Sending - {message}");
                    rabbitService.Send(message);
                    messageCount++;
                }
            }

            Console.ReadLine();
        }
    }
}
