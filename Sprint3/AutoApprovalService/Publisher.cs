using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoApprovalService
{
    public static class Publisher
    {
        private static string _approvedConnectionString = "!!!";
        private static string _notApprovedConnectionString = "!!!";
        private static readonly EventHubProducerClient _approvedPublisher;
        private static readonly EventHubProducerClient _notApprovedPublisher;

        static Publisher()
        {
            _approvedPublisher = new EventHubProducerClient(_approvedConnectionString);
            _notApprovedPublisher = new EventHubProducerClient(_notApprovedConnectionString);
        }

        public static async Task SendApprovalAsync(string requestJson)
        {
            await _approvedPublisher.SendAsync(GetEventData(requestJson));
        }

        public static async Task SendNonApprovalAsync(string requestJson)
        {
            await _notApprovedPublisher.SendAsync(GetEventData(requestJson));
        }

        private static List<EventData> GetEventData(string requestJson)
        {
            var requestBytes = Encoding.UTF8.GetBytes(requestJson);
            var eventDataList = new List<EventData>(1) { new EventData(new BinaryData(requestBytes)) };
            return eventDataList;
        }
    }
}