using System;
using System.Text;
using RabbitMQ.Client;

namespace publisher_api.Services
{
    // define interface and service
    public interface IMessageService
    {
        bool Enqueue(string message);
    }

    public class MessageService : IMessageService
    {
        public bool Enqueue(string messageString)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq:5672" };
            factory.UserName = "guest";
            factory.Password = "guest";
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = Encoding.UTF8.GetBytes(messageString);

                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
                Console.WriteLine(" [x] Sent {0}", messageString);
            }
            return true;
        }
    }
}

