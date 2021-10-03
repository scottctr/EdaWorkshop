using BusinessLogic;
using System;

namespace UI.Observers
{
    public class DecidedObserver: Observer<RequestForService>
    {
        public DecidedObserver(string hubConsumerStorageContainerName
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
