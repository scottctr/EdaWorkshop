using System;

namespace ProviderTransferServiceFunction.Domain
{
    public static class Randomizer
    {
        private static readonly Random _random = new Random();

        public static string GetMemberId()
        {
            var length = _random.Next(6, 12);
            var memberId = string.Empty;
            for (var i = 0; i < length; i++)
            {
                memberId += _random.Next(2) == 1
                    ? _random.Next(9)
                    : GetLetterCapital();
            }

            return memberId;
        }

        public static string GetName()
        {
            var length = _random.Next(2, 12);
            var name = GetLetterCapital().ToString();
            for (var i = 1; i < length; i++)
            { name += GetLetterLower(); }

            return name;
        }

        public static Priority GetPriority()
        {
            return _random.Next(10) > 8
                ? Priority.High
                : Priority.Standard;
        }

        public static RequestedService GetRequestedService()
        {
            return GetEnum<RequestedService>(skipUnknown: false);
        }

        private static T GetEnum<T>(bool skipUnknown = false) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));

            var randomIndex = skipUnknown
                ? _random.Next(1, values.Length)
                : _random.Next(values.Length);

            return (T)values.GetValue(randomIndex);
        }

        private static char GetLetterCapital()
        {
            //!!!
            return (char) _random.Next(61, 88);
        }

        private static char GetLetterLower()
        {
            //!!!
            return (char)_random.Next(89, 115);
        }

        public static DateTime GetTimeReceived()
        {
            return DateTime.Now.AddMinutes(_random.Next(240) * -1);
        }
    }
}