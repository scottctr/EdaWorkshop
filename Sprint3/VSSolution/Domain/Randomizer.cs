using System;

namespace Domain
{
    public static partial class Randomizer
    {
        private static readonly Random _random = new Random();
        private static readonly char[] _alphaNumericChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
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
            { memberId += _alphaNumericChars[_random.Next(_alphaNumericChars.Length)]; }

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
            var base62Length = _alphaNumericChars.Length;
            charArray[0] = _alphaNumericChars[_random.Next(base62Length)];
            charArray[1] = _alphaNumericChars[_random.Next(base62Length)];
            charArray[2] = _alphaNumericChars[_random.Next(base62Length)];
            charArray[3] = _alphaNumericChars[_random.Next(base62Length)];
            charArray[4] = _alphaNumericChars[_random.Next(base62Length)];
            charArray[5] = _alphaNumericChars[_random.Next(base62Length)];

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
    }
}