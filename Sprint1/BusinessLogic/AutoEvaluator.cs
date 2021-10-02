using System;

namespace BusinessLogic
{
    public class AutoEvaluator
    {
        private static readonly Random _randomizer = new Random();

        public static bool Approve()
        {
            return _randomizer.Next(0, 2) == 1;
        }
    }
}
