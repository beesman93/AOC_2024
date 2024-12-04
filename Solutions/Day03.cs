using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day03 : BaseDayWithInput
    {
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var line in _input)
            {
                Regex regex = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)");
                var matches = regex.Matches(line);
                foreach (Match match in matches)
                {
                    ans += Convert.ToInt64(match.Groups[1].Value) * Convert.ToInt64(match.Groups[2].Value);
                }
            }
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2()
        {
            var fullInput = "";
            foreach (var line in _input)
                fullInput += line;

            long ans = 0;
            StringBuilder sb = new();
            bool mulEnabled = true;
            for(int i = 0; i < fullInput.Length; i++)
            {
                if(mulEnabled)
                    sb.Append(fullInput[i]);
                if (i > 7)
                {
                    if (fullInput.Substring(i - 7, 7) == "don't()")
                        mulEnabled = false;
                    if(fullInput.Substring(i - 4, 4) == "do()")
                        mulEnabled = true;
                }
            }

            Regex regex = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)");
            var matches = regex.Matches(sb.ToString());
            foreach (Match match in matches)
            {
                ans += Convert.ToInt64(match.Groups[1].Value) * Convert.ToInt64(match.Groups[2].Value);
            }

            return new($"{ans}");
        }
    }
}
