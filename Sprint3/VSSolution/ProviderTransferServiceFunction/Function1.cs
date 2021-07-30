using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ProviderTransferServiceFunction
{
    public static class Function1
    {
        private static string _connectionString = "Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=Publisher;SharedAccessKey=jTvz2h4hr1mjndKPxnTY74n8IPh0PWq//OwV3a562TY=;EntityPath=requestreceived";
        private static EventHubProducerClient _publisher;

        [FunctionName("Function1")]
        public static async void Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _publisher = new EventHubProducerClient(_connectionString);

            var request = RequestGenerator.GetRandomRequest();
            var requestJson = JsonSerializer.Serialize(request);

            var requestBytes = Encoding.UTF8.GetBytes(requestJson);
            await _publisher.SendAsync(new List<EventData>(1) { new EventData(new BinaryData(requestBytes)) });

            log.LogInformation("Request received: " + requestJson);
        }
    }
}
