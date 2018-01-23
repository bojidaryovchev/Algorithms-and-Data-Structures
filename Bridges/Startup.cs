using System.Linq.Expressions;

namespace Bridges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Startup
    {
        public static void Main()
        {
            // Solving Bridges
            int[] numbers = HandleInput(Console.ReadLine)
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            int totalBridges = 0;
            int lastBridgeFinishIndex = 0;
            string[] bridgesRepresentation = new string[numbers.Length];

            for (int index = 0; index < numbers.Length; index++)
            {
                int currentNumber = numbers[index];
                bridgesRepresentation[index] = "X";

                for (int j = 1; index - j >= lastBridgeFinishIndex; j++)
                {
                    int number = numbers[index - j];

                    if (number == currentNumber)
                    {
                        lastBridgeFinishIndex = index;
                        bridgesRepresentation[index] = numbers[index].ToString();
                        bridgesRepresentation[index - j] = numbers[index - j].ToString();
                        totalBridges++;
                    }
                }
            }

            string result;

            if (totalBridges == 0)
            {
                result = string.Format("No bridges found\n{0}", CreateString("X ", numbers.Length).TrimEnd());
            }
            else if (totalBridges == 1)
            {
                result = string.Format("1 bridge found\n{0}", string.Join(" ", bridgesRepresentation));
            }
            else
            {
                result = string.Format("{0} bridges found\n{1}", totalBridges, string.Join(" ", bridgesRepresentation));
            }

            Console.WriteLine(result);
        }

        private static string HandleInput(Func<string> inputFunc)
        {
            string input = inputFunc.Invoke();

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty!");
            }

            return input;
        }

        private static string CreateString(string part, int count)
        {
            ICollection<string> createdString = new LinkedList<string>();

            for (int i = 0; i < count; i++)
            {
                createdString.Add(part);
            }

            return string.Join(string.Empty, createdString);
        }
    }
}
