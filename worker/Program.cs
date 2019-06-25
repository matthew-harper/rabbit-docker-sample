using System;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace worker
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        

        private static async Task GetFromQueue()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<string>));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync("http://publisher_api:80/api/Values");
            var resp = serializer.ReadObject(await streamTask) as List<string>;

            foreach (var t in resp)
                Console.WriteLine(t);

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Sleeping!");
            Task.Delay(15000).Wait();
            Console.WriteLine("Done Sleeping!");
            for(int i = 0; i < 5; i++)
            { 
                Console.WriteLine("Hello World!");
                GetFromQueue().Wait();
            }
        }
    }
}
