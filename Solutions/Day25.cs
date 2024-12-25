using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2024
{
    internal class Day25 : BaseDayWithInput
    {
        List<int> Keys;
        List<int> Locks;
        public Day25()
        {
            Keys = [];
            Locks = [];
            for(int x =0; x < _input.Length; x+=8)
            {
                int val = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        val |= _input[i+x+1][j] == '#' ? 1 << (i*5+j) : 0;
                    }
                }
                if (_input[x] == "#####")
                    Locks.Add(val);
                else
                    Keys.Add(val);
            }
        }
        public override ValueTask<string> Solve_1()
        {
            long ans = 0;
            foreach (var Key in Keys)
                foreach (var Lock in Locks)
                    if ((Key & Lock) == 0)
                        ans++;
            return new($"{ans}");
        }

        public override ValueTask<string> Solve_2() => new("Push button.");
    }
}
