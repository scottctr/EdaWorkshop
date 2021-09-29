using System;

namespace BusinessLogic
{
    public class RequestForService
    {
        public string Id { get; set; }
        public RequestedService RequestedService { get; set; }
        public Priority Priority { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime TimeDue { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string MemberId { get; set; }
        public Status Status { get; set; }
        public string AssignedToUser { get; set; }
    }
}
