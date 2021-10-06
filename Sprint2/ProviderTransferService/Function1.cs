using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ProviderTransferService
{
    public static class Function1
    {
        private static string _connectionString = "!!!";
        private static EventHubProducerClient _publisher;

        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            _publisher = new EventHubProducerClient(_connectionString);

            var requestList = new System.Collections.Generic.List<EventData>();
            var request = BusinessLogic.RequestGenerator.GetRandomRequest();
            var requestJson = System.Text.Json.JsonSerializer.Serialize(request);
            var requestBytes = System.Text.Encoding.UTF8.GetBytes(requestJson);

            requestList.Add(new EventData(new BinaryData(requestBytes)));
            Console.WriteLine("Request received: " + requestJson);

            await _publisher.SendAsync(requestList);
        }
    }
}
