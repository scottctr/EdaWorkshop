using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RoutingService
{
    class Program
    {
        private static Dictionary<int, RequestForService> _requests = new();
        private static int _requestId = 1;

        static async Task Main(string[] args)
        {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromHours(4));

            const string storageConnectionString = "!!!";
            const string blobContainerName = "routerrequestnotautoapprovedcontainer";

            const string eventHubsConnectionString = "!!!";
            const string eventHubName = "requestnotautoapproved";
            const string consumerGroup = "routingconsumer";

            var storageClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            var processor = new EventProcessorClient(storageClient, consumerGroup, eventHubsConnectionString, eventHubName);

            processor.ProcessEventAsync += processEventHandler;
            processor.ProcessErrorAsync += processErrorHandler;

            await processor.StartProcessingAsync(cancellationSource.Token);

            try
            {
                // The processor performs its work in the background; block until cancellation
                // to allow processing to take place.
                await Task.Delay(Timeout.Infinite, cancellationSource.Token);
            }
            catch (TaskCanceledException)
            {
                // This is expected when the delay is canceled.
                Console.WriteLine("Cancelled");
            }

            try
            {
                await processor.StopProcessingAsync(cancellationSource.Token);
            }
            finally
            {
                // To prevent leaks, the handlers should be removed when processing is complete.
                processor.ProcessEventAsync -= processEventHandler;
                processor.ProcessErrorAsync -= processErrorHandler;
            }
        }

        private static async Task processEventHandler(ProcessEventArgs eventArgs)
        {
            var messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());

            var rfs = JsonSerializer.Deserialize<RequestForService>(messageBody);
            _requests.Add(_requestId++, rfs);

            Console.WriteLine("Router received non-decision request: " + messageBody);

            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private static Task processErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"[Error]: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}
