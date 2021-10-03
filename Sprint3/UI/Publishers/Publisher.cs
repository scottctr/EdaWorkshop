using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UI.Publishers
{
    public abstract class Publisher
    {
        private readonly EventHubProducerClient _publisherClient;

        public Publisher(string hubSenderConnectionString)
        {
            _publisherClient = new EventHubProducerClient(hubSenderConnectionString);
        }

        public async Task SendAsync(string requestJson)
        {
            await _publisherClient.SendAsync(GetEventData(requestJson));
        }

        private static List<EventData> GetEventData(string requestJson)
        {
            var requestBytes = Encoding.UTF8.GetBytes(requestJson);
            var eventDataList = new List<EventData>(1) { new EventData(new BinaryData(requestBytes)) };
            return eventDataList;
        }
    }
}
