using BusinessLogic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ThreeAmigosHealthServer
{
    public static class RequestForServiceMetrics
    {
        private static ConcurrentDictionary<string, RequestForService> _requests = new ();
        private static ConcurrentDictionary<int, int> _countsByMinuteReceived = new ();
        private static ConcurrentDictionary<int, int> _autoApprovalsByMinuteReceived = new();
        private static ConcurrentDictionary<int, int> _totalByMinuteReceived = new();
        private static int _totalAutoApproved;
        private static DateTime _startTime = DateTime.Now;

        //!!! need to rethink open statuses
        public static int UndecidedCount => _requests.Count(r => r.Value.Status == Status.Received);
        public static int TotalCount => _requests.Count;

        public static void Add(RequestForService rfs)
        {
            _requests[rfs.Id] = rfs;

            var now = DateTime.Now;
            //_countsByMinuteReceived.AddOrUpdate(now.Hour * 100 + now.Minute, 1, (key, existingCount) => existingCount + 1);
            
            var minuteKey = now.Hour * 100 + now.Minute;

            if (rfs.Status != Status.AutoApproved)
            {
                if (_countsByMinuteReceived.TryGetValue(minuteKey, out var existingValue))
                {
                    _countsByMinuteReceived[minuteKey] = existingValue + 1;
                    _totalByMinuteReceived[minuteKey] = TotalCount;
                }
                else
                {
                    _countsByMinuteReceived[minuteKey] = 1;
                    _totalByMinuteReceived[minuteKey] = 1;
                }
            }
            else
            {
                _totalAutoApproved++;
                if (_autoApprovalsByMinuteReceived.TryGetValue(minuteKey, out var existingAutoApprovalCount))
                {
                    _autoApprovalsByMinuteReceived[minuteKey] = existingAutoApprovalCount + 1;
                }
                else
                {
                    _autoApprovalsByMinuteReceived[minuteKey] = 1;
                }
            }
        }

        public static double ElapsedMinutes => (DateTime.Now - _startTime).TotalMinutes;

        public static void Update(RequestForService rfs)
        {
            _requests[rfs.Id] = rfs;
        }

        public static int GetAutoApprovedCount() => _requests.Count(r => r.Value.Status == Status.AutoApproved);

        public static List<HourlyCount> GetAllCountsByMinute()
        {
            var hourlyList = new List<HourlyCount>(_countsByMinuteReceived.Count);

            //// Convert to Central time
            //for(var i = 0; i < 24; i++)
            //{ hourlyList.Add(new HourlyCount((i + 7) % 24, _countsByMinuteReceived[i])); }

            hourlyList.AddRange(_countsByMinuteReceived.Select(kv => new HourlyCount(kv.Key, kv.Value)));

            return hourlyList;
        }

        public static List<HourlyCount> GetAutoApprovalCountsByMinute()
        {
            var hourlyList = new List<HourlyCount>(_autoApprovalsByMinuteReceived.Count);

            hourlyList.AddRange(_autoApprovalsByMinuteReceived.Select(kv => new HourlyCount(kv.Key, kv.Value)));

            return hourlyList;
        }

        public static List<HourlyCount> GetTotalByMinute()
        {
            var hourlyList = new List<HourlyCount>(_totalByMinuteReceived.Count);

            hourlyList.AddRange(_totalByMinuteReceived.Select(kv => new HourlyCount(kv.Key, kv.Value)));

            return hourlyList;
        }

        public static double[] GetTotals()
        {
            double[] totals = new double[2];
            totals[0] = _requests.Count - _totalAutoApproved;
            totals[1] = _totalAutoApproved;

            return totals;
        }
    }

    public record HourlyCount(int Hour, int Count);
}
