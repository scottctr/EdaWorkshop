using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ProviderTransferServiceFunction
{
    public static class Function1
    {
        private static string _connectionString = "!!!";
        private static EventHubProducerClient _publisher;

        [FunctionName("Function1")]
        public static async void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            var config = GetConfig(context);
            //Console.WriteLine("name: " + config["name"]);


            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _publisher = new EventHubProducerClient(_connectionString);

            var batchSize = int.Parse(config["batchSize"]);
            await _publisher.SendAsync(GetRequests(batchSize));
        }

        private static List<EventData> GetRequests(int count)
        {
            var requestList = new List<EventData>(count);
            for(var i = 0; i < count; i++)
            {
                var request = RequestGenerator.GetRandomRequest();
                var requestJson = JsonSerializer.Serialize(request);

                var requestBytes = Encoding.UTF8.GetBytes(requestJson);
                requestList.Add(new EventData(new BinaryData(requestBytes)));
                Console.WriteLine($"Request {i} received: " + requestJson);
            }

            return requestList;
        }

        private static IConfigurationRoot GetConfig(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    // This gives you access to your application settings 
                    // in your local development environment
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    // This is what actually gets you the 
                    // application settings in Azure
                    .AddEnvironmentVariables()
                    .Build();
        }
    }
}
