using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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
            cache = [];
        }
        public override ValueTask<string> Solve_1() => new($"{texts.AsParallel().Count(text => GetToEnd(text) > 0)}");
        public override ValueTask<string> Solve_2() => new($"{texts.AsParallel().Sum(text => GetToEnd(text))}");
        long GetToEnd(string remain)
        {
            if (cache.TryGetValue(remain, out long val))
                return val;
            if (remain.Length == 0) return 1;
            long tot = 0;
            foreach(var towel in towels)
            {
                if (remain[0] == towel[0]&&towel.Length<=remain.Length)
                {
                    if (remain[..towel.Length]==towel)
                    {
                        long curr = GetToEnd(remain[towel.Length..]);
                        tot += curr;
                    }
                }
            }
            cache[remain] = tot;
            return tot;
        }


    }
}
