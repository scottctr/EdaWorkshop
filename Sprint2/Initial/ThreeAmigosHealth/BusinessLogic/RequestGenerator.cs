using System;

namespace BusinessLogic
{
    public static class RequestGenerator
    {
        public static RequestForService GetRandomRequest()
        {
            var priority = Randomizer.GetPriority();
            var timeDue = DateTime.Now.AddDays(priority == Priority.Standard ? 3 : 1);
            return new RequestForService
            {
                // Comment to test a push
                Id = Randomizer.GetId(),
                MemberId = Randomizer.GetMemberId(),
                PatientFirstName = Randomizer.GetFirstName(),
                PatientLastName = Randomizer.GetLastName(),
                Priority = priority,
                RequestedService = Randomizer.GetRequestedService(),
                TimeReceived = DateTime.Now,
                TimeDue = timeDue,
                Status = Status.Received,
                ProviderFirstName = Randomizer.GetFirstName(),
                ProviderLastName = Randomizer.GetLastName(),

            };
        }
    }
}
