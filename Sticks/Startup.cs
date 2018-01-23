namespace Sticks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Startup
    {
        private static int _sticksCount;
        private static int _placings;

        private static bool[] _upperSticks;
        private static bool[] _lowerSticks;

        private static HashSet<int>[] _lowerSticksByUpperStick;
        private static HashSet<int>[] _upperSticksByLowerStick;

        private static string HandleInput(Func<string> inputFunc)
        {
            string input = inputFunc.Invoke();

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty!", "inputFunc");
            }

            return input;
        }

        public static void Main()
        {
            _sticksCount = int.Parse(HandleInput(Console.ReadLine));
            _placings = int.Parse(HandleInput(Console.ReadLine));

            _upperSticks = new bool[_sticksCount];
            _lowerSticks = new bool[_sticksCount];

            _lowerSticksByUpperStick = new HashSet<int>[_sticksCount];
            _upperSticksByLowerStick = new HashSet<int>[_sticksCount];

            for (int stick = 0; stick < _sticksCount; stick++)
            {
                _lowerSticksByUpperStick[stick] = new HashSet<int>();
                _upperSticksByLowerStick[stick] = new HashSet<int>();
            }

            for (int i = 0; i < _placings; i++)
            {
                string input = HandleInput(Console.ReadLine);
                string[] inputArgs = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int upperStick = int.Parse(inputArgs[0]);
                int lowerStick = int.Parse(inputArgs[1]);

                _upperSticks[upperStick] = true;
                _lowerSticks[lowerStick] = true;

                _lowerSticksByUpperStick[upperStick].Add(lowerStick);
                _upperSticksByLowerStick[lowerStick].Add(upperStick);
            }

            ICollection<int> usedSticks = new HashSet<int>();
            ICollection<int> upmostSticks = GetUpmostSticks(usedSticks);
            
            while (upmostSticks.Any())
            {
                // take the upmost stick
                int upmostStick = upmostSticks.FirstOrDefault();

                // remove it
                _upperSticks[upmostStick] = false;

                if (_lowerSticksByUpperStick[upmostStick].Any())
                {
                    ICollection<int> lowerSticksCollection = _lowerSticksByUpperStick[upmostStick];

                    foreach (int stick in lowerSticksCollection)
                    {
                        _upperSticksByLowerStick[stick].Remove(upmostStick);

                        if (!_upperSticksByLowerStick[stick].Any())
                        {
                            _lowerSticks[stick] = false;
                        }
                    }

                    _lowerSticksByUpperStick[upmostStick] = new HashSet<int>();
                }

                // mark as used
                usedSticks.Add(upmostStick);

                upmostSticks = GetUpmostSticks(usedSticks);
            }

            if (usedSticks.Count == _sticksCount)
            {
                Console.WriteLine(string.Join(" ", usedSticks));
            }
            else
            {
                Console.WriteLine("Cannot lift all sticks\n{0}", string.Join(" ", usedSticks));
            }
        }

        private static ICollection<int> GetUpmostSticks(ICollection<int> usedSticks)
        {
            var resultingStack = new Stack<int>();

            for (int stick = 0; stick < _sticksCount; stick++)
            {
                bool isOnTopButNotBeneath = _upperSticks[stick] && !_lowerSticks[stick];
                bool isLonely = !_upperSticks[stick] && !_lowerSticks[stick];
                bool isLonelyButNotUsed = isLonely && !usedSticks.Contains(stick);

                if (isOnTopButNotBeneath || isLonelyButNotUsed)
                {
                    resultingStack.Push(stick);
                }
            }

            var result = new HashSet<int>();

            while (resultingStack.Any())
            {
                result.Add(resultingStack.Pop());
            }

            return result;
        }
    }
}
