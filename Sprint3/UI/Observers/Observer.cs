using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UI.Observers
{
    public abstract class Observer<T>
    {
        private readonly EventProcessorClient _processor;
        private readonly Action<T> _callback;

        public Observer(string hubConsumerStorageContainerName
            , string storageConnectionString
            , string eventHubName
            , string hubListenerConnectionString
            , string hubConsumerGroupName
            , Action<T> callback)
        {
            var storageClient = new BlobContainerClient(storageConnectionString, hubConsumerStorageContainerName);

            var options = new EventProcessorClientOptions { CacheEventCount = 100, PrefetchCount = 250 };
            _processor = new EventProcessorClient(storageClient, hubConsumerGroupName, hubListenerConnectionString, eventHubName, options);
            _callback = callback;

            _processor.ProcessEventAsync += processEventHandler;
            _processor.ProcessErrorAsync += processErrorHandler;
        }

        public async Task StartAsync(CancellationToken cancelToken = default)
        {
            try
            {
                await _processor.StartProcessingAsync(cancelToken);
            }
            catch (TaskCanceledException)
            {
                // This is expected when the delay is canceled.
                Console.WriteLine($"{GetType().Name} Cancelled");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name} ERROR: {ex.Message}");
            }
        }

        private async Task StopAsync(CancellationToken cancelToken = default)
        {
            try
            {
                await _processor.StopProcessingAsync(cancelToken);
            }
            finally
            {
                _processor.ProcessEventAsync -= processEventHandler;
                _processor.ProcessErrorAsync -= processErrorHandler;
            }
        }

        private async Task processEventHandler(ProcessEventArgs eventArgs)
        {
            try
            {
                string messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());

                var item = JsonSerializer.Deserialize<T>(messageBody);

                _callback(item);

                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task processErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"{GetType().Name} ERROR: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}
