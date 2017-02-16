using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Order Service");
            Console.WriteLine();
            Console.WriteLine();

            var messageCount = 0;
            var rabbitService = new RabbitService();

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                    break;

                if (key.Key == ConsoleKey.C)
                {
                    var message = $"OrderCreated: {messageCount}";
                    Console.WriteLine($"Sending - {message}");
                    rabbitService.Send(message, "order.events.created");
                    messageCount++;
                }

                if (key.Key == ConsoleKey.P)
                {
                    var message = $"OrderProcessed: {messageCount}";
                    Console.WriteLine($"Sending - {message}");
                    rabbitService.Send(message, "order.events.processed");
                    messageCount++;
                }
            }

            Console.ReadLine();
        }
    }
}
