using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using BusinessLogic;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UI.Observers
{
    public class ApprovedObserver
    {
        private EventProcessorClient processor;

        public async Task Start()
        {
            //Listen to Decided event
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromMinutes(10));

            var storageConnectionString = "!!!storageConnectString!!!";
            var blobContainerName = "uiapprovedrequestsconsumer";

            var eventHubsConnectionString = "!!!eventHubsConnectionString!!!";
            var eventHubName = "decidedrequests";
            var consumerGroup = "uidecidedrequestsconsumer";

            var storageClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            var options = new EventProcessorClientOptions { CacheEventCount = 100, PrefetchCount = 250 };
            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubsConnectionString, eventHubName, options);

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
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
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
            try
            {
                string messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());

                var rfs = JsonSerializer.Deserialize<RequestForService>(messageBody);
                RequestForServiceMetrics.Add(rfs);

                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Task processErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            //!!!!
            return Task.CompletedTask;
        }
    }
}
