using System;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace worker
{
    public class todo
    {
        public string name;
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        

        private static async Task GetFromQueue()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<todo>));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            //var stringTask = client.GetStringAsync("http://127.0.0.1:8080/api/Values");
            var streamTask = client.GetStreamAsync("http://127.0.0.1:8080/api/Values");
            var resp = serializer.ReadObject(await streamTask) as List<todo>;

            foreach (var t in resp)
                Console.WriteLine(t.name);

        }

        static void Main(string[] args)
        {
            for(int i = 0; i < 500; i++)
                Console.WriteLine("Hello World!");
                GetFromQueue().Wait();
        }
    }
}
