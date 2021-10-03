namespace UI
{
    public static class Variables
    {
        public const string StorageAccountConnectionString = /*!!!*/ "DefaultEndpointsProtocol=https;AccountName=workshopstorageslc;AccountKey=OwEx4ofRUY4zyhSGpuHnC4wj+mfTsUKKmyCCRTfPUVgjTsOkruJ7YKISb11c8mKTkpH4rtl9lo/mHHOq4OsU+Q==;EndpointSuffix=core.windows.net";

        // Assigned Observer
        public const string AssignedHubName = "requestassigned";
        public const string AssignedHubListenerConnectionString = /*!!!*/"Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=RequestAssignedListenerSAP;SharedAccessKey=hhVoljT5ncMO+VnXfdqrclgI13kBYJaxluII5E6NQ2c=;EntityPath=requestassigned";
        public const string AssignedHubConsumerGroupName = "uirequestassignedconsumer";
        public const string AssignedHubConsumerGroupStorageContainerName = "uirequestassignedconsumer";

        // Decision Publisher
        public const string DecisionHubSenderConnectionString = /*!!!*/ "Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=DecidedRequestsSenderSAP;SharedAccessKey=Ki87MOgTb4Uwrl6XHYzaP3a82yiOaohI/lwlNUfkvbQ=;EntityPath=decidedrequests";

        // Get Request Publisher
        public const string GetRequestHubSenderConnectionString = /*!!!*/ "Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=GetRequestSenderSAP;SharedAccessKey=tFONpYCL0ndy2Bly+6b3IZoZHoNQfFWvXO1qrRgpiXE=;EntityPath=getrequest";


        //*** Don't worry about anything below here if not using Dashboard ***
        public const bool UseDashboard = false;

        // Decided Observer
        public const string DecidedHubName = "decidedrequests";
        public const string DecidedHubListenerConnectionString = /*!!!*/"Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=DecidedRequestsListenerSAP;SharedAccessKey=elFLSp2NBu66ehpcCUKjK3F5JH5QYxxznPGKXEt/Zx0=;EntityPath=decidedrequests";
        public const string DecidedHubConsumerGroupName = "uirequestdecidedconsumer";
        public const string DecidedHubConsumerGroupStorageContainerName = "uirequestdecidedconsumer";

        // Received Observer
        public const string ReceivedHubName = "receivedrequests";
        public const string ReceivedHubListenerConnectionString = /*!!!*/"Endpoint=sb://edaworkshop.servicebus.windows.net/;SharedAccessKeyName=ReceivedRequestsListenerSAP;SharedAccessKey=A1V6XAnwAoeL6KHbrNgOgxObeIXKO8Hy9f8uw3wFxPs=;EntityPath=receivedrequests";
        public const string ReceivedHubConsumerGroupName = "uireceivedrequestconsumer";
        public const string ReceivedHubConsumerGroupStorageContainerName = "uireceivedrequestsconsumer";
    }
}
