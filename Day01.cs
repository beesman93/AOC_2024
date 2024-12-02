using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day01 : BaseDay
    {
        private readonly string[] _input;
        private readonly List<int> left;
        private readonly List<int> right;
        public Day01()
        {
            _input = File.ReadAllLines(InputFilePath);
            left = new List<int>();
            right = new List<int>();
            foreach (var line in _input)
            {
                var lr = line.Split("   ");
                left.Add(Convert.ToInt32(lr[0]));
                right.Add(Convert.ToInt32(lr[1]));
            }
        }
        public override async ValueTask<string> Solve_1() => new(await D1());
        public override async ValueTask<string> Solve_2() => new(await D2());

        private async Task<string> D1()
        {
            var result = 0;
            List<int> leftSorted = new(left);
            leftSorted.Sort();
            List<int> rightSorted = new(right);
            rightSorted.Sort();
            for(int i=0;i<leftSorted.Count;i++)
            {
                result += int.Abs(leftSorted[i]-rightSorted[i]);
            }

            return result.ToString();
        }
        private async Task<string> D2()
        {
            var result = 0;
            foreach (var num in left)
            {
                var cnt = (from temp in right where temp.Equals(num) select temp).Count();
                result += cnt * num;
            }
            return result.ToString();
        }
    }
}
