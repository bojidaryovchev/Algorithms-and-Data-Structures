namespace MessageSharing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Startup
    {
        private const string ConnectionPatern = @"(\w+) - (\w+)";
        private static readonly Regex ConnectionRegex = new Regex(ConnectionPatern);
        private static readonly char[] SplitSymbols = { ' ', ',', ':' };

        private static readonly IDictionary<string, ICollection<string>> KnownPeopleByPerson =
            new Dictionary<string, ICollection<string>>();
        private static readonly ICollection<string> PeopleWhoDontKnow = new SortedSet<string>();
        private static readonly ICollection<string> PeopleWhoAlreadyKnow = new HashSet<string>();

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
            string[] participants = HandleInput(Console.ReadLine)
                .Split(SplitSymbols, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .ToArray();

            foreach (string participant in participants)
            {
                KnownPeopleByPerson[participant] = new HashSet<string>();
                // at first nobody knows
                PeopleWhoDontKnow.Add(participant);
            }

            string connections = HandleInput(Console.ReadLine);
            var connectionMatches = ConnectionRegex.Matches(connections);

            foreach (Match connectionMatch in connectionMatches)
            {
                string parent = connectionMatch.Groups[1].Value;
                string child = connectionMatch.Groups[2].Value;

                // friendship is mutual
                KnownPeopleByPerson[parent].Add(child);
                KnownPeopleByPerson[child].Add(parent);
            }

            string[] initiallyToldPeople = HandleInput(Console.ReadLine)
                .Split(SplitSymbols, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .ToArray();

            foreach (string person in initiallyToldPeople)
            {
                // mark that the person already knows
                PeopleWhoAlreadyKnow.Add(person);
                PeopleWhoDontKnow.Remove(person);
            }

            int totalSteps = 0;

            if (PeopleWhoDontKnow.Count == 0)
            {
                Console.WriteLine("All people reached in {0} steps", totalSteps);
                Console.WriteLine("People at last step: {0}",
                    string.Join(", ", initiallyToldPeople.OrderBy(person => person)));

                return;
            }

            var peopleWhoDontKnow = GetPeopleWhoDontKnow(initiallyToldPeople);
            var savedPeopleWhoDontKnow = new SortedSet<string>();

            while (peopleWhoDontKnow.Count > 0)
            {
                foreach (string person in peopleWhoDontKnow)
                {
                    // skip if the person already knows the message
                    if (PeopleWhoAlreadyKnow.Contains(person))
                    {
                        continue;
                    }

                    // mark that the person already knows the message
                    PeopleWhoAlreadyKnow.Add(person);
                    // remove the person from the list of people who dont know
                    PeopleWhoDontKnow.Remove(person);
                }

                savedPeopleWhoDontKnow = new SortedSet<string>(peopleWhoDontKnow);
                peopleWhoDontKnow = GetPeopleWhoDontKnow(peopleWhoDontKnow);

                totalSteps++;
            }

            if (PeopleWhoDontKnow.Count == 0)
            {
                Console.WriteLine("All people reached in {0} steps", totalSteps);
                Console.WriteLine("People at last step: {0}", string.Join(", ", savedPeopleWhoDontKnow));
            }
            else
            {
                Console.WriteLine("Cannot reach: {0}", string.Join(", ", PeopleWhoDontKnow));
            }
        }

        private static ICollection<string> GetPeopleWhoDontKnow(ICollection<string> peopleWhoKnow)
        {
            var peopleWhoDontKnow = new HashSet<string>();

            foreach (string personWhoKnows in peopleWhoKnow)
            {
                // the friends of the person
                var people = KnownPeopleByPerson[personWhoKnows];

                foreach (string person in people)
                {
                    // skip if the person knows
                    if (PeopleWhoAlreadyKnow.Contains(person))
                    {
                        continue;
                    }

                    // otherwise add to the resulting collection
                    peopleWhoDontKnow.Add(person);
                }
            }

            return peopleWhoDontKnow;
        }
    }
}