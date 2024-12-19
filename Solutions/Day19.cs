using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day19 : BaseDayWithInput
    {
        List<string> towels;
        List<string> texts;
        public Day19()
        {
            towels = _input[0].Split(",").ToList<string>();
            for(int i = 0; i< towels.Count;i++)
            {
                towels[i] = towels[i].Trim();
            }
            texts = [];
            for (int i = 2; i < _input.Length; i++)
            {
                texts.Add(_input[i]);
            }
            index = [];
            towels.Sort();
            char c = towels[0][0];
            int from = 0;
            for (int i = 0; i < towels.Count; i++)
            {
                if(towels[i][0] != c)
                {
                    index.Add(c, (from, i-1));
                    c = towels[i][0];
                    from = i;
                }
            }
            index.Add(c, (from, towels.Count - 1));
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            towels.Sort();
            foreach (string text in texts)
            {
                if (getToEnd(text)>0)
                {
                    ans++;
                }
            }
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            long ans = 0;
            foreach (string text in texts)
            {
                ans += getToEnd(text);
            }

            return new($"{ans}");
        }

        Dictionary<char, (int from, int to)> index;
        Dictionary<string, long> cache = new();
        long getToEnd(string remain)
        {
            if (cache.ContainsKey(remain)) return cache[remain];
            if (remain.Length == 0) return 1;
            long tot = 0;
            for (int i = index[remain[0]].from;i<= index[remain[0]].to; i++)
            {
                if (remain[0] == towels[i][0])
                {
                    if (remain.StartsWith(towels[i]))
                    {
                        long curr = getToEnd(remain.Substring(towels[i].Length));
                        tot += curr;
                    }
                }

            }
            cache.Add(remain, tot);
            return tot;
        }


    }
}
