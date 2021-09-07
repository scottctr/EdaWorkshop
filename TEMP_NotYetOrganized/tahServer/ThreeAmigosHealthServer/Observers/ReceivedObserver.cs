using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using BusinessLogic;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeAmigosHealthServer.Observers
{
    public class ReceivedObserver
    {
        private EventProcessorClient processor;

        public async Task Start()
        {
            //Listen to NotDecided event
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromMinutes(10));

            var storageConnectionString = "!!!storageConnectString!!!";
            var blobContainerName = "uireceivedrequestsconsumer";

            var eventHubsConnectionString = "!!!eventHubsConnectionString!!!";
            var eventHubName = "receivedrequests";
            var consumerGroup = "$Default"; ///!!! let's not use $Default consumer groups

            var storageClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubsConnectionString, eventHubName);

            processor.ProcessEventAsync += processEventHandler;
            processor.ProcessErrorAsync += processErrorHandler;

            await processor.StartProcessingAsync(cancellationSource.Token);

            //try
            //{
            //    // The processor performs its work in the background; block until cancellation
            //    // to allow processing to take place.
            //    await Task.Delay(Timeout.Infinite, cancellationSource.Token);
            //}
            //catch (TaskCanceledException)
            //{
            //    // This is expected when the delay is canceled.
            //    Console.WriteLine("Cancelled");
            //}

            //try
            //{
            //    await processor.StopProcessingAsync(cancellationSource.Token);
            //}
            //finally
            //{
            //    // To prevent leaks, the handlers should be removed when processing is complete.
            //    processor.ProcessEventAsync -= processEventHandler;
            //    processor.ProcessErrorAsync -= processErrorHandler;
            //}
        }

        private async Task processEventHandler(ProcessEventArgs eventArgs)
        {
            string messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());

            var rfs = JsonSerializer.Deserialize<RequestForService>(messageBody);
            RequestForServiceMetrics.Add(rfs);

            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private static Task processErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            //!!!!
            return Task.CompletedTask;
        }
    }
}
