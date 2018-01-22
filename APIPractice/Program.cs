using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace APIPractice
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1
            var request = new RestRequest("Accounts/AC83afb7a1e4f2ee01be73562db78d8fae/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("AC83afb7a1e4f2ee01be73562db78d8fae", "5590f3b4e599dd15941cce15ce0afcf8");
            //2
            var response = new RestResponse();
            //3a
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //4
            Console.WriteLine(response.Content);
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Message> messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (Message message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }

        //3b
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

        public class Message
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Body { get; set; }
            public string Status { get; set; }
        }
    }
}
