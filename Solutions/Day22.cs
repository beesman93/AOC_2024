using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;

namespace AOC_2024
{
    internal class Day22 : BaseDayWithInput
    {
        const int TURNS = 2000;
        int[] monkeyTokens;
        int[,] monkeyPrices;
        public Day22()
        {
            int i = 0;
            monkeyTokens = new int[_input.Length];
            monkeyPrices = new int[_input.Length, TURNS];
            foreach (var line in _input)
            {
                monkeyTokens[i++] = int.Parse(line);
            }
        }

        //we prune by 16777216 so int is fine
        public int GenerateNext(int secret)
        {
            int next = (secret ^ (secret << 6)) & 16777215;
            next = (next ^ (next >> 5)) & 16777215;
            next = (next ^ (next << 11)) & 16777215;
            return next;
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            for (int i = 0; i < monkeyTokens.Length; i++)
            {
                for (int k = 0; k < TURNS; k++)
                {
                    monkeyPrices[i, k] = (int)(monkeyTokens[i] % 10);
                    monkeyTokens[i] = GenerateNext(monkeyTokens[i]);
                }
                ans += monkeyTokens[i];
            }
            return new($"{ans}");
        }
        public void CalculateBananas(ref Dictionary<int,int> bananas)
        {
            for (int i = 0; i < monkeyPrices.GetLength(0); i++)
            {
                HashSet<int> visited = [];
                for (int k = 0; k < TURNS - 4; k++)
                {
                    var key =   (monkeyPrices[i, k + 1] - monkeyPrices[i, k]) *1000000+
                                (monkeyPrices[i, k + 2] - monkeyPrices[i, k + 1])*10000+
                                (monkeyPrices[i, k + 3] - monkeyPrices[i, k + 2])*100+
                                (monkeyPrices[i, k + 4] - monkeyPrices[i, k + 3]);
                    if (!visited.Contains(key))
                        if(bananas.ContainsKey(key))
                            bananas[key] += monkeyPrices[i, k + 4];
                        else
                            bananas[key] = monkeyPrices[i, k + 4];
                    visited.Add(key);
                }
            }
        }
        public override ValueTask<string> Solve_2()
        {
            int ans = int.MinValue;
            Dictionary<int, int> bananas = [];
            CalculateBananas(ref bananas);
            foreach (var test in bananas.Values)
            {
                if (test > ans)
                {
                    ans = test;
                }
            }
            return new($"{ans}");
        }
    }
}
