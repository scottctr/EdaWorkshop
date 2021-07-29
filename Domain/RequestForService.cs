using System;

namespace ProviderTransferServiceFunction.Domain
{
    public class RequestForService
    {
        public RequestedService RequestedService { get; set; }
        public Priority Priority { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime TimeDue { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string MemberId { get; set; }
        public Status Status { get; set; }
    }
}
