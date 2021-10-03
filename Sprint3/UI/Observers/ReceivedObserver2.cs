using BusinessLogic;
using System;

namespace UI.Observers
{
    public class ReceivedObserver2: Observer<RequestForService>
    {
        public ReceivedObserver2(string hubConsumerStorageContainerName
            , string storageConnectionString
            , string eventHubName
            , string hubListenerConnectionString
            , string hubConsumerGroupName
            , Action<RequestForService> callback)
            : base(hubConsumerStorageContainerName
                  , storageConnectionString
                  , eventHubName
                  , hubListenerConnectionString
                  , hubConsumerGroupName
                  , callback)
        { }
    }
}
