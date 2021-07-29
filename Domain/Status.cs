namespace ProviderTransferServiceFunction.Domain
{
    public enum Status
    {
        Received,
        Assigned,
        AwaitingInfo,
        Denied,
        AutoApproved,
        Approved
    }
}