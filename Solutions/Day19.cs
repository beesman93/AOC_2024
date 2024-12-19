using System.Collections.Concurrent;
using AoCHelper;

namespace AOC_2024
{
    internal class Day19 : BaseDayWithInput
    {
        readonly string[] towels;
        readonly string[] texts;
        readonly ConcurrentDictionary<string, long> cache;
        public Day19()
        {
            towels = [.. _input[0].Split(", ")];
            texts = _input[2.._input.Length];
            cache = []; cache[""] = 1;
        }
        public override ValueTask<string> Solve_1() => new($"{texts.AsParallel().Count(t => G(t) > 0)}");
        public override ValueTask<string> Solve_2() => new($"{texts.Sum(G)}");
        long G(string r) => cache.GetOrAdd(r,_=>towels.Where(t=>t[0]==r[0]&&t.Length<=r.Length&&r[..t.Length]== t).Sum(t=>G(r[t.Length..])));
    }
}
