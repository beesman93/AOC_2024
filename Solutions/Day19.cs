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
            cache = new(new Dictionary<string, long>() { { "", 1 } });
        }
        public override ValueTask<string> Solve_1() => new($"{texts.AsParallel().Count(t => GetToEnd(t) > 0)}");
        public override ValueTask<string> Solve_2() => new($"{texts.Sum(GetToEnd)}");
        long GetToEnd(string r)
        {
            if (cache.TryGetValue(r, out long val)) return val;
            return cache[r] = towels.Where(t => t[0] == r[0] && t.Length <= r.Length && r[..t.Length] == t).Sum(towel => GetToEnd(r[towel.Length..]));
        }
    }
}
