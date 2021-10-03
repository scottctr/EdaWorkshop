namespace UI
{
    public static class Variables
    {
        public const string StorageAccountConnectionString = "!!!";

        // Assigned Observer
        public const string AssignedHubName = "requestassigned";
        public const string AssignedHubListenerConnectionString = "!!!";
        public const string AssignedHubConsumerGroupName = "uirequestassignedconsumer";
        public const string AssignedHubConsumerGroupStorageContainerName = "uirequestassignedconsumer";

        // Decision Publisher
        public const string DecisionHubSenderConnectionString = "!!!";

        // Get Request Publisher
        public const string GetRequestHubSenderConnectionString = "!!!";


        //*** Don't worry about anything below here if not using Dashboard ***
        public const bool UseDashboard = false;

        // Decided Observer
        public const string DecidedHubName = "decidedrequests";
        public const string DecidedHubListenerConnectionString = "!!!";
        public const string DecidedHubConsumerGroupName = "uirequestdecidedconsumer";
        public const string DecidedHubConsumerGroupStorageContainerName = "uirequestdecidedconsumer";

        // Received Observer
        public const string ReceivedHubName = "receivedrequests";
        public const string ReceivedHubListenerConnectionString = "!!!";
        public const string ReceivedHubConsumerGroupName = "uireceivedrequestconsumer";
        public const string ReceivedHubConsumerGroupStorageContainerName = "uireceivedrequestsconsumer";
    }
}
