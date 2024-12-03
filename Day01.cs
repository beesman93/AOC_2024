using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day01 : BaseDayWithInput
    {
        List<int> left;
        List<int> right;
        public Day01()
        {
            left = new List<int>();
            right = new List<int>();
            foreach (var line in _input)
            {
                var lr = line.Split("   ");
                left.Add(Convert.ToInt32(lr[0]));
                right.Add(Convert.ToInt32(lr[1]));
            }
        }
        public override ValueTask<string> Solve_1()
        {
            var ans = 0;
            left.Sort();
            right.Sort();
            for (int i = 0; i < left.Count; i++)
                ans += int.Abs(left[i] - right[i]);
            return new($"{ans}");
        }
        public override ValueTask<string> Solve_2()
        {
            var ans = 0;
            foreach (var num in left)
            {
                var cnt = (from temp in right where temp.Equals(num) select temp).Count();
                ans += cnt * num;
            }
            return new($"{ans}");
        }
    }
}
