using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day07 : BaseDayWithInput
    {
        Dictionary<long, List<long>> eqations;
        private readonly bool SOLVE_BACKWARDS = true;
        public Day07()
        {
            eqations = [];
            foreach (var line in _input)
            {
                var lr = line.Split(": ");
                var spl = lr[1].Split(" ").ToList();
                List<long> nums = [];
                foreach(var sp in spl)
                    nums.Add(long.Parse(sp));
                eqations.Add(long.Parse(lr[0]), nums);
            }
        }
        public override ValueTask<string> Solve_1() => new($"{solveTask(false)}");
        public override ValueTask<string> Solve_2() => new($"{solveTask(true)}");

        long solveTask(bool part2)
        {
            long ans = 0;
            foreach (var eq in eqations)
            {
                if (SOLVE_BACKWARDS)
                {
                    if (solveBackwards(eq.Key, eq.Value, part2))
                        ans += eq.Key;
                }
                else
                {
                    if (solve(eq.Value[0], eq.Key, eq.Value.Skip(1).ToList(), part2))
                        ans += eq.Key;
                }
            }
            return ans;
        }
        bool solve(long running, long ans, List<long> nums, bool part2)
        {
            if (running > ans)
                return false;
            if (nums.Count == 0 && running == ans)
                return true;
            if (nums.Count == 0)
                return false;
            bool concat = false;
            if (part2)
                concat = solve(
                        long.Parse(running.ToString() + nums[0].ToString()),
                        ans, nums.Skip(1).ToList(), part2);
            if (concat) return true;
            bool multi = solve(running * nums[0], ans, nums.Skip(1).ToList(), part2);
            if(multi) return true;
            bool plus = solve(running + nums[0], ans, nums.Skip(1).ToList(), part2);
            if (plus) return true;
            return false;
        }

        bool solveBackwards(long remainder, List<long> nums, bool part2)
        {
            if (nums.Count == 1 && remainder == nums[0])
                return true;
            if (nums.Count == 1)
                return false;
            //CONCAT
            bool concat = part2;
            var test = LastNDigits(remainder, Digits_Log10(nums.Last()));
            if (concat) concat = nums.Last() == test;
            if (concat)
                concat = solveBackwards(
                    remainder / (long)Math.Pow(10, Digits_Log10(nums.Last())),
                    nums.SkipLast(1).ToList(), part2);
            if (concat) return true;
            //MULTI
            bool multi = remainder % nums.Last() == 0;
            if (multi) multi = solveBackwards(remainder / nums.Last(), nums.SkipLast(1).ToList(), part2);
            if (multi) return true;
            //PLUS
            bool plus = remainder - nums.Last() >= 0;
            if (plus) plus = solveBackwards(remainder - nums.Last(), nums.SkipLast(1).ToList(), part2);
            if (plus) return true;
            //NONE WORKED
            return false;

        }
        private static int LastNDigits(long n, int digits) => (int)(n % Math.Pow(10, digits));
        private static int Digits_Log10(long n) =>
            n == 0 ? 1 : (n > 0 ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n));


    }
}
