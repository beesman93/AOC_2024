using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using MathNet.Numerics.Statistics;
using Spectre.Console.Rendering;

namespace AOC_2024
{
    internal class Day23 : BaseDayWithInput
    {
        Dictionary<string,HashSet<string>> computers;
        public Day23()
        {
            computers = [];
            foreach (var line in _input)
            {
                var parts = line.Split("-");
                var id1 = parts[0];
                var id2 = parts[1];
                computers.TryAdd(id1, []);
                computers.TryAdd(id2, []);
                computers[id1].Add(id2);
                computers[id2].Add(id1);
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            HashSet<string> tripples = [];
            foreach (var computer1 in computers.Keys)
            {
                if (computer1[0] =='t')
                {
                    foreach (var computer2 in computers[computer1])
                    {
                        foreach (var computer3 in computers[computer2])
                        {
                            if (computers[computer3].Contains(computer1))
                            {
                                List<string> tripple = new() { computer1, computer2, computer3 };
                                tripple.Sort();
                                tripples.Add(string.Join(',', tripple));
                            }
                        }
                    }
                }
            }
            return new($"{tripples.Count}");
        }

        private bool checkLanIsValid(HashSet<string> lan)
        {
            foreach (var computer in lan)
            {
                foreach (var otherComputer in lan)
                {
                    if ((!computers[computer].Contains(otherComputer)) && (computer!=otherComputer))
                        return false;
                }
            }
            return true;
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            int maxLanSize = computers.Values.Max(x => x.Count);//turns out all initial tests are same length
            //if not test largest first, when generating new tests add existing smaller tests to new tests
            Dictionary<string, HashSet<string>> tests = [];
            foreach (var computer in computers)
            {
                HashSet<string> test = new(computer.Value);
                test.Add(computer.Key);
                var l = test.ToList();
                l.Sort();
                tests.TryAdd(string.Join(",",l), test);
            }
            while (tests.First().Value.Count>1)
            {
                foreach (var test in tests)
                {
                    if (checkLanIsValid(test.Value))
                        return new($"{test.Key}");
                }
                Dictionary<string, HashSet<string>> newTests = [];
                foreach (var test in tests)
                {
                    foreach (var id in test.Value)
                    {
                        HashSet<string> newTestVal = new(test.Value);
                        newTestVal.Remove(id);
                        var l = newTestVal.ToList();
                        l.Sort();
                        newTests.TryAdd(string.Join(",", l), newTestVal);
                    }
                }
                tests = newTests;
            }
            return new($"Not found.");
        }
    }
}
