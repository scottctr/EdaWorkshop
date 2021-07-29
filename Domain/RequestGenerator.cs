using System;

namespace ProviderTransferServiceFunction.Domain
{
    public static class RequestGenerator
    {
        public static RequestForService GetRandomRequest()
        {
            var priority = Randomizer.GetPriority();
            var timeDue = DateTime.Now.AddDays(priority == Priority.Standard ? 3 : 1);
            return new RequestForService
            {
                MemberId = Randomizer.GetMemberId(),
                PatientFirstName = Randomizer.GetName(),
                PatientLastName = Randomizer.GetName(),
                Priority = priority,
                RequestedService = Randomizer.GetRequestedService(),
                TimeReceived = DateTime.Now,
                TimeDue = timeDue,
                Status = Status.Received
            };
        }
    }
}
