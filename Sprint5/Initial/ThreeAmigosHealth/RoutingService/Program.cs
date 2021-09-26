using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using BusinessLogic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RoutingService
{
    class Program
    {
        // User specific settings
        private const string BlobStorageConnectionString = "!!!";
        private const string GetRequestEventHubConnectionString = "!!!";
        private const string NotAutoApprovedEventHubConnectionString = "!!!";
        private const string RequestAssignedEventHubConnectionString = "!!!";
        private const string RequestDecidedEventHubConnectionString = "!!!";


        // Should be standard settings, but double check
        private const string GetRequestBlobContainerName = "routinggetrequestconsumer";
        private const string GetRequestEventHubName = "getrequest";
        private const string GetRequestConsumerGroup = "routinggetconsumer";
        private const string NotAutoApprovedBlobContainerName = "routingnotautoapprovedconsumer";
        private const string NotAutoApprovedEventHubName = "notautoapprovedrequests";
        private const string NotAutoApprovedConsumerGroup = "routingnotautoapprovedconsumer";
        private const string RequestDecidedBlobContainerName = "routingrequestdecidedconsumer";
        private const string RequestDecidedEventHubName = "requestdecidedrequests";
        private const string RequestDecidedConsumerGroup = "routingrequestdecidedconsumer";

        // Private fields
        private static readonly BlockingCollection<RequestForService> Requests = new();
        private static EventHubProducerClient _requestAssignedPublisher;
        private static EventProcessorClient _getRequestConsumer;
        private static EventProcessorClient _notAutoApprovedConsumer;
        private static EventProcessorClient _requestDecidedConsumer;


        static async Task Main(string[] args)
        {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromHours(4));

            initializeRequestAssignedProducer();
            await startEventConsumers(cancellationSource);

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
                await stopEventConsumers(cancellationSource);
            }
            finally
            {
                UnassignEventHandlers();
                await DisposeAsync();
            }
        }

        private static async Task DisposeAsync()
        {
            Requests?.Dispose();
            await _requestAssignedPublisher?.DisposeAsync().AsTask();
        }

        private static async Task getRequestEventHandlerAsync(ProcessEventArgs arg)
        {
            if (Requests.TryTake(out var request))
            {
                //!!!set user name so UI knows it for current user

                var requestJson = System.Text.Json.JsonSerializer.Serialize(request);
                //!!!Console.WriteLine($"[GetRequestConsumerError]: {arg.Exception.Message}");

                var requestBytes = Encoding.UTF8.GetBytes(requestJson);


                await _requestAssignedPublisher.SendAsync(new List<EventData>{ new (new BinaryData(requestBytes)) });
            }
        }

        private static Task getRequestErrorHandlerAsync(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"[GetRequestConsumerError]: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }

        private static void initializeRequestAssignedProducer()
        {
            _requestAssignedPublisher = new EventHubProducerClient(RequestAssignedEventHubConnectionString);
        }

        private static Task notAutoApprovedErrorHandlerAsync(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"[NotAutoApprovedConsumerError]: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }

        private static async Task notAutoApprovedEventHandlerAsync(ProcessEventArgs eventArgs)
        {
            var messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());

            var rfs = JsonSerializer.Deserialize<RequestForService>(messageBody);
            Requests.Add(rfs);

            Console.WriteLine("Router received not-auto approved request: " + messageBody);

            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private static Task requestDecidedErrorHandlerAsync(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"[RequestDecidedConsumerError]: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }

        private static async Task requestDecidedEventHandlerAsync(ProcessEventArgs eventArgs)
        {
            // NOTE: This isn't needed for the workshop since we don't need to route auto approved or assigned requests.

            var messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            var rfs = JsonSerializer.Deserialize<RequestForService>(messageBody);

            //Requests.Delete(rfs);
            Console.WriteLine("Router received decided request: " + messageBody);

            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        private static async Task startEventConsumers(CancellationTokenSource cancellationSource)
        {
            await startGetRequestConsumer(cancellationSource);
            await startNotAutoApprovedConsumer(cancellationSource);
            await startRequestDecidedConsumer(cancellationSource);
        }

        private static async Task startGetRequestConsumer(CancellationTokenSource cancellationSource)
        {
            var storageClient = new BlobContainerClient(BlobStorageConnectionString, GetRequestBlobContainerName);
            _getRequestConsumer = new EventProcessorClient(storageClient, GetRequestConsumerGroup, GetRequestEventHubConnectionString); //!!!, GetRequestEventHubName);

            _getRequestConsumer.ProcessEventAsync += getRequestEventHandlerAsync;
            _getRequestConsumer.ProcessErrorAsync += getRequestErrorHandlerAsync;

            await _getRequestConsumer.StartProcessingAsync(cancellationSource.Token);
        }

        private static async Task startNotAutoApprovedConsumer(CancellationTokenSource cancellationSource)
        {
            var storageClient = new BlobContainerClient(BlobStorageConnectionString, NotAutoApprovedBlobContainerName);
            _notAutoApprovedConsumer = new EventProcessorClient(storageClient, NotAutoApprovedConsumerGroup, NotAutoApprovedEventHubConnectionString); //!!!, NotAutoApprovedEventHubName);

            _notAutoApprovedConsumer.ProcessEventAsync += notAutoApprovedEventHandlerAsync;
            _notAutoApprovedConsumer.ProcessErrorAsync += notAutoApprovedErrorHandlerAsync;

            await _notAutoApprovedConsumer.StartProcessingAsync(cancellationSource.Token);
        }

        private static async Task startRequestDecidedConsumer(CancellationTokenSource cancellationSource)
        {
            var storageClient = new BlobContainerClient(BlobStorageConnectionString, RequestDecidedBlobContainerName);
            _requestDecidedConsumer = new EventProcessorClient(storageClient, RequestDecidedConsumerGroup, RequestDecidedEventHubConnectionString); //!!!???, RequestDecidedEventHubName);

            _requestDecidedConsumer.ProcessEventAsync += requestDecidedEventHandlerAsync;
            _requestDecidedConsumer.ProcessErrorAsync += requestDecidedErrorHandlerAsync;

            await _requestDecidedConsumer.StartProcessingAsync(cancellationSource.Token);
        }

        private static async Task stopEventConsumers(CancellationTokenSource cancellationSource)
        {
            await _getRequestConsumer.StopProcessingAsync(cancellationSource.Token);
            await _notAutoApprovedConsumer.StopProcessingAsync(cancellationSource.Token);
            await _requestDecidedConsumer.StopProcessingAsync(cancellationSource.Token);
        }

        private static void UnassignEventHandlers()
        {
            // To prevent leaks, the handlers should be removed when processing is complete.
            _getRequestConsumer.ProcessEventAsync -= getRequestEventHandlerAsync;
            _getRequestConsumer.ProcessErrorAsync -= getRequestErrorHandlerAsync;
            _notAutoApprovedConsumer.ProcessEventAsync -= notAutoApprovedEventHandlerAsync;
            _notAutoApprovedConsumer.ProcessErrorAsync -= notAutoApprovedErrorHandlerAsync;
            _requestDecidedConsumer.ProcessEventAsync -= requestDecidedEventHandlerAsync;
            _requestDecidedConsumer.ProcessErrorAsync -= requestDecidedErrorHandlerAsync;
        }
    }
}
