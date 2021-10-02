using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using BusinessLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProviderTransferServiceFunction
{
    public static class Function1
    {
        private static string _connectionString = "!!!";
        private static EventHubProducerClient _publisher;

        // For timer trigger syntax, see https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression
        // "* * * * * *" fires every second
        // "* * * * *" fires every minute
        [FunctionName("SendRequestsFunction")]
        public static async void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        {
            _publisher = new EventHubProducerClient(_connectionString);

            var requestList = new List<EventData>();
            var request = RequestGenerator.GetRandomRequest();
            var requestJson = System.Text.Json.JsonSerializer.Serialize(request);
            var requestBytes = Encoding.UTF8.GetBytes(requestJson);

            requestList.Add(new EventData(new BinaryData(requestBytes)));
            Console.WriteLine("Request received: " + requestJson);

            await _publisher.SendAsync(requestList);
        }
    }
}
