using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day07 : BaseDayWithInput
    {
        Dictionary<long, List<long>> eqations;
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
                if (solve(eq.Value[0], eq.Key, eq.Value.Skip(1).ToList(), part2))
                    ans += eq.Key;
            }
            return ans;
        }
        bool solve(long running, long ans, List<long> nums,bool part2)
        {
            if(running> ans)
                return false;
            if (running == ans && nums.Count==0)
                return true;
            if(nums.Count == 0)
                return false;
            bool multi = solve(running * nums[0], ans, nums.Skip(1).ToList(),part2);
            bool plus = solve(running + nums[0], ans, nums.Skip(1).ToList(), part2);
            bool concat = false;
            if(part2)
                concat = solve(
                        long.Parse(running.ToString() + nums[0].ToString()),
                        ans, nums.Skip(1).ToList(), part2);
            return multi || plus || concat;
        }


    }
}
