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
        

        // private static async Task GetFromQueue()
        // {
        //     var serializer = new DataContractJsonSerializer(typeof(List<string>));
        //     client.DefaultRequestHeaders.Accept.Clear();
        //     client.DefaultRequestHeaders.Accept.Add(
        //         new MediaTypeWithQualityHeaderValue("application/json"));

        //     var streamTask = client.GetStreamAsync("http://publisher_api:80/api/Values");
        //     var resp = serializer.ReadObject(await streamTask) as List<string>;

        //     foreach (var t in resp)
        //         Console.WriteLine(t);

        // }

        public static async Task PostToWebApi(string postData)
        {           
            //client.BaseAddress = new Uri("http://publisher_api:80");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("value", postData)
            });
            content.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Accept.Clear();
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
            for(int i = 0; i < 5; i++)
            { 
                Console.WriteLine("Hello World!");
                //GetFromQueue().Wait();
                PostToWebApi(testStrings[i]).Wait();
            }
        }
    }
}
