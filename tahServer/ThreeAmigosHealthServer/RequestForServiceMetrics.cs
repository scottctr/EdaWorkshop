using BusinessLogic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ThreeAmigosHealthServer
{
    public class RequestForServiceMetrics
    {
        private ConcurrentDictionary<string, RequestForService> _requests = new ();
        private ConcurrentDictionary<int, int> _countsByHourReceived = new ();

        //!!! need to rethink open statuses
        public int UndecidedCount => _requests.Count(r => r.Value.Status == Status.Received);
        public int TotalCount => _requests.Count;

        public void Add(RequestForService rfs)
        {
            _requests.TryAdd(rfs.Id, rfs);

            _countsByHourReceived.AddOrUpdate(rfs.TimeReceived.Hour, 1, (key, existingCount) => existingCount++);
        }

        public void Update(RequestForService rfs)
        {
            _requests.AddOrUpdate(rfs.Id, rfs, (existingKey, existingRfs) => rfs);
        }

        public void GetAutoApprovedCount() => _requests.Count(r => r.Value.Status == Status.AutoApproved);

        public List<HourlyCount> GetCountsByHour()
        {
            var hourlyList = new List<HourlyCount>(24);

            // Convert to Central time
            for(var i = 0; i < 24; i++)
            { hourlyList.Add(new HourlyCount((i + 7) % 24, _countsByHourReceived[i])); }

            return hourlyList;
        }
    }

    public record HourlyCount(int Hour, int Count);
}
