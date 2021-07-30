using System;

namespace Domain
{
    public static partial class Randomizer
    {
        private static readonly Random _random = new Random();
        private static readonly char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static string[] _firstNames;
        private static string[] _lastNames;

        public static string GetId()
        {
            return GetBase62();
        }

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

        public static string GetLastName()
        {
            ensureLastNamesLoaded();

            return _lastNames[_random.Next(_lastNames.Length)];
        }

        public static string GetFirstName()
        {
            ensureFirstNamesLoaded();

            return _firstNames[_random.Next(_firstNames.Length)];
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

        public static string GetBase62()
        {
            char[] charArray = new char[6];
            charArray[0] = _base62chars[_random.Next(62)];

            return new string(charArray);
        }

        private static void ensureFirstNamesLoaded()
        {
            if (_firstNames == null)
            { loadFirstNames(); }
        }

        private static void ensureLastNamesLoaded()
        {
            if (_lastNames == null)
            { loadLastNames(); }
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