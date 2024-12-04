using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024
{
    internal class Day02 : BaseDayWithInput
    {
        public override ValueTask<string> Solve_1()
        {
            var result = 0;
            foreach (var line in _input)
            {
                var levels = line.Split(" ");
                var row_test_full = new List<int>();
                foreach (var level in levels)
                    row_test_full.Add(Convert.ToInt32(level));
                if (safeTest(row_test_full))
                    result++;

            }
            return new($"{result}");
        }

        public override ValueTask<string> Solve_2()
        {
            var result = 0;
            foreach (var line in _input)
            {
                var levels = line.Split(" ");
                var row_test_full = new List<int>();
                foreach (var level in levels)
                    row_test_full.Add(Convert.ToInt32(level));
                bool lineIsSafe = safeTest(row_test_full);
                if (!lineIsSafe)
                {
                    for (int i = 0; i < levels.Length; i++)
                    {
                        var row_test = new List<int>();
                        for (int j = 0; j < levels.Length; j++)
                        {
                            if (j != i)
                                row_test.Add(Convert.ToInt32(levels[j]));
                        }
                        if (safeTest(row_test))
                        {
                            lineIsSafe = true;
                            break;
                        }
                    }
                }
                if (lineIsSafe) result++;
            }
            return new($"{result}");
        }

        bool safeTest(List<int> levels_row)
        {
            var levels = levels_row.ToArray<int>();
            int prev = Convert.ToInt32(levels[0]);
            bool? desc = null;
            bool? was_desc = null;
            bool safe = true;
            for (int i = 1; i < levels.Count(); i++)
            {
                was_desc = desc;

                int curr = Convert.ToInt32(levels[i]);
                if (curr > prev)
                    desc = true;
                else if (curr < prev)
                    desc = false;
                else
                {
                    safe = false;
                    break;
                }
                if (was_desc.HasValue)
                {
                    if (desc != was_desc)
                    {
                        safe = false;
                        break;
                    }
                }
                int diff = int.Abs(prev - curr);
                if (diff < 1 || diff > 3)
                {
                    safe = false;
                    break;
                }
                prev = curr;
            }
            return safe;
        }

    }
}
