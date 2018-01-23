using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public static class Startup
    {
        private static void TestCustomLinkedList(ICollection<int> linkedList, int iterations)
        {
            var stopwatch = Stopwatch.StartNew();
            var random = new Random();

            for (int i = 0; i < iterations; i++)
            {
                linkedList.Add(random.Next(0, iterations));
            }

            stopwatch.Stop();
            Console.WriteLine("Adding took: {0}", stopwatch.Elapsed);
            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                bool a = linkedList.Contains(random.Next(0, iterations));
            }

            stopwatch.Stop();
            Console.WriteLine("Searching took: {0}", stopwatch.Elapsed);
        }

        public static void Main(string[] args)
        {
        }
    }
}
