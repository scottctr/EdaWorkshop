namespace AutoApprovalService
{
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
            private static string _approvedConnectionString = "Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=Publisher;SharedAccessKey=KQL2H1MoSav3XrVhriz8/XFlYwIuF9/8tG5U4cyHP+M=;EntityPath=requestdecided";
            private static string _notApprovedConnectionString = "Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=Listener;SharedAccessKey=a/9Y+FyFnH8D1bWDDskAiPCND9fHKOUqPkYK7/ix0YA=;EntityPath=requestnotdecided";
            private static EventHubProducerClient _approvedPublisher;
            private static EventHubProducerClient _notApprovedPublisher;

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
}