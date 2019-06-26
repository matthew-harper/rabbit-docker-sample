using System;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace worker
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task PostToWebApi(string postData)
        {
            var json = JsonConvert.SerializeObject(postData);
            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.PostAsync("http://publisher_api:80/api/Values", content);

            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);
        }

        static void Main(string[] args)
        {
            string[] testStrings = new string[] {"one", "two", "three", "four", "five"};
            Console.WriteLine("Sleeping!");
            Task.Delay(10000).Wait();
            Console.WriteLine("Done Sleeping!");
            Console.WriteLine("Posting messages to webApi");
            for(int i = 0; i < 5; i++)
            { 
                PostToWebApi(testStrings[i]).Wait();
            }

            Console.WriteLine("Consuming Queue Now");
            
            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received from Rabbit: {0}", message);
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
