using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day11 : BaseDayWithInput
    {
        List<long> stones;
        Dictionary<(long stone,long blinks), long> stoneCache;
        public Day11()
        {
            stones = [];
            stoneCache = [];
            _input[0].Split(' ').ToList().ForEach(x => stones.Add(long.Parse(x)));
        }
        public override ValueTask<string> Solve_1() => new($"{stones.Sum(x => lenAfterNBlinks((x, 25)))}");
        public override ValueTask<string> Solve_2() => new($"{stones.Sum(x => lenAfterNBlinks((x, 75)))}");
        long lenAfterNBlinks((long stone, long blinksLeft) pair)
        {
            if (stoneCache.ContainsKey(pair))
                return stoneCache[pair];
            if (pair.blinksLeft == 0)
            {
                stoneCache[pair] = 1;
            }
            else if (pair.stone == 0)
            {
                stoneCache[pair] = lenAfterNBlinks((1, pair.blinksLeft - 1));
            }
            else if (pair.stone.ToString().Length % 2 == 0)
            {
                var strStone = pair.stone.ToString();
                var leftStone = long.Parse(strStone.Substring(0, strStone.Length / 2));
                var rightStone = long.Parse(strStone.Substring(strStone.Length / 2));
                stoneCache[pair] = lenAfterNBlinks((leftStone, pair.blinksLeft - 1)) + lenAfterNBlinks((rightStone, pair.blinksLeft - 1));
            }
            else
            {
                stoneCache[pair] = lenAfterNBlinks((pair.stone * 2024, pair.blinksLeft - 1));
            }
            return stoneCache[pair];
        }

    }
}
