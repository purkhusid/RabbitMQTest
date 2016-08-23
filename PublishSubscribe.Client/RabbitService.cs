using System;
using System.Text;
using RabbitMQ.Client;

namespace PublishSubscribe.Client
{
    public class RabbitService : IDisposable
    {
        private const string HostName = "localhost";
        private const string Username = "guest";
        private const string Password = "guest";
        private const string ExchangeName = "PublishSubscribeExchangeDemo"; //Fanout exchange

        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitService()
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
        }

        public void Send(string message)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var messageBuffer = Encoding.Default.GetBytes(message);

            //No routing key because the exchange is a fanout exchange.
            //All messages will be sent to all queues subscribing to the exchange
            channel.BasicPublish(ExchangeName, "", properties, messageBuffer);
        }

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
